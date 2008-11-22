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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ProcessHacker.PE;
using System.IO;

namespace ProcessHacker
{
    public class Symbols
    {
        private static List<KeyValuePair<int, string>> _libraryLookup;
        private static Dictionary<string, List<KeyValuePair<int, string>>> _symbols;

        static Symbols()
        {
            _libraryLookup = new List<KeyValuePair<int, string>>();
            _symbols = new Dictionary<string, List<KeyValuePair<int, string>>>();
        }

        public static void LoadLibrary(string path)
        {
            string realPath = Misc.GetRealPath(path).ToLower();

            int moduleHandle = Win32.LoadLibrary(realPath);

            ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
            int imageBase = -1;

            foreach (ProcessModule module in modules)
            {
                string thisPath = Misc.GetRealPath(module.FileName).ToLower();

                if (thisPath == realPath)
                {
                    imageBase = module.BaseAddress.ToInt32();
                    break;
                }
            }

            if (imageBase == -1)
                throw new Exception("Could not get image base of library.");

            PEFile file = new PEFile(realPath);
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

            for (int i = 0; i < file.ExportData.ExportNameTable.Count; i++)
            {
                string name = file.ExportData.ExportNameTable[i];
                list.Add(new KeyValuePair<int, string>(Win32.GetProcAddress(moduleHandle, name), name));
            }

            // sort the list
            list.Sort(new Comparison<KeyValuePair<int, string>>(
                    delegate(KeyValuePair<int, string> kvp1, KeyValuePair<int, string> kvp2)
                    {
                        return kvp2.Key.CompareTo(kvp1.Key);
                    })); 

            _libraryLookup.Add(new KeyValuePair<int, string>(imageBase, realPath));
            _symbols.Add(realPath, list);

            _libraryLookup.Sort(new Comparison<KeyValuePair<int, string>>(
                    delegate(KeyValuePair<int, string> kvp1, KeyValuePair<int, string> kvp2)
                    {
                        return kvp2.Key.CompareTo(kvp1.Key);
                    }));
        }

        public static string GetSymbolName(int address)
        {
            foreach (KeyValuePair<int, string> kvp in _libraryLookup)
            {
                if (address >= kvp.Key)
                {
                    List<KeyValuePair<int, string>> symbolList = _symbols[kvp.Value];

                    foreach (KeyValuePair<int, string> kvps in symbolList)
                    {
                        if (address >= kvps.Key)
                        {
                            FileInfo fi = new FileInfo(kvp.Value);

                            return string.Format("{0}!{1}+0x{2:x}",
                                fi.Name, kvps.Value, address - kvps.Key);
                        }
                    }
                }
            }

            return "0x" + address.ToString("x8");
        }
    }
}
