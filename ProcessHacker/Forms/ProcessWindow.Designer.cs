﻿namespace ProcessHacker
{
    partial class ProcessWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            Program.PWindows.Remove(_pid);
            Program.UpdateWindows();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessWindow));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.processMenuItem = new System.Windows.Forms.MenuItem();
            this.inspectImageFileMenuItem = new System.Windows.Forms.MenuItem();
            this.windowMenuItem = new System.Windows.Forms.MenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupProcess = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textDEP = new System.Windows.Forms.TextBox();
            this.labelDEP = new System.Windows.Forms.Label();
            this.buttonTerminate = new System.Windows.Forms.Button();
            this.buttonInspectPEB = new System.Windows.Forms.Button();
            this.buttonInspectParent = new System.Windows.Forms.Button();
            this.buttonEditDEP = new System.Windows.Forms.Button();
            this.buttonOpenCurDir = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textParent = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textPEBAddress = new System.Windows.Forms.TextBox();
            this.textCurrentDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textCmdLine = new System.Windows.Forms.TextBox();
            this.groupFile = new System.Windows.Forms.GroupBox();
            this.buttonOpenFileNameFolder = new System.Windows.Forms.Button();
            this.pictureIcon = new System.Windows.Forms.PictureBox();
            this.textFileDescription = new System.Windows.Forms.TextBox();
            this.textFileName = new System.Windows.Forms.TextBox();
            this.textFileCompany = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textFileVersion = new System.Windows.Forms.TextBox();
            this.tabPerformance = new System.Windows.Forms.TabPage();
            this.groupCPUUsage = new System.Windows.Forms.GroupBox();
            this.plotterCPUUsage = new ProcessHacker.Components.Plotter();
            this.tabThreads = new System.Windows.Forms.TabPage();
            this.listThreads = new ProcessHacker.ThreadList();
            this.tabToken = new System.Windows.Forms.TabPage();
            this.tabModules = new System.Windows.Forms.TabPage();
            this.listModules = new ProcessHacker.ModuleList();
            this.tabMemory = new System.Windows.Forms.TabPage();
            this.buttonSearch = new wyDay.Controls.SplitButton();
            this.menuSearch = new System.Windows.Forms.ContextMenu();
            this.newWindowSearchMenuItem = new System.Windows.Forms.MenuItem();
            this.literalSearchMenuItem = new System.Windows.Forms.MenuItem();
            this.regexSearchMenuItem = new System.Windows.Forms.MenuItem();
            this.stringScanMenuItem = new System.Windows.Forms.MenuItem();
            this.heapScanMenuItem = new System.Windows.Forms.MenuItem();
            this.checkHideFreeRegions = new System.Windows.Forms.CheckBox();
            this.listMemory = new ProcessHacker.MemoryList();
            this.tabHandles = new System.Windows.Forms.TabPage();
            this.checkHideHandlesNoName = new System.Windows.Forms.CheckBox();
            this.listHandles = new ProcessHacker.HandleList();
            this.tabServices = new System.Windows.Forms.TabPage();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.vistaMenu = new wyDay.Controls.VistaMenu(this.components);
            this.timerPerformance = new System.Windows.Forms.Timer(this.components);
            this.tabStatistics = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupProcess.SuspendLayout();
            this.groupFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).BeginInit();
            this.tabPerformance.SuspendLayout();
            this.groupCPUUsage.SuspendLayout();
            this.tabThreads.SuspendLayout();
            this.tabModules.SuspendLayout();
            this.tabMemory.SuspendLayout();
            this.tabHandles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.processMenuItem,
            this.windowMenuItem});
            // 
            // processMenuItem
            // 
            this.processMenuItem.Index = 0;
            this.processMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.inspectImageFileMenuItem});
            this.processMenuItem.Text = "&Process";
            // 
            // inspectImageFileMenuItem
            // 
            this.vistaMenu.SetImage(this.inspectImageFileMenuItem, ((System.Drawing.Image)(resources.GetObject("inspectImageFileMenuItem.Image"))));
            this.inspectImageFileMenuItem.Index = 0;
            this.inspectImageFileMenuItem.Text = "&Inspect Image File...";
            this.inspectImageFileMenuItem.Click += new System.EventHandler(this.inspectImageFileMenuItem_Click);
            // 
            // windowMenuItem
            // 
            this.windowMenuItem.Index = 1;
            this.windowMenuItem.Text = "&Window";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabStatistics);
            this.tabControl.Controls.Add(this.tabPerformance);
            this.tabControl.Controls.Add(this.tabThreads);
            this.tabControl.Controls.Add(this.tabToken);
            this.tabControl.Controls.Add(this.tabModules);
            this.tabControl.Controls.Add(this.tabMemory);
            this.tabControl.Controls.Add(this.tabHandles);
            this.tabControl.Controls.Add(this.tabServices);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageList;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(438, 376);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabGeneral
            // 
            this.tabGeneral.AutoScroll = true;
            this.tabGeneral.Controls.Add(this.groupProcess);
            this.tabGeneral.Controls.Add(this.groupFile);
            this.tabGeneral.ImageKey = "application";
            this.tabGeneral.Location = new System.Drawing.Point(4, 42);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(430, 330);
            this.tabGeneral.TabIndex = 2;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // groupProcess
            // 
            this.groupProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupProcess.Controls.Add(this.label7);
            this.groupProcess.Controls.Add(this.textDEP);
            this.groupProcess.Controls.Add(this.labelDEP);
            this.groupProcess.Controls.Add(this.buttonTerminate);
            this.groupProcess.Controls.Add(this.buttonInspectPEB);
            this.groupProcess.Controls.Add(this.buttonInspectParent);
            this.groupProcess.Controls.Add(this.buttonEditDEP);
            this.groupProcess.Controls.Add(this.buttonOpenCurDir);
            this.groupProcess.Controls.Add(this.label5);
            this.groupProcess.Controls.Add(this.textParent);
            this.groupProcess.Controls.Add(this.label4);
            this.groupProcess.Controls.Add(this.textPEBAddress);
            this.groupProcess.Controls.Add(this.textCurrentDirectory);
            this.groupProcess.Controls.Add(this.label2);
            this.groupProcess.Controls.Add(this.textCmdLine);
            this.groupProcess.Location = new System.Drawing.Point(8, 126);
            this.groupProcess.Name = "groupProcess";
            this.groupProcess.Size = new System.Drawing.Size(416, 198);
            this.groupProcess.TabIndex = 5;
            this.groupProcess.TabStop = false;
            this.groupProcess.Text = "Process";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "PEB Address:";
            // 
            // textDEP
            // 
            this.textDEP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textDEP.BackColor = System.Drawing.SystemColors.Control;
            this.textDEP.Location = new System.Drawing.Point(101, 156);
            this.textDEP.Name = "textDEP";
            this.textDEP.ReadOnly = true;
            this.textDEP.Size = new System.Drawing.Size(279, 20);
            this.textDEP.TabIndex = 7;
            // 
            // labelDEP
            // 
            this.labelDEP.AutoSize = true;
            this.labelDEP.Location = new System.Drawing.Point(6, 159);
            this.labelDEP.Name = "labelDEP";
            this.labelDEP.Size = new System.Drawing.Size(32, 13);
            this.labelDEP.TabIndex = 6;
            this.labelDEP.Text = "DEP:";
            // 
            // buttonTerminate
            // 
            this.buttonTerminate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTerminate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonTerminate.Location = new System.Drawing.Point(335, 19);
            this.buttonTerminate.Name = "buttonTerminate";
            this.buttonTerminate.Size = new System.Drawing.Size(75, 23);
            this.buttonTerminate.TabIndex = 5;
            this.buttonTerminate.Text = "Terminate";
            this.buttonTerminate.UseVisualStyleBackColor = true;
            this.buttonTerminate.Click += new System.EventHandler(this.buttonTerminate_Click);
            // 
            // buttonInspectPEB
            // 
            this.buttonInspectPEB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInspectPEB.Image = global::ProcessHacker.Properties.Resources.application_form_magnify;
            this.buttonInspectPEB.Location = new System.Drawing.Point(386, 101);
            this.buttonInspectPEB.Name = "buttonInspectPEB";
            this.buttonInspectPEB.Size = new System.Drawing.Size(24, 24);
            this.buttonInspectPEB.TabIndex = 4;
            this.buttonInspectPEB.UseVisualStyleBackColor = true;
            this.buttonInspectPEB.Click += new System.EventHandler(this.buttonInspectPEB_Click);
            // 
            // buttonInspectParent
            // 
            this.buttonInspectParent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInspectParent.Image = global::ProcessHacker.Properties.Resources.application_form_magnify;
            this.buttonInspectParent.Location = new System.Drawing.Point(386, 127);
            this.buttonInspectParent.Name = "buttonInspectParent";
            this.buttonInspectParent.Size = new System.Drawing.Size(24, 24);
            this.buttonInspectParent.TabIndex = 4;
            this.buttonInspectParent.UseVisualStyleBackColor = true;
            this.buttonInspectParent.Click += new System.EventHandler(this.buttonInspectParent_Click);
            // 
            // buttonEditDEP
            // 
            this.buttonEditDEP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEditDEP.Image = global::ProcessHacker.Properties.Resources.cog_edit;
            this.buttonEditDEP.Location = new System.Drawing.Point(386, 153);
            this.buttonEditDEP.Name = "buttonEditDEP";
            this.buttonEditDEP.Size = new System.Drawing.Size(24, 24);
            this.buttonEditDEP.TabIndex = 4;
            this.buttonEditDEP.UseVisualStyleBackColor = true;
            this.buttonEditDEP.Click += new System.EventHandler(this.buttonEditDEP_Click);
            // 
            // buttonOpenCurDir
            // 
            this.buttonOpenCurDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenCurDir.Image = global::ProcessHacker.Properties.Resources.folder_go;
            this.buttonOpenCurDir.Location = new System.Drawing.Point(386, 75);
            this.buttonOpenCurDir.Name = "buttonOpenCurDir";
            this.buttonOpenCurDir.Size = new System.Drawing.Size(24, 24);
            this.buttonOpenCurDir.TabIndex = 4;
            this.buttonOpenCurDir.UseVisualStyleBackColor = true;
            this.buttonOpenCurDir.Click += new System.EventHandler(this.buttonOpenCurDir_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Parent:";
            // 
            // textParent
            // 
            this.textParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textParent.BackColor = System.Drawing.SystemColors.Control;
            this.textParent.Location = new System.Drawing.Point(101, 130);
            this.textParent.Name = "textParent";
            this.textParent.ReadOnly = true;
            this.textParent.Size = new System.Drawing.Size(279, 20);
            this.textParent.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Current Directory:";
            // 
            // textPEBAddress
            // 
            this.textPEBAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textPEBAddress.Location = new System.Drawing.Point(101, 104);
            this.textPEBAddress.Name = "textPEBAddress";
            this.textPEBAddress.ReadOnly = true;
            this.textPEBAddress.Size = new System.Drawing.Size(279, 20);
            this.textPEBAddress.TabIndex = 3;
            // 
            // textCurrentDirectory
            // 
            this.textCurrentDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textCurrentDirectory.Location = new System.Drawing.Point(101, 78);
            this.textCurrentDirectory.Name = "textCurrentDirectory";
            this.textCurrentDirectory.ReadOnly = true;
            this.textCurrentDirectory.Size = new System.Drawing.Size(279, 20);
            this.textCurrentDirectory.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Command Line:";
            // 
            // textCmdLine
            // 
            this.textCmdLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textCmdLine.Location = new System.Drawing.Point(101, 52);
            this.textCmdLine.Name = "textCmdLine";
            this.textCmdLine.ReadOnly = true;
            this.textCmdLine.Size = new System.Drawing.Size(309, 20);
            this.textCmdLine.TabIndex = 3;
            // 
            // groupFile
            // 
            this.groupFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupFile.Controls.Add(this.buttonOpenFileNameFolder);
            this.groupFile.Controls.Add(this.pictureIcon);
            this.groupFile.Controls.Add(this.textFileDescription);
            this.groupFile.Controls.Add(this.textFileName);
            this.groupFile.Controls.Add(this.textFileCompany);
            this.groupFile.Controls.Add(this.label1);
            this.groupFile.Controls.Add(this.label3);
            this.groupFile.Controls.Add(this.textFileVersion);
            this.groupFile.Location = new System.Drawing.Point(6, 6);
            this.groupFile.Name = "groupFile";
            this.groupFile.Size = new System.Drawing.Size(418, 114);
            this.groupFile.TabIndex = 4;
            this.groupFile.TabStop = false;
            this.groupFile.Text = "File";
            // 
            // buttonOpenFileNameFolder
            // 
            this.buttonOpenFileNameFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFileNameFolder.Image = global::ProcessHacker.Properties.Resources.folder_go;
            this.buttonOpenFileNameFolder.Location = new System.Drawing.Point(388, 80);
            this.buttonOpenFileNameFolder.Name = "buttonOpenFileNameFolder";
            this.buttonOpenFileNameFolder.Size = new System.Drawing.Size(24, 24);
            this.buttonOpenFileNameFolder.TabIndex = 4;
            this.buttonOpenFileNameFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFileNameFolder.Click += new System.EventHandler(this.buttonOpenFileNameFolder_Click);
            // 
            // pictureIcon
            // 
            this.pictureIcon.Location = new System.Drawing.Point(6, 19);
            this.pictureIcon.Name = "pictureIcon";
            this.pictureIcon.Size = new System.Drawing.Size(32, 32);
            this.pictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureIcon.TabIndex = 1;
            this.pictureIcon.TabStop = false;
            // 
            // textFileDescription
            // 
            this.textFileDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textFileDescription.BackColor = System.Drawing.SystemColors.Window;
            this.textFileDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textFileDescription.Location = new System.Drawing.Point(44, 19);
            this.textFileDescription.Name = "textFileDescription";
            this.textFileDescription.ReadOnly = true;
            this.textFileDescription.Size = new System.Drawing.Size(368, 13);
            this.textFileDescription.TabIndex = 2;
            this.textFileDescription.Text = "File Description";
            // 
            // textFileName
            // 
            this.textFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textFileName.Location = new System.Drawing.Point(101, 83);
            this.textFileName.Name = "textFileName";
            this.textFileName.ReadOnly = true;
            this.textFileName.Size = new System.Drawing.Size(281, 20);
            this.textFileName.TabIndex = 3;
            // 
            // textFileCompany
            // 
            this.textFileCompany.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textFileCompany.BackColor = System.Drawing.SystemColors.Window;
            this.textFileCompany.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textFileCompany.Location = new System.Drawing.Point(44, 38);
            this.textFileCompany.Name = "textFileCompany";
            this.textFileCompany.ReadOnly = true;
            this.textFileCompany.Size = new System.Drawing.Size(368, 13);
            this.textFileCompany.TabIndex = 2;
            this.textFileCompany.Text = "File Company";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image Version:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Image File Name:";
            // 
            // textFileVersion
            // 
            this.textFileVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textFileVersion.Location = new System.Drawing.Point(101, 57);
            this.textFileVersion.Name = "textFileVersion";
            this.textFileVersion.ReadOnly = true;
            this.textFileVersion.Size = new System.Drawing.Size(311, 20);
            this.textFileVersion.TabIndex = 2;
            // 
            // tabPerformance
            // 
            this.tabPerformance.Controls.Add(this.groupCPUUsage);
            this.tabPerformance.ImageKey = "chart_pie";
            this.tabPerformance.Location = new System.Drawing.Point(4, 23);
            this.tabPerformance.Name = "tabPerformance";
            this.tabPerformance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPerformance.Size = new System.Drawing.Size(651, 349);
            this.tabPerformance.TabIndex = 8;
            this.tabPerformance.Text = "Performance";
            this.tabPerformance.UseVisualStyleBackColor = true;
            // 
            // groupCPUUsage
            // 
            this.groupCPUUsage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCPUUsage.Controls.Add(this.plotterCPUUsage);
            this.groupCPUUsage.Location = new System.Drawing.Point(6, 6);
            this.groupCPUUsage.Name = "groupCPUUsage";
            this.groupCPUUsage.Size = new System.Drawing.Size(639, 107);
            this.groupCPUUsage.TabIndex = 0;
            this.groupCPUUsage.TabStop = false;
            this.groupCPUUsage.Text = "CPU Usage";
            // 
            // plotterCPUUsage
            // 
            this.plotterCPUUsage.BackColor = System.Drawing.Color.Black;
            this.plotterCPUUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotterCPUUsage.GridColor = System.Drawing.Color.Green;
            this.plotterCPUUsage.GridSize = new System.Drawing.Size(12, 12);
            this.plotterCPUUsage.IsMoved = true;
            this.plotterCPUUsage.LineColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.plotterCPUUsage.LineColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.plotterCPUUsage.Location = new System.Drawing.Point(3, 16);
            this.plotterCPUUsage.MoveStep = 3;
            this.plotterCPUUsage.Name = "plotterCPUUsage";
            this.plotterCPUUsage.Size = new System.Drawing.Size(633, 88);
            this.plotterCPUUsage.TabIndex = 0;
            this.plotterCPUUsage.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.plotterCPUUsage.UseSecondLine = true;
            // 
            // tabThreads
            // 
            this.tabThreads.Controls.Add(this.listThreads);
            this.tabThreads.ImageKey = "hourglass";
            this.tabThreads.Location = new System.Drawing.Point(4, 23);
            this.tabThreads.Name = "tabThreads";
            this.tabThreads.Size = new System.Drawing.Size(651, 349);
            this.tabThreads.TabIndex = 3;
            this.tabThreads.Text = "Threads";
            this.tabThreads.UseVisualStyleBackColor = true;
            // 
            // listThreads
            // 
            this.listThreads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listThreads.DoubleBuffered = true;
            this.listThreads.Highlight = false;
            this.listThreads.Location = new System.Drawing.Point(0, 0);
            this.listThreads.Name = "listThreads";
            this.listThreads.Provider = null;
            this.listThreads.Size = new System.Drawing.Size(651, 349);
            this.listThreads.TabIndex = 0;
            // 
            // tabToken
            // 
            this.tabToken.ImageKey = "token";
            this.tabToken.Location = new System.Drawing.Point(4, 23);
            this.tabToken.Name = "tabToken";
            this.tabToken.Padding = new System.Windows.Forms.Padding(3);
            this.tabToken.Size = new System.Drawing.Size(651, 349);
            this.tabToken.TabIndex = 1;
            this.tabToken.Text = "Token";
            this.tabToken.UseVisualStyleBackColor = true;
            // 
            // tabModules
            // 
            this.tabModules.Controls.Add(this.listModules);
            this.tabModules.ImageKey = "page_white_wrench";
            this.tabModules.Location = new System.Drawing.Point(4, 23);
            this.tabModules.Name = "tabModules";
            this.tabModules.Size = new System.Drawing.Size(651, 349);
            this.tabModules.TabIndex = 6;
            this.tabModules.Text = "Modules";
            this.tabModules.UseVisualStyleBackColor = true;
            // 
            // listModules
            // 
            this.listModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listModules.DoubleBuffered = true;
            this.listModules.Highlight = false;
            this.listModules.Location = new System.Drawing.Point(0, 0);
            this.listModules.Name = "listModules";
            this.listModules.Provider = null;
            this.listModules.Size = new System.Drawing.Size(651, 349);
            this.listModules.TabIndex = 0;
            // 
            // tabMemory
            // 
            this.tabMemory.Controls.Add(this.buttonSearch);
            this.tabMemory.Controls.Add(this.checkHideFreeRegions);
            this.tabMemory.Controls.Add(this.listMemory);
            this.tabMemory.ImageKey = "database";
            this.tabMemory.Location = new System.Drawing.Point(4, 23);
            this.tabMemory.Name = "tabMemory";
            this.tabMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tabMemory.Size = new System.Drawing.Size(651, 349);
            this.tabMemory.TabIndex = 4;
            this.tabMemory.Text = "Memory";
            this.tabMemory.UseVisualStyleBackColor = true;
            // 
            // buttonSearch
            // 
            this.buttonSearch.AutoSize = true;
            this.buttonSearch.Location = new System.Drawing.Point(6, 6);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(99, 23);
            this.buttonSearch.SplitMenu = this.menuSearch;
            this.buttonSearch.TabIndex = 9;
            this.buttonSearch.Text = "&String Scan...";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // menuSearch
            // 
            this.menuSearch.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newWindowSearchMenuItem,
            this.literalSearchMenuItem,
            this.regexSearchMenuItem,
            this.stringScanMenuItem,
            this.heapScanMenuItem});
            // 
            // newWindowSearchMenuItem
            // 
            this.newWindowSearchMenuItem.Index = 0;
            this.newWindowSearchMenuItem.Text = "&New Window...";
            this.newWindowSearchMenuItem.Click += new System.EventHandler(this.newWindowSearchMenuItem_Click);
            // 
            // literalSearchMenuItem
            // 
            this.literalSearchMenuItem.Index = 1;
            this.literalSearchMenuItem.Text = "&Literal...";
            this.literalSearchMenuItem.Click += new System.EventHandler(this.literalSearchMenuItem_Click);
            // 
            // regexSearchMenuItem
            // 
            this.regexSearchMenuItem.Index = 2;
            this.regexSearchMenuItem.Text = "&Regex...";
            this.regexSearchMenuItem.Click += new System.EventHandler(this.regexSearchMenuItem_Click);
            // 
            // stringScanMenuItem
            // 
            this.stringScanMenuItem.Index = 3;
            this.stringScanMenuItem.Text = "&String Scan...";
            this.stringScanMenuItem.Click += new System.EventHandler(this.stringScanMenuItem_Click);
            // 
            // heapScanMenuItem
            // 
            this.heapScanMenuItem.Index = 4;
            this.heapScanMenuItem.Text = "&Heap Scan...";
            this.heapScanMenuItem.Click += new System.EventHandler(this.heapScanMenuItem_Click);
            // 
            // checkHideFreeRegions
            // 
            this.checkHideFreeRegions.AutoSize = true;
            this.checkHideFreeRegions.Checked = true;
            this.checkHideFreeRegions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkHideFreeRegions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkHideFreeRegions.Location = new System.Drawing.Point(6, 35);
            this.checkHideFreeRegions.Name = "checkHideFreeRegions";
            this.checkHideFreeRegions.Size = new System.Drawing.Size(120, 18);
            this.checkHideFreeRegions.TabIndex = 1;
            this.checkHideFreeRegions.Text = "Hide Free Regions";
            this.checkHideFreeRegions.UseVisualStyleBackColor = true;
            this.checkHideFreeRegions.CheckedChanged += new System.EventHandler(this.checkHideFreeRegions_CheckedChanged);
            // 
            // listMemory
            // 
            this.listMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listMemory.DoubleBuffered = true;
            this.listMemory.Highlight = false;
            this.listMemory.Location = new System.Drawing.Point(6, 59);
            this.listMemory.Name = "listMemory";
            this.listMemory.Provider = null;
            this.listMemory.Size = new System.Drawing.Size(639, 284);
            this.listMemory.TabIndex = 0;
            // 
            // tabHandles
            // 
            this.tabHandles.Controls.Add(this.checkHideHandlesNoName);
            this.tabHandles.Controls.Add(this.listHandles);
            this.tabHandles.ImageKey = "connect";
            this.tabHandles.Location = new System.Drawing.Point(4, 23);
            this.tabHandles.Name = "tabHandles";
            this.tabHandles.Padding = new System.Windows.Forms.Padding(3);
            this.tabHandles.Size = new System.Drawing.Size(651, 349);
            this.tabHandles.TabIndex = 5;
            this.tabHandles.Text = "Handles";
            this.tabHandles.UseVisualStyleBackColor = true;
            // 
            // checkHideHandlesNoName
            // 
            this.checkHideHandlesNoName.AutoSize = true;
            this.checkHideHandlesNoName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkHideHandlesNoName.Location = new System.Drawing.Point(6, 6);
            this.checkHideHandlesNoName.Name = "checkHideHandlesNoName";
            this.checkHideHandlesNoName.Size = new System.Drawing.Size(160, 18);
            this.checkHideHandlesNoName.TabIndex = 9;
            this.checkHideHandlesNoName.Text = "Hide handles with no name";
            this.checkHideHandlesNoName.UseVisualStyleBackColor = true;
            this.checkHideHandlesNoName.CheckedChanged += new System.EventHandler(this.checkHideHandlesNoName_CheckedChanged);
            // 
            // listHandles
            // 
            this.listHandles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listHandles.DoubleBuffered = true;
            this.listHandles.Highlight = false;
            this.listHandles.Location = new System.Drawing.Point(6, 30);
            this.listHandles.Name = "listHandles";
            this.listHandles.Provider = null;
            this.listHandles.Size = new System.Drawing.Size(639, 313);
            this.listHandles.TabIndex = 0;
            // 
            // tabServices
            // 
            this.tabServices.ImageKey = "cog";
            this.tabServices.Location = new System.Drawing.Point(4, 23);
            this.tabServices.Name = "tabServices";
            this.tabServices.Size = new System.Drawing.Size(651, 349);
            this.tabServices.TabIndex = 7;
            this.tabServices.Text = "Services";
            this.tabServices.UseVisualStyleBackColor = true;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "token");
            this.imageList.Images.SetKeyName(1, "application");
            this.imageList.Images.SetKeyName(2, "cog");
            this.imageList.Images.SetKeyName(3, "page_white_wrench");
            this.imageList.Images.SetKeyName(4, "database");
            this.imageList.Images.SetKeyName(5, "connect");
            this.imageList.Images.SetKeyName(6, "hourglass");
            this.imageList.Images.SetKeyName(7, "chart_pie");
            this.imageList.Images.SetKeyName(8, "chart_bar");
            // 
            // vistaMenu
            // 
            this.vistaMenu.ContainerControl = this;
            // 
            // timerPerformance
            // 
            this.timerPerformance.Interval = 1000;
            // 
            // tabStatistics
            // 
            this.tabStatistics.ImageKey = "chart_bar";
            this.tabStatistics.Location = new System.Drawing.Point(4, 42);
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.Padding = new System.Windows.Forms.Padding(3);
            this.tabStatistics.Size = new System.Drawing.Size(430, 330);
            this.tabStatistics.TabIndex = 9;
            this.tabStatistics.Text = "Statistics";
            this.tabStatistics.UseVisualStyleBackColor = true;
            // 
            // ProcessWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 376);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Menu = this.mainMenu;
            this.Name = "ProcessWindow";
            this.Text = "Process";
            this.Load += new System.EventHandler(this.ProcessWindow_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProcessWindow_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.groupProcess.ResumeLayout(false);
            this.groupProcess.PerformLayout();
            this.groupFile.ResumeLayout(false);
            this.groupFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).EndInit();
            this.tabPerformance.ResumeLayout(false);
            this.groupCPUUsage.ResumeLayout(false);
            this.tabThreads.ResumeLayout(false);
            this.tabModules.ResumeLayout(false);
            this.tabMemory.ResumeLayout(false);
            this.tabMemory.PerformLayout();
            this.tabHandles.ResumeLayout(false);
            this.tabHandles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private wyDay.Controls.VistaMenu vistaMenu;
        private System.Windows.Forms.MenuItem processMenuItem;
        private System.Windows.Forms.MenuItem windowMenuItem;
        private System.Windows.Forms.MenuItem inspectImageFileMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabToken;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabThreads;
        private ThreadList listThreads;
        private System.Windows.Forms.TabPage tabModules;
        private System.Windows.Forms.TabPage tabMemory;
        private System.Windows.Forms.TabPage tabHandles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureIcon;
        private System.Windows.Forms.TextBox textFileDescription;
        private System.Windows.Forms.TextBox textFileVersion;
        private System.Windows.Forms.TextBox textFileCompany;
        private System.Windows.Forms.TextBox textCmdLine;
        private System.Windows.Forms.TextBox textFileName;
        private System.Windows.Forms.GroupBox groupFile;
        private System.Windows.Forms.GroupBox groupProcess;
        private System.Windows.Forms.TabPage tabServices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textCurrentDirectory;
        private System.Windows.Forms.Button buttonOpenFileNameFolder;
        private System.Windows.Forms.Button buttonOpenCurDir;
        private ModuleList listModules;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textParent;
        private HandleList listHandles;
        private MemoryList listMemory;
        private System.Windows.Forms.Button buttonTerminate;
        private System.Windows.Forms.TextBox textDEP;
        private System.Windows.Forms.Label labelDEP;
        private System.Windows.Forms.Button buttonEditDEP;
        private System.Windows.Forms.Button buttonInspectParent;
        private System.Windows.Forms.ContextMenu menuSearch;
        private System.Windows.Forms.MenuItem newWindowSearchMenuItem;
        private System.Windows.Forms.MenuItem literalSearchMenuItem;
        private System.Windows.Forms.MenuItem regexSearchMenuItem;
        private System.Windows.Forms.MenuItem stringScanMenuItem;
        private System.Windows.Forms.MenuItem heapScanMenuItem;
        private System.Windows.Forms.CheckBox checkHideFreeRegions;
        private System.Windows.Forms.CheckBox checkHideHandlesNoName;
        private wyDay.Controls.SplitButton buttonSearch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textPEBAddress;
        private System.Windows.Forms.Button buttonInspectPEB;
        private System.Windows.Forms.TabPage tabPerformance;
        private System.Windows.Forms.GroupBox groupCPUUsage;
        private ProcessHacker.Components.Plotter plotterCPUUsage;
        private System.Windows.Forms.Timer timerPerformance;
        private System.Windows.Forms.TabPage tabStatistics;
    }
}