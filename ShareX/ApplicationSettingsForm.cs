using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.UploadersLib;

namespace ShareX;

public class ApplicationSettingsForm : Form
{
	private bool ready;

	private string lastPersonalPath;

	private IContainer components;

	private TabControl tcSettings;

	private TabPage tpGeneral;

	private TabPage tpProxy;

	private CheckBox cbSendToMenu;

	private Button btnOpenPersonalFolderPath;

	private CheckBox cbShowTray;

	private CheckBox cbStartWithWindows;

	private Label lblSaveImageSubFolderPatternPreview;

	private TextBox txtSaveImageSubFolderPattern;

	private Label lblSaveImageSubFolderPattern;

	private CheckBox cbUseCustomScreenshotsPath;

	private TabPage tpPaths;

	private Button btnBrowseCustomScreenshotsPath;

	private TextBox txtCustomScreenshotsPath;

	private Label lblProxyHost;

	private TextBox txtProxyHost;

	private NumericUpDown nudProxyPort;

	private Label lblProxyPort;

	private Label lblProxyPassword;

	private TextBox txtProxyPassword;

	private Label lblProxyUsername;

	private TextBox txtProxyUsername;

	private CheckBox cbShellContextMenu;

	private ComboBox cbProxyMethod;

	private Label lblProxyMethod;

	private TabPage tpUpload;

	private Label cbIfUploadFailRetryOnce;

	private Label lblUploadLimit;

	private ComboBox cbBufferSize;

	private Label lblUploadLimitHint;

	private Label lblBufferSize;

	private NumericUpDown nudUploadLimit;

	private Button btnClipboardFormatRemove;

	private Button btnClipboardFormatAdd;

	private MyListView lvClipboardFormats;

	private ColumnHeader chDescription;

	private ColumnHeader chFormat;

	private Button btnClipboardFormatEdit;

	private TabPage tpPrint;

	private CheckBox cbDontShowPrintSettingDialog;

	private Button btnShowImagePrintSettings;

	private TabPage tpAdvanced;

	private PropertyGrid pgSettings;

	private CheckBox cbTaskbarProgressEnabled;

	private CheckBox cbTrayIconProgressEnabled;

	private CheckBox cbRememberMainFormSize;

	private Label lblPreviewPersonalFolderPath;

	private Button btnBrowsePersonalFolderPath;

	private Label lblPersonalFolderPath;

	private TextBox txtPersonalFolderPath;

	private Button btnOpenScreenshotsFolder;

	private CheckBox cbSilentRun;

	private NumericUpDown nudRetryUpload;

	private GroupBox gbSecondaryImageUploaders;

	private MyListView lvSecondaryImageUploaders;

	private GroupBox gbSecondaryFileUploaders;

	private MyListView lvSecondaryFileUploaders;

	private GroupBox gbSecondaryTextUploaders;

	private MyListView lvSecondaryTextUploaders;

	private CheckBox cbUseSecondaryUploaders;

	private ColumnHeader chSecondaryImageUploaders;

	private ColumnHeader chSecondaryFileUploaders;

	private ColumnHeader chSecondaryTextUploaders;

	private CheckBox cbPrintDontShowWindowsDialog;

	private CheckBox cbRememberMainFormPosition;

	private Label lblLanguage;

	private TabToTreeView tttvMain;

	private MenuButton btnLanguages;

	private ContextMenuStrip cmsLanguages;

	private GroupBox gbWindows;

	private GroupBox gbChrome;

	private CheckBox cbSteamShowInApp;

	private TabPage tpIntegration;

	private GroupBox gbSteam;

	private TabPage tpSettings;

	private Button btnImport;

	private Button btnExport;

	private ProgressBar pbExportImport;

	private Button btnEditQuickTaskMenu;

	private TabPage tpHistory;

	private GroupBox gbRecentLinks;

	private CheckBox cbRecentTasksSave;

	private CheckBox cbRecentTasksShowInTrayMenu;

	private CheckBox cbRecentTasksShowInMainWindow;

	private Label lblRecentTasksMaxCount;

	private NumericUpDown nudRecentTasksMaxCount;

	private CheckBox cbRecentTasksTrayMenuMostRecentFirst;

	private GroupBox gbHistory;

	private CheckBox cbHistorySaveTasks;

	private CheckBox cbHistoryCheckURL;

	private Label lblTrayMiddleClickAction;

	private Label lblTrayLeftDoubleClickAction;

	private Label lblTrayLeftClickAction;

	private ComboBox cbTrayMiddleClickAction;

	private ComboBox cbTrayLeftDoubleClickAction;

	private ComboBox cbTrayLeftClickAction;

	private CheckBox cbCheckPreReleaseUpdates;

	private Button btnChromeOpenExtensionPage;

	private GroupBox gbFirefox;

	private Button btnFirefoxOpenAddonPage;

	private CheckBox cbChromeExtensionSupport;

	private CheckBox cbFirefoxAddonSupport;

	private Button btnResetSettings;

	private CheckBox cbEditWithShareX;

	private Button btnCheckDevBuild;

	private Button btnPersonalFolderPathApply;

	private CheckBox cbUseCustomTheme;

	private CheckBox cbUseWhiteShareXIcon;

	private TabPage tpTheme;

	private PropertyGrid pgTheme;

	private ComboBox cbThemes;

	private Button btnThemeRemove;

	private Button btnThemeAdd;

	private ExportImportControl eiTheme;

	private Button btnThemeReset;

	private Label lblExportImportNote;

	private CheckBox cbExportHistory;

	private CheckBox cbExportSettings;

	private PictureBox pbExportImportNote;

	private CheckBox cbAutomaticallyCleanupBackupFiles;

	private NumericUpDown nudCleanupKeepFileCount;

	private Label lblCleanupKeepFileCount;

	private CheckBox cbAutomaticallyCleanupLogFiles;

	private Label lblDefaultPrinterOverride;

	private TextBox txtDefaultPrinterOverride;

	private TabPage tpMainWindow;

	private Label lblMainWindowTaskViewMode;

	private ComboBox cbMainWindowTaskViewMode;

	private CheckBox cbMainWindowShowMenu;

	private GroupBox gbThumbnailView;

	private CheckBox cbThumbnailViewShowTitle;

	private ComboBox cbThumbnailViewTitleLocation;

	private Label lblThumbnailViewTitleLocation;

	private Label lblThumbnailViewThumbnailSize;

	private Label lblThumbnailViewThumbnailClickAction;

	private GroupBox gbListView;

	private CheckBox cbListViewShowColumns;

	private ComboBox cbListViewImagePreviewVisibility;

	private Label lblListViewImagePreviewVisibility;

	private Label lblListViewImagePreviewLocation;

	private ComboBox cbListViewImagePreviewLocation;

	private ComboBox cbThumbnailViewThumbnailClickAction;

	private NumericUpDown nudThumbnailViewThumbnailSizeHeight;

	private NumericUpDown nudThumbnailViewThumbnailSizeWidth;

	private Label lblThumbnailViewThumbnailSizeX;

	private Button btnThumbnailViewThumbnailSizeReset;

	private TabPage tpClipboardFormats;

	private Label lblClipboardFormatsTip;

	private TextBox txtSaveImageSubFolderPatternWindow;

	private Label lblSaveImageSubFolderPatternWindow;

	public ApplicationSettingsForm()
	{
		InitializeControls();
		ShareXResources.ApplyTheme(this);
	}

	private void SettingsForm_Shown(object sender, EventArgs e)
	{
		this.ForceActivate();
	}

	private void SettingsForm_Resize(object sender, EventArgs e)
	{
		Refresh();
	}

	private void tttvMain_TabChanged(TabPage tabPage)
	{
		if (tabPage == tpIntegration)
		{
			UpdateStartWithWindows();
		}
	}

