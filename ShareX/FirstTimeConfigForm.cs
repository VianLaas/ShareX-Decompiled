using System;
using System.ComponentModel;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class FirstTimeConfigForm : Form
{
	private bool loaded;

	private IContainer components;

	private CheckBox cbRunStartup;

	private CheckBox cbShellContextMenuButton;

	private CheckBox cbSendToMenu;

	private CheckBox cbSteamInApp;

	private Button btnOK;

	private Label lblNote;

	private Label lblTitle;

	public FirstTimeConfigForm()
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		StartupState startupState = StartupManagerSingletonProvider.CurrentStartupManager.State;
		cbRunStartup.Checked = startupState == StartupState.Enabled || startupState == StartupState.EnabledByPolicy;
		cbRunStartup.Enabled = startupState != StartupState.DisabledByUser && startupState != StartupState.DisabledByPolicy && startupState != StartupState.EnabledByPolicy;
		cbShellContextMenuButton.Checked = IntegrationHelpers.CheckShellContextMenuButton();
		cbSendToMenu.Checked = IntegrationHelpers.CheckSendToMenuButton();
		cbSteamInApp.Visible = false;
		loaded = true;
	}

	private void btnOK_MouseClick(object sender, MouseEventArgs e)
	{
		Close();
	}

	private void cbRunStartup_CheckedChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			StartupManagerSingletonProvider.CurrentStartupManager.State = (cbRunStartup.Checked ? StartupState.Enabled : StartupState.Disabled);
		}
	}

	private void cbShellContextMenuButton_CheckedChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			IntegrationHelpers.CreateShellContextMenuButton(cbShellContextMenuButton.Checked);
		}
	}

	private void cbSendToMenu_CheckedChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			IntegrationHelpers.CreateSendToMenuButton(cbSendToMenu.Checked);
		}
	}

	private void cbSteamInApp_CheckedChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			IntegrationHelpers.SteamShowInApp(cbSteamInApp.Checked);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.FirstTimeConfigForm));
		this.cbRunStartup = new System.Windows.Forms.CheckBox();
		this.cbShellContextMenuButton = new System.Windows.Forms.CheckBox();
		this.cbSendToMenu = new System.Windows.Forms.CheckBox();
		this.cbSteamInApp = new System.Windows.Forms.CheckBox();
		this.btnOK = new System.Windows.Forms.Button();
		this.lblNote = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.cbRunStartup, "cbRunStartup");
		this.cbRunStartup.Name = "cbRunStartup";
		this.cbRunStartup.UseVisualStyleBackColor = false;
		this.cbRunStartup.CheckedChanged += new System.EventHandler(cbRunStartup_CheckedChanged);
		componentResourceManager.ApplyResources(this.cbShellContextMenuButton, "cbShellContextMenuButton");
		this.cbShellContextMenuButton.Name = "cbShellContextMenuButton";
		this.cbShellContextMenuButton.UseVisualStyleBackColor = false;
		this.cbShellContextMenuButton.CheckedChanged += new System.EventHandler(cbShellContextMenuButton_CheckedChanged);
		componentResourceManager.ApplyResources(this.cbSendToMenu, "cbSendToMenu");
		this.cbSendToMenu.Name = "cbSendToMenu";
		this.cbSendToMenu.UseVisualStyleBackColor = false;
		this.cbSendToMenu.CheckedChanged += new System.EventHandler(cbSendToMenu_CheckedChanged);
		componentResourceManager.ApplyResources(this.cbSteamInApp, "cbSteamInApp");
		this.cbSteamInApp.Name = "cbSteamInApp";
		this.cbSteamInApp.UseVisualStyleBackColor = false;
		this.cbSteamInApp.CheckedChanged += new System.EventHandler(cbSteamInApp_CheckedChanged);
		componentResourceManager.ApplyResources(this.btnOK, "btnOK");
		this.btnOK.Name = "btnOK";
		this.btnOK.MouseClick += new System.Windows.Forms.MouseEventHandler(btnOK_MouseClick);
		componentResourceManager.ApplyResources(this.lblNote, "lblNote");
		this.lblNote.Name = "lblNote";
		componentResourceManager.ApplyResources(this.lblTitle, "lblTitle");
		this.lblTitle.Name = "lblTitle";
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.Controls.Add(this.lblTitle);
		base.Controls.Add(this.lblNote);
		base.Controls.Add(this.btnOK);
		base.Controls.Add(this.cbSteamInApp);
		base.Controls.Add(this.cbSendToMenu);
		base.Controls.Add(this.cbShellContextMenuButton);
		base.Controls.Add(this.cbRunStartup);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.Name = "FirstTimeConfigForm";
		base.TopMost = true;
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
