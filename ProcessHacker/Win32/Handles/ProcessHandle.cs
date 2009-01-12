﻿/*
 * Process Hacker
 * 
 * Copyright (C) 2008 wj32
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Runtime.InteropServices;

namespace ProcessHacker
{
    public partial class Win32
    {
        /// <summary>
        /// Represents a handle to a Windows process.
        /// </summary>
        public class ProcessHandle : Win32Handle, IWithToken
        {
            /// <summary>
            /// Specifies an offset in a process' process environment block (PEB).
            /// </summary>
            public enum PEBOffset
            {
                CurrentDirectoryPath = 0x24,
                DllPath = 0x30, // usually a copy of the PATH environment variable
                ImagePathName = 0x38,
                CommandLine = 0x40,
                WindowTitle = 0x70,
                DesktopName = 0x78,
                ShellInfo = 0x80,
                RuntimeData = 0x88
            }

            /// <summary>
            /// Specifies the DEP status of a process.
            /// </summary>
            [Flags]
            public enum DEPStatus
            {
                /// <summary>
                /// DEP is enabled.
                /// </summary>
                Enabled = 0x1,
                
                /// <summary>
                /// DEP is permanently enabled or disabled and cannot
                /// be enabled or disabled.
                /// </summary>
                Permanent = 0x2,
                
                /// <summary>
                /// DEP is enabled with DEP-ATL thunk emulation disabled.
                /// </summary>
                ATLThunkEmulationDisabled = 0x4
            }

            /// <summary>
            /// Creates a process handle using an existing handle. 
            /// The handle will not be closed automatically.
            /// </summary>
            /// <param name="Handle">The handle value.</param>
            /// <returns>The process handle.</returns>
            public static ProcessHandle FromHandle(int Handle)
            {
                return new ProcessHandle(Handle, false);
            }

            internal ProcessHandle(int Handle, bool Owned)
                : base(Handle, Owned)
            { }

            /// <summary>
            /// Creates a new process handle.
            /// </summary>
            /// <param name="PID">The ID of the process to open.</param>
            public ProcessHandle(int PID)
                : this(PID, PROCESS_RIGHTS.PROCESS_ALL_ACCESS)
            { }

            /// <summary>
            /// Creates a new process handle.
            /// </summary>
            /// <param name="PID">The ID of the process to open.</param>
            /// <param name="access">The desired access to the process.</param>
            public ProcessHandle(int PID, PROCESS_RIGHTS access)
            {
                this.Handle = OpenProcess(access, 0, PID);

                if (this.Handle == 0)
                    ThrowLastWin32Error();
            }

            /// <summary>
            /// Allocates a memory region in the process' virtual memory.
            /// </summary>      
            /// <param name="address">The base address of the region.</param>
            /// <param name="size">The size of the region.</param>
            /// <param name="protection">The protection of the region.</param>
            /// <returns>The base address of the allocated pages.</returns>
            public int AllocMemory(int address, int size, MEMORY_PROTECTION protection)
            {
                int newAddress;

                if ((newAddress = VirtualAllocEx(this, address, size, MEMORY_STATE.MEM_COMMIT, protection))
                    == 0)
                    ThrowLastWin32Error();

                return newAddress;
            }

            /// <summary>
            /// Allocates a memory region in the process' virtual memory. The function decides where 
            /// to allocate the memory.
            /// </summary>
            /// <param name="size">The size of the region.</param>
            /// <param name="protection">The protection of the region.</param>
            /// <returns>The base address of the allocated pages.</returns>
            public int AllocMemory(int size, MEMORY_PROTECTION protection)
            {
                return this.AllocMemory(0, size, protection);
            }

            /// <summary>
            /// Creates a remote thread in the process.
            /// </summary>
            /// <param name="startAddress">The address at which to begin execution (e.g. a function). The 
            /// function must be accessible from the remote process; that is, it must be in its 
            /// virtual address space, either copied using AllocMemory or loaded as module using 
            /// LoadLibrary.
            /// </param>
            /// <param name="parameter">The parameter to pass to the function.</param>
            /// <returns>The ID of the new thread.</returns>
            public int CreateThread(int startAddress, int parameter)
            {
                int threadId;

                if (!CreateRemoteThread(this, 0, 0, startAddress, parameter, 0, out threadId))
                    ThrowLastWin32Error();

                return threadId;
            }

            /// <summary>
            /// Frees a memory region in the process' virtual memory.
            /// </summary>
            /// <param name="address">The address of the region to free.</param>
            /// <param name="size">The size to free.</param>
            /// <param name="reserveOnly">Specifies whether or not to only 
            /// reserve the memory instead of freeing it.</param>
            public void FreeMemory(int address, int size, bool reserveOnly)
            {
                // size needs to be 0 if we're freeing
                if (!reserveOnly)
                    size = 0;

                if (!VirtualFreeEx(this, address, size,
                    reserveOnly ? MEMORY_STATE.MEM_DECOMMIT : MEMORY_STATE.MEM_RELEASE))
                    ThrowLastWin32Error();
            }

            /// <summary>
            /// Gets the process' basic information through the undocumented Native API function 
            /// ZwQueryInformationProcess. This function requires the PROCESS_QUERY_LIMITED_INFORMATION 
            /// permission.
            /// </summary>
            /// <returns>A PROCESS_BASIC_INFORMATION structure.</returns>
            public PROCESS_BASIC_INFORMATION GetBasicInformation()
            {
                PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
                int retLen;

                if (ZwQueryInformationProcess(this, PROCESS_INFORMATION_CLASS.ProcessBasicInformation,
                    ref pbi, Marshal.SizeOf(pbi), out retLen) != 0)
                    ThrowLastWin32Error();

                return pbi;
            }

            /// <summary>
            /// Gets the command line used to start the process. This requires 
            /// the PROCESS_QUERY_LIMITED_INFORMATION and PROCESS_VM_READ permissions.
            /// </summary>
            /// <returns>A string.</returns>
            public string GetCommandLine()
            {
                return this.GetPEBString(PEBOffset.CommandLine);
            }

            /// <summary>
            /// Gets the process' DEP policy.
            /// </summary>
            /// <returns>A DEPStatus enum.</returns>
            public DEPStatus GetDEPStatus()
            {
                DEPFLAGS flags;
                int perm;

                if (!GetProcessDEPPolicy(this, out flags, out perm))
                    ThrowLastWin32Error();

                return
                    ((flags & DEPFLAGS.PROCESS_DEP_ENABLE) != 0 ? DEPStatus.Enabled : 0) |
                    ((flags & DEPFLAGS.PROCESS_DEP_DISABLE_ATL_THUNK_EMULATION) != 0 ? 
                    (DEPStatus.Enabled | DEPStatus.ATLThunkEmulationDisabled) : 0) |
                    ((perm != 0) ? DEPStatus.Permanent : 0);
            }

            /// <summary>
            /// Gets the file name of the process' image. This requires 
            /// the PROCESS_QUERY_LIMITED_INFORMATION and PROCESS_VM_READ permissions.
            /// </summary>
            /// <returns>A file name, in kernel file name format.</returns>
            public string GetImageFileName()
            {
                return this.GetPEBString(PEBOffset.ImagePathName);
            }

            /// <summary>
            /// Gets the process' parent's process ID. This requires 
            /// the PROCESS_QUERY_LIMITED_INFORMATION permission.
            /// </summary>
            /// <returns>The process ID.</returns>
            public int GetParentPID()
            {
                return this.GetBasicInformation().InheritedFromUniqueProcessId;
            }

            /// <summary>
            /// Reads a UNICODE_STRING from the process' process environment block.
            /// </summary>
            /// <param name="offset">The offset to the UNICODE_STRING structure.</param>
            /// <returns>A string.</returns>
            public string GetPEBString(PEBOffset offset)
            {
                int pebBaseAddress = 0x7ffd7000;

                // get the real PEB address of the process if we can.
                try
                {
                    pebBaseAddress = this.GetBasicInformation().PebBaseAddress;
                }
                catch
                { }

                /* read address of parameter information block
                 *
                 * PEB
                 * off field
                 * +00 BOOLEAN InheritedAddressSpace;
                 * +01 BOOLEAN ReadImageFileExecOptions;
                 * +02 BOOLEAN BeingDebugged;
                 * +03 BOOLEAN Spare;
                 * +04 HANDLE Mutant;
                 * +08 PVOID ImageBaseAddress;
                 * +0c PVOID LoaderData;
                 * +10 PRTL_USER_PROCESS_PARAMETERS ProcessParameters; 
                 */
                int paramInfoAddrI = 
                    Misc.BytesToInt(this.ReadMemory(pebBaseAddress + 0x10, 4), Misc.Endianness.Little);

                // Read length (in bytes) of string. The offset of the UNICODE_STRING structure is 
                // specified in the enum.
                //
                // UNICODE_STRING
                // off field
                // +00 USHORT Length;
                // +02 USHORT MaximumLength;
                // +04 PWSTR Buffer;
                ushort strLength = Misc.BytesToUShort(
                    this.ReadMemory(paramInfoAddrI + (int)offset, 2), Misc.Endianness.Little);
                byte[] stringData = new byte[strLength];

                // read address of string
                int strAddr = Misc.BytesToInt(
                    this.ReadMemory(paramInfoAddrI + (int)offset + 0x4, 4), Misc.Endianness.Little);

                // read string and decode it
                return System.Text.UnicodeEncoding.Unicode.GetString(
                    this.ReadMemory(strAddr, strLength)).TrimEnd('\0');
            }

            /// <summary>
            /// Gets whether the process is currently being debugged. This requires 
            /// the PROCESS_QUERY_INFORMATION permission.
            /// </summary>
            /// <returns>A boolean value.</returns>
            public bool IsBeingDebugged()
            {
                bool debugged;

                if (!Win32.CheckRemoteDebuggerPresent(this, out debugged))
                    ThrowLastWin32Error();

                return debugged;
            }

            /// <summary>
            /// Determines whether the process is running in a job.
            /// </summary>
            /// <returns>A boolean.</returns>
            public bool IsInJob()
            {
                bool result;

                if (!IsProcessInJob(this, 0, out result))
                    ThrowLastWin32Error();

                return result;
            }

            /// <summary>
            /// Reads data from the process' virtual memory.
            /// </summary>
            /// <param name="offset">The offset at which to begin reading.</param>
            /// <param name="length">The length, in bytes, to read.</param>
            /// <returns>An array of bytes</returns>
            public byte[] ReadMemory(int offset, int length)
            {
                byte[] buf = new byte[length];
                int readLen;

                if (!ReadProcessMemory(this, offset, buf, length, out readLen))
                    ThrowLastWin32Error();

                return buf;
            }

            /// <summary>
            /// Waits for the process to terminate.
            /// </summary>
            /// <param name="Timeout">The timeout of the wait.</param>
            /// <returns>Either WAIT_OBJECT_0, WAIT_TIMEOUT or WAIT_FAILED.</returns>
            public int Wait(int Timeout)
            {
                return WaitForSingleObject(this.Handle, Timeout);
            }

            /// <summary>
            /// Writes data to the process' virtual memory.
            /// </summary>
            /// <param name="offset">The offset at which to begin writing.</param>
            /// <param name="data">The data to write.</param>
            /// <returns>The length, in bytes, that was written.</returns>
            public int WriteMemory(int offset, byte[] data)
            {
                int writLen;

                if (!WriteProcessMemory(this, offset, data, data.Length, out writLen))
                    ThrowLastWin32Error();

                return writLen;
            }

            /// <summary>
            /// Terminates the process. This requires the PROCESS_TERMINATE permission.
            /// </summary>
            public void Terminate()
            {
                this.Terminate(0);
            }

            /// <summary>
            /// Terminates the process, specifying the exit code. This requires the 
            /// PROCESS_TERMINATE permission.
            /// </summary>
            /// <param name="ExitCode">The exit code.</param>
            public void Terminate(int ExitCode)
            {
                if (!TerminateProcess(this, ExitCode))
                    ThrowLastWin32Error();
            }

            /// <summary>
            /// Opens and returns a handle to the process' token. This requires the 
            /// PROCESS_QUERY_LIMITED_INFORMATION permission.
            /// </summary>
            /// <returns>A handle to the process' token.</returns>
            public TokenHandle GetToken()
            {
                return GetToken(TOKEN_RIGHTS.TOKEN_ALL_ACCESS);
            }

            /// <summary>
            /// Opens and returns a handle to the process' token. This requires the 
            /// PROCESS_QUERY_LIMITED_INFORMATION permission.
            /// </summary>
            /// <param name="access">The desired access to the token.</param>
            /// <returns>A handle to the process' token.</returns>
            public TokenHandle GetToken(TOKEN_RIGHTS access)
            {
                return new TokenHandle(this, access);
            }
        }
    }
}