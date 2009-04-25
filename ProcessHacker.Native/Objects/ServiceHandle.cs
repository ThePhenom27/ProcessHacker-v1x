﻿/*
 * Process Hacker - 
 *   service handle
 * 
 * Copyright (C) 2008 wj32
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
using ProcessHacker.Native.Api;
using ProcessHacker.Native.Security;

namespace ProcessHacker.Native.Objects
{
    /// <summary>
    /// Represents a handle to a Windows service.
    /// </summary>
    public class ServiceHandle : ServiceBaseHandle<ServiceAccess>
    {
        /// <summary>
        /// Creates a service handle using an existing handle. 
        /// The handle will not be closed automatically.
        /// </summary>
        /// <param name="Handle">The handle value.</param>
        /// <returns>The service handle.</returns>
        public static ServiceHandle FromHandle(int handle)
        {
            return new ServiceHandle(handle, false);
        }

        internal ServiceHandle(int handle, bool owned)
            : base(handle, owned)
        { }

        /// <summary>
        /// Creates a new service handle.
        /// </summary>
        /// <param name="ServiceName">The name of the service to open.</param>
        public ServiceHandle(string serviceName)
            : this(serviceName, ServiceAccess.All)
        { }

        /// <summary>
        /// Creates a new service handle.
        /// </summary>
        /// <param name="ServiceName">The name of the service to open.</param>
        /// <param name="access">The desired access to the service.</param>
        public ServiceHandle(string serviceName, ServiceAccess access)
        {
            using (ServiceManagerHandle manager =
                new ServiceManagerHandle(ScManagerAccess.Connect))
            {
                this.Handle = Win32.OpenService(manager, serviceName, access);

                if (this.Handle == 0)
                    Win32.ThrowLastError();
            }
        }

        /// <summary>
        /// Sends a control message to the service.
        /// </summary>
        /// <param name="control">The message.</param>
        public void Control(ServiceControl control)
        {
            ServiceStatus status = new ServiceStatus();

            if (!Win32.ControlService(this.Handle, control, ref status))
                Win32.ThrowLastError();
        }

        /// <summary>
        /// Deletes the service.
        /// </summary>
        public void Delete()
        {
            if (!Win32.DeleteService(this.Handle))
                Win32.ThrowLastError();
        }

        /// <summary>
        /// Gets the service's configuration.
        /// </summary>
        public QueryServiceConfig GetConfig()
        {
            int requiredSize = 0;

            Win32.QueryServiceConfig(this, IntPtr.Zero, 0, ref requiredSize);

            using (MemoryAlloc data = new MemoryAlloc(requiredSize))
            {
                if (!Win32.QueryServiceConfig(this, data, data.Size, ref requiredSize))
                    Win32.ThrowLastError();

                return data.ReadStruct<QueryServiceConfig>();
            }
        }

        /// <summary>
        /// Gets the service's description.
        /// </summary>
        /// <returns>A string.</returns>
        public string GetDescription()
        {
            int retLen;

            Win32.QueryServiceConfig2(this, ServiceInfoLevel.Description, IntPtr.Zero, 0, out retLen);

            using (MemoryAlloc data = new MemoryAlloc(retLen))
            {
                if (!Win32.QueryServiceConfig2(this, ServiceInfoLevel.Description, data, retLen, out retLen))
                    Win32.ThrowLastError();

                return data.ReadStruct<ServiceDescription>().Description;
            }
        }

        /// <summary>
        /// Gets the status of the service.
        /// </summary>
        /// <returns>A SERVICE_STATUS_PROCESS structure.</returns>
        public ServiceStatusProcess GetStatus()
        {
            ServiceStatusProcess status = new ServiceStatusProcess();
            int retLen;

            if (!Win32.QueryServiceStatusEx(this, 0, ref status, Marshal.SizeOf(status), out retLen))
                Win32.ThrowLastError();

            return status;
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        public void Start()
        {
            if (!Win32.StartService(this.Handle, 0, 0))
                Win32.ThrowLastError();
        }
    }

    public enum ServiceAccept : int
    {
        NetBindChange = 0x10,
        ParamChange = 0x8,
        PauseContinue = 0x2,
        PreShutdown = 0x100,
        Shutdown = 0x4,
        Stop = 0x1,
        HardwareProfileChange = 0x20,
        PowerEvent = 0x40,
        SessionChange = 0x80
    }

    public enum ServiceControl : int
    {
        Continue = 0x3,
        Interrogate = 0x4,
        NetBindAdd = 0x7,
        NetBindDisable = 0xa,
        NetBindEnable = 0x9,
        NetBindRemove = 0x8,
        ParamChange = 0x6,
        Pause = 0x2,
        Stop = 0x1
    }

    public enum ServiceErrorControl : int
    {
        Critical = 0x3,
        Ignore = 0x0,
        Normal = 0x1,
        Severe = 0x2
    }

    public enum ServiceFlags : int
    {
        None = 0,
        RunsInSystemProcess = 0x1
    }

    public enum ServiceInfoLevel : int
    {
        Description = 1,
        FailureActions = 2,
        DelayedAutoStartInfo = 3,
        FailureActionsFlag = 4,
        SidInfo = 5,
        RequiredPrivilegesInfo = 6,
        PreShutdownInfo = 7,
        TriggerInfo = 8,
        PreferredNode = 9
    }

    public enum ServiceQueryState : int
    {
        Active = 1,
        Inactive = 2,
        All = 3
    }

    [Flags]
    public enum ServiceQueryType : int
    {
        Driver = 0xb,
        Win32 = 0x30
    }

    public enum ServiceStartType : int
    {
        AutoStart = 0x2,
        BootStart = 0x0,
        DemandStart = 0x3,
        Disabled = 0x4,
        SystemStart = 0x1
    }

    public enum ServiceState : int
    {
        ContinuePending = 0x5,
        PausePending = 0x6,
        Paused = 0x7,
        Running = 0x4,
        StartPending = 0x2,
        StopPending = 0x3,
        Stopped = 0x1
    }

    [Flags]
    public enum ServiceType : int
    {
        FileSystemDriver = 0x2,
        KernelDriver = 0x1,
        Win32OwnProcess = 0x10,
        Win32ShareProcess = 0x20,
        InteractiveProcess = 0x100
    }
}