﻿/*
 * Process Hacker - 
 *   operating system version information
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
using ProcessHacker.Native.Security;

namespace ProcessHacker.Native
{
    public enum OSArch
    {
        Unknown,
        I386,
        Amd64
    }

    public enum WindowsVersion
    {
        /// <summary>
        /// Windows 2000.
        /// </summary>
        TwoThousand = 50,

        /// <summary>
        /// Windows XP.
        /// </summary>
        XP = 51,

        /// <summary>
        /// Windows Server 2003.
        /// </summary>
        Server2003 = 52,

        /// <summary>
        /// Windows Vista, Windows Server 2008.
        /// </summary>
        Vista = 60,

        /// <summary>
        /// Windows 7, Windows Server 2008 R2.
        /// </summary>
        Seven = 61,

        /// <summary>
        /// Windows 8.
        /// </summary>
        Eight = 62,

        /// <summary>
        /// An unknown version of Windows.
        /// </summary>
        Unknown = int.MinValue
    }

    public static class OSVersion
    {
        private static readonly int _bits = IntPtr.Size * 8;
        private static readonly OSArch _arch = IntPtr.Size == 4 ? OSArch.I386 : OSArch.Amd64;
        private static readonly string _versionString;
        private static readonly WindowsVersion _windowsVersion;

        private static readonly ProcessAccess _minProcessQueryInfoAccess = ProcessAccess.QueryInformation;
        private static readonly ThreadAccess _minThreadQueryInfoAccess = ThreadAccess.QueryInformation;
        private static readonly ThreadAccess _minThreadSetInfoAccess = ThreadAccess.SetInformation;

        private static readonly bool _hasCycleTime;
        private static readonly bool _hasExtendedTaskbar;
        private static readonly bool _hasIoPriority;
        private static readonly bool _hasPagePriority;
        private static readonly bool _hasProtectedProcesses;
        private static readonly bool _hasPsSuspendResumeProcess;
        private static readonly bool _hasQueryLimitedInformation;
        private static readonly bool _hasSetAccessToken;
        private static readonly bool _hasTaskDialogs;
        private static readonly bool _hasThemes;
        private static readonly bool _hasUac;
        private static readonly bool _hasWin32ImageFileName;

        static OSVersion()
        {
            Version version = Environment.OSVersion.Version;

            if (version.Major == 5 && version.Minor == 0)
                _windowsVersion = WindowsVersion.TwoThousand;
            else if (version.Major == 5 && version.Minor == 1)
                _windowsVersion = WindowsVersion.XP;
            else if (version.Major == 5 && version.Minor == 2)
                _windowsVersion = WindowsVersion.Server2003;
            else if (version.Major == 6 && version.Minor == 0)
                _windowsVersion = WindowsVersion.Vista;
            else if (version.Major == 6 && version.Minor == 1)
                _windowsVersion = WindowsVersion.Seven;  
            else if (version.Major == 6 && version.Minor == 2)
                _windowsVersion = WindowsVersion.Eight;
            else
                _windowsVersion = WindowsVersion.Unknown;

            if (_windowsVersion != WindowsVersion.Unknown)
            {
                if (IsAboveOrEqual(WindowsVersion.XP))
                {
                    _hasThemes = true;
                }

                if (IsBelow(WindowsVersion.Vista))
                {
                    _hasSetAccessToken = true;
                }

                if (IsAboveOrEqual(WindowsVersion.Vista))
                {
                    _minProcessQueryInfoAccess = ProcessAccess.QueryLimitedInformation;
                    _minThreadQueryInfoAccess = ThreadAccess.QueryLimitedInformation;
                    _minThreadSetInfoAccess = ThreadAccess.SetLimitedInformation;

                    _hasCycleTime = true;
                    _hasIoPriority = true;
                    _hasPagePriority = true;
                    _hasProtectedProcesses = true;
                    _hasPsSuspendResumeProcess = true;
                    _hasQueryLimitedInformation = true;
                    _hasTaskDialogs = true;
                    _hasUac = true;
                    _hasWin32ImageFileName = true;
                }

                if (IsAboveOrEqual(WindowsVersion.Seven))
                {
                    _hasExtendedTaskbar = true;
                }
            }

            _versionString = Environment.OSVersion.VersionString;
        }

        public static int Bits
        {
            get { return _bits; }
        }

        public static string BitsString
        {
            get { return _bits.ToString() + "-" + "bit"; }
        }

        public static OSArch Architecture
        {
            get { return _arch; }
        }

        public static string VersionString
        {
            get { return _versionString; }
        }

        public static WindowsVersion WindowsVersion
        {
            get { return _windowsVersion; }
        }

        public static ProcessAccess MinProcessQueryInfoAccess
        {
            get { return _minProcessQueryInfoAccess; }
        }

        public static ThreadAccess MinThreadQueryInfoAccess
        {
            get { return _minThreadQueryInfoAccess; }
        }

        public static ThreadAccess MinThreadSetInfoAccess
        {
            get { return _minThreadSetInfoAccess; }
        }

        public static bool HasCycleTime
        {
            get { return _hasCycleTime; }
        }

        public static bool HasExtendedTaskbar
        {
            get { return _hasExtendedTaskbar; }
        }

        public static bool HasIoPriority
        {
            get { return _hasIoPriority; }
        }

        public static bool HasPagePriority
        {
            get { return _hasPagePriority; }
        }

        public static bool HasProtectedProcesses
        {
            get { return _hasProtectedProcesses; }
        }

        public static bool HasPsSuspendResumeProcess
        {
            get { return _hasPsSuspendResumeProcess; }
        }

        public static bool HasQueryLimitedInformation
        {
            get { return _hasQueryLimitedInformation; }
        }

        public static bool HasSetAccessToken
        {
            get { return _hasSetAccessToken; }
        }

        public static bool HasTaskDialogs
        {
            get { return _hasTaskDialogs; }
        }

        public static bool HasThemes
        {
            get { return _hasThemes; }
        }

        public static bool HasUac
        {
            get { return _hasUac; }
        }

        public static bool HasWin32ImageFileName
        {
            get { return _hasWin32ImageFileName; }
        }

        public static bool IsAmd64()
        {
            return _arch == OSArch.Amd64;
        }

        public static bool IsI386()
        {
            return _arch == OSArch.I386;
        }

        public static bool IsAbove(WindowsVersion version)
        {
            return _windowsVersion > version;
        }

        public static bool IsAboveOrEqual(WindowsVersion version)
        {
            return _windowsVersion >= version;
        }

        public static bool IsBelowOrEqual(WindowsVersion version)
        {
            return _windowsVersion <= version;
        }

        public static bool IsBelow(WindowsVersion version)
        {
            return _windowsVersion < version;
        }

        public static bool IsEqualTo(WindowsVersion version)
        {
            return _windowsVersion == version;
        }
    }
}
