using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;

namespace ShareX;

public class AutoCaptureForm : Form
{
	private static AutoCaptureForm instance;

	private bool isLoaded;

	private System.Windows.Forms.Timer statusTimer;

	private System.Timers.Timer screenshotTimer;

	private int delay;

	private int count;

	private int timeleft;

	private int percentage;

	private bool waitUploads;

	private Stopwatch stopwatch = new Stopwatch();

	private Rectangle customRegion;

	private IContainer components;

	private StatusStrip ssBar;

	private ToolStripProgressBar tspbBar;

	private Button btnExecute;

	private CheckBox cbWaitUploads;

	private ToolStripStatusLabel tsslStatus;

	private CheckBox cbAutoMinimize;

	private Label lblRegion;

	private Button btnRegion;

	private NumericUpDown nudRepeatTime;

	private Label lblDuration;

	private NotifyIcon niTray;

	private Label lblDurationSeconds;

	private GroupBox gbRegion;

	private RadioButton rbFullscreen;

	private RadioButton rbCustomRegion;

	public static AutoCaptureForm Instance
	{
		get
		{
			if (instance == null || instance.IsDisposed)
			{
				instance = new AutoCaptureForm();
			}
			return instance;
		}
	}

	public static bool IsRunning { get; private set; }

	public TaskSettings TaskSettings { get; internal set; }

