﻿/*
 * Process Hacker - 
 *   terminal server handles and objects
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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ProcessHacker.Common;
using ProcessHacker.Native.Api;
using ProcessHacker.Native.Security;

namespace ProcessHacker.Native.Objects
{
    public class TerminalServerHandle : NativeHandle
    {
        public static TerminalServerHandle GetCurrent()
        {
            return new TerminalServerHandle(IntPtr.Zero, false);
        }

        protected TerminalServerHandle(IntPtr handle, bool owned)
            : base(handle, owned)
        { }

        public TerminalServerHandle(string serverName)
        {
            this.Handle = Win32.WTSOpenServer(serverName);

            if (this.Handle == IntPtr.Zero)
                Win32.ThrowLastError();
        }

        protected override void Close()
        {
            Win32.WTSCloseServer(this);
        }

        public TerminalServerProcess[] GetProcesses()
        {
            IntPtr dataPtr;
            int count;
            TerminalServerProcess[] processes;

            if (!Win32.WTSEnumerateProcesses(this, 0, 1, out dataPtr, out count))
                Win32.ThrowLastError();

            using (var data = new WtsMemoryAlloc(dataPtr))
            {
                processes = new TerminalServerProcess[count];

                for (int i = 0; i < count; i++)
                {
                    var process = data.ReadStruct<WtsProcessInfo>(i);
                    processes[i] = new TerminalServerProcess(
                        process.ProcessId,
                        process.SessionId,
                        Marshal.PtrToStringUni(process.ProcessName),
                        new Sid(process.Sid)
                        );
                }

                return processes;
            }
        }

        public TerminalServerSession[] GetSessions()
        {
            IntPtr dataPtr;
            int count;
            TerminalServerSession[] sessions;

            if (!Win32.WTSEnumerateSessions(this, 0, 1, out dataPtr, out count))
                Win32.ThrowLastError();

            using (var data = new WtsMemoryAlloc(dataPtr))
            {
                sessions = new TerminalServerSession[count];

                for (int i = 0; i < count; i++)
                {
                    var session = data.ReadStruct<WtsSessionInfo>(i);
                    sessions[i] = new TerminalServerSession(
                        this,
                        session.SessionID,
                        session.WinStationName,
                        session.State
                        );
                }

                return sessions;
            }
        }

        public void RegisterNotifications(IWin32Window window, bool allSessions)
        {
            if (!Win32.WTSRegisterSessionNotificationEx(
                this,
                window.Handle,
                allSessions ? WtsNotificationFlags.AllSessions : WtsNotificationFlags.ThisSession
                ))
                Win32.ThrowLastError();
        }

        public void Shutdown(WtsShutdownFlags flag)
        {
            if (!Win32.WTSShutdownSystem(this, flag))
                Win32.ThrowLastError();
        }

        public void TerminateProcess(int pid, int exitCode)
        {
            if (!Win32.WTSTerminateProcess(this, pid, exitCode))
                Win32.ThrowLastError();
        }

        public void UnregisterNotifications(IWin32Window window)
        {
            if (!Win32.WTSUnRegisterSessionNotificationEx(this, window.Handle))
                Win32.ThrowLastError();
        }
    }

    public class TerminalServerSession : DisposableObject
    {
        public static int GetActiveConsoleId()
        {
            return Win32.WTSGetActiveConsoleSessionId();
        }

        private TerminalServerHandle _serverHandle;
        private int _sessionId;
        private string _name;
        private WtsConnectStateClass _state = (WtsConnectStateClass)(-1);
        private string _initialProgram;
        private string _applicationName;
        private string _workingDirectory;
        private string _userName;
        private string _domainName;
        private string _clientName;
        private string _clientDirectory;
        private System.Net.IPAddress _clientAddress;
        private WtsClientDisplay? _clientDisplay;

        public TerminalServerSession(TerminalServerHandle serverHandle, int sessionId)
        {
            _serverHandle = serverHandle;
            _sessionId = sessionId;
            _serverHandle.Reference();
        }

        internal TerminalServerSession(TerminalServerHandle serverHandle, int sessionId, string name, WtsConnectStateClass state)
        {
            _serverHandle = serverHandle;
            _sessionId = sessionId;
            _serverHandle.Reference();
            _name = name;
            _state = state;
        }

        protected override void DisposeObject(bool disposing)
        {
            _serverHandle.Dereference(disposing);
        }

        public int SessionId { get { return _sessionId; } }

        public string Name
        {
            get
            {
                if (_name == null)
                    _name = this.GetInformationString(WtsInformationClass.WinStationName);
                return _name;
            }
        }

        public WtsConnectStateClass State
        {
            get
            {
                if ((int)_state == -1)
                {
                    IntPtr dataPtr;
                    int length;

                    if (!Win32.WTSQuerySessionInformation(
                        _serverHandle, _sessionId, WtsInformationClass.ConnectState, out dataPtr, out length))
                        Win32.ThrowLastError();

                    using (var data = new WtsMemoryAlloc(dataPtr))
                        _state = (WtsConnectStateClass)data.ReadInt32(0);
                }

                return _state;
            }
        }

        public string InitialProgram
        {
            get
            {
                if (_initialProgram == null)
                    _initialProgram = this.GetInformationString(WtsInformationClass.InitialProgram);
                return _initialProgram;
            }
        }

        public string ApplicationName
        {
            get
            {
                if (_applicationName == null)
                    _applicationName = this.GetInformationString(WtsInformationClass.ApplicationName);
                return _applicationName;
            }
        }

        public string WorkingDirectory
        {
            get
            {
                if (_workingDirectory == null)
                    _workingDirectory = this.GetInformationString(WtsInformationClass.WorkingDirectory);
                return _workingDirectory;
            }
        }

        public string UserName
        {
            get
            {
                if (_userName == null)
                    _userName = this.GetInformationString(WtsInformationClass.UserName);
                return _userName;
            }
        }

        public string DomainName
        {
            get
            {
                if (_domainName == null)
                    _domainName = this.GetInformationString(WtsInformationClass.DomainName);
                return _domainName;
            }
        }

        public string ClientName
        {
            get
            {
                if (_clientName == null)
                    _clientName = this.GetInformationString(WtsInformationClass.ClientName);
                return _clientName;
            }
        }

        public string ClientDirectory
        {
            get
            {
                if (_clientDirectory == null)
                    _clientDirectory = this.GetInformationString(WtsInformationClass.ClientDirectory);
                return _clientDirectory;
            }
        }

        public System.Net.IPAddress ClientAddress
        {
            get
            {
                if (_clientAddress == null)
                {
                    IntPtr dataPtr;
                    int length;

                    if (!Win32.WTSQuerySessionInformation(
                        _serverHandle, _sessionId, WtsInformationClass.ClientAddress, out dataPtr, out length))
                        Win32.ThrowLastError();

                    unsafe
                    {
                        using (var data = new WtsMemoryAlloc(dataPtr))
                            _clientAddress = new System.Net.IPAddress(data.ReadStruct<WtsClientAddress>().Address);
                    }
                }

                return _clientAddress;
            }
        }

        public WtsClientDisplay ClientDisplay
        {
            get
            {
                if (_clientDisplay == null)
                {
                    IntPtr dataPtr;
                    int length;

                    if (!Win32.WTSQuerySessionInformation(
                        _serverHandle, _sessionId, WtsInformationClass.ClientDisplay, out dataPtr, out length))
                        Win32.ThrowLastError();

                    using (var data = new WtsMemoryAlloc(dataPtr))
                        _clientDisplay = data.ReadStruct<WtsClientDisplay>();
                }

                return _clientDisplay.Value;
            }
        }

        public void Disconnect()
        {
            this.Disconnect(false);
        }

        public void Disconnect(bool synchronous)
        {
            if (!Win32.WTSDisconnectSession(_serverHandle, _sessionId, synchronous))
                Win32.ThrowLastError();
        }

        public string GetInformationString(WtsInformationClass infoClass)
        {
            IntPtr data;
            int length;

            if (!Win32.WTSQuerySessionInformation(_serverHandle, _sessionId, infoClass, out data, out length))
                Win32.ThrowLastError();

            if (data == IntPtr.Zero)
                return null;

            using (new WtsMemoryAlloc(data))
                return Marshal.PtrToStringUni(data);
        }

        public void Logoff()
        {
            this.Logoff(false);
        }

        public void Logoff(bool synchronous)
        {
            if (!Win32.WTSLogoffSession(_serverHandle, _sessionId, synchronous))
                Win32.ThrowLastError();
        }

        public DialogResult SendMessage(string title, string message)
        {
            return this.SendMessage(title, message, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        public DialogResult SendMessage(
            string title,
            string message,
            MessageBoxButtons buttons,
            MessageBoxIcon icon
            )
        {
            return this.SendMessage(title, message, buttons, icon, 0, 0, 0, false);
        }

        public DialogResult SendMessage(
            string title,
            string message,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            MessageBoxOptions options,
            int secondsTimeout,
            bool synchronous
            )
        {
            DialogResult response;

            if (!Win32.WTSSendMessage(
                _serverHandle,
                _sessionId,
                title,
                title.Length * 2,
                message,
                message.Length * 2,
                (int)buttons | (int)icon | (int)defaultButton | (int)options,
                secondsTimeout,
                out response,
                synchronous
                ))
                Win32.ThrowLastError();

            return response;
        }
    }

    public class TerminalServerProcess
    {
        private int _processId;
        private int _sessionId;
        private string _name;
        private Sid _sid;

        internal TerminalServerProcess(int processId, int sessionId, string name, Sid sid)
        {
            _processId = processId;
            _sessionId = sessionId;
            _name = name;
            _sid = sid;
        }

        public int ProcessId { get { return _processId; } }
        public int SessionId { get { return _sessionId; } }
        public string Name { get { return _name; } }
        public Sid Sid { get { return _sid; } }
    }
}