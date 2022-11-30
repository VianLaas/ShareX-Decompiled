using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class AboutForm : Form
{
	private EasterEggAboutAnimation easterEgg;

	private bool checkUpdate;

	private IContainer components;

	private Label lblProductName;

	private Canvas cLogo;

	private ReadOnlyRichTextBox rtbInfo;

	private UpdateCheckerLabel uclUpdate;

	private PictureBox pbLogo;

	private Button btnClose;

	private Button btnShareXLicense;

	private Button btnLicenses;

	private Label lblBuild;

	public AboutForm()
	{
		InitializeComponent();
		lblProductName.Text = Program.Title;
		pbLogo.Image = ShareXResources.Logo;
		ShareXResources.ApplyTheme(this);
		if (!SystemOptions.DisableUpdateCheck)
		{
			uclUpdate.UpdateLoadingImage();
			checkUpdate = true;
		}
		else
		{
			uclUpdate.Visible = false;
		}
		rtbInfo.AppendLine(Resources.AboutForm_AboutForm_Links, FontStyle.Bold, 13f);
		rtbInfo.AppendLine(Resources.AboutForm_AboutForm_Website + ": https://getsharex.com\r\n" + Resources.AboutForm_AboutForm_Project_page + ": https://github.com/ShareX/ShareX\r\n" + Resources.AboutForm_AboutForm_Changelog + ": https://getsharex.com/changelog\r\n" + Resources.AboutForm_AboutForm_Privacy_policy + ": https://getsharex.com/privacy-policy\r\n", FontStyle.Regular);
		rtbInfo.AppendLine(Resources.AboutForm_AboutForm_Team, FontStyle.Bold, 13f);
		rtbInfo.AppendLine("Jaex: https://github.com/Jaex\r\nMcoreD: https://github.com/McoreD\r\n", FontStyle.Regular);
		rtbInfo.AppendLine(Resources.AboutForm_AboutForm_Translators, FontStyle.Bold, 13f);
		rtbInfo.AppendLine(Resources.AboutForm_AboutForm_Language_tr + ": https://github.com/Jaex\r\n" + Resources.AboutForm_AboutForm_Language_de + ": https://github.com/Starbug2 & https://github.com/Kaeltis\r\n" + Resources.AboutForm_AboutForm_Language_fr + ": https://github.com/nwies & https://github.com/Shadorc\r\n" + Resources.AboutForm_AboutForm_Language_zh_CH + ": https://github.com/jiajiechan\r\n" + Resources.AboutForm_AboutForm_Language_hu + ": https://github.com/devBluestar\r\n" + Resources.AboutForm_AboutForm_Language_ko_KR + ": https://github.com/123jimin\r\n" + Resources.AboutForm_AboutForm_Language_es + ": https://github.com/ovnisoftware\r\n" + Resources.AboutForm_AboutForm_Language_nl_NL + ": https://github.com/canihavesomecoffee\r\n" + Resources.AboutForm_AboutForm_Language_pt_BR + ": https://github.com/RockyTV & https://github.com/athosbr99\r\n" + Resources.AboutForm_AboutForm_Language_vi_VN + ": https://github.com/thanhpd\r\n" + Resources.AboutForm_AboutForm_Language_ru + ": https://github.com/L1Q\r\n" + Resources.AboutForm_AboutForm_Language_zh_TW + ": https://github.com/alantsai\r\n" + Resources.AboutForm_AboutForm_Language_it_IT + ": https://github.com/pjammo\r\n" + Resources.AboutForm_AboutForm_Language_uk + ": https://github.com/6c6c6\r\n" + Resources.AboutForm_AboutForm_Language_id_ID + ": https://github.com/Nicedward\r\n" + Resources.AboutForm_AboutForm_Language_es_MX + ": https://github.com/absay\r\n" + Resources.AboutForm_AboutForm_Language_fa_IR + ": https://github.com/pourmand1376\r\n" + Resources.AboutForm_AboutForm_Language_pt_PT + ": https://github.com/FarewellAngelina\r\n" + Resources.AboutForm_AboutForm_Language_ja_JP + ": https://github.com/kanaxx\r\n" + Resources.AboutForm_AboutForm_Language_ro + ": https://github.com/Edward205\r\n" + Resources.AboutForm_AboutForm_Language_pl + ": https://github.com/RikoDEV\r\n", FontStyle.Regular);
		rtbInfo.AppendLine(Resources.AboutForm_AboutForm_Credits, FontStyle.Bold, 13f);
		rtbInfo.AppendLine("Json.NET: https://github.com/JamesNK/Newtonsoft.Json\r\nSSH.NET: https://github.com/sshnet/SSH.NET\r\nIcons: http://p.yusukekamiyamane.com\r\nImageListView: https://github.com/oozcitak/imagelistview\r\nFFmpeg: https://www.ffmpeg.org\r\nRecorder devices: https://github.com/rdp/screen-capture-recorder-to-video-windows-free\r\nFluentFTP: https://github.com/robinrodricks/FluentFTP\r\nSteamworks.NET: https://github.com/rlabrecque/Steamworks.NET\r\nZXing.Net: https://github.com/micjahn/ZXing.Net\r\nMegaApiClient: https://github.com/gpailler/MegaApiClient\r\nInno Setup Dependency Installer: https://github.com/DomGries/InnoDependencyInstaller\r\nBlob Emoji: http://blobs.gg\r\n", FontStyle.Regular);
		rtbInfo.AppendText("Copyright (c) 2007-2022 ShareX Team", FontStyle.Bold, 13f);
		easterEgg = new EasterEggAboutAnimation(cLogo, this);
	}

	private async void AboutForm_Shown(object sender, EventArgs e)
	{
		this.ForceActivate();
		if (checkUpdate)
		{
			UpdateChecker updateChecker = Program.UpdateManager.CreateUpdateChecker();
			await uclUpdate.CheckUpdate(updateChecker);
		}
	}

	private void pbLogo_MouseDown(object sender, MouseEventArgs e)
	{
		easterEgg.Start();
		pbLogo.Visible = false;
	}

	private void rtb_LinkClicked(object sender, LinkClickedEventArgs e)
	{
		URLHelpers.OpenURL(e.LinkText);
	}

	private void btnShareXLicense_Click(object sender, EventArgs e)
	{
		FileHelpers.OpenFile(FileHelpers.GetAbsolutePath("Licenses\\ShareX_license.txt"));
	}

	private void btnLicenses_Click(object sender, EventArgs e)
	{
		FileHelpers.OpenFolder(FileHelpers.GetAbsolutePath("Licenses"));
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		if (easterEgg != null)
		{
			easterEgg.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.AboutForm));
		this.lblProductName = new System.Windows.Forms.Label();
		this.rtbInfo = new ShareX.HelpersLib.ReadOnlyRichTextBox();
		this.pbLogo = new System.Windows.Forms.PictureBox();
		this.btnClose = new System.Windows.Forms.Button();
		this.btnShareXLicense = new System.Windows.Forms.Button();
		this.btnLicenses = new System.Windows.Forms.Button();
		this.lblBuild = new System.Windows.Forms.Label();
		this.cLogo = new ShareX.HelpersLib.Canvas();
		this.uclUpdate = new ShareX.HelpersLib.UpdateCheckerLabel();
		((System.ComponentModel.ISupportInitialize)this.pbLogo).BeginInit();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.lblProductName, "lblProductName");
		this.lblProductName.BackColor = System.Drawing.Color.Transparent;
		this.lblProductName.Name = "lblProductName";
		componentResourceManager.ApplyResources(this.rtbInfo, "rtbInfo");
		this.rtbInfo.BackColor = System.Drawing.SystemColors.Window;
		this.rtbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.rtbInfo.ForeColor = System.Drawing.SystemColors.ControlText;
		this.rtbInfo.Name = "rtbInfo";
		this.rtbInfo.ReadOnly = true;
		this.rtbInfo.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(rtb_LinkClicked);
		componentResourceManager.ApplyResources(this.pbLogo, "pbLogo");
		this.pbLogo.BackColor = System.Drawing.Color.Transparent;
		this.pbLogo.Name = "pbLogo";
		this.pbLogo.TabStop = false;
		this.pbLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(pbLogo_MouseDown);
		componentResourceManager.ApplyResources(this.btnClose, "btnClose");
		this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnClose.Name = "btnClose";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		componentResourceManager.ApplyResources(this.btnShareXLicense, "btnShareXLicense");
		this.btnShareXLicense.Name = "btnShareXLicense";
		this.btnShareXLicense.UseVisualStyleBackColor = true;
		this.btnShareXLicense.Click += new System.EventHandler(btnShareXLicense_Click);
		componentResourceManager.ApplyResources(this.btnLicenses, "btnLicenses");
		this.btnLicenses.Name = "btnLicenses";
		this.btnLicenses.UseVisualStyleBackColor = true;
		this.btnLicenses.Click += new System.EventHandler(btnLicenses_Click);
		componentResourceManager.ApplyResources(this.lblBuild, "lblBuild");
		this.lblBuild.Name = "lblBuild";
		componentResourceManager.ApplyResources(this.cLogo, "cLogo");
		this.cLogo.Interval = 100;
		this.cLogo.Name = "cLogo";
		componentResourceManager.ApplyResources(this.uclUpdate, "uclUpdate");
		this.uclUpdate.Name = "uclUpdate";
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnClose;
		base.Controls.Add(this.pbLogo);
		base.Controls.Add(this.cLogo);
		base.Controls.Add(this.lblProductName);
		base.Controls.Add(this.lblBuild);
		base.Controls.Add(this.btnLicenses);
		base.Controls.Add(this.btnShareXLicense);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.rtbInfo);
		base.Controls.Add(this.uclUpdate);
		base.Name = "AboutForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.Shown += new System.EventHandler(AboutForm_Shown);
		((System.ComponentModel.ISupportInitialize)this.pbLogo).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
