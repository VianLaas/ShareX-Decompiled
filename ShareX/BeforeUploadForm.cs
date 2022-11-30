using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class BeforeUploadForm : Form
{
	private IContainer components;

	private BeforeUploadControl ucBeforeUpload;

	private Button btnOK;

	private Button btnCancel;

	private Label lblTitle;

	private MyPictureBox pbPreview;

	public BeforeUploadForm(TaskInfo info)
	{
		BeforeUploadForm beforeUploadForm = this;
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		base.DialogResult = DialogResult.OK;
		ucBeforeUpload.InitCompleted += delegate(string currentDestination)
		{
			string format = (string.IsNullOrEmpty(currentDestination) ? Resources.BeforeUploadForm_BeforeUploadForm_Please_choose_a_destination_ : Resources.BeforeUploadForm_BeforeUploadForm__0__is_about_to_be_uploaded_to__1___You_may_choose_a_different_destination_);
			beforeUploadForm.lblTitle.Text = string.Format(format, info.FileName, currentDestination);
			beforeUploadForm.pbPreview.LoadImageFromFileAsync(info.FilePath);
		};
		ucBeforeUpload.Init(info);
	}

	private void BeforeUploadForm_Shown(object sender, EventArgs e)
	{
		this.ForceActivate();
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.BeforeUploadForm));
		this.btnOK = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.lblTitle = new System.Windows.Forms.Label();
		this.pbPreview = new ShareX.HelpersLib.MyPictureBox();
		this.ucBeforeUpload = new ShareX.BeforeUploadControl();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.btnOK, "btnOK");
		this.btnOK.Name = "btnOK";
		this.btnOK.UseVisualStyleBackColor = true;
		this.btnOK.Click += new System.EventHandler(btnOK_Click);
		componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		componentResourceManager.ApplyResources(this.lblTitle, "lblTitle");
		this.lblTitle.Name = "lblTitle";
		componentResourceManager.ApplyResources(this.pbPreview, "pbPreview");
		this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pbPreview.DrawCheckeredBackground = true;
		this.pbPreview.EnableRightClickMenu = true;
		this.pbPreview.FullscreenOnClick = true;
		this.pbPreview.Name = "pbPreview";
		this.pbPreview.ShowImageSizeLabel = true;
		componentResourceManager.ApplyResources(this.ucBeforeUpload, "ucBeforeUpload");
		this.ucBeforeUpload.Name = "ucBeforeUpload";
		base.AcceptButton = this.btnOK;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnCancel;
		base.Controls.Add(this.pbPreview);
		base.Controls.Add(this.lblTitle);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOK);
		base.Controls.Add(this.ucBeforeUpload);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "BeforeUploadForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.TopMost = true;
		base.Shown += new System.EventHandler(BeforeUploadForm_Shown);
		base.ResumeLayout(false);
	}
}
