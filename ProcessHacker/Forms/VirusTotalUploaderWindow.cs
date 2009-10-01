﻿/*
 * Process Hacker - 
 *   ProcessHacker VirusTotal Implementation
 * 
 * Copyright (C) 2009 dmex
 * 
 * ProcessHacker permission to implement VirusTotal service authorized by:
 * Julio Canto | VirusTotal.com | Hispasec Sistemas Lab | Tlf: +34.902.161.025
 * Fax: +34.952.028.694 | PGP Key ID: EF618D2B | jcanto@hispasec.com 
 * 26/09/2009 - 2:39PM
 * 
 * This file is part of Process Hacker.
 * 
 * Process Hacker is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Process Hacker is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Process Hacker.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */


using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using ProcessHacker.Common;
using ProcessHacker.Components;
using ProcessHacker.Native;
using TaskbarLib;

namespace ProcessHacker
{
    public partial class VirusTotalUploaderWindow : Form
    {
        string filepath;
        string processName;
        
        long totalfilesize;
        long bytesPerSecond;
        long bytesTransferred;

        public VirusTotalUploaderWindow(string procName, string procPath)
        {
            InitializeComponent();
            this.AddEscapeToClose();
            this.SetTopMost();

            processName = procName;
            filepath = procPath;
            
            this.Icon = Program.HackerWindow.Icon;
        }

        private void VirusTotalUploaderWindow_Load(object sender, EventArgs e)
        {
            labelFile.Text = string.Format("Uploading: {0}", processName);

            FileInfo finfo = new FileInfo(filepath);
            if (!finfo.Exists)
            {
                if (OSVersion.HasTaskDialogs)
                {
                    TaskDialog td = new TaskDialog();
                    td.PositionRelativeToWindow = true;
                    td.Content = "The selected file doesn't exist or couldnt be found!";
                    td.MainInstruction = "File Location not Available!";
                    td.WindowTitle = "System Error";
                    td.MainIcon = TaskDialogIcon.CircleX;
                    td.CommonButtons = TaskDialogCommonButtons.Ok;
                    td.Show(Program.HackerWindow.Handle);
                }
                else
                {
                    MessageBox.Show(
                       this, "The selected file doesn't exist or couldnt be found!",
                       "System Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                       );
                }

                this.Close();
            }
            else if (finfo.Length >= 20971520 /* 20MB */)
            {
                if (OSVersion.HasTaskDialogs)
                {
                    TaskDialog td = new TaskDialog();
                    td.PositionRelativeToWindow = true;
                    td.Content = "This file is larger than 20MB, above the VirusTotal limit!";
                    td.MainInstruction = "File is too large";
                    td.WindowTitle = "VirusTotal Error";
                    td.MainIcon = TaskDialogIcon.CircleX;
                    td.CommonButtons = TaskDialogCommonButtons.Ok;
                    td.Show(Program.HackerWindow.Handle);
                }
                else
                {
                     MessageBox.Show(
                        this, "This file is larger than 20MB and is above the VirusTotal size limit!",
                        "VirusTotal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                        );
                }

                this.Close();
            }
            else
            {
                totalfilesize = finfo.Length;
            }

            uploadedLabel.Text = "Uploaded: Initializing";
            speedLabel.Text = "Speed: Initializing";

            BackgroundWorker getSessionToken = new BackgroundWorker();
            getSessionToken.RunWorkerCompleted += new RunWorkerCompletedEventHandler(getSessionToken_RunWorkerCompleted);
            getSessionToken.DoWork += new DoWorkEventHandler(getSessionToken_DoWork);
            getSessionToken.RunWorkerAsync();
        }

        private void VirusTotalUploaderWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UploadWorker.IsBusy)
                UploadWorker.CancelAsync();

            if (OSVersion.HasExtendedTaskbar)
                Windows7Taskbar.SetTaskbarProgressState(this, Windows7Taskbar.ThumbnailProgressState.NoProgress);
        }

        private void getSessionToken_DoWork(object sender, DoWorkEventArgs e)
        {
            HttpWebRequest sessionRequest = (HttpWebRequest)HttpWebRequest.Create("http://www.virustotal.com/vt/en/identificador");
            sessionRequest.ServicePoint.ConnectionLimit = 20;
            sessionRequest.UserAgent = "Process Hacker " + Application.ProductVersion;
            sessionRequest.Timeout = System.Threading.Timeout.Infinite;
            sessionRequest.KeepAlive = true;

            using (WebResponse Response = sessionRequest.GetResponse())
            using (Stream WebStream = Response.GetResponseStream())
            using (StreamReader Reader = new StreamReader(WebStream))
            {
                e.Result = Reader.ReadToEnd();
            }
        }

