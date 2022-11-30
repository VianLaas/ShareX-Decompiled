using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;
using ShareX.UploadersLib;

namespace ShareX;

public class TaskSettingsForm : Form
{
	private ToolStripDropDownItem tsmiImageFileUploaders;

	private ToolStripDropDownItem tsmiTextFileUploaders;

	private bool loaded;

	private IContainer components;

	private MenuButton btnAfterCapture;

	private ContextMenuStrip cmsAfterCapture;

	private MenuButton btnAfterUpload;

	private MenuButton btnDestinations;

	private ContextMenuStrip cmsAfterUpload;

	private CheckBox cbOverrideAfterCaptureSettings;

	private CheckBox cbOverrideAfterUploadSettings;

	private CheckBox cbOverrideDestinationSettings;

	private Label lblDescription;

	private TextBox tbDescription;

	private MenuButton btnTask;

	private ContextMenuStrip cmsTask;

	private TabControl tcTaskSettings;

	private TabPage tpImage;

	private TabPage tpCapture;

	private TabControl tcImage;

	private TabPage tpQuality;

	private Label lblImageFormat;

	private Label lblImageSizeLimitHint;

	private ComboBox cbImageFormat;

	private Label lblImageJPEGQualityHint;

	private Label lblImageJPEGQuality;

	private ComboBox cbImageGIFQuality;

	private Label lblImageGIFQuality;

	private NumericUpDown nudImageJPEGQuality;

	private NumericUpDown nudImageAutoUseJPEGSize;

	private TabPage tpEffects;

	private TabControl tcCapture;

	private TabPage tpCaptureGeneral;

	private CheckBox cbCaptureAutoHideTaskbar;

	private Label lblScreenshotDelayInfo;

	private NumericUpDown nudScreenshotDelay;

	private NumericUpDown nudCaptureShadowOffset;

	private CheckBox cbCaptureClientArea;

	private CheckBox cbCaptureShadow;

	private CheckBox cbShowCursor;

	private CheckBox cbCaptureTransparent;

	private TabPage tpRegionCapture;

	private TabPage tpScreenRecorder;

	private TabPage tpTask;

	private TabPage tpActions;

	private Button btnActionsEdit;

	private Button btnActionsRemove;

	private Button btnActionsAdd;

	private MyListView lvActions;

	private ColumnHeader chActionsName;

	private ColumnHeader chActionsPath;

	private ColumnHeader chActionsArgs;

	private TabPage tpUpload;

	private TabControl tcUpload;

	private TabPage tpUploadMain;

	private CheckBox cbFileUploadUseNamePattern;

	private Label lblNameFormatPattern;

	private TextBox txtNameFormatPatternActiveWindow;

	private Label lblNameFormatPatternActiveWindow;

	private TextBox txtNameFormatPattern;

	private Label lblNameFormatPatternPreview;

	private Label lblNameFormatPatternPreviewActiveWindow;

	private TabPage tpUploadClipboard;

	private CheckBox cbClipboardUploadShortenURL;

	private TabPage tpAdvanced;

	private PropertyGrid pgTaskSettings;

	private CheckBox cbOverrideImageSettings;

	private CheckBox cbOverrideCaptureSettings;

	private CheckBox cbOverrideActions;

	private CheckBox cbOverrideUploadSettings;

	private Panel pActions;

	private CheckBox cbOverrideAdvancedSettings;

	private CheckBox cbScreenRecorderFixedDuration;

	private NumericUpDown nudGIFFPS;

	private NumericUpDown nudScreenRecorderDuration;

	private Label lblGIFFPS;

	private TabPage tpWatchFolders;

	private CheckBox cbWatchFolderEnabled;

	private MyListView lvWatchFolderList;

	private ColumnHeader chWatchFolderFolderPath;

	private ColumnHeader chWatchFolderFilter;

	private ColumnHeader chWatchFolderIncludeSubdirectories;

	private Button btnWatchFolderRemove;

	private Button btnWatchFolderAdd;

	private TabPage tpGeneral;

	private CheckBox cbPlaySoundAfterCapture;

	private CheckBox cbPlaySoundAfterUpload;

	private CheckBox cbOverrideGeneralSettings;

	private TabPage tpTools;

	private NumericUpDown nudScreenRecorderStartDelay;

	private Button btnImageEffects;

	private CheckBox cbImageEffectOnlyRegionCapture;

	private CheckBox cbShowImageEffectsWindowAfterCapture;

	private CheckBox cbOverrideFTPAccount;

	private ComboBox cbFTPAccounts;

	private ContextMenuStrip cmsDestinations;

	private ToolStripMenuItem tsmiImageUploaders;

	private ToolStripMenuItem tsmiTextUploaders;

	private ToolStripMenuItem tsmiFileUploaders;

	private ToolStripMenuItem tsmiURLShorteners;

	private ToolStripMenuItem tsmiURLSharingServices;

	private ComboBox cbImageFileExist;

	private Label lblImageFileExist;

	private TabPage tpThumbnail;

	private Label lblThumbnailHeight;

	private Label lblThumbnailWidth;

	private NumericUpDown nudThumbnailHeight;

	private NumericUpDown nudThumbnailWidth;

	private Label lblThumbnailName;

	private TextBox txtThumbnailName;

	private Label lblThumbnailNamePreview;

	private CheckBox cbThumbnailIfSmaller;

	private CheckBox cbClipboardUploadAutoIndexFolder;

	private CheckBox cbClipboardUploadURLContents;

	private NumericUpDown nudScreenRecordFPS;

	private Label lblScreenRecordFPS;

	private Label lblScreenRecorderFixedDuration;

	private CheckBox cbClipboardUploadShareURL;

	private ColumnHeader chActionsExtensions;

	private Button btnActionsDuplicate;

	private Label lblImageEffectsNote;

	private Label lblCaptureShadowOffset;

	private CheckBox cbScreenRecordAutoStart;

	private Label lblScreenRecorderStartDelay;

	private TabToTreeView tttvMain;

	private Panel pImage;

	private Panel pCapture;

	private ComboBox cbCustomUploaders;

	private CheckBox cbOverrideCustomUploader;

	private Button btnScreenRecorderFFmpegOptions;

	private ComboBox cbNameFormatTimeZone;

	private CheckBox cbNameFormatCustomTimeZone;

	private Label lblCaptureCustomRegionWidth;

	private Label lblCaptureCustomRegionHeight;

	private Label lblCaptureCustomRegionY;

	private Label lblCaptureCustomRegionX;

	private NumericUpDown nudCaptureCustomRegionHeight;

	private NumericUpDown nudCaptureCustomRegionWidth;

	private NumericUpDown nudCaptureCustomRegionY;

	private NumericUpDown nudCaptureCustomRegionX;

	private CheckBox cbScreenRecorderShowCursor;

	private CheckBox cbOverrideToolsSettings;

	private TabPage tpFileNaming;

	private Label lblCaptureCustomRegion;

	private Button btnCaptureCustomRegionSelectRectangle;

	private CheckBox cbRegionCaptureMultiRegionMode;

	private Label lblRegionCaptureMouseRightClickAction;

	private ComboBox cbRegionCaptureMouse5ClickAction;

	private Label lblRegionCaptureMouse5ClickAction;

	private ComboBox cbRegionCaptureMouse4ClickAction;

	private Label lblRegionCaptureMouse4ClickAction;

	private ComboBox cbRegionCaptureMouseMiddleClickAction;

	private Label lblRegionCaptureMouseMiddleClickAction;

	private ComboBox cbRegionCaptureMouseRightClickAction;

	private CheckBox cbRegionCaptureDetectWindows;

	private CheckBox cbRegionCaptureDetectControls;

	private CheckBox cbRegionCaptureUseDimming;

	private CheckBox cbRegionCaptureUseCustomInfoText;

	private TextBox txtRegionCaptureCustomInfoText;

	private Label lblRegionCaptureSnapSizes;

	private ComboBox cbRegionCaptureSnapSizes;

	private Button btnRegionCaptureSnapSizesRemove;

	private Button btnRegionCaptureSnapSizesAdd;

	private Panel pRegionCaptureSnapSizes;

	private Button btnRegionCaptureSnapSizesDialogCancel;

	private Button btnRegionCaptureSnapSizesDialogAdd;

	private NumericUpDown nudRegionCaptureSnapSizesHeight;

	private Label RegionCaptureSnapSizesHeight;

	private NumericUpDown nudRegionCaptureSnapSizesWidth;

	private Label lblRegionCaptureSnapSizesWidth;

	private CheckBox cbRegionCaptureShowInfo;

	private CheckBox cbRegionCaptureShowMagnifier;

	private Label lblRegionCaptureMagnifierPixelCount;

	private CheckBox cbRegionCaptureUseSquareMagnifier;

	private NumericUpDown nudRegionCaptureMagnifierPixelCount;

	private NumericUpDown nudRegionCaptureMagnifierPixelSize;

	private Label lblRegionCaptureMagnifierPixelSize;

	private CheckBox cbRegionCaptureShowCrosshair;

	private FlowLayoutPanel flpRegionCaptureFixedSize;

	private Label lblRegionCaptureFixedSizeWidth;

	private NumericUpDown nudRegionCaptureFixedSizeWidth;

	private Label lblRegionCaptureFixedSizeHeight;

	private CheckBox cbRegionCaptureIsFixedSize;

	private NumericUpDown nudRegionCaptureFixedSizeHeight;

	private CheckBox cbRegionCaptureShowFPS;

	private CheckBox cbImageAutoUseJPEG;

	private Panel pTools;

	private TextBox txtToolsScreenColorPickerFormat;

	private Label lblToolsScreenColorPickerFormat;

	private TabPage tpUploaderFilters;

	private MyListView lvUploaderFiltersList;

	private ColumnHeader chUploaderFiltersName;

	private ColumnHeader chUploaderFiltersExtension;

	private Button btnUploaderFiltersRemove;

	private Button btnUploaderFiltersUpdate;

	private Button btnUploaderFiltersAdd;

	private Label lblUploaderFiltersDestination;

	private ComboBox cbUploaderFiltersDestination;

	private Label lblUploaderFiltersExtensionsExample;

	private Label lblUploaderFiltersExtensions;

	private TextBox txtUploaderFiltersExtensions;

	private ComboBox cbImagePNGBitDepth;

	private Label lblImagePNGBitDepth;

	private Button btnWatchFolderEdit;

	private CheckBox cbScreenRecordConfirmAbort;

	private CheckBox cbFileUploadReplaceProblematicCharacters;

	private CheckBox cbScreenRecordTwoPassEncoding;

	private TabPage tpOCR;

	private Label lblOCRDefaultLanguage;

	private ComboBox cbCaptureOCRDefaultLanguage;

	private CheckBox cbCaptureOCRSilent;

	private CheckBox cbCaptureOCRAutoCopy;

	private Label lblScreenshotDelay;

	private Label lblAutoIncrementNumber;

	private NumericUpDown nudAutoIncrementNumber;

	private Button btnAutoIncrementNumber;

	private Label lblActionsNote;

	private CheckBox cbScreenRecordTransparentRegion;

	private CheckBox cbOverrideScreenshotsFolder;

	private Button btnScreenshotsFolderBrowse;

	private TextBox txtScreenshotsFolder;

	private CheckBox cbURLRegexReplace;

	private Label lblURLRegexReplacePattern;

	private Label lblURLRegexReplaceReplacement;

	private TextBox txtURLRegexReplacePattern;

	private TextBox txtURLRegexReplaceReplacement;

	private TextBox txtToolsScreenColorPickerInfoText;

	private Label lblToolsScreenColorPickerInfoText;

	private TextBox txtToolsScreenColorPickerFormatCtrl;

	private Label lblToolsScreenColorPickerFormatCtrl;

	private TabControl tcGeneral;

	private TabPage tpGeneralMain;

	private TabPage tpNotifications;

	private CheckBox cbDisableNotificationsOnFullscreen;

	private CheckBox cbDisableNotifications;

	private GroupBox gbToastWindow;

	private Label lblToastWindowLeftClickAction;

	private Label lblToastWindowSize;

	private Label lblToastWindowPlacement;

	private Label lblToastWindowFadeDuration;

	private Label lblToastWindowDuration;

	private Label lblToastWindowMiddleClickAction;

	private Label lblToastWindowRightClickAction;

	private Label lblToastWindowSizeX;

	private ComboBox cbToastWindowMiddleClickAction;

	private ComboBox cbToastWindowRightClickAction;

	private ComboBox cbToastWindowLeftClickAction;

	private NumericUpDown nudToastWindowSizeHeight;

	private NumericUpDown nudToastWindowSizeWidth;

	private ComboBox cbToastWindowPlacement;

	private NumericUpDown nudToastWindowFadeDuration;

	private NumericUpDown nudToastWindowDuration;

	private TextBox txtCustomErrorSoundPath;

	private TextBox txtCustomTaskCompletedSoundPath;

	private TextBox txtCustomCaptureSoundPath;

	private CheckBox cbUseCustomErrorSound;

	private CheckBox cbUseCustomTaskCompletedSound;

	private CheckBox cbUseCustomCaptureSound;

	private Button btnCustomErrorSoundPath;

	private Button btnCustomTaskCompletedSoundPath;

	private Button btnCustomCaptureSoundPath;

	private CheckBox cbShowToastNotificationAfterTaskCompleted;

	private Label lblToastWindowFadeDurationSeconds;

	private Label lblToastWindowDurationSeconds;

	private Button btnActions;

	private CheckBox cbImageAutoJPEGQuality;

	private Label lblTask;

	private CheckBox cbToastWindowAutoHide;

	private NumericUpDown nudRegionCaptureFPSLimit;

	private Label lblRegionCaptureFPSLimit;

	private CheckBox cbRegionCaptureActiveMonitorMode;

	private Button btnCaptureOCRHelp;

	public TaskSettings TaskSettings { get; private set; }

	public bool IsDefault { get; private set; }

