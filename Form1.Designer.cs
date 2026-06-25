
namespace SoundModBuilder
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonFiles = new System.Windows.Forms.Button();
            this.buttonLog = new System.Windows.Forms.Button();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonGetDirectory = new System.Windows.Forms.Button();
            this.buttonHome = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.listBoxEvents = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.PreferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProjectOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPilotsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSilenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CleanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openSourceDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenResultDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConvertWemToWaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConvertWemToOggToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.DelWavToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelOggToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonFiles);
            this.splitContainer1.Panel1.Controls.Add(this.buttonLog);
            this.splitContainer1.Panel1.Controls.Add(this.listBoxLog);
            this.splitContainer1.Panel1.Controls.Add(this.listBoxFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.buttonGetDirectory);
            this.splitContainer1.Panel2.Controls.Add(this.buttonHome);
            this.splitContainer1.Panel2.Controls.Add(this.buttonBack);
            this.splitContainer1.Panel2.Controls.Add(this.buttonForward);
            this.splitContainer1.Panel2.Controls.Add(this.buttonUp);
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitContainer1_Paint);
            // 
            // buttonFiles
            // 
            this.buttonFiles.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.buttonFiles, "buttonFiles");
            this.buttonFiles.Name = "buttonFiles";
            this.buttonFiles.UseVisualStyleBackColor = true;
            this.buttonFiles.Click += new System.EventHandler(this.ButtonFiles_Click);
            // 
            // buttonLog
            // 
            this.buttonLog.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.buttonLog, "buttonLog");
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.ButtonLog_Click);
            // 
            // listBoxLog
            // 
            this.listBoxLog.AllowDrop = true;
            resources.ApplyResources(this.listBoxLog, "listBoxLog");
            this.listBoxLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxLog.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBoxLog_DrawItem);
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.AllowDrop = true;
            resources.ApplyResources(this.listBoxFiles, "listBoxFiles");
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListBoxFiles_DragDrop);
            this.listBoxFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListBoxFiles_DragDropEnter);
            this.listBoxFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListBoxFiles_KeyDown);
            this.listBoxFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBoxFiles_MouseDoubleClick);
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // buttonGetDirectory
            // 
            resources.ApplyResources(this.buttonGetDirectory, "buttonGetDirectory");
            this.buttonGetDirectory.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.buttonGetDirectory.Name = "buttonGetDirectory";
            this.toolTip1.SetToolTip(this.buttonGetDirectory, resources.GetString("buttonGetDirectory.ToolTip"));
            this.buttonGetDirectory.UseVisualStyleBackColor = true;
            this.buttonGetDirectory.Click += new System.EventHandler(this.ButtonGetDirectory_Click);
            // 
            // buttonHome
            // 
            this.buttonHome.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.buttonHome, "buttonHome");
            this.buttonHome.Name = "buttonHome";
            this.toolTip1.SetToolTip(this.buttonHome, resources.GetString("buttonHome.ToolTip"));
            this.buttonHome.UseVisualStyleBackColor = true;
            this.buttonHome.Click += new System.EventHandler(this.ButtonHome_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.buttonBack, "buttonBack");
            this.buttonBack.Name = "buttonBack";
            this.toolTip1.SetToolTip(this.buttonBack, resources.GetString("buttonBack.ToolTip"));
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // buttonForward
            // 
            this.buttonForward.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.buttonForward, "buttonForward");
            this.buttonForward.Name = "buttonForward";
            this.toolTip1.SetToolTip(this.buttonForward, resources.GetString("buttonForward.ToolTip"));
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.ButtonForward_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.FlatAppearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.buttonUp, "buttonUp");
            this.buttonUp.Name = "buttonUp";
            this.toolTip1.SetToolTip(this.buttonUp, resources.GetString("buttonUp.ToolTip"));
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.ButtonUp_Click);
            // 
            // webBrowser1
            // 
            resources.ApplyResources(this.webBrowser1, "webBrowser1");
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.WebBrowser1_Navigated);
            // 
            // listBoxEvents
            // 
            resources.ApplyResources(this.listBoxEvents, "listBoxEvents");
            this.listBoxEvents.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBoxEvents.FormattingEnabled = true;
            this.listBoxEvents.Name = "listBoxEvents";
            this.listBoxEvents.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBoxEvents_DrawItem);
            this.listBoxEvents.SelectedIndexChanged += new System.EventHandler(this.ListBoxEvents_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.ProjectToolStripMenuItem,
            this.RunConverterToolStripMenuItem,
            this.HelpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.ImportToolStripMenuItem,
            this.toolStripSeparator4,
            this.PreferencesToolStripMenuItem,
            this.toolStripSeparator3,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            resources.ApplyResources(this.FileToolStripMenuItem, "FileToolStripMenuItem");
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            resources.ApplyResources(this.OpenToolStripMenuItem, "OpenToolStripMenuItem");
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            resources.ApplyResources(this.SaveToolStripMenuItem, "SaveToolStripMenuItem");
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            resources.ApplyResources(this.SaveAsToolStripMenuItem, "SaveAsToolStripMenuItem");
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // ImportToolStripMenuItem
            // 
            this.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            resources.ApplyResources(this.ImportToolStripMenuItem, "ImportToolStripMenuItem");
            this.ImportToolStripMenuItem.Click += new System.EventHandler(this.Import_ToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // PreferencesToolStripMenuItem
            // 
            this.PreferencesToolStripMenuItem.Name = "PreferencesToolStripMenuItem";
            resources.ApplyResources(this.PreferencesToolStripMenuItem, "PreferencesToolStripMenuItem");
            this.PreferencesToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            resources.ApplyResources(this.ExitToolStripMenuItem, "ExitToolStripMenuItem");
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitCtrlXToolStripMenuItem_Click);
            // 
            // ProjectToolStripMenuItem
            // 
            this.ProjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BuildToolStripMenuItem,
            this.ProjectOptionsToolStripMenuItem,
            this.CheckToolStripMenuItem,
            this.toolStripSeparator2,
            this.CopyMessagesToolStripMenuItem,
            this.copyPilotsToolStripMenuItem,
            this.addSilenceToolStripMenuItem,
            this.CleanToolStripMenuItem,
            this.toolStripSeparator1,
            this.openSourceDirToolStripMenuItem,
            this.OpenResultDirToolStripMenuItem});
            this.ProjectToolStripMenuItem.Name = "ProjectToolStripMenuItem";
            resources.ApplyResources(this.ProjectToolStripMenuItem, "ProjectToolStripMenuItem");
            // 
            // BuildToolStripMenuItem
            // 
            this.BuildToolStripMenuItem.Name = "BuildToolStripMenuItem";
            resources.ApplyResources(this.BuildToolStripMenuItem, "BuildToolStripMenuItem");
            this.BuildToolStripMenuItem.Click += new System.EventHandler(this.BuildToolStripMenuItem_Click);
            // 
            // ProjectOptionsToolStripMenuItem
            // 
            this.ProjectOptionsToolStripMenuItem.Name = "ProjectOptionsToolStripMenuItem";
            resources.ApplyResources(this.ProjectOptionsToolStripMenuItem, "ProjectOptionsToolStripMenuItem");
            this.ProjectOptionsToolStripMenuItem.Click += new System.EventHandler(this.ProjectOptionsToolStripMenuItem_Click);
            // 
            // CheckToolStripMenuItem
            // 
            this.CheckToolStripMenuItem.Name = "CheckToolStripMenuItem";
            resources.ApplyResources(this.CheckToolStripMenuItem, "CheckToolStripMenuItem");
            this.CheckToolStripMenuItem.Click += new System.EventHandler(this.CheckToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // CopyMessagesToolStripMenuItem
            // 
            this.CopyMessagesToolStripMenuItem.Name = "CopyMessagesToolStripMenuItem";
            resources.ApplyResources(this.CopyMessagesToolStripMenuItem, "CopyMessagesToolStripMenuItem");
            this.CopyMessagesToolStripMenuItem.Click += new System.EventHandler(this.CopyMessagesToolStripMenuItem_Click);
            // 
            // copyPilotsToolStripMenuItem
            // 
            this.copyPilotsToolStripMenuItem.Name = "copyPilotsToolStripMenuItem";
            resources.ApplyResources(this.copyPilotsToolStripMenuItem, "copyPilotsToolStripMenuItem");
            this.copyPilotsToolStripMenuItem.Click += new System.EventHandler(this.CopyPilotsToolStripMenuItem_Click);
            // 
            // addSilenceToolStripMenuItem
            // 
            this.addSilenceToolStripMenuItem.Name = "addSilenceToolStripMenuItem";
            resources.ApplyResources(this.addSilenceToolStripMenuItem, "addSilenceToolStripMenuItem");
            this.addSilenceToolStripMenuItem.Click += new System.EventHandler(this.AddSilenceToolStripMenuItem_Click);
            // 
            // CleanToolStripMenuItem
            // 
            this.CleanToolStripMenuItem.Name = "CleanToolStripMenuItem";
            resources.ApplyResources(this.CleanToolStripMenuItem, "CleanToolStripMenuItem");
            this.CleanToolStripMenuItem.Click += new System.EventHandler(this.Clean_ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // openSourceDirToolStripMenuItem
            // 
            this.openSourceDirToolStripMenuItem.Name = "openSourceDirToolStripMenuItem";
            resources.ApplyResources(this.openSourceDirToolStripMenuItem, "openSourceDirToolStripMenuItem");
            this.openSourceDirToolStripMenuItem.Click += new System.EventHandler(this.OpenSourceDirToolStripMenuItem_Click);
            // 
            // OpenResultDirToolStripMenuItem
            // 
            this.OpenResultDirToolStripMenuItem.Name = "OpenResultDirToolStripMenuItem";
            resources.ApplyResources(this.OpenResultDirToolStripMenuItem, "OpenResultDirToolStripMenuItem");
            this.OpenResultDirToolStripMenuItem.Click += new System.EventHandler(this.OpenDirToolStripMenuItem_Click);
            // 
            // RunConverterToolStripMenuItem
            // 
            this.RunConverterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConvertWemToWaveToolStripMenuItem,
            this.ConvertWemToOggToolStripMenuItem,
            this.toolStripMenuItem2,
            this.DelWavToolStripMenuItem,
            this.DelOggToolStripMenuItem});
            this.RunConverterToolStripMenuItem.Name = "RunConverterToolStripMenuItem";
            resources.ApplyResources(this.RunConverterToolStripMenuItem, "RunConverterToolStripMenuItem");
            // 
            // ConvertWemToWaveToolStripMenuItem
            // 
            this.ConvertWemToWaveToolStripMenuItem.Name = "ConvertWemToWaveToolStripMenuItem";
            resources.ApplyResources(this.ConvertWemToWaveToolStripMenuItem, "ConvertWemToWaveToolStripMenuItem");
            this.ConvertWemToWaveToolStripMenuItem.Click += new System.EventHandler(this.Convert_WEM2WAV_ToolStripMenuItem_Click);
            // 
            // ConvertWemToOggToolStripMenuItem
            // 
            this.ConvertWemToOggToolStripMenuItem.Name = "ConvertWemToOggToolStripMenuItem";
            resources.ApplyResources(this.ConvertWemToOggToolStripMenuItem, "ConvertWemToOggToolStripMenuItem");
            this.ConvertWemToOggToolStripMenuItem.Click += new System.EventHandler(this.Convert_WEM2OGG_ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // DelWavToolStripMenuItem
            // 
            this.DelWavToolStripMenuItem.Name = "DelWavToolStripMenuItem";
            resources.ApplyResources(this.DelWavToolStripMenuItem, "DelWavToolStripMenuItem");
            this.DelWavToolStripMenuItem.Click += new System.EventHandler(this.RemoveWAV_ToolStripMenuItem_Click);
            // 
            // DelOggToolStripMenuItem
            // 
            this.DelOggToolStripMenuItem.Name = "DelOggToolStripMenuItem";
            resources.ApplyResources(this.DelOggToolStripMenuItem, "DelOggToolStripMenuItem");
            this.DelOggToolStripMenuItem.Click += new System.EventHandler(this.RemoveOGG_ToolStripMenuItem_Click);
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.HelpToolStripMenuItem.AutoToolTip = true;
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            resources.ApplyResources(this.HelpToolStripMenuItem, "HelpToolStripMenuItem");
            // 
            // UpdateToolStripMenuItem
            // 
            this.UpdateToolStripMenuItem.Name = "UpdateToolStripMenuItem";
            resources.ApplyResources(this.UpdateToolStripMenuItem, "UpdateToolStripMenuItem");
            this.UpdateToolStripMenuItem.Click += new System.EventHandler(this.CheckNewVersionToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.listBoxEvents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBoxEvents;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PreferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CleanToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem ProjectOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BuildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenResultDirToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonGetDirectory;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.ToolStripMenuItem CopyMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunConverterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem ConvertWemToWaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConvertWemToOggToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem DelWavToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DelOggToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem addSilenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem UpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.Button buttonFiles;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.ToolStripMenuItem openSourceDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPilotsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CheckToolStripMenuItem;
    }
}

