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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ProcessHacker
{
    public partial class VirtualProtectWindow : Form
    {
        private int _pid, _address, _size;

        public VirtualProtectWindow(int pid, int address, int size)
        {
            InitializeComponent();

            _pid = pid;
            _address = address;
            _size = size;
        }

        private void textNewProtection_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = buttonVirtualProtect;
        }

        private void buttonCloseVirtualProtect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonVirtualProtect_Click(object sender, EventArgs e)
        {
            try
            {
                int old = 0;
                int newprotect;

                try
                {
                    newprotect = (int)BaseConverter.ToNumberParse(textNewProtection.Text);
                }
                catch
                {
                    return;
                }

                using (Win32.ProcessHandle phandle =
                    new Win32.ProcessHandle(_pid, Win32.PROCESS_RIGHTS.PROCESS_VM_OPERATION))
                {
                    if (!Win32.VirtualProtectEx(phandle, _address,
                        _size, newprotect, out old))
                    {
                        MessageBox.Show("There was an error setting memory protection:\n\n" +
                            Win32.GetLastErrorMessage(), "Process Hacker",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting memory protection:\n\n" + ex.Message, "Process Hacker",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textNewProtection_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }
    }
}