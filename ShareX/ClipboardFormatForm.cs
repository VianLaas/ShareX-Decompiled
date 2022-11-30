using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class ClipboardFormatForm : Form
{
	private IContainer components;

	private TextBox txtFormat;

	private TextBox txtDescription;

	private Label lblFilter;

	private Label lblFolderPath;

	private Label lblExample;

	private Button btnCancel;

	private Button btnOK;

	public ClipboardFormat ClipboardFormat { get; private set; }

	public ClipboardFormatForm()
		: this(new ClipboardFormat())
	{
	}

	public ClipboardFormatForm(ClipboardFormat cbf)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		ClipboardFormat = cbf;
		txtDescription.Text = cbf.Description ?? "";
		txtFormat.Text = cbf.Format ?? "";
		CodeMenu.Create(txtFormat, Array.Empty<CodeMenuEntryFilename>());
		lblExample.Text = string.Format(Resources.ClipboardFormatForm_ClipboardFormatForm_Supported_variables___0__and_other_variables_such_as__1__etc_, "$result, $url, $shorturl, $thumbnailurl, $deletionurl, $filepath, $filename, $filenamenoext, $thumbnailfilename, $thumbnailfilenamenoext, $folderpath, $foldername, $uploadtime", "%y, %mo, %d");
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
		ClipboardFormat.Description = txtDescription.Text;
		ClipboardFormat.Format = txtFormat.Text;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.ClipboardFormatForm));
		this.txtFormat = new System.Windows.Forms.TextBox();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.lblFilter = new System.Windows.Forms.Label();
		this.lblFolderPath = new System.Windows.Forms.Label();
		this.lblExample = new System.Windows.Forms.Label();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnOK = new System.Windows.Forms.Button();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.txtFormat, "txtFormat");
		this.txtFormat.Name = "txtFormat";
		componentResourceManager.ApplyResources(this.txtDescription, "txtDescription");
		this.txtDescription.Name = "txtDescription";
		componentResourceManager.ApplyResources(this.lblFilter, "lblFilter");
		this.lblFilter.Name = "lblFilter";
		componentResourceManager.ApplyResources(this.lblFolderPath, "lblFolderPath");
		this.lblFolderPath.Name = "lblFolderPath";
		componentResourceManager.ApplyResources(this.lblExample, "lblExample");
		this.lblExample.Name = "lblExample";
		componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		componentResourceManager.ApplyResources(this.btnOK, "btnOK");
		this.btnOK.Name = "btnOK";
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		base.AcceptButton = this.btnOK;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnCancel;
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOK);
		base.Controls.Add(this.lblExample);
		base.Controls.Add(this.txtFormat);
		base.Controls.Add(this.txtDescription);
		base.Controls.Add(this.lblFilter);
		base.Controls.Add(this.lblFolderPath);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "ClipboardFormatForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
