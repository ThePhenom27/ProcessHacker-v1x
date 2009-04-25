﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ProcessHacker.Native.Api;
using System.Runtime.InteropServices;

namespace ProcessHacker.Native
{
    public static class FileUtils
    {
        static FileUtils()
        {
            RefreshDriveDevicePrefixes();
        }

        /// <summary>
        /// Used to resolve device prefixes (\Device\Harddisk1) into DOS drive names.
        /// </summary>
        private static Dictionary<string, string> _driveDevicePrefixes = new Dictionary<string, string>();

        public static Icon GetFileIcon(string fileName)
        {
            return GetFileIcon(fileName, false);
        }

        public static Icon GetFileIcon(string fileName, bool large)
        {
            var shinfo = new ShFileInfo();

            if (fileName == null || fileName == "")
                throw new Exception("File name cannot be empty.");

            try
            {
                if (Win32.SHGetFileInfo(fileName, 0, ref shinfo,
                      (uint)Marshal.SizeOf(shinfo),
                       Win32.ShgFiIcon |
                       (large ? Win32.ShgFiLargeIcon : Win32.ShgFiSmallIcon)) == 0)
                {
                    return null;
                }
                else
                {
                    return Icon.FromHandle(shinfo.hIcon);
                }
            }
            catch
            {
                return null;
            }
        }

        public static string FixPath(string path)
        {
            if (path.ToLower().StartsWith("\\systemroot"))
                return (new System.IO.FileInfo(Environment.SystemDirectory + "\\.." + path.Substring(11))).FullName;
            else if (path.StartsWith("\\??\\"))
                return path.Substring(4);
            else
                return path;
        }

        public static string DeviceFileNameToDos(string fileName)
        {
            // don't know if this is really necessary...
            var prefixes = _driveDevicePrefixes;

            foreach (var pair in prefixes)
                if (fileName.StartsWith(pair.Key))
                    return pair.Value + fileName.Substring(pair.Key.Length);

            return fileName;
        }

        public static void RefreshDriveDevicePrefixes()
        {
            // just create a new dictionary to avoid having to lock the existing one
            var newPrefixes = new Dictionary<string, string>();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                StringBuilder target = new StringBuilder(1024);

                if (Win32.QueryDosDevice(c.ToString() + ":", target, 1024) != 0)
                {
                    newPrefixes.Add(target.ToString(), c.ToString() + ":");
                }
            }

            _driveDevicePrefixes = newPrefixes;
        }

        public static void ShowProperties(string fileName)
        {
            var info = new ShellExecuteInfo();

            info.cbSize = Marshal.SizeOf(info);
            info.lpFile = fileName;
            info.nShow = ShowWindowType.Show;
            info.fMask = Win32.SeeMaskInvokeIdList;
            info.lpVerb = "properties";

            Win32.ShellExecuteEx(ref info);
        }
    }
}