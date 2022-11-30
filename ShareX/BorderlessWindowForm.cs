using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;

namespace ShareX;

public class BorderlessWindowForm : Form
{
	private IContainer components;

	private Label lblWindowTitle;

	private TextBox txtWindowTitle;

	private Button btnMakeWindowBorderless;

	private Button btnSettings;

	private MenuButton mbWindowList;

	private ContextMenuStrip cmsWindowList;

	public BorderlessWindowSettings Settings { get; private set; }

	public BorderlessWindowForm(BorderlessWindowSettings settings)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		Settings = settings;
	}

	private void UpdateWindowListMenu()
	{
		cmsWindowList.Items.Clear();
		List<WindowInfo> visibleWindowsList = new WindowsList(base.Handle).GetVisibleWindowsList();
		if (visibleWindowsList == null || visibleWindowsList.Count <= 0)
		{
			return;
		}
		List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
		string title;
		foreach (WindowInfo item in visibleWindowsList)
		{
			try
			{
				title = item.Text;
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(title.Truncate(50, "..."));
				toolStripMenuItem.Click += delegate
				{
					txtWindowTitle.Text = title;
				};
				using (Icon icon = item.Icon)
				{
					if (icon != null && icon.Width > 0 && icon.Height > 0)
					{
						toolStripMenuItem.Image = icon.ToBitmap();
					}
				}
				list.Add(toolStripMenuItem);
			}
			catch (Exception exception)
			{
				DebugHelper.WriteException(exception);
			}
		}
		ToolStripItemCollection items = cmsWindowList.Items;
		ToolStripItem[] toolStripItems = list.OrderBy((ToolStripMenuItem x) => x.Text).ToArray();
		items.AddRange(toolStripItems);
	}

	private void BorderlessWindowForm_Shown(object sender, EventArgs e)
	{
		if (Settings.RememberWindowTitle && !string.IsNullOrEmpty(Settings.WindowTitle))
		{
			txtWindowTitle.Text = Settings.WindowTitle;
			btnMakeWindowBorderless.Focus();
		}
	}

	private void mbWindowList_MouseDown(object sender, MouseEventArgs e)
	{
		UpdateWindowListMenu();
	}

	private void txtWindowTitle_TextChanged(object sender, EventArgs e)
	{
		btnMakeWindowBorderless.Enabled = !string.IsNullOrEmpty(txtWindowTitle.Text);
	}

	private void btnMakeWindowBorderless_Click(object sender, EventArgs e)
	{
		try
		{
			string windowTitle = txtWindowTitle.Text;
			if (Settings.RememberWindowTitle)
			{
				Settings.WindowTitle = windowTitle;
			}
			else
			{
				Settings.WindowTitle = "";
			}
			if (BorderlessWindowManager.MakeWindowBorderless(windowTitle, Settings.ExcludeTaskbarArea) && Settings.AutoCloseWindow)
			{
				Close();
			}
		}
		catch (Exception e2)
		{
			e2.ShowError();
		}
	}

	private void btnSettings_Click(object sender, EventArgs e)
	{
		using BorderlessWindowSettingsForm borderlessWindowSettingsForm = new BorderlessWindowSettingsForm(Settings);
		borderlessWindowSettingsForm.ShowDialog();
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.BorderlessWindowForm));
		this.lblWindowTitle = new System.Windows.Forms.Label();
		this.txtWindowTitle = new System.Windows.Forms.TextBox();
		this.btnMakeWindowBorderless = new System.Windows.Forms.Button();
		this.btnSettings = new System.Windows.Forms.Button();
		this.cmsWindowList = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.mbWindowList = new ShareX.HelpersLib.MenuButton();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.lblWindowTitle, "lblWindowTitle");
		this.lblWindowTitle.Name = "lblWindowTitle";
		componentResourceManager.ApplyResources(this.txtWindowTitle, "txtWindowTitle");
		this.txtWindowTitle.Name = "txtWindowTitle";
		this.txtWindowTitle.TextChanged += new System.EventHandler(txtWindowTitle_TextChanged);
		componentResourceManager.ApplyResources(this.btnMakeWindowBorderless, "btnMakeWindowBorderless");
		this.btnMakeWindowBorderless.Name = "btnMakeWindowBorderless";
		this.btnMakeWindowBorderless.UseVisualStyleBackColor = true;
		this.btnMakeWindowBorderless.Click += new System.EventHandler(btnMakeWindowBorderless_Click);
		componentResourceManager.ApplyResources(this.btnSettings, "btnSettings");
		this.btnSettings.Image = ShareX.Properties.Resources.gear;
		this.btnSettings.Name = "btnSettings";
		this.btnSettings.UseVisualStyleBackColor = true;
		this.btnSettings.Click += new System.EventHandler(btnSettings_Click);
		this.cmsWindowList.Name = "cmsWindowList";
		componentResourceManager.ApplyResources(this.cmsWindowList, "cmsWindowList");
		componentResourceManager.ApplyResources(this.mbWindowList, "mbWindowList");
		this.mbWindowList.Menu = this.cmsWindowList;
		this.mbWindowList.Name = "mbWindowList";
		this.mbWindowList.UseVisualStyleBackColor = true;
		this.mbWindowList.MouseDown += new System.Windows.Forms.MouseEventHandler(mbWindowList_MouseDown);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.Controls.Add(this.mbWindowList);
		base.Controls.Add(this.btnSettings);
		base.Controls.Add(this.btnMakeWindowBorderless);
		base.Controls.Add(this.txtWindowTitle);
		base.Controls.Add(this.lblWindowTitle);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "BorderlessWindowForm";
		base.Shown += new System.EventHandler(BorderlessWindowForm_Shown);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
