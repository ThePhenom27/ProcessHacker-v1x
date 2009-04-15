﻿/*
 * Process Hacker - 
 *   interfacing code to kernel-mode driver
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
using System.ComponentModel;
using ProcessHacker.PE;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProcessHacker
{
    /// <summary>
    /// Provides an interface to KProcessHacker.
    /// </summary>
    public class KProcessHacker
    {         
        /// <summary>
        /// A control code used by KProcessHacker to represent a specific function.
        /// </summary>
        private enum Control : uint
        {
            Read = 0,
            Write,
            GetFileObjectName,
            KphOpenProcess,
            KphOpenThread,
            KphOpenProcessToken,
            GetProcessProtected,
            SetProcessProtected,
            KphTerminateProcess,
            KphSuspendProcess,
            KphResumeProcess,
            KphReadVirtualMemory,
            KphWriteVirtualMemory,
            SetProcessToken,
            GetThreadWin32StartAddress,
            GetObjectName,
            GetHandleObjectName,
            KphOpenProcessJob,
            KphGetContextThread,
            KphSetContextThread,
            KphGetThreadWin32Thread
        }

        private string _deviceName;
        private Win32.FileHandle _fileHandle;
        private uint _baseControlNumber;
        private Win32.ServiceHandle _service;

        /// <summary>
        /// Creates a connection to KProcessHacker.
        /// </summary>
        /// <param name="deviceName">The device to connect to.</param>
        public KProcessHacker(string deviceName)
        {
            _deviceName = deviceName;

            bool started = false;

            if (!Properties.Settings.Default.EnableKPH)
                throw new Exception("KProcessHacker is not enabled.");

            // delete the service if it exists
            try
            {
                using (var shandle = new Win32.ServiceHandle(deviceName))
                {
                    started = shandle.GetStatus().CurrentState == Win32.SERVICE_STATE.Running;

                    if (!started)
                        shandle.Delete();
                }
            }
            catch
            { }

            try
            {
                Win32.ServiceManagerHandle scm =
                    new Win32.ServiceManagerHandle(Win32.SC_MANAGER_RIGHTS.SC_MANAGER_CREATE_SERVICE);

                _service = scm.CreateService(deviceName, deviceName, Win32.SERVICE_TYPE.KernelDriver,
                    Application.StartupPath + "\\kprocesshacker.sys");
                _service.Start();
            }
            catch
            { }

            _fileHandle = new Win32.FileHandle("\\\\.\\" + deviceName,
                Win32.FILE_RIGHTS.FILE_GENERIC_READ | Win32.FILE_RIGHTS.FILE_GENERIC_WRITE);

            try
            {
                if (!started)
                    _service.Delete(); // the service will automatically get deleted once it stops
            }
            catch
            { }

            _baseControlNumber = Misc.BytesToUInt(_fileHandle.Read(4), Misc.Endianness.Little);
        }

        public string DeviceName
        {
            get { return _deviceName; }
        }  

        private uint CtlCode(Control ctl)
        {
            return _baseControlNumber + ((uint)ctl * 4);
        }

        /// <summary>
        /// Closes the connection to KProcessHacker.
        /// </summary>
        public void Close()
        {
            _fileHandle.Dispose();
        }

        public string GetFileObjectName(Win32.SYSTEM_HANDLE_INFORMATION handle)
        {
            byte[] buffer = new byte[12];
            byte[] outBuffer = new byte[2048];

            Array.Copy(Misc.IntToBytes(handle.Handle, Misc.Endianness.Little), buffer, 4);
            Array.Copy(Misc.IntToBytes(handle.Object, Misc.Endianness.Little), 0, buffer, 4, 4);
            Array.Copy(Misc.IntToBytes(handle.ProcessId, Misc.Endianness.Little), 0, buffer, 8, 4);

            try
            {
                int len = _fileHandle.IoControl(CtlCode(Control.GetFileObjectName), buffer, outBuffer);

                return UnicodeEncoding.Unicode.GetString(outBuffer, 8, len - 8).TrimEnd('\0');
            }
            catch
            { }

            return null;
        }

        public string GetHandleObjectName(Win32.Win32Handle handle)
        {
            byte[] outBuffer = new byte[2048];

            try
            {
                int len = _fileHandle.IoControl(CtlCode(Control.GetHandleObjectName),
                    Misc.IntToBytes(handle, Misc.Endianness.Little), outBuffer);

                return UnicodeEncoding.Unicode.GetString(outBuffer, 8, len - 8).TrimEnd('\0');
            }
            catch
            { }

            return null;
        }

        public string GetObjectName(int obj)
        {
            byte[] outBuffer = new byte[2048];

            try
            {
                int len = _fileHandle.IoControl(CtlCode(Control.GetObjectName),
                    Misc.IntToBytes(obj, Misc.Endianness.Little), outBuffer);

                return UnicodeEncoding.Unicode.GetString(outBuffer, 8, len - 8).TrimEnd('\0');
            }
            catch
            { }

            return null;
        }

        public bool GetProcessProtected(int pid)
        {
            byte[] result = new byte[1];

            _fileHandle.IoControl(CtlCode(Control.GetProcessProtected),
                Misc.IntToBytes(pid, Misc.Endianness.Little), result);

            return result[0] != 0;
        }

        public uint GetThreadWin32StartAddress(Win32.ThreadHandle threadHandle)
        {
            byte[] buffer = new byte[4];

            _fileHandle.IoControl(CtlCode(Control.GetThreadWin32StartAddress),
                Misc.IntToBytes(threadHandle, Misc.Endianness.Little), buffer);

            return Misc.BytesToUInt(buffer, Misc.Endianness.Little);
        }

        public unsafe void KphGetContextThread(Win32.ThreadHandle threadHandle, Win32.CONTEXT* context)
        {
            byte[] data = new byte[8];

            Array.Copy(Misc.IntToBytes(threadHandle, Misc.Endianness.Little), 0, data, 0, 4);
            Array.Copy(Misc.IntToBytes((int)context, Misc.Endianness.Little), 0, data, 4, 4);

            _fileHandle.IoControl(CtlCode(Control.KphGetContextThread), data, null);
        }

        public int KphGetThreadWin32Thread(Win32.ThreadHandle threadHandle)
        {
            byte[] inData = Misc.IntToBytes(threadHandle, Misc.Endianness.Little);
            byte[] outData = new byte[4];

            _fileHandle.IoControl(CtlCode(Control.KphGetThreadWin32Thread), inData, outData);

            return Misc.BytesToInt(outData, Misc.Endianness.Little);
        }

        public int KphOpenProcess(int pid, Win32.PROCESS_RIGHTS desiredAccess)
        {
            byte[] inData = new byte[8];
            byte[] outData = new byte[4];

            Array.Copy(Misc.IntToBytes(pid, Misc.Endianness.Little), 0, inData, 0, 4);
            Array.Copy(Misc.UIntToBytes((uint)desiredAccess, Misc.Endianness.Little), 0, inData, 4, 4);

            _fileHandle.IoControl(CtlCode(Control.KphOpenProcess), inData, outData);

            return Misc.BytesToInt(outData, Misc.Endianness.Little);
        }

        public int KphOpenProcessJob(Win32.ProcessHandle processHandle, Win32.JOB_OBJECT_RIGHTS desiredAccess)
        {
            byte[] inData = new byte[8];
            byte[] outData = new byte[4];

            Array.Copy(Misc.IntToBytes(processHandle, Misc.Endianness.Little), 0, inData, 0, 4);
            Array.Copy(Misc.UIntToBytes((uint)desiredAccess, Misc.Endianness.Little), 0, inData, 4, 4);

            _fileHandle.IoControl(CtlCode(Control.KphOpenProcessJob), inData, outData);

            return Misc.BytesToInt(outData, Misc.Endianness.Little);
        }

        public int KphOpenProcessToken(Win32.ProcessHandle processHandle, Win32.TOKEN_RIGHTS desiredAccess)
        {
            byte[] inData = new byte[8];
            byte[] outData = new byte[4];

            Array.Copy(Misc.IntToBytes(processHandle, Misc.Endianness.Little), 0, inData, 0, 4);
            Array.Copy(Misc.UIntToBytes((uint)desiredAccess, Misc.Endianness.Little), 0, inData, 4, 4);

            _fileHandle.IoControl(CtlCode(Control.KphOpenProcessToken), inData, outData);

            return Misc.BytesToInt(outData, Misc.Endianness.Little);
        }

        public int KphOpenThread(int tid, Win32.THREAD_RIGHTS desiredAccess)
        {
            byte[] inData = new byte[8];
            byte[] outData = new byte[4];

            Array.Copy(Misc.IntToBytes(tid, Misc.Endianness.Little), 0, inData, 0, 4);
            Array.Copy(Misc.UIntToBytes((uint)desiredAccess, Misc.Endianness.Little), 0, inData, 4, 4);

            _fileHandle.IoControl(CtlCode(Control.KphOpenThread), inData, outData);

            return Misc.BytesToInt(outData, Misc.Endianness.Little);
        }

        public unsafe void KphReadVirtualMemory(Win32.ProcessHandle processHandle, int baseAddress, byte[] buffer, int length, out int bytesRead)
        {
            fixed (byte* bufferPointer = buffer)
            {
                this.KphReadVirtualMemory(processHandle, baseAddress, bufferPointer, length, out bytesRead);
            }
        }

        public unsafe void KphReadVirtualMemory(Win32.ProcessHandle processHandle, int baseAddress, void* buffer, int length, out int bytesRead)
        {
            byte[] data = new byte[0x15];
            int returnLength;

            Array.Copy(Misc.IntToBytes(processHandle, Misc.Endianness.Little), 0, data, 0x0, 4);
            Array.Copy(Misc.IntToBytes(baseAddress, Misc.Endianness.Little), 0, data, 0x4, 4);
            Array.Copy(Misc.IntToBytes((int)buffer, Misc.Endianness.Little), 0, data, 0x8, 4);
            Array.Copy(Misc.IntToBytes(length, Misc.Endianness.Little), 0, data, 0xc, 4);
            Array.Copy(Misc.IntToBytes((int)&returnLength, Misc.Endianness.Little), 0, data, 0x10, 4);

            _fileHandle.IoControl(CtlCode(Control.KphReadVirtualMemory), data, null);
            bytesRead = returnLength;
        }

        public unsafe bool KphReadVirtualMemorySafe(Win32.ProcessHandle processHandle, int baseAddress, byte[] buffer, int length, out int bytesRead)
        {
            byte[] data = new byte[0x15];
            int returnLength;
            int br;

            fixed (byte* bufferPointer = buffer)
            {
                Array.Copy(Misc.IntToBytes(processHandle, Misc.Endianness.Little), 0, data, 0x0, 4);
                Array.Copy(Misc.IntToBytes(baseAddress, Misc.Endianness.Little), 0, data, 0x4, 4);
                Array.Copy(Misc.IntToBytes((int)bufferPointer, Misc.Endianness.Little), 0, data, 0x8, 4);
                Array.Copy(Misc.IntToBytes(length, Misc.Endianness.Little), 0, data, 0xc, 4);
                Array.Copy(Misc.IntToBytes((int)&br, Misc.Endianness.Little), 0, data, 0x10, 4);

                bool r = Win32.DeviceIoControl(_fileHandle, (int)CtlCode(Control.KphReadVirtualMemory), data, data.Length, null, 0, out returnLength, 0);
                
                bytesRead = br;

                return r;
            }
        }

        public void KphResumeProcess(Win32.ProcessHandle processHandle)
        {
            _fileHandle.IoControl(CtlCode(Control.KphResumeProcess),
                Misc.IntToBytes(processHandle, Misc.Endianness.Little), null);
        }

        public unsafe void KphSetContextThread(Win32.ThreadHandle threadHandle, Win32.CONTEXT* context)
        {
            byte[] data = new byte[8];

            Array.Copy(Misc.IntToBytes(threadHandle, Misc.Endianness.Little), 0, data, 0, 4);
            Array.Copy(Misc.IntToBytes((int)context, Misc.Endianness.Little), 0, data, 4, 4);

            _fileHandle.IoControl(CtlCode(Control.KphSetContextThread), data, null);
        }

        public void KphSuspendProcess(Win32.ProcessHandle processHandle)
        {
            _fileHandle.IoControl(CtlCode(Control.KphSuspendProcess), 
                Misc.IntToBytes(processHandle, Misc.Endianness.Little), null);
        }

        public void KphTerminateProcess(Win32.ProcessHandle processHandle, int exitStatus)
        {
            byte[] data = new byte[8];

            Array.Copy(Misc.IntToBytes(processHandle, Misc.Endianness.Little), 0, data, 0, 4);
            Array.Copy(Misc.IntToBytes(exitStatus, Misc.Endianness.Little), 0, data, 4, 4);

            try
            {
                _fileHandle.IoControl(CtlCode(Control.KphTerminateProcess), data, null);
            }
            catch (WindowsException ex)
            {
                // STATUS_DISK_FULL means we tried to terminate ourself. Kernel-mode can't do it, 
                // so we do it now.
                if (ex.ErrorCode == 112)
                    Win32.ExitProcess(exitStatus);
            }
        }

        public unsafe void KphWriteVirtualMemory(Win32.ProcessHandle processHandle, int baseAddress, byte[] buffer, int length, out int bytesWritten)
        {
            byte[] data = new byte[0x14];
            int returnLength;

            fixed (byte* bufferPointer = buffer)
            {
                Array.Copy(Misc.IntToBytes(processHandle, Misc.Endianness.Little), 0, data, 0x0, 4);
                Array.Copy(Misc.IntToBytes(baseAddress, Misc.Endianness.Little), 0, data, 0x4, 4);
                Array.Copy(Misc.IntToBytes((int)bufferPointer, Misc.Endianness.Little), 0, data, 0x8, 4);
                Array.Copy(Misc.IntToBytes(length, Misc.Endianness.Little), 0, data, 0xc, 4);
                Array.Copy(Misc.IntToBytes((int)&returnLength, Misc.Endianness.Little), 0, data, 0x10, 4);

                _fileHandle.IoControl(CtlCode(Control.KphWriteVirtualMemory), data, null);
                bytesWritten = returnLength;
            }
        }

        public byte[] Read(int address, int length)
        {
            byte[] buffer = new byte[length];

            _fileHandle.IoControl(CtlCode(Control.Read), Misc.IntToBytes(address, Misc.Endianness.Little), buffer);

            return buffer;
        }

        public void SetProcessProtected(int pid, bool protecte)
        {
            byte[] data = new byte[5];

            Array.Copy(Misc.IntToBytes(pid, Misc.Endianness.Little), 0, data, 0, 4);
            data[4] = (byte)(protecte ? 1 : 0);

            _fileHandle.IoControl(CtlCode(Control.SetProcessProtected), data, null);
        }

        public void SetProcessToken(int sourcePid, int targetPid)
        {
            byte[] data = new byte[8];

            Array.Copy(Misc.IntToBytes(sourcePid, Misc.Endianness.Little), 0, data, 0, 4);
            Array.Copy(Misc.IntToBytes(targetPid, Misc.Endianness.Little), 0, data, 4, 4);

            _fileHandle.IoControl(CtlCode(Control.SetProcessToken), data, null); 
        }

        public int Write(int address, byte[] data)
        {
            byte[] newData = new byte[data.Length + 4];

            Array.Copy(Misc.IntToBytes(address, Misc.Endianness.Little), newData, 4);
            Array.Copy(data, 0, newData, 4, data.Length);

            return _fileHandle.IoControl(CtlCode(Control.Write), newData, null);
        }
    }
}