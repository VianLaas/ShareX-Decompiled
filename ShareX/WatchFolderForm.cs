using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class WatchFolderForm : Form
{
	private IContainer components;

	private Button btnPathBrowse;

	private TextBox txtFilter;

	private TextBox txtFolderPath;

	private Label lblFilter;

	private Label lblFolderPath;

	private Label lblFilterExample;

	private CheckBox cbIncludeSubdirectories;

	private Button btnCancel;

	private Button btnOK;

	private CheckBox cbMoveToScreenshotsFolder;

	public WatchFolderSettings WatchFolder { get; private set; }

	public WatchFolderForm()
		: this(new WatchFolderSettings())
	{
	}

	public WatchFolderForm(WatchFolderSettings watchFolder)
	{
		WatchFolder = watchFolder;
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		txtFolderPath.Text = watchFolder.FolderPath ?? "";
		txtFilter.Text = watchFolder.Filter ?? "";
		cbIncludeSubdirectories.Checked = watchFolder.IncludeSubdirectories;
		cbMoveToScreenshotsFolder.Checked = watchFolder.MoveFilesToScreenshotsFolder;
	}

	private void btnPathBrowse_Click(object sender, EventArgs e)
	{
		FileHelpers.BrowseFolder(txtFolderPath, "", detectSpecialFolders: true);
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
		WatchFolder.FolderPath = txtFolderPath.Text;
		WatchFolder.Filter = txtFilter.Text;
		WatchFolder.IncludeSubdirectories = cbIncludeSubdirectories.Checked;
		WatchFolder.MoveFilesToScreenshotsFolder = cbMoveToScreenshotsFolder.Checked;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.WatchFolderForm));
		this.btnPathBrowse = new System.Windows.Forms.Button();
		this.txtFilter = new System.Windows.Forms.TextBox();
		this.txtFolderPath = new System.Windows.Forms.TextBox();
		this.lblFilter = new System.Windows.Forms.Label();
		this.lblFolderPath = new System.Windows.Forms.Label();
		this.lblFilterExample = new System.Windows.Forms.Label();
		this.cbIncludeSubdirectories = new System.Windows.Forms.CheckBox();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnOK = new System.Windows.Forms.Button();
		this.cbMoveToScreenshotsFolder = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.btnPathBrowse, "btnPathBrowse");
		this.btnPathBrowse.Name = "btnPathBrowse";
		this.btnPathBrowse.UseVisualStyleBackColor = true;
		this.btnPathBrowse.Click += new System.EventHandler(btnPathBrowse_Click);
		componentResourceManager.ApplyResources(this.txtFilter, "txtFilter");
		this.txtFilter.Name = "txtFilter";
		componentResourceManager.ApplyResources(this.txtFolderPath, "txtFolderPath");
		this.txtFolderPath.Name = "txtFolderPath";
		componentResourceManager.ApplyResources(this.lblFilter, "lblFilter");
		this.lblFilter.Name = "lblFilter";
		componentResourceManager.ApplyResources(this.lblFolderPath, "lblFolderPath");
		this.lblFolderPath.Name = "lblFolderPath";
		componentResourceManager.ApplyResources(this.lblFilterExample, "lblFilterExample");
		this.lblFilterExample.Name = "lblFilterExample";
		componentResourceManager.ApplyResources(this.cbIncludeSubdirectories, "cbIncludeSubdirectories");
		this.cbIncludeSubdirectories.Name = "cbIncludeSubdirectories";
		this.cbIncludeSubdirectories.UseVisualStyleBackColor = true;
		componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		componentResourceManager.ApplyResources(this.btnOK, "btnOK");
		this.btnOK.Name = "btnOK";
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		componentResourceManager.ApplyResources(this.cbMoveToScreenshotsFolder, "cbMoveToScreenshotsFolder");
		this.cbMoveToScreenshotsFolder.Name = "cbMoveToScreenshotsFolder";
		this.cbMoveToScreenshotsFolder.UseVisualStyleBackColor = true;
		base.AcceptButton = this.btnOK;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnCancel;
		base.Controls.Add(this.cbMoveToScreenshotsFolder);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOK);
		base.Controls.Add(this.cbIncludeSubdirectories);
		base.Controls.Add(this.lblFilterExample);
		base.Controls.Add(this.btnPathBrowse);
		base.Controls.Add(this.txtFilter);
		base.Controls.Add(this.txtFolderPath);
		base.Controls.Add(this.lblFilter);
		base.Controls.Add(this.lblFolderPath);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.Name = "WatchFolderForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
