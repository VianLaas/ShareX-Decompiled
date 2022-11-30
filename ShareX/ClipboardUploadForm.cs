using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class ClipboardUploadForm : Form
{
	private TaskSettings taskSettings;

	private IContainer components;

	private Label lblQuestion;

	private Button btnCancel;

	public TextBox txtClipboard;

	private ListBox lbClipboard;

	private CheckBox cbDontShowThisWindow;

	private MyPictureBox pbClipboard;

	private Button btnUpload;

	public bool IsClipboardContentValid { get; private set; }

	public bool DontShowThisWindow { get; private set; }

	public object ClipboardContent { get; private set; }

	public bool KeepClipboardContent { get; private set; }

	public ClipboardUploadForm(TaskSettings taskSettings, bool showCheckBox = false)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		this.taskSettings = taskSettings;
		if (ShareXResources.UseCustomTheme)
		{
			lblQuestion.BackColor = ShareXResources.Theme.BorderColor;
		}
		cbDontShowThisWindow.Visible = showCheckBox;
		IsClipboardContentValid = CheckClipboardContent();
		btnUpload.Enabled = IsClipboardContentValid;
	}

	private bool CheckClipboardContent()
	{
		MyPictureBox myPictureBox = pbClipboard;
		TextBox textBox = txtClipboard;
		bool flag2 = (lbClipboard.Visible = false);
		bool visible = (textBox.Visible = flag2);
		myPictureBox.Visible = visible;
		if (ClipboardHelpers.ContainsImage())
		{
			using Bitmap bitmap = ClipboardHelpers.GetImage();
			if (bitmap != null)
			{
				ClipboardContent = bitmap.Clone();
				pbClipboard.LoadImage(bitmap);
				pbClipboard.Visible = true;
				lblQuestion.Text = string.Format(Resources.ClipboardContentViewer_ClipboardContentViewer_Load_Clipboard_content__Image__Size___0_x_1__, bitmap.Width, bitmap.Height);
				return true;
			}
		}
		else if (ClipboardHelpers.ContainsText())
		{
			string text = ClipboardHelpers.GetText();
			if (!string.IsNullOrEmpty(text))
			{
				ClipboardContent = text;
				txtClipboard.Text = text;
				txtClipboard.Visible = true;
				lblQuestion.Text = string.Format(Resources.ClipboardContentViewer_ClipboardContentViewer_Load_Clipboard_content__Text__Length___0__, text.Length);
				return true;
			}
		}
		else if (ClipboardHelpers.ContainsFileDropList())
		{
			string[] fileDropList = ClipboardHelpers.GetFileDropList();
			if (fileDropList != null && fileDropList.Length != 0)
			{
				ClipboardContent = fileDropList;
				ListBox.ObjectCollection items = lbClipboard.Items;
				object[] items2 = fileDropList;
				items.AddRange(items2);
				lbClipboard.Visible = true;
				lblQuestion.Text = string.Format(Resources.ClipboardContentViewer_ClipboardContentViewer_Load_Clipboard_content__File__Count___0__, fileDropList.Length);
				return true;
			}
		}
		lblQuestion.Text = Resources.ClipboardContentViewer_ClipboardContentViewer_Load_Clipboard_is_empty_or_contains_unknown_data_;
		return false;
	}

	private void ClipboardUpload()
	{
		if (!IsClipboardContentValid)
		{
			return;
		}
		object clipboardContent = ClipboardContent;
		if (!(clipboardContent is Bitmap bmp))
		{
			if (!(clipboardContent is string text))
			{
				if (clipboardContent is string[] files)
				{
					UploadManager.ProcessFilesUpload(files, taskSettings);
				}
			}
			else
			{
				UploadManager.ProcessTextUpload(text, taskSettings);
			}
		}
		else
		{
			KeepClipboardContent = true;
			UploadManager.ProcessImageUpload(bmp, taskSettings);
		}
	}

	private void ClipboardContentViewer_Shown(object sender, EventArgs e)
	{
		this.ForceActivate();
	}

	private void ClipboardUploadForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (!KeepClipboardContent && ClipboardContent is Bitmap bitmap)
		{
			bitmap.Dispose();
		}
	}

	private void cbDontShowThisWindow_CheckedChanged(object sender, EventArgs e)
	{
		DontShowThisWindow = cbDontShowThisWindow.Checked;
	}

	private void btnUpload_Click(object sender, EventArgs e)
	{
		ClipboardUpload();
		Close();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.ClipboardUploadForm));
		this.lblQuestion = new System.Windows.Forms.Label();
		this.btnCancel = new System.Windows.Forms.Button();
		this.txtClipboard = new System.Windows.Forms.TextBox();
		this.lbClipboard = new System.Windows.Forms.ListBox();
		this.cbDontShowThisWindow = new System.Windows.Forms.CheckBox();
		this.pbClipboard = new ShareX.HelpersLib.MyPictureBox();
		this.btnUpload = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.lblQuestion.BackColor = System.Drawing.Color.RoyalBlue;
		componentResourceManager.ApplyResources(this.lblQuestion, "lblQuestion");
		this.lblQuestion.ForeColor = System.Drawing.Color.White;
		this.lblQuestion.Name = "lblQuestion";
		componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		componentResourceManager.ApplyResources(this.txtClipboard, "txtClipboard");
		this.txtClipboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtClipboard.Name = "txtClipboard";
		this.txtClipboard.ReadOnly = true;
		componentResourceManager.ApplyResources(this.lbClipboard, "lbClipboard");
		this.lbClipboard.FormattingEnabled = true;
		this.lbClipboard.Name = "lbClipboard";
		componentResourceManager.ApplyResources(this.cbDontShowThisWindow, "cbDontShowThisWindow");
		this.cbDontShowThisWindow.Name = "cbDontShowThisWindow";
		this.cbDontShowThisWindow.UseVisualStyleBackColor = true;
		this.cbDontShowThisWindow.CheckedChanged += new System.EventHandler(cbDontShowThisWindow_CheckedChanged);
		componentResourceManager.ApplyResources(this.pbClipboard, "pbClipboard");
		this.pbClipboard.BackColor = System.Drawing.SystemColors.Window;
		this.pbClipboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pbClipboard.DrawCheckeredBackground = true;
		this.pbClipboard.FullscreenOnClick = true;
		this.pbClipboard.Name = "pbClipboard";
		this.pbClipboard.PictureBoxBackColor = System.Drawing.SystemColors.Window;
		this.pbClipboard.ShowImageSizeLabel = true;
		componentResourceManager.ApplyResources(this.btnUpload, "btnUpload");
		this.btnUpload.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btnUpload.Name = "btnUpload";
		this.btnUpload.UseVisualStyleBackColor = true;
		this.btnUpload.Click += new System.EventHandler(btnUpload_Click);
		base.AcceptButton = this.btnUpload;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnCancel;
		base.Controls.Add(this.btnUpload);
		base.Controls.Add(this.pbClipboard);
		base.Controls.Add(this.cbDontShowThisWindow);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.lblQuestion);
		base.Controls.Add(this.txtClipboard);
		base.Controls.Add(this.lbClipboard);
		base.Name = "ClipboardUploadForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ClipboardUploadForm_FormClosed);
		base.Shown += new System.EventHandler(ClipboardContentViewer_Shown);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
