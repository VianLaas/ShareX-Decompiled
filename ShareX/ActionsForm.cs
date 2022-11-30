using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class ActionsForm : Form
{
	private IContainer components;

	private Label lblName;

	private Label lblPath;

	private Label lblArgs;

	private TextBox txtName;

	private TextBox txtPath;

	private TextBox txtArguments;

	private Button btnPathBrowse;

	private Button btnOK;

	private Button btnCancel;

	private Label lblExtensions;

	private TextBox txtExtensions;

	private TextBox txtOutputExtension;

	private Label lblOutputExtension;

	private CheckBox cbHiddenWindow;

	private CheckBox cbDeleteInputFile;

	public ExternalProgram FileAction { get; private set; }

	public ActionsForm()
		: this(new ExternalProgram())
	{
	}

	public ActionsForm(ExternalProgram fileAction)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		FileAction = fileAction;
		txtName.Text = fileAction.Name ?? "";
		txtPath.Text = fileAction.Path ?? "";
		txtArguments.Text = fileAction.Args ?? "";
		CodeMenu.Create(txtArguments, Array.Empty<CodeMenuEntryActions>());
		txtOutputExtension.Text = fileAction.OutputExtension ?? "";
		txtExtensions.Text = fileAction.Extensions ?? "";
		cbHiddenWindow.Checked = fileAction.HiddenWindow;
		cbDeleteInputFile.Checked = fileAction.DeleteInputFile;
	}

	private void btnPathBrowse_Click(object sender, EventArgs e)
	{
		FileHelpers.BrowseFile(txtPath, "", detectSpecialFolders: true);
	}

	private void txtOutputExtension_TextChanged(object sender, EventArgs e)
	{
		cbDeleteInputFile.Enabled = txtOutputExtension.TextLength > 0;
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtName.Text))
		{
			MessageBox.Show(Resources.ActionsForm_btnOK_Click_Name_can_t_be_empty_, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (string.IsNullOrEmpty(txtPath.Text))
		{
			MessageBox.Show(Resources.ActionsForm_btnOK_Click_File_path_can_t_be_empty_, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		FileAction.Name = txtName.Text;
		FileAction.Path = txtPath.Text;
		FileAction.Args = txtArguments.Text;
		FileAction.Extensions = txtExtensions.Text;
		FileAction.OutputExtension = txtOutputExtension.Text;
		FileAction.HiddenWindow = cbHiddenWindow.Checked;
		FileAction.DeleteInputFile = cbDeleteInputFile.Checked;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.ActionsForm));
		this.lblName = new System.Windows.Forms.Label();
		this.lblPath = new System.Windows.Forms.Label();
		this.lblArgs = new System.Windows.Forms.Label();
		this.txtName = new System.Windows.Forms.TextBox();
		this.txtPath = new System.Windows.Forms.TextBox();
		this.txtArguments = new System.Windows.Forms.TextBox();
		this.btnPathBrowse = new System.Windows.Forms.Button();
		this.btnOK = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.lblExtensions = new System.Windows.Forms.Label();
		this.txtExtensions = new System.Windows.Forms.TextBox();
		this.txtOutputExtension = new System.Windows.Forms.TextBox();
		this.lblOutputExtension = new System.Windows.Forms.Label();
		this.cbHiddenWindow = new System.Windows.Forms.CheckBox();
		this.cbDeleteInputFile = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.lblName, "lblName");
		this.lblName.Name = "lblName";
		componentResourceManager.ApplyResources(this.lblPath, "lblPath");
		this.lblPath.Name = "lblPath";
		componentResourceManager.ApplyResources(this.lblArgs, "lblArgs");
		this.lblArgs.Name = "lblArgs";
		componentResourceManager.ApplyResources(this.txtName, "txtName");
		this.txtName.Name = "txtName";
		componentResourceManager.ApplyResources(this.txtPath, "txtPath");
		this.txtPath.Name = "txtPath";
		componentResourceManager.ApplyResources(this.txtArguments, "txtArguments");
		this.txtArguments.Name = "txtArguments";
		componentResourceManager.ApplyResources(this.btnPathBrowse, "btnPathBrowse");
		this.btnPathBrowse.Name = "btnPathBrowse";
		this.btnPathBrowse.UseVisualStyleBackColor = true;
		this.btnPathBrowse.Click += new System.EventHandler(btnPathBrowse_Click);
		componentResourceManager.ApplyResources(this.btnOK, "btnOK");
		this.btnOK.Name = "btnOK";
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		componentResourceManager.ApplyResources(this.lblExtensions, "lblExtensions");
		this.lblExtensions.Name = "lblExtensions";
		componentResourceManager.ApplyResources(this.txtExtensions, "txtExtensions");
		this.txtExtensions.Name = "txtExtensions";
		componentResourceManager.ApplyResources(this.txtOutputExtension, "txtOutputExtension");
		this.txtOutputExtension.Name = "txtOutputExtension";
		this.txtOutputExtension.TextChanged += new System.EventHandler(txtOutputExtension_TextChanged);
		componentResourceManager.ApplyResources(this.lblOutputExtension, "lblOutputExtension");
		this.lblOutputExtension.Name = "lblOutputExtension";
		componentResourceManager.ApplyResources(this.cbHiddenWindow, "cbHiddenWindow");
		this.cbHiddenWindow.Name = "cbHiddenWindow";
		this.cbHiddenWindow.UseVisualStyleBackColor = true;
		componentResourceManager.ApplyResources(this.cbDeleteInputFile, "cbDeleteInputFile");
		this.cbDeleteInputFile.Name = "cbDeleteInputFile";
		this.cbDeleteInputFile.UseVisualStyleBackColor = true;
		base.AcceptButton = this.btnOK;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnCancel;
		base.Controls.Add(this.cbDeleteInputFile);
		base.Controls.Add(this.cbHiddenWindow);
		base.Controls.Add(this.lblOutputExtension);
		base.Controls.Add(this.txtOutputExtension);
		base.Controls.Add(this.txtExtensions);
		base.Controls.Add(this.lblExtensions);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOK);
		base.Controls.Add(this.btnPathBrowse);
		base.Controls.Add(this.txtArguments);
		base.Controls.Add(this.txtPath);
		base.Controls.Add(this.txtName);
		base.Controls.Add(this.lblArgs);
		base.Controls.Add(this.lblPath);
		base.Controls.Add(this.lblName);
		base.Name = "ActionsForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
