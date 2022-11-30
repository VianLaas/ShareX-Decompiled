using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class FirstTimeUploadForm : Form
{
	private int countdown = 5;

	private string textYes;

	private IContainer components;

	private Label lblInfo;

	private Button btnYes;

	private Button btnNo;

	private Label lblHeader;

	private Timer tCountdown;

	public FirstTimeUploadForm()
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		lblHeader.BackColor = Color.DarkRed;
		lblHeader.ForeColor = Color.WhiteSmoke;
		btnYes.Enabled = false;
		textYes = btnYes.Text;
		UpdateCountdown();
		tCountdown.Start();
	}

	private void UpdateCountdown()
	{
		if (countdown < 1)
		{
			btnYes.Text = textYes;
			btnYes.Enabled = true;
			tCountdown.Stop();
		}
		else
		{
			btnYes.Text = textYes + " (" + countdown + ")";
			countdown--;
		}
	}

	private void FirstTimeUploadForm_Shown(object sender, EventArgs e)
	{
		this.ForceActivate();
	}

	private void tCountdown_Tick(object sender, EventArgs e)
	{
		if (!base.IsDisposed && NativeMethods.IsActive(base.Handle))
		{
			UpdateCountdown();
		}
	}

	private void btnYes_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Yes;
		Close();
	}

	private void btnNo_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.No;
		Close();
	}

	public static bool ShowForm()
	{
		using FirstTimeUploadForm firstTimeUploadForm = new FirstTimeUploadForm();
		return firstTimeUploadForm.ShowDialog() == DialogResult.Yes;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.FirstTimeUploadForm));
		this.lblInfo = new System.Windows.Forms.Label();
		this.btnYes = new System.Windows.Forms.Button();
		this.btnNo = new System.Windows.Forms.Button();
		this.lblHeader = new System.Windows.Forms.Label();
		this.tCountdown = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.lblInfo, "lblInfo");
		this.lblInfo.Name = "lblInfo";
		this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
		componentResourceManager.ApplyResources(this.btnYes, "btnYes");
		this.btnYes.Name = "btnYes";
		this.btnYes.UseVisualStyleBackColor = true;
		this.btnYes.Click += new System.EventHandler(btnYes_Click);
		this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
		componentResourceManager.ApplyResources(this.btnNo, "btnNo");
		this.btnNo.Name = "btnNo";
		this.btnNo.UseVisualStyleBackColor = true;
		this.btnNo.Click += new System.EventHandler(btnNo_Click);
		componentResourceManager.ApplyResources(this.lblHeader, "lblHeader");
		this.lblHeader.Name = "lblHeader";
		this.tCountdown.Interval = 1000;
		this.tCountdown.Tick += new System.EventHandler(tCountdown_Tick);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.btnNo;
		base.Controls.Add(this.lblHeader);
		base.Controls.Add(this.btnNo);
		base.Controls.Add(this.btnYes);
		base.Controls.Add(this.lblInfo);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "FirstTimeUploadForm";
		base.Shown += new System.EventHandler(FirstTimeUploadForm_Shown);
		base.ResumeLayout(false);
	}
}
