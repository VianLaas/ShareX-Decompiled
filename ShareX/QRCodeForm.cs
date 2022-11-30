using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace ShareX;

public class QRCodeForm : Form
{
	private static QRCodeForm instance;

	private bool isReady;

	private IContainer components;

	private TextBox txtQRCode;

	private ContextMenuStrip cmsQR;

	private ToolStripMenuItem tsmiCopy;

	private ToolStripMenuItem tsmiSaveAs;

	private PictureBox pbQRCode;

	private TabControl tcMain;

	private TabPage tpEncode;

	private TabPage tpDecode;

	private Button btnDecodeFromScreen;

	private Label lblDecodeResult;

	private Button btnDecodeFromFile;

	private ToolStripMenuItem tsmiDecode;

	private ToolStripMenuItem tsmiUpload;

	private ToolStripSeparator tss1;

	private RichTextBox rtbDecodeResult;

	private Panel pDecodeResult;

	public static QRCodeForm Instance
	{
		get
		{
			if (instance == null || instance.IsDisposed)
			{
				instance = new QRCodeForm();
			}
			return instance;
		}
	}

	public QRCodeForm(string text = null)
	{
		InitializeComponent();
		rtbDecodeResult.AddContextMenu();
		ShareXResources.ApplyTheme(this);
		if (!string.IsNullOrEmpty(text))
		{
			txtQRCode.Text = text;
		}
	}

	public static QRCodeForm EncodeClipboard()
	{
		string text = ClipboardHelpers.GetText(checkContainsText: true);
		if (!string.IsNullOrEmpty(text) && TaskHelpers.CheckQRCodeContent(text))
		{
			return new QRCodeForm(text);
		}
		return new QRCodeForm();
	}

	public static QRCodeForm OpenFormDecodeFromFile(string filePath)
	{
		QRCodeForm qRCodeForm = new QRCodeForm();
		qRCodeForm.tcMain.SelectedTab = qRCodeForm.tpDecode;
		qRCodeForm.DecodeFromFile(filePath);
		return qRCodeForm;
	}

	public static QRCodeForm OpenFormDecodeFromScreen()
	{
		QRCodeForm qRCodeForm = Instance;
		qRCodeForm.tcMain.SelectedTab = qRCodeForm.tpDecode;
		qRCodeForm.DecodeFromScreen();
		return qRCodeForm;
	}

	private void QRCodeForm_Shown(object sender, EventArgs e)
	{
		isReady = true;
		txtQRCode.SetWatermark(Resources.QRCodeForm_InputTextToEncode);
		EncodeText(txtQRCode.Text);
	}

	private void ClearQRCode()
	{
		if (pbQRCode.Image != null)
		{
			Image image = pbQRCode.Image;
			pbQRCode.Image = null;
			image.Dispose();
		}
	}

	private void EncodeText(string text)
	{
		if (isReady)
		{
			ClearQRCode();
			int size = Math.Min(pbQRCode.Width, pbQRCode.Height);
			pbQRCode.Image = TaskHelpers.CreateQRCode(text, size);
			pbQRCode.BackColor = Color.White;
		}
	}

	private void DecodeImage(Bitmap bmp)
	{
		string text = "";
		string[] array = TaskHelpers.BarcodeScan(bmp);
		if (array != null)
		{
			text = string.Join(Environment.NewLine + Environment.NewLine, array);
		}
		rtbDecodeResult.Text = text;
	}