	private AutoCaptureForm()
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		niTray.Icon = Resources.clock.ToIcon();
		screenshotTimer = new System.Timers.Timer();
		screenshotTimer.SynchronizingObject = this;
		screenshotTimer.Elapsed += screenshotTimer_Elapsed;
		statusTimer = new System.Windows.Forms.Timer
		{
			Interval = 250
		};
		statusTimer.Tick += delegate
		{
			UpdateStatus();
		};
		customRegion = Program.Settings.AutoCaptureRegion;
		UpdateRegion();
		nudRepeatTime.SetValue(Program.Settings.AutoCaptureRepeatTime);
		cbAutoMinimize.Checked = Program.Settings.AutoCaptureMinimizeToTray;
		cbWaitUploads.Checked = Program.Settings.AutoCaptureWaitUpload;
		isLoaded = true;
	}

	private void screenshotTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (IsRunning)
		{
			if (waitUploads && TaskManager.IsBusy)
			{
				screenshotTimer.Interval = 1000.0;
				return;
			}
			stopwatch.Reset();
			stopwatch.Start();
			screenshotTimer.Interval = delay;
			count++;
			TakeScreenshot();
		}
	}

	private void TakeScreenshot()
	{
		Rectangle autoCaptureRegion = Program.Settings.AutoCaptureRegion;
		if (!autoCaptureRegion.IsEmpty)
		{
			Bitmap bitmap = TaskHelpers.GetScreenshot(TaskSettings).CaptureRectangle(autoCaptureRegion);
			if (bitmap != null)
			{
				TaskSettings.UseDefaultAfterCaptureJob = false;
				TaskSettings.AfterCaptureJob = TaskSettings.AfterCaptureJob.Remove<AfterCaptureTasks>(AfterCaptureTasks.AnnotateImage);
				TaskSettings.UseDefaultAdvancedSettings = false;
				TaskSettings.GeneralSettings.DisableNotifications = true;
				UploadManager.RunImageTask(bitmap, TaskSettings, skipQuickTaskMenu: true, skipAfterCaptureWindow: true);
			}
		}
	}

	private void SelectRegion()
	{
		if (RegionCaptureTasks.GetRectangleRegion(out var rect, TaskSettings.CaptureSettings.SurfaceOptions))
		{
			Program.Settings.AutoCaptureRegion = rect;
			UpdateRegion();
		}
	}

	private void UpdateRegion()
	{
		Rectangle autoCaptureRegion = Program.Settings.AutoCaptureRegion;
		if (!autoCaptureRegion.IsEmpty)
		{
			lblRegion.Text = string.Format(Resources.AutoCaptureForm_UpdateRegion_X___0___Y___1___Width___2___Height___3_, autoCaptureRegion.X, autoCaptureRegion.Y, autoCaptureRegion.Width, autoCaptureRegion.Height);
			btnExecute.Enabled = true;
		}
	}

	private void UpdateStatus()
	{
		if (IsRunning && !base.IsDisposed)
		{
			timeleft = Math.Max(0, delay - (int)stopwatch.ElapsedMilliseconds);
			percentage = (int)(100.0 - (double)timeleft / (double)delay * 100.0);
			tspbBar.Value = percentage;
			string arg = ((float)timeleft / 1000f).ToString("0.0");
			tsslStatus.Text = " " + string.Format(Resources.AutoCaptureForm_UpdateStatus_Timeleft___0_s___1____Total___2_, arg, percentage, count);
		}
	}

	public void Execute()
	{
		if (IsRunning)
		{
			IsRunning = false;
			tspbBar.Value = 0;
			stopwatch.Reset();
			btnExecute.Text = Resources.AutoCaptureForm_Execute_Start;
		}
		else
		{
			IsRunning = true;
			btnExecute.Text = Resources.AutoCaptureForm_Execute_Stop;
			screenshotTimer.Interval = 1000.0;
			delay = (int)(Program.Settings.AutoCaptureRepeatTime * 1000m);
			waitUploads = Program.Settings.AutoCaptureWaitUpload;
			if (Program.Settings.AutoCaptureMinimizeToTray)
			{
				base.Visible = false;
				niTray.Visible = true;
			}
		}
		screenshotTimer.Enabled = IsRunning;
		statusTimer.Enabled = IsRunning;
	}

	private void rbFullscreen_CheckedChanged(object sender, EventArgs e)
	{
		if (isLoaded && rbFullscreen.Checked)
		{
			customRegion = Program.Settings.AutoCaptureRegion;
			Program.Settings.AutoCaptureRegion = CaptureHelpers.GetScreenBounds();
			UpdateRegion();
			btnRegion.Enabled = false;
		}
	}

	private void rbCustomRegion_CheckedChanged(object sender, EventArgs e)
	{
		if (isLoaded && rbCustomRegion.Checked)
		{
			Program.Settings.AutoCaptureRegion = customRegion;
			UpdateRegion();
			btnRegion.Enabled = true;
		}
	}

	private void btnRegion_Click(object sender, EventArgs e)
	{
		SelectRegion();
	}

	private void nudDuration_ValueChanged(object sender, EventArgs e)
	{
		Program.Settings.AutoCaptureRepeatTime = nudRepeatTime.Value;
	}

	private void cbAutoMinimize_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.AutoCaptureMinimizeToTray = cbAutoMinimize.Checked;
	}

	private void cbWaitUploads_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.AutoCaptureWaitUpload = cbWaitUploads.Checked;
	}

	private void btnExecute_Click(object sender, EventArgs e)
	{
		Execute();
	}

	private void AutoCapture_FormClosing(object sender, FormClosingEventArgs e)
	{
		IsRunning = false;
		screenshotTimer.Enabled = false;
		statusTimer.Enabled = false;
	}

	private void AutoCapture_Resize(object sender, EventArgs e)
	{
		if (Program.Settings.AutoCaptureMinimizeToTray && base.WindowState == FormWindowState.Minimized)
		{
			base.Visible = false;
			niTray.Visible = true;
		}
	}

	private void niTray_MouseClick(object sender, MouseEventArgs e)
	{
		niTray.Visible = false;
		this.ForceActivate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		if (screenshotTimer != null)
		{
			screenshotTimer.Dispose();
		}
		if (statusTimer != null)
		{
			statusTimer.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.AutoCaptureForm));
		this.ssBar = new System.Windows.Forms.StatusStrip();
		this.tspbBar = new System.Windows.Forms.ToolStripProgressBar();
		this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.btnExecute = new System.Windows.Forms.Button();
		this.cbWaitUploads = new System.Windows.Forms.CheckBox();
		this.cbAutoMinimize = new System.Windows.Forms.CheckBox();
		this.lblRegion = new System.Windows.Forms.Label();
		this.btnRegion = new System.Windows.Forms.Button();
		this.nudRepeatTime = new System.Windows.Forms.NumericUpDown();
		this.lblDuration = new System.Windows.Forms.Label();
		this.niTray = new System.Windows.Forms.NotifyIcon(this.components);
		this.lblDurationSeconds = new System.Windows.Forms.Label();
		this.gbRegion = new System.Windows.Forms.GroupBox();
		this.rbFullscreen = new System.Windows.Forms.RadioButton();
		this.rbCustomRegion = new System.Windows.Forms.RadioButton();
		this.ssBar.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRepeatTime).BeginInit();
		this.gbRegion.SuspendLayout();
		base.SuspendLayout();
		this.ssBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tspbBar, this.tsslStatus });
		resources.ApplyResources(this.ssBar, "ssBar");
		this.ssBar.Name = "ssBar";
		this.ssBar.SizingGrip = false;
		this.tspbBar.Name = "tspbBar";
		resources.ApplyResources(this.tspbBar, "tspbBar");
		this.tsslStatus.BackColor = System.Drawing.Color.Transparent;
		this.tsslStatus.Name = "tsslStatus";
		resources.ApplyResources(this.tsslStatus, "tsslStatus");
		resources.ApplyResources(this.btnExecute, "btnExecute");
		this.btnExecute.Name = "btnExecute";
		this.btnExecute.UseVisualStyleBackColor = true;
		this.btnExecute.Click += new System.EventHandler(btnExecute_Click);
		resources.ApplyResources(this.cbWaitUploads, "cbWaitUploads");
		this.cbWaitUploads.Name = "cbWaitUploads";
		this.cbWaitUploads.UseVisualStyleBackColor = true;
		this.cbWaitUploads.CheckedChanged += new System.EventHandler(cbWaitUploads_CheckedChanged);
		resources.ApplyResources(this.cbAutoMinimize, "cbAutoMinimize");
		this.cbAutoMinimize.Name = "cbAutoMinimize";
		this.cbAutoMinimize.UseVisualStyleBackColor = true;
		this.cbAutoMinimize.CheckedChanged += new System.EventHandler(cbAutoMinimize_CheckedChanged);
		resources.ApplyResources(this.lblRegion, "lblRegion");
		this.lblRegion.Name = "lblRegion";
		resources.ApplyResources(this.btnRegion, "btnRegion");
		this.btnRegion.Name = "btnRegion";
		this.btnRegion.UseVisualStyleBackColor = true;
		this.btnRegion.Click += new System.EventHandler(btnRegion_Click);
		this.nudRepeatTime.DecimalPlaces = 1;
		resources.ApplyResources(this.nudRepeatTime, "nudRepeatTime");
		this.nudRepeatTime.Maximum = new decimal(new int[4] { 86400, 0, 0, 0 });
		this.nudRepeatTime.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudRepeatTime.Name = "nudRepeatTime";
		this.nudRepeatTime.Value = new decimal(new int[4] { 3, 0, 0, 0 });
		this.nudRepeatTime.ValueChanged += new System.EventHandler(nudDuration_ValueChanged);
		resources.ApplyResources(this.lblDuration, "lblDuration");
		this.lblDuration.Name = "lblDuration";
		resources.ApplyResources(this.niTray, "niTray");
		this.niTray.MouseClick += new System.Windows.Forms.MouseEventHandler(niTray_MouseClick);
		resources.ApplyResources(this.lblDurationSeconds, "lblDurationSeconds");
		this.lblDurationSeconds.Name = "lblDurationSeconds";
		this.gbRegion.Controls.Add(this.rbFullscreen);
		this.gbRegion.Controls.Add(this.rbCustomRegion);
		this.gbRegion.Controls.Add(this.btnRegion);
		this.gbRegion.Controls.Add(this.lblRegion);
		resources.ApplyResources(this.gbRegion, "gbRegion");
		this.gbRegion.Name = "gbRegion";
		this.gbRegion.TabStop = false;
		resources.ApplyResources(this.rbFullscreen, "rbFullscreen");
		this.rbFullscreen.Name = "rbFullscreen";
		this.rbFullscreen.UseVisualStyleBackColor = true;
		this.rbFullscreen.CheckedChanged += new System.EventHandler(rbFullscreen_CheckedChanged);
		resources.ApplyResources(this.rbCustomRegion, "rbCustomRegion");
		this.rbCustomRegion.Checked = true;
		this.rbCustomRegion.Name = "rbCustomRegion";
		this.rbCustomRegion.TabStop = true;
		this.rbCustomRegion.UseVisualStyleBackColor = true;
		this.rbCustomRegion.CheckedChanged += new System.EventHandler(rbCustomRegion_CheckedChanged);
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.gbRegion);
		base.Controls.Add(this.lblDurationSeconds);
		base.Controls.Add(this.nudRepeatTime);
		base.Controls.Add(this.lblDuration);
		base.Controls.Add(this.cbAutoMinimize);
		base.Controls.Add(this.cbWaitUploads);
		base.Controls.Add(this.btnExecute);
		base.Controls.Add(this.ssBar);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.Name = "AutoCaptureForm";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AutoCapture_FormClosing);
		base.Resize += new System.EventHandler(AutoCapture_Resize);
		this.ssBar.ResumeLayout(false);
		this.ssBar.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRepeatTime).EndInit();
		this.gbRegion.ResumeLayout(false);
		this.gbRegion.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
