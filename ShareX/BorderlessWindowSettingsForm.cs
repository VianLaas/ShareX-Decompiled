using System;
using System.ComponentModel;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class BorderlessWindowSettingsForm : Form
{
	private IContainer components;

	private CheckBox cbRememberWindowTitle;

	private CheckBox cbAutoCloseWindow;

	private CheckBox cbExcludeTaskbarArea;

	public BorderlessWindowSettings Settings { get; private set; }

	public BorderlessWindowSettingsForm(BorderlessWindowSettings settings)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		Settings = settings;
		cbRememberWindowTitle.Checked = Settings.RememberWindowTitle;
		cbAutoCloseWindow.Checked = Settings.AutoCloseWindow;
		cbExcludeTaskbarArea.Checked = Settings.ExcludeTaskbarArea;
	}

	private void cbRememberWindowTitle_CheckedChanged(object sender, EventArgs e)
	{
		Settings.RememberWindowTitle = cbRememberWindowTitle.Checked;
	}

	private void cbAutoCloseWindow_CheckedChanged(object sender, EventArgs e)
	{
		Settings.AutoCloseWindow = cbAutoCloseWindow.Checked;
	}

	private void cbExcludeTaskbarArea_CheckedChanged(object sender, EventArgs e)
	{
		Settings.ExcludeTaskbarArea = cbExcludeTaskbarArea.Checked;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.BorderlessWindowSettingsForm));
		this.cbRememberWindowTitle = new System.Windows.Forms.CheckBox();
		this.cbAutoCloseWindow = new System.Windows.Forms.CheckBox();
		this.cbExcludeTaskbarArea = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.cbRememberWindowTitle, "cbRememberWindowTitle");
		this.cbRememberWindowTitle.Name = "cbRememberWindowTitle";
		this.cbRememberWindowTitle.UseVisualStyleBackColor = true;
		this.cbRememberWindowTitle.CheckedChanged += new System.EventHandler(cbRememberWindowTitle_CheckedChanged);
		componentResourceManager.ApplyResources(this.cbAutoCloseWindow, "cbAutoCloseWindow");
		this.cbAutoCloseWindow.Name = "cbAutoCloseWindow";
		this.cbAutoCloseWindow.UseVisualStyleBackColor = true;
		this.cbAutoCloseWindow.CheckedChanged += new System.EventHandler(cbAutoCloseWindow_CheckedChanged);
		componentResourceManager.ApplyResources(this.cbExcludeTaskbarArea, "cbExcludeTaskbarArea");
		this.cbExcludeTaskbarArea.Name = "cbExcludeTaskbarArea";
		this.cbExcludeTaskbarArea.UseVisualStyleBackColor = true;
		this.cbExcludeTaskbarArea.CheckedChanged += new System.EventHandler(cbExcludeTaskbarArea_CheckedChanged);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.cbExcludeTaskbarArea);
		base.Controls.Add(this.cbAutoCloseWindow);
		base.Controls.Add(this.cbRememberWindowTitle);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "BorderlessWindowSettingsForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
