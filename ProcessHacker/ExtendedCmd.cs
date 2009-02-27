﻿/*
 * Process Hacker - 
 *   extended command line options
 * 
 * Copyright (C) 2009 wj32
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
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ProcessHacker
{
    public static class ExtendedCmd
    {
        public static void Run(IDictionary<string, string> args)
        {
            if (!args.ContainsKey("-type"))
                throw new Exception("-type switch required.");

            string type = args["-type"].ToLower();

            if (!args.ContainsKey("-obj"))
                throw new Exception("-obj switch required.");

            string obj = args["-obj"];

            if (!args.ContainsKey("-action"))
                throw new Exception("-action switch required.");

            string action = args["-action"].ToLower();

            switch (type)
            {
                case "process":
                    {
                        foreach (string pid in obj.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            switch (action)
                            {
                                case "terminate":
                                    {
                                        try
                                        {
                                            using (Win32.ProcessHandle phandle = new Win32.ProcessHandle(int.Parse(pid),
                                                Win32.PROCESS_RIGHTS.PROCESS_TERMINATE))
                                                phandle.Terminate();
                                        }
                                        catch (Exception ex)
                                        {
                                            DialogResult result = MessageBox.Show("Could not terminate process with PID " + 
                                                pid + ":\n\n" +
                                                ex.Message, "Process Hacker", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                                            if (result == DialogResult.Cancel)
                                                return;
                                        }
                                    }
                                    break;
                                case "suspend":
                                    {
                                        if (Properties.Settings.Default.WarnDangerous && Misc.IsDangerousPID(int.Parse(pid)))
                                        {
                                            DialogResult result = MessageBox.Show("The process with PID " + pid + " is a system process. Are you" +
                                                " sure you want to suspend it?", "Process Hacker", MessageBoxButtons.YesNoCancel,
                                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                                            if (result == DialogResult.No)
                                                continue;
                                            else if (result == DialogResult.Cancel)
                                                return;
                                        }

                                        try
                                        {
                                            using (var phandle =
                                                new Win32.ProcessHandle(int.Parse(pid), Win32.PROCESS_RIGHTS.PROCESS_SUSPEND_RESUME))
                                                phandle.Suspend();
                                        }
                                        catch (Exception ex)
                                        {
                                            DialogResult result = MessageBox.Show("Could not suspend process with PID " +
                                                pid + ":\n\n" +
                                                ex.Message, "Process Hacker", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                                            if (result == DialogResult.Cancel)
                                                return;
                                        }
                                    }
                                    break;
                                case "resume":
                                    {
                                        if (Properties.Settings.Default.WarnDangerous && Misc.IsDangerousPID(int.Parse(pid)))
                                        {
                                            DialogResult result = MessageBox.Show("The process with PID " + pid + " is a system process. Are you" +
                                                " sure you want to resume it?", "Process Hacker", MessageBoxButtons.YesNoCancel,
                                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                                            if (result == DialogResult.No)
                                                continue;
                                            else if (result == DialogResult.Cancel)
                                                return;
                                        }

                                        try
                                        {
                                            using (var phandle =
                                                new Win32.ProcessHandle(int.Parse(pid), Win32.PROCESS_RIGHTS.PROCESS_SUSPEND_RESUME))
                                                phandle.Resume();
                                        }
                                        catch (Exception ex)
                                        {
                                            DialogResult result = MessageBox.Show("Could not resume process with PID " +
                                                pid + ":\n\n" +
                                                ex.Message, "Process Hacker", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                                            if (result == DialogResult.Cancel)
                                                return;
                                        }
                                    }
                                    break;
                                default:
                                    throw new Exception("Unknown action '" + action + "'");
                            }
                        }
                    }
                    break;

                case "thread":
                    {
                        foreach (string tid in obj.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            switch (action)
                            {
                                case "terminate":
                                    {
                                        try
                                        {
                                            using (var thandle =
                                                new Win32.ThreadHandle(int.Parse(tid), Win32.THREAD_RIGHTS.THREAD_TERMINATE))
                                                thandle.Terminate();
                                        }
                                        catch (Exception ex)
                                        {
                                            DialogResult result = MessageBox.Show("Could not terminate thread with ID " + tid + ":\n\n" +
                                                    ex.Message, "Process Hacker", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                                            if (result == DialogResult.Cancel)
                                                return;
                                        }
                                    }
                                    break;
                                case "suspend":
                                    {
                                        try
                                        {
                                            using (var thandle =
                                                new Win32.ThreadHandle(int.Parse(tid), Win32.THREAD_RIGHTS.THREAD_SUSPEND_RESUME))
                                                thandle.Suspend();
                                        }
                                        catch (Exception ex)
                                        {
                                            DialogResult result = MessageBox.Show("Could not suspend thread with ID " + tid + ":\n\n" +
                                                    ex.Message, "Process Hacker", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                                            if (result == DialogResult.Cancel)
                                                return;
                                        }
                                    }
                                    break;
                                case "resume":
                                    {
                                        try
                                        {
                                            using (var thandle =
                                                new Win32.ThreadHandle(int.Parse(tid), Win32.THREAD_RIGHTS.THREAD_SUSPEND_RESUME))
                                                thandle.Resume();
                                        }
                                        catch (Exception ex)
                                        {
                                            DialogResult result = MessageBox.Show("Could not resume thread with ID " + tid + ":\n\n" +
                                                    ex.Message, "Process Hacker", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                                            if (result == DialogResult.Cancel)
                                                return;
                                        }
                                    }
                                    break;
                                default:
                                    throw new Exception("Unknown action '" + action + "'");
                            }
                        }
                    }
                    break;

                case "service":
                    {
                        switch (action)
                        {
                            case "start":
                                {
                                    using (var shandle = new Win32.ServiceHandle(obj, Win32.SERVICE_RIGHTS.SERVICE_START))
                                        shandle.Start();
                                }
                                break;
                            case "continue":
                                {
                                    using (var shandle = new Win32.ServiceHandle(obj, Win32.SERVICE_RIGHTS.SERVICE_PAUSE_CONTINUE))
                                        shandle.Control(Win32.SERVICE_CONTROL.Continue);
                                }
                                break;
                            case "pause":
                                {
                                    using (var shandle = new Win32.ServiceHandle(obj, Win32.SERVICE_RIGHTS.SERVICE_PAUSE_CONTINUE))
                                        shandle.Control(Win32.SERVICE_CONTROL.Pause);
                                }
                                break;
                            case "stop":
                                {
                                    using (var shandle = new Win32.ServiceHandle(obj, Win32.SERVICE_RIGHTS.SERVICE_STOP))
                                        shandle.Control(Win32.SERVICE_CONTROL.Stop);
                                }
                                break;
                            case "delete":
                                {
                                    using (var shandle = new Win32.ServiceHandle(obj, (Win32.SERVICE_RIGHTS)Win32.STANDARD_RIGHTS.DELETE))
                                        shandle.Delete();
                                }
                                break;
                            default:
                                throw new Exception("Unknown action '" + action + "'");
                        }
                    }
                    break;

                default:
                    throw new Exception("Unknown object type '" + type + "'");
            }
        }
    }
}