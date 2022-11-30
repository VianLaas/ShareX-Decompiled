using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;

namespace ShareX;

public class OCRForm : Form
{
	private Bitmap bmpSource;

	private bool loaded;

	private bool busy;

	private IContainer components;

	private Label lblLanguage;

	private ComboBox cbLanguages;

	private Label lblResult;

	private TextBox txtResult;

	private Label lblScaleFactor;

	private NumericUpDown nudScaleFactor;

	private ComboBox cbServices;

	private Button btnOpenServiceLink;

	private Button cbEditServices;

	private Button btnOpenOCRHelp;

	private Label lblService;

	private Button btnSelectRegion;

	private CheckBox cbSingleLine;

	private Button btnCopyAll;

	public OCROptions Options { get; set; }

	public string Result { get; private set; }

	public OCRForm(Bitmap bmp, OCROptions options)
	{
		bmpSource = (Bitmap)bmp.Clone();
		Options = options;
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		OCRLanguage[] array = OCRHelper.AvailableLanguages.OrderBy((OCRLanguage x) => x.DisplayName).ToArray();
		if (array.Length != 0)
		{
			ComboBox.ObjectCollection items = cbLanguages.Items;
			object[] items2 = array;
			items.AddRange(items2);
			if (Options.Language == null)
			{
				cbLanguages.SelectedIndex = 0;
				Options.Language = array[0].LanguageTag;
			}
			else
			{
				int num = Array.FindIndex(array, (OCRLanguage x) => x.LanguageTag.Equals(Options.Language, StringComparison.OrdinalIgnoreCase));
				if (num >= 0)
				{
					cbLanguages.SelectedIndex = num;
				}
				else
				{
					cbLanguages.SelectedIndex = 0;
					Options.Language = array[0].LanguageTag;
				}
			}
		}
		else
		{
			cbLanguages.Enabled = false;
		}
		nudScaleFactor.SetValue((decimal)Options.ScaleFactor);
		cbSingleLine.Checked = Options.SingleLine;
		if (Options.ServiceLinks == null || Options.IsDefaultServiceLinks())
		{
			Options.ServiceLinks = OCROptions.DefaultServiceLinks;
		}
		if (Options.ServiceLinks.Count > 0)
		{
			ComboBox.ObjectCollection items3 = cbServices.Items;
			object[] items2 = Options.ServiceLinks.ToArray();
			items3.AddRange(items2);
			cbServices.SelectedIndex = Options.SelectedServiceLink;
		}
		else
		{
			cbServices.Enabled = false;
		}
		txtResult.SupportSelectAll();
		UpdateControls();
		loaded = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			components?.Dispose();
			bmpSource?.Dispose();
		}
		base.Dispose(disposing);
	}

	private void UpdateControls()
	{
		if (busy)
		{
			Cursor = Cursors.WaitCursor;
		}
		else
		{
			Cursor = Cursors.Default;
		}
		btnSelectRegion.Enabled = !busy;
		cbLanguages.Enabled = !busy;
		nudScaleFactor.Enabled = !busy;
		cbSingleLine.Enabled = !busy;
	}

	private async Task OCR(Bitmap bmp)
	{
		if (bmp == null || string.IsNullOrEmpty(Options.Language))
		{
			return;
		}
		busy = true;
		txtResult.Text = "";
		UpdateControls();
		try
		{
			Result = await OCRHelper.OCR(bmp, Options.Language, Options.ScaleFactor, Options.SingleLine);
			if (Options.AutoCopy && !string.IsNullOrEmpty(Result))
			{
				ClipboardHelpers.CopyText(Result);
			}
		}
		catch (Exception e)
		{
			e.ShowError(fullError: false);
		}
		if (!base.IsDisposed)
		{
			busy = false;
			txtResult.Text = Result;
			txtResult.Focus();
			txtResult.DeselectAll();
			UpdateControls();
		}
	}

	private async void OCRForm_Shown(object sender, EventArgs e)
	{
		await OCR(bmpSource);
	}

	private async void btnSelectRegion_Click(object sender, EventArgs e)
	{
		FormWindowState previousState = base.WindowState;
		base.WindowState = FormWindowState.Minimized;
		await Task.Delay(250);
		Bitmap regionImage = RegionCaptureTasks.GetRegionImage(new RegionCaptureOptions());
		base.WindowState = previousState;
		if (regionImage != null)
		{
			bmpSource?.Dispose();
			bmpSource = regionImage;
			await Task.Delay(250);
			await OCR(bmpSource);
		}
	}

	private async void cbLanguages_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			Options.Language = ((OCRLanguage)cbLanguages.SelectedItem).LanguageTag;
			await OCR(bmpSource);
		}
	}

	private void btnOpenOCRHelp_Click(object sender, EventArgs e)
	{
		URLHelpers.OpenURL("https://getsharex.com/docs/ocr");
	}

	private async void nudScaleFactor_ValueChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			Options.ScaleFactor = (float)nudScaleFactor.Value;
			await OCR(bmpSource);
		}
	}

	private async void cbSingleLine_CheckedChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			Options.SingleLine = cbSingleLine.Checked;
			await OCR(bmpSource);
		}
	}

	private void cbServices_SelectedIndexChanged(object sender, EventArgs e)
	{
		Options.SelectedServiceLink = cbServices.SelectedIndex;
	}

	private void btnOpenServiceLink_Click(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(Result) && cbServices.SelectedItem is ServiceLink serviceLink)
		{
			serviceLink.OpenLink(Result);
		}
	}

	private void cbEditServices_Click(object sender, EventArgs e)
	{
		using ServiceLinksForm serviceLinksForm = new ServiceLinksForm(Options.ServiceLinks);
		serviceLinksForm.ShowDialog();
		cbServices.Items.Clear();
		if (Options.ServiceLinks.Count > 0)
		{
			ComboBox.ObjectCollection items = cbServices.Items;
			object[] items2 = Options.ServiceLinks.ToArray();
			items.AddRange(items2);
			cbServices.SelectedIndex = 0;
			Options.SelectedServiceLink = 0;
		}
		cbServices.Enabled = cbServices.Items.Count > 0;
	}

	private void btnCopyAll_Click(object sender, EventArgs e)
	{
		ClipboardHelpers.CopyText(txtResult.Text);
	}

	private void txtResult_TextChanged(object sender, EventArgs e)
	{
		Result = txtResult.Text.Trim();
		Button button = btnOpenServiceLink;
		bool enabled = (btnCopyAll.Enabled = !string.IsNullOrEmpty(Result));
		button.Enabled = enabled;
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.OCRForm));
		this.lblLanguage = new System.Windows.Forms.Label();
		this.cbLanguages = new System.Windows.Forms.ComboBox();
		this.lblResult = new System.Windows.Forms.Label();
		this.txtResult = new System.Windows.Forms.TextBox();
		this.lblScaleFactor = new System.Windows.Forms.Label();
		this.nudScaleFactor = new System.Windows.Forms.NumericUpDown();
		this.cbServices = new System.Windows.Forms.ComboBox();
		this.btnOpenServiceLink = new System.Windows.Forms.Button();
		this.cbEditServices = new System.Windows.Forms.Button();
		this.btnOpenOCRHelp = new System.Windows.Forms.Button();
		this.lblService = new System.Windows.Forms.Label();
		this.btnSelectRegion = new System.Windows.Forms.Button();
		this.cbSingleLine = new System.Windows.Forms.CheckBox();
		this.btnCopyAll = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.nudScaleFactor).BeginInit();
		base.SuspendLayout();
		resources.ApplyResources(this.lblLanguage, "lblLanguage");
		this.lblLanguage.Name = "lblLanguage";
		this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		resources.ApplyResources(this.cbLanguages, "cbLanguages");
		this.cbLanguages.FormattingEnabled = true;
		this.cbLanguages.Name = "cbLanguages";
		this.cbLanguages.SelectedIndexChanged += new System.EventHandler(cbLanguages_SelectedIndexChanged);
		resources.ApplyResources(this.lblResult, "lblResult");
		this.lblResult.Name = "lblResult";
		resources.ApplyResources(this.txtResult, "txtResult");
		this.txtResult.Name = "txtResult";
		this.txtResult.TextChanged += new System.EventHandler(txtResult_TextChanged);
		resources.ApplyResources(this.lblScaleFactor, "lblScaleFactor");
		this.lblScaleFactor.Name = "lblScaleFactor";
		this.nudScaleFactor.DecimalPlaces = 1;
		resources.ApplyResources(this.nudScaleFactor, "nudScaleFactor");
		this.nudScaleFactor.Increment = new decimal(new int[4] { 5, 0, 0, 65536 });
		this.nudScaleFactor.Maximum = new decimal(new int[4] { 4, 0, 0, 0 });
		this.nudScaleFactor.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudScaleFactor.Name = "nudScaleFactor";
		this.nudScaleFactor.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudScaleFactor.ValueChanged += new System.EventHandler(nudScaleFactor_ValueChanged);
		this.cbServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		resources.ApplyResources(this.cbServices, "cbServices");
		this.cbServices.FormattingEnabled = true;
		this.cbServices.Name = "cbServices";
		this.cbServices.SelectedIndexChanged += new System.EventHandler(cbServices_SelectedIndexChanged);
		resources.ApplyResources(this.btnOpenServiceLink, "btnOpenServiceLink");
		this.btnOpenServiceLink.Name = "btnOpenServiceLink";
		this.btnOpenServiceLink.UseVisualStyleBackColor = true;
		this.btnOpenServiceLink.Click += new System.EventHandler(btnOpenServiceLink_Click);
		this.cbEditServices.Image = ShareX.Properties.Resources.gear;
		resources.ApplyResources(this.cbEditServices, "cbEditServices");
		this.cbEditServices.Name = "cbEditServices";
		this.cbEditServices.UseVisualStyleBackColor = true;
		this.cbEditServices.Click += new System.EventHandler(cbEditServices_Click);
		resources.ApplyResources(this.btnOpenOCRHelp, "btnOpenOCRHelp");
		this.btnOpenOCRHelp.Image = ShareX.Properties.Resources.question;
		this.btnOpenOCRHelp.Name = "btnOpenOCRHelp";
		this.btnOpenOCRHelp.UseVisualStyleBackColor = true;
		this.btnOpenOCRHelp.Click += new System.EventHandler(btnOpenOCRHelp_Click);
		resources.ApplyResources(this.lblService, "lblService");
		this.lblService.Name = "lblService";
		resources.ApplyResources(this.btnSelectRegion, "btnSelectRegion");
		this.btnSelectRegion.Name = "btnSelectRegion";
		this.btnSelectRegion.UseVisualStyleBackColor = true;
		this.btnSelectRegion.Click += new System.EventHandler(btnSelectRegion_Click);
		resources.ApplyResources(this.cbSingleLine, "cbSingleLine");
		this.cbSingleLine.Name = "cbSingleLine";
		this.cbSingleLine.UseVisualStyleBackColor = true;
		this.cbSingleLine.CheckedChanged += new System.EventHandler(cbSingleLine_CheckedChanged);
		resources.ApplyResources(this.btnCopyAll, "btnCopyAll");
		this.btnCopyAll.Name = "btnCopyAll";
		this.btnCopyAll.UseVisualStyleBackColor = true;
		this.btnCopyAll.Click += new System.EventHandler(btnCopyAll_Click);
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnCopyAll);
		base.Controls.Add(this.cbSingleLine);
		base.Controls.Add(this.lblService);
		base.Controls.Add(this.btnOpenOCRHelp);
		base.Controls.Add(this.btnSelectRegion);
		base.Controls.Add(this.cbEditServices);
		base.Controls.Add(this.btnOpenServiceLink);
		base.Controls.Add(this.cbServices);
		base.Controls.Add(this.nudScaleFactor);
		base.Controls.Add(this.lblScaleFactor);
		base.Controls.Add(this.txtResult);
		base.Controls.Add(this.lblResult);
		base.Controls.Add(this.cbLanguages);
		base.Controls.Add(this.lblLanguage);
		base.Name = "OCRForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.Shown += new System.EventHandler(OCRForm_Shown);
		((System.ComponentModel.ISupportInitialize)this.nudScaleFactor).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