	private void DecodeFromFile(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return;
		}
		using Bitmap bitmap = ImageHelpers.LoadImage(filePath);
		if (bitmap != null)
		{
			DecodeImage(bitmap);
		}
	}

	private void DecodeFromScreen()
	{
		try
		{
			if (base.Visible)
			{
				Hide();
				Thread.Sleep(250);
			}
			using Bitmap bitmap = RegionCaptureTasks.GetRegionImage(TaskSettings.GetDefaultTaskSettings().CaptureSettings.SurfaceOptions);
			if (bitmap != null)
			{
				DecodeImage(bitmap);
			}
		}
		finally
		{
			this.ForceActivate();
		}
	}

	private void QRCodeForm_Resize(object sender, EventArgs e)
	{
		EncodeText(txtQRCode.Text);
	}

	private void txtQRCode_TextChanged(object sender, EventArgs e)
	{
		EncodeText(txtQRCode.Text);
	}

	private void tsmiCopy_Click(object sender, EventArgs e)
	{
		if (pbQRCode.Image != null)
		{
			ClipboardHelpers.CopyImage(pbQRCode.Image);
		}
	}

	private void tsmiSaveAs_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtQRCode.Text))
		{
			return;
		}
		using SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|Bitmap (*.bmp)|*.bmp|SVG (*.svg)|*.svg";
		saveFileDialog.FileName = txtQRCode.Text;
		saveFileDialog.DefaultExt = "png";
		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			string fileName = saveFileDialog.FileName;
			if (fileName.EndsWith("svg", StringComparison.InvariantCultureIgnoreCase))
			{
				SvgRenderer.SvgImage svgImage = new BarcodeWriterSvg
				{
					Format = BarcodeFormat.QR_CODE,
					Options = new EncodingOptions
					{
						Width = pbQRCode.Width,
						Height = pbQRCode.Height
					}
				}.Write(txtQRCode.Text);
				File.WriteAllText(fileName, svgImage.Content, Encoding.UTF8);
			}
			else if (pbQRCode.Image != null)
			{
				ImageHelpers.SaveImage(pbQRCode.Image, fileName);
			}
		}
	}

	private void tsmiUpload_Click(object sender, EventArgs e)
	{
		if (pbQRCode.Image != null)
		{
			UploadManager.UploadImage((Bitmap)pbQRCode.Image.Clone());
		}
	}

	private void tsmiDecode_Click(object sender, EventArgs e)
	{
		if (pbQRCode.Image != null)
		{
			tcMain.SelectedTab = tpDecode;
			DecodeImage((Bitmap)pbQRCode.Image);
		}
	}

	private void btnDecodeFromScreen_Click(object sender, EventArgs e)
	{
		DecodeFromScreen();
	}

	private void btnDecodeFromFile_Click(object sender, EventArgs e)
	{
		string filePath = ImageHelpers.OpenImageFileDialog();
		DecodeFromFile(filePath);
	}

	private void rtbDecodeResult_LinkClicked(object sender, LinkClickedEventArgs e)
	{
		URLHelpers.OpenURL(e.LinkText);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.QRCodeForm));
		this.cmsQR = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiUpload = new System.Windows.Forms.ToolStripMenuItem();
		this.tss1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiDecode = new System.Windows.Forms.ToolStripMenuItem();
		this.txtQRCode = new System.Windows.Forms.TextBox();
		this.pbQRCode = new System.Windows.Forms.PictureBox();
		this.tcMain = new System.Windows.Forms.TabControl();
		this.tpEncode = new System.Windows.Forms.TabPage();
		this.tpDecode = new System.Windows.Forms.TabPage();
		this.btnDecodeFromFile = new System.Windows.Forms.Button();
		this.lblDecodeResult = new System.Windows.Forms.Label();
		this.btnDecodeFromScreen = new System.Windows.Forms.Button();
		this.rtbDecodeResult = new System.Windows.Forms.RichTextBox();
		this.pDecodeResult = new System.Windows.Forms.Panel();
		this.cmsQR.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbQRCode).BeginInit();
		this.tcMain.SuspendLayout();
		this.tpEncode.SuspendLayout();
		this.tpDecode.SuspendLayout();
		this.pDecodeResult.SuspendLayout();
		base.SuspendLayout();
		this.cmsQR.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsmiCopy, this.tsmiSaveAs, this.tsmiUpload, this.tss1, this.tsmiDecode });
		this.cmsQR.Name = "cmsQR";
		this.cmsQR.ShowImageMargin = false;
		resources.ApplyResources(this.cmsQR, "cmsQR");
		this.tsmiCopy.Name = "tsmiCopy";
		resources.ApplyResources(this.tsmiCopy, "tsmiCopy");
		this.tsmiCopy.Click += new System.EventHandler(tsmiCopy_Click);
		this.tsmiSaveAs.Name = "tsmiSaveAs";
		resources.ApplyResources(this.tsmiSaveAs, "tsmiSaveAs");
		this.tsmiSaveAs.Click += new System.EventHandler(tsmiSaveAs_Click);
		this.tsmiUpload.Name = "tsmiUpload";
		resources.ApplyResources(this.tsmiUpload, "tsmiUpload");
		this.tsmiUpload.Click += new System.EventHandler(tsmiUpload_Click);
		this.tss1.Name = "tss1";
		resources.ApplyResources(this.tss1, "tss1");
		this.tsmiDecode.Name = "tsmiDecode";
		resources.ApplyResources(this.tsmiDecode, "tsmiDecode");
		this.tsmiDecode.Click += new System.EventHandler(tsmiDecode_Click);
		resources.ApplyResources(this.txtQRCode, "txtQRCode");
		this.txtQRCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQRCode.Name = "txtQRCode";
		this.txtQRCode.TextChanged += new System.EventHandler(txtQRCode_TextChanged);
		resources.ApplyResources(this.pbQRCode, "pbQRCode");
		this.pbQRCode.ContextMenuStrip = this.cmsQR;
		this.pbQRCode.Name = "pbQRCode";
		this.pbQRCode.TabStop = false;
		this.tcMain.Controls.Add(this.tpEncode);
		this.tcMain.Controls.Add(this.tpDecode);
		resources.ApplyResources(this.tcMain, "tcMain");
		this.tcMain.Name = "tcMain";
		this.tcMain.SelectedIndex = 0;
		this.tpEncode.BackColor = System.Drawing.SystemColors.Window;
		this.tpEncode.Controls.Add(this.txtQRCode);
		this.tpEncode.Controls.Add(this.pbQRCode);
		resources.ApplyResources(this.tpEncode, "tpEncode");
		this.tpEncode.Name = "tpEncode";
		this.tpDecode.BackColor = System.Drawing.SystemColors.Window;
		this.tpDecode.Controls.Add(this.pDecodeResult);
		this.tpDecode.Controls.Add(this.btnDecodeFromFile);
		this.tpDecode.Controls.Add(this.lblDecodeResult);
		this.tpDecode.Controls.Add(this.btnDecodeFromScreen);
		resources.ApplyResources(this.tpDecode, "tpDecode");
		this.tpDecode.Name = "tpDecode";
		resources.ApplyResources(this.btnDecodeFromFile, "btnDecodeFromFile");
		this.btnDecodeFromFile.Name = "btnDecodeFromFile";
		this.btnDecodeFromFile.UseVisualStyleBackColor = true;
		this.btnDecodeFromFile.Click += new System.EventHandler(btnDecodeFromFile_Click);
		resources.ApplyResources(this.lblDecodeResult, "lblDecodeResult");
		this.lblDecodeResult.Name = "lblDecodeResult";
		resources.ApplyResources(this.btnDecodeFromScreen, "btnDecodeFromScreen");
		this.btnDecodeFromScreen.Name = "btnDecodeFromScreen";
		this.btnDecodeFromScreen.UseVisualStyleBackColor = true;
		this.btnDecodeFromScreen.Click += new System.EventHandler(btnDecodeFromScreen_Click);
		this.rtbDecodeResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
		resources.ApplyResources(this.rtbDecodeResult, "rtbDecodeResult");
		this.rtbDecodeResult.Name = "rtbDecodeResult";
		this.rtbDecodeResult.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(rtbDecodeResult_LinkClicked);
		resources.ApplyResources(this.pDecodeResult, "pDecodeResult");
		this.pDecodeResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pDecodeResult.Controls.Add(this.rtbDecodeResult);
		this.pDecodeResult.Name = "pDecodeResult";
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.tcMain);
		base.Name = "QRCodeForm";
		base.Shown += new System.EventHandler(QRCodeForm_Shown);
		base.Resize += new System.EventHandler(QRCodeForm_Resize);
		this.cmsQR.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pbQRCode).EndInit();
		this.tcMain.ResumeLayout(false);
		this.tpEncode.ResumeLayout(false);
		this.tpEncode.PerformLayout();
		this.tpDecode.ResumeLayout(false);
		this.tpDecode.PerformLayout();
		this.pDecodeResult.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
