using System;
using System.ComponentModel;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;

namespace ShareX;

public class InspectWindowForm : Form
{
	private IContainer components;

	private RichTextBox rtbInfo;

	private Panel pInfo;

	private Button btnInspectWindow;

	private Button btnInspectControl;

	private Button btnRefresh;

	private Button btnPinToTop;

	public WindowInfo SelectedWindow { get; private set; }

	public bool IsWindow { get; private set; }

	public InspectWindowForm()
	{
		InitializeComponent();
		rtbInfo.AddContextMenu();
		ShareXResources.ApplyTheme(this);
		SelectHandle(isWindow: true);
	}

	private bool SelectHandle(bool isWindow)
	{
		RegionCaptureOptions options = new RegionCaptureOptions
		{
			DetectControls = !isWindow
		};
		SelectedWindow = null;
		SimpleWindowInfo windowInfo = RegionCaptureTasks.GetWindowInfo(options);
		if (windowInfo != null)
		{
			SelectedWindow = new WindowInfo(windowInfo.Handle);
			IsWindow = isWindow;
			UpdateWindowInfo();
			return true;
		}
		return false;
	}

	private void UpdateWindowInfo()
	{
		btnPinToTop.Enabled = SelectedWindow != null && IsWindow;
		rtbInfo.ResetText();
		if (SelectedWindow != null)
		{
			try
			{
				AddInfo(Resources.InspectWindow_WindowHandle, SelectedWindow.Handle.ToString("X8"));
				AddInfo(Resources.InspectWindow_WindowTitle, SelectedWindow.Text);
				AddInfo(Resources.InspectWindow_ClassName, SelectedWindow.ClassName);
				AddInfo(Resources.InspectWindow_ProcessName, SelectedWindow.ProcessName);
				AddInfo(Resources.InspectWindow_ProcessFileName, SelectedWindow.ProcessFileName);
				AddInfo(Resources.InspectWindow_ProcessIdentifier, SelectedWindow.ProcessId.ToString());
				AddInfo(Resources.InspectWindow_WindowRectangle, SelectedWindow.Rectangle.ToStringProper());
				AddInfo(Resources.InspectWindow_ClientRectangle, SelectedWindow.ClientRectangle.ToStringProper());
				AddInfo(Resources.InspectWindow_WindowStyles, SelectedWindow.Style.ToString().Replace(", ", "\r\n"));
				AddInfo(Resources.InspectWindow_ExtendedWindowStyles, SelectedWindow.ExStyle.ToString().Replace(", ", "\r\n"));
			}
			catch
			{
			}
		}
	}

	private void AddInfo(string name, string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (rtbInfo.TextLength > 0)
			{
				rtbInfo.AppendLine();
				rtbInfo.AppendLine();
			}
			rtbInfo.SetFontBold();
			rtbInfo.AppendLine(name);
			rtbInfo.SetFontRegular();
			rtbInfo.AppendText(value);
		}
	}

	private void btnInspectWindow_Click(object sender, EventArgs e)
	{
		SelectHandle(isWindow: true);
	}

	private void btnInspectControl_Click(object sender, EventArgs e)
	{
		SelectHandle(isWindow: false);
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		UpdateWindowInfo();
	}

	private void btnPinToTop_Click(object sender, EventArgs e)
	{
		if (SelectedWindow != null)
		{
			WindowInfo windowInfo = new WindowInfo(SelectedWindow.Handle);
			windowInfo.TopMost = !windowInfo.TopMost;
			UpdateWindowInfo();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.InspectWindowForm));
		this.rtbInfo = new System.Windows.Forms.RichTextBox();
		this.pInfo = new System.Windows.Forms.Panel();
		this.btnInspectWindow = new System.Windows.Forms.Button();
		this.btnInspectControl = new System.Windows.Forms.Button();
		this.btnRefresh = new System.Windows.Forms.Button();
		this.btnPinToTop = new System.Windows.Forms.Button();
		this.pInfo.SuspendLayout();
		base.SuspendLayout();
		this.rtbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.rtbInfo.DetectUrls = false;
		componentResourceManager.ApplyResources(this.rtbInfo, "rtbInfo");
		this.rtbInfo.Name = "rtbInfo";
		this.rtbInfo.ReadOnly = true;
		componentResourceManager.ApplyResources(this.pInfo, "pInfo");
		this.pInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pInfo.Controls.Add(this.rtbInfo);
		this.pInfo.Name = "pInfo";
		componentResourceManager.ApplyResources(this.btnInspectWindow, "btnInspectWindow");
		this.btnInspectWindow.Name = "btnInspectWindow";
		this.btnInspectWindow.UseVisualStyleBackColor = true;
		this.btnInspectWindow.Click += new System.EventHandler(btnInspectWindow_Click);
		componentResourceManager.ApplyResources(this.btnInspectControl, "btnInspectControl");
		this.btnInspectControl.Name = "btnInspectControl";
		this.btnInspectControl.UseVisualStyleBackColor = true;
		this.btnInspectControl.Click += new System.EventHandler(btnInspectControl_Click);
		componentResourceManager.ApplyResources(this.btnRefresh, "btnRefresh");
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.UseVisualStyleBackColor = true;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		componentResourceManager.ApplyResources(this.btnPinToTop, "btnPinToTop");
		this.btnPinToTop.Name = "btnPinToTop";
		this.btnPinToTop.UseVisualStyleBackColor = true;
		this.btnPinToTop.Click += new System.EventHandler(btnPinToTop_Click);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnPinToTop);
		base.Controls.Add(this.btnRefresh);
		base.Controls.Add(this.btnInspectControl);
		base.Controls.Add(this.btnInspectWindow);
		base.Controls.Add(this.pInfo);
		base.Name = "InspectWindowForm";
		this.pInfo.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