	public TaskSettingsForm(TaskSettings hotkeySetting, bool isDefault = false)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		tsmiURLShorteners.Image = (ShareXResources.IsDarkTheme ? Resources.edit_scale_white : Resources.edit_scale);
		TaskSettings = hotkeySetting;
		IsDefault = isDefault;
		UpdateWindowTitle();
		object[] items2;
		if (IsDefault)
		{
			tcTaskSettings.TabPages.Remove(tpTask);
			cbOverrideGeneralSettings.Visible = (cbOverrideImageSettings.Visible = (cbOverrideCaptureSettings.Visible = (cbOverrideActions.Visible = (cbOverrideUploadSettings.Visible = (cbOverrideToolsSettings.Visible = (cbOverrideAdvancedSettings.Visible = false))))));
		}
		else
		{
			AddEnumItemsContextMenu(delegate(HotkeyType x)
			{
				TaskSettings.Job = x;
				UpdateWindowTitle();
			}, cmsTask);
			SetEnumCheckedContextMenu(TaskSettings.Job, cmsTask);
			tbDescription.Text = TaskSettings.Description;
			cbOverrideAfterCaptureSettings.Checked = !TaskSettings.UseDefaultAfterCaptureJob;
			btnAfterCapture.Enabled = !TaskSettings.UseDefaultAfterCaptureJob;
			AddMultiEnumItemsContextMenu(delegate(AfterCaptureTasks x)
			{
				TaskSettings.AfterCaptureJob = TaskSettings.AfterCaptureJob.Swap<AfterCaptureTasks>(x);
			}, cmsAfterCapture);
			SetMultiEnumCheckedContextMenu(TaskSettings.AfterCaptureJob, cmsAfterCapture);
			cbOverrideAfterUploadSettings.Checked = !TaskSettings.UseDefaultAfterUploadJob;
			btnAfterUpload.Enabled = !TaskSettings.UseDefaultAfterUploadJob;
			AddMultiEnumItemsContextMenu(delegate(AfterUploadTasks x)
			{
				TaskSettings.AfterUploadJob = TaskSettings.AfterUploadJob.Swap<AfterUploadTasks>(x);
			}, cmsAfterUpload);
			SetMultiEnumCheckedContextMenu(TaskSettings.AfterUploadJob, cmsAfterUpload);
			cbOverrideDestinationSettings.Checked = !TaskSettings.UseDefaultDestinations;
			btnDestinations.Enabled = !TaskSettings.UseDefaultDestinations;
			AddEnumItems(delegate(ImageDestination x)
			{
				TaskSettings.ImageDestination = x;
				if (x == ImageDestination.FileUploader)
				{
					SetEnumChecked(TaskSettings.ImageFileDestination, tsmiImageFileUploaders);
				}
				else
				{
					MainForm.Uncheck(tsmiImageFileUploaders);
				}
			}, tsmiImageUploaders);
			tsmiImageFileUploaders = (ToolStripDropDownItem)tsmiImageUploaders.DropDownItems[tsmiImageUploaders.DropDownItems.Count - 1];
			AddEnumItems(delegate(FileDestination x)
			{
				TaskSettings.ImageFileDestination = x;
				tsmiImageFileUploaders.PerformClick();
			}, tsmiImageFileUploaders);
			SetEnumChecked(TaskSettings.ImageDestination, tsmiImageUploaders);
			MainForm.SetImageFileDestinationChecked(TaskSettings.ImageDestination, TaskSettings.ImageFileDestination, tsmiImageFileUploaders);
			AddEnumItems(delegate(TextDestination x)
			{
				TaskSettings.TextDestination = x;
				if (x == TextDestination.FileUploader)
				{
					SetEnumChecked(TaskSettings.TextFileDestination, tsmiTextFileUploaders);
				}
				else
				{
					MainForm.Uncheck(tsmiTextFileUploaders);
				}
			}, tsmiTextUploaders);
			tsmiTextFileUploaders = (ToolStripDropDownItem)tsmiTextUploaders.DropDownItems[tsmiTextUploaders.DropDownItems.Count - 1];
			AddEnumItems(delegate(FileDestination x)
			{
				TaskSettings.TextFileDestination = x;
				tsmiTextFileUploaders.PerformClick();
			}, tsmiTextFileUploaders);
			SetEnumChecked(TaskSettings.TextDestination, tsmiTextUploaders);
			MainForm.SetTextFileDestinationChecked(TaskSettings.TextDestination, TaskSettings.TextFileDestination, tsmiTextFileUploaders);
			AddEnumItems(delegate(FileDestination x)
			{
				TaskSettings.FileDestination = x;
			}, tsmiFileUploaders);
			SetEnumChecked(TaskSettings.FileDestination, tsmiFileUploaders);
			AddEnumItems(delegate(UrlShortenerType x)
			{
				TaskSettings.URLShortenerDestination = x;
			}, tsmiURLShorteners);
			SetEnumChecked(TaskSettings.URLShortenerDestination, tsmiURLShorteners);
			AddEnumItems(delegate(URLSharingServices x)
			{
				TaskSettings.URLSharingServiceDestination = x;
			}, tsmiURLSharingServices);
			SetEnumChecked(TaskSettings.URLSharingServiceDestination, tsmiURLSharingServices);
			UpdateDestinationStates();
			if (Program.UploadersConfig != null)
			{
				cbOverrideFTPAccount.Enabled = (cbFTPAccounts.Enabled = Program.UploadersConfig.FTPAccountList.Count > 0);
				if (Program.UploadersConfig.FTPAccountList.Count > 0)
				{
					cbOverrideFTPAccount.Checked = TaskSettings.OverrideFTP;
					cbFTPAccounts.Enabled = TaskSettings.OverrideFTP;
					cbFTPAccounts.Items.Clear();
					ComboBox.ObjectCollection items = cbFTPAccounts.Items;
					items2 = Program.UploadersConfig.FTPAccountList.ToArray();
					items.AddRange(items2);
					cbFTPAccounts.SelectedIndex = TaskSettings.FTPIndex.BetweenOrDefault(0, Program.UploadersConfig.FTPAccountList.Count - 1, 0);
				}
				cbOverrideCustomUploader.Enabled = (cbCustomUploaders.Enabled = Program.UploadersConfig.CustomUploadersList.Count > 0);
				if (Program.UploadersConfig.CustomUploadersList.Count > 0)
				{
					cbOverrideCustomUploader.Checked = TaskSettings.OverrideCustomUploader;
					cbCustomUploaders.Enabled = TaskSettings.OverrideCustomUploader;
					cbCustomUploaders.Items.Clear();
					ComboBox.ObjectCollection items3 = cbCustomUploaders.Items;
					items2 = Program.UploadersConfig.CustomUploadersList.ToArray();
					items3.AddRange(items2);
					cbCustomUploaders.SelectedIndex = TaskSettings.CustomUploaderIndex.BetweenOrDefault(0, Program.UploadersConfig.CustomUploadersList.Count - 1, 0);
				}
			}
			cbOverrideScreenshotsFolder.Checked = TaskSettings.OverrideScreenshotsFolder;
			CodeMenu.Create<CodeMenuEntryFilename>(txtScreenshotsFolder, CodeMenuEntryFilename.t, CodeMenuEntryFilename.pn, CodeMenuEntryFilename.i, CodeMenuEntryFilename.width, CodeMenuEntryFilename.height, CodeMenuEntryFilename.n).MenuLocationBottom = true;
			txtScreenshotsFolder.Text = TaskSettings.ScreenshotsFolder;
			txtScreenshotsFolder.Enabled = (btnScreenshotsFolderBrowse.Enabled = TaskSettings.OverrideScreenshotsFolder);
			UpdateTaskTabMenuNames();
			cbOverrideGeneralSettings.Checked = !TaskSettings.UseDefaultGeneralSettings;
			cbOverrideImageSettings.Checked = !TaskSettings.UseDefaultImageSettings;
			cbOverrideCaptureSettings.Checked = !TaskSettings.UseDefaultCaptureSettings;
			cbOverrideActions.Checked = !TaskSettings.UseDefaultActions;
			cbOverrideUploadSettings.Checked = !TaskSettings.UseDefaultUploadSettings;
			cbOverrideToolsSettings.Checked = !TaskSettings.UseDefaultToolsSettings;
			cbOverrideAdvancedSettings.Checked = !TaskSettings.UseDefaultAdvancedSettings;
		}
		UpdateDefaultSettingVisibility();
		tttvMain.MainTabControl = tcTaskSettings;
		cbPlaySoundAfterCapture.Checked = TaskSettings.GeneralSettings.PlaySoundAfterCapture;
		cbPlaySoundAfterUpload.Checked = TaskSettings.GeneralSettings.PlaySoundAfterUpload;
		cbShowToastNotificationAfterTaskCompleted.Checked = TaskSettings.GeneralSettings.ShowToastNotificationAfterTaskCompleted;
		gbToastWindow.Enabled = TaskSettings.GeneralSettings.ShowToastNotificationAfterTaskCompleted;
		nudToastWindowDuration.SetValue((decimal)TaskSettings.GeneralSettings.ToastWindowDuration);
		nudToastWindowFadeDuration.SetValue((decimal)TaskSettings.GeneralSettings.ToastWindowFadeDuration);
		ComboBox.ObjectCollection items4 = cbToastWindowPlacement.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<ContentAlignment>();
		items4.AddRange(items2);
		cbToastWindowPlacement.SelectedIndex = TaskSettings.GeneralSettings.ToastWindowPlacement.GetIndex();
		nudToastWindowSizeWidth.SetValue(TaskSettings.GeneralSettings.ToastWindowSize.Width);
		nudToastWindowSizeHeight.SetValue(TaskSettings.GeneralSettings.ToastWindowSize.Height);
		ComboBox.ObjectCollection items5 = cbToastWindowLeftClickAction.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<ToastClickAction>();
		items5.AddRange(items2);
		cbToastWindowLeftClickAction.SelectedIndex = (int)TaskSettings.GeneralSettings.ToastWindowLeftClickAction;
		ComboBox.ObjectCollection items6 = cbToastWindowRightClickAction.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<ToastClickAction>();
		items6.AddRange(items2);
		cbToastWindowRightClickAction.SelectedIndex = (int)TaskSettings.GeneralSettings.ToastWindowRightClickAction;
		ComboBox.ObjectCollection items7 = cbToastWindowMiddleClickAction.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<ToastClickAction>();
		items7.AddRange(items2);
		cbToastWindowMiddleClickAction.SelectedIndex = (int)TaskSettings.GeneralSettings.ToastWindowMiddleClickAction;
		cbToastWindowAutoHide.Checked = TaskSettings.GeneralSettings.ToastWindowAutoHide;
		cbUseCustomCaptureSound.Checked = TaskSettings.GeneralSettings.UseCustomCaptureSound;
		txtCustomCaptureSoundPath.Enabled = (btnCustomCaptureSoundPath.Enabled = TaskSettings.GeneralSettings.UseCustomCaptureSound);
		txtCustomCaptureSoundPath.Text = TaskSettings.GeneralSettings.CustomCaptureSoundPath;
		cbUseCustomTaskCompletedSound.Checked = TaskSettings.GeneralSettings.UseCustomTaskCompletedSound;
		txtCustomTaskCompletedSoundPath.Enabled = (btnCustomTaskCompletedSoundPath.Enabled = TaskSettings.GeneralSettings.UseCustomTaskCompletedSound);
		txtCustomTaskCompletedSoundPath.Text = TaskSettings.GeneralSettings.CustomTaskCompletedSoundPath;
		cbUseCustomErrorSound.Checked = TaskSettings.GeneralSettings.UseCustomErrorSound;
		txtCustomErrorSoundPath.Enabled = (btnCustomErrorSoundPath.Enabled = TaskSettings.GeneralSettings.UseCustomErrorSound);
		txtCustomErrorSoundPath.Text = TaskSettings.GeneralSettings.CustomErrorSoundPath;
		cbDisableNotifications.Checked = TaskSettings.GeneralSettings.DisableNotifications;
		cbDisableNotificationsOnFullscreen.Checked = TaskSettings.GeneralSettings.DisableNotificationsOnFullscreen;
		ComboBox.ObjectCollection items8 = cbImageFormat.Items;
		items2 = Enum.GetNames(typeof(EImageFormat));
		items8.AddRange(items2);
		cbImageFormat.SelectedIndex = (int)TaskSettings.ImageSettings.ImageFormat;
		ComboBox.ObjectCollection items9 = cbImagePNGBitDepth.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<PNGBitDepth>();
		items9.AddRange(items2);
		cbImagePNGBitDepth.SelectedIndex = (int)TaskSettings.ImageSettings.ImagePNGBitDepth;
		nudImageJPEGQuality.SetValue(TaskSettings.ImageSettings.ImageJPEGQuality);
		ComboBox.ObjectCollection items10 = cbImageGIFQuality.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<GIFQuality>();
		items10.AddRange(items2);
		cbImageGIFQuality.SelectedIndex = (int)TaskSettings.ImageSettings.ImageGIFQuality;
		cbImageAutoUseJPEG.Checked = TaskSettings.ImageSettings.ImageAutoUseJPEG;
		nudImageAutoUseJPEGSize.Enabled = TaskSettings.ImageSettings.ImageAutoUseJPEG;
		cbImageAutoJPEGQuality.Enabled = TaskSettings.ImageSettings.ImageAutoUseJPEG;
		nudImageAutoUseJPEGSize.SetValue(TaskSettings.ImageSettings.ImageAutoUseJPEGSize);
		cbImageAutoJPEGQuality.Checked = TaskSettings.ImageSettings.ImageAutoJPEGQuality;
		cbImageFileExist.Items.Clear();
		ComboBox.ObjectCollection items11 = cbImageFileExist.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<FileExistAction>();
		items11.AddRange(items2);
		cbImageFileExist.SelectedIndex = (int)TaskSettings.ImageSettings.FileExistAction;
		cbShowImageEffectsWindowAfterCapture.Checked = TaskSettings.ImageSettings.ShowImageEffectsWindowAfterCapture;
		cbImageEffectOnlyRegionCapture.Checked = TaskSettings.ImageSettings.ImageEffectOnlyRegionCapture;
		nudThumbnailWidth.SetValue(TaskSettings.ImageSettings.ThumbnailWidth);
		nudThumbnailHeight.SetValue(TaskSettings.ImageSettings.ThumbnailHeight);
		txtThumbnailName.Text = TaskSettings.ImageSettings.ThumbnailName;
		lblThumbnailNamePreview.Text = "ImageName" + TaskSettings.ImageSettings.ThumbnailName + ".jpg";
		cbThumbnailIfSmaller.Checked = TaskSettings.ImageSettings.ThumbnailCheckSize;
		cbShowCursor.Checked = TaskSettings.CaptureSettings.ShowCursor;
		nudScreenshotDelay.SetValue(TaskSettings.CaptureSettings.ScreenshotDelay);
		cbCaptureTransparent.Checked = TaskSettings.CaptureSettings.CaptureTransparent;
		cbCaptureShadow.Enabled = TaskSettings.CaptureSettings.CaptureTransparent;
		cbCaptureShadow.Checked = TaskSettings.CaptureSettings.CaptureShadow;
		nudCaptureShadowOffset.SetValue(TaskSettings.CaptureSettings.CaptureShadowOffset);
		cbCaptureClientArea.Checked = TaskSettings.CaptureSettings.CaptureClientArea;
		cbCaptureAutoHideTaskbar.Checked = TaskSettings.CaptureSettings.CaptureAutoHideTaskbar;
		nudCaptureCustomRegionX.SetValue(TaskSettings.CaptureSettings.CaptureCustomRegion.X);
		nudCaptureCustomRegionY.SetValue(TaskSettings.CaptureSettings.CaptureCustomRegion.Y);
		nudCaptureCustomRegionWidth.SetValue(TaskSettings.CaptureSettings.CaptureCustomRegion.Width);
		nudCaptureCustomRegionHeight.SetValue(TaskSettings.CaptureSettings.CaptureCustomRegion.Height);
		cbRegionCaptureMultiRegionMode.Checked = !TaskSettings.CaptureSettings.SurfaceOptions.QuickCrop;
		ComboBox.ObjectCollection items12 = cbRegionCaptureMouseRightClickAction.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<RegionCaptureAction>();
		items12.AddRange(items2);
		cbRegionCaptureMouseRightClickAction.SelectedIndex = (int)TaskSettings.CaptureSettings.SurfaceOptions.RegionCaptureActionRightClick;
		ComboBox.ObjectCollection items13 = cbRegionCaptureMouseMiddleClickAction.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<RegionCaptureAction>();
		items13.AddRange(items2);
		cbRegionCaptureMouseMiddleClickAction.SelectedIndex = (int)TaskSettings.CaptureSettings.SurfaceOptions.RegionCaptureActionMiddleClick;
		ComboBox.ObjectCollection items14 = cbRegionCaptureMouse4ClickAction.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<RegionCaptureAction>();
		items14.AddRange(items2);
		cbRegionCaptureMouse4ClickAction.SelectedIndex = (int)TaskSettings.CaptureSettings.SurfaceOptions.RegionCaptureActionX1Click;
		ComboBox.ObjectCollection items15 = cbRegionCaptureMouse5ClickAction.Items;
		items2 = Helpers.GetLocalizedEnumDescriptions<RegionCaptureAction>();
		items15.AddRange(items2);
		cbRegionCaptureMouse5ClickAction.SelectedIndex = (int)TaskSettings.CaptureSettings.SurfaceOptions.RegionCaptureActionX2Click;
		cbRegionCaptureDetectWindows.Checked = TaskSettings.CaptureSettings.SurfaceOptions.DetectWindows;
		cbRegionCaptureDetectControls.Enabled = TaskSettings.CaptureSettings.SurfaceOptions.DetectWindows;
		cbRegionCaptureDetectControls.Checked = TaskSettings.CaptureSettings.SurfaceOptions.DetectControls;
		cbRegionCaptureUseDimming.Checked = TaskSettings.CaptureSettings.SurfaceOptions.UseDimming;
		cbRegionCaptureUseCustomInfoText.Checked = TaskSettings.CaptureSettings.SurfaceOptions.UseCustomInfoText;
		txtRegionCaptureCustomInfoText.Enabled = TaskSettings.CaptureSettings.SurfaceOptions.UseCustomInfoText;
		TaskSettings.CaptureSettings.SurfaceOptions.CustomInfoText = TaskSettings.CaptureSettings.SurfaceOptions.CustomInfoText.Replace("\r\n", "$n").Replace("\n", "$n");
		CodeMenu.Create(txtRegionCaptureCustomInfoText, Array.Empty<CodeMenuEntryPixelInfo>());
		txtRegionCaptureCustomInfoText.Text = TaskSettings.CaptureSettings.SurfaceOptions.CustomInfoText;
		ComboBox.ObjectCollection items16 = cbRegionCaptureSnapSizes.Items;
		items2 = TaskSettings.CaptureSettings.SurfaceOptions.SnapSizes.ToArray();
		items16.AddRange(items2);
		cbRegionCaptureShowInfo.Checked = TaskSettings.CaptureSettings.SurfaceOptions.ShowInfo;
		cbRegionCaptureShowMagnifier.Checked = TaskSettings.CaptureSettings.SurfaceOptions.ShowMagnifier;
		cbRegionCaptureUseSquareMagnifier.Enabled = (nudRegionCaptureMagnifierPixelCount.Enabled = (nudRegionCaptureMagnifierPixelSize.Enabled = TaskSettings.CaptureSettings.SurfaceOptions.ShowMagnifier));
		cbRegionCaptureUseSquareMagnifier.Checked = TaskSettings.CaptureSettings.SurfaceOptions.UseSquareMagnifier;
		nudRegionCaptureMagnifierPixelCount.Minimum = 3m;
		nudRegionCaptureMagnifierPixelCount.Maximum = 35m;
		nudRegionCaptureMagnifierPixelCount.SetValue(TaskSettings.CaptureSettings.SurfaceOptions.MagnifierPixelCount);
		nudRegionCaptureMagnifierPixelSize.Minimum = 3m;
		nudRegionCaptureMagnifierPixelSize.Maximum = 30m;
		nudRegionCaptureMagnifierPixelSize.SetValue(TaskSettings.CaptureSettings.SurfaceOptions.MagnifierPixelSize);
		cbRegionCaptureShowCrosshair.Checked = TaskSettings.CaptureSettings.SurfaceOptions.ShowCrosshair;
		cbRegionCaptureIsFixedSize.Checked = TaskSettings.CaptureSettings.SurfaceOptions.IsFixedSize;
		nudRegionCaptureFixedSizeWidth.Enabled = (nudRegionCaptureFixedSizeHeight.Enabled = TaskSettings.CaptureSettings.SurfaceOptions.IsFixedSize);
		nudRegionCaptureFixedSizeWidth.SetValue(TaskSettings.CaptureSettings.SurfaceOptions.FixedSize.Width);
		nudRegionCaptureFixedSizeHeight.SetValue(TaskSettings.CaptureSettings.SurfaceOptions.FixedSize.Height);
		cbRegionCaptureShowFPS.Checked = TaskSettings.CaptureSettings.SurfaceOptions.ShowFPS;
		nudRegionCaptureFPSLimit.SetValue(TaskSettings.CaptureSettings.SurfaceOptions.FPSLimit);
		cbRegionCaptureActiveMonitorMode.Checked = TaskSettings.CaptureSettings.SurfaceOptions.ActiveMonitorMode;
		nudScreenRecordFPS.SetValue(TaskSettings.CaptureSettings.ScreenRecordFPS);
		nudGIFFPS.SetValue(TaskSettings.CaptureSettings.GIFFPS);
		cbScreenRecorderFixedDuration.Checked = (nudScreenRecorderDuration.Enabled = TaskSettings.CaptureSettings.ScreenRecordFixedDuration);
		nudScreenRecorderDuration.SetValue((decimal)TaskSettings.CaptureSettings.ScreenRecordDuration);
		cbScreenRecordAutoStart.Checked = (nudScreenRecorderStartDelay.Enabled = TaskSettings.CaptureSettings.ScreenRecordAutoStart);
		nudScreenRecorderStartDelay.SetValue((decimal)TaskSettings.CaptureSettings.ScreenRecordStartDelay);
		cbScreenRecorderShowCursor.Checked = TaskSettings.CaptureSettings.ScreenRecordShowCursor;
		cbScreenRecordTwoPassEncoding.Checked = TaskSettings.CaptureSettings.ScreenRecordTwoPassEncoding;
		cbScreenRecordTransparentRegion.Checked = TaskSettings.CaptureSettings.ScreenRecordTransparentRegion;
		cbScreenRecordConfirmAbort.Checked = TaskSettings.CaptureSettings.ScreenRecordAskConfirmationOnAbort;
		OCROptions ocrOptions = TaskSettings.CaptureSettings.OCROptions;
		try
		{
			OCRLanguage[] array = OCRHelper.AvailableLanguages.OrderBy((OCRLanguage x) => x.DisplayName).ToArray();
			if (array.Length != 0)
			{
				ComboBox.ObjectCollection items17 = cbCaptureOCRDefaultLanguage.Items;
				items2 = array;
				items17.AddRange(items2);
				if (ocrOptions.Language == null)
				{
					cbCaptureOCRDefaultLanguage.SelectedIndex = 0;
					ocrOptions.Language = array[0].LanguageTag;
				}
				else
				{
					int num = Array.FindIndex(array, (OCRLanguage x) => x.LanguageTag.Equals(ocrOptions.Language, StringComparison.OrdinalIgnoreCase));
					if (num >= 0)
					{
						cbCaptureOCRDefaultLanguage.SelectedIndex = num;
					}
					else
					{
						cbCaptureOCRDefaultLanguage.SelectedIndex = 0;
						ocrOptions.Language = array[0].LanguageTag;
					}
				}
			}
		}
		catch
		{
			cbCaptureOCRDefaultLanguage.Enabled = false;
		}
		cbCaptureOCRSilent.Checked = ocrOptions.Silent;
		cbCaptureOCRAutoCopy.Enabled = !ocrOptions.Silent;
		cbCaptureOCRAutoCopy.Checked = ocrOptions.AutoCopy;
		txtNameFormatPattern.Text = TaskSettings.UploadSettings.NameFormatPattern;
		txtNameFormatPatternActiveWindow.Text = TaskSettings.UploadSettings.NameFormatPatternActiveWindow;
		CodeMenu.Create<CodeMenuEntryFilename>(txtNameFormatPattern, CodeMenuEntryFilename.n, CodeMenuEntryFilename.t, CodeMenuEntryFilename.pn);
		CodeMenu.Create<CodeMenuEntryFilename>(txtNameFormatPatternActiveWindow, CodeMenuEntryFilename.n);
		cbFileUploadUseNamePattern.Checked = TaskSettings.UploadSettings.FileUploadUseNamePattern;
		nudAutoIncrementNumber.Value = Program.Settings.NameParserAutoIncrementNumber;
		UpdateNameFormatPreviews();
		cbNameFormatCustomTimeZone.Checked = (cbNameFormatTimeZone.Enabled = TaskSettings.UploadSettings.UseCustomTimeZone);
		ComboBox.ObjectCollection items18 = cbNameFormatTimeZone.Items;
		items2 = TimeZoneInfo.GetSystemTimeZones().ToArray();
		items18.AddRange(items2);
		for (int i = 0; i < cbNameFormatTimeZone.Items.Count; i++)
		{
			if (cbNameFormatTimeZone.Items[i].Equals(TaskSettings.UploadSettings.CustomTimeZone))
			{
				cbNameFormatTimeZone.SelectedIndex = i;
				break;
			}
		}
		cbFileUploadReplaceProblematicCharacters.Checked = TaskSettings.UploadSettings.FileUploadReplaceProblematicCharacters;
		cbURLRegexReplace.Checked = TaskSettings.UploadSettings.URLRegexReplace;
		lblURLRegexReplacePattern.Enabled = (txtURLRegexReplacePattern.Enabled = (lblURLRegexReplaceReplacement.Enabled = (txtURLRegexReplaceReplacement.Enabled = TaskSettings.UploadSettings.URLRegexReplace)));
		txtURLRegexReplacePattern.Text = TaskSettings.UploadSettings.URLRegexReplacePattern;
		txtURLRegexReplaceReplacement.Text = TaskSettings.UploadSettings.URLRegexReplaceReplacement;
		cbClipboardUploadURLContents.Checked = TaskSettings.UploadSettings.ClipboardUploadURLContents;
		cbClipboardUploadShortenURL.Checked = TaskSettings.UploadSettings.ClipboardUploadShortenURL;
		cbClipboardUploadShareURL.Checked = TaskSettings.UploadSettings.ClipboardUploadShareURL;
		cbClipboardUploadAutoIndexFolder.Checked = TaskSettings.UploadSettings.ClipboardUploadAutoIndexFolder;
		ComboBox.ObjectCollection items19 = cbUploaderFiltersDestination.Items;
		items2 = UploaderFactory.AllGenericUploaderServices.OrderBy((IGenericUploaderService x) => x.ServiceName).ToArray();
		items19.AddRange(items2);
		if (TaskSettings.UploadSettings.UploaderFilters == null)
		{
			TaskSettings.UploadSettings.UploaderFilters = new List<UploaderFilter>();
		}
		foreach (UploaderFilter uploaderFilter in TaskSettings.UploadSettings.UploaderFilters)
		{
			AddUploaderFilterToList(uploaderFilter);
		}
		TaskHelpers.AddDefaultExternalPrograms(TaskSettings);
		TaskSettings.ExternalPrograms.ForEach(AddFileAction);
		cbWatchFolderEnabled.Checked = TaskSettings.WatchFolderEnabled;
		if (TaskSettings.WatchFolderList == null)
		{
			TaskSettings.WatchFolderList = new List<WatchFolderSettings>();
		}
		else
		{
			foreach (WatchFolderSettings watchFolder in TaskSettings.WatchFolderList)
			{
				WatchFolderAdd(watchFolder);
			}
		}
		CodeMenu.Create(txtToolsScreenColorPickerFormat, Array.Empty<CodeMenuEntryPixelInfo>());
		txtToolsScreenColorPickerFormat.Text = TaskSettings.ToolsSettings.ScreenColorPickerFormat;
		CodeMenu.Create(txtToolsScreenColorPickerFormatCtrl, Array.Empty<CodeMenuEntryPixelInfo>());
		txtToolsScreenColorPickerFormatCtrl.Text = TaskSettings.ToolsSettings.ScreenColorPickerFormatCtrl;
		CodeMenu.Create(txtToolsScreenColorPickerInfoText, Array.Empty<CodeMenuEntryPixelInfo>());
		txtToolsScreenColorPickerInfoText.Text = TaskSettings.ToolsSettings.ScreenColorPickerInfoText;
		pgTaskSettings.SelectedObject = TaskSettings.AdvancedSettings;
		loaded = true;
	}

	private void TaskSettingsForm_Resize(object sender, EventArgs e)
	{
		Refresh();
	}

	private void tttvMain_TabChanged(TabPage tabPage)
	{
		if (IsDefault && (tabPage == tpGeneralMain || tabPage == tpUploadMain))
		{
			tttvMain.SelectChildNode();
		}
	}

	private void UpdateWindowTitle()
	{
		if (IsDefault)
		{
			Text = "ShareX - " + Resources.TaskSettingsForm_UpdateWindowTitle_Task_settings;
		}
		else
		{
			Text = "ShareX - " + string.Format(Resources.TaskSettingsForm_UpdateWindowTitle_Task_settings_for__0_, TaskSettings);
		}
	}

	private void UpdateDefaultSettingVisibility()
	{
		if (!IsDefault)
		{
			tpNotifications.Enabled = !TaskSettings.UseDefaultGeneralSettings;
			Panel panel = pImage;
			TabPage tabPage = tpEffects;
			bool flag2 = (tpThumbnail.Enabled = !TaskSettings.UseDefaultImageSettings);
			bool enabled = (tabPage.Enabled = flag2);
			panel.Enabled = enabled;
			Panel panel2 = pCapture;
			TabPage tabPage2 = tpRegionCapture;
			TabPage tabPage3 = tpScreenRecorder;
			bool flag5 = (tpOCR.Enabled = !TaskSettings.UseDefaultCaptureSettings);
			flag2 = (tabPage3.Enabled = flag5);
			enabled = (tabPage2.Enabled = flag2);
			panel2.Enabled = enabled;
			pActions.Enabled = !TaskSettings.UseDefaultActions;
			TabPage tabPage4 = tpFileNaming;
			TabPage tabPage5 = tpUploadClipboard;
			flag2 = (tpUploaderFilters.Enabled = !TaskSettings.UseDefaultUploadSettings);
			enabled = (tabPage5.Enabled = flag2);
			tabPage4.Enabled = enabled;
			pTools.Enabled = !TaskSettings.UseDefaultToolsSettings;
			pgTaskSettings.Enabled = !TaskSettings.UseDefaultAdvancedSettings;
		}
	}

	private void UpdateDestinationStates()
	{
		if (Program.UploadersConfig != null)
		{
			EnableDisableToolStripMenuItems<ImageDestination>(new ToolStripDropDownItem[1] { tsmiImageUploaders });
			EnableDisableToolStripMenuItems<FileDestination>(new ToolStripDropDownItem[1] { tsmiImageFileUploaders });
			EnableDisableToolStripMenuItems<TextDestination>(new ToolStripDropDownItem[1] { tsmiTextUploaders });
			EnableDisableToolStripMenuItems<FileDestination>(new ToolStripDropDownItem[1] { tsmiTextFileUploaders });
			EnableDisableToolStripMenuItems<FileDestination>(new ToolStripDropDownItem[1] { tsmiFileUploaders });
			EnableDisableToolStripMenuItems<UrlShortenerType>(new ToolStripDropDownItem[1] { tsmiURLShorteners });
			EnableDisableToolStripMenuItems<URLSharingServices>(new ToolStripDropDownItem[1] { tsmiURLSharingServices });
		}
	}

	private void AddEnumItemsContextMenu<T>(Action<T> selectedEnum, params ToolStripDropDown[] parents) where T : Enum
	{
		EnumInfo[] array = (from x in Helpers.GetEnums<T>().OfType<Enum>()
			select new EnumInfo(x)).ToArray();
		ToolStripDropDown[] array2 = parents;
		foreach (ToolStripDropDown toolStripDropDown in array2)
		{
			EnumInfo[] array3 = array;
			foreach (EnumInfo enumInfo in array3)
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(enumInfo.Description.Replace("&", "&&"));
				toolStripMenuItem.Image = TaskHelpers.FindMenuIcon(enumInfo.Value);
				toolStripMenuItem.Tag = enumInfo;
				toolStripMenuItem.Click += delegate
				{
					SetEnumCheckedContextMenu(enumInfo, parents);
					selectedEnum((T)enumInfo.Value);
					UpdateTaskTabMenuNames();
				};
				if (!string.IsNullOrEmpty(enumInfo.Category))
				{
					ToolStripMenuItem toolStripMenuItem2 = toolStripDropDown.Items.OfType<ToolStripMenuItem>().FirstOrDefault((ToolStripMenuItem x) => x.Text == enumInfo.Category);
					if (toolStripMenuItem2 == null)
					{
						toolStripMenuItem2 = new ToolStripMenuItem(enumInfo.Category);
						toolStripDropDown.Items.Add(toolStripMenuItem2);
					}
					toolStripMenuItem2.DropDownItems.Add(toolStripMenuItem);
				}
				else
				{
					toolStripDropDown.Items.Add(toolStripMenuItem);
				}
			}
		}
	}

	private void SetEnumCheckedContextMenu(Enum value, params ToolStripDropDown[] parents)
	{
		SetEnumCheckedContextMenu(new EnumInfo(value), parents);
	}

	private void SetEnumCheckedContextMenu(EnumInfo enumInfo, params ToolStripDropDown[] parents)
	{
		for (int i = 0; i < parents.Length; i++)
		{
			foreach (ToolStripMenuItem item in parents[i].Items)
			{
				if (item.DropDownItems.Count > 0)
				{
					foreach (ToolStripMenuItem dropDownItem in item.DropDownItems)
					{
						EnumInfo enumInfo2 = (EnumInfo)dropDownItem.Tag;
						dropDownItem.Checked = enumInfo2.Value.Equals(enumInfo.Value);
					}
				}
				else
				{
					EnumInfo enumInfo2 = (EnumInfo)item.Tag;
					item.Checked = enumInfo2.Value.Equals(enumInfo.Value);
				}
			}
		}
	}

	private void AddMultiEnumItemsContextMenu<T>(Action<T> selectedEnum, params ToolStripDropDown[] parents) where T : Enum
	{
		string[] array = (from x in Helpers.GetLocalizedEnumDescriptions<T>().Skip(1)
			select x.Replace("&", "&&")).ToArray();
		ToolStripDropDown[] array2 = parents;
		foreach (ToolStripDropDown toolStripDropDown in array2)
		{
			for (int j = 0; j < array.Length; j++)
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(array[j]);
				toolStripMenuItem.Image = TaskHelpers.FindMenuIcon<T>(j + 1);
				int index = j;
				toolStripMenuItem.Click += delegate
				{
					ToolStripDropDown[] array3 = parents;
					for (int k = 0; k < array3.Length; k++)
					{
						ToolStripMenuItem obj = (ToolStripMenuItem)array3[k].Items[index];
						obj.Checked = !obj.Checked;
					}
					selectedEnum((T)Enum.ToObject(typeof(T), 1 << index));
					UpdateTaskTabMenuNames();
				};
				toolStripDropDown.Items.Add(toolStripMenuItem);
			}
		}
	}

	private void SetMultiEnumCheckedContextMenu(Enum value, params ToolStripDropDown[] parents)
	{
		for (int i = 0; i < parents[0].Items.Count; i++)
		{
			for (int j = 0; j < parents.Length; j++)
			{
				((ToolStripMenuItem)parents[j].Items[i]).Checked = value.HasFlag<int>(1 << i);
			}
		}
	}

	private void AddEnumItems<T>(Action<T> selectedEnum, params ToolStripDropDownItem[] parents)
	{
		string[] enums = Helpers.GetLocalizedEnumDescriptions<T>();
		ToolStripDropDownItem[] array = parents;
		foreach (ToolStripDropDownItem toolStripDropDownItem in array)
		{
			for (int j = 0; j < enums.Length; j++)
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(enums[j]);
				int index = j;
				toolStripMenuItem.Click += delegate
				{
					ToolStripDropDownItem[] array2 = parents;
					foreach (ToolStripDropDownItem toolStripDropDownItem2 in array2)
					{
						for (int l = 0; l < enums.Length; l++)
						{
							((ToolStripMenuItem)toolStripDropDownItem2.DropDownItems[l]).Checked = index == l;
						}
					}
					selectedEnum((T)Enum.ToObject(typeof(T), index));
					UpdateTaskTabMenuNames();
				};
				toolStripDropDownItem.DropDownItems.Add(toolStripMenuItem);
			}
		}
	}

	private void SetEnumChecked(Enum value, params ToolStripDropDownItem[] parents)
	{
		int index = value.GetIndex();
		for (int i = 0; i < parents.Length; i++)
		{
			((ToolStripMenuItem)parents[i].DropDownItems[index]).Checked = true;
		}
	}

	private void EnableDisableToolStripMenuItems<T>(params ToolStripDropDownItem[] parents)
	{
		foreach (ToolStripDropDownItem toolStripDropDownItem in parents)
		{
			for (int j = 0; j < toolStripDropDownItem.DropDownItems.Count; j++)
			{
				toolStripDropDownItem.DropDownItems[j].Enabled = UploadersConfigValidator.Validate<T>(j, Program.UploadersConfig);
			}
		}
	}

	private void UpdateTaskTabMenuNames()
	{
		btnTask.Text = TaskSettings.Job.GetLocalizedDescription();
		btnTask.Image = TaskHelpers.FindMenuIcon(TaskSettings.Job);
		btnAfterCapture.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_After_capture___0_, string.Join(", ", from x in TaskSettings.AfterCaptureJob.GetFlags()
			select x.GetLocalizedDescription()));
		btnAfterUpload.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_After_upload___0_, string.Join(", ", from x in TaskSettings.AfterUploadJob.GetFlags()
			select x.GetLocalizedDescription()));
		string arg = ((TaskSettings.ImageDestination == ImageDestination.FileUploader) ? TaskSettings.ImageFileDestination.GetLocalizedDescription() : TaskSettings.ImageDestination.GetLocalizedDescription());
		tsmiImageUploaders.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_Image_uploader___0_, arg);
		string arg2 = ((TaskSettings.TextDestination == TextDestination.FileUploader) ? TaskSettings.TextFileDestination.GetLocalizedDescription() : TaskSettings.TextDestination.GetLocalizedDescription());
		tsmiTextUploaders.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_Text_uploader___0_, arg2);
		tsmiFileUploaders.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_File_uploader___0_, TaskSettings.FileDestination.GetLocalizedDescription());
		tsmiURLShorteners.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_URL_shortener___0_, TaskSettings.URLShortenerDestination.GetLocalizedDescription());
		tsmiURLSharingServices.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_URL_sharing_service___0_, TaskSettings.URLSharingServiceDestination.GetLocalizedDescription());
	}

	private void tbDescription_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.Description = tbDescription.Text;
		UpdateWindowTitle();
	}

	private void cbUseDefaultAfterCaptureSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultAfterCaptureJob = !cbOverrideAfterCaptureSettings.Checked;
		btnAfterCapture.Enabled = !TaskSettings.UseDefaultAfterCaptureJob;
	}

	private void cbUseDefaultAfterUploadSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultAfterUploadJob = !cbOverrideAfterUploadSettings.Checked;
		btnAfterUpload.Enabled = !TaskSettings.UseDefaultAfterUploadJob;
	}

	private void cbUseDefaultDestinationSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultDestinations = !cbOverrideDestinationSettings.Checked;
		btnDestinations.Enabled = !TaskSettings.UseDefaultDestinations;
	}

	private void cbOverrideFTPAccount_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.OverrideFTP = cbOverrideFTPAccount.Checked;
		cbFTPAccounts.Enabled = TaskSettings.OverrideFTP;
	}

	private void cbFTPAccounts_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.FTPIndex = cbFTPAccounts.SelectedIndex;
	}

	private void cbOverrideCustomUploader_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.OverrideCustomUploader = cbOverrideCustomUploader.Checked;
		cbCustomUploaders.Enabled = TaskSettings.OverrideCustomUploader;
	}

	private void cbCustomUploaders_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.CustomUploaderIndex = cbCustomUploaders.SelectedIndex;
	}

	private void cbOverrideScreenshotsFolder_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.OverrideScreenshotsFolder = cbOverrideScreenshotsFolder.Checked;
		TextBox textBox = txtScreenshotsFolder;
		bool enabled = (btnScreenshotsFolderBrowse.Enabled = TaskSettings.OverrideScreenshotsFolder);
		textBox.Enabled = enabled;
	}

	private void txtScreenshotsFolder_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.ScreenshotsFolder = txtScreenshotsFolder.Text;
	}

	private void btnScreenshotsFolderBrowse_Click(object sender, EventArgs e)
	{
		FileHelpers.BrowseFolder(Resources.ApplicationSettingsForm_btnBrowseCustomScreenshotsPath_Click_Choose_screenshots_folder_path, txtScreenshotsFolder, TaskSettings.ScreenshotsFolder, detectSpecialFolders: true);
	}

	private void cbUseDefaultGeneralSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultGeneralSettings = !cbOverrideGeneralSettings.Checked;
		UpdateDefaultSettingVisibility();
	}

	private void cbPlaySoundAfterCapture_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.PlaySoundAfterCapture = cbPlaySoundAfterCapture.Checked;
	}

	private void cbPlaySoundAfterUpload_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.PlaySoundAfterUpload = cbPlaySoundAfterUpload.Checked;
	}

	private void cbShowToastNotificationAfterTaskCompleted_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ShowToastNotificationAfterTaskCompleted = cbShowToastNotificationAfterTaskCompleted.Checked;
		gbToastWindow.Enabled = TaskSettings.GeneralSettings.ShowToastNotificationAfterTaskCompleted;
	}

	private void nudToastWindowDuration_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowDuration = (float)nudToastWindowDuration.Value;
	}

	private void nudToastWindowFadeDuration_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowFadeDuration = (float)nudToastWindowFadeDuration.Value;
	}

	private void cbToastWindowPlacement_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowPlacement = Helpers.GetEnumFromIndex<ContentAlignment>(cbToastWindowPlacement.SelectedIndex);
	}

	private void nudToastWindowSizeWidth_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowSize = new Size((int)nudToastWindowSizeWidth.Value, TaskSettings.GeneralSettings.ToastWindowSize.Height);
	}

	private void nudToastWindowSizeHeight_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowSize = new Size(TaskSettings.GeneralSettings.ToastWindowSize.Width, (int)nudToastWindowSizeHeight.Value);
	}

	private void cbToastWindowLeftClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowLeftClickAction = (ToastClickAction)cbToastWindowLeftClickAction.SelectedIndex;
	}

	private void cbToastWindowRightClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowRightClickAction = (ToastClickAction)cbToastWindowRightClickAction.SelectedIndex;
	}

	private void cbToastWindowMiddleClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowMiddleClickAction = (ToastClickAction)cbToastWindowMiddleClickAction.SelectedIndex;
	}

	private void cbToastWindowAutoHide_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.ToastWindowAutoHide = cbToastWindowAutoHide.Checked;
	}

	private void cbUseCustomCaptureSound_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.UseCustomCaptureSound = cbUseCustomCaptureSound.Checked;
		TextBox textBox = txtCustomCaptureSoundPath;
		bool enabled = (btnCustomCaptureSoundPath.Enabled = TaskSettings.GeneralSettings.UseCustomCaptureSound);
		textBox.Enabled = enabled;
	}

	private void txtCustomCaptureSoundPath_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.CustomCaptureSoundPath = txtCustomCaptureSoundPath.Text;
	}

	private void btnCustomCaptureSoundPath_Click(object sender, EventArgs e)
	{
		FileHelpers.BrowseFile(txtCustomCaptureSoundPath);
	}

	private void cbUseCustomTaskCompletedSound_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.UseCustomTaskCompletedSound = cbUseCustomTaskCompletedSound.Checked;
		TextBox textBox = txtCustomTaskCompletedSoundPath;
		bool enabled = (btnCustomTaskCompletedSoundPath.Enabled = TaskSettings.GeneralSettings.UseCustomTaskCompletedSound);
		textBox.Enabled = enabled;
	}

	private void txtCustomTaskCompletedSoundPath_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.CustomTaskCompletedSoundPath = txtCustomTaskCompletedSoundPath.Text;
	}

	private void btnCustomTaskCompletedSoundPath_Click(object sender, EventArgs e)
	{
		FileHelpers.BrowseFile(txtCustomTaskCompletedSoundPath);
	}

	private void cbUseCustomErrorSound_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.UseCustomErrorSound = cbUseCustomErrorSound.Checked;
		TextBox textBox = txtCustomErrorSoundPath;
		bool enabled = (btnCustomErrorSoundPath.Enabled = TaskSettings.GeneralSettings.UseCustomErrorSound);
		textBox.Enabled = enabled;
	}

	private void txtCustomErrorSoundPath_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.CustomErrorSoundPath = txtCustomErrorSoundPath.Text;
	}

	private void btnCustomErrorSoundPath_Click(object sender, EventArgs e)
	{
		FileHelpers.BrowseFile(txtCustomErrorSoundPath);
	}

	private void cbDisableNotifications_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.DisableNotifications = cbDisableNotifications.Checked;
	}

	private void cbDisableNotificationsOnFullscreen_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.GeneralSettings.DisableNotificationsOnFullscreen = cbDisableNotificationsOnFullscreen.Checked;
	}

	private void cbUseDefaultImageSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultImageSettings = !cbOverrideImageSettings.Checked;
		UpdateDefaultSettingVisibility();
	}

	private void cbImageFormat_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ImageFormat = (EImageFormat)cbImageFormat.SelectedIndex;
	}

	private void cbImagePNGBitDepth_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ImagePNGBitDepth = (PNGBitDepth)cbImagePNGBitDepth.SelectedIndex;
	}

	private void nudImageJPEGQuality_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ImageJPEGQuality = (int)nudImageJPEGQuality.Value;
	}

	private void cbImageGIFQuality_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ImageGIFQuality = (GIFQuality)cbImageGIFQuality.SelectedIndex;
	}

	private void cbImageAutoUseJPEG_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ImageAutoUseJPEG = cbImageAutoUseJPEG.Checked;
		nudImageAutoUseJPEGSize.Enabled = TaskSettings.ImageSettings.ImageAutoUseJPEG;
		cbImageAutoJPEGQuality.Enabled = TaskSettings.ImageSettings.ImageAutoUseJPEG;
	}

	private void nudImageAutoUseJPEGSize_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ImageAutoUseJPEGSize = (int)nudImageAutoUseJPEGSize.Value;
	}

	private void cbImageAutoJPEGQuality_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ImageAutoJPEGQuality = cbImageAutoJPEGQuality.Checked;
	}

	private void cbImageFileExist_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.FileExistAction = (FileExistAction)cbImageFileExist.SelectedIndex;
	}

	private void cbImageEffectOnlyRegionCapture_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ImageEffectOnlyRegionCapture = cbImageEffectOnlyRegionCapture.Checked;
	}

	private void cbShowImageEffectsWindowAfterCapture_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ShowImageEffectsWindowAfterCapture = cbShowImageEffectsWindowAfterCapture.Checked;
	}

	private void btnImageEffects_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenImageEffectsSingleton(TaskSettings);
	}

	private void nudThumbnailWidth_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ThumbnailWidth = (int)nudThumbnailWidth.Value;
	}

	private void nudThumbnailHeight_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ThumbnailHeight = (int)nudThumbnailHeight.Value;
	}

	private void txtThumbnailName_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ThumbnailName = txtThumbnailName.Text;
		lblThumbnailNamePreview.Text = "ImageName" + TaskSettings.ImageSettings.ThumbnailName + ".jpg";
	}

	private void cbThumbnailIfSmaller_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.ImageSettings.ThumbnailCheckSize = cbThumbnailIfSmaller.Checked;
	}

	private void cbUseDefaultCaptureSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultCaptureSettings = !cbOverrideCaptureSettings.Checked;
		UpdateDefaultSettingVisibility();
	}

	private void cbShowCursor_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ShowCursor = cbShowCursor.Checked;
	}

	private void nudScreenshotDelay_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenshotDelay = nudScreenshotDelay.Value;
	}

	private void cbCaptureTransparent_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureTransparent = cbCaptureTransparent.Checked;
		cbCaptureShadow.Enabled = TaskSettings.CaptureSettings.CaptureTransparent;
	}

	private void cbCaptureShadow_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureShadow = cbCaptureShadow.Checked;
	}

	private void nudCaptureShadowOffset_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureShadowOffset = (int)nudCaptureShadowOffset.Value;
	}

	private void cbCaptureClientArea_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureClientArea = cbCaptureClientArea.Checked;
	}

	private void cbCaptureAutoHideTaskbar_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureAutoHideTaskbar = cbCaptureAutoHideTaskbar.Checked;
	}

	private void nudScreenRegionX_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureCustomRegion.X = (int)nudCaptureCustomRegionX.Value;
	}

	private void nudScreenRegionY_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureCustomRegion.Y = (int)nudCaptureCustomRegionY.Value;
	}

	private void nudScreenRegionWidth_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureCustomRegion.Width = (int)nudCaptureCustomRegionWidth.Value;
	}

	private void nudScreenRegionHeight_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.CaptureCustomRegion.Height = (int)nudCaptureCustomRegionHeight.Value;
	}

	private void btnCaptureCustomRegionSelectRectangle_Click(object sender, EventArgs e)
	{
		if (RegionCaptureTasks.GetRectangleRegion(out var rect, TaskSettings.CaptureSettings.SurfaceOptions))
		{
			nudCaptureCustomRegionX.SetValue(rect.X);
			nudCaptureCustomRegionY.SetValue(rect.Y);
			nudCaptureCustomRegionWidth.SetValue(rect.Width);
			nudCaptureCustomRegionHeight.SetValue(rect.Height);
		}
	}

	private void cbRegionCaptureMultiRegionMode_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.QuickCrop = !cbRegionCaptureMultiRegionMode.Checked;
	}

	private void cbRegionCaptureMouseRightClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.RegionCaptureActionRightClick = (RegionCaptureAction)cbRegionCaptureMouseRightClickAction.SelectedIndex;
	}

	private void cbRegionCaptureMouseMiddleClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.RegionCaptureActionMiddleClick = (RegionCaptureAction)cbRegionCaptureMouseMiddleClickAction.SelectedIndex;
	}

	private void cbRegionCaptureMouse4ClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.RegionCaptureActionX1Click = (RegionCaptureAction)cbRegionCaptureMouse4ClickAction.SelectedIndex;
	}

	private void cbRegionCaptureMouse5ClickAction_SelectedIndexChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.RegionCaptureActionX2Click = (RegionCaptureAction)cbRegionCaptureMouse5ClickAction.SelectedIndex;
	}

	private void cbRegionCaptureDetectWindows_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.DetectWindows = cbRegionCaptureDetectWindows.Checked;
		cbRegionCaptureDetectControls.Enabled = TaskSettings.CaptureSettings.SurfaceOptions.DetectWindows;
	}

	private void cbRegionCaptureDetectControls_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.DetectControls = cbRegionCaptureDetectControls.Checked;
	}

	private void cbRegionCaptureUseDimming_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.UseDimming = cbRegionCaptureUseDimming.Checked;
	}

	private void cbRegionCaptureUseCustomInfoText_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.UseCustomInfoText = cbRegionCaptureUseCustomInfoText.Checked;
		txtRegionCaptureCustomInfoText.Enabled = TaskSettings.CaptureSettings.SurfaceOptions.UseCustomInfoText;
	}

	private void txtRegionCaptureCustomInfoText_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.CustomInfoText = txtRegionCaptureCustomInfoText.Text;
	}

	private void btnRegionCaptureSnapSizesAdd_Click(object sender, EventArgs e)
	{
		pRegionCaptureSnapSizes.Visible = true;
	}

	private void btnRegionCaptureSnapSizesRemove_Click(object sender, EventArgs e)
	{
		int selectedIndex = cbRegionCaptureSnapSizes.SelectedIndex;
		if (selectedIndex > -1)
		{
			TaskSettings.CaptureSettings.SurfaceOptions.SnapSizes.RemoveAt(selectedIndex);
			cbRegionCaptureSnapSizes.Items.RemoveAt(selectedIndex);
			cbRegionCaptureSnapSizes.SelectedIndex = cbRegionCaptureSnapSizes.Items.Count - 1;
		}
	}

	private void btnRegionCaptureSnapSizesDialogAdd_Click(object sender, EventArgs e)
	{
		pRegionCaptureSnapSizes.Visible = false;
		SnapSize item = new SnapSize((int)nudRegionCaptureSnapSizesWidth.Value, (int)nudRegionCaptureSnapSizesHeight.Value);
		TaskSettings.CaptureSettings.SurfaceOptions.SnapSizes.Add(item);
		cbRegionCaptureSnapSizes.Items.Add(item);
		cbRegionCaptureSnapSizes.SelectedIndex = cbRegionCaptureSnapSizes.Items.Count - 1;
	}

	private void btnRegionCaptureSnapSizesDialogCancel_Click(object sender, EventArgs e)
	{
		pRegionCaptureSnapSizes.Visible = false;
	}

	private void cbRegionCaptureShowInfo_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.ShowInfo = cbRegionCaptureShowInfo.Checked;
	}

	private void cbRegionCaptureShowMagnifier_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.ShowMagnifier = cbRegionCaptureShowMagnifier.Checked;
		CheckBox checkBox = cbRegionCaptureUseSquareMagnifier;
		NumericUpDown numericUpDown = nudRegionCaptureMagnifierPixelCount;
		bool flag = (nudRegionCaptureMagnifierPixelSize.Enabled = TaskSettings.CaptureSettings.SurfaceOptions.ShowMagnifier);
		bool enabled = (numericUpDown.Enabled = flag);
		checkBox.Enabled = enabled;
	}

	private void cbRegionCaptureUseSquareMagnifier_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.UseSquareMagnifier = cbRegionCaptureUseSquareMagnifier.Checked;
	}

	private void nudRegionCaptureMagnifierPixelCount_ValueChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			TaskSettings.CaptureSettings.SurfaceOptions.MagnifierPixelCount = (int)nudRegionCaptureMagnifierPixelCount.Value;
		}
	}

	private void nudRegionCaptureMagnifierPixelSize_ValueChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			TaskSettings.CaptureSettings.SurfaceOptions.MagnifierPixelSize = (int)nudRegionCaptureMagnifierPixelSize.Value;
		}
	}

	private void cbRegionCaptureShowCrosshair_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.ShowCrosshair = cbRegionCaptureShowCrosshair.Checked;
	}

	private void cbRegionCaptureIsFixedSize_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.IsFixedSize = cbRegionCaptureIsFixedSize.Checked;
		NumericUpDown numericUpDown = nudRegionCaptureFixedSizeWidth;
		bool enabled = (nudRegionCaptureFixedSizeHeight.Enabled = TaskSettings.CaptureSettings.SurfaceOptions.IsFixedSize);
		numericUpDown.Enabled = enabled;
	}

	private void nudRegionCaptureFixedSizeWidth_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.FixedSize = new Size((int)nudRegionCaptureFixedSizeWidth.Value, TaskSettings.CaptureSettings.SurfaceOptions.FixedSize.Height);
	}

	private void nudRegionCaptureFixedSizeHeight_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.FixedSize = new Size(TaskSettings.CaptureSettings.SurfaceOptions.FixedSize.Width, (int)nudRegionCaptureFixedSizeHeight.Value);
	}

	private void cbRegionCaptureShowFPS_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.ShowFPS = cbRegionCaptureShowFPS.Checked;
	}

	private void nudRegionCaptureFPSLimit_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.FPSLimit = (int)nudRegionCaptureFPSLimit.Value;
	}

	private void cbRegionCaptureActiveMonitorMode_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.SurfaceOptions.ActiveMonitorMode = cbRegionCaptureActiveMonitorMode.Checked;
	}

	private void btnScreenRecorderFFmpegOptions_Click(object sender, EventArgs e)
	{
		using FFmpegOptionsForm fFmpegOptionsForm = new FFmpegOptionsForm(new ScreenRecordingOptions
		{
			IsRecording = true,
			FFmpeg = TaskSettings.CaptureSettings.FFmpegOptions,
			FPS = TaskSettings.CaptureSettings.ScreenRecordFPS,
			Duration = (TaskSettings.CaptureSettings.ScreenRecordFixedDuration ? TaskSettings.CaptureSettings.ScreenRecordDuration : 0f),
			OutputPath = "output.mp4",
			CaptureArea = Screen.PrimaryScreen.Bounds,
			DrawCursor = TaskSettings.CaptureSettings.ScreenRecordShowCursor
		});
		fFmpegOptionsForm.DefaultToolsFolder = Program.ToolsFolder;
		fFmpegOptionsForm.ShowDialog();
		TaskSettings.CaptureSettings.FFmpegOptions = fFmpegOptionsForm.Options.FFmpeg;
	}

	private void nudScreenRecordFPS_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordFPS = (int)nudScreenRecordFPS.Value;
	}

	private void nudGIFFPS_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.GIFFPS = (int)nudGIFFPS.Value;
	}

	private void cbScreenRecorderFixedDuration_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordFixedDuration = cbScreenRecorderFixedDuration.Checked;
		nudScreenRecorderDuration.Enabled = TaskSettings.CaptureSettings.ScreenRecordFixedDuration;
	}

	private void nudScreenRecorderDuration_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordDuration = (float)nudScreenRecorderDuration.Value;
	}

	private void cbScreenRecordAutoStart_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordAutoStart = cbScreenRecordAutoStart.Checked;
		nudScreenRecorderStartDelay.Enabled = cbScreenRecordAutoStart.Checked;
	}

	private void nudScreenRecorderStartDelay_ValueChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordStartDelay = (float)nudScreenRecorderStartDelay.Value;
	}

	private void cbScreenRecorderShowCursor_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordShowCursor = cbScreenRecorderShowCursor.Checked;
	}

	private void cbScreenRecordTwoPassEncoding_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordTwoPassEncoding = cbScreenRecordTwoPassEncoding.Checked;
	}

	private void cbScreenRecordTransparentRegion_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordTransparentRegion = cbScreenRecordTransparentRegion.Checked;
	}

	private void cbScreenRecordConfirmAbort_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.ScreenRecordAskConfirmationOnAbort = cbScreenRecordConfirmAbort.Checked;
	}

	private void cbCaptureOCRDefaultLanguage_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (loaded)
		{
			TaskSettings.CaptureSettings.OCROptions.Language = ((OCRLanguage)cbCaptureOCRDefaultLanguage.SelectedItem).LanguageTag;
		}
	}

	private void btnCaptureOCRHelp_Click(object sender, EventArgs e)
	{
		URLHelpers.OpenURL("https://getsharex.com/docs/ocr");
	}

	private void cbCaptureOCRSilent_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.OCROptions.Silent = cbCaptureOCRSilent.Checked;
		cbCaptureOCRAutoCopy.Enabled = !TaskSettings.CaptureSettings.OCROptions.Silent;
	}

	private void cbCaptureOCRAutoCopy_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.CaptureSettings.OCROptions.AutoCopy = cbCaptureOCRAutoCopy.Checked;
	}

	private void UpdateNameFormatPreviews()
	{
		NameParser nameParser = new NameParser(NameParserType.FileName)
		{
			AutoIncrementNumber = Program.Settings.NameParserAutoIncrementNumber,
			WindowText = Text,
			ProcessName = "ShareX",
			ImageWidth = 1920,
			ImageHeight = 1080,
			MaxNameLength = TaskSettings.AdvancedSettings.NamePatternMaxLength,
			MaxTitleLength = TaskSettings.AdvancedSettings.NamePatternMaxTitleLength,
			CustomTimeZone = (TaskSettings.UploadSettings.UseCustomTimeZone ? TaskSettings.UploadSettings.CustomTimeZone : null),
			IsPreviewMode = true
		};
		lblNameFormatPatternPreview.Text = Resources.TaskSettingsForm_txtNameFormatPatternActiveWindow_TextChanged_Preview_ + " " + nameParser.Parse(TaskSettings.UploadSettings.NameFormatPattern);
		lblNameFormatPatternPreviewActiveWindow.Text = Resources.TaskSettingsForm_txtNameFormatPatternActiveWindow_TextChanged_Preview_ + " " + nameParser.Parse(TaskSettings.UploadSettings.NameFormatPatternActiveWindow);
	}

	private void cbUseDefaultUploadSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultUploadSettings = !cbOverrideUploadSettings.Checked;
		UpdateDefaultSettingVisibility();
	}

	private void txtNameFormatPattern_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.NameFormatPattern = txtNameFormatPattern.Text;
		UpdateNameFormatPreviews();
	}

	private void txtNameFormatPatternActiveWindow_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.NameFormatPatternActiveWindow = txtNameFormatPatternActiveWindow.Text;
		UpdateNameFormatPreviews();
	}

	private void cbFileUploadUseNamePattern_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.FileUploadUseNamePattern = cbFileUploadUseNamePattern.Checked;
	}

	private void btnAutoIncrementNumber_Click(object sender, EventArgs e)
	{
		Program.Settings.NameParserAutoIncrementNumber = (int)nudAutoIncrementNumber.Value;
		UpdateNameFormatPreviews();
	}

	private void cbNameFormatCustomTimeZone_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.UseCustomTimeZone = cbNameFormatCustomTimeZone.Checked;
		cbNameFormatTimeZone.Enabled = TaskSettings.UploadSettings.UseCustomTimeZone;
		UpdateNameFormatPreviews();
	}

	private void cbNameFormatTimeZone_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cbNameFormatTimeZone.SelectedItem is TimeZoneInfo customTimeZone)
		{
			TaskSettings.UploadSettings.CustomTimeZone = customTimeZone;
		}
		UpdateNameFormatPreviews();
	}

	private void cbFileUploadReplaceProblematicCharacters_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.FileUploadReplaceProblematicCharacters = cbFileUploadReplaceProblematicCharacters.Checked;
	}

	private void cbURLRegexReplace_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.URLRegexReplace = cbURLRegexReplace.Checked;
		Label label = lblURLRegexReplacePattern;
		TextBox textBox = txtURLRegexReplacePattern;
		Label label2 = lblURLRegexReplaceReplacement;
		bool flag = (txtURLRegexReplaceReplacement.Enabled = TaskSettings.UploadSettings.URLRegexReplace);
		bool flag3 = (label2.Enabled = flag);
		bool enabled = (textBox.Enabled = flag3);
		label.Enabled = enabled;
	}

	private void txtURLRegexReplacePattern_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.URLRegexReplacePattern = txtURLRegexReplacePattern.Text;
	}

	private void txtURLRegexReplaceReplacement_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.URLRegexReplaceReplacement = txtURLRegexReplaceReplacement.Text;
	}

	private void cbClipboardUploadContents_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.ClipboardUploadURLContents = cbClipboardUploadURLContents.Checked;
	}

	private void cbClipboardUploadAutoDetectURL_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.ClipboardUploadShortenURL = cbClipboardUploadShortenURL.Checked;
	}

	private void cbClipboardUploadShareURL_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.ClipboardUploadShareURL = cbClipboardUploadShareURL.Checked;
	}

	private void cbClipboardUploadAutoIndexFolder_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UploadSettings.ClipboardUploadAutoIndexFolder = cbClipboardUploadAutoIndexFolder.Checked;
	}

	private UploaderFilter GetUploaderFilterFromFields()
	{
		if (cbUploaderFiltersDestination.SelectedItem is IGenericUploaderService genericUploaderService)
		{
			UploaderFilter uploaderFilter = new UploaderFilter();
			uploaderFilter.Uploader = genericUploaderService.ServiceIdentifier;
			uploaderFilter.SetExtensions(txtUploaderFiltersExtensions.Text);
			return uploaderFilter;
		}
		return null;
	}

	private void AddUploaderFilterToList(UploaderFilter filter)
	{
		if (filter != null)
		{
			ListViewItem listViewItem = new ListViewItem(filter.Uploader);
			listViewItem.SubItems.Add(filter.GetExtensions());
			listViewItem.Tag = filter;
			lvUploaderFiltersList.Items.Add(listViewItem);
		}
	}

	private void UpdateUploaderFilterFields(UploaderFilter filter)
	{
		if (filter == null)
		{
			filter = new UploaderFilter();
		}
		for (int i = 0; i < cbUploaderFiltersDestination.Items.Count; i++)
		{
			if (cbUploaderFiltersDestination.Items[i] is IGenericUploaderService genericUploaderService && genericUploaderService.ServiceIdentifier.Equals(filter.Uploader, StringComparison.InvariantCultureIgnoreCase))
			{
				cbUploaderFiltersDestination.SelectedIndex = i;
				break;
			}
		}
		txtUploaderFiltersExtensions.Text = filter.GetExtensions();
	}

	private void btnUploaderFiltersAdd_Click(object sender, EventArgs e)
	{
		UploaderFilter uploaderFilterFromFields = GetUploaderFilterFromFields();
		if (uploaderFilterFromFields != null)
		{
			TaskSettings.UploadSettings.UploaderFilters.Add(uploaderFilterFromFields);
			AddUploaderFilterToList(uploaderFilterFromFields);
			lvUploaderFiltersList.SelectedIndex = lvUploaderFiltersList.Items.Count - 1;
		}
	}

	private void btnUploaderFiltersUpdate_Click(object sender, EventArgs e)
	{
		int selectedIndex = lvUploaderFiltersList.SelectedIndex;
		if (selectedIndex > -1)
		{
			UploaderFilter uploaderFilterFromFields = GetUploaderFilterFromFields();
			if (uploaderFilterFromFields != null)
			{
				TaskSettings.UploadSettings.UploaderFilters[selectedIndex] = uploaderFilterFromFields;
				ListViewItem listViewItem = lvUploaderFiltersList.Items[selectedIndex];
				listViewItem.Text = uploaderFilterFromFields.Uploader;
				listViewItem.SubItems[1].Text = uploaderFilterFromFields.GetExtensions();
				listViewItem.Tag = uploaderFilterFromFields;
			}
		}
	}

	private void btnUploaderFiltersRemove_Click(object sender, EventArgs e)
	{
		int selectedIndex = lvUploaderFiltersList.SelectedIndex;
		if (selectedIndex > -1)
		{
			TaskSettings.UploadSettings.UploaderFilters.RemoveAt(selectedIndex);
			lvUploaderFiltersList.Items.RemoveAt(selectedIndex);
		}
	}

	private void lvUploaderFiltersList_SelectedIndexChanged(object sender, EventArgs e)
	{
		UploaderFilter filter = null;
		if (lvUploaderFiltersList.SelectedItems.Count > 0)
		{
			filter = lvUploaderFiltersList.SelectedItems[0].Tag as UploaderFilter;
		}
		UpdateUploaderFilterFields(filter);
	}

	private void cbUseDefaultActions_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultActions = !cbOverrideActions.Checked;
		UpdateDefaultSettingVisibility();
	}

	private void btnActionsAdd_Click(object sender, EventArgs e)
	{
		using ActionsForm actionsForm = new ActionsForm();
		if (actionsForm.ShowDialog() == DialogResult.OK)
		{
			ExternalProgram fileAction = actionsForm.FileAction;
			fileAction.IsActive = true;
			TaskSettings.ExternalPrograms.Add(fileAction);
			AddFileAction(fileAction);
		}
	}

	private void AddFileAction(ExternalProgram fileAction)
	{
		ListViewItem listViewItem = new ListViewItem(fileAction.Name ?? "");
		listViewItem.Tag = fileAction;
		listViewItem.Checked = fileAction.IsActive;
		listViewItem.SubItems.Add(fileAction.Path ?? "");
		listViewItem.SubItems.Add(fileAction.Args ?? "");
		listViewItem.SubItems.Add(fileAction.Extensions ?? "");
		lvActions.Items.Add(listViewItem);
	}

	private void btnActionsEdit_Click(object sender, EventArgs e)
	{
		if (lvActions.SelectedItems.Count <= 0)
		{
			return;
		}
		ListViewItem listViewItem = lvActions.SelectedItems[0];
		ExternalProgram externalProgram = listViewItem.Tag as ExternalProgram;
		using ActionsForm actionsForm = new ActionsForm(externalProgram);
		if (actionsForm.ShowDialog() == DialogResult.OK)
		{
			listViewItem.Text = externalProgram.Name ?? "";
			listViewItem.SubItems[1].Text = externalProgram.Path ?? "";
			listViewItem.SubItems[2].Text = externalProgram.Args ?? "";
			listViewItem.SubItems[3].Text = externalProgram.Extensions ?? "";
		}
	}

	private void btnActionsDuplicate_Click(object sender, EventArgs e)
	{
		foreach (ExternalProgram item in from ListViewItem x in lvActions.SelectedItems
			select ((ExternalProgram)x.Tag).Copy())
		{
			TaskSettings.ExternalPrograms.Add(item);
			AddFileAction(item);
		}
	}

	private void btnActionsRemove_Click(object sender, EventArgs e)
	{
		if (lvActions.SelectedItems.Count > 0)
		{
			ListViewItem listViewItem = lvActions.SelectedItems[0];
			ExternalProgram item = listViewItem.Tag as ExternalProgram;
			TaskSettings.ExternalPrograms.Remove(item);
			lvActions.Items.Remove(listViewItem);
		}
	}

	private void btnActions_Click(object sender, EventArgs e)
	{
		URLHelpers.OpenURL("https://getsharex.com/actions");
	}

	private void lvActions_SelectedIndexChanged(object sender, EventArgs e)
	{
		Button button = btnActionsEdit;
		Button button2 = btnActionsDuplicate;
		bool flag2 = (btnActionsRemove.Enabled = lvActions.SelectedItems.Count > 0);
		bool enabled = (button2.Enabled = flag2);
		button.Enabled = enabled;
	}

	private void lvActions_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		(e.Item.Tag as ExternalProgram).IsActive = e.Item.Checked;
	}

	private void lvActions_ItemMoved(object sender, int oldIndex, int newIndex)
	{
		TaskSettings.ExternalPrograms.Move(oldIndex, newIndex);
	}

	private void WatchFolderAdd(WatchFolderSettings watchFolderSetting)
	{
		if (Program.WatchFolderManager != null && watchFolderSetting != null)
		{
			Program.WatchFolderManager.AddWatchFolder(watchFolderSetting, TaskSettings);
			ListViewItem listViewItem = new ListViewItem(watchFolderSetting.FolderPath ?? "");
			listViewItem.Tag = watchFolderSetting;
			listViewItem.SubItems.Add(watchFolderSetting.Filter ?? "");
			listViewItem.SubItems.Add(watchFolderSetting.IncludeSubdirectories.ToString());
			lvWatchFolderList.Items.Add(listViewItem);
		}
	}

	private void WatchFolderEditSelected()
	{
		if (lvWatchFolderList.SelectedItems.Count <= 0)
		{
			return;
		}
		ListViewItem listViewItem = lvWatchFolderList.SelectedItems[0];
		WatchFolderSettings watchFolderSettings = listViewItem.Tag as WatchFolderSettings;
		using WatchFolderForm watchFolderForm = new WatchFolderForm(watchFolderSettings);
		if (watchFolderForm.ShowDialog() == DialogResult.OK)
		{
			listViewItem.Text = watchFolderSettings.FolderPath ?? "";
			listViewItem.SubItems[1].Text = watchFolderSettings.Filter ?? "";
			listViewItem.SubItems[2].Text = watchFolderSettings.IncludeSubdirectories.ToString();
			Program.WatchFolderManager.UpdateWatchFolderState(watchFolderSettings);
		}
	}

	private void cbWatchFolderEnabled_CheckedChanged(object sender, EventArgs e)
	{
		if (!loaded)
		{
			return;
		}
		TaskSettings.WatchFolderEnabled = cbWatchFolderEnabled.Checked;
		foreach (WatchFolderSettings watchFolder in TaskSettings.WatchFolderList)
		{
			Program.WatchFolderManager.UpdateWatchFolderState(watchFolder);
		}
	}

	private void btnWatchFolderAdd_Click(object sender, EventArgs e)
	{
		using WatchFolderForm watchFolderForm = new WatchFolderForm();
		if (watchFolderForm.ShowDialog() == DialogResult.OK)
		{
			WatchFolderAdd(watchFolderForm.WatchFolder);
		}
	}

	private void btnWatchFolderEdit_Click(object sender, EventArgs e)
	{
		WatchFolderEditSelected();
	}

	private void btnWatchFolderRemove_Click(object sender, EventArgs e)
	{
		if (lvWatchFolderList.SelectedItems.Count > 0)
		{
			ListViewItem listViewItem = lvWatchFolderList.SelectedItems[0];
			WatchFolderSettings watchFolderSetting = listViewItem.Tag as WatchFolderSettings;
			Program.WatchFolderManager.RemoveWatchFolder(watchFolderSetting);
			lvWatchFolderList.Items.Remove(listViewItem);
		}
	}

	private void lvWatchFolderList_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		WatchFolderEditSelected();
	}

	private void cbUseDefaultToolsSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultToolsSettings = !cbOverrideToolsSettings.Checked;
		UpdateDefaultSettingVisibility();
	}

	private void txtToolsScreenColorPickerFormat_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.ToolsSettings.ScreenColorPickerFormat = txtToolsScreenColorPickerFormat.Text;
	}

	private void txtToolsScreenColorPickerFormatCtrl_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.ToolsSettings.ScreenColorPickerFormatCtrl = txtToolsScreenColorPickerFormatCtrl.Text;
	}

	private void txtToolsScreenColorPickerInfoText_TextChanged(object sender, EventArgs e)
	{
		TaskSettings.ToolsSettings.ScreenColorPickerInfoText = txtToolsScreenColorPickerInfoText.Text;
	}

	private void cbUseDefaultAdvancedSettings_CheckedChanged(object sender, EventArgs e)
	{
		TaskSettings.UseDefaultAdvancedSettings = !cbOverrideAdvancedSettings.Checked;
		UpdateDefaultSettingVisibility();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.TaskSettingsForm));
		this.cmsAfterCapture = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.cmsAfterUpload = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.cbOverrideAfterCaptureSettings = new System.Windows.Forms.CheckBox();
		this.cbOverrideAfterUploadSettings = new System.Windows.Forms.CheckBox();
		this.cbOverrideDestinationSettings = new System.Windows.Forms.CheckBox();
		this.lblDescription = new System.Windows.Forms.Label();
		this.tbDescription = new System.Windows.Forms.TextBox();
		this.cmsTask = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tcTaskSettings = new System.Windows.Forms.TabControl();
		this.tpTask = new System.Windows.Forms.TabPage();
		this.lblTask = new System.Windows.Forms.Label();
		this.btnScreenshotsFolderBrowse = new System.Windows.Forms.Button();
		this.txtScreenshotsFolder = new System.Windows.Forms.TextBox();
		this.cbOverrideScreenshotsFolder = new System.Windows.Forms.CheckBox();
		this.cbCustomUploaders = new System.Windows.Forms.ComboBox();
		this.cbOverrideCustomUploader = new System.Windows.Forms.CheckBox();
		this.cbOverrideFTPAccount = new System.Windows.Forms.CheckBox();
		this.cbFTPAccounts = new System.Windows.Forms.ComboBox();
		this.btnAfterCapture = new ShareX.HelpersLib.MenuButton();
		this.btnAfterUpload = new ShareX.HelpersLib.MenuButton();
		this.btnDestinations = new ShareX.HelpersLib.MenuButton();
		this.cmsDestinations = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmiImageUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTextUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiFileUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiURLShorteners = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiURLSharingServices = new System.Windows.Forms.ToolStripMenuItem();
		this.btnTask = new ShareX.HelpersLib.MenuButton();
		this.tpGeneral = new System.Windows.Forms.TabPage();
		this.tcGeneral = new System.Windows.Forms.TabControl();
		this.tpGeneralMain = new System.Windows.Forms.TabPage();
		this.cbOverrideGeneralSettings = new System.Windows.Forms.CheckBox();
		this.tpNotifications = new System.Windows.Forms.TabPage();
		this.cbShowToastNotificationAfterTaskCompleted = new System.Windows.Forms.CheckBox();
		this.btnCustomErrorSoundPath = new System.Windows.Forms.Button();
		this.btnCustomTaskCompletedSoundPath = new System.Windows.Forms.Button();
		this.btnCustomCaptureSoundPath = new System.Windows.Forms.Button();
		this.txtCustomErrorSoundPath = new System.Windows.Forms.TextBox();
		this.txtCustomTaskCompletedSoundPath = new System.Windows.Forms.TextBox();
		this.txtCustomCaptureSoundPath = new System.Windows.Forms.TextBox();
		this.cbUseCustomErrorSound = new System.Windows.Forms.CheckBox();
		this.cbUseCustomTaskCompletedSound = new System.Windows.Forms.CheckBox();
		this.cbUseCustomCaptureSound = new System.Windows.Forms.CheckBox();
		this.gbToastWindow = new System.Windows.Forms.GroupBox();
		this.cbToastWindowAutoHide = new System.Windows.Forms.CheckBox();
		this.lblToastWindowFadeDurationSeconds = new System.Windows.Forms.Label();
		this.lblToastWindowDurationSeconds = new System.Windows.Forms.Label();
		this.lblToastWindowSizeX = new System.Windows.Forms.Label();
		this.cbToastWindowMiddleClickAction = new System.Windows.Forms.ComboBox();
		this.cbToastWindowRightClickAction = new System.Windows.Forms.ComboBox();
		this.cbToastWindowLeftClickAction = new System.Windows.Forms.ComboBox();
		this.nudToastWindowSizeHeight = new System.Windows.Forms.NumericUpDown();
		this.nudToastWindowSizeWidth = new System.Windows.Forms.NumericUpDown();
		this.cbToastWindowPlacement = new System.Windows.Forms.ComboBox();
		this.nudToastWindowFadeDuration = new System.Windows.Forms.NumericUpDown();
		this.nudToastWindowDuration = new System.Windows.Forms.NumericUpDown();
		this.lblToastWindowMiddleClickAction = new System.Windows.Forms.Label();
		this.lblToastWindowRightClickAction = new System.Windows.Forms.Label();
		this.lblToastWindowLeftClickAction = new System.Windows.Forms.Label();
		this.lblToastWindowSize = new System.Windows.Forms.Label();
		this.lblToastWindowPlacement = new System.Windows.Forms.Label();
		this.lblToastWindowFadeDuration = new System.Windows.Forms.Label();
		this.lblToastWindowDuration = new System.Windows.Forms.Label();
		this.cbDisableNotificationsOnFullscreen = new System.Windows.Forms.CheckBox();
		this.cbDisableNotifications = new System.Windows.Forms.CheckBox();
		this.cbPlaySoundAfterCapture = new System.Windows.Forms.CheckBox();
		this.cbPlaySoundAfterUpload = new System.Windows.Forms.CheckBox();
		this.tpImage = new System.Windows.Forms.TabPage();
		this.tcImage = new System.Windows.Forms.TabControl();
		this.tpQuality = new System.Windows.Forms.TabPage();
		this.pImage = new System.Windows.Forms.Panel();
		this.cbImageAutoJPEGQuality = new System.Windows.Forms.CheckBox();
		this.cbImagePNGBitDepth = new System.Windows.Forms.ComboBox();
		this.lblImagePNGBitDepth = new System.Windows.Forms.Label();
		this.cbImageAutoUseJPEG = new System.Windows.Forms.CheckBox();
		this.lblImageFormat = new System.Windows.Forms.Label();
		this.cbImageFileExist = new System.Windows.Forms.ComboBox();
		this.lblImageFileExist = new System.Windows.Forms.Label();
		this.nudImageAutoUseJPEGSize = new System.Windows.Forms.NumericUpDown();
		this.lblImageSizeLimitHint = new System.Windows.Forms.Label();
		this.nudImageJPEGQuality = new System.Windows.Forms.NumericUpDown();
		this.cbImageFormat = new System.Windows.Forms.ComboBox();
		this.lblImageJPEGQualityHint = new System.Windows.Forms.Label();
		this.lblImageGIFQuality = new System.Windows.Forms.Label();
		this.lblImageJPEGQuality = new System.Windows.Forms.Label();
		this.cbImageGIFQuality = new System.Windows.Forms.ComboBox();
		this.cbOverrideImageSettings = new System.Windows.Forms.CheckBox();
		this.tpEffects = new System.Windows.Forms.TabPage();
		this.lblImageEffectsNote = new System.Windows.Forms.Label();
		this.cbShowImageEffectsWindowAfterCapture = new System.Windows.Forms.CheckBox();
		this.cbImageEffectOnlyRegionCapture = new System.Windows.Forms.CheckBox();
		this.btnImageEffects = new System.Windows.Forms.Button();
		this.tpThumbnail = new System.Windows.Forms.TabPage();
		this.cbThumbnailIfSmaller = new System.Windows.Forms.CheckBox();
		this.lblThumbnailNamePreview = new System.Windows.Forms.Label();
		this.lblThumbnailName = new System.Windows.Forms.Label();
		this.txtThumbnailName = new System.Windows.Forms.TextBox();
		this.lblThumbnailHeight = new System.Windows.Forms.Label();
		this.lblThumbnailWidth = new System.Windows.Forms.Label();
		this.nudThumbnailHeight = new System.Windows.Forms.NumericUpDown();
		this.nudThumbnailWidth = new System.Windows.Forms.NumericUpDown();
		this.tpCapture = new System.Windows.Forms.TabPage();
		this.tcCapture = new System.Windows.Forms.TabControl();
		this.tpCaptureGeneral = new System.Windows.Forms.TabPage();
		this.pCapture = new System.Windows.Forms.Panel();
		this.lblScreenshotDelay = new System.Windows.Forms.Label();
		this.btnCaptureCustomRegionSelectRectangle = new System.Windows.Forms.Button();
		this.lblCaptureCustomRegion = new System.Windows.Forms.Label();
		this.lblCaptureCustomRegionWidth = new System.Windows.Forms.Label();
		this.lblCaptureCustomRegionHeight = new System.Windows.Forms.Label();
		this.lblCaptureCustomRegionY = new System.Windows.Forms.Label();
		this.lblCaptureCustomRegionX = new System.Windows.Forms.Label();
		this.nudCaptureCustomRegionHeight = new System.Windows.Forms.NumericUpDown();
		this.nudCaptureCustomRegionWidth = new System.Windows.Forms.NumericUpDown();
		this.nudCaptureCustomRegionY = new System.Windows.Forms.NumericUpDown();
		this.nudCaptureCustomRegionX = new System.Windows.Forms.NumericUpDown();
		this.cbShowCursor = new System.Windows.Forms.CheckBox();
		this.lblCaptureShadowOffset = new System.Windows.Forms.Label();
		this.cbCaptureTransparent = new System.Windows.Forms.CheckBox();
		this.cbCaptureAutoHideTaskbar = new System.Windows.Forms.CheckBox();
		this.cbCaptureShadow = new System.Windows.Forms.CheckBox();
		this.lblScreenshotDelayInfo = new System.Windows.Forms.Label();
		this.cbCaptureClientArea = new System.Windows.Forms.CheckBox();
		this.nudScreenshotDelay = new System.Windows.Forms.NumericUpDown();
		this.nudCaptureShadowOffset = new System.Windows.Forms.NumericUpDown();
		this.cbOverrideCaptureSettings = new System.Windows.Forms.CheckBox();
		this.tpRegionCapture = new System.Windows.Forms.TabPage();
		this.cbRegionCaptureActiveMonitorMode = new System.Windows.Forms.CheckBox();
		this.nudRegionCaptureFPSLimit = new System.Windows.Forms.NumericUpDown();
		this.lblRegionCaptureFPSLimit = new System.Windows.Forms.Label();
		this.cbRegionCaptureShowFPS = new System.Windows.Forms.CheckBox();
		this.flpRegionCaptureFixedSize = new System.Windows.Forms.FlowLayoutPanel();
		this.lblRegionCaptureFixedSizeWidth = new System.Windows.Forms.Label();
		this.nudRegionCaptureFixedSizeWidth = new System.Windows.Forms.NumericUpDown();
		this.lblRegionCaptureFixedSizeHeight = new System.Windows.Forms.Label();
		this.nudRegionCaptureFixedSizeHeight = new System.Windows.Forms.NumericUpDown();
		this.cbRegionCaptureIsFixedSize = new System.Windows.Forms.CheckBox();
		this.cbRegionCaptureShowCrosshair = new System.Windows.Forms.CheckBox();
		this.lblRegionCaptureMagnifierPixelSize = new System.Windows.Forms.Label();
		this.lblRegionCaptureMagnifierPixelCount = new System.Windows.Forms.Label();
		this.cbRegionCaptureUseSquareMagnifier = new System.Windows.Forms.CheckBox();
		this.cbRegionCaptureShowMagnifier = new System.Windows.Forms.CheckBox();
		this.cbRegionCaptureShowInfo = new System.Windows.Forms.CheckBox();
		this.btnRegionCaptureSnapSizesRemove = new System.Windows.Forms.Button();
		this.btnRegionCaptureSnapSizesAdd = new System.Windows.Forms.Button();
		this.cbRegionCaptureSnapSizes = new System.Windows.Forms.ComboBox();
		this.lblRegionCaptureSnapSizes = new System.Windows.Forms.Label();
		this.cbRegionCaptureUseCustomInfoText = new System.Windows.Forms.CheckBox();
		this.cbRegionCaptureDetectControls = new System.Windows.Forms.CheckBox();
		this.cbRegionCaptureDetectWindows = new System.Windows.Forms.CheckBox();
		this.cbRegionCaptureMouse5ClickAction = new System.Windows.Forms.ComboBox();
		this.lblRegionCaptureMouse5ClickAction = new System.Windows.Forms.Label();
		this.cbRegionCaptureMouse4ClickAction = new System.Windows.Forms.ComboBox();
		this.lblRegionCaptureMouse4ClickAction = new System.Windows.Forms.Label();
		this.cbRegionCaptureMouseMiddleClickAction = new System.Windows.Forms.ComboBox();
		this.lblRegionCaptureMouseMiddleClickAction = new System.Windows.Forms.Label();
		this.cbRegionCaptureMouseRightClickAction = new System.Windows.Forms.ComboBox();
		this.lblRegionCaptureMouseRightClickAction = new System.Windows.Forms.Label();
		this.cbRegionCaptureMultiRegionMode = new System.Windows.Forms.CheckBox();
		this.pRegionCaptureSnapSizes = new System.Windows.Forms.Panel();
		this.btnRegionCaptureSnapSizesDialogCancel = new System.Windows.Forms.Button();
		this.btnRegionCaptureSnapSizesDialogAdd = new System.Windows.Forms.Button();
		this.nudRegionCaptureSnapSizesHeight = new System.Windows.Forms.NumericUpDown();
		this.RegionCaptureSnapSizesHeight = new System.Windows.Forms.Label();
		this.nudRegionCaptureSnapSizesWidth = new System.Windows.Forms.NumericUpDown();
		this.lblRegionCaptureSnapSizesWidth = new System.Windows.Forms.Label();
		this.cbRegionCaptureUseDimming = new System.Windows.Forms.CheckBox();
		this.txtRegionCaptureCustomInfoText = new System.Windows.Forms.TextBox();
		this.nudRegionCaptureMagnifierPixelCount = new System.Windows.Forms.NumericUpDown();
		this.nudRegionCaptureMagnifierPixelSize = new System.Windows.Forms.NumericUpDown();
		this.tpScreenRecorder = new System.Windows.Forms.TabPage();
		this.cbScreenRecordTransparentRegion = new System.Windows.Forms.CheckBox();
		this.cbScreenRecordTwoPassEncoding = new System.Windows.Forms.CheckBox();
		this.cbScreenRecordConfirmAbort = new System.Windows.Forms.CheckBox();
		this.cbScreenRecorderShowCursor = new System.Windows.Forms.CheckBox();
		this.btnScreenRecorderFFmpegOptions = new System.Windows.Forms.Button();
		this.lblScreenRecorderStartDelay = new System.Windows.Forms.Label();
		this.cbScreenRecordAutoStart = new System.Windows.Forms.CheckBox();
		this.lblScreenRecorderFixedDuration = new System.Windows.Forms.Label();
		this.nudScreenRecordFPS = new System.Windows.Forms.NumericUpDown();
		this.lblScreenRecordFPS = new System.Windows.Forms.Label();
		this.nudScreenRecorderDuration = new System.Windows.Forms.NumericUpDown();
		this.nudScreenRecorderStartDelay = new System.Windows.Forms.NumericUpDown();
		this.cbScreenRecorderFixedDuration = new System.Windows.Forms.CheckBox();
		this.nudGIFFPS = new System.Windows.Forms.NumericUpDown();
		this.lblGIFFPS = new System.Windows.Forms.Label();
		this.tpOCR = new System.Windows.Forms.TabPage();
		this.btnCaptureOCRHelp = new System.Windows.Forms.Button();
		this.cbCaptureOCRAutoCopy = new System.Windows.Forms.CheckBox();
		this.cbCaptureOCRSilent = new System.Windows.Forms.CheckBox();
		this.lblOCRDefaultLanguage = new System.Windows.Forms.Label();
		this.cbCaptureOCRDefaultLanguage = new System.Windows.Forms.ComboBox();
		this.tpUpload = new System.Windows.Forms.TabPage();
		this.tcUpload = new System.Windows.Forms.TabControl();
		this.tpUploadMain = new System.Windows.Forms.TabPage();
		this.cbOverrideUploadSettings = new System.Windows.Forms.CheckBox();
		this.tpFileNaming = new System.Windows.Forms.TabPage();
		this.txtURLRegexReplaceReplacement = new System.Windows.Forms.TextBox();
		this.lblURLRegexReplaceReplacement = new System.Windows.Forms.Label();
		this.txtURLRegexReplacePattern = new System.Windows.Forms.TextBox();
		this.lblURLRegexReplacePattern = new System.Windows.Forms.Label();
		this.cbURLRegexReplace = new System.Windows.Forms.CheckBox();
		this.btnAutoIncrementNumber = new System.Windows.Forms.Button();
		this.lblAutoIncrementNumber = new System.Windows.Forms.Label();
		this.nudAutoIncrementNumber = new System.Windows.Forms.NumericUpDown();
		this.cbFileUploadReplaceProblematicCharacters = new System.Windows.Forms.CheckBox();
		this.cbNameFormatCustomTimeZone = new System.Windows.Forms.CheckBox();
		this.lblNameFormatPatternPreview = new System.Windows.Forms.Label();
		this.lblNameFormatPatternActiveWindow = new System.Windows.Forms.Label();
		this.lblNameFormatPatternPreviewActiveWindow = new System.Windows.Forms.Label();
		this.cbNameFormatTimeZone = new System.Windows.Forms.ComboBox();
		this.txtNameFormatPatternActiveWindow = new System.Windows.Forms.TextBox();
		this.cbFileUploadUseNamePattern = new System.Windows.Forms.CheckBox();
		this.lblNameFormatPattern = new System.Windows.Forms.Label();
		this.txtNameFormatPattern = new System.Windows.Forms.TextBox();
		this.tpUploadClipboard = new System.Windows.Forms.TabPage();
		this.cbClipboardUploadShareURL = new System.Windows.Forms.CheckBox();
		this.cbClipboardUploadURLContents = new System.Windows.Forms.CheckBox();
		this.cbClipboardUploadAutoIndexFolder = new System.Windows.Forms.CheckBox();
		this.cbClipboardUploadShortenURL = new System.Windows.Forms.CheckBox();
		this.tpUploaderFilters = new System.Windows.Forms.TabPage();
		this.lvUploaderFiltersList = new ShareX.HelpersLib.MyListView();
		this.chUploaderFiltersName = new System.Windows.Forms.ColumnHeader();
		this.chUploaderFiltersExtension = new System.Windows.Forms.ColumnHeader();
		this.btnUploaderFiltersRemove = new System.Windows.Forms.Button();
		this.btnUploaderFiltersUpdate = new System.Windows.Forms.Button();
		this.btnUploaderFiltersAdd = new System.Windows.Forms.Button();
		this.lblUploaderFiltersDestination = new System.Windows.Forms.Label();
		this.cbUploaderFiltersDestination = new System.Windows.Forms.ComboBox();
		this.lblUploaderFiltersExtensionsExample = new System.Windows.Forms.Label();
		this.lblUploaderFiltersExtensions = new System.Windows.Forms.Label();
		this.txtUploaderFiltersExtensions = new System.Windows.Forms.TextBox();
		this.tpActions = new System.Windows.Forms.TabPage();
		this.pActions = new System.Windows.Forms.Panel();
		this.btnActions = new System.Windows.Forms.Button();
		this.lblActionsNote = new System.Windows.Forms.Label();
		this.btnActionsDuplicate = new System.Windows.Forms.Button();
		this.btnActionsAdd = new System.Windows.Forms.Button();
		this.lvActions = new ShareX.HelpersLib.MyListView();
		this.chActionsName = new System.Windows.Forms.ColumnHeader();
		this.chActionsPath = new System.Windows.Forms.ColumnHeader();
		this.chActionsArgs = new System.Windows.Forms.ColumnHeader();
		this.chActionsExtensions = new System.Windows.Forms.ColumnHeader();
		this.btnActionsEdit = new System.Windows.Forms.Button();
		this.btnActionsRemove = new System.Windows.Forms.Button();
		this.cbOverrideActions = new System.Windows.Forms.CheckBox();
		this.tpWatchFolders = new System.Windows.Forms.TabPage();
		this.btnWatchFolderEdit = new System.Windows.Forms.Button();
		this.cbWatchFolderEnabled = new System.Windows.Forms.CheckBox();
		this.lvWatchFolderList = new ShareX.HelpersLib.MyListView();
		this.chWatchFolderFolderPath = new System.Windows.Forms.ColumnHeader();
		this.chWatchFolderFilter = new System.Windows.Forms.ColumnHeader();
		this.chWatchFolderIncludeSubdirectories = new System.Windows.Forms.ColumnHeader();
		this.btnWatchFolderRemove = new System.Windows.Forms.Button();
		this.btnWatchFolderAdd = new System.Windows.Forms.Button();
		this.tpTools = new System.Windows.Forms.TabPage();
		this.pTools = new System.Windows.Forms.Panel();
		this.txtToolsScreenColorPickerFormatCtrl = new System.Windows.Forms.TextBox();
		this.lblToolsScreenColorPickerFormatCtrl = new System.Windows.Forms.Label();
		this.txtToolsScreenColorPickerInfoText = new System.Windows.Forms.TextBox();
		this.lblToolsScreenColorPickerInfoText = new System.Windows.Forms.Label();
		this.txtToolsScreenColorPickerFormat = new System.Windows.Forms.TextBox();
		this.lblToolsScreenColorPickerFormat = new System.Windows.Forms.Label();
		this.cbOverrideToolsSettings = new System.Windows.Forms.CheckBox();
		this.tpAdvanced = new System.Windows.Forms.TabPage();
		this.pgTaskSettings = new System.Windows.Forms.PropertyGrid();
		this.cbOverrideAdvancedSettings = new System.Windows.Forms.CheckBox();
		this.tttvMain = new ShareX.HelpersLib.TabToTreeView();
		this.tcTaskSettings.SuspendLayout();
		this.tpTask.SuspendLayout();
		this.cmsDestinations.SuspendLayout();
		this.tpGeneral.SuspendLayout();
		this.tcGeneral.SuspendLayout();
		this.tpGeneralMain.SuspendLayout();
		this.tpNotifications.SuspendLayout();
		this.gbToastWindow.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudToastWindowSizeHeight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudToastWindowSizeWidth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudToastWindowFadeDuration).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudToastWindowDuration).BeginInit();
		this.tpImage.SuspendLayout();
		this.tcImage.SuspendLayout();
		this.tpQuality.SuspendLayout();
		this.pImage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudImageAutoUseJPEGSize).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudImageJPEGQuality).BeginInit();
		this.tpEffects.SuspendLayout();
		this.tpThumbnail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudThumbnailHeight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudThumbnailWidth).BeginInit();
		this.tpCapture.SuspendLayout();
		this.tcCapture.SuspendLayout();
		this.tpCaptureGeneral.SuspendLayout();
		this.pCapture.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureCustomRegionHeight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureCustomRegionWidth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureCustomRegionY).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureCustomRegionX).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudScreenshotDelay).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureShadowOffset).BeginInit();
		this.tpRegionCapture.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureFPSLimit).BeginInit();
		this.flpRegionCaptureFixedSize.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureFixedSizeWidth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureFixedSizeHeight).BeginInit();
		this.pRegionCaptureSnapSizes.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureSnapSizesHeight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureSnapSizesWidth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureMagnifierPixelCount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureMagnifierPixelSize).BeginInit();
		this.tpScreenRecorder.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudScreenRecordFPS).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudScreenRecorderDuration).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudScreenRecorderStartDelay).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudGIFFPS).BeginInit();
		this.tpOCR.SuspendLayout();
		this.tpUpload.SuspendLayout();
		this.tcUpload.SuspendLayout();
		this.tpUploadMain.SuspendLayout();
		this.tpFileNaming.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nudAutoIncrementNumber).BeginInit();
		this.tpUploadClipboard.SuspendLayout();
		this.tpUploaderFilters.SuspendLayout();
		this.tpActions.SuspendLayout();
		this.pActions.SuspendLayout();
		this.tpWatchFolders.SuspendLayout();
		this.tpTools.SuspendLayout();
		this.pTools.SuspendLayout();
		this.tpAdvanced.SuspendLayout();
		base.SuspendLayout();
		this.cmsAfterCapture.Name = "cmsAfterCapture";
		resources.ApplyResources(this.cmsAfterCapture, "cmsAfterCapture");
		this.cmsAfterUpload.Name = "cmsAfterCapture";
		resources.ApplyResources(this.cmsAfterUpload, "cmsAfterUpload");
		resources.ApplyResources(this.cbOverrideAfterCaptureSettings, "cbOverrideAfterCaptureSettings");
		this.cbOverrideAfterCaptureSettings.Name = "cbOverrideAfterCaptureSettings";
		this.cbOverrideAfterCaptureSettings.UseVisualStyleBackColor = true;
		this.cbOverrideAfterCaptureSettings.CheckedChanged += new System.EventHandler(cbUseDefaultAfterCaptureSettings_CheckedChanged);
		resources.ApplyResources(this.cbOverrideAfterUploadSettings, "cbOverrideAfterUploadSettings");
		this.cbOverrideAfterUploadSettings.Name = "cbOverrideAfterUploadSettings";
		this.cbOverrideAfterUploadSettings.UseVisualStyleBackColor = true;
		this.cbOverrideAfterUploadSettings.CheckedChanged += new System.EventHandler(cbUseDefaultAfterUploadSettings_CheckedChanged);
		resources.ApplyResources(this.cbOverrideDestinationSettings, "cbOverrideDestinationSettings");
		this.cbOverrideDestinationSettings.Name = "cbOverrideDestinationSettings";
		this.cbOverrideDestinationSettings.UseVisualStyleBackColor = true;
		this.cbOverrideDestinationSettings.CheckedChanged += new System.EventHandler(cbUseDefaultDestinationSettings_CheckedChanged);
		resources.ApplyResources(this.lblDescription, "lblDescription");
		this.lblDescription.Name = "lblDescription";
		resources.ApplyResources(this.tbDescription, "tbDescription");
		this.tbDescription.Name = "tbDescription";
		this.tbDescription.TextChanged += new System.EventHandler(tbDescription_TextChanged);
		this.cmsTask.Name = "cmsAfterCapture";
		resources.ApplyResources(this.cmsTask, "cmsTask");
		this.tcTaskSettings.Controls.Add(this.tpTask);
		this.tcTaskSettings.Controls.Add(this.tpGeneral);
		this.tcTaskSettings.Controls.Add(this.tpImage);
		this.tcTaskSettings.Controls.Add(this.tpCapture);
		this.tcTaskSettings.Controls.Add(this.tpUpload);
		this.tcTaskSettings.Controls.Add(this.tpActions);
		this.tcTaskSettings.Controls.Add(this.tpWatchFolders);
		this.tcTaskSettings.Controls.Add(this.tpTools);
		this.tcTaskSettings.Controls.Add(this.tpAdvanced);
		resources.ApplyResources(this.tcTaskSettings, "tcTaskSettings");
		this.tcTaskSettings.Name = "tcTaskSettings";
		this.tcTaskSettings.SelectedIndex = 0;
		this.tpTask.BackColor = System.Drawing.SystemColors.Window;
		this.tpTask.Controls.Add(this.lblTask);
		this.tpTask.Controls.Add(this.btnScreenshotsFolderBrowse);
		this.tpTask.Controls.Add(this.txtScreenshotsFolder);
		this.tpTask.Controls.Add(this.cbOverrideScreenshotsFolder);
		this.tpTask.Controls.Add(this.cbCustomUploaders);
		this.tpTask.Controls.Add(this.cbOverrideCustomUploader);
		this.tpTask.Controls.Add(this.cbOverrideFTPAccount);
		this.tpTask.Controls.Add(this.cbFTPAccounts);
		this.tpTask.Controls.Add(this.tbDescription);
		this.tpTask.Controls.Add(this.btnAfterCapture);
		this.tpTask.Controls.Add(this.btnAfterUpload);
		this.tpTask.Controls.Add(this.btnDestinations);
		this.tpTask.Controls.Add(this.cbOverrideAfterCaptureSettings);
		this.tpTask.Controls.Add(this.btnTask);
		this.tpTask.Controls.Add(this.cbOverrideAfterUploadSettings);
		this.tpTask.Controls.Add(this.cbOverrideDestinationSettings);
		this.tpTask.Controls.Add(this.lblDescription);
		resources.ApplyResources(this.tpTask, "tpTask");
		this.tpTask.Name = "tpTask";
		resources.ApplyResources(this.lblTask, "lblTask");
		this.lblTask.Name = "lblTask";
		resources.ApplyResources(this.btnScreenshotsFolderBrowse, "btnScreenshotsFolderBrowse");
		this.btnScreenshotsFolderBrowse.Name = "btnScreenshotsFolderBrowse";
		this.btnScreenshotsFolderBrowse.UseVisualStyleBackColor = true;
		this.btnScreenshotsFolderBrowse.Click += new System.EventHandler(btnScreenshotsFolderBrowse_Click);
		resources.ApplyResources(this.txtScreenshotsFolder, "txtScreenshotsFolder");
		this.txtScreenshotsFolder.Name = "txtScreenshotsFolder";
		this.txtScreenshotsFolder.TextChanged += new System.EventHandler(txtScreenshotsFolder_TextChanged);
		resources.ApplyResources(this.cbOverrideScreenshotsFolder, "cbOverrideScreenshotsFolder");
		this.cbOverrideScreenshotsFolder.Name = "cbOverrideScreenshotsFolder";
		this.cbOverrideScreenshotsFolder.UseVisualStyleBackColor = true;
		this.cbOverrideScreenshotsFolder.CheckedChanged += new System.EventHandler(cbOverrideScreenshotsFolder_CheckedChanged);
		this.cbCustomUploaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbCustomUploaders.FormattingEnabled = true;
		resources.ApplyResources(this.cbCustomUploaders, "cbCustomUploaders");
		this.cbCustomUploaders.Name = "cbCustomUploaders";
		this.cbCustomUploaders.SelectedIndexChanged += new System.EventHandler(cbCustomUploaders_SelectedIndexChanged);
		resources.ApplyResources(this.cbOverrideCustomUploader, "cbOverrideCustomUploader");
		this.cbOverrideCustomUploader.Name = "cbOverrideCustomUploader";
		this.cbOverrideCustomUploader.UseVisualStyleBackColor = true;
		this.cbOverrideCustomUploader.CheckedChanged += new System.EventHandler(cbOverrideCustomUploader_CheckedChanged);
		resources.ApplyResources(this.cbOverrideFTPAccount, "cbOverrideFTPAccount");
		this.cbOverrideFTPAccount.Name = "cbOverrideFTPAccount";
		this.cbOverrideFTPAccount.UseVisualStyleBackColor = true;
		this.cbOverrideFTPAccount.CheckedChanged += new System.EventHandler(cbOverrideFTPAccount_CheckedChanged);
		this.cbFTPAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbFTPAccounts.FormattingEnabled = true;
		resources.ApplyResources(this.cbFTPAccounts, "cbFTPAccounts");
		this.cbFTPAccounts.Name = "cbFTPAccounts";
		this.cbFTPAccounts.SelectedIndexChanged += new System.EventHandler(cbFTPAccounts_SelectedIndexChanged);
		resources.ApplyResources(this.btnAfterCapture, "btnAfterCapture");
		this.btnAfterCapture.Menu = this.cmsAfterCapture;
		this.btnAfterCapture.Name = "btnAfterCapture";
		this.btnAfterCapture.UseMnemonic = false;
		this.btnAfterCapture.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.btnAfterUpload, "btnAfterUpload");
		this.btnAfterUpload.Menu = this.cmsAfterUpload;
		this.btnAfterUpload.Name = "btnAfterUpload";
		this.btnAfterUpload.UseMnemonic = false;
		this.btnAfterUpload.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.btnDestinations, "btnDestinations");
		this.btnDestinations.Menu = this.cmsDestinations;
		this.btnDestinations.Name = "btnDestinations";
		this.btnDestinations.UseMnemonic = false;
		this.btnDestinations.UseVisualStyleBackColor = true;
		this.cmsDestinations.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsmiImageUploaders, this.tsmiTextUploaders, this.tsmiFileUploaders, this.tsmiURLShorteners, this.tsmiURLSharingServices });
		this.cmsDestinations.Name = "cmsDestinations";
		resources.ApplyResources(this.cmsDestinations, "cmsDestinations");
		this.tsmiImageUploaders.Image = ShareX.Properties.Resources.image;
		this.tsmiImageUploaders.Name = "tsmiImageUploaders";
		resources.ApplyResources(this.tsmiImageUploaders, "tsmiImageUploaders");
		this.tsmiTextUploaders.Image = ShareX.Properties.Resources.notebook;
		this.tsmiTextUploaders.Name = "tsmiTextUploaders";
		resources.ApplyResources(this.tsmiTextUploaders, "tsmiTextUploaders");
		this.tsmiFileUploaders.Image = ShareX.Properties.Resources.application_block;
		this.tsmiFileUploaders.Name = "tsmiFileUploaders";
		resources.ApplyResources(this.tsmiFileUploaders, "tsmiFileUploaders");
		this.tsmiURLShorteners.Image = ShareX.Properties.Resources.edit_scale;
		this.tsmiURLShorteners.Name = "tsmiURLShorteners";
		resources.ApplyResources(this.tsmiURLShorteners, "tsmiURLShorteners");
		this.tsmiURLSharingServices.Image = ShareX.Properties.Resources.globe_share;
		this.tsmiURLSharingServices.Name = "tsmiURLSharingServices";
		resources.ApplyResources(this.tsmiURLSharingServices, "tsmiURLSharingServices");
		this.btnTask.Image = ShareX.Properties.Resources.gear;
		resources.ApplyResources(this.btnTask, "btnTask");
		this.btnTask.Menu = this.cmsTask;
		this.btnTask.Name = "btnTask";
		this.btnTask.UseMnemonic = false;
		this.btnTask.UseVisualStyleBackColor = true;
		this.tpGeneral.BackColor = System.Drawing.SystemColors.Window;
		this.tpGeneral.Controls.Add(this.tcGeneral);
		resources.ApplyResources(this.tpGeneral, "tpGeneral");
		this.tpGeneral.Name = "tpGeneral";
		this.tcGeneral.Controls.Add(this.tpGeneralMain);
		this.tcGeneral.Controls.Add(this.tpNotifications);
		resources.ApplyResources(this.tcGeneral, "tcGeneral");
		this.tcGeneral.Name = "tcGeneral";
		this.tcGeneral.SelectedIndex = 0;
		this.tpGeneralMain.Controls.Add(this.cbOverrideGeneralSettings);
		resources.ApplyResources(this.tpGeneralMain, "tpGeneralMain");
		this.tpGeneralMain.Name = "tpGeneralMain";
		this.tpGeneralMain.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.cbOverrideGeneralSettings, "cbOverrideGeneralSettings");
		this.cbOverrideGeneralSettings.Checked = true;
		this.cbOverrideGeneralSettings.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbOverrideGeneralSettings.Name = "cbOverrideGeneralSettings";
		this.cbOverrideGeneralSettings.UseVisualStyleBackColor = true;
		this.cbOverrideGeneralSettings.CheckedChanged += new System.EventHandler(cbUseDefaultGeneralSettings_CheckedChanged);
		this.tpNotifications.Controls.Add(this.cbShowToastNotificationAfterTaskCompleted);
		this.tpNotifications.Controls.Add(this.btnCustomErrorSoundPath);
		this.tpNotifications.Controls.Add(this.btnCustomTaskCompletedSoundPath);
		this.tpNotifications.Controls.Add(this.btnCustomCaptureSoundPath);
		this.tpNotifications.Controls.Add(this.txtCustomErrorSoundPath);
		this.tpNotifications.Controls.Add(this.txtCustomTaskCompletedSoundPath);
		this.tpNotifications.Controls.Add(this.txtCustomCaptureSoundPath);
		this.tpNotifications.Controls.Add(this.cbUseCustomErrorSound);
		this.tpNotifications.Controls.Add(this.cbUseCustomTaskCompletedSound);
		this.tpNotifications.Controls.Add(this.cbUseCustomCaptureSound);
		this.tpNotifications.Controls.Add(this.gbToastWindow);
		this.tpNotifications.Controls.Add(this.cbDisableNotificationsOnFullscreen);
		this.tpNotifications.Controls.Add(this.cbDisableNotifications);
		this.tpNotifications.Controls.Add(this.cbPlaySoundAfterCapture);
		this.tpNotifications.Controls.Add(this.cbPlaySoundAfterUpload);
		resources.ApplyResources(this.tpNotifications, "tpNotifications");
		this.tpNotifications.Name = "tpNotifications";
		this.tpNotifications.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.cbShowToastNotificationAfterTaskCompleted, "cbShowToastNotificationAfterTaskCompleted");
		this.cbShowToastNotificationAfterTaskCompleted.Name = "cbShowToastNotificationAfterTaskCompleted";
		this.cbShowToastNotificationAfterTaskCompleted.UseVisualStyleBackColor = true;
		this.cbShowToastNotificationAfterTaskCompleted.CheckedChanged += new System.EventHandler(cbShowToastNotificationAfterTaskCompleted_CheckedChanged);
		resources.ApplyResources(this.btnCustomErrorSoundPath, "btnCustomErrorSoundPath");
		this.btnCustomErrorSoundPath.Name = "btnCustomErrorSoundPath";
		this.btnCustomErrorSoundPath.UseVisualStyleBackColor = true;
		this.btnCustomErrorSoundPath.Click += new System.EventHandler(btnCustomErrorSoundPath_Click);
		resources.ApplyResources(this.btnCustomTaskCompletedSoundPath, "btnCustomTaskCompletedSoundPath");
		this.btnCustomTaskCompletedSoundPath.Name = "btnCustomTaskCompletedSoundPath";
		this.btnCustomTaskCompletedSoundPath.UseVisualStyleBackColor = true;
		this.btnCustomTaskCompletedSoundPath.Click += new System.EventHandler(btnCustomTaskCompletedSoundPath_Click);
		resources.ApplyResources(this.btnCustomCaptureSoundPath, "btnCustomCaptureSoundPath");
		this.btnCustomCaptureSoundPath.Name = "btnCustomCaptureSoundPath";
		this.btnCustomCaptureSoundPath.UseVisualStyleBackColor = true;
		this.btnCustomCaptureSoundPath.Click += new System.EventHandler(btnCustomCaptureSoundPath_Click);
		resources.ApplyResources(this.txtCustomErrorSoundPath, "txtCustomErrorSoundPath");
		this.txtCustomErrorSoundPath.Name = "txtCustomErrorSoundPath";
		this.txtCustomErrorSoundPath.TextChanged += new System.EventHandler(txtCustomErrorSoundPath_TextChanged);
		resources.ApplyResources(this.txtCustomTaskCompletedSoundPath, "txtCustomTaskCompletedSoundPath");
		this.txtCustomTaskCompletedSoundPath.Name = "txtCustomTaskCompletedSoundPath";
		this.txtCustomTaskCompletedSoundPath.TextChanged += new System.EventHandler(txtCustomTaskCompletedSoundPath_TextChanged);
		resources.ApplyResources(this.txtCustomCaptureSoundPath, "txtCustomCaptureSoundPath");
		this.txtCustomCaptureSoundPath.Name = "txtCustomCaptureSoundPath";
		this.txtCustomCaptureSoundPath.TextChanged += new System.EventHandler(txtCustomCaptureSoundPath_TextChanged);
		resources.ApplyResources(this.cbUseCustomErrorSound, "cbUseCustomErrorSound");
		this.cbUseCustomErrorSound.Name = "cbUseCustomErrorSound";
		this.cbUseCustomErrorSound.UseVisualStyleBackColor = true;
		this.cbUseCustomErrorSound.CheckedChanged += new System.EventHandler(cbUseCustomErrorSound_CheckedChanged);
		resources.ApplyResources(this.cbUseCustomTaskCompletedSound, "cbUseCustomTaskCompletedSound");
		this.cbUseCustomTaskCompletedSound.Name = "cbUseCustomTaskCompletedSound";
		this.cbUseCustomTaskCompletedSound.UseVisualStyleBackColor = true;
		this.cbUseCustomTaskCompletedSound.CheckedChanged += new System.EventHandler(cbUseCustomTaskCompletedSound_CheckedChanged);
		resources.ApplyResources(this.cbUseCustomCaptureSound, "cbUseCustomCaptureSound");
		this.cbUseCustomCaptureSound.Name = "cbUseCustomCaptureSound";
		this.cbUseCustomCaptureSound.UseVisualStyleBackColor = true;
		this.cbUseCustomCaptureSound.CheckedChanged += new System.EventHandler(cbUseCustomCaptureSound_CheckedChanged);
		this.gbToastWindow.Controls.Add(this.cbToastWindowAutoHide);
		this.gbToastWindow.Controls.Add(this.lblToastWindowFadeDurationSeconds);
		this.gbToastWindow.Controls.Add(this.lblToastWindowDurationSeconds);
		this.gbToastWindow.Controls.Add(this.lblToastWindowSizeX);
		this.gbToastWindow.Controls.Add(this.cbToastWindowMiddleClickAction);
		this.gbToastWindow.Controls.Add(this.cbToastWindowRightClickAction);
		this.gbToastWindow.Controls.Add(this.cbToastWindowLeftClickAction);
		this.gbToastWindow.Controls.Add(this.nudToastWindowSizeHeight);
		this.gbToastWindow.Controls.Add(this.nudToastWindowSizeWidth);
		this.gbToastWindow.Controls.Add(this.cbToastWindowPlacement);
		this.gbToastWindow.Controls.Add(this.nudToastWindowFadeDuration);
		this.gbToastWindow.Controls.Add(this.nudToastWindowDuration);
		this.gbToastWindow.Controls.Add(this.lblToastWindowMiddleClickAction);
		this.gbToastWindow.Controls.Add(this.lblToastWindowRightClickAction);
		this.gbToastWindow.Controls.Add(this.lblToastWindowLeftClickAction);
		this.gbToastWindow.Controls.Add(this.lblToastWindowSize);
		this.gbToastWindow.Controls.Add(this.lblToastWindowPlacement);
		this.gbToastWindow.Controls.Add(this.lblToastWindowFadeDuration);
		this.gbToastWindow.Controls.Add(this.lblToastWindowDuration);
		resources.ApplyResources(this.gbToastWindow, "gbToastWindow");
		this.gbToastWindow.Name = "gbToastWindow";
		this.gbToastWindow.TabStop = false;
		resources.ApplyResources(this.cbToastWindowAutoHide, "cbToastWindowAutoHide");
		this.cbToastWindowAutoHide.Name = "cbToastWindowAutoHide";
		this.cbToastWindowAutoHide.UseVisualStyleBackColor = true;
		this.cbToastWindowAutoHide.CheckedChanged += new System.EventHandler(cbToastWindowAutoHide_CheckedChanged);
		resources.ApplyResources(this.lblToastWindowFadeDurationSeconds, "lblToastWindowFadeDurationSeconds");
		this.lblToastWindowFadeDurationSeconds.Name = "lblToastWindowFadeDurationSeconds";
		resources.ApplyResources(this.lblToastWindowDurationSeconds, "lblToastWindowDurationSeconds");
		this.lblToastWindowDurationSeconds.Name = "lblToastWindowDurationSeconds";
		resources.ApplyResources(this.lblToastWindowSizeX, "lblToastWindowSizeX");
		this.lblToastWindowSizeX.Name = "lblToastWindowSizeX";
		this.cbToastWindowMiddleClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbToastWindowMiddleClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbToastWindowMiddleClickAction, "cbToastWindowMiddleClickAction");
		this.cbToastWindowMiddleClickAction.Name = "cbToastWindowMiddleClickAction";
		this.cbToastWindowMiddleClickAction.SelectedIndexChanged += new System.EventHandler(cbToastWindowMiddleClickAction_SelectedIndexChanged);
		this.cbToastWindowRightClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbToastWindowRightClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbToastWindowRightClickAction, "cbToastWindowRightClickAction");
		this.cbToastWindowRightClickAction.Name = "cbToastWindowRightClickAction";
		this.cbToastWindowRightClickAction.SelectedIndexChanged += new System.EventHandler(cbToastWindowRightClickAction_SelectedIndexChanged);
		this.cbToastWindowLeftClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbToastWindowLeftClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbToastWindowLeftClickAction, "cbToastWindowLeftClickAction");
		this.cbToastWindowLeftClickAction.Name = "cbToastWindowLeftClickAction";
		this.cbToastWindowLeftClickAction.SelectedIndexChanged += new System.EventHandler(cbToastWindowLeftClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.nudToastWindowSizeHeight, "nudToastWindowSizeHeight");
		this.nudToastWindowSizeHeight.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		this.nudToastWindowSizeHeight.Minimum = new decimal(new int[4] { 100, 0, 0, 0 });
		this.nudToastWindowSizeHeight.Name = "nudToastWindowSizeHeight";
		this.nudToastWindowSizeHeight.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		this.nudToastWindowSizeHeight.ValueChanged += new System.EventHandler(nudToastWindowSizeHeight_ValueChanged);
		resources.ApplyResources(this.nudToastWindowSizeWidth, "nudToastWindowSizeWidth");
		this.nudToastWindowSizeWidth.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		this.nudToastWindowSizeWidth.Minimum = new decimal(new int[4] { 100, 0, 0, 0 });
		this.nudToastWindowSizeWidth.Name = "nudToastWindowSizeWidth";
		this.nudToastWindowSizeWidth.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		this.nudToastWindowSizeWidth.ValueChanged += new System.EventHandler(nudToastWindowSizeWidth_ValueChanged);
		this.cbToastWindowPlacement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbToastWindowPlacement.FormattingEnabled = true;
		resources.ApplyResources(this.cbToastWindowPlacement, "cbToastWindowPlacement");
		this.cbToastWindowPlacement.Name = "cbToastWindowPlacement";
		this.cbToastWindowPlacement.SelectedIndexChanged += new System.EventHandler(cbToastWindowPlacement_SelectedIndexChanged);
		this.nudToastWindowFadeDuration.DecimalPlaces = 1;
		resources.ApplyResources(this.nudToastWindowFadeDuration, "nudToastWindowFadeDuration");
		this.nudToastWindowFadeDuration.Maximum = new decimal(new int[4] { 30, 0, 0, 0 });
		this.nudToastWindowFadeDuration.Name = "nudToastWindowFadeDuration";
		this.nudToastWindowFadeDuration.ValueChanged += new System.EventHandler(nudToastWindowFadeDuration_ValueChanged);
		this.nudToastWindowDuration.DecimalPlaces = 1;
		resources.ApplyResources(this.nudToastWindowDuration, "nudToastWindowDuration");
		this.nudToastWindowDuration.Maximum = new decimal(new int[4] { 30, 0, 0, 0 });
		this.nudToastWindowDuration.Name = "nudToastWindowDuration";
		this.nudToastWindowDuration.ValueChanged += new System.EventHandler(nudToastWindowDuration_ValueChanged);
		resources.ApplyResources(this.lblToastWindowMiddleClickAction, "lblToastWindowMiddleClickAction");
		this.lblToastWindowMiddleClickAction.Name = "lblToastWindowMiddleClickAction";
		resources.ApplyResources(this.lblToastWindowRightClickAction, "lblToastWindowRightClickAction");
		this.lblToastWindowRightClickAction.Name = "lblToastWindowRightClickAction";
		resources.ApplyResources(this.lblToastWindowLeftClickAction, "lblToastWindowLeftClickAction");
		this.lblToastWindowLeftClickAction.Name = "lblToastWindowLeftClickAction";
		resources.ApplyResources(this.lblToastWindowSize, "lblToastWindowSize");
		this.lblToastWindowSize.Name = "lblToastWindowSize";
		resources.ApplyResources(this.lblToastWindowPlacement, "lblToastWindowPlacement");
		this.lblToastWindowPlacement.Name = "lblToastWindowPlacement";
		resources.ApplyResources(this.lblToastWindowFadeDuration, "lblToastWindowFadeDuration");
		this.lblToastWindowFadeDuration.Name = "lblToastWindowFadeDuration";
		resources.ApplyResources(this.lblToastWindowDuration, "lblToastWindowDuration");
		this.lblToastWindowDuration.Name = "lblToastWindowDuration";
		resources.ApplyResources(this.cbDisableNotificationsOnFullscreen, "cbDisableNotificationsOnFullscreen");
		this.cbDisableNotificationsOnFullscreen.Name = "cbDisableNotificationsOnFullscreen";
		this.cbDisableNotificationsOnFullscreen.UseVisualStyleBackColor = true;
		this.cbDisableNotificationsOnFullscreen.CheckedChanged += new System.EventHandler(cbDisableNotificationsOnFullscreen_CheckedChanged);
		resources.ApplyResources(this.cbDisableNotifications, "cbDisableNotifications");
		this.cbDisableNotifications.Name = "cbDisableNotifications";
		this.cbDisableNotifications.UseVisualStyleBackColor = true;
		this.cbDisableNotifications.CheckedChanged += new System.EventHandler(cbDisableNotifications_CheckedChanged);
		resources.ApplyResources(this.cbPlaySoundAfterCapture, "cbPlaySoundAfterCapture");
		this.cbPlaySoundAfterCapture.Name = "cbPlaySoundAfterCapture";
		this.cbPlaySoundAfterCapture.UseVisualStyleBackColor = true;
		this.cbPlaySoundAfterCapture.CheckedChanged += new System.EventHandler(cbPlaySoundAfterCapture_CheckedChanged);
		resources.ApplyResources(this.cbPlaySoundAfterUpload, "cbPlaySoundAfterUpload");
		this.cbPlaySoundAfterUpload.Name = "cbPlaySoundAfterUpload";
		this.cbPlaySoundAfterUpload.UseVisualStyleBackColor = true;
		this.cbPlaySoundAfterUpload.CheckedChanged += new System.EventHandler(cbPlaySoundAfterUpload_CheckedChanged);
		this.tpImage.BackColor = System.Drawing.SystemColors.Window;
		this.tpImage.Controls.Add(this.tcImage);
		resources.ApplyResources(this.tpImage, "tpImage");
		this.tpImage.Name = "tpImage";
		this.tcImage.Controls.Add(this.tpQuality);
		this.tcImage.Controls.Add(this.tpEffects);
		this.tcImage.Controls.Add(this.tpThumbnail);
		resources.ApplyResources(this.tcImage, "tcImage");
		this.tcImage.Name = "tcImage";
		this.tcImage.SelectedIndex = 0;
		this.tpQuality.BackColor = System.Drawing.SystemColors.Window;
		this.tpQuality.Controls.Add(this.pImage);
		this.tpQuality.Controls.Add(this.cbOverrideImageSettings);
		resources.ApplyResources(this.tpQuality, "tpQuality");
		this.tpQuality.Name = "tpQuality";
		this.pImage.Controls.Add(this.cbImageAutoJPEGQuality);
		this.pImage.Controls.Add(this.cbImagePNGBitDepth);
		this.pImage.Controls.Add(this.lblImagePNGBitDepth);
		this.pImage.Controls.Add(this.cbImageAutoUseJPEG);
		this.pImage.Controls.Add(this.lblImageFormat);
		this.pImage.Controls.Add(this.cbImageFileExist);
		this.pImage.Controls.Add(this.lblImageFileExist);
		this.pImage.Controls.Add(this.nudImageAutoUseJPEGSize);
		this.pImage.Controls.Add(this.lblImageSizeLimitHint);
		this.pImage.Controls.Add(this.nudImageJPEGQuality);
		this.pImage.Controls.Add(this.cbImageFormat);
		this.pImage.Controls.Add(this.lblImageJPEGQualityHint);
		this.pImage.Controls.Add(this.lblImageGIFQuality);
		this.pImage.Controls.Add(this.lblImageJPEGQuality);
		this.pImage.Controls.Add(this.cbImageGIFQuality);
		resources.ApplyResources(this.pImage, "pImage");
		this.pImage.Name = "pImage";
		resources.ApplyResources(this.cbImageAutoJPEGQuality, "cbImageAutoJPEGQuality");
		this.cbImageAutoJPEGQuality.Name = "cbImageAutoJPEGQuality";
		this.cbImageAutoJPEGQuality.UseVisualStyleBackColor = true;
		this.cbImageAutoJPEGQuality.CheckedChanged += new System.EventHandler(cbImageAutoJPEGQuality_CheckedChanged);
		this.cbImagePNGBitDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbImagePNGBitDepth.FormattingEnabled = true;
		resources.ApplyResources(this.cbImagePNGBitDepth, "cbImagePNGBitDepth");
		this.cbImagePNGBitDepth.Name = "cbImagePNGBitDepth";
		this.cbImagePNGBitDepth.SelectedIndexChanged += new System.EventHandler(cbImagePNGBitDepth_SelectedIndexChanged);
		resources.ApplyResources(this.lblImagePNGBitDepth, "lblImagePNGBitDepth");
		this.lblImagePNGBitDepth.Name = "lblImagePNGBitDepth";
		resources.ApplyResources(this.cbImageAutoUseJPEG, "cbImageAutoUseJPEG");
		this.cbImageAutoUseJPEG.Name = "cbImageAutoUseJPEG";
		this.cbImageAutoUseJPEG.UseVisualStyleBackColor = true;
		this.cbImageAutoUseJPEG.CheckedChanged += new System.EventHandler(cbImageAutoUseJPEG_CheckedChanged);
		resources.ApplyResources(this.lblImageFormat, "lblImageFormat");
		this.lblImageFormat.Name = "lblImageFormat";
		this.cbImageFileExist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbImageFileExist.FormattingEnabled = true;
		resources.ApplyResources(this.cbImageFileExist, "cbImageFileExist");
		this.cbImageFileExist.Name = "cbImageFileExist";
		this.cbImageFileExist.SelectedIndexChanged += new System.EventHandler(cbImageFileExist_SelectedIndexChanged);
		resources.ApplyResources(this.lblImageFileExist, "lblImageFileExist");
		this.lblImageFileExist.Name = "lblImageFileExist";
		resources.ApplyResources(this.nudImageAutoUseJPEGSize, "nudImageAutoUseJPEGSize");
		this.nudImageAutoUseJPEGSize.Maximum = new decimal(new int[4] { 100000, 0, 0, 0 });
		this.nudImageAutoUseJPEGSize.Minimum = new decimal(new int[4] { 100, 0, 0, 0 });
		this.nudImageAutoUseJPEGSize.Name = "nudImageAutoUseJPEGSize";
		this.nudImageAutoUseJPEGSize.Value = new decimal(new int[4] { 2048, 0, 0, 0 });
		this.nudImageAutoUseJPEGSize.ValueChanged += new System.EventHandler(nudImageAutoUseJPEGSize_ValueChanged);
		resources.ApplyResources(this.lblImageSizeLimitHint, "lblImageSizeLimitHint");
		this.lblImageSizeLimitHint.Name = "lblImageSizeLimitHint";
		resources.ApplyResources(this.nudImageJPEGQuality, "nudImageJPEGQuality");
		this.nudImageJPEGQuality.Name = "nudImageJPEGQuality";
		this.nudImageJPEGQuality.Value = new decimal(new int[4] { 90, 0, 0, 0 });
		this.nudImageJPEGQuality.ValueChanged += new System.EventHandler(nudImageJPEGQuality_ValueChanged);
		this.cbImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbImageFormat.FormattingEnabled = true;
		resources.ApplyResources(this.cbImageFormat, "cbImageFormat");
		this.cbImageFormat.Name = "cbImageFormat";
		this.cbImageFormat.SelectedIndexChanged += new System.EventHandler(cbImageFormat_SelectedIndexChanged);
		resources.ApplyResources(this.lblImageJPEGQualityHint, "lblImageJPEGQualityHint");
		this.lblImageJPEGQualityHint.Name = "lblImageJPEGQualityHint";
		resources.ApplyResources(this.lblImageGIFQuality, "lblImageGIFQuality");
		this.lblImageGIFQuality.Name = "lblImageGIFQuality";
		resources.ApplyResources(this.lblImageJPEGQuality, "lblImageJPEGQuality");
		this.lblImageJPEGQuality.Name = "lblImageJPEGQuality";
		this.cbImageGIFQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbImageGIFQuality.FormattingEnabled = true;
		resources.ApplyResources(this.cbImageGIFQuality, "cbImageGIFQuality");
		this.cbImageGIFQuality.Name = "cbImageGIFQuality";
		this.cbImageGIFQuality.SelectedIndexChanged += new System.EventHandler(cbImageGIFQuality_SelectedIndexChanged);
		resources.ApplyResources(this.cbOverrideImageSettings, "cbOverrideImageSettings");
		this.cbOverrideImageSettings.Checked = true;
		this.cbOverrideImageSettings.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbOverrideImageSettings.Name = "cbOverrideImageSettings";
		this.cbOverrideImageSettings.UseVisualStyleBackColor = true;
		this.cbOverrideImageSettings.CheckedChanged += new System.EventHandler(cbUseDefaultImageSettings_CheckedChanged);
		this.tpEffects.BackColor = System.Drawing.SystemColors.Window;
		this.tpEffects.Controls.Add(this.lblImageEffectsNote);
		this.tpEffects.Controls.Add(this.cbShowImageEffectsWindowAfterCapture);
		this.tpEffects.Controls.Add(this.cbImageEffectOnlyRegionCapture);
		this.tpEffects.Controls.Add(this.btnImageEffects);
		resources.ApplyResources(this.tpEffects, "tpEffects");
		this.tpEffects.Name = "tpEffects";
		resources.ApplyResources(this.lblImageEffectsNote, "lblImageEffectsNote");
		this.lblImageEffectsNote.Name = "lblImageEffectsNote";
		resources.ApplyResources(this.cbShowImageEffectsWindowAfterCapture, "cbShowImageEffectsWindowAfterCapture");
		this.cbShowImageEffectsWindowAfterCapture.Name = "cbShowImageEffectsWindowAfterCapture";
		this.cbShowImageEffectsWindowAfterCapture.UseVisualStyleBackColor = true;
		this.cbShowImageEffectsWindowAfterCapture.CheckedChanged += new System.EventHandler(cbShowImageEffectsWindowAfterCapture_CheckedChanged);
		resources.ApplyResources(this.cbImageEffectOnlyRegionCapture, "cbImageEffectOnlyRegionCapture");
		this.cbImageEffectOnlyRegionCapture.Name = "cbImageEffectOnlyRegionCapture";
		this.cbImageEffectOnlyRegionCapture.UseVisualStyleBackColor = true;
		this.cbImageEffectOnlyRegionCapture.CheckedChanged += new System.EventHandler(cbImageEffectOnlyRegionCapture_CheckedChanged);
		resources.ApplyResources(this.btnImageEffects, "btnImageEffects");
		this.btnImageEffects.Name = "btnImageEffects";
		this.btnImageEffects.UseVisualStyleBackColor = true;
		this.btnImageEffects.Click += new System.EventHandler(btnImageEffects_Click);
		this.tpThumbnail.BackColor = System.Drawing.SystemColors.Window;
		this.tpThumbnail.Controls.Add(this.cbThumbnailIfSmaller);
		this.tpThumbnail.Controls.Add(this.lblThumbnailNamePreview);
		this.tpThumbnail.Controls.Add(this.lblThumbnailName);
		this.tpThumbnail.Controls.Add(this.txtThumbnailName);
		this.tpThumbnail.Controls.Add(this.lblThumbnailHeight);
		this.tpThumbnail.Controls.Add(this.lblThumbnailWidth);
		this.tpThumbnail.Controls.Add(this.nudThumbnailHeight);
		this.tpThumbnail.Controls.Add(this.nudThumbnailWidth);
		resources.ApplyResources(this.tpThumbnail, "tpThumbnail");
		this.tpThumbnail.Name = "tpThumbnail";
		resources.ApplyResources(this.cbThumbnailIfSmaller, "cbThumbnailIfSmaller");
		this.cbThumbnailIfSmaller.Name = "cbThumbnailIfSmaller";
		this.cbThumbnailIfSmaller.UseVisualStyleBackColor = true;
		this.cbThumbnailIfSmaller.CheckedChanged += new System.EventHandler(cbThumbnailIfSmaller_CheckedChanged);
		resources.ApplyResources(this.lblThumbnailNamePreview, "lblThumbnailNamePreview");
		this.lblThumbnailNamePreview.Name = "lblThumbnailNamePreview";
		resources.ApplyResources(this.lblThumbnailName, "lblThumbnailName");
		this.lblThumbnailName.Name = "lblThumbnailName";
		resources.ApplyResources(this.txtThumbnailName, "txtThumbnailName");
		this.txtThumbnailName.Name = "txtThumbnailName";
		this.txtThumbnailName.TextChanged += new System.EventHandler(txtThumbnailName_TextChanged);
		resources.ApplyResources(this.lblThumbnailHeight, "lblThumbnailHeight");
		this.lblThumbnailHeight.Name = "lblThumbnailHeight";
		resources.ApplyResources(this.lblThumbnailWidth, "lblThumbnailWidth");
		this.lblThumbnailWidth.Name = "lblThumbnailWidth";
		resources.ApplyResources(this.nudThumbnailHeight, "nudThumbnailHeight");
		this.nudThumbnailHeight.Maximum = new decimal(new int[4] { 2000, 0, 0, 0 });
		this.nudThumbnailHeight.Name = "nudThumbnailHeight";
		this.nudThumbnailHeight.ValueChanged += new System.EventHandler(nudThumbnailHeight_ValueChanged);
		resources.ApplyResources(this.nudThumbnailWidth, "nudThumbnailWidth");
		this.nudThumbnailWidth.Maximum = new decimal(new int[4] { 2000, 0, 0, 0 });
		this.nudThumbnailWidth.Name = "nudThumbnailWidth";
		this.nudThumbnailWidth.ValueChanged += new System.EventHandler(nudThumbnailWidth_ValueChanged);
		this.tpCapture.BackColor = System.Drawing.SystemColors.Window;
		this.tpCapture.Controls.Add(this.tcCapture);
		resources.ApplyResources(this.tpCapture, "tpCapture");
		this.tpCapture.Name = "tpCapture";
		this.tcCapture.Controls.Add(this.tpCaptureGeneral);
		this.tcCapture.Controls.Add(this.tpRegionCapture);
		this.tcCapture.Controls.Add(this.tpScreenRecorder);
		this.tcCapture.Controls.Add(this.tpOCR);
		resources.ApplyResources(this.tcCapture, "tcCapture");
		this.tcCapture.Name = "tcCapture";
		this.tcCapture.SelectedIndex = 0;
		this.tpCaptureGeneral.BackColor = System.Drawing.SystemColors.Window;
		this.tpCaptureGeneral.Controls.Add(this.pCapture);
		this.tpCaptureGeneral.Controls.Add(this.cbOverrideCaptureSettings);
		resources.ApplyResources(this.tpCaptureGeneral, "tpCaptureGeneral");
		this.tpCaptureGeneral.Name = "tpCaptureGeneral";
		this.pCapture.Controls.Add(this.lblScreenshotDelay);
		this.pCapture.Controls.Add(this.btnCaptureCustomRegionSelectRectangle);
		this.pCapture.Controls.Add(this.lblCaptureCustomRegion);
		this.pCapture.Controls.Add(this.lblCaptureCustomRegionWidth);
		this.pCapture.Controls.Add(this.lblCaptureCustomRegionHeight);
		this.pCapture.Controls.Add(this.lblCaptureCustomRegionY);
		this.pCapture.Controls.Add(this.lblCaptureCustomRegionX);
		this.pCapture.Controls.Add(this.nudCaptureCustomRegionHeight);
		this.pCapture.Controls.Add(this.nudCaptureCustomRegionWidth);
		this.pCapture.Controls.Add(this.nudCaptureCustomRegionY);
		this.pCapture.Controls.Add(this.nudCaptureCustomRegionX);
		this.pCapture.Controls.Add(this.cbShowCursor);
		this.pCapture.Controls.Add(this.lblCaptureShadowOffset);
		this.pCapture.Controls.Add(this.cbCaptureTransparent);
		this.pCapture.Controls.Add(this.cbCaptureAutoHideTaskbar);
		this.pCapture.Controls.Add(this.cbCaptureShadow);
		this.pCapture.Controls.Add(this.lblScreenshotDelayInfo);
		this.pCapture.Controls.Add(this.cbCaptureClientArea);
		this.pCapture.Controls.Add(this.nudScreenshotDelay);
		this.pCapture.Controls.Add(this.nudCaptureShadowOffset);
		resources.ApplyResources(this.pCapture, "pCapture");
		this.pCapture.Name = "pCapture";
		resources.ApplyResources(this.lblScreenshotDelay, "lblScreenshotDelay");
		this.lblScreenshotDelay.Name = "lblScreenshotDelay";
		resources.ApplyResources(this.btnCaptureCustomRegionSelectRectangle, "btnCaptureCustomRegionSelectRectangle");
		this.btnCaptureCustomRegionSelectRectangle.Name = "btnCaptureCustomRegionSelectRectangle";
		this.btnCaptureCustomRegionSelectRectangle.UseVisualStyleBackColor = true;
		this.btnCaptureCustomRegionSelectRectangle.Click += new System.EventHandler(btnCaptureCustomRegionSelectRectangle_Click);
		resources.ApplyResources(this.lblCaptureCustomRegion, "lblCaptureCustomRegion");
		this.lblCaptureCustomRegion.Name = "lblCaptureCustomRegion";
		resources.ApplyResources(this.lblCaptureCustomRegionWidth, "lblCaptureCustomRegionWidth");
		this.lblCaptureCustomRegionWidth.Name = "lblCaptureCustomRegionWidth";
		resources.ApplyResources(this.lblCaptureCustomRegionHeight, "lblCaptureCustomRegionHeight");
		this.lblCaptureCustomRegionHeight.Name = "lblCaptureCustomRegionHeight";
		resources.ApplyResources(this.lblCaptureCustomRegionY, "lblCaptureCustomRegionY");
		this.lblCaptureCustomRegionY.Name = "lblCaptureCustomRegionY";
		resources.ApplyResources(this.lblCaptureCustomRegionX, "lblCaptureCustomRegionX");
		this.lblCaptureCustomRegionX.Name = "lblCaptureCustomRegionX";
		resources.ApplyResources(this.nudCaptureCustomRegionHeight, "nudCaptureCustomRegionHeight");
		this.nudCaptureCustomRegionHeight.Maximum = new decimal(new int[4] { -2147483648, 0, 0, 0 });
		this.nudCaptureCustomRegionHeight.Minimum = new decimal(new int[4] { -2147483648, 0, 0, -2147483648 });
		this.nudCaptureCustomRegionHeight.Name = "nudCaptureCustomRegionHeight";
		this.nudCaptureCustomRegionHeight.ValueChanged += new System.EventHandler(nudScreenRegionHeight_ValueChanged);
		resources.ApplyResources(this.nudCaptureCustomRegionWidth, "nudCaptureCustomRegionWidth");
		this.nudCaptureCustomRegionWidth.Maximum = new decimal(new int[4] { -2147483648, 0, 0, 0 });
		this.nudCaptureCustomRegionWidth.Minimum = new decimal(new int[4] { -2147483648, 0, 0, -2147483648 });
		this.nudCaptureCustomRegionWidth.Name = "nudCaptureCustomRegionWidth";
		this.nudCaptureCustomRegionWidth.ValueChanged += new System.EventHandler(nudScreenRegionWidth_ValueChanged);
		resources.ApplyResources(this.nudCaptureCustomRegionY, "nudCaptureCustomRegionY");
		this.nudCaptureCustomRegionY.Maximum = new decimal(new int[4] { -2147483648, 0, 0, 0 });
		this.nudCaptureCustomRegionY.Minimum = new decimal(new int[4] { -2147483648, 0, 0, -2147483648 });
		this.nudCaptureCustomRegionY.Name = "nudCaptureCustomRegionY";
		this.nudCaptureCustomRegionY.ValueChanged += new System.EventHandler(nudScreenRegionY_ValueChanged);
		resources.ApplyResources(this.nudCaptureCustomRegionX, "nudCaptureCustomRegionX");
		this.nudCaptureCustomRegionX.Maximum = new decimal(new int[4] { -2147483648, 0, 0, 0 });
		this.nudCaptureCustomRegionX.Minimum = new decimal(new int[4] { -2147483648, 0, 0, -2147483648 });
		this.nudCaptureCustomRegionX.Name = "nudCaptureCustomRegionX";
		this.nudCaptureCustomRegionX.ValueChanged += new System.EventHandler(nudScreenRegionX_ValueChanged);
		resources.ApplyResources(this.cbShowCursor, "cbShowCursor");
		this.cbShowCursor.Name = "cbShowCursor";
		this.cbShowCursor.UseVisualStyleBackColor = true;
		this.cbShowCursor.CheckedChanged += new System.EventHandler(cbShowCursor_CheckedChanged);
		resources.ApplyResources(this.lblCaptureShadowOffset, "lblCaptureShadowOffset");
		this.lblCaptureShadowOffset.Name = "lblCaptureShadowOffset";
		resources.ApplyResources(this.cbCaptureTransparent, "cbCaptureTransparent");
		this.cbCaptureTransparent.Name = "cbCaptureTransparent";
		this.cbCaptureTransparent.UseVisualStyleBackColor = true;
		this.cbCaptureTransparent.CheckedChanged += new System.EventHandler(cbCaptureTransparent_CheckedChanged);
		resources.ApplyResources(this.cbCaptureAutoHideTaskbar, "cbCaptureAutoHideTaskbar");
		this.cbCaptureAutoHideTaskbar.Name = "cbCaptureAutoHideTaskbar";
		this.cbCaptureAutoHideTaskbar.UseVisualStyleBackColor = true;
		this.cbCaptureAutoHideTaskbar.CheckedChanged += new System.EventHandler(cbCaptureAutoHideTaskbar_CheckedChanged);
		resources.ApplyResources(this.cbCaptureShadow, "cbCaptureShadow");
		this.cbCaptureShadow.Name = "cbCaptureShadow";
		this.cbCaptureShadow.UseVisualStyleBackColor = true;
		this.cbCaptureShadow.CheckedChanged += new System.EventHandler(cbCaptureShadow_CheckedChanged);
		resources.ApplyResources(this.lblScreenshotDelayInfo, "lblScreenshotDelayInfo");
		this.lblScreenshotDelayInfo.Name = "lblScreenshotDelayInfo";
		resources.ApplyResources(this.cbCaptureClientArea, "cbCaptureClientArea");
		this.cbCaptureClientArea.Name = "cbCaptureClientArea";
		this.cbCaptureClientArea.UseVisualStyleBackColor = true;
		this.cbCaptureClientArea.CheckedChanged += new System.EventHandler(cbCaptureClientArea_CheckedChanged);
		this.nudScreenshotDelay.DecimalPlaces = 1;
		resources.ApplyResources(this.nudScreenshotDelay, "nudScreenshotDelay");
		this.nudScreenshotDelay.Maximum = new decimal(new int[4] { 300, 0, 0, 0 });
		this.nudScreenshotDelay.Name = "nudScreenshotDelay";
		this.nudScreenshotDelay.ValueChanged += new System.EventHandler(nudScreenshotDelay_ValueChanged);
		resources.ApplyResources(this.nudCaptureShadowOffset, "nudCaptureShadowOffset");
		this.nudCaptureShadowOffset.Maximum = new decimal(new int[4] { 200, 0, 0, 0 });
		this.nudCaptureShadowOffset.Name = "nudCaptureShadowOffset";
		this.nudCaptureShadowOffset.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		this.nudCaptureShadowOffset.ValueChanged += new System.EventHandler(nudCaptureShadowOffset_ValueChanged);
		resources.ApplyResources(this.cbOverrideCaptureSettings, "cbOverrideCaptureSettings");
		this.cbOverrideCaptureSettings.Checked = true;
		this.cbOverrideCaptureSettings.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbOverrideCaptureSettings.Name = "cbOverrideCaptureSettings";
		this.cbOverrideCaptureSettings.UseVisualStyleBackColor = true;
		this.cbOverrideCaptureSettings.CheckedChanged += new System.EventHandler(cbUseDefaultCaptureSettings_CheckedChanged);
		this.tpRegionCapture.BackColor = System.Drawing.SystemColors.Window;
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureActiveMonitorMode);
		this.tpRegionCapture.Controls.Add(this.nudRegionCaptureFPSLimit);
		this.tpRegionCapture.Controls.Add(this.lblRegionCaptureFPSLimit);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureShowFPS);
		this.tpRegionCapture.Controls.Add(this.flpRegionCaptureFixedSize);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureIsFixedSize);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureShowCrosshair);
		this.tpRegionCapture.Controls.Add(this.lblRegionCaptureMagnifierPixelSize);
		this.tpRegionCapture.Controls.Add(this.lblRegionCaptureMagnifierPixelCount);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureUseSquareMagnifier);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureShowMagnifier);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureShowInfo);
		this.tpRegionCapture.Controls.Add(this.btnRegionCaptureSnapSizesRemove);
		this.tpRegionCapture.Controls.Add(this.btnRegionCaptureSnapSizesAdd);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureSnapSizes);
		this.tpRegionCapture.Controls.Add(this.lblRegionCaptureSnapSizes);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureUseCustomInfoText);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureDetectControls);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureDetectWindows);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureMouse5ClickAction);
		this.tpRegionCapture.Controls.Add(this.lblRegionCaptureMouse5ClickAction);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureMouse4ClickAction);
		this.tpRegionCapture.Controls.Add(this.lblRegionCaptureMouse4ClickAction);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureMouseMiddleClickAction);
		this.tpRegionCapture.Controls.Add(this.lblRegionCaptureMouseMiddleClickAction);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureMouseRightClickAction);
		this.tpRegionCapture.Controls.Add(this.lblRegionCaptureMouseRightClickAction);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureMultiRegionMode);
		this.tpRegionCapture.Controls.Add(this.pRegionCaptureSnapSizes);
		this.tpRegionCapture.Controls.Add(this.cbRegionCaptureUseDimming);
		this.tpRegionCapture.Controls.Add(this.txtRegionCaptureCustomInfoText);
		this.tpRegionCapture.Controls.Add(this.nudRegionCaptureMagnifierPixelCount);
		this.tpRegionCapture.Controls.Add(this.nudRegionCaptureMagnifierPixelSize);
		resources.ApplyResources(this.tpRegionCapture, "tpRegionCapture");
		this.tpRegionCapture.Name = "tpRegionCapture";
		resources.ApplyResources(this.cbRegionCaptureActiveMonitorMode, "cbRegionCaptureActiveMonitorMode");
		this.cbRegionCaptureActiveMonitorMode.Name = "cbRegionCaptureActiveMonitorMode";
		this.cbRegionCaptureActiveMonitorMode.UseVisualStyleBackColor = true;
		this.cbRegionCaptureActiveMonitorMode.CheckedChanged += new System.EventHandler(cbRegionCaptureActiveMonitorMode_CheckedChanged);
		resources.ApplyResources(this.nudRegionCaptureFPSLimit, "nudRegionCaptureFPSLimit");
		this.nudRegionCaptureFPSLimit.Maximum = new decimal(new int[4] { 300, 0, 0, 0 });
		this.nudRegionCaptureFPSLimit.Name = "nudRegionCaptureFPSLimit";
		this.nudRegionCaptureFPSLimit.ValueChanged += new System.EventHandler(nudRegionCaptureFPSLimit_ValueChanged);
		resources.ApplyResources(this.lblRegionCaptureFPSLimit, "lblRegionCaptureFPSLimit");
		this.lblRegionCaptureFPSLimit.Name = "lblRegionCaptureFPSLimit";
		resources.ApplyResources(this.cbRegionCaptureShowFPS, "cbRegionCaptureShowFPS");
		this.cbRegionCaptureShowFPS.Name = "cbRegionCaptureShowFPS";
		this.cbRegionCaptureShowFPS.UseVisualStyleBackColor = true;
		this.cbRegionCaptureShowFPS.CheckedChanged += new System.EventHandler(cbRegionCaptureShowFPS_CheckedChanged);
		resources.ApplyResources(this.flpRegionCaptureFixedSize, "flpRegionCaptureFixedSize");
		this.flpRegionCaptureFixedSize.Controls.Add(this.lblRegionCaptureFixedSizeWidth);
		this.flpRegionCaptureFixedSize.Controls.Add(this.nudRegionCaptureFixedSizeWidth);
		this.flpRegionCaptureFixedSize.Controls.Add(this.lblRegionCaptureFixedSizeHeight);
		this.flpRegionCaptureFixedSize.Controls.Add(this.nudRegionCaptureFixedSizeHeight);
		this.flpRegionCaptureFixedSize.Name = "flpRegionCaptureFixedSize";
		resources.ApplyResources(this.lblRegionCaptureFixedSizeWidth, "lblRegionCaptureFixedSizeWidth");
		this.lblRegionCaptureFixedSizeWidth.Name = "lblRegionCaptureFixedSizeWidth";
		this.nudRegionCaptureFixedSizeWidth.Increment = new decimal(new int[4] { 10, 0, 0, 0 });
		resources.ApplyResources(this.nudRegionCaptureFixedSizeWidth, "nudRegionCaptureFixedSizeWidth");
		this.nudRegionCaptureFixedSizeWidth.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.nudRegionCaptureFixedSizeWidth.Minimum = new decimal(new int[4] { 10, 0, 0, 0 });
		this.nudRegionCaptureFixedSizeWidth.Name = "nudRegionCaptureFixedSizeWidth";
		this.nudRegionCaptureFixedSizeWidth.Value = new decimal(new int[4] { 10, 0, 0, 0 });
		this.nudRegionCaptureFixedSizeWidth.ValueChanged += new System.EventHandler(nudRegionCaptureFixedSizeWidth_ValueChanged);
		resources.ApplyResources(this.lblRegionCaptureFixedSizeHeight, "lblRegionCaptureFixedSizeHeight");
		this.lblRegionCaptureFixedSizeHeight.Name = "lblRegionCaptureFixedSizeHeight";
		this.nudRegionCaptureFixedSizeHeight.Increment = new decimal(new int[4] { 10, 0, 0, 0 });
		resources.ApplyResources(this.nudRegionCaptureFixedSizeHeight, "nudRegionCaptureFixedSizeHeight");
		this.nudRegionCaptureFixedSizeHeight.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.nudRegionCaptureFixedSizeHeight.Minimum = new decimal(new int[4] { 10, 0, 0, 0 });
		this.nudRegionCaptureFixedSizeHeight.Name = "nudRegionCaptureFixedSizeHeight";
		this.nudRegionCaptureFixedSizeHeight.Value = new decimal(new int[4] { 10, 0, 0, 0 });
		this.nudRegionCaptureFixedSizeHeight.ValueChanged += new System.EventHandler(nudRegionCaptureFixedSizeHeight_ValueChanged);
		resources.ApplyResources(this.cbRegionCaptureIsFixedSize, "cbRegionCaptureIsFixedSize");
		this.cbRegionCaptureIsFixedSize.Name = "cbRegionCaptureIsFixedSize";
		this.cbRegionCaptureIsFixedSize.UseVisualStyleBackColor = true;
		this.cbRegionCaptureIsFixedSize.CheckedChanged += new System.EventHandler(cbRegionCaptureIsFixedSize_CheckedChanged);
		resources.ApplyResources(this.cbRegionCaptureShowCrosshair, "cbRegionCaptureShowCrosshair");
		this.cbRegionCaptureShowCrosshair.Name = "cbRegionCaptureShowCrosshair";
		this.cbRegionCaptureShowCrosshair.UseVisualStyleBackColor = true;
		this.cbRegionCaptureShowCrosshair.CheckedChanged += new System.EventHandler(cbRegionCaptureShowCrosshair_CheckedChanged);
		resources.ApplyResources(this.lblRegionCaptureMagnifierPixelSize, "lblRegionCaptureMagnifierPixelSize");
		this.lblRegionCaptureMagnifierPixelSize.Name = "lblRegionCaptureMagnifierPixelSize";
		resources.ApplyResources(this.lblRegionCaptureMagnifierPixelCount, "lblRegionCaptureMagnifierPixelCount");
		this.lblRegionCaptureMagnifierPixelCount.Name = "lblRegionCaptureMagnifierPixelCount";
		resources.ApplyResources(this.cbRegionCaptureUseSquareMagnifier, "cbRegionCaptureUseSquareMagnifier");
		this.cbRegionCaptureUseSquareMagnifier.Name = "cbRegionCaptureUseSquareMagnifier";
		this.cbRegionCaptureUseSquareMagnifier.UseVisualStyleBackColor = true;
		this.cbRegionCaptureUseSquareMagnifier.CheckedChanged += new System.EventHandler(cbRegionCaptureUseSquareMagnifier_CheckedChanged);
		resources.ApplyResources(this.cbRegionCaptureShowMagnifier, "cbRegionCaptureShowMagnifier");
		this.cbRegionCaptureShowMagnifier.Name = "cbRegionCaptureShowMagnifier";
		this.cbRegionCaptureShowMagnifier.UseVisualStyleBackColor = true;
		this.cbRegionCaptureShowMagnifier.CheckedChanged += new System.EventHandler(cbRegionCaptureShowMagnifier_CheckedChanged);
		resources.ApplyResources(this.cbRegionCaptureShowInfo, "cbRegionCaptureShowInfo");
		this.cbRegionCaptureShowInfo.Name = "cbRegionCaptureShowInfo";
		this.cbRegionCaptureShowInfo.UseVisualStyleBackColor = true;
		this.cbRegionCaptureShowInfo.CheckedChanged += new System.EventHandler(cbRegionCaptureShowInfo_CheckedChanged);
		resources.ApplyResources(this.btnRegionCaptureSnapSizesRemove, "btnRegionCaptureSnapSizesRemove");
		this.btnRegionCaptureSnapSizesRemove.Name = "btnRegionCaptureSnapSizesRemove";
		this.btnRegionCaptureSnapSizesRemove.UseVisualStyleBackColor = true;
		this.btnRegionCaptureSnapSizesRemove.Click += new System.EventHandler(btnRegionCaptureSnapSizesRemove_Click);
		resources.ApplyResources(this.btnRegionCaptureSnapSizesAdd, "btnRegionCaptureSnapSizesAdd");
		this.btnRegionCaptureSnapSizesAdd.Name = "btnRegionCaptureSnapSizesAdd";
		this.btnRegionCaptureSnapSizesAdd.UseVisualStyleBackColor = true;
		this.btnRegionCaptureSnapSizesAdd.Click += new System.EventHandler(btnRegionCaptureSnapSizesAdd_Click);
		this.cbRegionCaptureSnapSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbRegionCaptureSnapSizes.FormattingEnabled = true;
		resources.ApplyResources(this.cbRegionCaptureSnapSizes, "cbRegionCaptureSnapSizes");
		this.cbRegionCaptureSnapSizes.Name = "cbRegionCaptureSnapSizes";
		resources.ApplyResources(this.lblRegionCaptureSnapSizes, "lblRegionCaptureSnapSizes");
		this.lblRegionCaptureSnapSizes.Name = "lblRegionCaptureSnapSizes";
		resources.ApplyResources(this.cbRegionCaptureUseCustomInfoText, "cbRegionCaptureUseCustomInfoText");
		this.cbRegionCaptureUseCustomInfoText.Name = "cbRegionCaptureUseCustomInfoText";
		this.cbRegionCaptureUseCustomInfoText.UseVisualStyleBackColor = true;
		this.cbRegionCaptureUseCustomInfoText.CheckedChanged += new System.EventHandler(cbRegionCaptureUseCustomInfoText_CheckedChanged);
		resources.ApplyResources(this.cbRegionCaptureDetectControls, "cbRegionCaptureDetectControls");
		this.cbRegionCaptureDetectControls.Name = "cbRegionCaptureDetectControls";
		this.cbRegionCaptureDetectControls.UseVisualStyleBackColor = true;
		this.cbRegionCaptureDetectControls.CheckedChanged += new System.EventHandler(cbRegionCaptureDetectControls_CheckedChanged);
		resources.ApplyResources(this.cbRegionCaptureDetectWindows, "cbRegionCaptureDetectWindows");
		this.cbRegionCaptureDetectWindows.Name = "cbRegionCaptureDetectWindows";
		this.cbRegionCaptureDetectWindows.UseVisualStyleBackColor = true;
		this.cbRegionCaptureDetectWindows.CheckedChanged += new System.EventHandler(cbRegionCaptureDetectWindows_CheckedChanged);
		this.cbRegionCaptureMouse5ClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbRegionCaptureMouse5ClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbRegionCaptureMouse5ClickAction, "cbRegionCaptureMouse5ClickAction");
		this.cbRegionCaptureMouse5ClickAction.Name = "cbRegionCaptureMouse5ClickAction";
		this.cbRegionCaptureMouse5ClickAction.SelectedIndexChanged += new System.EventHandler(cbRegionCaptureMouse5ClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.lblRegionCaptureMouse5ClickAction, "lblRegionCaptureMouse5ClickAction");
		this.lblRegionCaptureMouse5ClickAction.Name = "lblRegionCaptureMouse5ClickAction";
		this.cbRegionCaptureMouse4ClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbRegionCaptureMouse4ClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbRegionCaptureMouse4ClickAction, "cbRegionCaptureMouse4ClickAction");
		this.cbRegionCaptureMouse4ClickAction.Name = "cbRegionCaptureMouse4ClickAction";
		this.cbRegionCaptureMouse4ClickAction.SelectedIndexChanged += new System.EventHandler(cbRegionCaptureMouse4ClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.lblRegionCaptureMouse4ClickAction, "lblRegionCaptureMouse4ClickAction");
		this.lblRegionCaptureMouse4ClickAction.Name = "lblRegionCaptureMouse4ClickAction";
		this.cbRegionCaptureMouseMiddleClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbRegionCaptureMouseMiddleClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbRegionCaptureMouseMiddleClickAction, "cbRegionCaptureMouseMiddleClickAction");
		this.cbRegionCaptureMouseMiddleClickAction.Name = "cbRegionCaptureMouseMiddleClickAction";
		this.cbRegionCaptureMouseMiddleClickAction.SelectedIndexChanged += new System.EventHandler(cbRegionCaptureMouseMiddleClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.lblRegionCaptureMouseMiddleClickAction, "lblRegionCaptureMouseMiddleClickAction");
		this.lblRegionCaptureMouseMiddleClickAction.Name = "lblRegionCaptureMouseMiddleClickAction";
		this.cbRegionCaptureMouseRightClickAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbRegionCaptureMouseRightClickAction.FormattingEnabled = true;
		resources.ApplyResources(this.cbRegionCaptureMouseRightClickAction, "cbRegionCaptureMouseRightClickAction");
		this.cbRegionCaptureMouseRightClickAction.Name = "cbRegionCaptureMouseRightClickAction";
		this.cbRegionCaptureMouseRightClickAction.SelectedIndexChanged += new System.EventHandler(cbRegionCaptureMouseRightClickAction_SelectedIndexChanged);
		resources.ApplyResources(this.lblRegionCaptureMouseRightClickAction, "lblRegionCaptureMouseRightClickAction");
		this.lblRegionCaptureMouseRightClickAction.Name = "lblRegionCaptureMouseRightClickAction";
		resources.ApplyResources(this.cbRegionCaptureMultiRegionMode, "cbRegionCaptureMultiRegionMode");
		this.cbRegionCaptureMultiRegionMode.Name = "cbRegionCaptureMultiRegionMode";
		this.cbRegionCaptureMultiRegionMode.UseVisualStyleBackColor = true;
		this.cbRegionCaptureMultiRegionMode.CheckedChanged += new System.EventHandler(cbRegionCaptureMultiRegionMode_CheckedChanged);
		this.pRegionCaptureSnapSizes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pRegionCaptureSnapSizes.Controls.Add(this.btnRegionCaptureSnapSizesDialogCancel);
		this.pRegionCaptureSnapSizes.Controls.Add(this.btnRegionCaptureSnapSizesDialogAdd);
		this.pRegionCaptureSnapSizes.Controls.Add(this.nudRegionCaptureSnapSizesHeight);
		this.pRegionCaptureSnapSizes.Controls.Add(this.RegionCaptureSnapSizesHeight);
		this.pRegionCaptureSnapSizes.Controls.Add(this.nudRegionCaptureSnapSizesWidth);
		this.pRegionCaptureSnapSizes.Controls.Add(this.lblRegionCaptureSnapSizesWidth);
		resources.ApplyResources(this.pRegionCaptureSnapSizes, "pRegionCaptureSnapSizes");
		this.pRegionCaptureSnapSizes.Name = "pRegionCaptureSnapSizes";
		resources.ApplyResources(this.btnRegionCaptureSnapSizesDialogCancel, "btnRegionCaptureSnapSizesDialogCancel");
		this.btnRegionCaptureSnapSizesDialogCancel.Name = "btnRegionCaptureSnapSizesDialogCancel";
		this.btnRegionCaptureSnapSizesDialogCancel.UseVisualStyleBackColor = true;
		this.btnRegionCaptureSnapSizesDialogCancel.Click += new System.EventHandler(btnRegionCaptureSnapSizesDialogCancel_Click);
		resources.ApplyResources(this.btnRegionCaptureSnapSizesDialogAdd, "btnRegionCaptureSnapSizesDialogAdd");
		this.btnRegionCaptureSnapSizesDialogAdd.Name = "btnRegionCaptureSnapSizesDialogAdd";
		this.btnRegionCaptureSnapSizesDialogAdd.UseVisualStyleBackColor = true;
		this.btnRegionCaptureSnapSizesDialogAdd.Click += new System.EventHandler(btnRegionCaptureSnapSizesDialogAdd_Click);
		resources.ApplyResources(this.nudRegionCaptureSnapSizesHeight, "nudRegionCaptureSnapSizesHeight");
		this.nudRegionCaptureSnapSizesHeight.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.nudRegionCaptureSnapSizesHeight.Minimum = new decimal(new int[4] { 2, 0, 0, 0 });
		this.nudRegionCaptureSnapSizesHeight.Name = "nudRegionCaptureSnapSizesHeight";
		this.nudRegionCaptureSnapSizesHeight.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		resources.ApplyResources(this.RegionCaptureSnapSizesHeight, "RegionCaptureSnapSizesHeight");
		this.RegionCaptureSnapSizesHeight.Name = "RegionCaptureSnapSizesHeight";
		resources.ApplyResources(this.nudRegionCaptureSnapSizesWidth, "nudRegionCaptureSnapSizesWidth");
		this.nudRegionCaptureSnapSizesWidth.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.nudRegionCaptureSnapSizesWidth.Minimum = new decimal(new int[4] { 2, 0, 0, 0 });
		this.nudRegionCaptureSnapSizesWidth.Name = "nudRegionCaptureSnapSizesWidth";
		this.nudRegionCaptureSnapSizesWidth.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		resources.ApplyResources(this.lblRegionCaptureSnapSizesWidth, "lblRegionCaptureSnapSizesWidth");
		this.lblRegionCaptureSnapSizesWidth.Name = "lblRegionCaptureSnapSizesWidth";
		resources.ApplyResources(this.cbRegionCaptureUseDimming, "cbRegionCaptureUseDimming");
		this.cbRegionCaptureUseDimming.Name = "cbRegionCaptureUseDimming";
		this.cbRegionCaptureUseDimming.UseVisualStyleBackColor = true;
		this.cbRegionCaptureUseDimming.CheckedChanged += new System.EventHandler(cbRegionCaptureUseDimming_CheckedChanged);
		resources.ApplyResources(this.txtRegionCaptureCustomInfoText, "txtRegionCaptureCustomInfoText");
		this.txtRegionCaptureCustomInfoText.Name = "txtRegionCaptureCustomInfoText";
		this.txtRegionCaptureCustomInfoText.TextChanged += new System.EventHandler(txtRegionCaptureCustomInfoText_TextChanged);
		this.nudRegionCaptureMagnifierPixelCount.Increment = new decimal(new int[4] { 2, 0, 0, 0 });
		resources.ApplyResources(this.nudRegionCaptureMagnifierPixelCount, "nudRegionCaptureMagnifierPixelCount");
		this.nudRegionCaptureMagnifierPixelCount.Name = "nudRegionCaptureMagnifierPixelCount";
		this.nudRegionCaptureMagnifierPixelCount.ValueChanged += new System.EventHandler(nudRegionCaptureMagnifierPixelCount_ValueChanged);
		resources.ApplyResources(this.nudRegionCaptureMagnifierPixelSize, "nudRegionCaptureMagnifierPixelSize");
		this.nudRegionCaptureMagnifierPixelSize.Name = "nudRegionCaptureMagnifierPixelSize";
		this.nudRegionCaptureMagnifierPixelSize.ValueChanged += new System.EventHandler(nudRegionCaptureMagnifierPixelSize_ValueChanged);
		this.tpScreenRecorder.BackColor = System.Drawing.SystemColors.Window;
		this.tpScreenRecorder.Controls.Add(this.cbScreenRecordTransparentRegion);
		this.tpScreenRecorder.Controls.Add(this.cbScreenRecordTwoPassEncoding);
		this.tpScreenRecorder.Controls.Add(this.cbScreenRecordConfirmAbort);
		this.tpScreenRecorder.Controls.Add(this.cbScreenRecorderShowCursor);
		this.tpScreenRecorder.Controls.Add(this.btnScreenRecorderFFmpegOptions);
		this.tpScreenRecorder.Controls.Add(this.lblScreenRecorderStartDelay);
		this.tpScreenRecorder.Controls.Add(this.cbScreenRecordAutoStart);
		this.tpScreenRecorder.Controls.Add(this.lblScreenRecorderFixedDuration);
		this.tpScreenRecorder.Controls.Add(this.nudScreenRecordFPS);
		this.tpScreenRecorder.Controls.Add(this.lblScreenRecordFPS);
		this.tpScreenRecorder.Controls.Add(this.nudScreenRecorderDuration);
		this.tpScreenRecorder.Controls.Add(this.nudScreenRecorderStartDelay);
		this.tpScreenRecorder.Controls.Add(this.cbScreenRecorderFixedDuration);
		this.tpScreenRecorder.Controls.Add(this.nudGIFFPS);
		this.tpScreenRecorder.Controls.Add(this.lblGIFFPS);
		resources.ApplyResources(this.tpScreenRecorder, "tpScreenRecorder");
		this.tpScreenRecorder.Name = "tpScreenRecorder";
		resources.ApplyResources(this.cbScreenRecordTransparentRegion, "cbScreenRecordTransparentRegion");
		this.cbScreenRecordTransparentRegion.Name = "cbScreenRecordTransparentRegion";
		this.cbScreenRecordTransparentRegion.UseVisualStyleBackColor = true;
		this.cbScreenRecordTransparentRegion.CheckedChanged += new System.EventHandler(cbScreenRecordTransparentRegion_CheckedChanged);
		resources.ApplyResources(this.cbScreenRecordTwoPassEncoding, "cbScreenRecordTwoPassEncoding");
		this.cbScreenRecordTwoPassEncoding.Name = "cbScreenRecordTwoPassEncoding";
		this.cbScreenRecordTwoPassEncoding.UseVisualStyleBackColor = true;
		this.cbScreenRecordTwoPassEncoding.CheckedChanged += new System.EventHandler(cbScreenRecordTwoPassEncoding_CheckedChanged);
		resources.ApplyResources(this.cbScreenRecordConfirmAbort, "cbScreenRecordConfirmAbort");
		this.cbScreenRecordConfirmAbort.Name = "cbScreenRecordConfirmAbort";
		this.cbScreenRecordConfirmAbort.UseVisualStyleBackColor = true;
		this.cbScreenRecordConfirmAbort.CheckedChanged += new System.EventHandler(cbScreenRecordConfirmAbort_CheckedChanged);
		resources.ApplyResources(this.cbScreenRecorderShowCursor, "cbScreenRecorderShowCursor");
		this.cbScreenRecorderShowCursor.Name = "cbScreenRecorderShowCursor";
		this.cbScreenRecorderShowCursor.UseVisualStyleBackColor = true;
		this.cbScreenRecorderShowCursor.CheckedChanged += new System.EventHandler(cbScreenRecorderShowCursor_CheckedChanged);
		resources.ApplyResources(this.btnScreenRecorderFFmpegOptions, "btnScreenRecorderFFmpegOptions");
		this.btnScreenRecorderFFmpegOptions.Name = "btnScreenRecorderFFmpegOptions";
		this.btnScreenRecorderFFmpegOptions.UseVisualStyleBackColor = true;
		this.btnScreenRecorderFFmpegOptions.Click += new System.EventHandler(btnScreenRecorderFFmpegOptions_Click);
		resources.ApplyResources(this.lblScreenRecorderStartDelay, "lblScreenRecorderStartDelay");
		this.lblScreenRecorderStartDelay.Name = "lblScreenRecorderStartDelay";
		resources.ApplyResources(this.cbScreenRecordAutoStart, "cbScreenRecordAutoStart");
		this.cbScreenRecordAutoStart.Name = "cbScreenRecordAutoStart";
		this.cbScreenRecordAutoStart.UseVisualStyleBackColor = true;
		this.cbScreenRecordAutoStart.CheckedChanged += new System.EventHandler(cbScreenRecordAutoStart_CheckedChanged);
		resources.ApplyResources(this.lblScreenRecorderFixedDuration, "lblScreenRecorderFixedDuration");
		this.lblScreenRecorderFixedDuration.Name = "lblScreenRecorderFixedDuration";
		resources.ApplyResources(this.nudScreenRecordFPS, "nudScreenRecordFPS");
		this.nudScreenRecordFPS.Maximum = new decimal(new int[4] { 60, 0, 0, 0 });
		this.nudScreenRecordFPS.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudScreenRecordFPS.Name = "nudScreenRecordFPS";
		this.nudScreenRecordFPS.Value = new decimal(new int[4] { 20, 0, 0, 0 });
		this.nudScreenRecordFPS.ValueChanged += new System.EventHandler(nudScreenRecordFPS_ValueChanged);
		resources.ApplyResources(this.lblScreenRecordFPS, "lblScreenRecordFPS");
		this.lblScreenRecordFPS.Name = "lblScreenRecordFPS";
		this.nudScreenRecorderDuration.DecimalPlaces = 1;
		this.nudScreenRecorderDuration.Increment = new decimal(new int[4] { 5, 0, 0, 65536 });
		resources.ApplyResources(this.nudScreenRecorderDuration, "nudScreenRecorderDuration");
		this.nudScreenRecorderDuration.Maximum = new decimal(new int[4] { 60, 0, 0, 0 });
		this.nudScreenRecorderDuration.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudScreenRecorderDuration.Name = "nudScreenRecorderDuration";
		this.nudScreenRecorderDuration.Value = new decimal(new int[4] { 3, 0, 0, 0 });
		this.nudScreenRecorderDuration.ValueChanged += new System.EventHandler(nudScreenRecorderDuration_ValueChanged);
		this.nudScreenRecorderStartDelay.DecimalPlaces = 1;
		this.nudScreenRecorderStartDelay.Increment = new decimal(new int[4] { 5, 0, 0, 65536 });
		resources.ApplyResources(this.nudScreenRecorderStartDelay, "nudScreenRecorderStartDelay");
		this.nudScreenRecorderStartDelay.Maximum = new decimal(new int[4] { 60, 0, 0, 0 });
		this.nudScreenRecorderStartDelay.Name = "nudScreenRecorderStartDelay";
		this.nudScreenRecorderStartDelay.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudScreenRecorderStartDelay.ValueChanged += new System.EventHandler(nudScreenRecorderStartDelay_ValueChanged);
		resources.ApplyResources(this.cbScreenRecorderFixedDuration, "cbScreenRecorderFixedDuration");
		this.cbScreenRecorderFixedDuration.Name = "cbScreenRecorderFixedDuration";
		this.cbScreenRecorderFixedDuration.UseVisualStyleBackColor = true;
		this.cbScreenRecorderFixedDuration.CheckedChanged += new System.EventHandler(cbScreenRecorderFixedDuration_CheckedChanged);
		resources.ApplyResources(this.nudGIFFPS, "nudGIFFPS");
		this.nudGIFFPS.Maximum = new decimal(new int[4] { 30, 0, 0, 0 });
		this.nudGIFFPS.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.nudGIFFPS.Name = "nudGIFFPS";
		this.nudGIFFPS.Value = new decimal(new int[4] { 5, 0, 0, 0 });
		this.nudGIFFPS.ValueChanged += new System.EventHandler(nudGIFFPS_ValueChanged);
		resources.ApplyResources(this.lblGIFFPS, "lblGIFFPS");
		this.lblGIFFPS.Name = "lblGIFFPS";
		this.tpOCR.Controls.Add(this.btnCaptureOCRHelp);
		this.tpOCR.Controls.Add(this.cbCaptureOCRAutoCopy);
		this.tpOCR.Controls.Add(this.cbCaptureOCRSilent);
		this.tpOCR.Controls.Add(this.lblOCRDefaultLanguage);
		this.tpOCR.Controls.Add(this.cbCaptureOCRDefaultLanguage);
		resources.ApplyResources(this.tpOCR, "tpOCR");
		this.tpOCR.Name = "tpOCR";
		this.tpOCR.UseVisualStyleBackColor = true;
		this.btnCaptureOCRHelp.Image = ShareX.Properties.Resources.question;
		resources.ApplyResources(this.btnCaptureOCRHelp, "btnCaptureOCRHelp");
		this.btnCaptureOCRHelp.Name = "btnCaptureOCRHelp";
		this.btnCaptureOCRHelp.UseVisualStyleBackColor = true;
		this.btnCaptureOCRHelp.Click += new System.EventHandler(btnCaptureOCRHelp_Click);
		resources.ApplyResources(this.cbCaptureOCRAutoCopy, "cbCaptureOCRAutoCopy");
		this.cbCaptureOCRAutoCopy.Name = "cbCaptureOCRAutoCopy";
		this.cbCaptureOCRAutoCopy.UseVisualStyleBackColor = true;
		this.cbCaptureOCRAutoCopy.CheckedChanged += new System.EventHandler(cbCaptureOCRAutoCopy_CheckedChanged);
		resources.ApplyResources(this.cbCaptureOCRSilent, "cbCaptureOCRSilent");
		this.cbCaptureOCRSilent.Name = "cbCaptureOCRSilent";
		this.cbCaptureOCRSilent.UseVisualStyleBackColor = true;
		this.cbCaptureOCRSilent.CheckedChanged += new System.EventHandler(cbCaptureOCRSilent_CheckedChanged);
		resources.ApplyResources(this.lblOCRDefaultLanguage, "lblOCRDefaultLanguage");
		this.lblOCRDefaultLanguage.Name = "lblOCRDefaultLanguage";
		this.cbCaptureOCRDefaultLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbCaptureOCRDefaultLanguage.FormattingEnabled = true;
		resources.ApplyResources(this.cbCaptureOCRDefaultLanguage, "cbCaptureOCRDefaultLanguage");
		this.cbCaptureOCRDefaultLanguage.Name = "cbCaptureOCRDefaultLanguage";
		this.cbCaptureOCRDefaultLanguage.SelectedIndexChanged += new System.EventHandler(cbCaptureOCRDefaultLanguage_SelectedIndexChanged);
		this.tpUpload.BackColor = System.Drawing.SystemColors.Window;
		this.tpUpload.Controls.Add(this.tcUpload);
		resources.ApplyResources(this.tpUpload, "tpUpload");
		this.tpUpload.Name = "tpUpload";
		this.tcUpload.Controls.Add(this.tpUploadMain);
		this.tcUpload.Controls.Add(this.tpFileNaming);
		this.tcUpload.Controls.Add(this.tpUploadClipboard);
		this.tcUpload.Controls.Add(this.tpUploaderFilters);
		resources.ApplyResources(this.tcUpload, "tcUpload");
		this.tcUpload.Name = "tcUpload";
		this.tcUpload.SelectedIndex = 0;
		this.tpUploadMain.BackColor = System.Drawing.SystemColors.Window;
		this.tpUploadMain.Controls.Add(this.cbOverrideUploadSettings);
		resources.ApplyResources(this.tpUploadMain, "tpUploadMain");
		this.tpUploadMain.Name = "tpUploadMain";
		resources.ApplyResources(this.cbOverrideUploadSettings, "cbOverrideUploadSettings");
		this.cbOverrideUploadSettings.Checked = true;
		this.cbOverrideUploadSettings.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbOverrideUploadSettings.Name = "cbOverrideUploadSettings";
		this.cbOverrideUploadSettings.UseVisualStyleBackColor = true;
		this.cbOverrideUploadSettings.CheckedChanged += new System.EventHandler(cbUseDefaultUploadSettings_CheckedChanged);
		this.tpFileNaming.BackColor = System.Drawing.SystemColors.Window;
		this.tpFileNaming.Controls.Add(this.txtURLRegexReplaceReplacement);
		this.tpFileNaming.Controls.Add(this.lblURLRegexReplaceReplacement);
		this.tpFileNaming.Controls.Add(this.txtURLRegexReplacePattern);
		this.tpFileNaming.Controls.Add(this.lblURLRegexReplacePattern);
		this.tpFileNaming.Controls.Add(this.cbURLRegexReplace);
		this.tpFileNaming.Controls.Add(this.btnAutoIncrementNumber);
		this.tpFileNaming.Controls.Add(this.lblAutoIncrementNumber);
		this.tpFileNaming.Controls.Add(this.nudAutoIncrementNumber);
		this.tpFileNaming.Controls.Add(this.cbFileUploadReplaceProblematicCharacters);
		this.tpFileNaming.Controls.Add(this.cbNameFormatCustomTimeZone);
		this.tpFileNaming.Controls.Add(this.lblNameFormatPatternPreview);
		this.tpFileNaming.Controls.Add(this.lblNameFormatPatternActiveWindow);
		this.tpFileNaming.Controls.Add(this.lblNameFormatPatternPreviewActiveWindow);
		this.tpFileNaming.Controls.Add(this.cbNameFormatTimeZone);
		this.tpFileNaming.Controls.Add(this.txtNameFormatPatternActiveWindow);
		this.tpFileNaming.Controls.Add(this.cbFileUploadUseNamePattern);
		this.tpFileNaming.Controls.Add(this.lblNameFormatPattern);
		this.tpFileNaming.Controls.Add(this.txtNameFormatPattern);
		resources.ApplyResources(this.tpFileNaming, "tpFileNaming");
		this.tpFileNaming.Name = "tpFileNaming";
		resources.ApplyResources(this.txtURLRegexReplaceReplacement, "txtURLRegexReplaceReplacement");
		this.txtURLRegexReplaceReplacement.Name = "txtURLRegexReplaceReplacement";
		this.txtURLRegexReplaceReplacement.TextChanged += new System.EventHandler(txtURLRegexReplaceReplacement_TextChanged);
		resources.ApplyResources(this.lblURLRegexReplaceReplacement, "lblURLRegexReplaceReplacement");
		this.lblURLRegexReplaceReplacement.Name = "lblURLRegexReplaceReplacement";
		resources.ApplyResources(this.txtURLRegexReplacePattern, "txtURLRegexReplacePattern");
		this.txtURLRegexReplacePattern.Name = "txtURLRegexReplacePattern";
		this.txtURLRegexReplacePattern.TextChanged += new System.EventHandler(txtURLRegexReplacePattern_TextChanged);
		resources.ApplyResources(this.lblURLRegexReplacePattern, "lblURLRegexReplacePattern");
		this.lblURLRegexReplacePattern.Name = "lblURLRegexReplacePattern";
		resources.ApplyResources(this.cbURLRegexReplace, "cbURLRegexReplace");
		this.cbURLRegexReplace.Name = "cbURLRegexReplace";
		this.cbURLRegexReplace.UseVisualStyleBackColor = true;
		this.cbURLRegexReplace.CheckedChanged += new System.EventHandler(cbURLRegexReplace_CheckedChanged);
		resources.ApplyResources(this.btnAutoIncrementNumber, "btnAutoIncrementNumber");
		this.btnAutoIncrementNumber.Name = "btnAutoIncrementNumber";
		this.btnAutoIncrementNumber.UseVisualStyleBackColor = true;
		this.btnAutoIncrementNumber.Click += new System.EventHandler(btnAutoIncrementNumber_Click);
		resources.ApplyResources(this.lblAutoIncrementNumber, "lblAutoIncrementNumber");
		this.lblAutoIncrementNumber.Name = "lblAutoIncrementNumber";
		resources.ApplyResources(this.nudAutoIncrementNumber, "nudAutoIncrementNumber");
		this.nudAutoIncrementNumber.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		this.nudAutoIncrementNumber.Name = "nudAutoIncrementNumber";
		resources.ApplyResources(this.cbFileUploadReplaceProblematicCharacters, "cbFileUploadReplaceProblematicCharacters");
		this.cbFileUploadReplaceProblematicCharacters.Name = "cbFileUploadReplaceProblematicCharacters";
		this.cbFileUploadReplaceProblematicCharacters.UseVisualStyleBackColor = true;
		this.cbFileUploadReplaceProblematicCharacters.CheckedChanged += new System.EventHandler(cbFileUploadReplaceProblematicCharacters_CheckedChanged);
		resources.ApplyResources(this.cbNameFormatCustomTimeZone, "cbNameFormatCustomTimeZone");
		this.cbNameFormatCustomTimeZone.Name = "cbNameFormatCustomTimeZone";
		this.cbNameFormatCustomTimeZone.UseVisualStyleBackColor = true;
		this.cbNameFormatCustomTimeZone.CheckedChanged += new System.EventHandler(cbNameFormatCustomTimeZone_CheckedChanged);
		resources.ApplyResources(this.lblNameFormatPatternPreview, "lblNameFormatPatternPreview");
		this.lblNameFormatPatternPreview.Name = "lblNameFormatPatternPreview";
		resources.ApplyResources(this.lblNameFormatPatternActiveWindow, "lblNameFormatPatternActiveWindow");
		this.lblNameFormatPatternActiveWindow.Name = "lblNameFormatPatternActiveWindow";
		resources.ApplyResources(this.lblNameFormatPatternPreviewActiveWindow, "lblNameFormatPatternPreviewActiveWindow");
		this.lblNameFormatPatternPreviewActiveWindow.Name = "lblNameFormatPatternPreviewActiveWindow";
		this.cbNameFormatTimeZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbNameFormatTimeZone.FormattingEnabled = true;
		resources.ApplyResources(this.cbNameFormatTimeZone, "cbNameFormatTimeZone");
		this.cbNameFormatTimeZone.Name = "cbNameFormatTimeZone";
		this.cbNameFormatTimeZone.SelectedIndexChanged += new System.EventHandler(cbNameFormatTimeZone_SelectedIndexChanged);
		resources.ApplyResources(this.txtNameFormatPatternActiveWindow, "txtNameFormatPatternActiveWindow");
		this.txtNameFormatPatternActiveWindow.Name = "txtNameFormatPatternActiveWindow";
		this.txtNameFormatPatternActiveWindow.TextChanged += new System.EventHandler(txtNameFormatPatternActiveWindow_TextChanged);
		resources.ApplyResources(this.cbFileUploadUseNamePattern, "cbFileUploadUseNamePattern");
		this.cbFileUploadUseNamePattern.Name = "cbFileUploadUseNamePattern";
		this.cbFileUploadUseNamePattern.UseVisualStyleBackColor = true;
		this.cbFileUploadUseNamePattern.CheckedChanged += new System.EventHandler(cbFileUploadUseNamePattern_CheckedChanged);
		resources.ApplyResources(this.lblNameFormatPattern, "lblNameFormatPattern");
		this.lblNameFormatPattern.Name = "lblNameFormatPattern";
		resources.ApplyResources(this.txtNameFormatPattern, "txtNameFormatPattern");
		this.txtNameFormatPattern.Name = "txtNameFormatPattern";
		this.txtNameFormatPattern.TextChanged += new System.EventHandler(txtNameFormatPattern_TextChanged);
		this.tpUploadClipboard.BackColor = System.Drawing.SystemColors.Window;
		this.tpUploadClipboard.Controls.Add(this.cbClipboardUploadShareURL);
		this.tpUploadClipboard.Controls.Add(this.cbClipboardUploadURLContents);
		this.tpUploadClipboard.Controls.Add(this.cbClipboardUploadAutoIndexFolder);
		this.tpUploadClipboard.Controls.Add(this.cbClipboardUploadShortenURL);
		resources.ApplyResources(this.tpUploadClipboard, "tpUploadClipboard");
		this.tpUploadClipboard.Name = "tpUploadClipboard";
		resources.ApplyResources(this.cbClipboardUploadShareURL, "cbClipboardUploadShareURL");
		this.cbClipboardUploadShareURL.Name = "cbClipboardUploadShareURL";
		this.cbClipboardUploadShareURL.UseVisualStyleBackColor = true;
		this.cbClipboardUploadShareURL.CheckedChanged += new System.EventHandler(cbClipboardUploadShareURL_CheckedChanged);
		resources.ApplyResources(this.cbClipboardUploadURLContents, "cbClipboardUploadURLContents");
		this.cbClipboardUploadURLContents.Name = "cbClipboardUploadURLContents";
		this.cbClipboardUploadURLContents.UseVisualStyleBackColor = true;
		this.cbClipboardUploadURLContents.CheckedChanged += new System.EventHandler(cbClipboardUploadContents_CheckedChanged);
		resources.ApplyResources(this.cbClipboardUploadAutoIndexFolder, "cbClipboardUploadAutoIndexFolder");
		this.cbClipboardUploadAutoIndexFolder.Name = "cbClipboardUploadAutoIndexFolder";
		this.cbClipboardUploadAutoIndexFolder.UseVisualStyleBackColor = true;
		this.cbClipboardUploadAutoIndexFolder.CheckedChanged += new System.EventHandler(cbClipboardUploadAutoIndexFolder_CheckedChanged);
		resources.ApplyResources(this.cbClipboardUploadShortenURL, "cbClipboardUploadShortenURL");
		this.cbClipboardUploadShortenURL.Name = "cbClipboardUploadShortenURL";
		this.cbClipboardUploadShortenURL.UseVisualStyleBackColor = true;
		this.cbClipboardUploadShortenURL.CheckedChanged += new System.EventHandler(cbClipboardUploadAutoDetectURL_CheckedChanged);
		this.tpUploaderFilters.BackColor = System.Drawing.SystemColors.Window;
		this.tpUploaderFilters.Controls.Add(this.lvUploaderFiltersList);
		this.tpUploaderFilters.Controls.Add(this.btnUploaderFiltersRemove);
		this.tpUploaderFilters.Controls.Add(this.btnUploaderFiltersUpdate);
		this.tpUploaderFilters.Controls.Add(this.btnUploaderFiltersAdd);
		this.tpUploaderFilters.Controls.Add(this.lblUploaderFiltersDestination);
		this.tpUploaderFilters.Controls.Add(this.cbUploaderFiltersDestination);
		this.tpUploaderFilters.Controls.Add(this.lblUploaderFiltersExtensionsExample);
		this.tpUploaderFilters.Controls.Add(this.lblUploaderFiltersExtensions);
		this.tpUploaderFilters.Controls.Add(this.txtUploaderFiltersExtensions);
		resources.ApplyResources(this.tpUploaderFilters, "tpUploaderFilters");
		this.tpUploaderFilters.Name = "tpUploaderFilters";
		resources.ApplyResources(this.lvUploaderFiltersList, "lvUploaderFiltersList");
		this.lvUploaderFiltersList.AutoFillColumn = true;
		this.lvUploaderFiltersList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.chUploaderFiltersName, this.chUploaderFiltersExtension });
		this.lvUploaderFiltersList.FullRowSelect = true;
		this.lvUploaderFiltersList.HideSelection = false;
		this.lvUploaderFiltersList.Name = "lvUploaderFiltersList";
		this.lvUploaderFiltersList.UseCompatibleStateImageBehavior = false;
		this.lvUploaderFiltersList.View = System.Windows.Forms.View.Details;
		this.lvUploaderFiltersList.SelectedIndexChanged += new System.EventHandler(lvUploaderFiltersList_SelectedIndexChanged);
		resources.ApplyResources(this.chUploaderFiltersName, "chUploaderFiltersName");
		resources.ApplyResources(this.chUploaderFiltersExtension, "chUploaderFiltersExtension");
		resources.ApplyResources(this.btnUploaderFiltersRemove, "btnUploaderFiltersRemove");
		this.btnUploaderFiltersRemove.Name = "btnUploaderFiltersRemove";
		this.btnUploaderFiltersRemove.UseVisualStyleBackColor = true;
		this.btnUploaderFiltersRemove.Click += new System.EventHandler(btnUploaderFiltersRemove_Click);
		resources.ApplyResources(this.btnUploaderFiltersUpdate, "btnUploaderFiltersUpdate");
		this.btnUploaderFiltersUpdate.Name = "btnUploaderFiltersUpdate";
		this.btnUploaderFiltersUpdate.UseVisualStyleBackColor = true;
		this.btnUploaderFiltersUpdate.Click += new System.EventHandler(btnUploaderFiltersUpdate_Click);
		resources.ApplyResources(this.btnUploaderFiltersAdd, "btnUploaderFiltersAdd");
		this.btnUploaderFiltersAdd.Name = "btnUploaderFiltersAdd";
		this.btnUploaderFiltersAdd.UseVisualStyleBackColor = true;
		this.btnUploaderFiltersAdd.Click += new System.EventHandler(btnUploaderFiltersAdd_Click);
		resources.ApplyResources(this.lblUploaderFiltersDestination, "lblUploaderFiltersDestination");
		this.lblUploaderFiltersDestination.Name = "lblUploaderFiltersDestination";
		this.cbUploaderFiltersDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbUploaderFiltersDestination.FormattingEnabled = true;
		resources.ApplyResources(this.cbUploaderFiltersDestination, "cbUploaderFiltersDestination");
		this.cbUploaderFiltersDestination.Name = "cbUploaderFiltersDestination";
		resources.ApplyResources(this.lblUploaderFiltersExtensionsExample, "lblUploaderFiltersExtensionsExample");
		this.lblUploaderFiltersExtensionsExample.Name = "lblUploaderFiltersExtensionsExample";
		resources.ApplyResources(this.lblUploaderFiltersExtensions, "lblUploaderFiltersExtensions");
		this.lblUploaderFiltersExtensions.Name = "lblUploaderFiltersExtensions";
		resources.ApplyResources(this.txtUploaderFiltersExtensions, "txtUploaderFiltersExtensions");
		this.txtUploaderFiltersExtensions.Name = "txtUploaderFiltersExtensions";
		this.tpActions.BackColor = System.Drawing.SystemColors.Window;
		this.tpActions.Controls.Add(this.pActions);
		this.tpActions.Controls.Add(this.cbOverrideActions);
		resources.ApplyResources(this.tpActions, "tpActions");
		this.tpActions.Name = "tpActions";
		this.pActions.Controls.Add(this.btnActions);
		this.pActions.Controls.Add(this.lblActionsNote);
		this.pActions.Controls.Add(this.btnActionsDuplicate);
		this.pActions.Controls.Add(this.btnActionsAdd);
		this.pActions.Controls.Add(this.lvActions);
		this.pActions.Controls.Add(this.btnActionsEdit);
		this.pActions.Controls.Add(this.btnActionsRemove);
		resources.ApplyResources(this.pActions, "pActions");
		this.pActions.Name = "pActions";
		resources.ApplyResources(this.btnActions, "btnActions");
		this.btnActions.Name = "btnActions";
		this.btnActions.UseVisualStyleBackColor = true;
		this.btnActions.Click += new System.EventHandler(btnActions_Click);
		resources.ApplyResources(this.lblActionsNote, "lblActionsNote");
		this.lblActionsNote.Name = "lblActionsNote";
		resources.ApplyResources(this.btnActionsDuplicate, "btnActionsDuplicate");
		this.btnActionsDuplicate.Name = "btnActionsDuplicate";
		this.btnActionsDuplicate.UseVisualStyleBackColor = true;
		this.btnActionsDuplicate.Click += new System.EventHandler(btnActionsDuplicate_Click);
		resources.ApplyResources(this.btnActionsAdd, "btnActionsAdd");
		this.btnActionsAdd.Name = "btnActionsAdd";
		this.btnActionsAdd.UseVisualStyleBackColor = true;
		this.btnActionsAdd.Click += new System.EventHandler(btnActionsAdd_Click);
		this.lvActions.AllowDrop = true;
		this.lvActions.AllowItemDrag = true;
		resources.ApplyResources(this.lvActions, "lvActions");
		this.lvActions.AutoFillColumn = true;
		this.lvActions.AutoFillColumnIndex = 2;
		this.lvActions.CheckBoxes = true;
		this.lvActions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.chActionsName, this.chActionsPath, this.chActionsArgs, this.chActionsExtensions });
		this.lvActions.FullRowSelect = true;
		this.lvActions.HideSelection = false;
		this.lvActions.MultiSelect = false;
		this.lvActions.Name = "lvActions";
		this.lvActions.UseCompatibleStateImageBehavior = false;
		this.lvActions.View = System.Windows.Forms.View.Details;
		this.lvActions.ItemMoved += new ShareX.HelpersLib.MyListView.ListViewItemMovedEventHandler(lvActions_ItemMoved);
		this.lvActions.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lvActions_ItemChecked);
		this.lvActions.SelectedIndexChanged += new System.EventHandler(lvActions_SelectedIndexChanged);
		resources.ApplyResources(this.chActionsName, "chActionsName");
		resources.ApplyResources(this.chActionsPath, "chActionsPath");
		resources.ApplyResources(this.chActionsArgs, "chActionsArgs");
		resources.ApplyResources(this.chActionsExtensions, "chActionsExtensions");
		resources.ApplyResources(this.btnActionsEdit, "btnActionsEdit");
		this.btnActionsEdit.Name = "btnActionsEdit";
		this.btnActionsEdit.UseVisualStyleBackColor = true;
		this.btnActionsEdit.Click += new System.EventHandler(btnActionsEdit_Click);
		resources.ApplyResources(this.btnActionsRemove, "btnActionsRemove");
		this.btnActionsRemove.Name = "btnActionsRemove";
		this.btnActionsRemove.UseVisualStyleBackColor = true;
		this.btnActionsRemove.Click += new System.EventHandler(btnActionsRemove_Click);
		resources.ApplyResources(this.cbOverrideActions, "cbOverrideActions");
		this.cbOverrideActions.Checked = true;
		this.cbOverrideActions.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbOverrideActions.Name = "cbOverrideActions";
		this.cbOverrideActions.UseVisualStyleBackColor = true;
		this.cbOverrideActions.CheckedChanged += new System.EventHandler(cbUseDefaultActions_CheckedChanged);
		this.tpWatchFolders.BackColor = System.Drawing.SystemColors.Window;
		this.tpWatchFolders.Controls.Add(this.btnWatchFolderEdit);
		this.tpWatchFolders.Controls.Add(this.cbWatchFolderEnabled);
		this.tpWatchFolders.Controls.Add(this.lvWatchFolderList);
		this.tpWatchFolders.Controls.Add(this.btnWatchFolderRemove);
		this.tpWatchFolders.Controls.Add(this.btnWatchFolderAdd);
		resources.ApplyResources(this.tpWatchFolders, "tpWatchFolders");
		this.tpWatchFolders.Name = "tpWatchFolders";
		resources.ApplyResources(this.btnWatchFolderEdit, "btnWatchFolderEdit");
		this.btnWatchFolderEdit.Name = "btnWatchFolderEdit";
		this.btnWatchFolderEdit.UseVisualStyleBackColor = true;
		this.btnWatchFolderEdit.Click += new System.EventHandler(btnWatchFolderEdit_Click);
		resources.ApplyResources(this.cbWatchFolderEnabled, "cbWatchFolderEnabled");
		this.cbWatchFolderEnabled.Name = "cbWatchFolderEnabled";
		this.cbWatchFolderEnabled.UseVisualStyleBackColor = true;
		this.cbWatchFolderEnabled.CheckedChanged += new System.EventHandler(cbWatchFolderEnabled_CheckedChanged);
		resources.ApplyResources(this.lvWatchFolderList, "lvWatchFolderList");
		this.lvWatchFolderList.AutoFillColumn = true;
		this.lvWatchFolderList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3] { this.chWatchFolderFolderPath, this.chWatchFolderFilter, this.chWatchFolderIncludeSubdirectories });
		this.lvWatchFolderList.FullRowSelect = true;
		this.lvWatchFolderList.HideSelection = false;
		this.lvWatchFolderList.Name = "lvWatchFolderList";
		this.lvWatchFolderList.UseCompatibleStateImageBehavior = false;
		this.lvWatchFolderList.View = System.Windows.Forms.View.Details;
		this.lvWatchFolderList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(lvWatchFolderList_MouseDoubleClick);
		resources.ApplyResources(this.chWatchFolderFolderPath, "chWatchFolderFolderPath");
		resources.ApplyResources(this.chWatchFolderFilter, "chWatchFolderFilter");
		resources.ApplyResources(this.chWatchFolderIncludeSubdirectories, "chWatchFolderIncludeSubdirectories");
		resources.ApplyResources(this.btnWatchFolderRemove, "btnWatchFolderRemove");
		this.btnWatchFolderRemove.Name = "btnWatchFolderRemove";
		this.btnWatchFolderRemove.UseVisualStyleBackColor = true;
		this.btnWatchFolderRemove.Click += new System.EventHandler(btnWatchFolderRemove_Click);
		resources.ApplyResources(this.btnWatchFolderAdd, "btnWatchFolderAdd");
		this.btnWatchFolderAdd.Name = "btnWatchFolderAdd";
		this.btnWatchFolderAdd.UseVisualStyleBackColor = true;
		this.btnWatchFolderAdd.Click += new System.EventHandler(btnWatchFolderAdd_Click);
		this.tpTools.BackColor = System.Drawing.SystemColors.Window;
		this.tpTools.Controls.Add(this.pTools);
		this.tpTools.Controls.Add(this.cbOverrideToolsSettings);
		resources.ApplyResources(this.tpTools, "tpTools");
		this.tpTools.Name = "tpTools";
		this.pTools.Controls.Add(this.txtToolsScreenColorPickerFormatCtrl);
		this.pTools.Controls.Add(this.lblToolsScreenColorPickerFormatCtrl);
		this.pTools.Controls.Add(this.txtToolsScreenColorPickerInfoText);
		this.pTools.Controls.Add(this.lblToolsScreenColorPickerInfoText);
		this.pTools.Controls.Add(this.txtToolsScreenColorPickerFormat);
		this.pTools.Controls.Add(this.lblToolsScreenColorPickerFormat);
		resources.ApplyResources(this.pTools, "pTools");
		this.pTools.Name = "pTools";
		resources.ApplyResources(this.txtToolsScreenColorPickerFormatCtrl, "txtToolsScreenColorPickerFormatCtrl");
		this.txtToolsScreenColorPickerFormatCtrl.Name = "txtToolsScreenColorPickerFormatCtrl";
		this.txtToolsScreenColorPickerFormatCtrl.TextChanged += new System.EventHandler(txtToolsScreenColorPickerFormatCtrl_TextChanged);
		resources.ApplyResources(this.lblToolsScreenColorPickerFormatCtrl, "lblToolsScreenColorPickerFormatCtrl");
		this.lblToolsScreenColorPickerFormatCtrl.Name = "lblToolsScreenColorPickerFormatCtrl";
		resources.ApplyResources(this.txtToolsScreenColorPickerInfoText, "txtToolsScreenColorPickerInfoText");
		this.txtToolsScreenColorPickerInfoText.Name = "txtToolsScreenColorPickerInfoText";
		this.txtToolsScreenColorPickerInfoText.TextChanged += new System.EventHandler(txtToolsScreenColorPickerInfoText_TextChanged);
		resources.ApplyResources(this.lblToolsScreenColorPickerInfoText, "lblToolsScreenColorPickerInfoText");
		this.lblToolsScreenColorPickerInfoText.Name = "lblToolsScreenColorPickerInfoText";
		resources.ApplyResources(this.txtToolsScreenColorPickerFormat, "txtToolsScreenColorPickerFormat");
		this.txtToolsScreenColorPickerFormat.Name = "txtToolsScreenColorPickerFormat";
		this.txtToolsScreenColorPickerFormat.TextChanged += new System.EventHandler(txtToolsScreenColorPickerFormat_TextChanged);
		resources.ApplyResources(this.lblToolsScreenColorPickerFormat, "lblToolsScreenColorPickerFormat");
		this.lblToolsScreenColorPickerFormat.Name = "lblToolsScreenColorPickerFormat";
		resources.ApplyResources(this.cbOverrideToolsSettings, "cbOverrideToolsSettings");
		this.cbOverrideToolsSettings.Checked = true;
		this.cbOverrideToolsSettings.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbOverrideToolsSettings.Name = "cbOverrideToolsSettings";
		this.cbOverrideToolsSettings.UseVisualStyleBackColor = true;
		this.cbOverrideToolsSettings.CheckedChanged += new System.EventHandler(cbUseDefaultToolsSettings_CheckedChanged);
		this.tpAdvanced.BackColor = System.Drawing.SystemColors.Window;
		this.tpAdvanced.Controls.Add(this.pgTaskSettings);
		this.tpAdvanced.Controls.Add(this.cbOverrideAdvancedSettings);
		resources.ApplyResources(this.tpAdvanced, "tpAdvanced");
		this.tpAdvanced.Name = "tpAdvanced";
		resources.ApplyResources(this.pgTaskSettings, "pgTaskSettings");
		this.pgTaskSettings.Name = "pgTaskSettings";
		this.pgTaskSettings.PropertySort = System.Windows.Forms.PropertySort.Categorized;
		this.pgTaskSettings.ToolbarVisible = false;
		resources.ApplyResources(this.cbOverrideAdvancedSettings, "cbOverrideAdvancedSettings");
		this.cbOverrideAdvancedSettings.Checked = true;
		this.cbOverrideAdvancedSettings.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbOverrideAdvancedSettings.Name = "cbOverrideAdvancedSettings";
		this.cbOverrideAdvancedSettings.UseVisualStyleBackColor = true;
		this.cbOverrideAdvancedSettings.CheckedChanged += new System.EventHandler(cbUseDefaultAdvancedSettings_CheckedChanged);
		resources.ApplyResources(this.tttvMain, "tttvMain");
		this.tttvMain.ImageList = null;
		this.tttvMain.LeftPanelBackColor = System.Drawing.SystemColors.Window;
		this.tttvMain.MainTabControl = null;
		this.tttvMain.Name = "tttvMain";
		this.tttvMain.SeparatorColor = System.Drawing.SystemColors.ControlDark;
		this.tttvMain.TreeViewFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 162);
		this.tttvMain.TreeViewSize = 190;
		this.tttvMain.TabChanged += new ShareX.HelpersLib.TabToTreeView.TabChangedEventHandler(tttvMain_TabChanged);
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.tcTaskSettings);
		base.Controls.Add(this.tttvMain);
		base.Name = "TaskSettingsForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.Resize += new System.EventHandler(TaskSettingsForm_Resize);
		this.tcTaskSettings.ResumeLayout(false);
		this.tpTask.ResumeLayout(false);
		this.tpTask.PerformLayout();
		this.cmsDestinations.ResumeLayout(false);
		this.tpGeneral.ResumeLayout(false);
		this.tcGeneral.ResumeLayout(false);
		this.tpGeneralMain.ResumeLayout(false);
		this.tpGeneralMain.PerformLayout();
		this.tpNotifications.ResumeLayout(false);
		this.tpNotifications.PerformLayout();
		this.gbToastWindow.ResumeLayout(false);
		this.gbToastWindow.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudToastWindowSizeHeight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudToastWindowSizeWidth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudToastWindowFadeDuration).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudToastWindowDuration).EndInit();
		this.tpImage.ResumeLayout(false);
		this.tcImage.ResumeLayout(false);
		this.tpQuality.ResumeLayout(false);
		this.tpQuality.PerformLayout();
		this.pImage.ResumeLayout(false);
		this.pImage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudImageAutoUseJPEGSize).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudImageJPEGQuality).EndInit();
		this.tpEffects.ResumeLayout(false);
		this.tpEffects.PerformLayout();
		this.tpThumbnail.ResumeLayout(false);
		this.tpThumbnail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudThumbnailHeight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudThumbnailWidth).EndInit();
		this.tpCapture.ResumeLayout(false);
		this.tcCapture.ResumeLayout(false);
		this.tpCaptureGeneral.ResumeLayout(false);
		this.tpCaptureGeneral.PerformLayout();
		this.pCapture.ResumeLayout(false);
		this.pCapture.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureCustomRegionHeight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureCustomRegionWidth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureCustomRegionY).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureCustomRegionX).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudScreenshotDelay).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudCaptureShadowOffset).EndInit();
		this.tpRegionCapture.ResumeLayout(false);
		this.tpRegionCapture.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureFPSLimit).EndInit();
		this.flpRegionCaptureFixedSize.ResumeLayout(false);
		this.flpRegionCaptureFixedSize.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureFixedSizeWidth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureFixedSizeHeight).EndInit();
		this.pRegionCaptureSnapSizes.ResumeLayout(false);
		this.pRegionCaptureSnapSizes.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureSnapSizesHeight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureSnapSizesWidth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureMagnifierPixelCount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudRegionCaptureMagnifierPixelSize).EndInit();
		this.tpScreenRecorder.ResumeLayout(false);
		this.tpScreenRecorder.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudScreenRecordFPS).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudScreenRecorderDuration).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudScreenRecorderStartDelay).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudGIFFPS).EndInit();
		this.tpOCR.ResumeLayout(false);
		this.tpOCR.PerformLayout();
		this.tpUpload.ResumeLayout(false);
		this.tcUpload.ResumeLayout(false);
		this.tpUploadMain.ResumeLayout(false);
		this.tpUploadMain.PerformLayout();
		this.tpFileNaming.ResumeLayout(false);
		this.tpFileNaming.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.nudAutoIncrementNumber).EndInit();
		this.tpUploadClipboard.ResumeLayout(false);
		this.tpUploadClipboard.PerformLayout();
		this.tpUploaderFilters.ResumeLayout(false);
		this.tpUploaderFilters.PerformLayout();
		this.tpActions.ResumeLayout(false);
		this.tpActions.PerformLayout();
		this.pActions.ResumeLayout(false);
		this.pActions.PerformLayout();
		this.tpWatchFolders.ResumeLayout(false);
		this.tpWatchFolders.PerformLayout();
		this.tpTools.ResumeLayout(false);
		this.tpTools.PerformLayout();
		this.pTools.ResumeLayout(false);
		this.pTools.PerformLayout();
		this.tpAdvanced.ResumeLayout(false);
		this.tpAdvanced.PerformLayout();
		base.ResumeLayout(false);
	}
}