        private void getSessionToken_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UploadWorker.RunWorkerAsync(e.Result); 
        }

        private void UploadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

            HttpWebRequest uploadRequest = (HttpWebRequest)WebRequest.Create("http://www.virustotal.com/vt/en/recepcionf?" + e.Argument);
            uploadRequest.ServicePoint.ConnectionLimit = 20;
            uploadRequest.UserAgent = "ProcessHacker " + Application.ProductVersion;
            uploadRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            uploadRequest.Timeout = System.Threading.Timeout.Infinite;
            uploadRequest.KeepAlive = true;
            uploadRequest.Method = WebRequestMethods.Http.Post;

            // Build up the 'post' message header
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append(@"Content-Disposition: form-data; name=""archivo""; filename=" + processName + "");
            sb.Append("\r\n");
            sb.Append("Content-Type: application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array
            // ensuring the boundary appears on a line by itself
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            if (UploadWorker.CancellationPending)
            {
                uploadRequest.Abort();
                UploadWorker.CancelAsync();
                return;
            }

            try
            {
                using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    uploadRequest.ContentLength = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;

                    using (Stream requestStream = uploadRequest.GetRequestStream())
                    {
                        // Write out our post header
                        requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                        // Write out the file contents
                        byte[] buffer = new Byte[checked((uint)Math.Min(32, (int)fileStream.Length))];

                        int bytesRead = 0;
                        Stopwatch stopwatch = new Stopwatch();

                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            if (UploadWorker.CancellationPending)
                            {
                                uploadRequest.Abort();
                                UploadWorker.CancelAsync();
                                break;
                            }

                            stopwatch.Start();
                            requestStream.Write(buffer, 0, bytesRead);
                            stopwatch.Stop();

                            int progress = (int)(((float)fileStream.Position / (float)fileStream.Length) * 100);

                            if (stopwatch.ElapsedMilliseconds > 0)
                                bytesPerSecond = fileStream.Position * 1000 / stopwatch.ElapsedMilliseconds;

                            bytesTransferred = fileStream.Position;

                            stopwatch.Reset();

                            UploadWorker.ReportProgress(progress);
                        }

                        if (UploadWorker.CancellationPending)
                        {
                            uploadRequest.Abort();
                            UploadWorker.CancelAsync();
                            return;
                        }

                        // Write out the trailing boundary
                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);

                        // Write all data before we close the stream.
                        requestStream.Flush();
                        requestStream.Close();
                    }
                }
            }
            catch (WebException ex)
            {   //RequestCanceled will occour when we cancel the WebRequest
                //filter that exception but log all others
                if (ex != null)
                {
                    if (ex.Status != WebExceptionStatus.RequestCanceled)
                    {
                        PhUtils.ShowException("Unable to download the VirusTotal SessionToken", ex);
                        Logging.Log(ex);
                        this.Close();
                    }
                }
            }

            if (UploadWorker.CancellationPending)
            {
                uploadRequest.Abort();
                UploadWorker.CancelAsync();
                return;
            }

            WebResponse response = uploadRequest.GetResponse();

            //Stream s = responce.GetResponseStream(); 
            //StreamReader sr = new StreamReader(s);
            //sr.ReadToEnd();

            //Return the response URL 
            e.Result = response.ResponseUri.AbsoluteUri;
        }

        private void UploadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            uploadedLabel.Text = "Uploaded: " + Utils.FormatSize(bytesTransferred);
            totalSizeLabel.Text = "Total Size: " + Utils.FormatSize(totalfilesize);
            speedLabel.Text = "Speed: " + Utils.FormatSize(bytesPerSecond) + "/s";
            label1.Text = string.Format("{0}%", e.ProgressPercentage);
            progressUpload.Value = e.ProgressPercentage;

            if (OSVersion.HasExtendedTaskbar)
                Windows7Taskbar.SetTaskbarProgress(this, this.progressUpload);
        }

        private void UploadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO: future additions will parse the page and  
            //display the appropriate infomation but for now just mirror  
            //the functionality of the VirusTotal desktop client and
            //launch the URL in the default browser

            var webException = e.Error as WebException;
            if (webException != null && webException.Status != WebExceptionStatus.Success)
            {
                if (webException.Status != WebExceptionStatus.RequestCanceled)
                {
                    PhUtils.ShowException("Unable to Upload the file", webException);
                    this.Close();
                }
            }
            else if (e.Result != null && !e.Cancelled) //sanity check
            {
                Program.TryStart(e.Result.ToString());
            }

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}