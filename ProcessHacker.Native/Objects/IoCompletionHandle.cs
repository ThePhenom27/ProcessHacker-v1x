﻿/*
 * Process Hacker - 
 *   I/O completion handle
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
using ProcessHacker.Native.Api;
using ProcessHacker.Native.Security;

namespace ProcessHacker.Native.Objects
{
    public sealed class IoCompletionHandle : NativeHandle<IoCompletionAccess>
    {
        public static IoCompletionHandle Create(IoCompletionAccess access)
        {
            return Create(access, 0);
        }

        public static IoCompletionHandle Create(IoCompletionAccess access, int count)
        {
            return Create(access, null, count);
        }

        public static IoCompletionHandle Create(IoCompletionAccess access, string name, int count)
        {
            return Create(access, name, 0, null, count);
        }

        public static IoCompletionHandle Create(IoCompletionAccess access, string name, ObjectFlags objectFlags, DirectoryHandle rootDirectory, int count)
        {
            ObjectAttributes oa = new ObjectAttributes(name, objectFlags, rootDirectory);
            IntPtr handle;

            try
            {
                Win32.NtCreateIoCompletion(out handle, access, ref oa, count).ThrowIf();
            }
            finally
            {
                oa.Dispose();
            }

            return new IoCompletionHandle(handle, true);
        }

        private IoCompletionHandle(IntPtr handle, bool owned)
            : base(handle, owned)
        { }

        public IoCompletionHandle(string name, ObjectFlags objectFlags, DirectoryHandle rootDirectory, IoCompletionAccess access)
        {
            ObjectAttributes oa = new ObjectAttributes(name, objectFlags, rootDirectory);
            IntPtr handle;

            try
            {
                Win32.NtOpenIoCompletion(out handle, access, ref oa).ThrowIf();
            }
            finally
            {
                oa.Dispose();
            }

            this.Handle = handle;
        }

        public IoCompletionHandle(string name, IoCompletionAccess access)
            : this(name, 0, null, access)
        { }

        public bool Remove(out IoStatusBlock isb, out IntPtr keyContext, out IntPtr apcContext, long timeout)
        {
            return this.Remove(out isb, out keyContext, out apcContext, timeout, true);
        }

        public bool Remove(out IoStatusBlock isb, out IntPtr keyContext, out IntPtr apcContext, long timeout, bool relative)
        {
            long realTimeout = relative ? -timeout : timeout;

            return Win32.NtRemoveIoCompletion(this, out keyContext, out apcContext, out isb, ref realTimeout) != NtStatus.Timeout;
        }

        public void Set(IntPtr keyContext, IntPtr apcContext, NtStatus ioStatus, IntPtr ioInformation)
        {
            Win32.NtSetIoCompletion(
                this, 
                keyContext, 
                apcContext, 
                ioStatus, 
                ioInformation
                ).ThrowIf();
        }
    }
}
