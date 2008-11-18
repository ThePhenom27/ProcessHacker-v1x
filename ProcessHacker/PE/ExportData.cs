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
using System.IO;

namespace ProcessHacker.PE
{
    public class ExportEntry
    {
        public enum ExportType
        {
            Export,
            Forwarder
        }

        public ExportType Type;
        public uint ExportRVA;
        public string ForwardedString;
    }

    public class ExportData
    {
        public ExportData(BinaryReader br, uint imageBase)
        {                 
            this.ExportFlags = br.ReadUInt32();
            this.TimeDateStamp = br.ReadUInt32();
            this.MajorVersion = br.ReadUInt16();
            this.MinorVersion = br.ReadUInt16();
            this.NameRVA = br.ReadUInt32();
            this.OrdinalBase = br.ReadUInt32();
            this.AddressTableEntries = br.ReadUInt32();
            this.NumberOfNamePointers = br.ReadUInt32();
            this.ExportAddressTableRVA = br.ReadUInt32();
            this.NamePointerRVA = br.ReadUInt32();
            this.OrdinalTableRVA = br.ReadUInt32();

            // read address table
            br.BaseStream.Seek(imageBase + this.ExportAddressTableRVA, SeekOrigin.Begin);

            for (int i = 0; i < this.AddressTableEntries; i++)
            {
                uint address = br.ReadUInt32();

                this.ExportAddressTable.Add(new ExportEntry()
                {
                    ExportRVA = address
                });
            }

            // read ordinal table
            br.BaseStream.Seek(imageBase + this.OrdinalTableRVA, SeekOrigin.Begin);

            for (int i = 0; i < this.AddressTableEntries; i++)
            {
                this.ExportOrdinalTable.Add(br.ReadUInt16());
            }

            // read name pointer table
            br.BaseStream.Seek(imageBase + this.NamePointerRVA, SeekOrigin.Begin);

            for (int i = 0; i < this.NumberOfNamePointers; i++)
            {
                this.ExportNamePointerTable.Add(br.ReadUInt32());
            }

            // read names
            for (int i = 0; i < this.ExportNamePointerTable.Count; i++)
            {
                br.BaseStream.Seek(imageBase + this.ExportNamePointerTable[i], SeekOrigin.Begin);

                string temp = "";

                while (true)
                {
                    byte b = br.ReadByte();

                    if (b == 0)
                        break;

                    temp += (char)b;
                }

                this.ExportNameTable.Add(temp);
            }
        }

        public uint ExportFlags;
        public uint TimeDateStamp;
        public ushort MajorVersion;
        public ushort MinorVersion;
        public uint NameRVA;
        public uint OrdinalBase;
        public uint AddressTableEntries;
        public uint NumberOfNamePointers;
        public uint ExportAddressTableRVA;
        public uint NamePointerRVA;
        public uint OrdinalTableRVA;

        public List<ushort> ExportOrdinalTable = new List<ushort>();
        public List<ExportEntry> ExportAddressTable = new List<ExportEntry>();
        public List<uint> ExportNamePointerTable = new List<uint>();
        public List<string> ExportNameTable = new List<string>();
    }
}
