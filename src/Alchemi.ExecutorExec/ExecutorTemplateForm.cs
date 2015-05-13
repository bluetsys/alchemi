#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ExecutorTemplateForm.cs
* Project		:	Alchemi Executor
* Created on	:	2006
* Copyright		:	Copyright � 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Krishna Nadiminti (kna@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion

using System;
using System.Threading;
using System.Windows.Forms;
using Alchemi.Core;
using Alchemi.Executor;
using Alchemi.ExecutorExec;
using Timer = System.Windows.Forms.Timer;

    public class ExecutorTemplateForm : Form
    {
		protected static readonly Logger logger = new Logger();

        protected TextBox txLog;
		private System.ComponentModel.IContainer components;
        protected MainMenu MainMenu;
        protected MenuItem menuItem1;
        protected MenuItem mmExit;
        protected NotifyIcon TrayIcon;
        protected ContextMenu TrayMenu;
        protected MenuItem tmExit;
        protected MenuItem menuItem2;
        protected MenuItem mmAbout;
        protected Timer HideTimer;
        
        protected Configuration Config;
        protected ExecutorContainer _container = null;
        protected Button btReset;
        protected Button btDisconnect;
        protected GroupBox groupBox1;
        protected GroupBox groupBox2;
        protected CheckBox cbDedicated;
        protected Label label4;
        protected Button btConnect;
        protected Button btStopExec;
        protected Button btStartExec;
        protected GroupBox groupBox3;
        protected Label label5;
        protected Label label6;
        protected TextBox txPassword;
        protected TextBox txUsername;
		protected NumericUpDown udHBInterval;
		protected Label lblHBInt;
		protected Label label7;
		protected Label label8;
		protected CheckBox chkRetryConnect;
		protected Label label9;
		protected Label label10;
		protected NumericUpDown udMaxRetry;
		protected Label label11;
		protected NumericUpDown udReconnectInterval;
		protected ProgressBar pbar;
		protected System.Windows.Forms.StatusBar statusBar;
		protected System.Windows.Forms.TabControl tabControl;
		protected System.Windows.Forms.TabPage tabConnection;
		protected System.Windows.Forms.TabPage tabExecution;
        protected System.Windows.Forms.TabPage tabOptions;
        protected TextBox txId;
        protected Label label12;
        protected LinkLabel lnkViewLog;
        private CheckBox checkBox1;
        protected Alchemi.Core.EndPointUtils.EndPointControl ucOwnEndPoint;
        protected Alchemi.Core.EndPointUtils.EndPointControl ucManagerEndPoint;

		protected static bool silentMode = true;

		public ExecutorTemplateForm()
		{
            InitializeComponent();
		}

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecutorTemplateForm));
            this.txLog = new System.Windows.Forms.TextBox();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenu();
            this.tmExit = new System.Windows.Forms.MenuItem();
            this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mmExit = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mmAbout = new System.Windows.Forms.MenuItem();
            this.HideTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txPassword = new System.Windows.Forms.TextBox();
            this.txUsername = new System.Windows.Forms.TextBox();
            this.btReset = new System.Windows.Forms.Button();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txId = new System.Windows.Forms.TextBox();
            this.cbDedicated = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btConnect = new System.Windows.Forms.Button();
            this.tabExecution = new System.Windows.Forms.TabPage();
            this.btStopExec = new System.Windows.Forms.Button();
            this.btStartExec = new System.Windows.Forms.Button();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.udMaxRetry = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkRetryConnect = new System.Windows.Forms.CheckBox();
            this.udReconnectInterval = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblHBInt = new System.Windows.Forms.Label();
            this.udHBInterval = new System.Windows.Forms.NumericUpDown();
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.label12 = new System.Windows.Forms.Label();
            this.lnkViewLog = new System.Windows.Forms.LinkLabel();
            this.ucManagerEndPoint = new Alchemi.Core.EndPointUtils.EndPointControl();
            this.ucOwnEndPoint = new Alchemi.Core.EndPointUtils.EndPointControl();
            this.tabControl.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabExecution.SuspendLayout();
            this.tabOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMaxRetry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udReconnectInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHBInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // txLog
            // 
            this.txLog.BackColor = System.Drawing.SystemColors.Info;
            this.txLog.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txLog.Location = new System.Drawing.Point(16, 427);
            this.txLog.Multiline = true;
            this.txLog.Name = "txLog";
            this.txLog.ReadOnly = true;
            this.txLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txLog.Size = new System.Drawing.Size(424, 96);
            this.txLog.TabIndex = 11;
            this.txLog.TabStop = false;
            this.txLog.DoubleClick += new System.EventHandler(this.txLog_DoubleClick);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 521);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(458, 22);
            this.statusBar.TabIndex = 2;
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenu = this.TrayMenu;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "Alchemi Executor";
            this.TrayIcon.Visible = true;
            this.TrayIcon.Click += new System.EventHandler(this.TrayIcon_Click);
            // 
            // TrayMenu
            // 
            this.TrayMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.tmExit});
            // 
            // tmExit
            // 
            this.tmExit.Index = 0;
            this.tmExit.Text = "Exit";
            this.tmExit.Click += new System.EventHandler(this.tmExit_Click);
            // 
            // MainMenu
            // 
            this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mmExit});
            this.menuItem1.Text = "Executor";
            // 
            // mmExit
            // 
            this.mmExit.Index = 0;
            this.mmExit.Text = "Exit";
            this.mmExit.Click += new System.EventHandler(this.mmExit_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mmAbout});
            this.menuItem2.Text = "Help";
            // 
            // mmAbout
            // 
            this.mmAbout.Index = 0;
            this.mmAbout.Text = "About";
            this.mmAbout.Click += new System.EventHandler(this.mmAbout_Click);
            // 
            // HideTimer
            // 
            this.HideTimer.Interval = 1000;
            this.HideTimer.Tick += new System.EventHandler(this.HideTimer_Tick);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabConnection);
            this.tabControl.Controls.Add(this.tabExecution);
            this.tabControl.Controls.Add(this.tabOptions);
            this.tabControl.ItemSize = new System.Drawing.Size(97, 18);
            this.tabControl.Location = new System.Drawing.Point(8, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(440, 392);
            this.tabControl.TabIndex = 6;
            // 
            // tabConnection
            // 
            this.tabConnection.BackColor = System.Drawing.SystemColors.Control;
            this.tabConnection.Controls.Add(this.groupBox3);
            this.tabConnection.Controls.Add(this.btReset);
            this.tabConnection.Controls.Add(this.btDisconnect);
            this.tabConnection.Controls.Add(this.groupBox1);
            this.tabConnection.Controls.Add(this.groupBox2);
            this.tabConnection.Controls.Add(this.btConnect);
            this.tabConnection.Location = new System.Drawing.Point(4, 22);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Size = new System.Drawing.Size(432, 366);
            this.tabConnection.TabIndex = 0;
            this.tabConnection.Text = "Setup Connection";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txPassword);
            this.groupBox3.Controls.Add(this.txUsername);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Location = new System.Drawing.Point(4, 199);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 72);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Credentials";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(106, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Password";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = "Username";
            // 
            // txPassword
            // 
            this.txPassword.Location = new System.Drawing.Point(106, 40);
            this.txPassword.Name = "txPassword";
            this.txPassword.PasswordChar = '*';
            this.txPassword.Size = new System.Drawing.Size(84, 20);
            this.txPassword.TabIndex = 4;
            // 
            // txUsername
            // 
            this.txUsername.Location = new System.Drawing.Point(16, 40);
            this.txUsername.Name = "txUsername";
            this.txUsername.Size = new System.Drawing.Size(84, 20);
            this.txUsername.TabIndex = 3;
            // 
            // btReset
            // 
            this.btReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btReset.Location = new System.Drawing.Point(96, 296);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(224, 23);
            this.btReset.TabIndex = 8;
            this.btReset.TabStop = false;
            this.btReset.Text = "Reset";
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // btDisconnect
            // 
            this.btDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btDisconnect.Location = new System.Drawing.Point(208, 328);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(112, 23);
            this.btDisconnect.TabIndex = 10;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.Click += new System.EventHandler(this.btDisconnect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucManagerEndPoint);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 190);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manager Node";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ucOwnEndPoint);
            this.groupBox2.Controls.Add(this.txId);
            this.groupBox2.Controls.Add(this.cbDedicated);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(218, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(210, 268);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Own Node";
            // 
            // txId
            // 
            this.txId.BackColor = System.Drawing.SystemColors.Window;
            this.txId.Location = new System.Drawing.Point(16, 39);
            this.txId.Name = "txId";
            this.txId.ReadOnly = true;
            this.txId.Size = new System.Drawing.Size(188, 20);
            this.txId.TabIndex = 12;
            this.txId.TabStop = false;
            // 
            // cbDedicated
            // 
            this.cbDedicated.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbDedicated.Location = new System.Drawing.Point(16, 65);
            this.cbDedicated.Name = "cbDedicated";
            this.cbDedicated.Size = new System.Drawing.Size(88, 32);
            this.cbDedicated.TabIndex = 6;
            this.cbDedicated.Text = "Dedicated?";
            this.cbDedicated.CheckedChanged += new System.EventHandler(this.cbDedicated_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Id";
            // 
            // btConnect
            // 
            this.btConnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btConnect.Location = new System.Drawing.Point(96, 328);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(104, 23);
            this.btConnect.TabIndex = 9;
            this.btConnect.Text = "Connect";
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // tabExecution
            // 
            this.tabExecution.BackColor = System.Drawing.SystemColors.Control;
            this.tabExecution.Controls.Add(this.btStopExec);
            this.tabExecution.Controls.Add(this.btStartExec);
            this.tabExecution.Location = new System.Drawing.Point(4, 22);
            this.tabExecution.Name = "tabExecution";
            this.tabExecution.Size = new System.Drawing.Size(432, 366);
            this.tabExecution.TabIndex = 1;
            this.tabExecution.Text = "Manage Execution";
            // 
            // btStopExec
            // 
            this.btStopExec.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btStopExec.Location = new System.Drawing.Point(120, 152);
            this.btStopExec.Name = "btStopExec";
            this.btStopExec.Size = new System.Drawing.Size(192, 23);
            this.btStopExec.TabIndex = 1;
            this.btStopExec.Text = "Stop Executing";
            this.btStopExec.Click += new System.EventHandler(this.btStopExec_Click);
            // 
            // btStartExec
            // 
            this.btStartExec.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btStartExec.Location = new System.Drawing.Point(120, 112);
            this.btStartExec.Name = "btStartExec";
            this.btStartExec.Size = new System.Drawing.Size(192, 23);
            this.btStartExec.TabIndex = 0;
            this.btStartExec.Text = "StartApplication Executing";
            this.btStartExec.Click += new System.EventHandler(this.btStartExec_Click);
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.checkBox1);
            this.tabOptions.Controls.Add(this.label11);
            this.tabOptions.Controls.Add(this.udMaxRetry);
            this.tabOptions.Controls.Add(this.label10);
            this.tabOptions.Controls.Add(this.label9);
            this.tabOptions.Controls.Add(this.chkRetryConnect);
            this.tabOptions.Controls.Add(this.udReconnectInterval);
            this.tabOptions.Controls.Add(this.label8);
            this.tabOptions.Controls.Add(this.label7);
            this.tabOptions.Controls.Add(this.lblHBInt);
            this.tabOptions.Controls.Add(this.udHBInterval);
            this.tabOptions.Location = new System.Drawing.Point(4, 22);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new System.Drawing.Size(432, 366);
            this.tabOptions.TabIndex = 2;
            this.tabOptions.Text = "Options";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(27, 184);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(299, 30);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Enforce secure sanboxed execution\r\n(Turn off this option to allow legacy applicat" +
                "ions ie. GJobs)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(183, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 16);
            this.label11.TabIndex = 9;
            this.label11.Text = "times at most.";
            // 
            // udMaxRetry
            // 
            this.udMaxRetry.Location = new System.Drawing.Point(118, 124);
            this.udMaxRetry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.udMaxRetry.Name = "udMaxRetry";
            this.udMaxRetry.Size = new System.Drawing.Size(52, 20);
            this.udMaxRetry.TabIndex = 8;
            this.udMaxRetry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.udMaxRetry.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(24, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 16);
            this.label10.TabIndex = 7;
            this.label10.Text = "Try to reconnect ";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(24, 99);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(176, 16);
            this.label9.TabIndex = 6;
            this.label9.Text = "Continue to try and connect every";
            // 
            // chkRetryConnect
            // 
            this.chkRetryConnect.Checked = true;
            this.chkRetryConnect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRetryConnect.Location = new System.Drawing.Point(24, 72);
            this.chkRetryConnect.Name = "chkRetryConnect";
            this.chkRetryConnect.Size = new System.Drawing.Size(267, 17);
            this.chkRetryConnect.TabIndex = 5;
            this.chkRetryConnect.Text = "Retry connecting to Manager on disconnection.";
            // 
            // udReconnectInterval
            // 
            this.udReconnectInterval.Location = new System.Drawing.Point(200, 97);
            this.udReconnectInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udReconnectInterval.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udReconnectInterval.Name = "udReconnectInterval";
            this.udReconnectInterval.Size = new System.Drawing.Size(72, 20);
            this.udReconnectInterval.TabIndex = 4;
            this.udReconnectInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.udReconnectInterval.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(280, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "seconds.";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(216, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "seconds.";
            // 
            // lblHBInt
            // 
            this.lblHBInt.Location = new System.Drawing.Point(24, 23);
            this.lblHBInt.Name = "lblHBInt";
            this.lblHBInt.Size = new System.Drawing.Size(100, 16);
            this.lblHBInt.TabIndex = 1;
            this.lblHBInt.Text = "Heartbeat interval";
            // 
            // udHBInterval
            // 
            this.udHBInterval.Location = new System.Drawing.Point(133, 20);
            this.udHBInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udHBInterval.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udHBInterval.Name = "udHBInterval";
            this.udHBInterval.Size = new System.Drawing.Size(72, 20);
            this.udHBInterval.TabIndex = 0;
            this.udHBInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.udHBInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // pbar
            // 
            this.pbar.Location = new System.Drawing.Point(16, 527);
            this.pbar.Maximum = 5;
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(424, 8);
            this.pbar.Step = 1;
            this.pbar.TabIndex = 12;
            this.pbar.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 408);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Log Messages";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lnkViewLog
            // 
            this.lnkViewLog.AutoSize = true;
            this.lnkViewLog.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkViewLog.Location = new System.Drawing.Point(96, 408);
            this.lnkViewLog.Name = "lnkViewLog";
            this.lnkViewLog.Size = new System.Drawing.Size(87, 13);
            this.lnkViewLog.TabIndex = 14;
            this.lnkViewLog.TabStop = true;
            this.lnkViewLog.Text = "( View full log ... )";
            this.lnkViewLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucManagerEndPoint
            // 
            this.ucManagerEndPoint.AddressPart = "";
            this.ucManagerEndPoint.BindingConfigurationName = "";
            this.ucManagerEndPoint.BindingSettingType = Alchemi.Core.WCFBindingSettingType.Default;
            this.ucManagerEndPoint.FixedAddressPart = false;
            this.ucManagerEndPoint.FullAddress = "http://localhost:0/";
            this.ucManagerEndPoint.Host = "localhost";
            this.ucManagerEndPoint.HostNameForPublishing = "localhost";
            this.ucManagerEndPoint.Location = new System.Drawing.Point(6, 19);
            this.ucManagerEndPoint.Name = "ucManagerEndPoint";
            this.ucManagerEndPoint.Port = 0;
            this.ucManagerEndPoint.Protocol = "";
            this.ucManagerEndPoint.SelectedRemotingMechanism = Alchemi.Core.RemotingMechanism.WCF;
            this.ucManagerEndPoint.ServiceConfigurationName = "";
            this.ucManagerEndPoint.Size = new System.Drawing.Size(198, 165);
            this.ucManagerEndPoint.TabIndex = 0;
            this.ucManagerEndPoint.WCFBinding = Alchemi.Core.WCFBinding.None;
            // 
            // ucOwnEndPoint
            // 
            this.ucOwnEndPoint.AddressPart = "";
            this.ucOwnEndPoint.BindingConfigurationName = "";
            this.ucOwnEndPoint.BindingSettingType = Alchemi.Core.WCFBindingSettingType.Default;
            this.ucOwnEndPoint.FixedAddressPart = false;
            this.ucOwnEndPoint.FullAddress = "http://localhost:0/";
            this.ucOwnEndPoint.Host = "localhost";
            this.ucOwnEndPoint.HostNameForPublishing = "localhost";
            this.ucOwnEndPoint.Location = new System.Drawing.Point(9, 103);
            this.ucOwnEndPoint.Name = "ucOwnEndPoint";
            this.ucOwnEndPoint.Port = 0;
            this.ucOwnEndPoint.Protocol = "";
            this.ucOwnEndPoint.SelectedRemotingMechanism = Alchemi.Core.RemotingMechanism.WCF;
            this.ucOwnEndPoint.ServiceConfigurationName = "";
            this.ucOwnEndPoint.Size = new System.Drawing.Size(195, 153);
            this.ucOwnEndPoint.TabIndex = 13;
            this.ucOwnEndPoint.WCFBinding = Alchemi.Core.WCFBinding.None;
            // 
            // ExecutorTemplateForm
            // 
            this.AcceptButton = this.btConnect;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(458, 543);
            this.Controls.Add(this.lnkViewLog);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txLog);
            this.Controls.Add(this.pbar);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.MainMenu;
            this.Name = "ExecutorTemplateForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alchemi Executor";
            this.Load += new System.EventHandler(this.ExecutorTemplateForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabExecution.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMaxRetry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udReconnectInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHBInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        //-----------------------------------------------------------------------------------------------        
        //
        // Form Event Handlers
        //
        //-----------------------------------------------------------------------------------------------    
    
        protected virtual void ExecutorTemplateForm_Load(object sender, EventArgs e)
        {
			//normally this should not cause any problems,
			//but during design-time, it is needed to avoid exceptions due to things that dont mean anything during design time.
			try
			{
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);
			
				//for windows forms apps unhandled exceptions on the main thread
				Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            
				// avoid multiple instances
				/*
				bool isOnlyInstance = false;
				Mutex mtx = new Mutex(true, "AlchemiExecutor_Mutex", out isOnlyInstance);
				if(!isOnlyInstance)
				{
					MessageBox.Show("An instance of this application is already running. The program will now exit.", "Alchemi Executor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Application.Exit();
				}
				*/

                ucOwnEndPoint.RemoveRemotingMechanism(RemotingMechanism.WCFCustom);
                ucOwnEndPoint.AddressPart = "Executor";
                ucOwnEndPoint.FixedAddressPart = true;
			}
			catch {}
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void btConnect_Click(object sender, EventArgs e)
        {
			ConnectExecutor();
        }

    	protected void GetConfigFromUI()
    	{
			if (Config == null)
			{
				Config = new Configuration();
			}
			// read config from ui controls
            //Config.ManagerHost = txMgrHost.Text;
            //Config.ManagerPort = int.Parse(txMgrPort.Text);
            //Config.OwnPort = int.Parse(txOwnPort.Text);     
            ucManagerEndPoint.WriteEndPointConfiguration(Config.ManagerEndPoint);
            ucOwnEndPoint.WriteEndPointConfiguration(Config.OwnEndPoint);
			Config.Dedicated = cbDedicated.Checked;
			Config.HeartBeatInterval = (int)udHBInterval.Value;
			Config.RetryConnect = chkRetryConnect.Checked;
			Config.RetryInterval = (int)udReconnectInterval.Value;
			Config.RetryMax = (int)udMaxRetry.Value;
		}

    	//-----------------------------------------------------------------------------------------------    
    
        private void mmExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        //-----------------------------------------------------------------------------------------------    

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND & (int) m.WParam == SC_CLOSE)
            {
                // 'X' button clicked .. minimise to system tray
                Hide();
                return;
            }
            base.WndProc(ref m);
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void TrayIcon_Click(object sender, EventArgs e)
        {
            Restore();
        }

        //-----------------------------------------------------------------------------------------------    

        private void tmExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void btStartExec_Click(object sender, EventArgs e)
        {
			StartExecuting();
		}

        //-----------------------------------------------------------------------------------------------    
    
        private void btStopExec_Click(object sender, EventArgs e)
        {
			StopExecuting();
		}
    
        //-----------------------------------------------------------------------------------------------    

        private void mmAbout_Click(object sender, EventArgs e)
        {
            ShowSplashScreen();
        }

        //-----------------------------------------------------------------------------------------------    

        static void DefaultErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception) args.ExceptionObject;
//            if (e.GetType() == typeof(AppDomainUnloadedException))
//            {
//                // can't figure out why this exception is being thrown .. in any case, seems safe to ignore it
//                //return;
//            }
			HandleAllUnknownErrors(sender.ToString(),e);
        }                                   

        //-----------------------------------------------------------------------------------------------    
    
        private void HideTimer_Tick(object sender, EventArgs e)
        {
            Hide();
            HideTimer.Enabled = false;
        }

        //-----------------------------------------------------------------------------------------------        
        //
        // Core Methods
        //
        //-----------------------------------------------------------------------------------------------    

        protected void Log(string s)
        {
            if (txLog != null && !txLog.IsDisposed)
            {
				if (txLog.Text.Length + s.Length >= txLog.MaxLength)
				{
					//remove all old stuff except the last 10 lines.
					string[] s1 = new string[10];
					for (int i = 0 ; i < 10 ; i++)
					{
						s1[9-i]=txLog.Lines[txLog.Lines.Length-1-i];
					}
					txLog.Lines = s1;
				}
				txLog.AppendText(s + Environment.NewLine);
            }
        }

        
		//-----------------------------------------------------------------------------------------------            

        private void Restore()
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void btDisconnect_Click(object sender, EventArgs e)
        {
			DisconnectExecutor();
		}

    	//-----------------------------------------------------------------------------------------------    

        private void btReset_Click(object sender, EventArgs e)
        {
			ResetExecutor();
		}

        //-----------------------------------------------------------------------------------------------    

        private void cbDedicated_CheckedChanged(object sender, EventArgs e)
        {
            ucOwnEndPoint.Enabled = cbDedicated.Checked;
        }

        //-----------------------------------------------------------------------------------------------

        private void ShowSplashScreen()
        {
            SplashScreen ss = new SplashScreen();
            ss.ShowDialog();
        }

        private void txLog_DoubleClick(object sender, EventArgs e)
        {
            txLog.Clear();
        }

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			HandleAllUnknownErrors(sender.ToString(),e.Exception);
		}

		static void HandleAllUnknownErrors(string sender, Exception e)
		{
			logger.Error("Unknown Error from: " + sender,e);
			if (!silentMode)
			{
				MessageBox.Show(e.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#region Methods to be implemented by sub-classes
		
		//These methods actually should be "abstract", so that the methods are forcibly implemented.
		//but we need the template class to be non-abstract, for designer-support.
		//this is why we have declared these as virtual

		/// <summary>
		/// Specifies whether the Executor is Connected / ExecutorService is Started
		/// </summary>
		protected virtual bool Started
		{
			get
			{
				return false;
			}
		}

		protected virtual void ConnectExecutor(){}
		protected virtual void DisconnectExecutor(){}
		protected virtual void ResetExecutor(){}

		protected virtual void StartExecuting(){}
		protected virtual void StopExecuting(){}

		protected virtual void RefreshUIControls(){}

		protected virtual void Exit(){}

		#endregion


    }