	private void InitializeControls()
	{
		InitializeComponent();
		SupportedLanguage[] enums = Helpers.GetEnums<SupportedLanguage>();
		foreach (SupportedLanguage supportedLanguage in enums)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(supportedLanguage.GetLocalizedDescription());
			toolStripMenuItem.Image = LanguageHelper.GetLanguageIcon(supportedLanguage);
			toolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
			SupportedLanguage lang = supportedLanguage;
			toolStripMenuItem.Click += delegate
			{
				ChangeLanguage(lang);
			};
			cmsLanguages.Items.Add(toolStripMenuItem);
		}
		ComboBox.ObjectCollection items = cbTrayLeftDoubleClickAction.Items;
		object[] localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<HotkeyType>();
		items.AddRange(localizedEnumDescriptions);
		ComboBox.ObjectCollection items2 = cbTrayLeftClickAction.Items;
		localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<HotkeyType>();
		items2.AddRange(localizedEnumDescriptions);
		ComboBox.ObjectCollection items3 = cbTrayMiddleClickAction.Items;
		localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<HotkeyType>();
		items3.AddRange(localizedEnumDescriptions);
		ComboBox.ObjectCollection items4 = cbMainWindowTaskViewMode.Items;
		localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<TaskViewMode>();
		items4.AddRange(localizedEnumDescriptions);
		ComboBox.ObjectCollection items5 = cbThumbnailViewTitleLocation.Items;
		localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<ThumbnailTitleLocation>();
		items5.AddRange(localizedEnumDescriptions);
		ComboBox.ObjectCollection items6 = cbThumbnailViewThumbnailClickAction.Items;
		localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<ThumbnailViewClickAction>();
		items6.AddRange(localizedEnumDescriptions);
		ComboBox.ObjectCollection items7 = cbListViewImagePreviewVisibility.Items;
		localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<ImagePreviewVisibility>();
		items7.AddRange(localizedEnumDescriptions);
		ComboBox.ObjectCollection items8 = cbListViewImagePreviewLocation.Items;
		localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<ImagePreviewLocation>();
		items8.AddRange(localizedEnumDescriptions);
		eiTheme.ObjectType = typeof(ShareXTheme);
		CodeMenu.Create<CodeMenuEntryFilename>(txtSaveImageSubFolderPattern, CodeMenuEntryFilename.t, CodeMenuEntryFilename.pn, CodeMenuEntryFilename.i, CodeMenuEntryFilename.width, CodeMenuEntryFilename.height, CodeMenuEntryFilename.n);
		CodeMenu.Create<CodeMenuEntryFilename>(txtSaveImageSubFolderPatternWindow, CodeMenuEntryFilename.i, CodeMenuEntryFilename.n);
		ComboBox.ObjectCollection items9 = cbProxyMethod.Items;
		localizedEnumDescriptions = Helpers.GetLocalizedEnumDescriptions<ProxyMethod>();
		items9.AddRange(localizedEnumDescriptions);
		UpdateControls();
	}

	private void UpdateControls()
	{
		ready = false;
		ChangeLanguage(Program.Settings.Language);
		cbShowTray.Checked = Program.Settings.ShowTray;
		cbSilentRun.Enabled = Program.Settings.ShowTray;
		cbSilentRun.Checked = Program.Settings.SilentRun;
		cbTrayIconProgressEnabled.Checked = Program.Settings.TrayIconProgressEnabled;
		cbTaskbarProgressEnabled.Enabled = TaskbarManager.IsPlatformSupported;
		cbTaskbarProgressEnabled.Checked = Program.Settings.TaskbarProgressEnabled;
		cbUseCustomTheme.Checked = Program.Settings.UseCustomTheme;
		cbUseWhiteShareXIcon.Checked = Program.Settings.UseWhiteShareXIcon;
		cbRememberMainFormPosition.Checked = Program.Settings.RememberMainFormPosition;
		cbRememberMainFormSize.Checked = Program.Settings.RememberMainFormSize;
		cbTrayLeftDoubleClickAction.SelectedIndex = (int)Program.Settings.TrayLeftDoubleClickAction;
		cbTrayLeftClickAction.SelectedIndex = (int)Program.Settings.TrayLeftClickAction;
		cbTrayMiddleClickAction.SelectedIndex = (int)Program.Settings.TrayMiddleClickAction;
		if (SystemOptions.DisableUpdateCheck)
		{
			cbCheckPreReleaseUpdates.Visible = false;
			btnCheckDevBuild.Visible = false;
		}
		else
		{
			cbCheckPreReleaseUpdates.Checked = Program.Settings.CheckPreReleaseUpdates;
		}
		ComboBox.ObjectCollection items = cbThemes.Items;
		object[] items2 = Program.Settings.Themes.ToArray();
		items.AddRange(items2);
		cbThemes.SelectedIndex = Program.Settings.SelectedTheme;
		pgTheme.SelectedObject = Program.Settings.Themes[Program.Settings.SelectedTheme];
		UpdateThemeControls();
		cbShellContextMenu.Checked = IntegrationHelpers.CheckShellContextMenuButton();
		cbEditWithShareX.Checked = IntegrationHelpers.CheckEditShellContextMenuButton();
		cbSendToMenu.Checked = IntegrationHelpers.CheckSendToMenuButton();
		cbChromeExtensionSupport.Checked = IntegrationHelpers.CheckChromeExtensionSupport();
		btnChromeOpenExtensionPage.Enabled = cbChromeExtensionSupport.Checked;
		cbFirefoxAddonSupport.Checked = IntegrationHelpers.CheckFirefoxAddonSupport();
		btnFirefoxOpenAddonPage.Enabled = cbFirefoxAddonSupport.Checked;
		gbSteam.Visible = false;
		lastPersonalPath = Program.ReadPersonalPathConfig();
		txtPersonalFolderPath.Text = lastPersonalPath;
		UpdatePersonalFolderPathPreview();
		cbUseCustomScreenshotsPath.Checked = Program.Settings.UseCustomScreenshotsPath;
		txtCustomScreenshotsPath.Text = Program.Settings.CustomScreenshotsPath;
		txtSaveImageSubFolderPattern.Text = Program.Settings.SaveImageSubFolderPattern;
		txtSaveImageSubFolderPatternWindow.Text = Program.Settings.SaveImageSubFolderPatternWindow;
		cbAutomaticallyCleanupBackupFiles.Checked = Program.Settings.AutoCleanupBackupFiles;
		cbAutomaticallyCleanupLogFiles.Checked = Program.Settings.AutoCleanupLogFiles;
		nudCleanupKeepFileCount.SetValue(Program.Settings.CleanupKeepFileCount);
		cbMainWindowShowMenu.Checked = Program.Settings.ShowMenu;
		cbMainWindowTaskViewMode.SelectedIndex = (int)Program.Settings.TaskViewMode;
		cbThumbnailViewShowTitle.Checked = Program.Settings.ShowThumbnailTitle;
		cbThumbnailViewTitleLocation.SelectedIndex = (int)Program.Settings.ThumbnailTitleLocation;
		nudThumbnailViewThumbnailSizeWidth.SetValue(Program.Settings.ThumbnailSize.Width);
		nudThumbnailViewThumbnailSizeHeight.SetValue(Program.Settings.ThumbnailSize.Height);
		cbThumbnailViewThumbnailClickAction.SelectedIndex = (int)Program.Settings.ThumbnailClickAction;
		cbListViewShowColumns.Checked = Program.Settings.ShowColumns;
		cbListViewImagePreviewVisibility.SelectedIndex = (int)Program.Settings.ImagePreview;
		cbListViewImagePreviewLocation.SelectedIndex = (int)Program.Settings.ImagePreviewLocation;
		lvClipboardFormats.Items.Clear();
		foreach (ClipboardFormat clipboardContentFormat in Program.Settings.ClipboardContentFormats)
		{
			AddClipboardFormat(clipboardContentFormat);
		}
		nudUploadLimit.SetValue(Program.Settings.UploadLimit);
		cbBufferSize.Items.Clear();
		int num = 14;
		for (int i = 0; i < num; i++)
		{
			string item = ((long)(Math.Pow(2.0, i) * 1024.0)).ToSizeString(Program.Settings.BinaryUnits, 0);
			cbBufferSize.Items.Add(item);
		}
		cbBufferSize.SelectedIndex = Program.Settings.BufferSizePower.Clamp(0, num);
		nudRetryUpload.SetValue(Program.Settings.MaxUploadFailRetry);
		cbUseSecondaryUploaders.Checked = Program.Settings.UseSecondaryUploaders;
		Program.Settings.SecondaryImageUploaders.AddRange(from n in Helpers.GetEnums<ImageDestination>()
			where Program.Settings.SecondaryImageUploaders.All((ImageDestination e) => e != n)
			select n);
		Program.Settings.SecondaryTextUploaders.AddRange(from n in Helpers.GetEnums<TextDestination>()
			where Program.Settings.SecondaryTextUploaders.All((TextDestination e) => e != n)
			select n);
		Program.Settings.SecondaryFileUploaders.AddRange(from n in Helpers.GetEnums<FileDestination>()
			where Program.Settings.SecondaryFileUploaders.All((FileDestination e) => e != n)
			select n);
		Program.Settings.SecondaryImageUploaders.Where((ImageDestination n) => Helpers.GetEnums<ImageDestination>().All((ImageDestination e) => e != n)).ForEach(delegate(ImageDestination x)
		{
			Program.Settings.SecondaryImageUploaders.Remove(x);
		});
		Program.Settings.SecondaryTextUploaders.Where((TextDestination n) => Helpers.GetEnums<TextDestination>().All((TextDestination e) => e != n)).ForEach(delegate(TextDestination x)
		{
			Program.Settings.SecondaryTextUploaders.Remove(x);
		});
		Program.Settings.SecondaryFileUploaders.Where((FileDestination n) => Helpers.GetEnums<FileDestination>().All((FileDestination e) => e != n)).ForEach(delegate(FileDestination x)
		{
			Program.Settings.SecondaryFileUploaders.Remove(x);
		});
		lvSecondaryImageUploaders.Items.Clear();
		Extensions.ForEach(Program.Settings.SecondaryImageUploaders, delegate(ImageDestination x)
		{
			lvSecondaryImageUploaders.Items.Add(new ListViewItem(x.GetLocalizedDescription())
			{
				Tag = x
			});
		});
		lvSecondaryTextUploaders.Items.Clear();
		Extensions.ForEach(Program.Settings.SecondaryTextUploaders, delegate(TextDestination x)
		{
			lvSecondaryTextUploaders.Items.Add(new ListViewItem(x.GetLocalizedDescription())
			{
				Tag = x
			});
		});
		lvSecondaryFileUploaders.Items.Clear();
		Extensions.ForEach(Program.Settings.SecondaryFileUploaders, delegate(FileDestination x)
		{
			lvSecondaryFileUploaders.Items.Add(new ListViewItem(x.GetLocalizedDescription())
			{
				Tag = x
			});
		});
		cbHistorySaveTasks.Checked = Program.Settings.HistorySaveTasks;
		cbHistoryCheckURL.Checked = Program.Settings.HistoryCheckURL;
		cbRecentTasksSave.Checked = Program.Settings.RecentTasksSave;
		nudRecentTasksMaxCount.SetValue(Program.Settings.RecentTasksMaxCount);
		cbRecentTasksShowInMainWindow.Checked = Program.Settings.RecentTasksShowInMainWindow;
		cbRecentTasksShowInTrayMenu.Checked = Program.Settings.RecentTasksShowInTrayMenu;
		cbRecentTasksTrayMenuMostRecentFirst.Checked = Program.Settings.RecentTasksTrayMenuMostRecentFirst;
		cbDontShowPrintSettingDialog.Checked = Program.Settings.DontShowPrintSettingsDialog;
		cbPrintDontShowWindowsDialog.Checked = !Program.Settings.PrintSettings.ShowPrintDialog;
		txtDefaultPrinterOverride.Text = Program.Settings.PrintSettings.DefaultPrinterOverride;
		Label label = lblDefaultPrinterOverride;
		bool visible = (txtDefaultPrinterOverride.Visible = !Program.Settings.PrintSettings.ShowPrintDialog);
		label.Visible = visible;
		cbProxyMethod.SelectedIndex = (int)Program.Settings.ProxySettings.ProxyMethod;
		txtProxyUsername.Text = Program.Settings.ProxySettings.Username;
		txtProxyPassword.Text = Program.Settings.ProxySettings.Password;
		txtProxyHost.Text = Program.Settings.ProxySettings.Host ?? "";
		nudProxyPort.SetValue(Program.Settings.ProxySettings.Port);
		UpdateProxyControls();
		pgSettings.SelectedObject = Program.Settings;
		tttvMain.MainTabControl = tcSettings;
		ready = true;
	}

	private void ChangeLanguage(SupportedLanguage language)
	{
		btnLanguages.Text = language.GetLocalizedDescription();
		btnLanguages.Image = LanguageHelper.GetLanguageIcon(language);
		if (ready)
		{
			Program.Settings.Language = language;
			if (LanguageHelper.ChangeLanguage(Program.Settings.Language) && MessageBox.Show(Resources.ApplicationSettingsForm_cbLanguage_SelectedIndexChanged_Language_Restart, Resources.ShareXConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Program.Restart();
			}
		}
	}

	private void UpdateStartWithWindows()
	{
		ready = false;
		cbStartWithWindows.Text = Resources.ApplicationSettingsForm_cbStartWithWindows_Text;
		cbStartWithWindows.Enabled = false;
		try
		{
			StartupState startupState = StartupManagerSingletonProvider.CurrentStartupManager.State;
			cbStartWithWindows.Checked = startupState == StartupState.Enabled || startupState == StartupState.EnabledByPolicy;
			switch (startupState)
			{
			case StartupState.DisabledByUser:
				cbStartWithWindows.Text = Resources.ApplicationSettingsForm_cbStartWithWindows_DisabledByUser_Text;
				break;
			case StartupState.DisabledByPolicy:
				cbStartWithWindows.Text = Resources.ApplicationSettingsForm_cbStartWithWindows_DisabledByPolicy_Text;
				break;
			case StartupState.EnabledByPolicy:
				cbStartWithWindows.Text = Resources.ApplicationSettingsForm_cbStartWithWindows_EnabledByPolicy_Text;
				break;
			default:
				cbStartWithWindows.Enabled = true;
				break;
			}
		}
		catch (Exception e)
		{
			e.ShowError();
		}
		ready = true;
	}

	private void UpdateProxyControls()
	{
		switch (Program.Settings.ProxySettings.ProxyMethod)
		{
		case ProxyMethod.None:
		{
			TextBox textBox6 = txtProxyUsername;
			TextBox textBox7 = txtProxyPassword;
			TextBox textBox8 = txtProxyHost;
			bool flag4 = (nudProxyPort.Enabled = false);
			bool flag6 = (textBox8.Enabled = flag4);
			bool enabled = (textBox7.Enabled = flag6);
			textBox6.Enabled = enabled;
			break;
		}
		case ProxyMethod.Manual:
		{
			TextBox textBox3 = txtProxyUsername;
			TextBox textBox4 = txtProxyPassword;
			TextBox textBox5 = txtProxyHost;
			bool flag4 = (nudProxyPort.Enabled = true);
			bool flag6 = (textBox5.Enabled = flag4);
			bool enabled = (textBox4.Enabled = flag6);
			textBox3.Enabled = enabled;
			break;
		}
		case ProxyMethod.Automatic:
		{
			TextBox textBox = txtProxyUsername;
			bool enabled = (txtProxyPassword.Enabled = true);
			textBox.Enabled = enabled;
			TextBox textBox2 = txtProxyHost;
			enabled = (nudProxyPort.Enabled = false);
			textBox2.Enabled = enabled;
			break;
		}
		}
	}

	private void UpdatePersonalFolderPathPreview()
	{
		try
		{
			string text = FileHelpers.SanitizePath(txtPersonalFolderPath.Text);
			text = ((!string.IsNullOrEmpty(text)) ? FileHelpers.GetAbsolutePath(text) : ((!Program.Portable) ? Program.DefaultPersonalFolder : Program.PortablePersonalFolder));
			lblPreviewPersonalFolderPath.Text = text;
			btnPersonalFolderPathApply.Enabled = !text.Equals(lastPersonalPath, StringComparison.OrdinalIgnoreCase);
		}
		catch (Exception ex)
		{
			btnPersonalFolderPathApply.Enabled = false;
			lblPreviewPersonalFolderPath.Text = "Error: " + ex.Message;
		}
	}

	private void UpdateScreenshotsFolderPathPreview()
	{
		try
		{
			lblSaveImageSubFolderPatternPreview.Text = TaskHelpers.GetScreenshotsFolder();
		}
		catch (Exception ex)
		{
			lblSaveImageSubFolderPatternPreview.Text = "Error: " + ex.Message;
		}
	}

	private void cbShowTray_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.ShowTray = cbShowTray.Checked;
		if (ready)
		{
			Program.MainForm.niTray.Visible = Program.Settings.ShowTray;
		}
		cbSilentRun.Enabled = Program.Settings.ShowTray;
	}

	private void cbSilentRun_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.SilentRun = cbSilentRun.Checked;
	}

	private void cbTrayIconProgressEnabled_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.TrayIconProgressEnabled = cbTrayIconProgressEnabled.Checked;
	}

	private void cbTaskbarProgressEnabled_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.TaskbarProgressEnabled = cbTaskbarProgressEnabled.Checked;
		if (ready)
		{
			TaskbarManager.Enabled = Program.Settings.TaskbarProgressEnabled;
		}
	}

	private void CbUseWhiteShareXIcon_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.UseWhiteShareXIcon = cbUseWhiteShareXIcon.Checked;
	}

	private void cbRememberMainFormPosition_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.RememberMainFormPosition = cbRememberMainFormPosition.Checked;
	}

	private void cbRememberMainFormSize_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.RememberMainFormSize = cbRememberMainFormSize.Checked;
	}

	private void cbTrayLeftDoubleClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.TrayLeftDoubleClickAction = (HotkeyType)cbTrayLeftDoubleClickAction.SelectedIndex;
	}

	private void cbTrayLeftClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.TrayLeftClickAction = (HotkeyType)cbTrayLeftClickAction.SelectedIndex;
	}

	private void cbTrayMiddleClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.TrayMiddleClickAction = (HotkeyType)cbTrayMiddleClickAction.SelectedIndex;
	}

	private void btnEditQuickTaskMenu_Click(object sender, EventArgs e)
	{
		new QuickTaskMenuEditorForm().ShowDialog();
	}

	private void cbCheckPreReleaseUpdates_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.CheckPreReleaseUpdates = cbCheckPreReleaseUpdates.Checked;
	}

	private async void btnCheckDevBuild_Click(object sender, EventArgs e)
	{
		btnCheckDevBuild.Enabled = false;
		if (MessageBox.Show(Resources.ApplicationSettingsForm_btnCheckDevBuild_Click_DevBuilds_Warning, "ShareX", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
		{
			await TaskHelpers.DownloadAppVeyorBuild();
		}
		if (!base.IsDisposed)
		{
			btnCheckDevBuild.Enabled = true;
		}
	}

	private void UpdateThemeControls()
	{
		Button button = btnThemeAdd;
		ExportImportControl exportImportControl = eiTheme;
		Button button2 = btnThemeReset;
		bool flag = (pgTheme.Enabled = Program.Settings.UseCustomTheme);
		bool flag3 = (button2.Enabled = flag);
		bool enabled = (exportImportControl.Enabled = flag3);
		button.Enabled = enabled;
		ComboBox comboBox = cbThemes;
		enabled = (btnThemeRemove.Enabled = Program.Settings.UseCustomTheme && cbThemes.Items.Count > 0);
		comboBox.Enabled = enabled;
	}

	private void ApplySelectedTheme()
	{
		Program.MainForm.UpdateTheme();
		ShareXResources.ApplyTheme(this);
	}

	private void AddTheme(ShareXTheme theme)
	{
		if (theme != null)
		{
			Program.Settings.Themes.Add(theme);
			cbThemes.Items.Add(theme);
			int num = Program.Settings.Themes.Count - 1;
			Program.Settings.SelectedTheme = num;
			cbThemes.SelectedIndex = num;
			UpdateThemeControls();
		}
	}

	private void CbUseCustomTheme_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.UseCustomTheme = cbUseCustomTheme.Checked;
		UpdateThemeControls();
		ApplySelectedTheme();
	}

	private void CbThemes_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.SelectedTheme = cbThemes.SelectedIndex;
		if (cbThemes.SelectedItem != null)
		{
			pgTheme.SelectedObject = cbThemes.SelectedItem;
		}
		else
		{
			pgTheme.SelectedObject = null;
		}
		UpdateThemeControls();
		ApplySelectedTheme();
	}

	private void BtnThemeAdd_Click(object sender, EventArgs e)
	{
		AddTheme(ShareXTheme.DarkTheme);
	}

	private void BtnThemeRemove_Click(object sender, EventArgs e)
	{
		int selectedIndex = cbThemes.SelectedIndex;
		if (selectedIndex > -1)
		{
			Program.Settings.Themes.RemoveAt(selectedIndex);
			cbThemes.Items.RemoveAt(selectedIndex);
			selectedIndex = ((Program.Settings.Themes.Count <= 0) ? (-1) : 0);
			Program.Settings.SelectedTheme = selectedIndex;
			cbThemes.SelectedIndex = selectedIndex;
			pgTheme.SelectedObject = cbThemes.SelectedItem;
			UpdateThemeControls();
		}
	}

	private object EiTheme_ExportRequested()
	{
		return pgTheme.SelectedObject as ShareXTheme;
	}

	private void EiTheme_ImportRequested(object obj)
	{
		AddTheme(obj as ShareXTheme);
	}

	private void BtnThemeReset_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show(Resources.WouldYouLikeToResetThemes, "ShareX - " + Resources.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
		{
			Program.Settings.Themes = ShareXTheme.GetDefaultThemes();
			Program.Settings.SelectedTheme = 0;
			cbThemes.Items.Clear();
			ComboBox.ObjectCollection items = cbThemes.Items;
			object[] items2 = Program.Settings.Themes.ToArray();
			items.AddRange(items2);
			cbThemes.SelectedIndex = Program.Settings.SelectedTheme;
			pgTheme.SelectedObject = Program.Settings.Themes[Program.Settings.SelectedTheme];
		}
	}

	private void pgTheme_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		UpdateThemeControls();
		ApplySelectedTheme();
	}

	private void cbStartWithWindows_CheckedChanged(object sender, EventArgs e)
	{
		if (ready)
		{
			try
			{
				StartupManagerSingletonProvider.CurrentStartupManager.State = (cbStartWithWindows.Checked ? StartupState.Enabled : StartupState.Disabled);
				UpdateStartWithWindows();
			}
			catch (Exception e2)
			{
				e2.ShowError();
			}
		}
	}

	private void cbShellContextMenu_CheckedChanged(object sender, EventArgs e)
	{
		if (ready)
		{
			IntegrationHelpers.CreateShellContextMenuButton(cbShellContextMenu.Checked);
		}
	}

	private void cbEditWithShareX_CheckedChanged(object sender, EventArgs e)
	{
		if (ready)
		{
			IntegrationHelpers.CreateEditShellContextMenuButton(cbEditWithShareX.Checked);
		}
	}

	private void cbSendToMenu_CheckedChanged(object sender, EventArgs e)
	{
		if (ready)
		{
			IntegrationHelpers.CreateSendToMenuButton(cbSendToMenu.Checked);
		}
	}

	private void cbChromeExtensionSupport_CheckedChanged(object sender, EventArgs e)
	{
		if (ready)
		{
			IntegrationHelpers.CreateChromeExtensionSupport(cbChromeExtensionSupport.Checked);
			btnChromeOpenExtensionPage.Enabled = cbChromeExtensionSupport.Checked;
		}
	}

	private void btnChromeOpenExtensionPage_Click(object sender, EventArgs e)
	{
		URLHelpers.OpenURL("https://chrome.google.com/webstore/detail/sharex/nlkoigbdolhchiicbonbihbphgamnaoc");
	}

	private void cbFirefoxAddonSupport_CheckedChanged(object sender, EventArgs e)
	{
		if (ready)
		{
			IntegrationHelpers.CreateFirefoxAddonSupport(cbFirefoxAddonSupport.Checked);
			btnFirefoxOpenAddonPage.Enabled = cbFirefoxAddonSupport.Checked;
		}
	}

	private void btnFirefoxOpenAddonPage_Click(object sender, EventArgs e)
	{
		URLHelpers.OpenURL("https://addons.mozilla.org/en-US/firefox/addon/sharex/");
	}

	private void cbSteamShowInApp_CheckedChanged(object sender, EventArgs e)
	{
		if (ready)
		{
			IntegrationHelpers.SteamShowInApp(cbSteamShowInApp.Checked);
		}
	}

	private void txtPersonalFolderPath_TextChanged(object sender, EventArgs e)
	{
		UpdatePersonalFolderPathPreview();
	}

	private void btnBrowsePersonalFolderPath_Click(object sender, EventArgs e)
	{
		FileHelpers.BrowseFolder(Resources.ApplicationSettingsForm_btnBrowsePersonalFolderPath_Click_Choose_ShareX_personal_folder_path, txtPersonalFolderPath, Program.PersonalFolder, detectSpecialFolders: true);
	}

	private void btnPersonalFolderPathApply_Click(object sender, EventArgs e)
	{
		string text = FileHelpers.SanitizePath(txtPersonalFolderPath.Text);
		if (!text.Equals(lastPersonalPath, StringComparison.OrdinalIgnoreCase) && Program.WritePersonalPathConfig(text))
		{
			lastPersonalPath = text;
			btnPersonalFolderPathApply.Enabled = false;
			if (MessageBox.Show(Resources.ShareXNeedsToBeRestartedForThePersonalFolderChangesToApply, Resources.ShareXConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Program.Restart();
			}
		}
	}

	private void btnOpenPersonalFolder_Click(object sender, EventArgs e)
	{
		FileHelpers.OpenFolder(lblPreviewPersonalFolderPath.Text);
	}

	private void cbUseCustomScreenshotsPath_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.UseCustomScreenshotsPath = cbUseCustomScreenshotsPath.Checked;
		UpdateScreenshotsFolderPathPreview();
	}

	private void txtCustomScreenshotsPath_TextChanged(object sender, EventArgs e)
	{
		Program.Settings.CustomScreenshotsPath = FileHelpers.SanitizePath(txtCustomScreenshotsPath.Text);
		UpdateScreenshotsFolderPathPreview();
	}

	private void btnBrowseCustomScreenshotsPath_Click(object sender, EventArgs e)
	{
		FileHelpers.BrowseFolder(Resources.ApplicationSettingsForm_btnBrowseCustomScreenshotsPath_Click_Choose_screenshots_folder_path, txtCustomScreenshotsPath, Program.PersonalFolder, detectSpecialFolders: true);
	}

	private void txtSaveImageSubFolderPattern_TextChanged(object sender, EventArgs e)
	{
		Program.Settings.SaveImageSubFolderPattern = FileHelpers.SanitizePath(txtSaveImageSubFolderPattern.Text);
		UpdateScreenshotsFolderPathPreview();
	}

	private void btnOpenScreenshotsFolder_Click(object sender, EventArgs e)
	{
		FileHelpers.OpenFolder(lblSaveImageSubFolderPatternPreview.Text);
	}

	private void txtSaveImageSubFolderPatternWindow_TextChanged(object sender, EventArgs e)
	{
		Program.Settings.SaveImageSubFolderPatternWindow = FileHelpers.SanitizePath(txtSaveImageSubFolderPatternWindow.Text);
	}

	private void cbExportSettings_CheckedChanged(object sender, EventArgs e)
	{
		btnExport.Enabled = cbExportSettings.Checked || cbExportHistory.Checked;
	}

	private void cbExportHistory_CheckedChanged(object sender, EventArgs e)
	{
		btnExport.Enabled = cbExportSettings.Checked || cbExportHistory.Checked;
	}

	private async void btnExport_Click(object sender, EventArgs e)
	{
		bool exportSettings = cbExportSettings.Checked;
		bool exportHistory = cbExportHistory.Checked;
		if (!(exportSettings || exportHistory))
		{
			return;
		}
		using SaveFileDialog sfd = new SaveFileDialog();
		sfd.DefaultExt = "sxb";
		sfd.FileName = "ShareX-" + Application.ProductVersion + "-backup.sxb";
		sfd.Filter = "ShareX backup (*.sxb)|*.sxb|All files (*.*)|*.*";
		if (sfd.ShowDialog() == DialogResult.OK)
		{
			btnExport.Enabled = false;
			btnImport.Enabled = false;
			pbExportImport.Location = btnExport.Location;
			pbExportImport.Visible = true;
			string exportPath = sfd.FileName;
			DebugHelper.WriteLine("Export started: " + exportPath);
			await Task.Run(delegate
			{
				SettingManager.SaveAllSettings();
				SettingManager.Export(exportPath, exportSettings, exportHistory);
			});
			if (!base.IsDisposed)
			{
				pbExportImport.Visible = false;
				btnExport.Enabled = true;
				btnImport.Enabled = true;
			}
			DebugHelper.WriteLine("Export completed: " + exportPath);
		}
	}

	private async void btnImport_Click(object sender, EventArgs e)
	{
		using OpenFileDialog ofd = new OpenFileDialog();
		ofd.Filter = "ShareX backup (*.sxb)|*.sxb|All files (*.*)|*.*";
		if (ofd.ShowDialog() == DialogResult.OK)
		{
			btnExport.Enabled = false;
			btnImport.Enabled = false;
			pbExportImport.Location = btnImport.Location;
			pbExportImport.Visible = true;
			string importPath = ofd.FileName;
			DebugHelper.WriteLine("Import started: " + importPath);
			await Task.Run(delegate
			{
				SettingManager.Import(importPath);
				SettingManager.LoadAllSettings();
			});
			if (!base.IsDisposed)
			{
				UpdateControls();
				pbExportImport.Visible = false;
				btnExport.Enabled = true;
				btnImport.Enabled = true;
			}
			LanguageHelper.ChangeLanguage(Program.Settings.Language);
			Program.MainForm.UpdateControls();
			DebugHelper.WriteLine("Import completed: " + importPath);
		}
	}

	private void btnResetSettings_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show(Resources.ApplicationSettingsForm_btnResetSettings_Click_WouldYouLikeToResetShareXSettings, "ShareX - " + Resources.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
		{
			SettingManager.ResetSettings();
			SettingManager.SaveAllSettings();
			UpdateControls();
			LanguageHelper.ChangeLanguage(Program.Settings.Language);
			Program.MainForm.UpdateControls();
			DebugHelper.WriteLine("Settings reset.");
		}
	}

	private void cbAutomaticallyCleanupBackupFiles_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.AutoCleanupBackupFiles = cbAutomaticallyCleanupBackupFiles.Checked;
	}

	private void cbAutomaticallyCleanupLogFiles_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.AutoCleanupLogFiles = cbAutomaticallyCleanupLogFiles.Checked;
	}

	private void nudCleanupKeepFileCount_ValueChanged(object sender, EventArgs e)
	{
		Program.Settings.CleanupKeepFileCount = (int)nudCleanupKeepFileCount.Value;
	}

	private void cbMainWindowShowMenu_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.ShowMenu = cbMainWindowShowMenu.Checked;
	}

	private void cbMainWindowTaskViewMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.TaskViewMode = (TaskViewMode)cbMainWindowTaskViewMode.SelectedIndex;
	}

	private void cbThumbnailViewShowTitle_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.ShowThumbnailTitle = cbThumbnailViewShowTitle.Checked;
	}

	private void cbThumbnailViewTitleLocation_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.ThumbnailTitleLocation = (ThumbnailTitleLocation)cbThumbnailViewTitleLocation.SelectedIndex;
	}

	private void nudThumbnailViewThumbnailSizeWidth_ValueChanged(object sender, EventArgs e)
	{
		Program.Settings.ThumbnailSize = new Size((int)nudThumbnailViewThumbnailSizeWidth.Value, Program.Settings.ThumbnailSize.Height);
	}

	private void nudThumbnailViewThumbnailSizeHeight_ValueChanged(object sender, EventArgs e)
	{
		Program.Settings.ThumbnailSize = new Size(Program.Settings.ThumbnailSize.Width, (int)nudThumbnailViewThumbnailSizeHeight.Value);
	}

	private void btnThumbnailViewThumbnailSizeReset_Click(object sender, EventArgs e)
	{
		nudThumbnailViewThumbnailSizeWidth.SetValue(200m);
		nudThumbnailViewThumbnailSizeHeight.SetValue(150m);
	}

	private void cbThumbnailViewThumbnailClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.ThumbnailClickAction = (ThumbnailViewClickAction)cbThumbnailViewThumbnailClickAction.SelectedIndex;
	}

	private void cbListViewShowColumns_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.ShowColumns = cbListViewShowColumns.Checked;
	}

	private void cbListViewImagePreviewVisibility_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.ImagePreview = (ImagePreviewVisibility)cbListViewImagePreviewVisibility.SelectedIndex;
	}

	private void cbListViewImagePreviewLocation_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.ImagePreviewLocation = (ImagePreviewLocation)cbListViewImagePreviewLocation.SelectedIndex;
	}

	private void AddClipboardFormat(ClipboardFormat cf)
	{
		ListViewItem listViewItem = new ListViewItem(cf.Description ?? "");
		listViewItem.Tag = cf;
		listViewItem.SubItems.Add(cf.Format ?? "");
		lvClipboardFormats.Items.Add(listViewItem);
	}

	private void ClipboardFormatsEditSelected()
	{
		if (lvClipboardFormats.SelectedItems.Count <= 0)
		{
			return;
		}
		ListViewItem listViewItem = lvClipboardFormats.SelectedItems[0];
		using ClipboardFormatForm clipboardFormatForm = new ClipboardFormatForm(listViewItem.Tag as ClipboardFormat);
		if (clipboardFormatForm.ShowDialog() == DialogResult.OK)
		{
			listViewItem.Text = clipboardFormatForm.ClipboardFormat.Description ?? "";
			listViewItem.Tag = clipboardFormatForm.ClipboardFormat;
			listViewItem.SubItems[1].Text = clipboardFormatForm.ClipboardFormat.Format ?? "";
		}
	}

	private void lvClipboardFormats_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ClipboardFormatsEditSelected();
		}
	}

	private void btnAddClipboardFormat_Click(object sender, EventArgs e)
	{
		using ClipboardFormatForm clipboardFormatForm = new ClipboardFormatForm();
		if (clipboardFormatForm.ShowDialog() == DialogResult.OK)
		{
			ClipboardFormat clipboardFormat = clipboardFormatForm.ClipboardFormat;
			Program.Settings.ClipboardContentFormats.Add(clipboardFormat);
			AddClipboardFormat(clipboardFormat);
		}
	}

	private void btnClipboardFormatEdit_Click(object sender, EventArgs e)
	{
		ClipboardFormatsEditSelected();
	}

	private void btnClipboardFormatRemove_Click(object sender, EventArgs e)
	{
		if (lvClipboardFormats.SelectedItems.Count > 0)
		{
			ListViewItem listViewItem = lvClipboardFormats.SelectedItems[0];
			ClipboardFormat item = listViewItem.Tag as ClipboardFormat;
			Program.Settings.ClipboardContentFormats.Remove(item);
			lvClipboardFormats.Items.Remove(listViewItem);
		}
	}

	private void nudUploadLimit_ValueChanged(object sender, EventArgs e)
	{
		Program.Settings.UploadLimit = (int)nudUploadLimit.Value;
	}

	private void cbBufferSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.BufferSizePower = cbBufferSize.SelectedIndex;
	}

	private void nudRetryUpload_ValueChanged(object sender, EventArgs e)
	{
		Program.Settings.MaxUploadFailRetry = (int)nudRetryUpload.Value;
	}

	private void cbUseSecondaryUploaders_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.UseSecondaryUploaders = cbUseSecondaryUploaders.Checked;
	}

	private void lvSecondaryUploaders_MouseUp(object sender, MouseEventArgs e)
	{
		Program.Settings.SecondaryImageUploaders = (from ListViewItem x in lvSecondaryImageUploaders.Items
			select (ImageDestination)x.Tag).ToList();
		Program.Settings.SecondaryTextUploaders = (from ListViewItem x in lvSecondaryTextUploaders.Items
			select (TextDestination)x.Tag).ToList();
		Program.Settings.SecondaryFileUploaders = (from ListViewItem x in lvSecondaryFileUploaders.Items
			select (FileDestination)x.Tag).ToList();
	}

	private void cbHistorySaveTasks_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.HistorySaveTasks = cbHistorySaveTasks.Checked;
	}

	private void cbHistoryCheckURL_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.HistoryCheckURL = cbHistoryCheckURL.Checked;
	}

	private void cbRecentTasksSave_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.RecentTasksSave = cbRecentTasksSave.Checked;
	}

	private void nudRecentTasksMaxCount_ValueChanged(object sender, EventArgs e)
	{
		Program.Settings.RecentTasksMaxCount = (int)nudRecentTasksMaxCount.Value;
	}

	private void cbRecentTasksShowInMainWindow_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.RecentTasksShowInMainWindow = cbRecentTasksShowInMainWindow.Checked;
	}

	private void cbRecentTasksShowInTrayMenu_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.RecentTasksShowInTrayMenu = cbRecentTasksShowInTrayMenu.Checked;
	}

	private void cbRecentTasksTrayMenuMostRecentFirst_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.RecentTasksTrayMenuMostRecentFirst = cbRecentTasksTrayMenuMostRecentFirst.Checked;
	}

	private void cbDontShowPrintSettingDialog_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.DontShowPrintSettingsDialog = cbDontShowPrintSettingDialog.Checked;
	}

	private void btnShowImagePrintSettings_Click(object sender, EventArgs e)
	{
		using Image img = TaskHelpers.GetScreenshot().CaptureActiveMonitor();
		using PrintForm printForm = new PrintForm(img, Program.Settings.PrintSettings, previewOnly: true);
		printForm.ShowDialog();
	}

	private void cbPrintDontShowWindowsDialog_CheckedChanged(object sender, EventArgs e)
	{
		Program.Settings.PrintSettings.ShowPrintDialog = !cbPrintDontShowWindowsDialog.Checked;
		Label label = lblDefaultPrinterOverride;
		bool visible = (txtDefaultPrinterOverride.Visible = !Program.Settings.PrintSettings.ShowPrintDialog);
		label.Visible = visible;
	}

	private void txtDefaultPrinterOverride_TextChanged(object sender, EventArgs e)
	{
		Program.Settings.PrintSettings.DefaultPrinterOverride = txtDefaultPrinterOverride.Text;
	}

	private void cbProxyMethod_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.Settings.ProxySettings.ProxyMethod = (ProxyMethod)cbProxyMethod.SelectedIndex;
		if (Program.Settings.ProxySettings.ProxyMethod == ProxyMethod.Automatic)
		{
			Program.Settings.ProxySettings.IsValidProxy();
			txtProxyHost.Text = Program.Settings.ProxySettings.Host ?? "";
			nudProxyPort.SetValue(Program.Settings.ProxySettings.Port);
		}
		UpdateProxyControls();
	}

	private void txtProxyUsername_TextChanged(object sender, EventArgs e)
	{
		Program.Settings.ProxySettings.Username = txtProxyUsername.Text;
	}

	private void txtProxyPassword_TextChanged(object sender, EventArgs e)
	{
		Program.Settings.ProxySettings.Password = txtProxyPassword.Text;
	}

	private void txtProxyHost_TextChanged(object sender, EventArgs e)
	{
		Program.Settings.ProxySettings.Host = txtProxyHost.Text;
	}

	private void nudProxyPort_ValueChanged(object sender, EventArgs e)
	{
		Program.Settings.ProxySettings.Port = (int)nudProxyPort.Value;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.ApplicationSettingsForm));
		this.tcSettings = new System.Windows.Forms.TabControl();
		this.tpGeneral = new System.Windows.Forms.TabPage();
		this.cbUseWhiteShareXIcon = new System.Windows.Forms.CheckBox();
		this.btnCheckDevBuild = new System.Windows.Forms.Button();
		this.cbCheckPreReleaseUpdates = new System.Windows.Forms.CheckBox();
		this.cbTrayMiddleClickAction = new System.Windows.Forms.ComboBox();
		this.lblTrayMiddleClickAction = new System.Windows.Forms.Label();
		this.cbTrayLeftDoubleClickAction = new System.Windows.Forms.ComboBox();
		this.lblTrayLeftDoubleClickAction = new System.Windows.Forms.Label();
		this.cbTrayLeftClickAction = new System.Windows.Forms.ComboBox();
		this.lblTrayLeftClickAction = new System.Windows.Forms.Label();
		this.btnEditQuickTaskMenu = new System.Windows.Forms.Button();
		this.cbShowTray = new System.Windows.Forms.CheckBox();
		this.cbTrayIconProgressEnabled = new System.Windows.Forms.CheckBox();
		this.btnLanguages = new ShareX.HelpersLib.MenuButton();
		this.cmsLanguages = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.cbRememberMainFormPosition = new System.Windows.Forms.CheckBox();
		this.cbSilentRun = new System.Windows.Forms.CheckBox();
		this.cbTaskbarProgressEnabled = new System.Windows.Forms.CheckBox();
		this.cbRememberMainFormSize = new System.Windows.Forms.CheckBox();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.tpTheme = new System.Windows.Forms.TabPage();
		this.btnThemeReset = new System.Windows.Forms.Button();
		this.btnThemeRemove = new System.Windows.Forms.Button();
		this.btnThemeAdd = new System.Windows.Forms.Button();
		this.cbThemes = new System.Windows.Forms.ComboBox();
		this.pgTheme = new System.Windows.Forms.PropertyGrid();
		this.cbUseCustomTheme = new System.Windows.Forms.CheckBox();
		this.eiTheme = new ShareX.HelpersLib.ExportImportControl();
		this.tpIntegration = new System.Windows.Forms.TabPage();
		this.gbFirefox = new System.Windows.Forms.GroupBox();
		this.cbFirefoxAddonSupport = new System.Windows.Forms.CheckBox();
		this.btnFirefoxOpenAddonPage = new System.Windows.Forms.Button();
		this.gbSteam = new System.Windows.Forms.GroupBox();
		this.cbSteamShowInApp = new System.Windows.Forms.CheckBox();
		this.gbChrome = new System.Windows.Forms.GroupBox();
		this.cbChromeExtensionSupport = new System.Windows.Forms.CheckBox();
		this.btnChromeOpenExtensionPage = new System.Windows.Forms.Button();
		this.gbWindows = new System.Windows.Forms.GroupBox();
		this.cbEditWithShareX = new System.Windows.Forms.CheckBox();
		this.cbStartWithWindows = new System.Windows.Forms.CheckBox();
		this.cbSendToMenu = new System.Windows.Forms.CheckBox();
		this.cbShellContextMenu = new System.Windows.Forms.CheckBox();
		this.tpPaths = new System.Windows.Forms.TabPage();
		this.btnPersonalFolderPathApply = new System.Windows.Forms.Button();
		this.btnOpenScreenshotsFolder = new System.Windows.Forms.Button();
		this.lblPreviewPersonalFolderPath = new System.Windows.Forms.Label();
		this.btnBrowsePersonalFolderPath = new System.Windows.Forms.Button();
		this.lblPersonalFolderPath = new System.Windows.Forms.Label();
		this.txtPersonalFolderPath = new System.Windows.Forms.TextBox();
		this.btnBrowseCustomScreenshotsPath = new System.Windows.Forms.Button();
		this.btnOpenPersonalFolderPath = new System.Windows.Forms.Button();
		this.txtCustomScreenshotsPath = new System.Windows.Forms.TextBox();
		this.cbUseCustomScreenshotsPath = new System.Windows.Forms.CheckBox();
		this.lblSaveImageSubFolderPattern = new System.Windows.Forms.Label();
		this.lblSaveImageSubFolderPatternPreview = new System.Windows.Forms.Label();
		this.txtSaveImageSubFolderPattern = new System.Windows.Forms.TextBox();
		this.tpSettings = new System.Windows.Forms.TabPage();
		this.cbAutomaticallyCleanupLogFiles = new System.Windows.Forms.CheckBox();
		this.nudCleanupKeepFileCount = new System.Windows.Forms.NumericUpDown();
		this.lblCleanupKeepFileCount = new System.Windows.Forms.Label();
		this.cbAutomaticallyCleanupBackupFiles = new System.Windows.Forms.CheckBox();
		this.pbExportImportNote = new System.Windows.Forms.PictureBox();
		this.cbExportHistory = new System.Windows.Forms.CheckBox();
		this.cbExportSettings = new System.Windows.Forms.CheckBox();
		this.lblExportImportNote = new System.Windows.Forms.Label();
		this.btnResetSettings = new System.Windows.Forms.Button();
		this.pbExportImport = new System.Windows.Forms.ProgressBar();
		this.btnExport = new System.Windows.Forms.Button();
		this.btnImport = new System.Windows.Forms.Button();
		this.tpMainWindow = new System.Windows.Forms.TabPage();
		this.gbListView = new System.Windows.Forms.GroupBox();
		this.cbListViewImagePreviewLocation = new System.Windows.Forms.ComboBox();
		this.lblListViewImagePreviewLocation = new System.Windows.Forms.Label();
		this.cbListViewImagePreviewVisibility = new System.Windows.Forms.ComboBox();
		this.lblListViewImagePreviewVisibility = new System.Windows.Forms.Label();
		this.cbListViewShowColumns = new System.Windows.Forms.CheckBox();
		this.gbThumbnailView = new System.Windows.Forms.GroupBox();
		this.btnThumbnailViewThumbnailSizeReset = new System.Windows.Forms.Button();
		this.lblThumbnailViewThumbnailSizeX = new System.Windows.Forms.Label();
		this.nudThumbnailViewThumbnailSizeHeight = new System.Windows.Forms.NumericUpDown();
		this.nudThumbnailViewThumbnailSizeWidth = new System.Windows.Forms.NumericUpDown();
		this.cbThumbnailViewThumbnailClickAction = new System.Windows.Forms.ComboBox();
		this.lblThumbnailViewThumbnailClickAction = new System.Windows.Forms.Label();
		this.lblThumbnailViewThumbnailSize = new System.Windows.Forms.Label();
		this.cbThumbnailViewTitleLocation = new System.Windows.Forms.ComboBox();
		this.lblThumbnailViewTitleLocation = new System.Windows.Forms.Label();
		this.cbThumbnailViewShowTitle = new System.Windows.Forms.CheckBox();
		this.cbMainWindowShowMenu = new System.Windows.Forms.CheckBox();
		this.cbMainWindowTaskViewMode = new System.Windows.Forms.ComboBox();
		this.lblMainWindowTaskViewMode = new System.Windows.Forms.Label();
		this.tpClipboardFormats = new System.Windows.Forms.TabPage();
		this.lblClipboardFormatsTip = new System.Windows.Forms.Label();
		this.btnClipboardFormatEdit = new System.Windows.Forms.Button();
		this.btnClipboardFormatRemove = new System.Windows.Forms.Button();
		this.btnClipboardFormatAdd = new System.Windows.Forms.Button();
		this.lvClipboardFormats = new ShareX.HelpersLib.MyListView();
		this.chDescription = new System.Windows.Forms.ColumnHeader();
		this.chFormat = new System.Windows.Forms.ColumnHeader();
		this.tpUpload = new System.Windows.Forms.TabPage();
		this.gbSecondaryFileUploaders = new System.Windows.Forms.GroupBox();
		this.lvSecondaryFileUploaders = new ShareX.HelpersLib.MyListView();
		this.chSecondaryFileUploaders = new System.Windows.Forms.ColumnHeader();
		this.lblUploadLimit = new System.Windows.Forms.Label();
		this.gbSecondaryImageUploaders = new System.Windows.Forms.GroupBox();
		this.lvSecondaryImageUploaders = new ShareX.HelpersLib.MyListView();
		this.chSecondaryImageUploaders = new System.Windows.Forms.ColumnHeader();
		this.gbSecondaryTextUploaders = new System.Windows.Forms.GroupBox();
		this.lvSecondaryTextUploaders = new ShareX.HelpersLib.MyListView();
		this.chSecondaryTextUploaders = new System.Windows.Forms.ColumnHeader();
		this.nudUploadLimit = new System.Windows.Forms.NumericUpDown();
		this.cbUseSecondaryUploaders = new System.Windows.Forms.CheckBox();
		this.lblUploadLimitHint = new System.Windows.Forms.Label();
		this.cbIfUploadFailRetryOnce = new System.Windows.Forms.Label();
		this.lblBufferSize = new System.Windows.Forms.Label();
		this.nudRetryUpload = new System.Windows.Forms.NumericUpDown();
		this.cbBufferSize = new System.Windows.Forms.ComboBox();
		this.tpHistory = new System.Windows.Forms.TabPage();
		this.gbHistory = new System.Windows.Forms.GroupBox();
		this.cbHistoryCheckURL = new System.Windows.Forms.CheckBox();
		this.cbHistorySaveTasks = new System.Windows.Forms.CheckBox();
		this.gbRecentLinks = new System.Windows.Forms.GroupBox();
		this.cbRecentTasksTrayMenuMostRecentFirst = new System.Windows.Forms.CheckBox();
		this.lblRecentTasksMaxCount = new System.Windows.Forms.Label();
		this.nudRecentTasksMaxCount = new System.Windows.Forms.NumericUpDown();
		this.cbRecentTasksShowInTrayMenu = new System.Windows.Forms.CheckBox();
		this.cbRecentTasksShowInMainWindow = new System.Windows.Forms.CheckBox();
		this.cbRecentTasksSave = new System.Windows.Forms.CheckBox();
		this.tpPrint = new System.Windows.Forms.TabPage();
		this.lblDefaultPrinterOverride = new System.Windows.Forms.Label();
		this.txtDefaultPrinterOverride = new System.Windows.Forms.TextBox();
		this.cbPrintDontShowWindowsDialog = new System.Windows.Forms.CheckBox();
		this.cbDontShowPrintSettingDialog = new System.Windows.Forms.CheckBox();
		this.btnShowImagePrintSettings = new System.Windows.Forms.Button();
		this.tpProxy = new System.Windows.Forms.TabPage();
		this.cbProxyMethod = new System.Windows.Forms.ComboBox();
		this.lblProxyMethod = new System.Windows.Forms.Label();
		this.lblProxyHost = new System.Windows.Forms.Label();
		this.txtProxyHost = new System.Windows.Forms.TextBox();
		this.nudProxyPort = new System.Windows.Forms.NumericUpDown();
		this.lblProxyPort = new System.Windows.Forms.Label();
		this.lblProxyPassword = new System.Windows.Forms.Label();
		this.txtProxyPassword = new System.Windows.Forms.TextBox();
		this.lblProxyUsername = new System.Windows.Forms.Label();
		this.txtProxyUsername = new System.Windows.Forms.TextBox();
		this.tpAdvanced = new System.Windows.Forms.TabPage();
		this.pgSettings = new System.Windows.Forms.PropertyGrid();
		this.tttvMain = new ShareX.HelpersLib.TabToTreeView();
		this.lblSaveImageSubFolderPatternWindow = new System.Windows.Forms.Label();
		this.txtSaveImageSubFolderPatternWindow = new System.Windows.Forms.TextBox();
		this.tcSettings.SuspendLayout();
		this.tpGeneral.SuspendLayout();
		this.tpTheme.SuspendLayout();
		this.tpIntegration.SuspendLayout();
		this.gbFirefox.SuspendLayout();
		this.gbSteam.SuspendLayout();
		this.gbChrome.SuspendLayout();
		this.gbWindows.SuspendLayout();
		this.tpPaths.SuspendLayout();
		this.tpSettings.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudCleanupKeepFileCount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pbExportImportNote).BeginInit();
		this.tpMainWindow.SuspendLayout();
		this.gbListView.SuspendLayout();
		this.gbThumbnailView.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudThumbnailViewThumbnailSizeHeight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudThumbnailViewThumbnailSizeWidth).BeginInit();
		this.tpClipboardFormats.SuspendLayout();
		this.tpUpload.SuspendLayout();
		this.gbSecondaryFileUploaders.SuspendLayout();
		this.gbSecondaryImageUploaders.SuspendLayout();
		this.gbSecondaryTextUploaders.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudUploadLimit).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudRetryUpload).BeginInit();
		this.tpHistory.SuspendLayout();
		this.gbHistory.SuspendLayout();
		this.gbRecentLinks.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRecentTasksMaxCount).BeginInit();
		this.tpPrint.SuspendLayout();
		this.tpProxy.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudProxyPort).BeginInit();
		this.tpAdvanced.SuspendLayout();
		base.SuspendLayout();
		resources.ApplyResources(this.tcSettings, "tcSettings");
		this.tcSettings.Controls.Add(this.tpGeneral);
		this.tcSettings.Controls.Add(this.tpTheme);
		this.tcSettings.Controls.Add(this.tpIntegration);
		this.tcSettings.Controls.Add(this.tpPaths);
		this.tcSettings.Controls.Add(this.tpSettings);
		this.tcSettings.Controls.Add(this.tpMainWindow);
		this.tcSettings.Controls.Add(this.tpClipboardFormats);
		this.tcSettings.Controls.Add(this.tpUpload);
		this.tcSettings.Controls.Add(this.tpHistory);
		this.tcSettings.Controls.Add(this.tpPrint);
		this.tcSettings.Controls.Add(this.tpProxy);
		this.tcSettings.Controls.Add(this.tpAdvanced);
		this.tcSettings.Name = "tcSettings";
		this.tcSettings.SelectedIndex = 0;
		this.tpGeneral.BackColor = System.Drawing.SystemColors.Window;
		this.tpGeneral.Controls.Add(this.cbUseWhiteShareXIcon);
		this.tpGeneral.Controls.Add(this.btnCheckDevBuild);
		this.tpGeneral.Controls.Add(this.cbCheckPreReleaseUpdates);
		this.tpGeneral.Controls.Add(this.cbTrayMiddleClickAction);
		this.tpGeneral.Controls.Add(this.lblTrayMiddleClickAction);
		this.tpGeneral.Controls.Add(this.cbTrayLeftDoubleClickAction);
		this.tpGeneral.Controls.Add(this.lblTrayLeftDoubleClickAction);
		this.tpGeneral.Controls.Add(this.cbTrayLeftClickAction);
		this.tpGeneral.Controls.Add(this.lblTrayLeftClickAction);
		this.tpGeneral.Controls.Add(this.btnEditQuickTaskMenu);
		this.tpGeneral.Controls.Add(this.cbShowTray);
		this.tpGeneral.Controls.Add(this.cbTrayIconProgressEnabled);
		this.tpGeneral.Controls.Add(this.btnLanguages);
		this.tpGeneral.Controls.Add(this.cbRememberMainFormPosition);
		this.tpGeneral.Controls.Add(this.cbSilentRun);
		this.tpGeneral.Controls.Add(this.cbTaskbarProgressEnabled);
		this.tpGeneral.Controls.Add(this.cbRememberMainFormSize);
		this.tpGeneral.Controls.Add(this.lblLanguage);
		resources.ApplyResources(this.tpGeneral, "tpGeneral");
		this.tpGeneral.Name = "tpGeneral";
		resources.ApplyResources(this.cbUseWhiteShareXIcon, "cbUseWhiteShareXIcon");
		this.cbUseWhiteShareXIcon.Name = "cbUseWhiteShareXIcon";
		this.cbUseWhiteShareXIcon.UseVisualStyleBackColor = true;
		this.cbUseWhiteShareXIcon.CheckedChanged += new System.EventHandler(CbUseWhiteShareXIcon_CheckedChanged);
		resources.ApplyResources(this.btnCheckDevBuild, "btnCheckDevBuild");
		this.btnCheckDevBuild.Name = "btnCheckDevBuild";
		this.btnCheckDevBuild.UseVisualStyleBackColor = true;
		this.btnCheckDevBuild.Click += new System.EventHandler(btnCheckDevBuild_Click);
		resources.ApplyResources(this.cbCheckPreReleaseUpdates, "cbCheckPreReleaseUpdates");
		this.cbCheckPreReleaseUpdates.Name = "cbCheckPreReleaseUpdates";
		this.cbCheckPreReleaseUpdates.UseVisualStyleBackColor = true;
		this.cbCheckPreReleaseUpdates.CheckedChanged += new System.EventHandler(cbCheckPreReleaseUpdates_CheckedChanged);
		this.cbTrayMiddleClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbTrayMiddleClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbTrayMiddleClickAction, "cbTrayMiddleClickAction");
		this.cbTrayMiddleClickAction.Name = "cbTrayMiddleClickAction";
		this.cbTrayMiddleClickAction.SelectedIndexChanged += new System.EventHandler(cbTrayMiddleClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.lblTrayMiddleClickAction, "lblTrayMiddleClickAction");
		this.lblTrayMiddleClickAction.Name = "lblTrayMiddleClickAction";
		this.cbTrayLeftDoubleClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbTrayLeftDoubleClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbTrayLeftDoubleClickAction, "cbTrayLeftDoubleClickAction");
		this.cbTrayLeftDoubleClickAction.Name = "cbTrayLeftDoubleClickAction";
		this.cbTrayLeftDoubleClickAction.SelectedIndexChanged += new System.EventHandler(cbTrayLeftDoubleClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.lblTrayLeftDoubleClickAction, "lblTrayLeftDoubleClickAction");
		this.lblTrayLeftDoubleClickAction.Name = "lblTrayLeftDoubleClickAction";
		this.cbTrayLeftClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbTrayLeftClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbTrayLeftClickAction, "cbTrayLeftClickAction");
		this.cbTrayLeftClickAction.Name = "cbTrayLeftClickAction";
		this.cbTrayLeftClickAction.SelectedIndexChanged += new System.EventHandler(cbTrayLeftClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.lblTrayLeftClickAction, "lblTrayLeftClickAction");
		this.lblTrayLeftClickAction.Name = "lblTrayLeftClickAction";
		resources.ApplyResources(this.btnEditQuickTaskMenu, "btnEditQuickTaskMenu");
		this.btnEditQuickTaskMenu.Name = "btnEditQuickTaskMenu";
		this.btnEditQuickTaskMenu.UseVisualStyleBackColor = true;
		this.btnEditQuickTaskMenu.Click += new System.EventHandler(btnEditQuickTaskMenu_Click);
		resources.ApplyResources(this.cbShowTray, "cbShowTray");
		this.cbShowTray.Name = "cbShowTray";
		this.cbShowTray.UseVisualStyleBackColor = true;
		this.cbShowTray.CheckedChanged += new System.EventHandler(cbShowTray_CheckedChanged);
		resources.ApplyResources(this.cbTrayIconProgressEnabled, "cbTrayIconProgressEnabled");
		this.cbTrayIconProgressEnabled.Name = "cbTrayIconProgressEnabled";
		this.cbTrayIconProgressEnabled.UseVisualStyleBackColor = true;
		this.cbTrayIconProgressEnabled.CheckedChanged += new System.EventHandler(cbTrayIconProgressEnabled_CheckedChanged);
		resources.ApplyResources(this.btnLanguages, "btnLanguages");
		this.btnLanguages.Menu = this.cmsLanguages;
		this.btnLanguages.Name = "btnLanguages";
		this.btnLanguages.UseVisualStyleBackColor = true;
		this.cmsLanguages.Name = "cmsLanguages";
		resources.ApplyResources(this.cmsLanguages, "cmsLanguages");
		resources.ApplyResources(this.cbRememberMainFormPosition, "cbRememberMainFormPosition");
		this.cbRememberMainFormPosition.Name = "cbRememberMainFormPosition";
		this.cbRememberMainFormPosition.UseVisualStyleBackColor = true;
		this.cbRememberMainFormPosition.CheckedChanged += new System.EventHandler(cbRememberMainFormPosition_CheckedChanged);
		resources.ApplyResources(this.cbSilentRun, "cbSilentRun");
		this.cbSilentRun.Name = "cbSilentRun";
		this.cbSilentRun.UseVisualStyleBackColor = true;
		this.cbSilentRun.CheckedChanged += new System.EventHandler(cbSilentRun_CheckedChanged);
		resources.ApplyResources(this.cbTaskbarProgressEnabled, "cbTaskbarProgressEnabled");
		this.cbTaskbarProgressEnabled.Name = "cbTaskbarProgressEnabled";
		this.cbTaskbarProgressEnabled.UseVisualStyleBackColor = true;
		this.cbTaskbarProgressEnabled.CheckedChanged += new System.EventHandler(cbTaskbarProgressEnabled_CheckedChanged);
		resources.ApplyResources(this.cbRememberMainFormSize, "cbRememberMainFormSize");
		this.cbRememberMainFormSize.Name = "cbRememberMainFormSize";
		this.cbRememberMainFormSize.UseVisualStyleBackColor = true;
		this.cbRememberMainFormSize.CheckedChanged += new System.EventHandler(cbRememberMainFormSize_CheckedChanged);
		resources.ApplyResources(this.lblLanguage, "lblLanguage");
		this.lblLanguage.Name = "lblLanguage";
		this.tpTheme.Controls.Add(this.btnThemeReset);
		this.tpTheme.Controls.Add(this.btnThemeRemove);
		this.tpTheme.Controls.Add(this.btnThemeAdd);
		this.tpTheme.Controls.Add(this.cbThemes);
		this.tpTheme.Controls.Add(this.pgTheme);
		this.tpTheme.Controls.Add(this.cbUseCustomTheme);
		this.tpTheme.Controls.Add(this.eiTheme);
		resources.ApplyResources(this.tpTheme, "tpTheme");
		this.tpTheme.Name = "tpTheme";
		this.tpTheme.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.btnThemeReset, "btnThemeReset");
		this.btnThemeReset.Name = "btnThemeReset";
		this.btnThemeReset.UseVisualStyleBackColor = true;
		this.btnThemeReset.Click += new System.EventHandler(BtnThemeReset_Click);
		resources.ApplyResources(this.btnThemeRemove, "btnThemeRemove");
		this.btnThemeRemove.Name = "btnThemeRemove";
		this.btnThemeRemove.UseVisualStyleBackColor = true;
		this.btnThemeRemove.Click += new System.EventHandler(BtnThemeRemove_Click);
		resources.ApplyResources(this.btnThemeAdd, "btnThemeAdd");
		this.btnThemeAdd.Name = "btnThemeAdd";
		this.btnThemeAdd.UseVisualStyleBackColor = true;
		this.btnThemeAdd.Click += new System.EventHandler(BtnThemeAdd_Click);
		this.cbThemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbThemes.FormattingEnabled = true;
		resources.ApplyResources(this.cbThemes, "cbThemes");
		this.cbThemes.Name = "cbThemes";
		this.cbThemes.SelectedIndexChanged += new System.EventHandler(CbThemes_SelectedIndexChanged);
		resources.ApplyResources(this.pgTheme, "pgTheme");
		this.pgTheme.Name = "pgTheme";
		this.pgTheme.PropertySort = System.Windows.Forms.PropertySort.NoSort;
		this.pgTheme.ToolbarVisible = false;
		this.pgTheme.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(pgTheme_PropertyValueChanged);
		resources.ApplyResources(this.cbUseCustomTheme, "cbUseCustomTheme");
		this.cbUseCustomTheme.Name = "cbUseCustomTheme";
		this.cbUseCustomTheme.UseVisualStyleBackColor = true;
		this.cbUseCustomTheme.CheckedChanged += new System.EventHandler(CbUseCustomTheme_CheckedChanged);
		this.eiTheme.DefaultFileName = null;
		resources.ApplyResources(this.eiTheme, "eiTheme");
		this.eiTheme.Name = "eiTheme";
		this.eiTheme.ObjectType = null;
		this.eiTheme.SerializationBinder = null;
		this.eiTheme.ExportRequested += new ShareX.HelpersLib.ExportImportControl.ExportEventHandler(EiTheme_ExportRequested);
		this.eiTheme.ImportRequested += new ShareX.HelpersLib.ExportImportControl.ImportEventHandler(EiTheme_ImportRequested);
		this.tpIntegration.BackColor = System.Drawing.SystemColors.Window;
		this.tpIntegration.Controls.Add(this.gbFirefox);
		this.tpIntegration.Controls.Add(this.gbSteam);
		this.tpIntegration.Controls.Add(this.gbChrome);
		this.tpIntegration.Controls.Add(this.gbWindows);
		resources.ApplyResources(this.tpIntegration, "tpIntegration");
		this.tpIntegration.Name = "tpIntegration";
		this.gbFirefox.Controls.Add(this.cbFirefoxAddonSupport);
		this.gbFirefox.Controls.Add(this.btnFirefoxOpenAddonPage);
		resources.ApplyResources(this.gbFirefox, "gbFirefox");
		this.gbFirefox.Name = "gbFirefox";
		this.gbFirefox.TabStop = false;
		resources.ApplyResources(this.cbFirefoxAddonSupport, "cbFirefoxAddonSupport");
		this.cbFirefoxAddonSupport.Name = "cbFirefoxAddonSupport";
		this.cbFirefoxAddonSupport.UseVisualStyleBackColor = true;
		this.cbFirefoxAddonSupport.CheckedChanged += new System.EventHandler(cbFirefoxAddonSupport_CheckedChanged);
		resources.ApplyResources(this.btnFirefoxOpenAddonPage, "btnFirefoxOpenAddonPage");
		this.btnFirefoxOpenAddonPage.Name = "btnFirefoxOpenAddonPage";
		this.btnFirefoxOpenAddonPage.UseVisualStyleBackColor = true;
		this.btnFirefoxOpenAddonPage.Click += new System.EventHandler(btnFirefoxOpenAddonPage_Click);
		this.gbSteam.Controls.Add(this.cbSteamShowInApp);
		resources.ApplyResources(this.gbSteam, "gbSteam");
		this.gbSteam.Name = "gbSteam";
		this.gbSteam.TabStop = false;
		resources.ApplyResources(this.cbSteamShowInApp, "cbSteamShowInApp");
		this.cbSteamShowInApp.Name = "cbSteamShowInApp";
		this.cbSteamShowInApp.UseVisualStyleBackColor = true;
		this.cbSteamShowInApp.CheckedChanged += new System.EventHandler(cbSteamShowInApp_CheckedChanged);
		this.gbChrome.Controls.Add(this.cbChromeExtensionSupport);
		this.gbChrome.Controls.Add(this.btnChromeOpenExtensionPage);
		resources.ApplyResources(this.gbChrome, "gbChrome");
		this.gbChrome.Name = "gbChrome";
		this.gbChrome.TabStop = false;
		resources.ApplyResources(this.cbChromeExtensionSupport, "cbChromeExtensionSupport");
		this.cbChromeExtensionSupport.Name = "cbChromeExtensionSupport";
		this.cbChromeExtensionSupport.UseVisualStyleBackColor = true;
		this.cbChromeExtensionSupport.CheckedChanged += new System.EventHandler(cbChromeExtensionSupport_CheckedChanged);
		resources.ApplyResources(this.btnChromeOpenExtensionPage, "btnChromeOpenExtensionPage");
		this.btnChromeOpenExtensionPage.Name = "btnChromeOpenExtensionPage";
		this.btnChromeOpenExtensionPage.UseVisualStyleBackColor = true;
		this.btnChromeOpenExtensionPage.Click += new System.EventHandler(btnChromeOpenExtensionPage_Click);
		this.gbWindows.Controls.Add(this.cbEditWithShareX);
		this.gbWindows.Controls.Add(this.cbStartWithWindows);
		this.gbWindows.Controls.Add(this.cbSendToMenu);
		this.gbWindows.Controls.Add(this.cbShellContextMenu);
		resources.ApplyResources(this.gbWindows, "gbWindows");
		this.gbWindows.Name = "gbWindows";
		this.gbWindows.TabStop = false;
		resources.ApplyResources(this.cbEditWithShareX, "cbEditWithShareX");
		this.cbEditWithShareX.Name = "cbEditWithShareX";
		this.cbEditWithShareX.UseVisualStyleBackColor = true;
		this.cbEditWithShareX.CheckedChanged += new System.EventHandler(cbEditWithShareX_CheckedChanged);
		resources.ApplyResources(this.cbStartWithWindows, "cbStartWithWindows");
		this.cbStartWithWindows.Name = "cbStartWithWindows";
		this.cbStartWithWindows.UseVisualStyleBackColor = true;
		this.cbStartWithWindows.CheckedChanged += new System.EventHandler(cbStartWithWindows_CheckedChanged);
		resources.ApplyResources(this.cbSendToMenu, "cbSendToMenu");
		this.cbSendToMenu.Name = "cbSendToMenu";
		this.cbSendToMenu.UseVisualStyleBackColor = true;
		this.cbSendToMenu.CheckedChanged += new System.EventHandler(cbSendToMenu_CheckedChanged);
		resources.ApplyResources(this.cbShellContextMenu, "cbShellContextMenu");
		this.cbShellContextMenu.Name = "cbShellContextMenu";
		this.cbShellContextMenu.UseVisualStyleBackColor = true;
		this.cbShellContextMenu.CheckedChanged += new System.EventHandler(cbShellContextMenu_CheckedChanged);
		this.tpPaths.BackColor = System.Drawing.SystemColors.Window;
		this.tpPaths.Controls.Add(this.txtSaveImageSubFolderPatternWindow);
		this.tpPaths.Controls.Add(this.lblSaveImageSubFolderPatternWindow);
		this.tpPaths.Controls.Add(this.btnPersonalFolderPathApply);
		this.tpPaths.Controls.Add(this.btnOpenScreenshotsFolder);
		this.tpPaths.Controls.Add(this.lblPreviewPersonalFolderPath);
		this.tpPaths.Controls.Add(this.btnBrowsePersonalFolderPath);
		this.tpPaths.Controls.Add(this.lblPersonalFolderPath);
		this.tpPaths.Controls.Add(this.txtPersonalFolderPath);
		this.tpPaths.Controls.Add(this.btnBrowseCustomScreenshotsPath);
		this.tpPaths.Controls.Add(this.btnOpenPersonalFolderPath);
		this.tpPaths.Controls.Add(this.txtCustomScreenshotsPath);
		this.tpPaths.Controls.Add(this.cbUseCustomScreenshotsPath);
		this.tpPaths.Controls.Add(this.lblSaveImageSubFolderPattern);
		this.tpPaths.Controls.Add(this.lblSaveImageSubFolderPatternPreview);
		this.tpPaths.Controls.Add(this.txtSaveImageSubFolderPattern);
		resources.ApplyResources(this.tpPaths, "tpPaths");
		this.tpPaths.Name = "tpPaths";
		resources.ApplyResources(this.btnPersonalFolderPathApply, "btnPersonalFolderPathApply");
		this.btnPersonalFolderPathApply.Name = "btnPersonalFolderPathApply";
		this.btnPersonalFolderPathApply.UseVisualStyleBackColor = true;
		this.btnPersonalFolderPathApply.Click += new System.EventHandler(btnPersonalFolderPathApply_Click);
		resources.ApplyResources(this.btnOpenScreenshotsFolder, "btnOpenScreenshotsFolder");
		this.btnOpenScreenshotsFolder.Name = "btnOpenScreenshotsFolder";
		this.btnOpenScreenshotsFolder.UseVisualStyleBackColor = true;
		this.btnOpenScreenshotsFolder.Click += new System.EventHandler(btnOpenScreenshotsFolder_Click);
		resources.ApplyResources(this.lblPreviewPersonalFolderPath, "lblPreviewPersonalFolderPath");
		this.lblPreviewPersonalFolderPath.Name = "lblPreviewPersonalFolderPath";
		resources.ApplyResources(this.btnBrowsePersonalFolderPath, "btnBrowsePersonalFolderPath");
		this.btnBrowsePersonalFolderPath.Name = "btnBrowsePersonalFolderPath";
		this.btnBrowsePersonalFolderPath.UseVisualStyleBackColor = true;
		this.btnBrowsePersonalFolderPath.Click += new System.EventHandler(btnBrowsePersonalFolderPath_Click);
		resources.ApplyResources(this.lblPersonalFolderPath, "lblPersonalFolderPath");
		this.lblPersonalFolderPath.Name = "lblPersonalFolderPath";
		resources.ApplyResources(this.txtPersonalFolderPath, "txtPersonalFolderPath");
		this.txtPersonalFolderPath.Name = "txtPersonalFolderPath";
		this.txtPersonalFolderPath.TextChanged += new System.EventHandler(txtPersonalFolderPath_TextChanged);
		resources.ApplyResources(this.btnBrowseCustomScreenshotsPath, "btnBrowseCustomScreenshotsPath");
		this.btnBrowseCustomScreenshotsPath.Name = "btnBrowseCustomScreenshotsPath";
		this.btnBrowseCustomScreenshotsPath.UseVisualStyleBackColor = true;
		this.btnBrowseCustomScreenshotsPath.Click += new System.EventHandler(btnBrowseCustomScreenshotsPath_Click);
		resources.ApplyResources(this.btnOpenPersonalFolderPath, "btnOpenPersonalFolderPath");
		this.btnOpenPersonalFolderPath.Name = "btnOpenPersonalFolderPath";
		this.btnOpenPersonalFolderPath.UseVisualStyleBackColor = true;
		this.btnOpenPersonalFolderPath.Click += new System.EventHandler(btnOpenPersonalFolder_Click);
		resources.ApplyResources(this.txtCustomScreenshotsPath, "txtCustomScreenshotsPath");
		this.txtCustomScreenshotsPath.Name = "txtCustomScreenshotsPath";
		this.txtCustomScreenshotsPath.TextChanged += new System.EventHandler(txtCustomScreenshotsPath_TextChanged);
		resources.ApplyResources(this.cbUseCustomScreenshotsPath, "cbUseCustomScreenshotsPath");
		this.cbUseCustomScreenshotsPath.Name = "cbUseCustomScreenshotsPath";
		this.cbUseCustomScreenshotsPath.UseVisualStyleBackColor = true;
		this.cbUseCustomScreenshotsPath.CheckedChanged += new System.EventHandler(cbUseCustomScreenshotsPath_CheckedChanged);
		resources.ApplyResources(this.lblSaveImageSubFolderPattern, "lblSaveImageSubFolderPattern");
		this.lblSaveImageSubFolderPattern.Name = "lblSaveImageSubFolderPattern";
		resources.ApplyResources(this.lblSaveImageSubFolderPatternPreview, "lblSaveImageSubFolderPatternPreview");
		this.lblSaveImageSubFolderPatternPreview.Name = "lblSaveImageSubFolderPatternPreview";
		resources.ApplyResources(this.txtSaveImageSubFolderPattern, "txtSaveImageSubFolderPattern");
		this.txtSaveImageSubFolderPattern.Name = "txtSaveImageSubFolderPattern";
		this.txtSaveImageSubFolderPattern.TextChanged += new System.EventHandler(txtSaveImageSubFolderPattern_TextChanged);
		this.tpSettings.BackColor = System.Drawing.SystemColors.Window;
		this.tpSettings.Controls.Add(this.cbAutomaticallyCleanupLogFiles);
		this.tpSettings.Controls.Add(this.nudCleanupKeepFileCount);
		this.tpSettings.Controls.Add(this.lblCleanupKeepFileCount);
		this.tpSettings.Controls.Add(this.cbAutomaticallyCleanupBackupFiles);
		this.tpSettings.Controls.Add(this.pbExportImportNote);
		this.tpSettings.Controls.Add(this.cbExportHistory);
		this.tpSettings.Controls.Add(this.cbExportSettings);
		this.tpSettings.Controls.Add(this.lblExportImportNote);
		this.tpSettings.Controls.Add(this.btnResetSettings);
		this.tpSettings.Controls.Add(this.pbExportImport);
		this.tpSettings.Controls.Add(this.btnExport);
		this.tpSettings.Controls.Add(this.btnImport);
		resources.ApplyResources(this.tpSettings, "tpSettings");
		this.tpSettings.Name = "tpSettings";
		resources.ApplyResources(this.cbAutomaticallyCleanupLogFiles, "cbAutomaticallyCleanupLogFiles");
		this.cbAutomaticallyCleanupLogFiles.Name = "cbAutomaticallyCleanupLogFiles";
		this.cbAutomaticallyCleanupLogFiles.UseVisualStyleBackColor = true;
		this.cbAutomaticallyCleanupLogFiles.CheckedChanged += new System.EventHandler(cbAutomaticallyCleanupLogFiles_CheckedChanged);
		resources.ApplyResources(this.nudCleanupKeepFileCount, "nudCleanupKeepFileCount");
		this.nudCleanupKeepFileCount.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudCleanupKeepFileCount.Name = "nudCleanupKeepFileCount";
		this.nudCleanupKeepFileCount.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudCleanupKeepFileCount.ValueChanged += new System.EventHandler(nudCleanupKeepFileCount_ValueChanged);
		resources.ApplyResources(this.lblCleanupKeepFileCount, "lblCleanupKeepFileCount");
		this.lblCleanupKeepFileCount.Name = "lblCleanupKeepFileCount";
		resources.ApplyResources(this.cbAutomaticallyCleanupBackupFiles, "cbAutomaticallyCleanupBackupFiles");
		this.cbAutomaticallyCleanupBackupFiles.Name = "cbAutomaticallyCleanupBackupFiles";
		this.cbAutomaticallyCleanupBackupFiles.UseVisualStyleBackColor = true;
		this.cbAutomaticallyCleanupBackupFiles.CheckedChanged += new System.EventHandler(cbAutomaticallyCleanupBackupFiles_CheckedChanged);
		this.pbExportImportNote.Image = ShareX.Properties.Resources.exclamation;
		resources.ApplyResources(this.pbExportImportNote, "pbExportImportNote");
		this.pbExportImportNote.Name = "pbExportImportNote";
		this.pbExportImportNote.TabStop = false;
		resources.ApplyResources(this.cbExportHistory, "cbExportHistory");
		this.cbExportHistory.Checked = true;
		this.cbExportHistory.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbExportHistory.Name = "cbExportHistory";
		this.cbExportHistory.UseVisualStyleBackColor = true;
		this.cbExportHistory.CheckedChanged += new System.EventHandler(cbExportHistory_CheckedChanged);
		resources.ApplyResources(this.cbExportSettings, "cbExportSettings");
		this.cbExportSettings.Checked = true;
		this.cbExportSettings.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbExportSettings.Name = "cbExportSettings";
		this.cbExportSettings.UseVisualStyleBackColor = true;
		this.cbExportSettings.CheckedChanged += new System.EventHandler(cbExportSettings_CheckedChanged);
		resources.ApplyResources(this.lblExportImportNote, "lblExportImportNote");
		this.lblExportImportNote.Name = "lblExportImportNote";
		resources.ApplyResources(this.btnResetSettings, "btnResetSettings");
		this.btnResetSettings.Name = "btnResetSettings";
		this.btnResetSettings.UseVisualStyleBackColor = true;
		this.btnResetSettings.Click += new System.EventHandler(btnResetSettings_Click);
		resources.ApplyResources(this.pbExportImport, "pbExportImport");
		this.pbExportImport.MarqueeAnimationSpeed = 50;
		this.pbExportImport.Name = "pbExportImport";
		this.pbExportImport.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
		resources.ApplyResources(this.btnExport, "btnExport");
		this.btnExport.Name = "btnExport";
		this.btnExport.UseVisualStyleBackColor = true;
		this.btnExport.Click += new System.EventHandler(btnExport_Click);
		resources.ApplyResources(this.btnImport, "btnImport");
		this.btnImport.Name = "btnImport";
		this.btnImport.UseVisualStyleBackColor = true;
		this.btnImport.Click += new System.EventHandler(btnImport_Click);
		this.tpMainWindow.Controls.Add(this.gbListView);
		this.tpMainWindow.Controls.Add(this.gbThumbnailView);
		this.tpMainWindow.Controls.Add(this.cbMainWindowShowMenu);
		this.tpMainWindow.Controls.Add(this.cbMainWindowTaskViewMode);
		this.tpMainWindow.Controls.Add(this.lblMainWindowTaskViewMode);
		resources.ApplyResources(this.tpMainWindow, "tpMainWindow");
		this.tpMainWindow.Name = "tpMainWindow";
		this.tpMainWindow.UseVisualStyleBackColor = true;
		this.gbListView.Controls.Add(this.cbListViewImagePreviewLocation);
		this.gbListView.Controls.Add(this.lblListViewImagePreviewLocation);
		this.gbListView.Controls.Add(this.cbListViewImagePreviewVisibility);
		this.gbListView.Controls.Add(this.lblListViewImagePreviewVisibility);
		this.gbListView.Controls.Add(this.cbListViewShowColumns);
		resources.ApplyResources(this.gbListView, "gbListView");
		this.gbListView.Name = "gbListView";
		this.gbListView.TabStop = false;
		this.cbListViewImagePreviewLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbListViewImagePreviewLocation.FormattingEnabled = true;
		resources.ApplyResources(this.cbListViewImagePreviewLocation, "cbListViewImagePreviewLocation");
		this.cbListViewImagePreviewLocation.Name = "cbListViewImagePreviewLocation";
		this.cbListViewImagePreviewLocation.SelectedIndexChanged += new System.EventHandler(cbListViewImagePreviewLocation_SelectedIndexChanged);
		resources.ApplyResources(this.lblListViewImagePreviewLocation, "lblListViewImagePreviewLocation");
		this.lblListViewImagePreviewLocation.Name = "lblListViewImagePreviewLocation";
		this.cbListViewImagePreviewVisibility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbListViewImagePreviewVisibility.FormattingEnabled = true;
		resources.ApplyResources(this.cbListViewImagePreviewVisibility, "cbListViewImagePreviewVisibility");
		this.cbListViewImagePreviewVisibility.Name = "cbListViewImagePreviewVisibility";
		this.cbListViewImagePreviewVisibility.SelectedIndexChanged += new System.EventHandler(cbListViewImagePreviewVisibility_SelectedIndexChanged);
		resources.ApplyResources(this.lblListViewImagePreviewVisibility, "lblListViewImagePreviewVisibility");
		this.lblListViewImagePreviewVisibility.Name = "lblListViewImagePreviewVisibility";
		resources.ApplyResources(this.cbListViewShowColumns, "cbListViewShowColumns");
		this.cbListViewShowColumns.Name = "cbListViewShowColumns";
		this.cbListViewShowColumns.UseVisualStyleBackColor = true;
		this.cbListViewShowColumns.CheckedChanged += new System.EventHandler(cbListViewShowColumns_CheckedChanged);
		this.gbThumbnailView.Controls.Add(this.btnThumbnailViewThumbnailSizeReset);
		this.gbThumbnailView.Controls.Add(this.lblThumbnailViewThumbnailSizeX);
		this.gbThumbnailView.Controls.Add(this.nudThumbnailViewThumbnailSizeHeight);
		this.gbThumbnailView.Controls.Add(this.nudThumbnailViewThumbnailSizeWidth);
		this.gbThumbnailView.Controls.Add(this.cbThumbnailViewThumbnailClickAction);
		this.gbThumbnailView.Controls.Add(this.lblThumbnailViewThumbnailClickAction);
		this.gbThumbnailView.Controls.Add(this.lblThumbnailViewThumbnailSize);
		this.gbThumbnailView.Controls.Add(this.cbThumbnailViewTitleLocation);
		this.gbThumbnailView.Controls.Add(this.lblThumbnailViewTitleLocation);
		this.gbThumbnailView.Controls.Add(this.cbThumbnailViewShowTitle);
		resources.ApplyResources(this.gbThumbnailView, "gbThumbnailView");
		this.gbThumbnailView.Name = "gbThumbnailView";
		this.gbThumbnailView.TabStop = false;
		resources.ApplyResources(this.btnThumbnailViewThumbnailSizeReset, "btnThumbnailViewThumbnailSizeReset");
		this.btnThumbnailViewThumbnailSizeReset.Name = "btnThumbnailViewThumbnailSizeReset";
		this.btnThumbnailViewThumbnailSizeReset.UseVisualStyleBackColor = true;
		this.btnThumbnailViewThumbnailSizeReset.Click += new System.EventHandler(btnThumbnailViewThumbnailSizeReset_Click);
		resources.ApplyResources(this.lblThumbnailViewThumbnailSizeX, "lblThumbnailViewThumbnailSizeX");
		this.lblThumbnailViewThumbnailSizeX.Name = "lblThumbnailViewThumbnailSizeX";
		resources.ApplyResources(this.nudThumbnailViewThumbnailSizeHeight, "nudThumbnailViewThumbnailSizeHeight");
		this.nudThumbnailViewThumbnailSizeHeight.Maximum = new decimal(new int[4] { 500, 0, 0, 0 });
		this.nudThumbnailViewThumbnailSizeHeight.Minimum = new decimal(new int[4] { 50, 0, 0, 0 });
		this.nudThumbnailViewThumbnailSizeHeight.Name = "nudThumbnailViewThumbnailSizeHeight";
		this.nudThumbnailViewThumbnailSizeHeight.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		this.nudThumbnailViewThumbnailSizeHeight.ValueChanged += new System.EventHandler(nudThumbnailViewThumbnailSizeHeight_ValueChanged);
		resources.ApplyResources(this.nudThumbnailViewThumbnailSizeWidth, "nudThumbnailViewThumbnailSizeWidth");
		this.nudThumbnailViewThumbnailSizeWidth.Maximum = new decimal(new int[4] { 500, 0, 0, 0 });
		this.nudThumbnailViewThumbnailSizeWidth.Minimum = new decimal(new int[4] { 50, 0, 0, 0 });
		this.nudThumbnailViewThumbnailSizeWidth.Name = "nudThumbnailViewThumbnailSizeWidth";
		this.nudThumbnailViewThumbnailSizeWidth.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		this.nudThumbnailViewThumbnailSizeWidth.ValueChanged += new System.EventHandler(nudThumbnailViewThumbnailSizeWidth_ValueChanged);
		this.cbThumbnailViewThumbnailClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbThumbnailViewThumbnailClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbThumbnailViewThumbnailClickAction, "cbThumbnailViewThumbnailClickAction");
		this.cbThumbnailViewThumbnailClickAction.Name = "cbThumbnailViewThumbnailClickAction";
		this.cbThumbnailViewThumbnailClickAction.SelectedIndexChanged += new System.EventHandler(cbThumbnailViewThumbnailClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.lblThumbnailViewThumbnailClickAction, "lblThumbnailViewThumbnailClickAction");
		this.lblThumbnailViewThumbnailClickAction.Name = "lblThumbnailViewThumbnailClickAction";
		resources.ApplyResources(this.lblThumbnailViewThumbnailSize, "lblThumbnailViewThumbnailSize");
		this.lblThumbnailViewThumbnailSize.Name = "lblThumbnailViewThumbnailSize";
		this.cbThumbnailViewTitleLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbThumbnailViewTitleLocation.FormattingEnabled = true;
		resources.ApplyResources(this.cbThumbnailViewTitleLocation, "cbThumbnailViewTitleLocation");
		this.cbThumbnailViewTitleLocation.Name = "cbThumbnailViewTitleLocation";
		this.cbThumbnailViewTitleLocation.SelectedIndexChanged += new System.EventHandler(cbThumbnailViewTitleLocation_SelectedIndexChanged);
		resources.ApplyResources(this.lblThumbnailViewTitleLocation, "lblThumbnailViewTitleLocation");
		this.lblThumbnailViewTitleLocation.Name = "lblThumbnailViewTitleLocation";
		resources.ApplyResources(this.cbThumbnailViewShowTitle, "cbThumbnailViewShowTitle");
		this.cbThumbnailViewShowTitle.Name = "cbThumbnailViewShowTitle";
		this.cbThumbnailViewShowTitle.UseVisualStyleBackColor = true;
		this.cbThumbnailViewShowTitle.CheckedChanged += new System.EventHandler(cbThumbnailViewShowTitle_CheckedChanged);
		resources.ApplyResources(this.cbMainWindowShowMenu, "cbMainWindowShowMenu");
		this.cbMainWindowShowMenu.Name = "cbMainWindowShowMenu";
		this.cbMainWindowShowMenu.UseVisualStyleBackColor = true;
		this.cbMainWindowShowMenu.CheckedChanged += new System.EventHandler(cbMainWindowShowMenu_CheckedChanged);
		this.cbMainWindowTaskViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbMainWindowTaskViewMode.FormattingEnabled = true;
		resources.ApplyResources(this.cbMainWindowTaskViewMode, "cbMainWindowTaskViewMode");
		this.cbMainWindowTaskViewMode.Name = "cbMainWindowTaskViewMode";
		this.cbMainWindowTaskViewMode.SelectedIndexChanged += new System.EventHandler(cbMainWindowTaskViewMode_SelectedIndexChanged);
		resources.ApplyResources(this.lblMainWindowTaskViewMode, "lblMainWindowTaskViewMode");
		this.lblMainWindowTaskViewMode.Name = "lblMainWindowTaskViewMode";
		this.tpClipboardFormats.Controls.Add(this.lblClipboardFormatsTip);
		this.tpClipboardFormats.Controls.Add(this.btnClipboardFormatEdit);
		this.tpClipboardFormats.Controls.Add(this.btnClipboardFormatRemove);
		this.tpClipboardFormats.Controls.Add(this.btnClipboardFormatAdd);
		this.tpClipboardFormats.Controls.Add(this.lvClipboardFormats);
		resources.ApplyResources(this.tpClipboardFormats, "tpClipboardFormats");
		this.tpClipboardFormats.Name = "tpClipboardFormats";
		this.tpClipboardFormats.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.lblClipboardFormatsTip, "lblClipboardFormatsTip");
		this.lblClipboardFormatsTip.Name = "lblClipboardFormatsTip";
		resources.ApplyResources(this.btnClipboardFormatEdit, "btnClipboardFormatEdit");
		this.btnClipboardFormatEdit.Name = "btnClipboardFormatEdit";
		this.btnClipboardFormatEdit.UseVisualStyleBackColor = true;
		this.btnClipboardFormatEdit.Click += new System.EventHandler(btnClipboardFormatEdit_Click);
		resources.ApplyResources(this.btnClipboardFormatRemove, "btnClipboardFormatRemove");
		this.btnClipboardFormatRemove.Name = "btnClipboardFormatRemove";
		this.btnClipboardFormatRemove.UseVisualStyleBackColor = true;
		this.btnClipboardFormatRemove.Click += new System.EventHandler(btnClipboardFormatRemove_Click);
		resources.ApplyResources(this.btnClipboardFormatAdd, "btnClipboardFormatAdd");
		this.btnClipboardFormatAdd.Name = "btnClipboardFormatAdd";
		this.btnClipboardFormatAdd.UseVisualStyleBackColor = true;
		this.btnClipboardFormatAdd.Click += new System.EventHandler(btnAddClipboardFormat_Click);
		resources.ApplyResources(this.lvClipboardFormats, "lvClipboardFormats");
		this.lvClipboardFormats.AutoFillColumn = true;
		this.lvClipboardFormats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.chDescription, this.chFormat });
		this.lvClipboardFormats.FullRowSelect = true;
		this.lvClipboardFormats.HideSelection = false;
		this.lvClipboardFormats.Name = "lvClipboardFormats";
		this.lvClipboardFormats.UseCompatibleStateImageBehavior = false;
		this.lvClipboardFormats.View = System.Windows.Forms.View.Details;
		this.lvClipboardFormats.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(lvClipboardFormats_MouseDoubleClick);
		resources.ApplyResources(this.chDescription, "chDescription");
		resources.ApplyResources(this.chFormat, "chFormat");
		this.tpUpload.BackColor = System.Drawing.SystemColors.Window;
		this.tpUpload.Controls.Add(this.gbSecondaryFileUploaders);
		this.tpUpload.Controls.Add(this.lblUploadLimit);
		this.tpUpload.Controls.Add(this.gbSecondaryImageUploaders);
		this.tpUpload.Controls.Add(this.gbSecondaryTextUploaders);
		this.tpUpload.Controls.Add(this.nudUploadLimit);
		this.tpUpload.Controls.Add(this.cbUseSecondaryUploaders);
		this.tpUpload.Controls.Add(this.lblUploadLimitHint);
		this.tpUpload.Controls.Add(this.cbIfUploadFailRetryOnce);
		this.tpUpload.Controls.Add(this.lblBufferSize);
		this.tpUpload.Controls.Add(this.nudRetryUpload);
		this.tpUpload.Controls.Add(this.cbBufferSize);
		resources.ApplyResources(this.tpUpload, "tpUpload");
		this.tpUpload.Name = "tpUpload";
		this.gbSecondaryFileUploaders.Controls.Add(this.lvSecondaryFileUploaders);
		resources.ApplyResources(this.gbSecondaryFileUploaders, "gbSecondaryFileUploaders");
		this.gbSecondaryFileUploaders.Name = "gbSecondaryFileUploaders";
		this.gbSecondaryFileUploaders.TabStop = false;
		this.lvSecondaryFileUploaders.AllowDrop = true;
		this.lvSecondaryFileUploaders.AllowItemDrag = true;
		this.lvSecondaryFileUploaders.AutoFillColumn = true;
		this.lvSecondaryFileUploaders.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lvSecondaryFileUploaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.chSecondaryFileUploaders });
		resources.ApplyResources(this.lvSecondaryFileUploaders, "lvSecondaryFileUploaders");
		this.lvSecondaryFileUploaders.FullRowSelect = true;
		this.lvSecondaryFileUploaders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		this.lvSecondaryFileUploaders.HideSelection = false;
		this.lvSecondaryFileUploaders.MultiSelect = false;
		this.lvSecondaryFileUploaders.Name = "lvSecondaryFileUploaders";
		this.lvSecondaryFileUploaders.UseCompatibleStateImageBehavior = false;
		this.lvSecondaryFileUploaders.View = System.Windows.Forms.View.Details;
		this.lvSecondaryFileUploaders.MouseUp += new System.Windows.Forms.MouseEventHandler(lvSecondaryUploaders_MouseUp);
		resources.ApplyResources(this.lblUploadLimit, "lblUploadLimit");
		this.lblUploadLimit.Name = "lblUploadLimit";
		this.gbSecondaryImageUploaders.Controls.Add(this.lvSecondaryImageUploaders);
		resources.ApplyResources(this.gbSecondaryImageUploaders, "gbSecondaryImageUploaders");
		this.gbSecondaryImageUploaders.Name = "gbSecondaryImageUploaders";
		this.gbSecondaryImageUploaders.TabStop = false;
		this.lvSecondaryImageUploaders.AllowDrop = true;
		this.lvSecondaryImageUploaders.AllowItemDrag = true;
		this.lvSecondaryImageUploaders.AutoFillColumn = true;
		this.lvSecondaryImageUploaders.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lvSecondaryImageUploaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.chSecondaryImageUploaders });
		resources.ApplyResources(this.lvSecondaryImageUploaders, "lvSecondaryImageUploaders");
		this.lvSecondaryImageUploaders.FullRowSelect = true;
		this.lvSecondaryImageUploaders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		this.lvSecondaryImageUploaders.HideSelection = false;
		this.lvSecondaryImageUploaders.MultiSelect = false;
		this.lvSecondaryImageUploaders.Name = "lvSecondaryImageUploaders";
		this.lvSecondaryImageUploaders.UseCompatibleStateImageBehavior = false;
		this.lvSecondaryImageUploaders.View = System.Windows.Forms.View.Details;
		this.lvSecondaryImageUploaders.MouseUp += new System.Windows.Forms.MouseEventHandler(lvSecondaryUploaders_MouseUp);
		this.gbSecondaryTextUploaders.Controls.Add(this.lvSecondaryTextUploaders);
		resources.ApplyResources(this.gbSecondaryTextUploaders, "gbSecondaryTextUploaders");
		this.gbSecondaryTextUploaders.Name = "gbSecondaryTextUploaders";
		this.gbSecondaryTextUploaders.TabStop = false;
		this.lvSecondaryTextUploaders.AllowDrop = true;
		this.lvSecondaryTextUploaders.AllowItemDrag = true;
		this.lvSecondaryTextUploaders.AutoFillColumn = true;
		this.lvSecondaryTextUploaders.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lvSecondaryTextUploaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.chSecondaryTextUploaders });
		resources.ApplyResources(this.lvSecondaryTextUploaders, "lvSecondaryTextUploaders");
		this.lvSecondaryTextUploaders.FullRowSelect = true;
		this.lvSecondaryTextUploaders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		this.lvSecondaryTextUploaders.HideSelection = false;
		this.lvSecondaryTextUploaders.MultiSelect = false;
		this.lvSecondaryTextUploaders.Name = "lvSecondaryTextUploaders";
		this.lvSecondaryTextUploaders.UseCompatibleStateImageBehavior = false;
		this.lvSecondaryTextUploaders.View = System.Windows.Forms.View.Details;
		this.lvSecondaryTextUploaders.MouseUp += new System.Windows.Forms.MouseEventHandler(lvSecondaryUploaders_MouseUp);
		resources.ApplyResources(this.nudUploadLimit, "nudUploadLimit");
		this.nudUploadLimit.Maximum = new decimal(new int[4] { 25, 0, 0, 0 });
		this.nudUploadLimit.Name = "nudUploadLimit";
		this.nudUploadLimit.Value = new decimal(new int[4] { 5, 0, 0, 0 });
		this.nudUploadLimit.ValueChanged += new System.EventHandler(nudUploadLimit_ValueChanged);
		resources.ApplyResources(this.cbUseSecondaryUploaders, "cbUseSecondaryUploaders");
		this.cbUseSecondaryUploaders.Name = "cbUseSecondaryUploaders";
		this.cbUseSecondaryUploaders.UseVisualStyleBackColor = true;
		this.cbUseSecondaryUploaders.CheckedChanged += new System.EventHandler(cbUseSecondaryUploaders_CheckedChanged);
		resources.ApplyResources(this.lblUploadLimitHint, "lblUploadLimitHint");
		this.lblUploadLimitHint.Name = "lblUploadLimitHint";
		resources.ApplyResources(this.cbIfUploadFailRetryOnce, "cbIfUploadFailRetryOnce");
		this.cbIfUploadFailRetryOnce.Name = "cbIfUploadFailRetryOnce";
		resources.ApplyResources(this.lblBufferSize, "lblBufferSize");
		this.lblBufferSize.Name = "lblBufferSize";
		resources.ApplyResources(this.nudRetryUpload, "nudRetryUpload");
		this.nudRetryUpload.Maximum = new decimal(new int[4] { 5, 0, 0, 0 });
		this.nudRetryUpload.Name = "nudRetryUpload";
		this.nudRetryUpload.ValueChanged += new System.EventHandler(nudRetryUpload_ValueChanged);
		this.cbBufferSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbBufferSize.FormattingEnabled = true;
		resources.ApplyResources(this.cbBufferSize, "cbBufferSize");
		this.cbBufferSize.Name = "cbBufferSize";
		this.cbBufferSize.SelectedIndexChanged += new System.EventHandler(cbBufferSize_SelectedIndexChanged);
		this.tpHistory.BackColor = System.Drawing.SystemColors.Window;
		this.tpHistory.Controls.Add(this.gbHistory);
		this.tpHistory.Controls.Add(this.gbRecentLinks);
		resources.ApplyResources(this.tpHistory, "tpHistory");
		this.tpHistory.Name = "tpHistory";
		this.gbHistory.Controls.Add(this.cbHistoryCheckURL);
		this.gbHistory.Controls.Add(this.cbHistorySaveTasks);
		resources.ApplyResources(this.gbHistory, "gbHistory");
		this.gbHistory.Name = "gbHistory";
		this.gbHistory.TabStop = false;
		resources.ApplyResources(this.cbHistoryCheckURL, "cbHistoryCheckURL");
		this.cbHistoryCheckURL.Name = "cbHistoryCheckURL";
		this.cbHistoryCheckURL.UseVisualStyleBackColor = true;
		this.cbHistoryCheckURL.CheckedChanged += new System.EventHandler(cbHistoryCheckURL_CheckedChanged);
		resources.ApplyResources(this.cbHistorySaveTasks, "cbHistorySaveTasks");
		this.cbHistorySaveTasks.Name = "cbHistorySaveTasks";
		this.cbHistorySaveTasks.UseVisualStyleBackColor = true;
		this.cbHistorySaveTasks.CheckedChanged += new System.EventHandler(cbHistorySaveTasks_CheckedChanged);
		this.gbRecentLinks.Controls.Add(this.cbRecentTasksTrayMenuMostRecentFirst);
		this.gbRecentLinks.Controls.Add(this.lblRecentTasksMaxCount);
		this.gbRecentLinks.Controls.Add(this.nudRecentTasksMaxCount);
		this.gbRecentLinks.Controls.Add(this.cbRecentTasksShowInTrayMenu);
		this.gbRecentLinks.Controls.Add(this.cbRecentTasksShowInMainWindow);
		this.gbRecentLinks.Controls.Add(this.cbRecentTasksSave);
		resources.ApplyResources(this.gbRecentLinks, "gbRecentLinks");
		this.gbRecentLinks.Name = "gbRecentLinks";
		this.gbRecentLinks.TabStop = false;
		resources.ApplyResources(this.cbRecentTasksTrayMenuMostRecentFirst, "cbRecentTasksTrayMenuMostRecentFirst");
		this.cbRecentTasksTrayMenuMostRecentFirst.Name = "cbRecentTasksTrayMenuMostRecentFirst";
		this.cbRecentTasksTrayMenuMostRecentFirst.UseVisualStyleBackColor = true;
		this.cbRecentTasksTrayMenuMostRecentFirst.CheckedChanged += new System.EventHandler(cbRecentTasksTrayMenuMostRecentFirst_CheckedChanged);
		resources.ApplyResources(this.lblRecentTasksMaxCount, "lblRecentTasksMaxCount");
		this.lblRecentTasksMaxCount.Name = "lblRecentTasksMaxCount";
		resources.ApplyResources(this.nudRecentTasksMaxCount, "nudRecentTasksMaxCount");
		this.nudRecentTasksMaxCount.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudRecentTasksMaxCount.Name = "nudRecentTasksMaxCount";
		this.nudRecentTasksMaxCount.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudRecentTasksMaxCount.ValueChanged += new System.EventHandler(nudRecentTasksMaxCount_ValueChanged);
		resources.ApplyResources(this.cbRecentTasksShowInTrayMenu, "cbRecentTasksShowInTrayMenu");
		this.cbRecentTasksShowInTrayMenu.Name = "cbRecentTasksShowInTrayMenu";
		this.cbRecentTasksShowInTrayMenu.UseVisualStyleBackColor = true;
		this.cbRecentTasksShowInTrayMenu.CheckedChanged += new System.EventHandler(cbRecentTasksShowInTrayMenu_CheckedChanged);
		resources.ApplyResources(this.cbRecentTasksShowInMainWindow, "cbRecentTasksShowInMainWindow");
		this.cbRecentTasksShowInMainWindow.Name = "cbRecentTasksShowInMainWindow";
		this.cbRecentTasksShowInMainWindow.UseVisualStyleBackColor = true;
		this.cbRecentTasksShowInMainWindow.CheckedChanged += new System.EventHandler(cbRecentTasksShowInMainWindow_CheckedChanged);
		resources.ApplyResources(this.cbRecentTasksSave, "cbRecentTasksSave");
		this.cbRecentTasksSave.Name = "cbRecentTasksSave";
		this.cbRecentTasksSave.UseVisualStyleBackColor = true;
		this.cbRecentTasksSave.CheckedChanged += new System.EventHandler(cbRecentTasksSave_CheckedChanged);
		this.tpPrint.BackColor = System.Drawing.SystemColors.Window;
		this.tpPrint.Controls.Add(this.lblDefaultPrinterOverride);
		this.tpPrint.Controls.Add(this.txtDefaultPrinterOverride);
		this.tpPrint.Controls.Add(this.cbPrintDontShowWindowsDialog);
		this.tpPrint.Controls.Add(this.cbDontShowPrintSettingDialog);
		this.tpPrint.Controls.Add(this.btnShowImagePrintSettings);
		resources.ApplyResources(this.tpPrint, "tpPrint");
		this.tpPrint.Name = "tpPrint";
		resources.ApplyResources(this.lblDefaultPrinterOverride, "lblDefaultPrinterOverride");
		this.lblDefaultPrinterOverride.Name = "lblDefaultPrinterOverride";
		resources.ApplyResources(this.txtDefaultPrinterOverride, "txtDefaultPrinterOverride");
		this.txtDefaultPrinterOverride.Name = "txtDefaultPrinterOverride";
		this.txtDefaultPrinterOverride.TextChanged += new System.EventHandler(txtDefaultPrinterOverride_TextChanged);
		resources.ApplyResources(this.cbPrintDontShowWindowsDialog, "cbPrintDontShowWindowsDialog");
		this.cbPrintDontShowWindowsDialog.Name = "cbPrintDontShowWindowsDialog";
		this.cbPrintDontShowWindowsDialog.UseVisualStyleBackColor = true;
		this.cbPrintDontShowWindowsDialog.CheckedChanged += new System.EventHandler(cbPrintDontShowWindowsDialog_CheckedChanged);
		resources.ApplyResources(this.cbDontShowPrintSettingDialog, "cbDontShowPrintSettingDialog");
		this.cbDontShowPrintSettingDialog.Name = "cbDontShowPrintSettingDialog";
		this.cbDontShowPrintSettingDialog.UseVisualStyleBackColor = true;
		this.cbDontShowPrintSettingDialog.CheckedChanged += new System.EventHandler(cbDontShowPrintSettingDialog_CheckedChanged);
		resources.ApplyResources(this.btnShowImagePrintSettings, "btnShowImagePrintSettings");
		this.btnShowImagePrintSettings.Name = "btnShowImagePrintSettings";
		this.btnShowImagePrintSettings.UseVisualStyleBackColor = true;
		this.btnShowImagePrintSettings.Click += new System.EventHandler(btnShowImagePrintSettings_Click);
		this.tpProxy.BackColor = System.Drawing.SystemColors.Window;
		this.tpProxy.Controls.Add(this.cbProxyMethod);
		this.tpProxy.Controls.Add(this.lblProxyMethod);
		this.tpProxy.Controls.Add(this.lblProxyHost);
		this.tpProxy.Controls.Add(this.txtProxyHost);
		this.tpProxy.Controls.Add(this.nudProxyPort);
		this.tpProxy.Controls.Add(this.lblProxyPort);
		this.tpProxy.Controls.Add(this.lblProxyPassword);
		this.tpProxy.Controls.Add(this.txtProxyPassword);
		this.tpProxy.Controls.Add(this.lblProxyUsername);
		this.tpProxy.Controls.Add(this.txtProxyUsername);
		resources.ApplyResources(this.tpProxy, "tpProxy");
		this.tpProxy.Name = "tpProxy";
		this.cbProxyMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbProxyMethod.FormattingEnabled = true;
		resources.ApplyResources(this.cbProxyMethod, "cbProxyMethod");
		this.cbProxyMethod.Name = "cbProxyMethod";
		this.cbProxyMethod.SelectedIndexChanged += new System.EventHandler(cbProxyMethod_SelectedIndexChanged);
		resources.ApplyResources(this.lblProxyMethod, "lblProxyMethod");
		this.lblProxyMethod.Name = "lblProxyMethod";
		resources.ApplyResources(this.lblProxyHost, "lblProxyHost");
		this.lblProxyHost.Name = "lblProxyHost";
		resources.ApplyResources(this.txtProxyHost, "txtProxyHost");
		this.txtProxyHost.Name = "txtProxyHost";
		this.txtProxyHost.TextChanged += new System.EventHandler(txtProxyHost_TextChanged);
		resources.ApplyResources(this.nudProxyPort, "nudProxyPort");
		this.nudProxyPort.Maximum = new decimal(new int[4] { 65535, 0, 0, 0 });
		this.nudProxyPort.Name = "nudProxyPort";
		this.nudProxyPort.ValueChanged += new System.EventHandler(nudProxyPort_ValueChanged);
		resources.ApplyResources(this.lblProxyPort, "lblProxyPort");
		this.lblProxyPort.Name = "lblProxyPort";
		resources.ApplyResources(this.lblProxyPassword, "lblProxyPassword");
		this.lblProxyPassword.Name = "lblProxyPassword";
		resources.ApplyResources(this.txtProxyPassword, "txtProxyPassword");
		this.txtProxyPassword.Name = "txtProxyPassword";
		this.txtProxyPassword.UseSystemPasswordChar = true;
		this.txtProxyPassword.TextChanged += new System.EventHandler(txtProxyPassword_TextChanged);
		resources.ApplyResources(this.lblProxyUsername, "lblProxyUsername");
		this.lblProxyUsername.Name = "lblProxyUsername";
		resources.ApplyResources(this.txtProxyUsername, "txtProxyUsername");
		this.txtProxyUsername.Name = "txtProxyUsername";
		this.txtProxyUsername.TextChanged += new System.EventHandler(txtProxyUsername_TextChanged);
		this.tpAdvanced.BackColor = System.Drawing.SystemColors.Window;
		this.tpAdvanced.Controls.Add(this.pgSettings);
		resources.ApplyResources(this.tpAdvanced, "tpAdvanced");
		this.tpAdvanced.Name = "tpAdvanced";
		resources.ApplyResources(this.pgSettings, "pgSettings");
		this.pgSettings.Name = "pgSettings";
		this.pgSettings.PropertySort = System.Windows.Forms.PropertySort.Categorized;
		this.pgSettings.ToolbarVisible = false;
		resources.ApplyResources(this.tttvMain, "tttvMain");
		this.tttvMain.ImageList = null;
		this.tttvMain.LeftPanelBackColor = System.Drawing.SystemColors.Window;
		this.tttvMain.MainTabControl = null;
		this.tttvMain.Name = "tttvMain";
		this.tttvMain.SeparatorColor = System.Drawing.SystemColors.ControlDark;
		this.tttvMain.TreeViewFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 162);
		this.tttvMain.TreeViewSize = 175;
		this.tttvMain.TabChanged += new ShareX.HelpersLib.TabToTreeView.TabChangedEventHandler(tttvMain_TabChanged);
		resources.ApplyResources(this.lblSaveImageSubFolderPatternWindow, "lblSaveImageSubFolderPatternWindow");
		this.lblSaveImageSubFolderPatternWindow.Name = "lblSaveImageSubFolderPatternWindow";
		resources.ApplyResources(this.txtSaveImageSubFolderPatternWindow, "txtSaveImageSubFolderPatternWindow");
		this.txtSaveImageSubFolderPatternWindow.Name = "txtSaveImageSubFolderPatternWindow";
		this.txtSaveImageSubFolderPatternWindow.TextChanged += new System.EventHandler(txtSaveImageSubFolderPatternWindow_TextChanged);
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.tcSettings);
		base.Controls.Add(this.tttvMain);
		base.Name = "ApplicationSettingsForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.Shown += new System.EventHandler(SettingsForm_Shown);
		base.Resize += new System.EventHandler(SettingsForm_Resize);
		this.tcSettings.ResumeLayout(false);
		this.tpGeneral.ResumeLayout(false);
		this.tpGeneral.PerformLayout();
		this.tpTheme.ResumeLayout(false);
		this.tpTheme.PerformLayout();
		this.tpIntegration.ResumeLayout(false);
		this.gbFirefox.ResumeLayout(false);
		this.gbFirefox.PerformLayout();
		this.gbSteam.ResumeLayout(false);
		this.gbSteam.PerformLayout();
		this.gbChrome.ResumeLayout(false);
		this.gbChrome.PerformLayout();
		this.gbWindows.ResumeLayout(false);
		this.gbWindows.PerformLayout();
		this.tpPaths.ResumeLayout(false);
		this.tpPaths.PerformLayout();
		this.tpSettings.ResumeLayout(false);
		this.tpSettings.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudCleanupKeepFileCount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pbExportImportNote).EndInit();
		this.tpMainWindow.ResumeLayout(false);
		this.tpMainWindow.PerformLayout();
		this.gbListView.ResumeLayout(false);
		this.gbListView.PerformLayout();
		this.gbThumbnailView.ResumeLayout(false);
		this.gbThumbnailView.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudThumbnailViewThumbnailSizeHeight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudThumbnailViewThumbnailSizeWidth).EndInit();
		this.tpClipboardFormats.ResumeLayout(false);
		this.tpClipboardFormats.PerformLayout();
		this.tpUpload.ResumeLayout(false);
		this.tpUpload.PerformLayout();
		this.gbSecondaryFileUploaders.ResumeLayout(false);
		this.gbSecondaryImageUploaders.ResumeLayout(false);
		this.gbSecondaryTextUploaders.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.nudUploadLimit).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudRetryUpload).EndInit();
		this.tpHistory.ResumeLayout(false);
		this.gbHistory.ResumeLayout(false);
		this.gbHistory.PerformLayout();
		this.gbRecentLinks.ResumeLayout(false);
		this.gbRecentLinks.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRecentTasksMaxCount).EndInit();
		this.tpPrint.ResumeLayout(false);
		this.tpPrint.PerformLayout();
		this.tpProxy.ResumeLayout(false);
		this.tpProxy.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudProxyPort).EndInit();
		this.tpAdvanced.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
