using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.ImageEffectsLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;
using ShareX.UploadersLib;

namespace ShareX;

public class MainForm : HotkeyForm
{
	private bool forceClose;

	private bool trayMenuSaveSettings = true;

	private int trayClickCount;

	private UploadInfoManager uim;

	private ToolStripDropDownItem tsmiImageFileUploaders;

	private ToolStripDropDownItem tsmiTrayImageFileUploaders;

	private ToolStripDropDownItem tsmiTextFileUploaders;

	private ToolStripDropDownItem tsmiTrayTextFileUploaders;

	private ImageFilesCache actionsMenuIconCache = new ImageFilesCache();

	private IContainer components;

	private MyListView lvUploads;

	private ColumnHeader chStatus;

	private ColumnHeader chURL;

	private ColumnHeader chFilename;

	private ColumnHeader chProgress;

	private ToolStripBorderRight tsMain;

	private ToolStripSeparator tssMain1;

	private ColumnHeader chSpeed;

	private ColumnHeader chRemaining;

	private ColumnHeader chElapsed;

	private ToolStripButton tsbHistory;

	private ToolStripMenuItem tsmiImageUploaders;

	private ToolStripMenuItem tsmiTextUploaders;

	private ToolStripMenuItem tsmiFileUploaders;

	private ToolStripMenuItem tsmiURLShorteners;

	private ToolStripDropDownButton tsddbDestinations;

	private ToolStripMenuItem tsmiTrayExit;

	private ToolStripSeparator tssTray1;

	public NotifyIcon niTray;

	private ToolStripDropDownButton tsddbCapture;

	private ToolStripMenuItem tsmiFullscreen;

	private ToolStripMenuItem tsmiRectangle;

	private ToolStripMenuItem tsmiWindow;

	private ToolStripMenuItem tsmiTrayHistory;

	private ToolStripSeparator tssTray2;

	private ToolStripMenuItem tsmiTrayCapture;

	private ToolStripMenuItem tsmiTrayFullscreen;

	private ToolStripMenuItem tsmiTrayWindow;

	private ToolStripMenuItem tsmiTrayRectangle;

	private ToolStripMenuItem tsmiLastRegion;

	private ToolStripMenuItem tsmiTrayLastRegion;

	private SplitContainerCustomSplitter scMain;

	private ToolStripMenuItem tsmiTrayDestinations;

	private ToolStripMenuItem tsmiTrayImageUploaders;

	private ToolStripMenuItem tsmiTrayTextUploaders;

	private ToolStripMenuItem tsmiTrayFileUploaders;

	private ToolStripMenuItem tsmiTrayURLShorteners;

	private ContextMenuStrip cmsTaskInfo;

	private ToolStripMenuItem tsmiOpen;

	private ToolStripMenuItem tsmiOpenURL;

	private ToolStripMenuItem tsmiOpenShortenedURL;

	private ToolStripMenuItem tsmiOpenThumbnailURL;

	private ToolStripMenuItem tsmiOpenDeletionURL;

	private ToolStripSeparator tssOpen1;

	private ToolStripMenuItem tsmiOpenFile;

	private ToolStripMenuItem tsmiOpenFolder;

	private ToolStripMenuItem tsmiCopy;

	private ToolStripMenuItem tsmiCopyURL;

	private ToolStripMenuItem tsmiCopyShortenedURL;

	private ToolStripMenuItem tsmiCopyThumbnailURL;

	private ToolStripMenuItem tsmiCopyDeletionURL;

	private ToolStripSeparator tssCopy1;

	private ToolStripMenuItem tsmiCopyFile;

	private ToolStripMenuItem tsmiCopyImage;

	private ToolStripMenuItem tsmiCopyText;

	private ToolStripSeparator tssCopy2;

	private ToolStripMenuItem tsmiCopyHTMLLink;

	private ToolStripMenuItem tsmiCopyHTMLImage;

	private ToolStripMenuItem tsmiCopyHTMLLinkedImage;

	private ToolStripSeparator tssCopy3;

	private ToolStripMenuItem tsmiCopyForumLink;

	private ToolStripMenuItem tsmiCopyForumImage;

	private ToolStripMenuItem tsmiCopyForumLinkedImage;

	private ToolStripSeparator tssCopy4;

	private ToolStripMenuItem tsmiCopyFilePath;

	private ToolStripMenuItem tsmiCopyFileName;

	private ToolStripMenuItem tsmiCopyFileNameWithExtension;

	private ToolStripMenuItem tsmiCopyFolder;

	private ToolStripMenuItem tsmiStopUpload;

	private MyPictureBox pbPreview;

	private ToolStripMenuItem tsmiShowErrors;

	private ToolStripMenuItem tsmiShowResponse;

	private ToolStripMenuItem tsmiScreenshotsFolder;

	private ToolStripDropDownButton tsddbAfterCaptureTasks;

	private ToolStripMenuItem tsmiTrayAfterCaptureTasks;

	private ToolStripMenuItem tsmiURLSharingServices;

	private ToolStripMenuItem tsmiTrayURLSharingServices;

	private ToolStripDropDownButton tsddbAfterUploadTasks;

	private ToolStripButton tsbScreenshotsFolder;

	private ToolStripMenuItem tsmiTrayAfterUploadTasks;

	private ToolStripSeparator tssUploadInfo1;

	private ToolStripMenuItem tsmiUploadSelectedFile;

	private ToolStripButton tsbImageHistory;

	private ToolStripMenuItem tsmiTrayImageHistory;

	private ToolStripMenuItem tsmiTrayTools;

	private ToolStripMenuItem tsmiTrayColorPicker;

	private ToolStripDropDownButton tsddbTools;

	private ToolStripMenuItem tsmiColorPicker;

	private ToolStripMenuItem tsmiClearList;

	private ToolStripMenuItem tsmiScreenRecordingGIF;

	private ToolStripMenuItem tsmiTrayScreenRecordingGIF;

	private ToolStripMenuItem tsmiHashCheck;

	private ToolStripMenuItem tsmiTrayHashCheck;

	private ToolStripMenuItem tsmiMonitor;

	private ToolStripMenuItem tsmiTrayMonitor;

	private ToolStripMenuItem tsmiAutoCapture;

	private ToolStripMenuItem tsmiTrayAutoCapture;

	private ToolStripDropDownButton tsddbDebug;

	private ToolStripMenuItem tsmiTestImageUpload;

	private ToolStripMenuItem tsmiTestTextUpload;

	private ToolStripMenuItem tsmiTestFileUpload;

	private ToolStripMenuItem tsmiTestURLShortener;

	private ToolStripSeparator tssCopy5;

	private ToolStripMenuItem tsmiShowDebugLog;

	private ToolStripButton tsbApplicationSettings;

	private ToolStripButton tsbTaskSettings;

	private ToolStripButton tsbHotkeySettings;

	private ToolStripSeparator tssMain2;

	private ToolStripMenuItem tsmiTrayApplicationSettings;

	private ToolStripMenuItem tsmiTrayTaskSettings;

	private ToolStripMenuItem tsmiTrayHotkeySettings;

	private ToolStripSeparator tssTray3;

	private ToolStripMenuItem tsmiIndexFolder;

	private ToolStripMenuItem tsmiTrayIndexFolder;

	private ToolStripMenuItem tsmiImageEffects;

	private ToolStripMenuItem tsmiTrayImageEffects;

	private ToolStripButton tsbAbout;

	private ToolStripMenuItem tsmiMonitorTest;

	private ToolStripMenuItem tsmiTrayMonitorTest;

	private ToolStripMenuItem tsmiTrayShow;

	private ToolStripMenuItem tsmiDNSChanger;

	private ToolStripMenuItem tsmiTrayDNSChanger;

	private ToolStripMenuItem tsmiRuler;

	private ToolStripMenuItem tsmiTrayRuler;

	private ToolStripMenuItem tsmiOpenThumbnailFile;

	private ToolStripMenuItem tsmiCopyThumbnailFile;

	private ToolStripMenuItem tsmiCopyThumbnailImage;

	private ToolStripMenuItem tsmiImageEditor;

	private ToolStripMenuItem tsmiTrayImageEditor;

	private ToolStripDropDownButton tsddbWorkflows;

	private ToolStripMenuItem tsmiTrayWorkflows;

	private ToolStripMenuItem tsmiShowQRCode;

	private ToolStripMenuItem tsmiQRCode;

	private ToolStripMenuItem tsmiTrayQRCode;

	private ToolStripMenuItem tsmiRectangleLight;

	private ToolStripMenuItem tsmiTrayRectangleLight;

	private ToolStripDropDownButton tsddbUpload;

	private ToolStripMenuItem tsmiUploadFile;

	private ToolStripMenuItem tsmiUploadClipboard;

	private ToolStripMenuItem tsmiUploadURL;

	private ToolStripMenuItem tsmiUploadDragDrop;

	private ToolStripSeparator tssDestinations1;

	private ToolStripMenuItem tsmiDestinationSettings;

	private ToolStripMenuItem tsmiTrayUpload;

	private ToolStripMenuItem tsmiTrayUploadFile;

	private ToolStripMenuItem tsmiTrayUploadClipboard;

	private ToolStripMenuItem tsmiTrayUploadURL;

	private ToolStripMenuItem tsmiTrayUploadDragDrop;

	private ToolStripSeparator tssTrayDestinations1;

	private ToolStripMenuItem tsmiTrayDestinationSettings;

	private ToolStripMenuItem tsmiShareSelectedURL;

	private ToolStripMenuItem tsmiShortenSelectedURL;

	private ToolStripMenuItem tsmiEditSelectedFile;

	private ToolStripMenuItem tsmiTestURLSharing;

	private ToolStripMenuItem tsmiDeleteSelectedFile;

	private ToolStripMenuItem tsmiScreenRecordingFFmpeg;

	private ToolStripMenuItem tsmiTrayScreenRecordingFFmpeg;

	private ToolStripMenuItem tsmiUploadFolder;

	private ToolStripMenuItem tsmiTrayUploadFolder;

	public Label lblListViewTip;

	private ToolStripMenuItem tsmiScreenColorPicker;

	private ToolStripMenuItem tsmiTrayScreenColorPicker;

	public ToolStripMenuItem tsmiTrayRecentItems;

	private ContextMenuStrip cmsTray;

	private ToolStripMenuItem tsmiRectangleTransparent;

	private ToolStripMenuItem tsmiTrayRectangleTransparent;

	private ToolStripMenuItem tsmiTrayToggleHotkeys;

	private ToolStripMenuItem tsmiVideoThumbnailer;

	private ToolStripMenuItem tsmiTrayVideoThumbnailer;

	private Timer timerTraySingleClick;

	private ToolStripMenuItem tsmiScrollingCapture;

	private ToolStripMenuItem tsmiTrayScrollingCapture;

	private ToolStripMenuItem tsmiImageCombiner;

	private ToolStripMenuItem tsmiTrayImageCombiner;

	private ToolStripMenuItem tsmiDownloadSelectedURL;

	private ToolStripMenuItem tsmiOCRImage;

	private ToolStripMenuItem tsmiCombineImages;

	private ToolStripMenuItem tsmiOpenActionsToolbar;

	private ToolStripMenuItem tsmiDeleteSelectedItem;

	private ToolStripMenuItem tsmiGoogleImageSearch;

	private ToolStripMenuItem tsmiImageThumbnailer;

	private ToolStripMenuItem tsmiTrayImageThumbnailer;

	private ToolStripMenuItem tsmiUploadText;

	private ToolStripMenuItem tsmiShortenURL;

	private ToolStripMenuItem tsmiTrayUploadText;

	private ToolStripMenuItem tsmiTrayShortenURL;

	private ToolStripMenuItem tsmiCopyMarkdownLink;

	private ToolStripMenuItem tsmiCopyMarkdownImage;

	private ToolStripMenuItem tsmiCopyMarkdownLinkedImage;

	private ToolStripSeparator tssCopy6;

	private ToolStripSeparator tssCapture1;

	private ToolStripMenuItem tsmiShowCursor;

	private ToolStripSeparator tssTrayCapture1;

	private ToolStripMenuItem tsmiTrayShowCursor;

	private ToolStripMenuItem tsmiCopyImageDimensions;

	private ToolStripMenuItem tsmiScreenshotDelay;

	private ToolStripMenuItem tsmiScreenshotDelay0;

	private ToolStripMenuItem tsmiScreenshotDelay1;

	private ToolStripMenuItem tsmiScreenshotDelay2;

	private ToolStripMenuItem tsmiScreenshotDelay3;

	private ToolStripMenuItem tsmiScreenshotDelay4;

	private ToolStripMenuItem tsmiScreenshotDelay5;

	private ToolStripMenuItem tsmiTrayScreenshotDelay;

	private ToolStripMenuItem tsmiTrayScreenshotDelay0;

	private ToolStripMenuItem tsmiTrayScreenshotDelay1;

	private ToolStripMenuItem tsmiTrayScreenshotDelay2;

	private ToolStripMenuItem tsmiTrayScreenshotDelay3;

	private ToolStripMenuItem tsmiTrayScreenshotDelay4;

	private ToolStripMenuItem tsmiTrayScreenshotDelay5;

	private ToolStripMenuItem tsmiCustomUploaderSettings;

	private ToolStripMenuItem tsmiTrayCustomUploaderSettings;

	private Panel pThumbnailView;

	private TaskThumbnailView ucTaskThumbnailView;

	private ToolStripMenuItem tsmiSwitchTaskViewMode;

	public Label lblThumbnailViewTip;

	private ToolTip ttMain;

	private ToolStripMenuItem tsmiRunAction;

	private Panel pToolbars;

	private ToolStripMenuItem tsmiImageSplitter;

	private ToolStripMenuItem tsmiTrayImageSplitter;

	private ToolStripMenuItem tsmiVideoConverter;

	private ToolStripMenuItem tsmiTrayVideoConverter;

	private ToolStripMenuItem tsmiAddImageEffects;

	private ToolStripMenuItem tsmiClipboardViewer;

	private ToolStripMenuItem tsmiTrayClipboardViewer;

	private ToolStripMenuItem tsmiRestartAsAdmin;

	private ToolStripMenuItem tsmiInspectWindow;

	private ToolStripMenuItem tsmiTrayInspectWindow;

	private ToolStripMenuItem tsmiTweetMessage;

	private ToolStripMenuItem tsmiTrayTweetMessage;

	private ToolStripSeparator tssTools1;

	private ToolStripSeparator tssTools2;

	private ToolStripSeparator tssTools3;

	private ToolStripSeparator tssTools4;

	private ToolStripSeparator tssTrayTools1;

	private ToolStripSeparator tssTrayTools2;

	private ToolStripSeparator tssTrayTools3;

	private ToolStripSeparator tssTrayTools4;

	private ToolStripMenuItem tsmiCombineImagesHorizontally;

	private ToolStripMenuItem tsmiCombineImagesVertically;

	private ToolStripMenuItem tsmiBingVisualSearch;

	private ToolStripButton tsbTwitter;

	private ToolStripButton tsbDiscord;

	private ToolStripSeparator tssMain3;

	private ToolStripButton tsbDonate;

	private ToolStripMenuItem tsmiBorderlessWindow;

	private ToolStripMenuItem tsmiTrayBorderlessWindow;

	private ToolStripMenuItem tsmiImageViewer;

	private ToolStripMenuItem tsmiTrayImageViewer;

	private ToolStripMenuItem tsmiOCR;

	private ToolStripMenuItem tsmiTrayOCR;

	public bool IsReady { get; private set; }

	public MainForm()
	{
		InitializeControls();
	}

	private async void MainForm_HandleCreated(object sender, EventArgs e)
	{
		RunPuushTasks();
		NativeMethods.UseImmersiveDarkMode(base.Handle, ShareXResources.IsDarkTheme);
		UpdateControls();
		DebugHelper.WriteLine("Startup time: {0} ms", Program.StartTimer.ElapsedMilliseconds);
		await Program.CLI.UseCommandLineArgs();
		if (Program.Settings.ActionsToolbarRunAtStartup)
		{
			TaskHelpers.OpenActionsToolbar();
		}
	}

	private void InitializeControls()
	{
		InitializeComponent();
		ShareXResources.UseWhiteIcon = Program.Settings.UseWhiteShareXIcon;
		base.Icon = ShareXResources.Icon;
		niTray.Icon = ShareXResources.Icon;
		Text = Program.Title;
		UpdateTheme();
		cmsTray.IgnoreSeparatorClick();
		cmsTaskInfo.IgnoreSeparatorClick();
		tsddbWorkflows.HideImageMargin();
		tsmiTrayWorkflows.HideImageMargin();
		tsmiMonitor.HideImageMargin();
		tsmiTrayMonitor.HideImageMargin();
		tsmiOpen.HideImageMargin();
		tsmiCopy.HideImageMargin();
		tsmiShortenSelectedURL.HideImageMargin();
		tsmiShareSelectedURL.HideImageMargin();
		tsmiTrayRecentItems.HideImageMargin();
		AddMultiEnumItems(delegate(AfterCaptureTasks x)
		{
			Program.DefaultTaskSettings.AfterCaptureJob = Program.DefaultTaskSettings.AfterCaptureJob.Swap<AfterCaptureTasks>(x);
		}, tsddbAfterCaptureTasks, tsmiTrayAfterCaptureTasks);
		tsddbAfterCaptureTasks.DropDownOpening += TsddbAfterCaptureTasks_DropDownOpening;
		tsmiTrayAfterCaptureTasks.DropDownOpening += TsmiTrayAfterCaptureTasks_DropDownOpening;
		AddMultiEnumItems(delegate(AfterUploadTasks x)
		{
			Program.DefaultTaskSettings.AfterUploadJob = Program.DefaultTaskSettings.AfterUploadJob.Swap<AfterUploadTasks>(x);
		}, tsddbAfterUploadTasks, tsmiTrayAfterUploadTasks);
		AddEnumItems(delegate(ImageDestination x)
		{
			Program.DefaultTaskSettings.ImageDestination = x;
			if (x == ImageDestination.FileUploader)
			{
				SetEnumChecked(Program.DefaultTaskSettings.ImageFileDestination, tsmiImageFileUploaders, tsmiTrayImageFileUploaders);
			}
			else
			{
				Uncheck(tsmiImageFileUploaders, tsmiTrayImageFileUploaders);
			}
		}, tsmiImageUploaders, tsmiTrayImageUploaders);
		tsmiImageFileUploaders = (ToolStripDropDownItem)tsmiImageUploaders.DropDownItems[tsmiImageUploaders.DropDownItems.Count - 1];
		tsmiTrayImageFileUploaders = (ToolStripDropDownItem)tsmiTrayImageUploaders.DropDownItems[tsmiTrayImageUploaders.DropDownItems.Count - 1];
		AddEnumItems(delegate(FileDestination x)
		{
			Program.DefaultTaskSettings.ImageFileDestination = x;
			tsmiImageFileUploaders.PerformClick();
			tsmiTrayImageFileUploaders.PerformClick();
		}, tsmiImageFileUploaders, tsmiTrayImageFileUploaders);
		AddEnumItems(delegate(TextDestination x)
		{
			Program.DefaultTaskSettings.TextDestination = x;
			if (x == TextDestination.FileUploader)
			{
				SetEnumChecked(Program.DefaultTaskSettings.TextFileDestination, tsmiTextFileUploaders, tsmiTrayTextFileUploaders);
			}
			else
			{
				Uncheck(tsmiTextFileUploaders, tsmiTrayTextFileUploaders);
			}
		}, tsmiTextUploaders, tsmiTrayTextUploaders);
		tsmiTextFileUploaders = (ToolStripDropDownItem)tsmiTextUploaders.DropDownItems[tsmiTextUploaders.DropDownItems.Count - 1];
		tsmiTrayTextFileUploaders = (ToolStripDropDownItem)tsmiTrayTextUploaders.DropDownItems[tsmiTrayTextUploaders.DropDownItems.Count - 1];
		AddEnumItems(delegate(FileDestination x)
		{
			Program.DefaultTaskSettings.TextFileDestination = x;
			tsmiTextFileUploaders.PerformClick();
			tsmiTrayTextFileUploaders.PerformClick();
		}, tsmiTextFileUploaders, tsmiTrayTextFileUploaders);
		AddEnumItems(delegate(FileDestination x)
		{
			Program.DefaultTaskSettings.FileDestination = x;
		}, tsmiFileUploaders, tsmiTrayFileUploaders);
		AddEnumItems(delegate(UrlShortenerType x)
		{
			Program.DefaultTaskSettings.URLShortenerDestination = x;
		}, tsmiURLShorteners, tsmiTrayURLShorteners);
		AddEnumItems(delegate(URLSharingServices x)
		{
			Program.DefaultTaskSettings.URLSharingServiceDestination = x;
		}, tsmiURLSharingServices, tsmiTrayURLSharingServices);
		UrlShortenerType[] enums = Helpers.GetEnums<UrlShortenerType>();
		foreach (UrlShortenerType urlShortener in enums)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(urlShortener.GetLocalizedDescription());
			toolStripMenuItem.Click += delegate
			{
				uim.ShortenURL(urlShortener);
			};
			tsmiShortenSelectedURL.DropDownItems.Add(toolStripMenuItem);
		}
		URLSharingServices[] enums2 = Helpers.GetEnums<URLSharingServices>();
		foreach (URLSharingServices urlSharingService in enums2)
		{
			ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(urlSharingService.GetLocalizedDescription());
			toolStripMenuItem2.Click += delegate
			{
				uim.ShareURL(urlSharingService);
			};
			tsmiShareSelectedURL.DropDownItems.Add(toolStripMenuItem2);
		}
		lvUploads.SupportCustomTheme();
		ImageList imageList = new ImageList();
		imageList.ColorDepth = ColorDepth.Depth32Bit;
		imageList.Images.Add(Resources.navigation_090_button);
		imageList.Images.Add(Resources.cross_button);
		imageList.Images.Add(Resources.tick_button);
		imageList.Images.Add(Resources.navigation_000_button);
		imageList.Images.Add(Resources.clock);
		lvUploads.SmallImageList = imageList;
		TaskManager.TaskListView = new TaskListView(lvUploads);
		TaskManager.TaskThumbnailView = ucTaskThumbnailView;
		uim = new UploadInfoManager();
		lblListViewTip.Parent = lvUploads;
		ToolStripDropDownItem[] array = new ToolStripDropDownItem[20]
		{
			tsddbAfterCaptureTasks, tsddbAfterUploadTasks, tsmiImageUploaders, tsmiImageFileUploaders, tsmiTextUploaders, tsmiTextFileUploaders, tsmiFileUploaders, tsmiURLShorteners, tsmiURLSharingServices, tsmiTrayAfterCaptureTasks,
			tsmiTrayAfterUploadTasks, tsmiTrayImageUploaders, tsmiTrayImageFileUploaders, tsmiTrayTextUploaders, tsmiTrayTextFileUploaders, tsmiTrayFileUploaders, tsmiTrayURLShorteners, tsmiTrayURLSharingServices, tsmiScreenshotDelay, tsmiTrayScreenshotDelay
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DisableMenuCloseOnClick();
		}
		ExportImportControl.UploadRequested += delegate(string json)
		{
			UploadManager.UploadText(json);
		};
		base.HandleCreated += MainForm_HandleCreated;
	}

	public void UpdateControls()
	{
		IsReady = false;
		niTray.Visible = Program.Settings.ShowTray;
		TaskManager.UpdateMainFormTip();
		TaskManager.RecentManager.InitItems();
		bool flag = false;
		if (Program.Settings.RememberMainFormPosition && !Program.Settings.MainFormPosition.IsEmpty && CaptureHelpers.GetScreenBounds().IntersectsWith(new Rectangle(Program.Settings.MainFormPosition, Program.Settings.MainFormSize)))
		{
			base.StartPosition = FormStartPosition.Manual;
			base.Location = Program.Settings.MainFormPosition;
			flag = true;
		}
		tsMain.Width = tsMain.PreferredSize.Width;
		int num = base.Size.Height + tsMain.PreferredSize.Height - tsMain.Height;
		MinimumSize = new Size(MinimumSize.Width, num);
		if (Program.Settings.RememberMainFormSize && !Program.Settings.MainFormSize.IsEmpty)
		{
			base.Size = Program.Settings.MainFormSize;
			if (!flag)
			{
				base.StartPosition = FormStartPosition.Manual;
				Rectangle activeScreenBounds = CaptureHelpers.GetActiveScreenBounds();
				base.Location = new Point(activeScreenBounds.Width / 2 - base.Size.Width / 2, activeScreenBounds.Height / 2 - base.Size.Height / 2);
			}
		}
		else
		{
			base.Size = new Size(base.Size.Width, num);
		}
		if (Program.Settings.PreviewSplitterDistance > 0)
		{
			scMain.SplitterDistance = Program.Settings.PreviewSplitterDistance;
		}
		if (Program.Settings.TaskListViewColumnWidths != null)
		{
			int num2 = Math.Min(lvUploads.Columns.Count - 1, Program.Settings.TaskListViewColumnWidths.Count);
			for (int i = 0; i < num2; i++)
			{
				lvUploads.Columns[i].Width = Program.Settings.TaskListViewColumnWidths[i];
			}
		}
		TaskbarManager.Enabled = Program.Settings.TaskbarProgressEnabled;
		UpdateCheckStates();
		UpdateUploaderMenuNames();
		UpdateDestinationStates();
		UpdateToggleHotkeyButton();
		AfterTaskSettingsJobs();
		AfterApplicationSettingsJobs();
		InitHotkeys();
		IsReady = true;
	}

	protected override void WndProc(ref Message m)
	{
		if (m.Msg == 17)
		{
			EndSessionReasons endSessionReasons = (EndSessionReasons)m.LParam.ToInt64();
			if (endSessionReasons.HasFlag(EndSessionReasons.ENDSESSION_CLOSEAPP))
			{
				NativeMethods.RegisterApplicationRestart("-silent", (RegisterApplicationRestartFlags)0u);
			}
			m.Result = new IntPtr(1);
		}
		else if (m.Msg == 22)
		{
			if (m.WParam != IntPtr.Zero)
			{
				Program.CloseSequence();
			}
			m.Result = IntPtr.Zero;
		}
		else
		{
			base.WndProc(ref m);
		}
	}

	private void AfterShownJobs()
	{
		if (!Program.Settings.ShowMostRecentTaskFirst && lvUploads.Items.Count > 0)
		{
			lvUploads.Items[lvUploads.Items.Count - 1].EnsureVisible();
		}
		if (Program.SteamFirstTimeConfig)
		{
			using (FirstTimeConfigForm firstTimeConfigForm = new FirstTimeConfigForm())
			{
				firstTimeConfigForm.ShowDialog();
				return;
			}
		}
		this.ForceActivate();
	}

	private void InitHotkeys()
	{
		Task.Run(delegate
		{
			SettingManager.WaitHotkeysConfig();
		}).ContinueInCurrentContext(delegate
		{
			if (Program.HotkeyManager == null)
			{
				Program.HotkeyManager = new HotkeyManager(this);
				HotkeyManager hotkeyManager = Program.HotkeyManager;
				hotkeyManager.HotkeyTrigger = (HotkeyManager.HotkeyTriggerEventHandler)Delegate.Combine(hotkeyManager.HotkeyTrigger, new HotkeyManager.HotkeyTriggerEventHandler(HandleHotkeys));
			}
			Program.HotkeyManager.UpdateHotkeys(Program.HotkeysConfig.Hotkeys, !Program.IgnoreHotkeyWarning);
			DebugHelper.WriteLine("HotkeyManager started.");
			if (Program.WatchFolderManager == null)
			{
				Program.WatchFolderManager = new WatchFolderManager();
			}
			Program.WatchFolderManager.UpdateWatchFolders();
			DebugHelper.WriteLine("WatchFolderManager started.");
			UpdateWorkflowsMenu();
		});
	}

	private async void HandleHotkeys(HotkeySettings hotkeySetting)
	{
		DebugHelper.WriteLine("Hotkey triggered. " + hotkeySetting);
		await TaskHelpers.ExecuteJob(hotkeySetting.TaskSettings);
	}

	private void UpdateWorkflowsMenu()
	{
		tsddbWorkflows.DropDownItems.Clear();
		tsmiTrayWorkflows.DropDownItems.Clear();
		foreach (HotkeySettings hotkey in Program.HotkeysConfig.Hotkeys)
		{
			if (hotkey.TaskSettings.Job != 0 && (!Program.Settings.WorkflowsOnlyShowEdited || !hotkey.TaskSettings.IsUsingDefaultSettings))
			{
				tsddbWorkflows.DropDownItems.Add(WorkflowMenuItem(hotkey));
				tsmiTrayWorkflows.DropDownItems.Add(WorkflowMenuItem(hotkey));
			}
		}
		if (tsddbWorkflows.DropDownItems.Count > 0)
		{
			ToolStripSeparator value = new ToolStripSeparator();
			tsddbWorkflows.DropDownItems.Add(value);
		}
		ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(Resources.MainForm_UpdateWorkflowsMenu_You_can_add_workflows_from_hotkey_settings___);
		toolStripMenuItem.Click += tsbHotkeySettings_Click;
		tsddbWorkflows.DropDownItems.Add(toolStripMenuItem);
		tsmiTrayWorkflows.Visible = tsmiTrayWorkflows.DropDownItems.Count > 0;
		UpdateMainFormTip();
	}

	private void UpdateMainFormTip()
	{
		TaskManager.UpdateMainFormTip();
		List<HotkeySettings> list = Program.HotkeysConfig.Hotkeys.Where((HotkeySettings x) => x.HotkeyInfo.IsValidHotkey).ToList();
		if (list.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = list.Max((HotkeySettings x) => x.HotkeyInfo.ToString().Length);
			int num2 = list.Max((HotkeySettings x) => x.TaskSettings.ToString().Length);
			stringBuilder.AppendFormat("┌{0}┬{1}┐\r\n", Resources.Hotkey.PadCenter(num + 2, '─'), Resources.Description.PadCenter(num2 + 2, '─'));
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.AppendFormat("│ {0} │ {1} │\r\n", list[i].HotkeyInfo.ToString().PadRight(num), list[i].TaskSettings.ToString().PadRight(num2));
				if (i + 1 < list.Count)
				{
					stringBuilder.AppendFormat("├{0}┼{1}┤\r\n", new string('─', num + 2), new string('─', num2 + 2));
				}
			}
			stringBuilder.AppendFormat("└{0}┴{1}┘", new string('─', num + 2), new string('─', num2 + 2));
			string text3 = (lblListViewTip.Text = (lblThumbnailViewTip.Text = stringBuilder.ToString()));
		}
		else
		{
			string text3 = (lblListViewTip.Text = (lblThumbnailViewTip.Text = ""));
		}
	}

	private ToolStripMenuItem WorkflowMenuItem(HotkeySettings hotkeySetting)
	{
		ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(hotkeySetting.TaskSettings.ToString().Replace("&", "&&"));
		if (hotkeySetting.HotkeyInfo.IsValidHotkey)
		{
			toolStripMenuItem.ShortcutKeyDisplayString = "  " + hotkeySetting.HotkeyInfo;
		}
		if (!hotkeySetting.TaskSettings.IsUsingDefaultSettings)
		{
			toolStripMenuItem.Font = new Font(toolStripMenuItem.Font, FontStyle.Bold);
		}
		toolStripMenuItem.Click += async delegate
		{
			await TaskHelpers.ExecuteJob(hotkeySetting.TaskSettings);
		};
		return toolStripMenuItem;
	}

	private void UpdateDestinationStates()
	{
		if (Program.UploadersConfig != null)
		{
			EnableDisableToolStripMenuItems<ImageDestination>(new ToolStripDropDownItem[2] { tsmiImageUploaders, tsmiTrayImageUploaders });
			EnableDisableToolStripMenuItems<FileDestination>(new ToolStripDropDownItem[2] { tsmiImageFileUploaders, tsmiTrayImageFileUploaders });
			EnableDisableToolStripMenuItems<TextDestination>(new ToolStripDropDownItem[2] { tsmiTextUploaders, tsmiTrayTextUploaders });
			EnableDisableToolStripMenuItems<FileDestination>(new ToolStripDropDownItem[2] { tsmiTextFileUploaders, tsmiTrayTextFileUploaders });
			EnableDisableToolStripMenuItems<FileDestination>(new ToolStripDropDownItem[2] { tsmiFileUploaders, tsmiTrayFileUploaders });
			EnableDisableToolStripMenuItems<UrlShortenerType>(new ToolStripDropDownItem[2] { tsmiURLShorteners, tsmiTrayURLShorteners });
			EnableDisableToolStripMenuItems<URLSharingServices>(new ToolStripDropDownItem[2] { tsmiURLSharingServices, tsmiTrayURLSharingServices });
		}
	}

	private void AddEnumItems<T>(Action<T> selectedEnum, params ToolStripDropDownItem[] parents) where T : Enum
	{
		T[] enums = Helpers.GetEnums<T>();
		ToolStripDropDownItem[] array = parents;
		foreach (ToolStripDropDownItem toolStripDropDownItem in array)
		{
			for (int j = 0; j < enums.Length; j++)
			{
				T currentEnum = enums[j];
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(currentEnum.GetLocalizedDescription());
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
					selectedEnum(currentEnum);
					UpdateUploaderMenuNames();
				};
				toolStripDropDownItem.DropDownItems.Add(toolStripMenuItem);
			}
		}
	}

	public static void Uncheck(params ToolStripDropDownItem[] lists)
	{
		for (int i = 0; i < lists.Length; i++)
		{
			foreach (ToolStripItem dropDownItem in lists[i].DropDownItems)
			{
				((ToolStripMenuItem)dropDownItem).Checked = false;
			}
		}
	}

	private static void SetEnumChecked(Enum value, params ToolStripDropDownItem[] parents)
	{
		if (value != null)
		{
			int index = value.GetIndex();
			for (int i = 0; i < parents.Length; i++)
			{
				((ToolStripMenuItem)parents[i].DropDownItems[index]).RadioCheck();
			}
		}
	}

	private void AddMultiEnumItems<T>(Action<T> selectedEnum, params ToolStripDropDownItem[] parents) where T : Enum
	{
		T[] array = Helpers.GetEnums<T>().Skip(1).ToArray();
		ToolStripDropDownItem[] array2 = parents;
		foreach (ToolStripDropDownItem toolStripDropDownItem in array2)
		{
			for (int j = 0; j < array.Length; j++)
			{
				T currentEnum = array[j];
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(currentEnum.GetLocalizedDescription());
				toolStripMenuItem.Image = TaskHelpers.FindMenuIcon(currentEnum);
				int index = j;
				toolStripMenuItem.Click += delegate
				{
					ToolStripDropDownItem[] array3 = parents;
					for (int k = 0; k < array3.Length; k++)
					{
						ToolStripMenuItem obj = (ToolStripMenuItem)array3[k].DropDownItems[index];
						obj.Checked = !obj.Checked;
					}
					selectedEnum(currentEnum);
					UpdateUploaderMenuNames();
				};
				toolStripDropDownItem.DropDownItems.Add(toolStripMenuItem);
			}
		}
	}

	private void UpdateImageEffectsMenu(ToolStripDropDownItem parent)
	{
		int index = AfterCaptureTasks.AddImageEffects.GetIndex() - 1;
		ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)parent.DropDownItems[index];
		toolStripMenuItem.DisableMenuCloseOnClick();
		toolStripMenuItem.DropDownItems.Clear();
		if (Program.DefaultTaskSettings.ImageSettings.ImageEffectPresets == null)
		{
			Program.DefaultTaskSettings.ImageSettings.ImageEffectPresets = new List<ImageEffectPreset>();
		}
		int count = Program.DefaultTaskSettings.ImageSettings.ImageEffectPresets.Count;
		if (count <= 0)
		{
			return;
		}
		List<ToolStripItem> list = new List<ToolStripItem>();
		for (int i = 0; i < count; i++)
		{
			ImageEffectPreset imageEffectPreset = Program.DefaultTaskSettings.ImageSettings.ImageEffectPresets[i];
			if (imageEffectPreset != null)
			{
				ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(imageEffectPreset.ToString());
				toolStripMenuItem2.Checked = i == Program.DefaultTaskSettings.ImageSettings.SelectedImageEffectPreset;
				int indexSelected = i;
				toolStripMenuItem2.Click += delegate(object sender, EventArgs e)
				{
					Program.DefaultTaskSettings.ImageSettings.SelectedImageEffectPreset = indexSelected;
					((ToolStripMenuItem)sender).RadioCheck();
				};
				list.Add(toolStripMenuItem2);
			}
		}
		if (list.Count > 0)
		{
			toolStripMenuItem.DropDownItems.AddRange(list.ToArray());
		}
	}

	private void SetMultiEnumChecked(Enum value, params ToolStripDropDownItem[] parents)
	{
		for (int i = 0; i < parents[0].DropDownItems.Count; i++)
		{
			for (int j = 0; j < parents.Length; j++)
			{
				((ToolStripMenuItem)parents[j].DropDownItems[i]).Checked = value.HasFlag<int>(1 << i);
			}
		}
	}

	private void EnableDisableToolStripMenuItems<T>(params ToolStripDropDownItem[] parents)
	{
		foreach (ToolStripDropDownItem toolStripDropDownItem in parents)
		{
			for (int j = 0; j < toolStripDropDownItem.DropDownItems.Count; j++)
			{
				toolStripDropDownItem.DropDownItems[j].ForeColor = (UploadersConfigValidator.Validate<T>(j, Program.UploadersConfig) ? SystemColors.ControlText : Color.FromArgb(200, 0, 0));
			}
		}
	}

	private void UpdateInfoManager()
	{
		cmsTaskInfo.SuspendLayout();
		ToolStripMenuItem toolStripMenuItem = tsmiStopUpload;
		ToolStripMenuItem toolStripMenuItem2 = tsmiOpen;
		ToolStripMenuItem toolStripMenuItem3 = tsmiCopy;
		ToolStripMenuItem toolStripMenuItem4 = tsmiShowErrors;
		ToolStripMenuItem toolStripMenuItem5 = tsmiShowResponse;
		ToolStripMenuItem toolStripMenuItem6 = tsmiGoogleImageSearch;
		ToolStripMenuItem toolStripMenuItem7 = tsmiBingVisualSearch;
		ToolStripMenuItem toolStripMenuItem8 = tsmiShowQRCode;
		ToolStripMenuItem toolStripMenuItem9 = tsmiOCRImage;
		ToolStripMenuItem toolStripMenuItem10 = tsmiCombineImages;
		ToolStripMenuItem toolStripMenuItem11 = tsmiUploadSelectedFile;
		ToolStripMenuItem toolStripMenuItem12 = tsmiDownloadSelectedURL;
		ToolStripMenuItem toolStripMenuItem13 = tsmiEditSelectedFile;
		ToolStripMenuItem toolStripMenuItem14 = tsmiAddImageEffects;
		ToolStripMenuItem toolStripMenuItem15 = tsmiRunAction;
		ToolStripMenuItem toolStripMenuItem16 = tsmiDeleteSelectedItem;
		ToolStripMenuItem toolStripMenuItem17 = tsmiDeleteSelectedFile;
		ToolStripMenuItem toolStripMenuItem18 = tsmiShortenSelectedURL;
		bool flag2 = (tsmiShareSelectedURL.Visible = false);
		bool flag4 = (toolStripMenuItem18.Visible = flag2);
		bool flag6 = (toolStripMenuItem17.Visible = flag4);
		bool flag8 = (toolStripMenuItem16.Visible = flag6);
		bool flag10 = (toolStripMenuItem15.Visible = flag8);
		bool flag12 = (toolStripMenuItem14.Visible = flag10);
		bool flag14 = (toolStripMenuItem13.Visible = flag12);
		bool flag16 = (toolStripMenuItem12.Visible = flag14);
		bool flag18 = (toolStripMenuItem11.Visible = flag16);
		bool flag20 = (toolStripMenuItem10.Visible = flag18);
		bool flag22 = (toolStripMenuItem9.Visible = flag20);
		bool flag24 = (toolStripMenuItem8.Visible = flag22);
		bool flag26 = (toolStripMenuItem7.Visible = flag24);
		bool flag28 = (toolStripMenuItem6.Visible = flag26);
		bool flag30 = (toolStripMenuItem5.Visible = flag28);
		bool flag32 = (toolStripMenuItem4.Visible = flag30);
		bool flag34 = (toolStripMenuItem3.Visible = flag32);
		bool visible = (toolStripMenuItem2.Visible = flag34);
		toolStripMenuItem.Visible = visible;
		if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
		{
			pbPreview.Reset();
			uim.UpdateSelectedItems(from ListViewItem x in lvUploads.SelectedItems
				select x.Tag as WorkerTask);
			switch (Program.Settings.ImagePreview)
			{
			case ImagePreviewVisibility.Show:
				scMain.Panel2Collapsed = false;
				break;
			case ImagePreviewVisibility.Hide:
				scMain.Panel2Collapsed = true;
				break;
			case ImagePreviewVisibility.Automatic:
				scMain.Panel2Collapsed = !uim.IsItemSelected || (!uim.SelectedItem.IsImageFile && !uim.SelectedItem.IsImageURL);
				break;
			}
			switch (Program.Settings.ImagePreviewLocation)
			{
			case ImagePreviewLocation.Side:
				scMain.Orientation = Orientation.Vertical;
				break;
			case ImagePreviewLocation.Bottom:
				scMain.Orientation = Orientation.Horizontal;
				break;
			}
		}
		else if (Program.Settings.TaskViewMode == TaskViewMode.ThumbnailView)
		{
			uim.UpdateSelectedItems(ucTaskThumbnailView.SelectedPanels.Select((TaskThumbnailPanel x) => x.Task));
		}
		if (uim.IsItemSelected)
		{
			tsmiOpen.Visible = true;
			tsmiOpenURL.Enabled = uim.SelectedItem.IsURLExist;
			tsmiOpenShortenedURL.Enabled = uim.SelectedItem.IsShortenedURLExist;
			tsmiOpenThumbnailURL.Enabled = uim.SelectedItem.IsThumbnailURLExist;
			tsmiOpenDeletionURL.Enabled = uim.SelectedItem.IsDeletionURLExist;
			tsmiOpenFile.Enabled = uim.SelectedItem.IsFileExist;
			tsmiOpenFolder.Enabled = uim.SelectedItem.IsFileExist;
			tsmiOpenThumbnailFile.Enabled = uim.SelectedItem.IsThumbnailFileExist;
			if (uim.SelectedItems != null && uim.SelectedItems.Any((UploadInfoStatus x) => x.Task.IsWorking))
			{
				tsmiStopUpload.Visible = true;
			}
			else
			{
				tsmiShowErrors.Visible = uim.SelectedItem.Info.Result.IsError;
				tsmiCopy.Visible = true;
				tsmiCopyURL.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsURLExist);
				tsmiCopyShortenedURL.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsShortenedURLExist);
				tsmiCopyThumbnailURL.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsThumbnailURLExist);
				tsmiCopyDeletionURL.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsDeletionURLExist);
				tsmiCopyFile.Enabled = uim.SelectedItem.IsFileExist;
				tsmiCopyImage.Enabled = uim.SelectedItem.IsImageFile;
				tsmiCopyImageDimensions.Enabled = uim.SelectedItem.IsImageFile;
				tsmiCopyText.Enabled = uim.SelectedItem.IsTextFile;
				tsmiCopyThumbnailFile.Enabled = uim.SelectedItem.IsThumbnailFileExist;
				tsmiCopyThumbnailImage.Enabled = uim.SelectedItem.IsThumbnailFileExist;
				tsmiCopyHTMLLink.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsURLExist);
				tsmiCopyHTMLImage.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsImageURL);
				tsmiCopyHTMLLinkedImage.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsImageURL && x.IsThumbnailURLExist);
				tsmiCopyForumLink.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsURLExist);
				tsmiCopyForumImage.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsImageURL && x.IsURLExist);
				tsmiCopyForumLinkedImage.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsImageURL && x.IsThumbnailURLExist);
				tsmiCopyMarkdownLink.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsURLExist);
				tsmiCopyMarkdownImage.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsImageURL);
				tsmiCopyMarkdownLinkedImage.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsImageURL && x.IsThumbnailURLExist);
				tsmiCopyFilePath.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsFilePathValid);
				tsmiCopyFileName.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsFilePathValid);
				tsmiCopyFileNameWithExtension.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsFilePathValid);
				tsmiCopyFolder.Enabled = uim.SelectedItems.Any((UploadInfoStatus x) => x.IsFilePathValid);
				CleanCustomClipboardFormats();
				if (Program.Settings.ClipboardContentFormats != null && Program.Settings.ClipboardContentFormats.Count > 0)
				{
					tssCopy6.Visible = true;
					foreach (ClipboardFormat clipboardContentFormat in Program.Settings.ClipboardContentFormats)
					{
						ToolStripMenuItem toolStripMenuItem19 = new ToolStripMenuItem(clipboardContentFormat.Description);
						toolStripMenuItem19.Tag = clipboardContentFormat;
						toolStripMenuItem19.Click += tsmiClipboardFormat_Click;
						tsmiCopy.DropDownItems.Add(toolStripMenuItem19);
					}
				}
				tsmiUploadSelectedFile.Visible = uim.SelectedItem.IsFileExist;
				tsmiDownloadSelectedURL.Visible = uim.SelectedItem.IsFileURL;
				tsmiEditSelectedFile.Visible = uim.SelectedItem.IsImageFile;
				tsmiAddImageEffects.Visible = uim.SelectedItem.IsImageFile;
				UpdateActionsMenu(uim.SelectedItem.Info.FilePath);
				tsmiDeleteSelectedItem.Visible = true;
				tsmiDeleteSelectedFile.Visible = uim.SelectedItem.IsFileExist;
				tsmiShortenSelectedURL.Visible = uim.SelectedItem.IsURLExist;
				tsmiShareSelectedURL.Visible = uim.SelectedItem.IsURLExist;
				tsmiGoogleImageSearch.Visible = uim.SelectedItem.IsURLExist;
				tsmiBingVisualSearch.Visible = uim.SelectedItem.IsURLExist;
				tsmiShowQRCode.Visible = uim.SelectedItem.IsURLExist;
				tsmiOCRImage.Visible = uim.SelectedItem.IsImageFile;
				tsmiCombineImages.Visible = uim.SelectedItems.Count((UploadInfoStatus x) => x.IsImageFile) > 1;
				tsmiShowResponse.Visible = !string.IsNullOrEmpty(uim.SelectedItem.Info.Result.Response);
			}
			if (Program.Settings.TaskViewMode == TaskViewMode.ListView && !scMain.Panel2Collapsed)
			{
				if (uim.SelectedItem.IsImageFile)
				{
					pbPreview.LoadImageFromFileAsync(uim.SelectedItem.Info.FilePath);
				}
				else if (uim.SelectedItem.IsImageURL)
				{
					pbPreview.LoadImageFromURLAsync(uim.SelectedItem.Info.Result.URL);
				}
			}
		}
		ToolStripMenuItem toolStripMenuItem20 = tsmiClearList;
		visible = (tssUploadInfo1.Visible = lvUploads.Items.Count > 0);
		toolStripMenuItem20.Visible = visible;
		cmsTaskInfo.ResumeLayout();
		Refresh();
	}

	private void UpdateTaskViewMode()
	{
		if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
		{
			tsmiSwitchTaskViewMode.Text = Resources.SwitchToThumbnailView;
			tsmiSwitchTaskViewMode.Image = Resources.application_icon_large;
			scMain.Visible = true;
			pThumbnailView.Visible = false;
			scMain.Focus();
		}
		else
		{
			tsmiSwitchTaskViewMode.Text = Resources.SwitchToListView;
			tsmiSwitchTaskViewMode.Image = Resources.application_list;
			pThumbnailView.Visible = true;
			scMain.Visible = false;
			pThumbnailView.Focus();
		}
	}

	public void UpdateTheme()
	{
		if (Program.Settings.Themes == null || Program.Settings.Themes.Count == 0)
		{
			Program.Settings.Themes = ShareXTheme.GetDefaultThemes();
			Program.Settings.SelectedTheme = 0;
		}
		if (!Program.Settings.Themes.IsValidIndex(Program.Settings.SelectedTheme))
		{
			Program.Settings.SelectedTheme = 0;
		}
		ShareXResources.Theme = Program.Settings.Themes[Program.Settings.SelectedTheme];
		ShareXResources.UseCustomTheme = Program.Settings.UseCustomTheme;
		if (base.IsHandleCreated)
		{
			NativeMethods.UseImmersiveDarkMode(base.Handle, ShareXResources.IsDarkTheme);
		}
		if (ShareXResources.UseCustomTheme)
		{
			tsMain.Font = ShareXResources.Theme.MenuFont;
			tsMain.Renderer = new ToolStripDarkRenderer();
			tsMain.DrawCustomBorder = false;
			ShareXResources.ApplyCustomThemeToContextMenuStrip(cmsTray);
			ShareXResources.ApplyCustomThemeToContextMenuStrip(cmsTaskInfo);
			ttMain.BackColor = ShareXResources.Theme.BackgroundColor;
			ttMain.ForeColor = ShareXResources.Theme.TextColor;
			lvUploads.BackColor = ShareXResources.Theme.BackgroundColor;
			lvUploads.ForeColor = ShareXResources.Theme.TextColor;
			lblListViewTip.ForeColor = ShareXResources.Theme.TextColor;
			scMain.SplitterColor = ShareXResources.Theme.BackgroundColor;
			scMain.SplitterLineColor = ShareXResources.Theme.BorderColor;
			pThumbnailView.BackColor = ShareXResources.Theme.BackgroundColor;
			lblThumbnailViewTip.ForeColor = ShareXResources.Theme.TextColor;
		}
		else
		{
			tsMain.Renderer = new ToolStripCustomRenderer();
			tsMain.DrawCustomBorder = true;
			cmsTray.Renderer = new ToolStripCustomRenderer();
			cmsTray.Opacity = 1.0;
			cmsTaskInfo.Renderer = new ToolStripCustomRenderer();
			cmsTaskInfo.Opacity = 1.0;
			ttMain.BackColor = SystemColors.Window;
			ttMain.ForeColor = SystemColors.ControlText;
			lvUploads.BackColor = SystemColors.Window;
			lvUploads.ForeColor = SystemColors.ControlText;
			lblListViewTip.ForeColor = Color.Silver;
			scMain.SplitterColor = Color.White;
			scMain.SplitterLineColor = ProfessionalColors.SeparatorDark;
			pThumbnailView.BackColor = SystemColors.Window;
			lblThumbnailViewTip.ForeColor = Color.Silver;
		}
		if (ShareXResources.IsDarkTheme)
		{
			tsmiQRCode.Image = Resources.barcode_2d_white;
			tsmiTrayQRCode.Image = Resources.barcode_2d_white;
			tsmiShowQRCode.Image = Resources.barcode_2d_white;
			tsmiOCR.Image = Resources.edit_drop_cap_white;
			tsmiTrayOCR.Image = Resources.edit_drop_cap_white;
			tsmiOCRImage.Image = Resources.edit_drop_cap_white;
			tsmiShortenURL.Image = Resources.edit_scale_white;
			tsmiTrayShortenURL.Image = Resources.edit_scale_white;
			tsmiURLShorteners.Image = Resources.edit_scale_white;
			tsmiTrayURLShorteners.Image = Resources.edit_scale_white;
			tsmiTestURLShortener.Image = Resources.edit_scale_white;
			tsmiShortenSelectedURL.Image = Resources.edit_scale_white;
		}
		else
		{
			tsmiQRCode.Image = Resources.barcode_2d;
			tsmiTrayQRCode.Image = Resources.barcode_2d;
			tsmiShowQRCode.Image = Resources.barcode_2d;
			tsmiOCR.Image = Resources.edit_drop_cap;
			tsmiTrayOCR.Image = Resources.edit_drop_cap;
			tsmiOCRImage.Image = Resources.edit_drop_cap;
			tsmiShortenURL.Image = Resources.edit_scale;
			tsmiTrayShortenURL.Image = Resources.edit_scale;
			tsmiURLShorteners.Image = Resources.edit_scale;
			tsmiTrayURLShorteners.Image = Resources.edit_scale;
			tsmiTestURLShortener.Image = Resources.edit_scale;
			tsmiShortenSelectedURL.Image = Resources.edit_scale;
		}
		pbPreview.UpdateTheme();
		pbPreview.UpdateCheckers(forceUpdate: true);
		ucTaskThumbnailView.UpdateTheme();
	}

	private void CleanCustomClipboardFormats()
	{
		tssCopy6.Visible = false;
		int num = tsmiCopy.DropDownItems.IndexOf(tssCopy6);
		while (num < tsmiCopy.DropDownItems.Count - 1)
		{
			using ToolStripItem value = tsmiCopy.DropDownItems[tsmiCopy.DropDownItems.Count - 1];
			tsmiCopy.DropDownItems.Remove(value);
		}
	}

	private void UpdateActionsMenu(string filePath)
	{
		tsmiRunAction.DropDownItems.Clear();
		if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
		{
			return;
		}
		IEnumerable<ExternalProgram> enumerable = Program.DefaultTaskSettings.ExternalPrograms.Where((ExternalProgram x) => !string.IsNullOrEmpty(x.Name) && x.CheckExtension(filePath));
		if (enumerable.Count() <= 0)
		{
			return;
		}
		tsmiRunAction.Visible = true;
		foreach (ExternalProgram action in enumerable)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(action.Name.Truncate(50, "..."));
			try
			{
				string fullPath = action.GetFullPath();
				toolStripMenuItem.Image = actionsMenuIconCache.GetFileIconAsImage(fullPath);
			}
			catch
			{
			}
			toolStripMenuItem.Click += async delegate
			{
				await action.RunAsync(filePath);
			};
			tsmiRunAction.DropDownItems.Add(toolStripMenuItem);
		}
	}

	private void AfterApplicationSettingsJobs()
	{
		base.HotkeyRepeatLimit = Program.Settings.HotkeyRepeatLimit;
		HelpersOptions.CurrentProxy = Program.Settings.ProxySettings;
		HelpersOptions.AcceptInvalidSSLCertificates = Program.Settings.AcceptInvalidSSLCertificates;
		HelpersOptions.URLEncodeIgnoreEmoji = Program.Settings.URLEncodeIgnoreEmoji;
		HelpersOptions.DefaultCopyImageFillBackground = Program.Settings.DefaultClipboardCopyImageFillBackground;
		HelpersOptions.UseAlternativeClipboardCopyImage = Program.Settings.UseAlternativeClipboardCopyImage;
		HelpersOptions.UseAlternativeClipboardGetImage = Program.Settings.UseAlternativeClipboardGetImage;
		HelpersOptions.RotateImageByExifOrientationData = Program.Settings.RotateImageByExifOrientationData;
		HelpersOptions.BrowserPath = Program.Settings.BrowserPath;
		HelpersOptions.RecentColors = Program.Settings.RecentColors;
		HelpersOptions.DevMode = Program.Settings.DevMode;
		Program.UpdateHelpersSpecialFolders();
		TaskManager.RecentManager.MaxCount = Program.Settings.RecentTasksMaxCount;
		UpdateTheme();
		Refresh();
		if (ShareXResources.UseWhiteIcon != Program.Settings.UseWhiteShareXIcon)
		{
			ShareXResources.UseWhiteIcon = Program.Settings.UseWhiteShareXIcon;
			base.Icon = ShareXResources.Icon;
			niTray.Icon = ShareXResources.Icon;
		}
		Text = Program.Title;
		niTray.Text = Program.TitleShort;
		tsmiRestartAsAdmin.Visible = HelpersOptions.DevMode && !Helpers.IsAdministrator();
		ConfigureAutoUpdate();
		UpdateTaskViewMode();
		UpdateMainWindowLayout();
		UpdateInfoManager();
	}

	private void ConfigureAutoUpdate()
	{
		Program.UpdateManager.AutoUpdateEnabled = !SystemOptions.DisableUpdateCheck && Program.Settings.AutoCheckUpdate;
		Program.UpdateManager.CheckPreReleaseUpdates = Program.Settings.CheckPreReleaseUpdates;
		Program.UpdateManager.ConfigureAutoUpdate();
	}

	private void AfterTaskSettingsJobs()
	{
		ToolStripMenuItem toolStripMenuItem = tsmiShowCursor;
		bool @checked = (tsmiTrayShowCursor.Checked = Program.DefaultTaskSettings.CaptureSettings.ShowCursor);
		toolStripMenuItem.Checked = @checked;
		SetScreenshotDelay(Program.DefaultTaskSettings.CaptureSettings.ScreenshotDelay);
	}

	public void UpdateCheckStates()
	{
		SetMultiEnumChecked(Program.DefaultTaskSettings.AfterCaptureJob, tsddbAfterCaptureTasks, tsmiTrayAfterCaptureTasks);
		SetMultiEnumChecked(Program.DefaultTaskSettings.AfterUploadJob, tsddbAfterUploadTasks, tsmiTrayAfterUploadTasks);
		SetEnumChecked(Program.DefaultTaskSettings.ImageDestination, tsmiImageUploaders, tsmiTrayImageUploaders);
		SetImageFileDestinationChecked(Program.DefaultTaskSettings.ImageDestination, Program.DefaultTaskSettings.ImageFileDestination, tsmiImageFileUploaders, tsmiTrayImageFileUploaders);
		SetEnumChecked(Program.DefaultTaskSettings.TextDestination, tsmiTextUploaders, tsmiTrayTextUploaders);
		SetTextFileDestinationChecked(Program.DefaultTaskSettings.TextDestination, Program.DefaultTaskSettings.TextFileDestination, tsmiTextFileUploaders, tsmiTrayTextFileUploaders);
		SetEnumChecked(Program.DefaultTaskSettings.FileDestination, tsmiFileUploaders, tsmiTrayFileUploaders);
		SetEnumChecked(Program.DefaultTaskSettings.URLShortenerDestination, tsmiURLShorteners, tsmiTrayURLShorteners);
		SetEnumChecked(Program.DefaultTaskSettings.URLSharingServiceDestination, tsmiURLSharingServices, tsmiTrayURLSharingServices);
	}

	public static void SetTextFileDestinationChecked(TextDestination textDestination, FileDestination textFileDestination, params ToolStripDropDownItem[] lists)
	{
		if (textDestination == TextDestination.FileUploader)
		{
			SetEnumChecked(textFileDestination, lists);
		}
		else
		{
			Uncheck(lists);
		}
	}

	public static void SetImageFileDestinationChecked(ImageDestination imageDestination, FileDestination imageFileDestination, params ToolStripDropDownItem[] lists)
	{
		if (imageDestination == ImageDestination.FileUploader)
		{
			SetEnumChecked(imageFileDestination, lists);
		}
		else
		{
			Uncheck(lists);
		}
	}

	public void UpdateUploaderMenuNames()
	{
		string arg = ((Program.DefaultTaskSettings.ImageDestination == ImageDestination.FileUploader) ? Program.DefaultTaskSettings.ImageFileDestination.GetLocalizedDescription() : Program.DefaultTaskSettings.ImageDestination.GetLocalizedDescription());
		string text3 = (tsmiImageUploaders.Text = (tsmiTrayImageUploaders.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_Image_uploader___0_, arg)));
		string arg2 = ((Program.DefaultTaskSettings.TextDestination == TextDestination.FileUploader) ? Program.DefaultTaskSettings.TextFileDestination.GetLocalizedDescription() : Program.DefaultTaskSettings.TextDestination.GetLocalizedDescription());
		text3 = (tsmiTextUploaders.Text = (tsmiTrayTextUploaders.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_Text_uploader___0_, arg2)));
		text3 = (tsmiFileUploaders.Text = (tsmiTrayFileUploaders.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_File_uploader___0_, Program.DefaultTaskSettings.FileDestination.GetLocalizedDescription())));
		text3 = (tsmiURLShorteners.Text = (tsmiTrayURLShorteners.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_URL_shortener___0_, Program.DefaultTaskSettings.URLShortenerDestination.GetLocalizedDescription())));
		text3 = (tsmiURLSharingServices.Text = (tsmiTrayURLSharingServices.Text = string.Format(Resources.TaskSettingsForm_UpdateUploaderMenuNames_URL_sharing_service___0_, Program.DefaultTaskSettings.URLSharingServiceDestination.GetLocalizedDescription())));
	}

	private WorkerTask[] GetSelectedTasks()
	{
		if (lvUploads.SelectedItems.Count > 0)
		{
			return (from ListViewItem x in lvUploads.SelectedItems
				select x.Tag as WorkerTask into x
				where x != null
				select x).ToArray();
		}
		return null;
	}

	private void RemoveTasks(WorkerTask[] tasks)
	{
		if (tasks == null)
		{
			return;
		}
		foreach (WorkerTask item in tasks.Where((WorkerTask x) => x != null && !x.IsWorking))
		{
			TaskManager.Remove(item);
		}
		UpdateInfoManager();
	}

	private void RemoveSelectedItems()
	{
		IEnumerable<WorkerTask> source = null;
		if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
		{
			source = from ListViewItem x in lvUploads.SelectedItems
				select x.Tag as WorkerTask;
		}
		else if (Program.Settings.TaskViewMode == TaskViewMode.ThumbnailView)
		{
			source = ucTaskThumbnailView.SelectedPanels.Select((TaskThumbnailPanel x) => x.Task);
		}
		RemoveTasks(source.ToArray());
	}

	private void RemoveAllItems()
	{
		RemoveTasks((from ListViewItem x in lvUploads.Items
			select x.Tag as WorkerTask).ToArray());
	}

	private void UpdateMainWindowLayout()
	{
		tsMain.Visible = Program.Settings.ShowMenu;
		ucTaskThumbnailView.TitleVisible = Program.Settings.ShowThumbnailTitle;
		ucTaskThumbnailView.TitleLocation = Program.Settings.ThumbnailTitleLocation;
		ucTaskThumbnailView.ThumbnailSize = Program.Settings.ThumbnailSize;
		ucTaskThumbnailView.ClickAction = Program.Settings.ThumbnailClickAction;
		lvUploads.HeaderStyle = (Program.Settings.ShowColumns ? ColumnHeaderStyle.Nonclickable : ColumnHeaderStyle.None);
		Refresh();
	}

	public void UpdateToggleHotkeyButton()
	{
		if (Program.Settings.DisableHotkeys)
		{
			tsmiTrayToggleHotkeys.Text = Resources.MainForm_UpdateToggleHotkeyButton_Enable_hotkeys;
			tsmiTrayToggleHotkeys.Image = Resources.keyboard__plus;
		}
		else
		{
			tsmiTrayToggleHotkeys.Text = Resources.MainForm_UpdateToggleHotkeyButton_Disable_hotkeys;
			tsmiTrayToggleHotkeys.Image = Resources.keyboard__minus;
		}
	}

	private void RunPuushTasks()
	{
		if (!Program.PuushMode || !Program.Settings.IsFirstTimeRun)
		{
			return;
		}
		using PuushLoginForm puushLoginForm = new PuushLoginForm();
		if (puushLoginForm.ShowDialog() == DialogResult.OK)
		{
			Program.DefaultTaskSettings.ImageDestination = ImageDestination.FileUploader;
			Program.DefaultTaskSettings.ImageFileDestination = FileDestination.Puush;
			Program.DefaultTaskSettings.TextDestination = TextDestination.FileUploader;
			Program.DefaultTaskSettings.TextFileDestination = FileDestination.Puush;
			Program.DefaultTaskSettings.FileDestination = FileDestination.Puush;
			SettingManager.WaitUploadersConfig();
			if (Program.UploadersConfig != null)
			{
				Program.UploadersConfig.PuushAPIKey = puushLoginForm.APIKey;
			}
		}
	}

	private void SetScreenshotDelay(decimal delay)
	{
		Program.DefaultTaskSettings.CaptureSettings.ScreenshotDelay = delay;
		if (delay <= 2m)
		{
			if (!(delay == 0m))
			{
				if (!(delay == 1m))
				{
					if (!(delay == 2m))
					{
						goto IL_007e;
					}
					tsmiScreenshotDelay2.RadioCheck();
					tsmiTrayScreenshotDelay2.RadioCheck();
				}
				else
				{
					tsmiScreenshotDelay1.RadioCheck();
					tsmiTrayScreenshotDelay1.RadioCheck();
				}
			}
			else
			{
				tsmiScreenshotDelay0.RadioCheck();
				tsmiTrayScreenshotDelay0.RadioCheck();
			}
		}
		else if (!(delay == 3m))
		{
			if (!(delay == 4m))
			{
				if (!(delay == 5m))
				{
					goto IL_007e;
				}
				tsmiScreenshotDelay5.RadioCheck();
				tsmiTrayScreenshotDelay5.RadioCheck();
			}
			else
			{
				tsmiScreenshotDelay4.RadioCheck();
				tsmiTrayScreenshotDelay4.RadioCheck();
			}
		}
		else
		{
			tsmiScreenshotDelay3.RadioCheck();
			tsmiTrayScreenshotDelay3.RadioCheck();
		}
		goto IL_0129;
		IL_0129:
		string text3 = (tsmiScreenshotDelay.Text = (tsmiTrayScreenshotDelay.Text = string.Format(Resources.ScreenshotDelay0S, delay.ToString("0.#"))));
		ToolStripMenuItem toolStripMenuItem = tsmiScreenshotDelay;
		bool @checked = (tsmiTrayScreenshotDelay.Checked = delay > 0m);
		toolStripMenuItem.Checked = @checked;
		return;
		IL_007e:
		tsmiScreenshotDelay.UpdateCheckedAll(check: false);
		tsmiTrayScreenshotDelay.UpdateCheckedAll(check: false);
		goto IL_0129;
	}

	private async Task PrepareCaptureMenuAsync(ToolStripMenuItem tsmiWindow, EventHandler handlerWindow, ToolStripMenuItem tsmiMonitor, EventHandler handlerMonitor)
	{
		tsmiWindow.DropDownItems.Clear();
		WindowsList windowsList = new WindowsList();
		List<WindowInfo> list = await Task.Run(() => windowsList.GetVisibleWindowsList());
		if (list != null && list.Count > 0)
		{
			List<ToolStripItem> items = new List<ToolStripItem>();
			foreach (WindowInfo window in list)
			{
				try
				{
					string text = window.Text.Truncate(50, "...");
					ToolStripMenuItem tsmi = new ToolStripMenuItem(text);
					tsmi.Tag = window;
					tsmi.Click += handlerWindow;
					items.Add(tsmi);
					using Icon icon = await Task.Run(() => window.Icon);
					if (icon != null && icon.Width > 0 && icon.Height > 0)
					{
						tsmi.Image = icon.ToBitmap();
					}
				}
				catch (Exception exception)
				{
					DebugHelper.WriteException(exception);
				}
			}
			tsmiWindow.DropDownItems.AddRange(items.ToArray());
		}
		tsmiWindow.Invalidate();
		tsmiMonitor.DropDownItems.Clear();
		Screen[] allScreens = Screen.AllScreens;
		if (allScreens != null && allScreens.Length != 0)
		{
			ToolStripItem[] array = new ToolStripItem[allScreens.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Screen screen = allScreens[i];
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem($"{i + 1}. {screen.Bounds.Width}x{screen.Bounds.Height}");
				toolStripMenuItem.Tag = screen.Bounds;
				toolStripMenuItem.Click += handlerMonitor;
				array[i] = toolStripMenuItem;
			}
			tsmiMonitor.DropDownItems.AddRange(array);
		}
		tsmiMonitor.Invalidate();
	}

	public void ForceClose()
	{
		forceClose = true;
		Close();
	}

	protected override void SetVisibleCore(bool value)
	{
		if (value && !base.IsHandleCreated && (Program.SilentRun || Program.Settings.SilentRun) && Program.Settings.ShowTray)
		{
			CreateHandle();
			value = false;
		}
		base.SetVisibleCore(value);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape)
		{
			Close();
			return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void MainForm_Shown(object sender, EventArgs e)
	{
		AfterShownJobs();
	}

	private void MainForm_Resize(object sender, EventArgs e)
	{
		Refresh();
	}

	private void MainForm_LocationChanged(object sender, EventArgs e)
	{
		if (IsReady && base.WindowState == FormWindowState.Normal)
		{
			Program.Settings.MainFormPosition = base.Location;
		}
	}

	private void MainForm_SizeChanged(object sender, EventArgs e)
	{
		if (IsReady && base.WindowState == FormWindowState.Normal)
		{
			Program.Settings.MainFormSize = base.Size;
		}
	}

	private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (e.CloseReason == CloseReason.UserClosing && Program.Settings.ShowTray && !forceClose)
		{
			e.Cancel = true;
			Hide();
			SettingManager.SaveAllSettingsAsync();
			if (Program.Settings.FirstTimeMinimizeToTray)
			{
				TaskHelpers.ShowNotificationTip(Resources.ShareXIsMinimizedToTheSystemTray, "ShareX", 8000);
				Program.Settings.FirstTimeMinimizeToTray = false;
			}
		}
	}

	private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		TaskManager.StopAllTasks();
	}

	private void MainForm_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop, autoConvert: false) || e.Data.GetDataPresent(DataFormats.Bitmap, autoConvert: false) || e.Data.GetDataPresent(DataFormats.Text, autoConvert: false))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void MainForm_DragDrop(object sender, DragEventArgs e)
	{
		UploadManager.DragDropUpload(e.Data);
	}

	private void TtMain_Draw(object sender, DrawToolTipEventArgs e)
	{
		e.DrawBackground();
		e.DrawBorder();
		e.DrawText();
	}

	private void lblListViewTip_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			lvUploads.Focus();
		}
		else if (e.Button == MouseButtons.Right)
		{
			UpdateInfoManager();
			cmsTaskInfo.Show((Control)sender, e.X + 1, e.Y + 1);
		}
	}

	private async void lvUploads_SelectedIndexChanged(object sender, EventArgs e)
	{
		lvUploads.SelectedIndexChanged -= lvUploads_SelectedIndexChanged;
		await Task.Delay(1);
		lvUploads.SelectedIndexChanged += lvUploads_SelectedIndexChanged;
		UpdateInfoManager();
	}

	private void lvUploads_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			UpdateInfoManager();
			cmsTaskInfo.Show(lvUploads, e.X + 1, e.Y + 1);
		}
	}

	private void lvUploads_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			uim.TryOpen();
		}
	}

	private void scMain_SplitterMoved(object sender, SplitterEventArgs e)
	{
		Program.Settings.PreviewSplitterDistance = scMain.SplitterDistance;
	}

	private void lvUploads_KeyDown(object sender, KeyEventArgs e)
	{
		switch (e.KeyData)
		{
		default:
			return;
		case Keys.Return:
			uim.TryOpen();
			break;
		case Keys.Return | Keys.Control:
			uim.OpenFile();
			break;
		case Keys.Return | Keys.Shift:
			uim.OpenFolder();
			break;
		case Keys.C | Keys.Control:
			uim.TryCopy();
			break;
		case Keys.C | Keys.Shift:
			uim.CopyFile();
			break;
		case Keys.C | Keys.Alt:
			uim.CopyImage();
			break;
		case Keys.C | Keys.Shift | Keys.Control:
			uim.CopyFilePath();
			break;
		case Keys.X | Keys.Control:
			uim.TryCopy();
			RemoveSelectedItems();
			break;
		case Keys.V | Keys.Control:
			UploadManager.ClipboardUploadMainWindow();
			break;
		case Keys.U | Keys.Control:
			uim.Upload();
			break;
		case Keys.D | Keys.Control:
			uim.Download();
			break;
		case Keys.E | Keys.Control:
			uim.EditImage();
			break;
		case Keys.Delete:
			RemoveSelectedItems();
			break;
		case Keys.Delete | Keys.Shift:
			uim.DeleteFiles();
			RemoveSelectedItems();
			break;
		case Keys.Apps:
			if (lvUploads.SelectedItems.Count > 0)
			{
				UpdateInfoManager();
				Rectangle itemRect = lvUploads.GetItemRect(lvUploads.SelectedIndex);
				cmsTaskInfo.Show(lvUploads, new Point(itemRect.X, itemRect.Bottom));
			}
			break;
		}
		bool handled = (e.SuppressKeyPress = true);
		e.Handled = handled;
	}

	private void pbPreview_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && lvUploads.SelectedIndices.Count > 0)
		{
			string[] files = (from ListViewItem x in lvUploads.Items
				select ((WorkerTask)x.Tag).Info?.FilePath).ToArray();
			int imageIndex = lvUploads.SelectedIndices[0];
			ImageViewer.ShowImage(files, imageIndex);
		}
	}

	private void ucTaskThumbnailView_SelectedPanelChanged(object sender, EventArgs e)
	{
		UpdateInfoManager();
	}

	private void UcTaskView_ContextMenuRequested(object sender, MouseEventArgs e)
	{
		cmsTaskInfo.Show(sender as Control, e.X + 1, e.Y + 1);
	}

	private void LblThumbnailViewTip_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			UcTaskView_ContextMenuRequested(lblThumbnailViewTip, e);
		}
	}

	private void cmsTaskInfo_Closing(object sender, ToolStripDropDownClosingEventArgs e)
	{
		if (e.CloseReason == ToolStripDropDownCloseReason.Keyboard)
		{
			e.Cancel = NativeMethods.GetKeyState(93) >= 0 && NativeMethods.GetKeyState(121) >= 0 && NativeMethods.GetKeyState(27) >= 0;
		}
	}

	private void cmsTaskInfo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		if (e.KeyData == Keys.Apps)
		{
			cmsTaskInfo.Close();
		}
	}

	private void lvUploads_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
	{
		if (IsReady)
		{
			Program.Settings.TaskListViewColumnWidths = new List<int>();
			for (int i = 0; i < lvUploads.Columns.Count; i++)
			{
				Program.Settings.TaskListViewColumnWidths.Add(lvUploads.Columns[i].Width);
			}
		}
	}

	private void lvUploads_ItemDrag(object sender, ItemDragEventArgs e)
	{
		TaskInfo[] array = (from x in GetSelectedTasks()
			select x.Info into x
			where x != null
			select x).ToArray();
		if (array.Length == 0)
		{
			return;
		}
		IDataObject dataObject = null;
		if (Control.ModifierKeys.HasFlag(Keys.Control))
		{
			string[] array2 = (from x in array
				select x.ToString() into x
				where !string.IsNullOrEmpty(x)
				select x).ToArray();
			if (array2.Length != 0)
			{
				dataObject = new DataObject(DataFormats.Text, string.Join(Environment.NewLine, array2));
			}
		}
		else
		{
			string[] array3 = (from x in array
				select x.FilePath into x
				where !string.IsNullOrEmpty(x) && File.Exists(x)
				select x).ToArray();
			if (array3.Length != 0)
			{
				dataObject = new DataObject(DataFormats.FileDrop, array3);
			}
		}
		if (dataObject != null)
		{
			AllowDrop = false;
			lvUploads.DoDragDrop(dataObject, DragDropEffects.Copy | DragDropEffects.Move);
			AllowDrop = true;
		}
	}

	private void tsmiFullscreen_Click(object sender, EventArgs e)
	{
		new CaptureFullscreen().Capture(autoHideForm: true);
	}

	private async void tsddbCapture_DropDownOpening(object sender, EventArgs e)
	{
		await PrepareCaptureMenuAsync(tsmiWindow, tsmiWindowItems_Click, tsmiMonitor, tsmiMonitorItems_Click);
	}

	private void tsmiWindowItems_Click(object sender, EventArgs e)
	{
		if (((ToolStripItem)sender).Tag is WindowInfo windowInfo)
		{
			new CaptureWindow(windowInfo.Handle).Capture(autoHideForm: true);
		}
	}

	private void tsmiMonitorItems_Click(object sender, EventArgs e)
	{
		Rectangle monitorRectangle = (Rectangle)((ToolStripItem)sender).Tag;
		if (!monitorRectangle.IsEmpty)
		{
			new CaptureMonitor(monitorRectangle).Capture(autoHideForm: true);
		}
	}

	private void tsmiRectangle_Click(object sender, EventArgs e)
	{
		new CaptureRegion().Capture(autoHideForm: true);
	}

	private void tsmiRectangleLight_Click(object sender, EventArgs e)
	{
		new CaptureRegion(RegionCaptureType.Light).Capture(autoHideForm: true);
	}

	private void tsmiRectangleTransparent_Click(object sender, EventArgs e)
	{
		new CaptureRegion(RegionCaptureType.Transparent).Capture(autoHideForm: true);
	}

	private void tsmiLastRegion_Click(object sender, EventArgs e)
	{
		new CaptureLastRegion().Capture(autoHideForm: true);
	}

	private void tsmiScreenRecordingFFmpeg_Click(object sender, EventArgs e)
	{
		TaskHelpers.StartScreenRecording(ScreenRecordOutput.FFmpeg, ScreenRecordStartMethod.Region);
	}

	private void tsmiScreenRecordingGIF_Click(object sender, EventArgs e)
	{
		TaskHelpers.StartScreenRecording(ScreenRecordOutput.GIF, ScreenRecordStartMethod.Region);
	}

	private void tsmiScrollingCapture_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenScrollingCapture();
	}

	private void tsmiAutoCapture_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenAutoCapture();
	}

	private void tsmiShowCursor_Click(object sender, EventArgs e)
	{
		Program.DefaultTaskSettings.CaptureSettings.ShowCursor = ((ToolStripMenuItem)sender).Checked;
		ToolStripMenuItem toolStripMenuItem = tsmiShowCursor;
		bool @checked = (tsmiTrayShowCursor.Checked = Program.DefaultTaskSettings.CaptureSettings.ShowCursor);
		toolStripMenuItem.Checked = @checked;
	}

	private void tsmiScreenshotDelay0_Click(object sender, EventArgs e)
	{
		SetScreenshotDelay(0m);
	}

	private void tsmiScreenshotDelay1_Click(object sender, EventArgs e)
	{
		SetScreenshotDelay(1m);
	}

	private void tsmiScreenshotDelay2_Click(object sender, EventArgs e)
	{
		SetScreenshotDelay(2m);
	}

	private void tsmiScreenshotDelay3_Click(object sender, EventArgs e)
	{
		SetScreenshotDelay(3m);
	}

	private void tsmiScreenshotDelay4_Click(object sender, EventArgs e)
	{
		SetScreenshotDelay(4m);
	}

	private void tsmiScreenshotDelay5_Click(object sender, EventArgs e)
	{
		SetScreenshotDelay(5m);
	}

	private void tsbFileUpload_Click(object sender, EventArgs e)
	{
		UploadManager.UploadFile();
	}

	private void tsmiUploadFolder_Click(object sender, EventArgs e)
	{
		UploadManager.UploadFolder();
	}

	private void tsbClipboardUpload_Click(object sender, EventArgs e)
	{
		UploadManager.ClipboardUploadMainWindow();
	}

	private void tsmiUploadText_Click(object sender, EventArgs e)
	{
		UploadManager.ShowTextUploadDialog();
	}

	private void tsmiUploadURL_Click(object sender, EventArgs e)
	{
		UploadManager.UploadURL();
	}

	private void tsbDragDropUpload_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenDropWindow();
	}

	private void tsmiShortenURL_Click(object sender, EventArgs e)
	{
		UploadManager.ShowShortenURLDialog();
	}

	private void tsmiTweetMessage_Click(object sender, EventArgs e)
	{
		TaskHelpers.TweetMessage();
	}

	private void tsmiColorPicker_Click(object sender, EventArgs e)
	{
		TaskHelpers.ShowScreenColorPickerDialog();
	}

	private void tsmiScreenColorPicker_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenScreenColorPicker();
	}

	private void tsmiRuler_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenRuler();
	}

	private void tsmiImageEditor_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenImageEditor();
	}

	private void tsmiImageEffects_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenImageEffects();
	}

	private void tsmiImageViewer_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenImageViewer();
	}

	private void tsmiImageCombiner_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenImageCombiner();
	}

	private void TsmiImageSplitter_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenImageSplitter();
	}

	private void tsmiImageThumbnailer_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenImageThumbnailer();
	}

	private void tsmiVideoConverter_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenVideoConverter();
	}

	private void tsmiVideoThumbnailer_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenVideoThumbnailer();
	}

	private async void tsmiOCR_Click(object sender, EventArgs e)
	{
		Hide();
		await Task.Delay(250);
		try
		{
			await TaskHelpers.OCRImage();
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		finally
		{
			this.ForceActivate();
		}
	}

	private void tsmiQRCode_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenQRCode();
	}

	private void tsmiHashCheck_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenHashCheck();
	}

	private void tsmiIndexFolder_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenDirectoryIndexer();
	}

	private void tsmiClipboardViewer_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenClipboardViewer();
	}

	private void tsmiBorderlessWindow_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenBorderlessWindow();
	}

	private void tsmiInspectWindow_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenInspectWindow();
	}

	private void tsmiMonitorTest_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenMonitorTest();
	}

	private void tsmiDNSChanger_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenDNSChanger();
	}

	private void TsddbAfterCaptureTasks_DropDownOpening(object sender, EventArgs e)
	{
		UpdateImageEffectsMenu(tsddbAfterCaptureTasks);
	}

	private void TsmiTrayAfterCaptureTasks_DropDownOpening(object sender, EventArgs e)
	{
		UpdateImageEffectsMenu(tsmiTrayAfterCaptureTasks);
	}

	private void tsddbDestinations_DropDownOpened(object sender, EventArgs e)
	{
		UpdateDestinationStates();
	}

	private void tsmiDestinationSettings_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenUploadersConfigWindow();
	}

	private void tsmiCustomUploaderSettings_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenCustomUploaderSettingsWindow();
	}

	private void tsbApplicationSettings_Click(object sender, EventArgs e)
	{
		using (ApplicationSettingsForm applicationSettingsForm = new ApplicationSettingsForm())
		{
			applicationSettingsForm.ShowDialog();
		}
		if (!base.IsDisposed)
		{
			AfterApplicationSettingsJobs();
			UpdateWorkflowsMenu();
			SettingManager.SaveApplicationConfigAsync();
		}
	}

	private void tsbTaskSettings_Click(object sender, EventArgs e)
	{
		using (TaskSettingsForm taskSettingsForm = new TaskSettingsForm(Program.DefaultTaskSettings, isDefault: true))
		{
			taskSettingsForm.ShowDialog();
		}
		if (!base.IsDisposed)
		{
			AfterTaskSettingsJobs();
			SettingManager.SaveApplicationConfigAsync();
		}
	}

	private void tsbHotkeySettings_Click(object sender, EventArgs e)
	{
		if (Program.HotkeyManager != null)
		{
			using (HotkeySettingsForm hotkeySettingsForm = new HotkeySettingsForm(Program.HotkeyManager))
			{
				hotkeySettingsForm.ShowDialog();
			}
			if (!base.IsDisposed)
			{
				UpdateWorkflowsMenu();
				SettingManager.SaveHotkeysConfigAsync();
			}
		}
	}

	private void tsbScreenshotsFolder_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenScreenshotsFolder();
	}

	private void tsbHistory_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenHistory();
	}

	private void tsbImageHistory_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenImageHistory();
	}

	private void tsmiShowDebugLog_Click(object sender, EventArgs e)
	{
		TaskHelpers.OpenDebugLog();
	}

	private void tsmiTestImageUpload_Click(object sender, EventArgs e)
	{
		UploadManager.UploadImage(ShareXResources.Logo);
	}

	private void tsmiTestTextUpload_Click(object sender, EventArgs e)
	{
		UploadManager.UploadText(Resources.MainForm_tsmiTestTextUpload_Click_Text_upload_test);
	}

	private void tsmiTestFileUpload_Click(object sender, EventArgs e)
	{
		UploadManager.UploadImage(ShareXResources.Logo, ImageDestination.FileUploader, Program.DefaultTaskSettings.FileDestination);
	}

	private void tsmiTestURLShortener_Click(object sender, EventArgs e)
	{
		UploadManager.ShortenURL("https://getsharex.com");
	}

	private void tsmiTestURLSharing_Click(object sender, EventArgs e)
	{
		UploadManager.ShareURL("https://getsharex.com");
	}

	private void tsbDonate_Click(object sender, EventArgs e)
	{
		URLHelpers.OpenURL("https://getsharex.com/donate");
	}

	private void tsbTwitter_Click(object sender, EventArgs e)
	{
		URLHelpers.OpenURL("https://twitter.com/ShareX");
	}

	private void tsbDiscord_Click(object sender, EventArgs e)
	{
		URLHelpers.OpenURL("https://discord.gg/ShareX");
	}

	private void tsbAbout_Click(object sender, EventArgs e)
	{
		using AboutForm aboutForm = new AboutForm();
		aboutForm.ShowDialog();
	}

	private async void niTray_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			if (Program.Settings.TrayLeftDoubleClickAction == HotkeyType.None)
			{
				await TaskHelpers.ExecuteJob(Program.Settings.TrayLeftClickAction);
				return;
			}
			trayClickCount++;
			if (trayClickCount == 1)
			{
				timerTraySingleClick.Interval = SystemInformation.DoubleClickTime;
				timerTraySingleClick.Start();
			}
			else
			{
				trayClickCount = 0;
				timerTraySingleClick.Stop();
				await TaskHelpers.ExecuteJob(Program.Settings.TrayLeftDoubleClickAction);
			}
		}
		else if (e.Button == MouseButtons.Middle)
		{
			await TaskHelpers.ExecuteJob(Program.Settings.TrayMiddleClickAction);
		}
	}

	private async void timerTraySingleClick_Tick(object sender, EventArgs e)
	{
		if (trayClickCount == 1)
		{
			trayClickCount = 0;
			timerTraySingleClick.Stop();
			await TaskHelpers.ExecuteJob(Program.Settings.TrayLeftClickAction);
		}
	}

	private void niTray_BalloonTipClicked(object sender, EventArgs e)
	{
		if (niTray.Tag is BalloonTipAction balloonTipAction)
		{
			switch (balloonTipAction.ClickAction)
			{
			case BalloonTipClickAction.OpenURL:
				URLHelpers.OpenURL(balloonTipAction.Text);
				break;
			case BalloonTipClickAction.OpenDebugLog:
				TaskHelpers.OpenDebugLog();
				break;
			}
		}
	}

	private void cmsTray_Opened(object sender, EventArgs e)
	{
		if (Program.Settings.TrayAutoExpandCaptureMenu)
		{
			tsmiTrayCapture.Select();
			tsmiTrayCapture.ShowDropDown();
		}
	}

	private void tsmiTrayFullscreen_Click(object sender, EventArgs e)
	{
		new CaptureFullscreen().Capture();
	}

	private async void tsmiCapture_DropDownOpening(object sender, EventArgs e)
	{
		await PrepareCaptureMenuAsync(tsmiTrayWindow, tsmiTrayWindowItems_Click, tsmiTrayMonitor, tsmiTrayMonitorItems_Click);
	}

	private void tsmiTrayWindowItems_Click(object sender, EventArgs e)
	{
		if (((ToolStripItem)sender).Tag is WindowInfo windowInfo)
		{
			new CaptureWindow(windowInfo.Handle).Capture();
		}
	}

	private void tsmiTrayMonitorItems_Click(object sender, EventArgs e)
	{
		Rectangle monitorRectangle = (Rectangle)((ToolStripItem)sender).Tag;
		if (!monitorRectangle.IsEmpty)
		{
			new CaptureMonitor(monitorRectangle).Capture();
		}
	}

	private void tsmiTrayRectangle_Click(object sender, EventArgs e)
	{
		new CaptureRegion().Capture();
	}

	private void tsmiTrayRectangleLight_Click(object sender, EventArgs e)
	{
		new CaptureRegion(RegionCaptureType.Light).Capture();
	}

	private void tsmiTrayRectangleTransparent_Click(object sender, EventArgs e)
	{
		new CaptureRegion(RegionCaptureType.Transparent).Capture();
	}

	private void tsmiTrayLastRegion_Click(object sender, EventArgs e)
	{
		new CaptureLastRegion().Capture();
	}

	private async void tsmiTrayOCR_Click(object sender, EventArgs e)
	{
		try
		{
			await TaskHelpers.OCRImage();
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	private void tsmiTrayToggleHotkeys_Click(object sender, EventArgs e)
	{
		TaskHelpers.ToggleHotkeys();
	}

	private void tsmiRestartAsAdmin_Click(object sender, EventArgs e)
	{
		Program.Restart(asAdmin: true);
	}

	private void tsmiOpenActionsToolbar_Click(object sender, EventArgs e)
	{
		TaskHelpers.ToggleActionsToolbar();
	}

	private void tsmiTrayShow_Click(object sender, EventArgs e)
	{
		this.ForceActivate();
	}

	private void tsmiTrayExit_MouseDown(object sender, MouseEventArgs e)
	{
		trayMenuSaveSettings = false;
	}

	private void cmsTray_Closed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		if (trayMenuSaveSettings)
		{
			SettingManager.SaveAllSettingsAsync();
		}
		trayMenuSaveSettings = true;
	}

	private void tsmiTrayExit_Click(object sender, EventArgs e)
	{
		ForceClose();
	}

	private void tsmiShowErrors_Click(object sender, EventArgs e)
	{
		uim.ShowErrors();
	}

	private void tsmiStopUpload_Click(object sender, EventArgs e)
	{
		uim.StopUpload();
	}

	private void tsmiOpenURL_Click(object sender, EventArgs e)
	{
		uim.OpenURL();
	}

	private void tsmiOpenShortenedURL_Click(object sender, EventArgs e)
	{
		uim.OpenShortenedURL();
	}

	private void tsmiOpenThumbnailURL_Click(object sender, EventArgs e)
	{
		uim.OpenThumbnailURL();
	}

	private void tsmiOpenDeletionURL_Click(object sender, EventArgs e)
	{
		uim.OpenDeletionURL();
	}

	private void tsmiOpenFile_Click(object sender, EventArgs e)
	{
		uim.OpenFile();
	}

	private void tsmiOpenThumbnailFile_Click(object sender, EventArgs e)
	{
		uim.OpenThumbnailFile();
	}

	private void tsmiOpenFolder_Click(object sender, EventArgs e)
	{
		uim.OpenFolder();
	}

	private void tsmiCopyURL_Click(object sender, EventArgs e)
	{
		uim.CopyURL();
	}

	private void tsmiCopyShortenedURL_Click(object sender, EventArgs e)
	{
		uim.CopyShortenedURL();
	}

	private void tsmiCopyThumbnailURL_Click(object sender, EventArgs e)
	{
		uim.CopyThumbnailURL();
	}

	private void tsmiCopyDeletionURL_Click(object sender, EventArgs e)
	{
		uim.CopyDeletionURL();
	}

	private void tsmiCopyFile_Click(object sender, EventArgs e)
	{
		uim.CopyFile();
	}

	private void tsmiCopyImage_Click(object sender, EventArgs e)
	{
		uim.CopyImage();
	}

	private void tsmiCopyImageDimensions_Click(object sender, EventArgs e)
	{
		uim.CopyImageDimensions();
	}

	private void tsmiCopyText_Click(object sender, EventArgs e)
	{
		uim.CopyText();
	}

	private void tsmiCopyThumbnailFile_Click(object sender, EventArgs e)
	{
		uim.CopyThumbnailFile();
	}

	private void tsmiCopyThumbnailImage_Click(object sender, EventArgs e)
	{
		uim.CopyThumbnailImage();
	}

	private void tsmiCopyHTMLLink_Click(object sender, EventArgs e)
	{
		uim.CopyHTMLLink();
	}

	private void tsmiCopyHTMLImage_Click(object sender, EventArgs e)
	{
		uim.CopyHTMLImage();
	}

	private void tsmiCopyHTMLLinkedImage_Click(object sender, EventArgs e)
	{
		uim.CopyHTMLLinkedImage();
	}

	private void tsmiCopyForumLink_Click(object sender, EventArgs e)
	{
		uim.CopyForumLink();
	}

	private void tsmiCopyForumImage_Click(object sender, EventArgs e)
	{
		uim.CopyForumImage();
	}

	private void tsmiCopyForumLinkedImage_Click(object sender, EventArgs e)
	{
		uim.CopyForumLinkedImage();
	}

	private void tsmiCopyMarkdownLink_Click(object sender, EventArgs e)
	{
		uim.CopyMarkdownLink();
	}

	private void tsmiCopyMarkdownImage_Click(object sender, EventArgs e)
	{
		uim.CopyMarkdownImage();
	}

	private void tsmiCopyMarkdownLinkedImage_Click(object sender, EventArgs e)
	{
		uim.CopyMarkdownLinkedImage();
	}

	private void tsmiCopyFilePath_Click(object sender, EventArgs e)
	{
		uim.CopyFilePath();
	}

	private void tsmiCopyFileName_Click(object sender, EventArgs e)
	{
		uim.CopyFileName();
	}

	private void tsmiCopyFileNameWithExtension_Click(object sender, EventArgs e)
	{
		uim.CopyFileNameWithExtension();
	}

	private void tsmiCopyFolder_Click(object sender, EventArgs e)
	{
		uim.CopyFolder();
	}

	private void tsmiClipboardFormat_Click(object sender, EventArgs e)
	{
		ClipboardFormat clipboardFormat = (sender as ToolStripMenuItem).Tag as ClipboardFormat;
		uim.CopyCustomFormat(clipboardFormat.Format);
	}

	private void tsmiUploadSelectedFile_Click(object sender, EventArgs e)
	{
		uim.Upload();
	}

	private void tsmiDownloadSelectedURL_Click(object sender, EventArgs e)
	{
		uim.Download();
	}

	private void tsmiDeleteSelectedItem_Click(object sender, EventArgs e)
	{
		RemoveSelectedItems();
	}

	private void tsmiDeleteSelectedFile_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show(Resources.MainForm_tsmiDeleteSelectedFile_Click_Do_you_really_want_to_delete_this_file_, "ShareX - " + Resources.MainForm_tsmiDeleteSelectedFile_Click_File_delete_confirmation, MessageBoxButtons.YesNo) == DialogResult.Yes)
		{
			uim.DeleteFiles();
			RemoveSelectedItems();
		}
	}

	private void tsmiEditSelectedFile_Click(object sender, EventArgs e)
	{
		uim.EditImage();
	}

	private void tsmiAddImageEffects_Click(object sender, EventArgs e)
	{
		uim.AddImageEffects();
	}

	private void tsmiGoogleImageSearch_Click(object sender, EventArgs e)
	{
		uim.SearchImageUsingGoogle();
	}

	private void tsmiBingVisualSearch_Click(object sender, EventArgs e)
	{
		uim.SearchImageUsingBing();
	}

	private void tsmiShowQRCode_Click(object sender, EventArgs e)
	{
		uim.ShowQRCode();
	}

	private async void tsmiOCRImage_Click(object sender, EventArgs e)
	{
		await uim.OCRImage();
	}

	private void tsmiCombineImages_Click(object sender, EventArgs e)
	{
		uim.CombineImages();
	}

	private void tsmiCombineImagesHorizontally_Click(object sender, EventArgs e)
	{
		uim.CombineImages(Orientation.Horizontal);
	}

	private void tsmiCombineImagesVertically_Click(object sender, EventArgs e)
	{
		uim.CombineImages(Orientation.Vertical);
	}

	private void tsmiShowResponse_Click(object sender, EventArgs e)
	{
		uim.ShowResponse();
	}

	private void tsmiClearList_Click(object sender, EventArgs e)
	{
		RemoveAllItems();
		TaskManager.RecentManager.Clear();
	}

	private void TsmiSwitchTaskViewMode_Click(object sender, EventArgs e)
	{
		tsMain.SendToBack();
		if (Program.Settings.TaskViewMode == TaskViewMode.ListView)
		{
			Program.Settings.TaskViewMode = TaskViewMode.ThumbnailView;
			ucTaskThumbnailView.UpdateAllThumbnails();
		}
		else
		{
			Program.Settings.TaskViewMode = TaskViewMode.ListView;
		}
		UpdateTaskViewMode();
		UpdateMainWindowLayout();
		UpdateInfoManager();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.MainForm));
		this.scMain = new ShareX.HelpersLib.SplitContainerCustomSplitter();
		this.lblListViewTip = new System.Windows.Forms.Label();
		this.lvUploads = new ShareX.HelpersLib.MyListView();
		this.chFilename = new System.Windows.Forms.ColumnHeader();
		this.chStatus = new System.Windows.Forms.ColumnHeader();
		this.chProgress = new System.Windows.Forms.ColumnHeader();
		this.chSpeed = new System.Windows.Forms.ColumnHeader();
		this.chElapsed = new System.Windows.Forms.ColumnHeader();
		this.chRemaining = new System.Windows.Forms.ColumnHeader();
		this.chURL = new System.Windows.Forms.ColumnHeader();
		this.pbPreview = new ShareX.HelpersLib.MyPictureBox();
		this.tsMain = new ShareX.HelpersLib.ToolStripBorderRight();
		this.tsddbCapture = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsmiFullscreen = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiWindow = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiMonitor = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiRectangle = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiRectangleLight = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiRectangleTransparent = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiLastRegion = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenRecordingFFmpeg = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenRecordingGIF = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScrollingCapture = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiAutoCapture = new System.Windows.Forms.ToolStripMenuItem();
		this.tssCapture1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiShowCursor = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenshotDelay = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenshotDelay0 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenshotDelay1 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenshotDelay2 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenshotDelay3 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenshotDelay4 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenshotDelay5 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsddbUpload = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsmiUploadFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiUploadFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiUploadClipboard = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiUploadText = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiUploadURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiUploadDragDrop = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiShortenURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTweetMessage = new System.Windows.Forms.ToolStripMenuItem();
		this.tsddbWorkflows = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsddbTools = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsmiColorPicker = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiScreenColorPicker = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiRuler = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTools1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiImageEditor = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiImageEffects = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiImageViewer = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiImageCombiner = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiImageSplitter = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiImageThumbnailer = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTools2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiVideoConverter = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiVideoThumbnailer = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTools3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiOCR = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiQRCode = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiHashCheck = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiIndexFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTools4 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiClipboardViewer = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiBorderlessWindow = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiInspectWindow = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiMonitorTest = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiDNSChanger = new System.Windows.Forms.ToolStripMenuItem();
		this.tssMain1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsddbAfterCaptureTasks = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsddbAfterUploadTasks = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsddbDestinations = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsmiImageUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTextUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiFileUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiURLShorteners = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiURLSharingServices = new System.Windows.Forms.ToolStripMenuItem();
		this.tssDestinations1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiDestinationSettings = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCustomUploaderSettings = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbApplicationSettings = new System.Windows.Forms.ToolStripButton();
		this.tsbTaskSettings = new System.Windows.Forms.ToolStripButton();
		this.tsbHotkeySettings = new System.Windows.Forms.ToolStripButton();
		this.tssMain2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbScreenshotsFolder = new System.Windows.Forms.ToolStripButton();
		this.tsbHistory = new System.Windows.Forms.ToolStripButton();
		this.tsbImageHistory = new System.Windows.Forms.ToolStripButton();
		this.tssMain3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsddbDebug = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsmiShowDebugLog = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTestImageUpload = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTestTextUpload = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTestFileUpload = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTestURLShortener = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTestURLSharing = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbDonate = new System.Windows.Forms.ToolStripButton();
		this.tsbTwitter = new System.Windows.Forms.ToolStripButton();
		this.tsbDiscord = new System.Windows.Forms.ToolStripButton();
		this.tsbAbout = new System.Windows.Forms.ToolStripButton();
		this.cmsTaskInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmiShowErrors = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiStopUpload = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOpenURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOpenShortenedURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOpenThumbnailURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOpenDeletionURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tssOpen1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiOpenFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOpenThumbnailFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyShortenedURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyThumbnailURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyDeletionURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tssCopy1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiCopyFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyImageDimensions = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyText = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyThumbnailFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyThumbnailImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tssCopy2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiCopyHTMLLink = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyHTMLImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyHTMLLinkedImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tssCopy3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiCopyForumLink = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyForumImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyForumLinkedImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tssCopy4 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiCopyMarkdownLink = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyMarkdownImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyMarkdownLinkedImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tssCopy5 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiCopyFilePath = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyFileName = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyFileNameWithExtension = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCopyFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.tssCopy6 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiUploadSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiDownloadSelectedURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiEditSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiAddImageEffects = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiRunAction = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiDeleteSelectedItem = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiDeleteSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiShortenSelectedURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiShareSelectedURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiGoogleImageSearch = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiBingVisualSearch = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiShowQRCode = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOCRImage = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCombineImages = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCombineImagesHorizontally = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiCombineImagesVertically = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiShowResponse = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiClearList = new System.Windows.Forms.ToolStripMenuItem();
		this.tssUploadInfo1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiSwitchTaskViewMode = new System.Windows.Forms.ToolStripMenuItem();
		this.niTray = new System.Windows.Forms.NotifyIcon(this.components);
		this.cmsTray = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmiTrayCapture = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayFullscreen = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayWindow = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayMonitor = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayRectangle = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayRectangleLight = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayRectangleTransparent = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayLastRegion = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenRecordingFFmpeg = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenRecordingGIF = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScrollingCapture = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayAutoCapture = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTrayCapture1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiTrayShowCursor = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenshotDelay = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenshotDelay0 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenshotDelay1 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenshotDelay2 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenshotDelay3 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenshotDelay4 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenshotDelay5 = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayUpload = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayUploadFile = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayUploadFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayUploadClipboard = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayUploadText = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayUploadURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayUploadDragDrop = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayShortenURL = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayTweetMessage = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayWorkflows = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayTools = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayColorPicker = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayScreenColorPicker = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayRuler = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTrayTools1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiTrayImageEditor = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayImageEffects = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayImageViewer = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayImageCombiner = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayImageSplitter = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayImageThumbnailer = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTrayTools2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiTrayVideoConverter = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayVideoThumbnailer = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTrayTools3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiTrayOCR = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayQRCode = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayHashCheck = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayIndexFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTrayTools4 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiTrayClipboardViewer = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayBorderlessWindow = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayInspectWindow = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayMonitorTest = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayDNSChanger = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTray1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiTrayAfterCaptureTasks = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayAfterUploadTasks = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayDestinations = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayImageUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayTextUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayFileUploaders = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayURLShorteners = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayURLSharingServices = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTrayDestinations1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiTrayDestinationSettings = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayCustomUploaderSettings = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayApplicationSettings = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayTaskSettings = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayHotkeySettings = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayToggleHotkeys = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTray2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiScreenshotsFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayImageHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.tssTray3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmiRestartAsAdmin = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayRecentItems = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiOpenActionsToolbar = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayShow = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmiTrayExit = new System.Windows.Forms.ToolStripMenuItem();
		this.timerTraySingleClick = new System.Windows.Forms.Timer(this.components);
		this.pThumbnailView = new System.Windows.Forms.Panel();
		this.lblThumbnailViewTip = new System.Windows.Forms.Label();
		this.ucTaskThumbnailView = new ShareX.TaskThumbnailView();
		this.ttMain = new System.Windows.Forms.ToolTip(this.components);
		this.pToolbars = new System.Windows.Forms.Panel();
		((System.ComponentModel.ISupportInitialize)this.scMain).BeginInit();
		this.scMain.Panel1.SuspendLayout();
		this.scMain.Panel2.SuspendLayout();
		this.scMain.SuspendLayout();
		this.tsMain.SuspendLayout();
		this.cmsTaskInfo.SuspendLayout();
		this.cmsTray.SuspendLayout();
		this.pThumbnailView.SuspendLayout();
		this.pToolbars.SuspendLayout();
		base.SuspendLayout();
		resources.ApplyResources(this.scMain, "scMain");
		this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.scMain.Name = "scMain";
		this.scMain.Panel1.Controls.Add(this.lblListViewTip);
		this.scMain.Panel1.Controls.Add(this.lvUploads);
		this.scMain.Panel2.Controls.Add(this.pbPreview);
		this.scMain.SplitterColor = System.Drawing.Color.White;
		this.scMain.SplitterLineColor = System.Drawing.Color.FromArgb(189, 189, 189);
		this.scMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(scMain_SplitterMoved);
		resources.ApplyResources(this.lblListViewTip, "lblListViewTip");
		this.lblListViewTip.BackColor = System.Drawing.Color.Transparent;
		this.lblListViewTip.ForeColor = System.Drawing.Color.Silver;
		this.lblListViewTip.Name = "lblListViewTip";
		this.lblListViewTip.UseMnemonic = false;
		this.lblListViewTip.MouseUp += new System.Windows.Forms.MouseEventHandler(lblListViewTip_MouseUp);
		this.lvUploads.AutoFillColumn = true;
		this.lvUploads.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lvUploads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[7] { this.chFilename, this.chStatus, this.chProgress, this.chSpeed, this.chElapsed, this.chRemaining, this.chURL });
		resources.ApplyResources(this.lvUploads, "lvUploads");
		this.lvUploads.FullRowSelect = true;
		this.lvUploads.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lvUploads.HideSelection = false;
		this.lvUploads.Name = "lvUploads";
		this.lvUploads.ShowItemToolTips = true;
		this.lvUploads.UseCompatibleStateImageBehavior = false;
		this.lvUploads.View = System.Windows.Forms.View.Details;
		this.lvUploads.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lvUploads_ColumnWidthChanged);
		this.lvUploads.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(lvUploads_ItemDrag);
		this.lvUploads.SelectedIndexChanged += new System.EventHandler(lvUploads_SelectedIndexChanged);
		this.lvUploads.KeyDown += new System.Windows.Forms.KeyEventHandler(lvUploads_KeyDown);
		this.lvUploads.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(lvUploads_MouseDoubleClick);
		this.lvUploads.MouseUp += new System.Windows.Forms.MouseEventHandler(lvUploads_MouseUp);
		resources.ApplyResources(this.chFilename, "chFilename");
		resources.ApplyResources(this.chStatus, "chStatus");
		resources.ApplyResources(this.chProgress, "chProgress");
		resources.ApplyResources(this.chSpeed, "chSpeed");
		resources.ApplyResources(this.chElapsed, "chElapsed");
		resources.ApplyResources(this.chRemaining, "chRemaining");
		resources.ApplyResources(this.chURL, "chURL");
		this.pbPreview.BackColor = System.Drawing.SystemColors.Window;
		this.pbPreview.Cursor = System.Windows.Forms.Cursors.Hand;
		resources.ApplyResources(this.pbPreview, "pbPreview");
		this.pbPreview.DrawCheckeredBackground = true;
		this.pbPreview.EnableRightClickMenu = true;
		this.pbPreview.Name = "pbPreview";
		this.pbPreview.PictureBoxBackColor = System.Drawing.SystemColors.Control;
		this.pbPreview.ShowImageSizeLabel = true;
		this.pbPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(pbPreview_MouseDown);
		this.tsMain.CanOverflow = false;
		resources.ApplyResources(this.tsMain, "tsMain");
		this.tsMain.DrawCustomBorder = true;
		this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[21]
		{
			this.tsddbCapture, this.tsddbUpload, this.tsddbWorkflows, this.tsddbTools, this.tssMain1, this.tsddbAfterCaptureTasks, this.tsddbAfterUploadTasks, this.tsddbDestinations, this.tsbApplicationSettings, this.tsbTaskSettings,
			this.tsbHotkeySettings, this.tssMain2, this.tsbScreenshotsFolder, this.tsbHistory, this.tsbImageHistory, this.tssMain3, this.tsddbDebug, this.tsbDonate, this.tsbTwitter, this.tsbDiscord,
			this.tsbAbout
		});
		this.tsMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
		this.tsMain.Name = "tsMain";
		this.tsMain.ShowItemToolTips = false;
		this.tsMain.TabStop = true;
		this.tsddbCapture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.tsmiFullscreen, this.tsmiWindow, this.tsmiMonitor, this.tsmiRectangle, this.tsmiRectangleLight, this.tsmiRectangleTransparent, this.tsmiLastRegion, this.tsmiScreenRecordingFFmpeg, this.tsmiScreenRecordingGIF, this.tsmiScrollingCapture,
			this.tsmiAutoCapture, this.tssCapture1, this.tsmiShowCursor, this.tsmiScreenshotDelay
		});
		this.tsddbCapture.Image = ShareX.Properties.Resources.camera;
		resources.ApplyResources(this.tsddbCapture, "tsddbCapture");
		this.tsddbCapture.Name = "tsddbCapture";
		this.tsddbCapture.DropDownOpening += new System.EventHandler(tsddbCapture_DropDownOpening);
		this.tsmiFullscreen.Image = ShareX.Properties.Resources.layer_fullscreen;
		this.tsmiFullscreen.Name = "tsmiFullscreen";
		resources.ApplyResources(this.tsmiFullscreen, "tsmiFullscreen");
		this.tsmiFullscreen.Click += new System.EventHandler(tsmiFullscreen_Click);
		this.tsmiWindow.Image = ShareX.Properties.Resources.application_blue;
		this.tsmiWindow.Name = "tsmiWindow";
		resources.ApplyResources(this.tsmiWindow, "tsmiWindow");
		this.tsmiMonitor.Image = ShareX.Properties.Resources.monitor;
		this.tsmiMonitor.Name = "tsmiMonitor";
		resources.ApplyResources(this.tsmiMonitor, "tsmiMonitor");
		this.tsmiRectangle.Image = ShareX.Properties.Resources.layer_shape;
		this.tsmiRectangle.Name = "tsmiRectangle";
		resources.ApplyResources(this.tsmiRectangle, "tsmiRectangle");
		this.tsmiRectangle.Click += new System.EventHandler(tsmiRectangle_Click);
		this.tsmiRectangleLight.Image = ShareX.Properties.Resources.Rectangle;
		this.tsmiRectangleLight.Name = "tsmiRectangleLight";
		resources.ApplyResources(this.tsmiRectangleLight, "tsmiRectangleLight");
		this.tsmiRectangleLight.Click += new System.EventHandler(tsmiRectangleLight_Click);
		this.tsmiRectangleTransparent.Image = ShareX.Properties.Resources.layer_transparent;
		this.tsmiRectangleTransparent.Name = "tsmiRectangleTransparent";
		resources.ApplyResources(this.tsmiRectangleTransparent, "tsmiRectangleTransparent");
		this.tsmiRectangleTransparent.Click += new System.EventHandler(tsmiRectangleTransparent_Click);
		this.tsmiLastRegion.Image = ShareX.Properties.Resources.layers;
		this.tsmiLastRegion.Name = "tsmiLastRegion";
		resources.ApplyResources(this.tsmiLastRegion, "tsmiLastRegion");
		this.tsmiLastRegion.Click += new System.EventHandler(tsmiLastRegion_Click);
		this.tsmiScreenRecordingFFmpeg.Image = ShareX.Properties.Resources.camcorder_image;
		this.tsmiScreenRecordingFFmpeg.Name = "tsmiScreenRecordingFFmpeg";
		resources.ApplyResources(this.tsmiScreenRecordingFFmpeg, "tsmiScreenRecordingFFmpeg");
		this.tsmiScreenRecordingFFmpeg.Click += new System.EventHandler(tsmiScreenRecordingFFmpeg_Click);
		this.tsmiScreenRecordingGIF.Image = ShareX.Properties.Resources.film;
		this.tsmiScreenRecordingGIF.Name = "tsmiScreenRecordingGIF";
		resources.ApplyResources(this.tsmiScreenRecordingGIF, "tsmiScreenRecordingGIF");
		this.tsmiScreenRecordingGIF.Click += new System.EventHandler(tsmiScreenRecordingGIF_Click);
		this.tsmiScrollingCapture.Image = ShareX.Properties.Resources.ui_scroll_pane_image;
		this.tsmiScrollingCapture.Name = "tsmiScrollingCapture";
		resources.ApplyResources(this.tsmiScrollingCapture, "tsmiScrollingCapture");
		this.tsmiScrollingCapture.Click += new System.EventHandler(tsmiScrollingCapture_Click);
		this.tsmiAutoCapture.Image = ShareX.Properties.Resources.clock;
		this.tsmiAutoCapture.Name = "tsmiAutoCapture";
		resources.ApplyResources(this.tsmiAutoCapture, "tsmiAutoCapture");
		this.tsmiAutoCapture.Click += new System.EventHandler(tsmiAutoCapture_Click);
		this.tssCapture1.Name = "tssCapture1";
		resources.ApplyResources(this.tssCapture1, "tssCapture1");
		this.tsmiShowCursor.CheckOnClick = true;
		this.tsmiShowCursor.Image = ShareX.Properties.Resources.cursor;
		this.tsmiShowCursor.Name = "tsmiShowCursor";
		resources.ApplyResources(this.tsmiShowCursor, "tsmiShowCursor");
		this.tsmiShowCursor.Click += new System.EventHandler(tsmiShowCursor_Click);
		this.tsmiScreenshotDelay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsmiScreenshotDelay0, this.tsmiScreenshotDelay1, this.tsmiScreenshotDelay2, this.tsmiScreenshotDelay3, this.tsmiScreenshotDelay4, this.tsmiScreenshotDelay5 });
		this.tsmiScreenshotDelay.Image = ShareX.Properties.Resources.clock_select;
		this.tsmiScreenshotDelay.Name = "tsmiScreenshotDelay";
		resources.ApplyResources(this.tsmiScreenshotDelay, "tsmiScreenshotDelay");
		this.tsmiScreenshotDelay0.Name = "tsmiScreenshotDelay0";
		resources.ApplyResources(this.tsmiScreenshotDelay0, "tsmiScreenshotDelay0");
		this.tsmiScreenshotDelay0.Click += new System.EventHandler(tsmiScreenshotDelay0_Click);
		this.tsmiScreenshotDelay1.Name = "tsmiScreenshotDelay1";
		resources.ApplyResources(this.tsmiScreenshotDelay1, "tsmiScreenshotDelay1");
		this.tsmiScreenshotDelay1.Click += new System.EventHandler(tsmiScreenshotDelay1_Click);
		this.tsmiScreenshotDelay2.Name = "tsmiScreenshotDelay2";
		resources.ApplyResources(this.tsmiScreenshotDelay2, "tsmiScreenshotDelay2");
		this.tsmiScreenshotDelay2.Click += new System.EventHandler(tsmiScreenshotDelay2_Click);
		this.tsmiScreenshotDelay3.Name = "tsmiScreenshotDelay3";
		resources.ApplyResources(this.tsmiScreenshotDelay3, "tsmiScreenshotDelay3");
		this.tsmiScreenshotDelay3.Click += new System.EventHandler(tsmiScreenshotDelay3_Click);
		this.tsmiScreenshotDelay4.Name = "tsmiScreenshotDelay4";
		resources.ApplyResources(this.tsmiScreenshotDelay4, "tsmiScreenshotDelay4");
		this.tsmiScreenshotDelay4.Click += new System.EventHandler(tsmiScreenshotDelay4_Click);
		this.tsmiScreenshotDelay5.Name = "tsmiScreenshotDelay5";
		resources.ApplyResources(this.tsmiScreenshotDelay5, "tsmiScreenshotDelay5");
		this.tsmiScreenshotDelay5.Click += new System.EventHandler(tsmiScreenshotDelay5_Click);
		this.tsddbUpload.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.tsmiUploadFile, this.tsmiUploadFolder, this.tsmiUploadClipboard, this.tsmiUploadText, this.tsmiUploadURL, this.tsmiUploadDragDrop, this.tsmiShortenURL, this.tsmiTweetMessage });
		this.tsddbUpload.Image = ShareX.Properties.Resources.arrow_090;
		resources.ApplyResources(this.tsddbUpload, "tsddbUpload");
		this.tsddbUpload.Name = "tsddbUpload";
		this.tsmiUploadFile.Image = ShareX.Properties.Resources.folder_open_document;
		this.tsmiUploadFile.Name = "tsmiUploadFile";
		resources.ApplyResources(this.tsmiUploadFile, "tsmiUploadFile");
		this.tsmiUploadFile.Click += new System.EventHandler(tsbFileUpload_Click);
		this.tsmiUploadFolder.Image = ShareX.Properties.Resources.folder;
		this.tsmiUploadFolder.Name = "tsmiUploadFolder";
		resources.ApplyResources(this.tsmiUploadFolder, "tsmiUploadFolder");
		this.tsmiUploadFolder.Click += new System.EventHandler(tsmiUploadFolder_Click);
		this.tsmiUploadClipboard.Image = ShareX.Properties.Resources.clipboard;
		this.tsmiUploadClipboard.Name = "tsmiUploadClipboard";
		resources.ApplyResources(this.tsmiUploadClipboard, "tsmiUploadClipboard");
		this.tsmiUploadClipboard.Click += new System.EventHandler(tsbClipboardUpload_Click);
		this.tsmiUploadText.Image = ShareX.Properties.Resources.notebook;
		this.tsmiUploadText.Name = "tsmiUploadText";
		resources.ApplyResources(this.tsmiUploadText, "tsmiUploadText");
		this.tsmiUploadText.Click += new System.EventHandler(tsmiUploadText_Click);
		this.tsmiUploadURL.Image = ShareX.Properties.Resources.drive;
		this.tsmiUploadURL.Name = "tsmiUploadURL";
		resources.ApplyResources(this.tsmiUploadURL, "tsmiUploadURL");
		this.tsmiUploadURL.Click += new System.EventHandler(tsmiUploadURL_Click);
		this.tsmiUploadDragDrop.Image = ShareX.Properties.Resources.inbox;
		this.tsmiUploadDragDrop.Name = "tsmiUploadDragDrop";
		resources.ApplyResources(this.tsmiUploadDragDrop, "tsmiUploadDragDrop");
		this.tsmiUploadDragDrop.Click += new System.EventHandler(tsbDragDropUpload_Click);
		this.tsmiShortenURL.Image = ShareX.Properties.Resources.edit_scale;
		this.tsmiShortenURL.Name = "tsmiShortenURL";
		resources.ApplyResources(this.tsmiShortenURL, "tsmiShortenURL");
		this.tsmiShortenURL.Click += new System.EventHandler(tsmiShortenURL_Click);
		this.tsmiTweetMessage.Image = ShareX.Properties.Resources.Twitter;
		this.tsmiTweetMessage.Name = "tsmiTweetMessage";
		resources.ApplyResources(this.tsmiTweetMessage, "tsmiTweetMessage");
		this.tsmiTweetMessage.Click += new System.EventHandler(tsmiTweetMessage_Click);
		this.tsddbWorkflows.Image = ShareX.Properties.Resources.categories;
		resources.ApplyResources(this.tsddbWorkflows, "tsddbWorkflows");
		this.tsddbWorkflows.Name = "tsddbWorkflows";
		this.tsddbTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[24]
		{
			this.tsmiColorPicker, this.tsmiScreenColorPicker, this.tsmiRuler, this.tssTools1, this.tsmiImageEditor, this.tsmiImageEffects, this.tsmiImageViewer, this.tsmiImageCombiner, this.tsmiImageSplitter, this.tsmiImageThumbnailer,
			this.tssTools2, this.tsmiVideoConverter, this.tsmiVideoThumbnailer, this.tssTools3, this.tsmiOCR, this.tsmiQRCode, this.tsmiHashCheck, this.tsmiIndexFolder, this.tssTools4, this.tsmiClipboardViewer,
			this.tsmiBorderlessWindow, this.tsmiInspectWindow, this.tsmiMonitorTest, this.tsmiDNSChanger
		});
		this.tsddbTools.Image = ShareX.Properties.Resources.toolbox;
		resources.ApplyResources(this.tsddbTools, "tsddbTools");
		this.tsddbTools.Name = "tsddbTools";
		this.tsmiColorPicker.Image = ShareX.Properties.Resources.color;
		this.tsmiColorPicker.Name = "tsmiColorPicker";
		resources.ApplyResources(this.tsmiColorPicker, "tsmiColorPicker");
		this.tsmiColorPicker.Click += new System.EventHandler(tsmiColorPicker_Click);
		this.tsmiScreenColorPicker.Image = ShareX.Properties.Resources.pipette;
		this.tsmiScreenColorPicker.Name = "tsmiScreenColorPicker";
		resources.ApplyResources(this.tsmiScreenColorPicker, "tsmiScreenColorPicker");
		this.tsmiScreenColorPicker.Click += new System.EventHandler(tsmiScreenColorPicker_Click);
		this.tsmiRuler.Image = ShareX.Properties.Resources.ruler_triangle;
		this.tsmiRuler.Name = "tsmiRuler";
		resources.ApplyResources(this.tsmiRuler, "tsmiRuler");
		this.tsmiRuler.Click += new System.EventHandler(tsmiRuler_Click);
		this.tssTools1.Name = "tssTools1";
		resources.ApplyResources(this.tssTools1, "tssTools1");
		this.tsmiImageEditor.Image = ShareX.Properties.Resources.image_pencil;
		this.tsmiImageEditor.Name = "tsmiImageEditor";
		resources.ApplyResources(this.tsmiImageEditor, "tsmiImageEditor");
		this.tsmiImageEditor.Click += new System.EventHandler(tsmiImageEditor_Click);
		this.tsmiImageEffects.Image = ShareX.Properties.Resources.image_saturation;
		this.tsmiImageEffects.Name = "tsmiImageEffects";
		resources.ApplyResources(this.tsmiImageEffects, "tsmiImageEffects");
		this.tsmiImageEffects.Click += new System.EventHandler(tsmiImageEffects_Click);
		this.tsmiImageViewer.Image = ShareX.Properties.Resources.images_flickr;
		this.tsmiImageViewer.Name = "tsmiImageViewer";
		resources.ApplyResources(this.tsmiImageViewer, "tsmiImageViewer");
		this.tsmiImageViewer.Click += new System.EventHandler(tsmiImageViewer_Click);
		this.tsmiImageCombiner.Image = ShareX.Properties.Resources.document_break;
		this.tsmiImageCombiner.Name = "tsmiImageCombiner";
		resources.ApplyResources(this.tsmiImageCombiner, "tsmiImageCombiner");
		this.tsmiImageCombiner.Click += new System.EventHandler(tsmiImageCombiner_Click);
		this.tsmiImageSplitter.Image = ShareX.Properties.Resources.image_split;
		this.tsmiImageSplitter.Name = "tsmiImageSplitter";
		resources.ApplyResources(this.tsmiImageSplitter, "tsmiImageSplitter");
		this.tsmiImageSplitter.Click += new System.EventHandler(TsmiImageSplitter_Click);
		this.tsmiImageThumbnailer.Image = ShareX.Properties.Resources.image_resize_actual;
		this.tsmiImageThumbnailer.Name = "tsmiImageThumbnailer";
		resources.ApplyResources(this.tsmiImageThumbnailer, "tsmiImageThumbnailer");
		this.tsmiImageThumbnailer.Click += new System.EventHandler(tsmiImageThumbnailer_Click);
		this.tssTools2.Name = "tssTools2";
		resources.ApplyResources(this.tssTools2, "tssTools2");
		this.tsmiVideoConverter.Image = ShareX.Properties.Resources.camcorder_pencil;
		this.tsmiVideoConverter.Name = "tsmiVideoConverter";
		resources.ApplyResources(this.tsmiVideoConverter, "tsmiVideoConverter");
		this.tsmiVideoConverter.Click += new System.EventHandler(tsmiVideoConverter_Click);
		this.tsmiVideoThumbnailer.Image = ShareX.Properties.Resources.images_stack;
		this.tsmiVideoThumbnailer.Name = "tsmiVideoThumbnailer";
		resources.ApplyResources(this.tsmiVideoThumbnailer, "tsmiVideoThumbnailer");
		this.tsmiVideoThumbnailer.Click += new System.EventHandler(tsmiVideoThumbnailer_Click);
		this.tssTools3.Name = "tssTools3";
		resources.ApplyResources(this.tssTools3, "tssTools3");
		this.tsmiOCR.Image = ShareX.Properties.Resources.edit_drop_cap;
		this.tsmiOCR.Name = "tsmiOCR";
		resources.ApplyResources(this.tsmiOCR, "tsmiOCR");
		this.tsmiOCR.Click += new System.EventHandler(tsmiOCR_Click);
		this.tsmiQRCode.Image = ShareX.Properties.Resources.barcode_2d;
		this.tsmiQRCode.Name = "tsmiQRCode";
		resources.ApplyResources(this.tsmiQRCode, "tsmiQRCode");
		this.tsmiQRCode.Click += new System.EventHandler(tsmiQRCode_Click);
		this.tsmiHashCheck.Image = ShareX.Properties.Resources.application_task;
		this.tsmiHashCheck.Name = "tsmiHashCheck";
		resources.ApplyResources(this.tsmiHashCheck, "tsmiHashCheck");
		this.tsmiHashCheck.Click += new System.EventHandler(tsmiHashCheck_Click);
		this.tsmiIndexFolder.Image = ShareX.Properties.Resources.folder_tree;
		this.tsmiIndexFolder.Name = "tsmiIndexFolder";
		resources.ApplyResources(this.tsmiIndexFolder, "tsmiIndexFolder");
		this.tsmiIndexFolder.Click += new System.EventHandler(tsmiIndexFolder_Click);
		this.tssTools4.Name = "tssTools4";
		resources.ApplyResources(this.tssTools4, "tssTools4");
		this.tsmiClipboardViewer.Image = ShareX.Properties.Resources.clipboard_block;
		this.tsmiClipboardViewer.Name = "tsmiClipboardViewer";
		resources.ApplyResources(this.tsmiClipboardViewer, "tsmiClipboardViewer");
		this.tsmiClipboardViewer.Click += new System.EventHandler(tsmiClipboardViewer_Click);
		this.tsmiBorderlessWindow.Image = ShareX.Properties.Resources.application_resize_full;
		this.tsmiBorderlessWindow.Name = "tsmiBorderlessWindow";
		resources.ApplyResources(this.tsmiBorderlessWindow, "tsmiBorderlessWindow");
		this.tsmiBorderlessWindow.Click += new System.EventHandler(tsmiBorderlessWindow_Click);
		this.tsmiInspectWindow.Image = ShareX.Properties.Resources.application_search_result;
		this.tsmiInspectWindow.Name = "tsmiInspectWindow";
		resources.ApplyResources(this.tsmiInspectWindow, "tsmiInspectWindow");
		this.tsmiInspectWindow.Click += new System.EventHandler(tsmiInspectWindow_Click);
		this.tsmiMonitorTest.Image = ShareX.Properties.Resources.monitor;
		this.tsmiMonitorTest.Name = "tsmiMonitorTest";
		resources.ApplyResources(this.tsmiMonitorTest, "tsmiMonitorTest");
		this.tsmiMonitorTest.Click += new System.EventHandler(tsmiMonitorTest_Click);
		this.tsmiDNSChanger.Image = ShareX.Properties.Resources.network_ip;
		this.tsmiDNSChanger.Name = "tsmiDNSChanger";
		resources.ApplyResources(this.tsmiDNSChanger, "tsmiDNSChanger");
		this.tsmiDNSChanger.Click += new System.EventHandler(tsmiDNSChanger_Click);
		this.tssMain1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 6);
		this.tssMain1.Name = "tssMain1";
		resources.ApplyResources(this.tssMain1, "tssMain1");
		this.tsddbAfterCaptureTasks.Image = ShareX.Properties.Resources.image_export;
		resources.ApplyResources(this.tsddbAfterCaptureTasks, "tsddbAfterCaptureTasks");
		this.tsddbAfterCaptureTasks.Name = "tsddbAfterCaptureTasks";
		this.tsddbAfterUploadTasks.Image = ShareX.Properties.Resources.upload_cloud;
		resources.ApplyResources(this.tsddbAfterUploadTasks, "tsddbAfterUploadTasks");
		this.tsddbAfterUploadTasks.Name = "tsddbAfterUploadTasks";
		this.tsddbDestinations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.tsmiImageUploaders, this.tsmiTextUploaders, this.tsmiFileUploaders, this.tsmiURLShorteners, this.tsmiURLSharingServices, this.tssDestinations1, this.tsmiDestinationSettings, this.tsmiCustomUploaderSettings });
		this.tsddbDestinations.Image = ShareX.Properties.Resources.drive_globe;
		resources.ApplyResources(this.tsddbDestinations, "tsddbDestinations");
		this.tsddbDestinations.Name = "tsddbDestinations";
		this.tsddbDestinations.DropDownOpened += new System.EventHandler(tsddbDestinations_DropDownOpened);
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
		this.tssDestinations1.Name = "tssDestinations1";
		resources.ApplyResources(this.tssDestinations1, "tssDestinations1");
		this.tsmiDestinationSettings.Image = ShareX.Properties.Resources.globe_pencil;
		this.tsmiDestinationSettings.Name = "tsmiDestinationSettings";
		resources.ApplyResources(this.tsmiDestinationSettings, "tsmiDestinationSettings");
		this.tsmiDestinationSettings.Click += new System.EventHandler(tsmiDestinationSettings_Click);
		this.tsmiCustomUploaderSettings.Image = ShareX.Properties.Resources.network_cloud;
		this.tsmiCustomUploaderSettings.Name = "tsmiCustomUploaderSettings";
		resources.ApplyResources(this.tsmiCustomUploaderSettings, "tsmiCustomUploaderSettings");
		this.tsmiCustomUploaderSettings.Click += new System.EventHandler(tsmiCustomUploaderSettings_Click);
		this.tsbApplicationSettings.Image = ShareX.Properties.Resources.wrench_screwdriver;
		resources.ApplyResources(this.tsbApplicationSettings, "tsbApplicationSettings");
		this.tsbApplicationSettings.Name = "tsbApplicationSettings";
		this.tsbApplicationSettings.Click += new System.EventHandler(tsbApplicationSettings_Click);
		this.tsbTaskSettings.Image = ShareX.Properties.Resources.gear;
		resources.ApplyResources(this.tsbTaskSettings, "tsbTaskSettings");
		this.tsbTaskSettings.Name = "tsbTaskSettings";
		this.tsbTaskSettings.Click += new System.EventHandler(tsbTaskSettings_Click);
		this.tsbHotkeySettings.Image = ShareX.Properties.Resources.keyboard;
		resources.ApplyResources(this.tsbHotkeySettings, "tsbHotkeySettings");
		this.tsbHotkeySettings.Name = "tsbHotkeySettings";
		this.tsbHotkeySettings.Click += new System.EventHandler(tsbHotkeySettings_Click);
		this.tssMain2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 6);
		this.tssMain2.Name = "tssMain2";
		resources.ApplyResources(this.tssMain2, "tssMain2");
		this.tsbScreenshotsFolder.Image = ShareX.Properties.Resources.folder_open_image;
		resources.ApplyResources(this.tsbScreenshotsFolder, "tsbScreenshotsFolder");
		this.tsbScreenshotsFolder.Name = "tsbScreenshotsFolder";
		this.tsbScreenshotsFolder.Click += new System.EventHandler(tsbScreenshotsFolder_Click);
		this.tsbHistory.Image = ShareX.Properties.Resources.application_blog;
		resources.ApplyResources(this.tsbHistory, "tsbHistory");
		this.tsbHistory.Name = "tsbHistory";
		this.tsbHistory.Click += new System.EventHandler(tsbHistory_Click);
		this.tsbImageHistory.Image = ShareX.Properties.Resources.application_icon_large;
		resources.ApplyResources(this.tsbImageHistory, "tsbImageHistory");
		this.tsbImageHistory.Name = "tsbImageHistory";
		this.tsbImageHistory.Click += new System.EventHandler(tsbImageHistory_Click);
		this.tssMain3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 6);
		this.tssMain3.Name = "tssMain3";
		resources.ApplyResources(this.tssMain3, "tssMain3");
		this.tsddbDebug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsmiShowDebugLog, this.tsmiTestImageUpload, this.tsmiTestTextUpload, this.tsmiTestFileUpload, this.tsmiTestURLShortener, this.tsmiTestURLSharing });
		this.tsddbDebug.Image = ShareX.Properties.Resources.traffic_cone;
		resources.ApplyResources(this.tsddbDebug, "tsddbDebug");
		this.tsddbDebug.Name = "tsddbDebug";
		this.tsmiShowDebugLog.Image = ShareX.Properties.Resources.application_monitor;
		this.tsmiShowDebugLog.Name = "tsmiShowDebugLog";
		resources.ApplyResources(this.tsmiShowDebugLog, "tsmiShowDebugLog");
		this.tsmiShowDebugLog.Click += new System.EventHandler(tsmiShowDebugLog_Click);
		this.tsmiTestImageUpload.Image = ShareX.Properties.Resources.image;
		this.tsmiTestImageUpload.Name = "tsmiTestImageUpload";
		resources.ApplyResources(this.tsmiTestImageUpload, "tsmiTestImageUpload");
		this.tsmiTestImageUpload.Click += new System.EventHandler(tsmiTestImageUpload_Click);
		this.tsmiTestTextUpload.Image = ShareX.Properties.Resources.notebook;
		this.tsmiTestTextUpload.Name = "tsmiTestTextUpload";
		resources.ApplyResources(this.tsmiTestTextUpload, "tsmiTestTextUpload");
		this.tsmiTestTextUpload.Click += new System.EventHandler(tsmiTestTextUpload_Click);
		this.tsmiTestFileUpload.Image = ShareX.Properties.Resources.application_block;
		this.tsmiTestFileUpload.Name = "tsmiTestFileUpload";
		resources.ApplyResources(this.tsmiTestFileUpload, "tsmiTestFileUpload");
		this.tsmiTestFileUpload.Click += new System.EventHandler(tsmiTestFileUpload_Click);
		this.tsmiTestURLShortener.Image = ShareX.Properties.Resources.edit_scale;
		this.tsmiTestURLShortener.Name = "tsmiTestURLShortener";
		resources.ApplyResources(this.tsmiTestURLShortener, "tsmiTestURLShortener");
		this.tsmiTestURLShortener.Click += new System.EventHandler(tsmiTestURLShortener_Click);
		this.tsmiTestURLSharing.Image = ShareX.Properties.Resources.globe_share;
		this.tsmiTestURLSharing.Name = "tsmiTestURLSharing";
		resources.ApplyResources(this.tsmiTestURLSharing, "tsmiTestURLSharing");
		this.tsmiTestURLSharing.Click += new System.EventHandler(tsmiTestURLSharing_Click);
		this.tsbDonate.Image = ShareX.Properties.Resources.heart;
		resources.ApplyResources(this.tsbDonate, "tsbDonate");
		this.tsbDonate.Name = "tsbDonate";
		this.tsbDonate.Click += new System.EventHandler(tsbDonate_Click);
		this.tsbTwitter.Image = ShareX.Properties.Resources.Twitter;
		resources.ApplyResources(this.tsbTwitter, "tsbTwitter");
		this.tsbTwitter.Name = "tsbTwitter";
		this.tsbTwitter.Click += new System.EventHandler(tsbTwitter_Click);
		this.tsbDiscord.Image = ShareX.Properties.Resources.Discord;
		resources.ApplyResources(this.tsbDiscord, "tsbDiscord");
		this.tsbDiscord.Name = "tsbDiscord";
		this.tsbDiscord.Click += new System.EventHandler(tsbDiscord_Click);
		this.tsbAbout.Image = ShareX.Properties.Resources.crown;
		resources.ApplyResources(this.tsbAbout, "tsbAbout");
		this.tsbAbout.Name = "tsbAbout";
		this.tsbAbout.Click += new System.EventHandler(tsbAbout_Click);
		this.cmsTaskInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[22]
		{
			this.tsmiShowErrors, this.tsmiStopUpload, this.tsmiOpen, this.tsmiCopy, this.tsmiUploadSelectedFile, this.tsmiDownloadSelectedURL, this.tsmiEditSelectedFile, this.tsmiAddImageEffects, this.tsmiRunAction, this.tsmiDeleteSelectedItem,
			this.tsmiDeleteSelectedFile, this.tsmiShortenSelectedURL, this.tsmiShareSelectedURL, this.tsmiGoogleImageSearch, this.tsmiBingVisualSearch, this.tsmiShowQRCode, this.tsmiOCRImage, this.tsmiCombineImages, this.tsmiShowResponse, this.tsmiClearList,
			this.tssUploadInfo1, this.tsmiSwitchTaskViewMode
		});
		this.cmsTaskInfo.Name = "cmsHistory";
		resources.ApplyResources(this.cmsTaskInfo, "cmsTaskInfo");
		this.cmsTaskInfo.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(cmsTaskInfo_Closing);
		this.cmsTaskInfo.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(cmsTaskInfo_PreviewKeyDown);
		this.tsmiShowErrors.Image = ShareX.Properties.Resources.exclamation_button;
		this.tsmiShowErrors.Name = "tsmiShowErrors";
		resources.ApplyResources(this.tsmiShowErrors, "tsmiShowErrors");
		this.tsmiShowErrors.Click += new System.EventHandler(tsmiShowErrors_Click);
		this.tsmiStopUpload.Image = ShareX.Properties.Resources.cross_button;
		this.tsmiStopUpload.Name = "tsmiStopUpload";
		resources.ApplyResources(this.tsmiStopUpload, "tsmiStopUpload");
		this.tsmiStopUpload.Click += new System.EventHandler(tsmiStopUpload_Click);
		this.tsmiOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.tsmiOpenURL, this.tsmiOpenShortenedURL, this.tsmiOpenThumbnailURL, this.tsmiOpenDeletionURL, this.tssOpen1, this.tsmiOpenFile, this.tsmiOpenFolder, this.tsmiOpenThumbnailFile });
		this.tsmiOpen.Image = ShareX.Properties.Resources.folder_open_document;
		this.tsmiOpen.Name = "tsmiOpen";
		resources.ApplyResources(this.tsmiOpen, "tsmiOpen");
		this.tsmiOpenURL.Name = "tsmiOpenURL";
		resources.ApplyResources(this.tsmiOpenURL, "tsmiOpenURL");
		this.tsmiOpenURL.Click += new System.EventHandler(tsmiOpenURL_Click);
		this.tsmiOpenShortenedURL.Name = "tsmiOpenShortenedURL";
		resources.ApplyResources(this.tsmiOpenShortenedURL, "tsmiOpenShortenedURL");
		this.tsmiOpenShortenedURL.Click += new System.EventHandler(tsmiOpenShortenedURL_Click);
		this.tsmiOpenThumbnailURL.Name = "tsmiOpenThumbnailURL";
		resources.ApplyResources(this.tsmiOpenThumbnailURL, "tsmiOpenThumbnailURL");
		this.tsmiOpenThumbnailURL.Click += new System.EventHandler(tsmiOpenThumbnailURL_Click);
		this.tsmiOpenDeletionURL.Name = "tsmiOpenDeletionURL";
		resources.ApplyResources(this.tsmiOpenDeletionURL, "tsmiOpenDeletionURL");
		this.tsmiOpenDeletionURL.Click += new System.EventHandler(tsmiOpenDeletionURL_Click);
		this.tssOpen1.Name = "tssOpen1";
		resources.ApplyResources(this.tssOpen1, "tssOpen1");
		this.tsmiOpenFile.Name = "tsmiOpenFile";
		resources.ApplyResources(this.tsmiOpenFile, "tsmiOpenFile");
		this.tsmiOpenFile.Click += new System.EventHandler(tsmiOpenFile_Click);
		this.tsmiOpenFolder.Name = "tsmiOpenFolder";
		resources.ApplyResources(this.tsmiOpenFolder, "tsmiOpenFolder");
		this.tsmiOpenFolder.Click += new System.EventHandler(tsmiOpenFolder_Click);
		this.tsmiOpenThumbnailFile.Name = "tsmiOpenThumbnailFile";
		resources.ApplyResources(this.tsmiOpenThumbnailFile, "tsmiOpenThumbnailFile");
		this.tsmiOpenThumbnailFile.Click += new System.EventHandler(tsmiOpenThumbnailFile_Click);
		this.tsmiCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[29]
		{
			this.tsmiCopyURL, this.tsmiCopyShortenedURL, this.tsmiCopyThumbnailURL, this.tsmiCopyDeletionURL, this.tssCopy1, this.tsmiCopyFile, this.tsmiCopyImage, this.tsmiCopyImageDimensions, this.tsmiCopyText, this.tsmiCopyThumbnailFile,
			this.tsmiCopyThumbnailImage, this.tssCopy2, this.tsmiCopyHTMLLink, this.tsmiCopyHTMLImage, this.tsmiCopyHTMLLinkedImage, this.tssCopy3, this.tsmiCopyForumLink, this.tsmiCopyForumImage, this.tsmiCopyForumLinkedImage, this.tssCopy4,
			this.tsmiCopyMarkdownLink, this.tsmiCopyMarkdownImage, this.tsmiCopyMarkdownLinkedImage, this.tssCopy5, this.tsmiCopyFilePath, this.tsmiCopyFileName, this.tsmiCopyFileNameWithExtension, this.tsmiCopyFolder, this.tssCopy6
		});
		this.tsmiCopy.Image = ShareX.Properties.Resources.document_copy;
		this.tsmiCopy.Name = "tsmiCopy";
		resources.ApplyResources(this.tsmiCopy, "tsmiCopy");
		this.tsmiCopyURL.Name = "tsmiCopyURL";
		resources.ApplyResources(this.tsmiCopyURL, "tsmiCopyURL");
		this.tsmiCopyURL.Click += new System.EventHandler(tsmiCopyURL_Click);
		this.tsmiCopyShortenedURL.Name = "tsmiCopyShortenedURL";
		resources.ApplyResources(this.tsmiCopyShortenedURL, "tsmiCopyShortenedURL");
		this.tsmiCopyShortenedURL.Click += new System.EventHandler(tsmiCopyShortenedURL_Click);
		this.tsmiCopyThumbnailURL.Name = "tsmiCopyThumbnailURL";
		resources.ApplyResources(this.tsmiCopyThumbnailURL, "tsmiCopyThumbnailURL");
		this.tsmiCopyThumbnailURL.Click += new System.EventHandler(tsmiCopyThumbnailURL_Click);
		this.tsmiCopyDeletionURL.Name = "tsmiCopyDeletionURL";
		resources.ApplyResources(this.tsmiCopyDeletionURL, "tsmiCopyDeletionURL");
		this.tsmiCopyDeletionURL.Click += new System.EventHandler(tsmiCopyDeletionURL_Click);
		this.tssCopy1.Name = "tssCopy1";
		resources.ApplyResources(this.tssCopy1, "tssCopy1");
		this.tsmiCopyFile.Name = "tsmiCopyFile";
		resources.ApplyResources(this.tsmiCopyFile, "tsmiCopyFile");
		this.tsmiCopyFile.Click += new System.EventHandler(tsmiCopyFile_Click);
		this.tsmiCopyImage.Name = "tsmiCopyImage";
		resources.ApplyResources(this.tsmiCopyImage, "tsmiCopyImage");
		this.tsmiCopyImage.Click += new System.EventHandler(tsmiCopyImage_Click);
		this.tsmiCopyImageDimensions.Name = "tsmiCopyImageDimensions";
		resources.ApplyResources(this.tsmiCopyImageDimensions, "tsmiCopyImageDimensions");
		this.tsmiCopyImageDimensions.Click += new System.EventHandler(tsmiCopyImageDimensions_Click);
		this.tsmiCopyText.Name = "tsmiCopyText";
		resources.ApplyResources(this.tsmiCopyText, "tsmiCopyText");
		this.tsmiCopyText.Click += new System.EventHandler(tsmiCopyText_Click);
		this.tsmiCopyThumbnailFile.Name = "tsmiCopyThumbnailFile";
		resources.ApplyResources(this.tsmiCopyThumbnailFile, "tsmiCopyThumbnailFile");
		this.tsmiCopyThumbnailFile.Click += new System.EventHandler(tsmiCopyThumbnailFile_Click);
		this.tsmiCopyThumbnailImage.Name = "tsmiCopyThumbnailImage";
		resources.ApplyResources(this.tsmiCopyThumbnailImage, "tsmiCopyThumbnailImage");
		this.tsmiCopyThumbnailImage.Click += new System.EventHandler(tsmiCopyThumbnailImage_Click);
		this.tssCopy2.Name = "tssCopy2";
		resources.ApplyResources(this.tssCopy2, "tssCopy2");
		this.tsmiCopyHTMLLink.Name = "tsmiCopyHTMLLink";
		resources.ApplyResources(this.tsmiCopyHTMLLink, "tsmiCopyHTMLLink");
		this.tsmiCopyHTMLLink.Click += new System.EventHandler(tsmiCopyHTMLLink_Click);
		this.tsmiCopyHTMLImage.Name = "tsmiCopyHTMLImage";
		resources.ApplyResources(this.tsmiCopyHTMLImage, "tsmiCopyHTMLImage");
		this.tsmiCopyHTMLImage.Click += new System.EventHandler(tsmiCopyHTMLImage_Click);
		this.tsmiCopyHTMLLinkedImage.Name = "tsmiCopyHTMLLinkedImage";
		resources.ApplyResources(this.tsmiCopyHTMLLinkedImage, "tsmiCopyHTMLLinkedImage");
		this.tsmiCopyHTMLLinkedImage.Click += new System.EventHandler(tsmiCopyHTMLLinkedImage_Click);
		this.tssCopy3.Name = "tssCopy3";
		resources.ApplyResources(this.tssCopy3, "tssCopy3");
		this.tsmiCopyForumLink.Name = "tsmiCopyForumLink";
		resources.ApplyResources(this.tsmiCopyForumLink, "tsmiCopyForumLink");
		this.tsmiCopyForumLink.Click += new System.EventHandler(tsmiCopyForumLink_Click);
		this.tsmiCopyForumImage.Name = "tsmiCopyForumImage";
		resources.ApplyResources(this.tsmiCopyForumImage, "tsmiCopyForumImage");
		this.tsmiCopyForumImage.Click += new System.EventHandler(tsmiCopyForumImage_Click);
		this.tsmiCopyForumLinkedImage.Name = "tsmiCopyForumLinkedImage";
		resources.ApplyResources(this.tsmiCopyForumLinkedImage, "tsmiCopyForumLinkedImage");
		this.tsmiCopyForumLinkedImage.Click += new System.EventHandler(tsmiCopyForumLinkedImage_Click);
		this.tssCopy4.Name = "tssCopy4";
		resources.ApplyResources(this.tssCopy4, "tssCopy4");
		this.tsmiCopyMarkdownLink.Name = "tsmiCopyMarkdownLink";
		resources.ApplyResources(this.tsmiCopyMarkdownLink, "tsmiCopyMarkdownLink");
		this.tsmiCopyMarkdownLink.Click += new System.EventHandler(tsmiCopyMarkdownLink_Click);
		this.tsmiCopyMarkdownImage.Name = "tsmiCopyMarkdownImage";
		resources.ApplyResources(this.tsmiCopyMarkdownImage, "tsmiCopyMarkdownImage");
		this.tsmiCopyMarkdownImage.Click += new System.EventHandler(tsmiCopyMarkdownImage_Click);
		this.tsmiCopyMarkdownLinkedImage.Name = "tsmiCopyMarkdownLinkedImage";
		resources.ApplyResources(this.tsmiCopyMarkdownLinkedImage, "tsmiCopyMarkdownLinkedImage");
		this.tsmiCopyMarkdownLinkedImage.Click += new System.EventHandler(tsmiCopyMarkdownLinkedImage_Click);
		this.tssCopy5.Name = "tssCopy5";
		resources.ApplyResources(this.tssCopy5, "tssCopy5");
		this.tsmiCopyFilePath.Name = "tsmiCopyFilePath";
		resources.ApplyResources(this.tsmiCopyFilePath, "tsmiCopyFilePath");
		this.tsmiCopyFilePath.Click += new System.EventHandler(tsmiCopyFilePath_Click);
		this.tsmiCopyFileName.Name = "tsmiCopyFileName";
		resources.ApplyResources(this.tsmiCopyFileName, "tsmiCopyFileName");
		this.tsmiCopyFileName.Click += new System.EventHandler(tsmiCopyFileName_Click);
		this.tsmiCopyFileNameWithExtension.Name = "tsmiCopyFileNameWithExtension";
		resources.ApplyResources(this.tsmiCopyFileNameWithExtension, "tsmiCopyFileNameWithExtension");
		this.tsmiCopyFileNameWithExtension.Click += new System.EventHandler(tsmiCopyFileNameWithExtension_Click);
		this.tsmiCopyFolder.Name = "tsmiCopyFolder";
		resources.ApplyResources(this.tsmiCopyFolder, "tsmiCopyFolder");
		this.tsmiCopyFolder.Click += new System.EventHandler(tsmiCopyFolder_Click);
		this.tssCopy6.Name = "tssCopy6";
		resources.ApplyResources(this.tssCopy6, "tssCopy6");
		this.tsmiUploadSelectedFile.Image = ShareX.Properties.Resources.drive_upload;
		this.tsmiUploadSelectedFile.Name = "tsmiUploadSelectedFile";
		resources.ApplyResources(this.tsmiUploadSelectedFile, "tsmiUploadSelectedFile");
		this.tsmiUploadSelectedFile.Click += new System.EventHandler(tsmiUploadSelectedFile_Click);
		this.tsmiDownloadSelectedURL.Image = ShareX.Properties.Resources.drive_download;
		this.tsmiDownloadSelectedURL.Name = "tsmiDownloadSelectedURL";
		resources.ApplyResources(this.tsmiDownloadSelectedURL, "tsmiDownloadSelectedURL");
		this.tsmiDownloadSelectedURL.Click += new System.EventHandler(tsmiDownloadSelectedURL_Click);
		this.tsmiEditSelectedFile.Image = ShareX.Properties.Resources.image_pencil;
		this.tsmiEditSelectedFile.Name = "tsmiEditSelectedFile";
		resources.ApplyResources(this.tsmiEditSelectedFile, "tsmiEditSelectedFile");
		this.tsmiEditSelectedFile.Click += new System.EventHandler(tsmiEditSelectedFile_Click);
		this.tsmiAddImageEffects.Image = ShareX.Properties.Resources.image_saturation;
		this.tsmiAddImageEffects.Name = "tsmiAddImageEffects";
		resources.ApplyResources(this.tsmiAddImageEffects, "tsmiAddImageEffects");
		this.tsmiAddImageEffects.Click += new System.EventHandler(tsmiAddImageEffects_Click);
		this.tsmiRunAction.Image = ShareX.Properties.Resources.application_terminal;
		this.tsmiRunAction.Name = "tsmiRunAction";
		resources.ApplyResources(this.tsmiRunAction, "tsmiRunAction");
		this.tsmiDeleteSelectedItem.Image = ShareX.Properties.Resources.script__minus;
		this.tsmiDeleteSelectedItem.Name = "tsmiDeleteSelectedItem";
		resources.ApplyResources(this.tsmiDeleteSelectedItem, "tsmiDeleteSelectedItem");
		this.tsmiDeleteSelectedItem.Click += new System.EventHandler(tsmiDeleteSelectedItem_Click);
		this.tsmiDeleteSelectedFile.Image = ShareX.Properties.Resources.bin;
		this.tsmiDeleteSelectedFile.Name = "tsmiDeleteSelectedFile";
		resources.ApplyResources(this.tsmiDeleteSelectedFile, "tsmiDeleteSelectedFile");
		this.tsmiDeleteSelectedFile.Click += new System.EventHandler(tsmiDeleteSelectedFile_Click);
		this.tsmiShortenSelectedURL.Image = ShareX.Properties.Resources.edit_scale;
		this.tsmiShortenSelectedURL.Name = "tsmiShortenSelectedURL";
		resources.ApplyResources(this.tsmiShortenSelectedURL, "tsmiShortenSelectedURL");
		this.tsmiShareSelectedURL.Image = ShareX.Properties.Resources.globe_share;
		this.tsmiShareSelectedURL.Name = "tsmiShareSelectedURL";
		resources.ApplyResources(this.tsmiShareSelectedURL, "tsmiShareSelectedURL");
		this.tsmiGoogleImageSearch.Image = ShareX.Properties.Resources.Google;
		this.tsmiGoogleImageSearch.Name = "tsmiGoogleImageSearch";
		resources.ApplyResources(this.tsmiGoogleImageSearch, "tsmiGoogleImageSearch");
		this.tsmiGoogleImageSearch.Click += new System.EventHandler(tsmiGoogleImageSearch_Click);
		this.tsmiBingVisualSearch.Image = ShareX.Properties.Resources.Bing;
		this.tsmiBingVisualSearch.Name = "tsmiBingVisualSearch";
		resources.ApplyResources(this.tsmiBingVisualSearch, "tsmiBingVisualSearch");
		this.tsmiBingVisualSearch.Click += new System.EventHandler(tsmiBingVisualSearch_Click);
		this.tsmiShowQRCode.Image = ShareX.Properties.Resources.barcode_2d;
		this.tsmiShowQRCode.Name = "tsmiShowQRCode";
		resources.ApplyResources(this.tsmiShowQRCode, "tsmiShowQRCode");
		this.tsmiShowQRCode.Click += new System.EventHandler(tsmiShowQRCode_Click);
		this.tsmiOCRImage.Image = ShareX.Properties.Resources.edit_drop_cap;
		this.tsmiOCRImage.Name = "tsmiOCRImage";
		resources.ApplyResources(this.tsmiOCRImage, "tsmiOCRImage");
		this.tsmiOCRImage.Click += new System.EventHandler(tsmiOCRImage_Click);
		this.tsmiCombineImages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmiCombineImagesHorizontally, this.tsmiCombineImagesVertically });
		this.tsmiCombineImages.Image = ShareX.Properties.Resources.document_break;
		this.tsmiCombineImages.Name = "tsmiCombineImages";
		resources.ApplyResources(this.tsmiCombineImages, "tsmiCombineImages");
		this.tsmiCombineImages.Click += new System.EventHandler(tsmiCombineImages_Click);
		this.tsmiCombineImagesHorizontally.Image = ShareX.Properties.Resources.application_tile_horizontal;
		this.tsmiCombineImagesHorizontally.Name = "tsmiCombineImagesHorizontally";
		resources.ApplyResources(this.tsmiCombineImagesHorizontally, "tsmiCombineImagesHorizontally");
		this.tsmiCombineImagesHorizontally.Click += new System.EventHandler(tsmiCombineImagesHorizontally_Click);
		this.tsmiCombineImagesVertically.Image = ShareX.Properties.Resources.application_tile_vertical;
		this.tsmiCombineImagesVertically.Name = "tsmiCombineImagesVertically";
		resources.ApplyResources(this.tsmiCombineImagesVertically, "tsmiCombineImagesVertically");
		this.tsmiCombineImagesVertically.Click += new System.EventHandler(tsmiCombineImagesVertically_Click);
		this.tsmiShowResponse.Image = ShareX.Properties.Resources.application_browser;
		this.tsmiShowResponse.Name = "tsmiShowResponse";
		resources.ApplyResources(this.tsmiShowResponse, "tsmiShowResponse");
		this.tsmiShowResponse.Click += new System.EventHandler(tsmiShowResponse_Click);
		this.tsmiClearList.Image = ShareX.Properties.Resources.eraser;
		this.tsmiClearList.Name = "tsmiClearList";
		resources.ApplyResources(this.tsmiClearList, "tsmiClearList");
		this.tsmiClearList.Click += new System.EventHandler(tsmiClearList_Click);
		this.tssUploadInfo1.Name = "tssUploadInfo1";
		resources.ApplyResources(this.tssUploadInfo1, "tssUploadInfo1");
		this.tsmiSwitchTaskViewMode.Name = "tsmiSwitchTaskViewMode";
		resources.ApplyResources(this.tsmiSwitchTaskViewMode, "tsmiSwitchTaskViewMode");
		this.tsmiSwitchTaskViewMode.Click += new System.EventHandler(TsmiSwitchTaskViewMode_Click);
		this.niTray.ContextMenuStrip = this.cmsTray;
		resources.ApplyResources(this.niTray, "niTray");
		this.niTray.BalloonTipClicked += new System.EventHandler(niTray_BalloonTipClicked);
		this.niTray.MouseUp += new System.Windows.Forms.MouseEventHandler(niTray_MouseUp);
		this.cmsTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[22]
		{
			this.tsmiTrayCapture, this.tsmiTrayUpload, this.tsmiTrayWorkflows, this.tsmiTrayTools, this.tssTray1, this.tsmiTrayAfterCaptureTasks, this.tsmiTrayAfterUploadTasks, this.tsmiTrayDestinations, this.tsmiTrayApplicationSettings, this.tsmiTrayTaskSettings,
			this.tsmiTrayHotkeySettings, this.tsmiTrayToggleHotkeys, this.tssTray2, this.tsmiScreenshotsFolder, this.tsmiTrayHistory, this.tsmiTrayImageHistory, this.tssTray3, this.tsmiRestartAsAdmin, this.tsmiTrayRecentItems, this.tsmiOpenActionsToolbar,
			this.tsmiTrayShow, this.tsmiTrayExit
		});
		this.cmsTray.Name = "cmsTray";
		resources.ApplyResources(this.cmsTray, "cmsTray");
		this.cmsTray.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(cmsTray_Closed);
		this.cmsTray.Opened += new System.EventHandler(cmsTray_Opened);
		this.tsmiTrayCapture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.tsmiTrayFullscreen, this.tsmiTrayWindow, this.tsmiTrayMonitor, this.tsmiTrayRectangle, this.tsmiTrayRectangleLight, this.tsmiTrayRectangleTransparent, this.tsmiTrayLastRegion, this.tsmiTrayScreenRecordingFFmpeg, this.tsmiTrayScreenRecordingGIF, this.tsmiTrayScrollingCapture,
			this.tsmiTrayAutoCapture, this.tssTrayCapture1, this.tsmiTrayShowCursor, this.tsmiTrayScreenshotDelay
		});
		this.tsmiTrayCapture.Image = ShareX.Properties.Resources.camera;
		this.tsmiTrayCapture.Name = "tsmiTrayCapture";
		resources.ApplyResources(this.tsmiTrayCapture, "tsmiTrayCapture");
		this.tsmiTrayCapture.DropDownOpening += new System.EventHandler(tsmiCapture_DropDownOpening);
		this.tsmiTrayFullscreen.Image = ShareX.Properties.Resources.layer_fullscreen;
		this.tsmiTrayFullscreen.Name = "tsmiTrayFullscreen";
		resources.ApplyResources(this.tsmiTrayFullscreen, "tsmiTrayFullscreen");
		this.tsmiTrayFullscreen.Click += new System.EventHandler(tsmiTrayFullscreen_Click);
		this.tsmiTrayWindow.Image = ShareX.Properties.Resources.application_blue;
		this.tsmiTrayWindow.Name = "tsmiTrayWindow";
		resources.ApplyResources(this.tsmiTrayWindow, "tsmiTrayWindow");
		this.tsmiTrayMonitor.Image = ShareX.Properties.Resources.monitor;
		this.tsmiTrayMonitor.Name = "tsmiTrayMonitor";
		resources.ApplyResources(this.tsmiTrayMonitor, "tsmiTrayMonitor");
		this.tsmiTrayRectangle.Image = ShareX.Properties.Resources.layer_shape;
		this.tsmiTrayRectangle.Name = "tsmiTrayRectangle";
		resources.ApplyResources(this.tsmiTrayRectangle, "tsmiTrayRectangle");
		this.tsmiTrayRectangle.Click += new System.EventHandler(tsmiTrayRectangle_Click);
		this.tsmiTrayRectangleLight.Image = ShareX.Properties.Resources.Rectangle;
		this.tsmiTrayRectangleLight.Name = "tsmiTrayRectangleLight";
		resources.ApplyResources(this.tsmiTrayRectangleLight, "tsmiTrayRectangleLight");
		this.tsmiTrayRectangleLight.Click += new System.EventHandler(tsmiTrayRectangleLight_Click);
		this.tsmiTrayRectangleTransparent.Image = ShareX.Properties.Resources.layer_transparent;
		this.tsmiTrayRectangleTransparent.Name = "tsmiTrayRectangleTransparent";
		resources.ApplyResources(this.tsmiTrayRectangleTransparent, "tsmiTrayRectangleTransparent");
		this.tsmiTrayRectangleTransparent.Click += new System.EventHandler(tsmiTrayRectangleTransparent_Click);
		this.tsmiTrayLastRegion.Image = ShareX.Properties.Resources.layers;
		this.tsmiTrayLastRegion.Name = "tsmiTrayLastRegion";
		resources.ApplyResources(this.tsmiTrayLastRegion, "tsmiTrayLastRegion");
		this.tsmiTrayLastRegion.Click += new System.EventHandler(tsmiTrayLastRegion_Click);
		this.tsmiTrayScreenRecordingFFmpeg.Image = ShareX.Properties.Resources.camcorder_image;
		this.tsmiTrayScreenRecordingFFmpeg.Name = "tsmiTrayScreenRecordingFFmpeg";
		resources.ApplyResources(this.tsmiTrayScreenRecordingFFmpeg, "tsmiTrayScreenRecordingFFmpeg");
		this.tsmiTrayScreenRecordingFFmpeg.Click += new System.EventHandler(tsmiScreenRecordingFFmpeg_Click);
		this.tsmiTrayScreenRecordingGIF.Image = ShareX.Properties.Resources.film;
		this.tsmiTrayScreenRecordingGIF.Name = "tsmiTrayScreenRecordingGIF";
		resources.ApplyResources(this.tsmiTrayScreenRecordingGIF, "tsmiTrayScreenRecordingGIF");
		this.tsmiTrayScreenRecordingGIF.Click += new System.EventHandler(tsmiScreenRecordingGIF_Click);
		this.tsmiTrayScrollingCapture.Image = ShareX.Properties.Resources.ui_scroll_pane_image;
		this.tsmiTrayScrollingCapture.Name = "tsmiTrayScrollingCapture";
		resources.ApplyResources(this.tsmiTrayScrollingCapture, "tsmiTrayScrollingCapture");
		this.tsmiTrayScrollingCapture.Click += new System.EventHandler(tsmiScrollingCapture_Click);
		this.tsmiTrayAutoCapture.Image = ShareX.Properties.Resources.clock;
		this.tsmiTrayAutoCapture.Name = "tsmiTrayAutoCapture";
		resources.ApplyResources(this.tsmiTrayAutoCapture, "tsmiTrayAutoCapture");
		this.tsmiTrayAutoCapture.Click += new System.EventHandler(tsmiAutoCapture_Click);
		this.tssTrayCapture1.Name = "tssTrayCapture1";
		resources.ApplyResources(this.tssTrayCapture1, "tssTrayCapture1");
		this.tsmiTrayShowCursor.CheckOnClick = true;
		this.tsmiTrayShowCursor.Image = ShareX.Properties.Resources.cursor;
		this.tsmiTrayShowCursor.Name = "tsmiTrayShowCursor";
		resources.ApplyResources(this.tsmiTrayShowCursor, "tsmiTrayShowCursor");
		this.tsmiTrayShowCursor.Click += new System.EventHandler(tsmiShowCursor_Click);
		this.tsmiTrayScreenshotDelay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsmiTrayScreenshotDelay0, this.tsmiTrayScreenshotDelay1, this.tsmiTrayScreenshotDelay2, this.tsmiTrayScreenshotDelay3, this.tsmiTrayScreenshotDelay4, this.tsmiTrayScreenshotDelay5 });
		this.tsmiTrayScreenshotDelay.Image = ShareX.Properties.Resources.clock_select;
		this.tsmiTrayScreenshotDelay.Name = "tsmiTrayScreenshotDelay";
		resources.ApplyResources(this.tsmiTrayScreenshotDelay, "tsmiTrayScreenshotDelay");
		this.tsmiTrayScreenshotDelay0.Name = "tsmiTrayScreenshotDelay0";
		resources.ApplyResources(this.tsmiTrayScreenshotDelay0, "tsmiTrayScreenshotDelay0");
		this.tsmiTrayScreenshotDelay0.Click += new System.EventHandler(tsmiScreenshotDelay0_Click);
		this.tsmiTrayScreenshotDelay1.Name = "tsmiTrayScreenshotDelay1";
		resources.ApplyResources(this.tsmiTrayScreenshotDelay1, "tsmiTrayScreenshotDelay1");
		this.tsmiTrayScreenshotDelay1.Click += new System.EventHandler(tsmiScreenshotDelay1_Click);
		this.tsmiTrayScreenshotDelay2.Name = "tsmiTrayScreenshotDelay2";
		resources.ApplyResources(this.tsmiTrayScreenshotDelay2, "tsmiTrayScreenshotDelay2");
		this.tsmiTrayScreenshotDelay2.Click += new System.EventHandler(tsmiScreenshotDelay2_Click);
		this.tsmiTrayScreenshotDelay3.Name = "tsmiTrayScreenshotDelay3";
		resources.ApplyResources(this.tsmiTrayScreenshotDelay3, "tsmiTrayScreenshotDelay3");
		this.tsmiTrayScreenshotDelay3.Click += new System.EventHandler(tsmiScreenshotDelay3_Click);
		this.tsmiTrayScreenshotDelay4.Name = "tsmiTrayScreenshotDelay4";
		resources.ApplyResources(this.tsmiTrayScreenshotDelay4, "tsmiTrayScreenshotDelay4");
		this.tsmiTrayScreenshotDelay4.Click += new System.EventHandler(tsmiScreenshotDelay4_Click);
		this.tsmiTrayScreenshotDelay5.Name = "tsmiTrayScreenshotDelay5";
		resources.ApplyResources(this.tsmiTrayScreenshotDelay5, "tsmiTrayScreenshotDelay5");
		this.tsmiTrayScreenshotDelay5.Click += new System.EventHandler(tsmiScreenshotDelay5_Click);
		this.tsmiTrayUpload.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.tsmiTrayUploadFile, this.tsmiTrayUploadFolder, this.tsmiTrayUploadClipboard, this.tsmiTrayUploadText, this.tsmiTrayUploadURL, this.tsmiTrayUploadDragDrop, this.tsmiTrayShortenURL, this.tsmiTrayTweetMessage });
		this.tsmiTrayUpload.Image = ShareX.Properties.Resources.arrow_090;
		this.tsmiTrayUpload.Name = "tsmiTrayUpload";
		resources.ApplyResources(this.tsmiTrayUpload, "tsmiTrayUpload");
		this.tsmiTrayUploadFile.Image = ShareX.Properties.Resources.folder_open_document;
		this.tsmiTrayUploadFile.Name = "tsmiTrayUploadFile";
		resources.ApplyResources(this.tsmiTrayUploadFile, "tsmiTrayUploadFile");
		this.tsmiTrayUploadFile.Click += new System.EventHandler(tsbFileUpload_Click);
		this.tsmiTrayUploadFolder.Image = ShareX.Properties.Resources.folder;
		this.tsmiTrayUploadFolder.Name = "tsmiTrayUploadFolder";
		resources.ApplyResources(this.tsmiTrayUploadFolder, "tsmiTrayUploadFolder");
		this.tsmiTrayUploadFolder.Click += new System.EventHandler(tsmiUploadFolder_Click);
		this.tsmiTrayUploadClipboard.Image = ShareX.Properties.Resources.clipboard;
		this.tsmiTrayUploadClipboard.Name = "tsmiTrayUploadClipboard";
		resources.ApplyResources(this.tsmiTrayUploadClipboard, "tsmiTrayUploadClipboard");
		this.tsmiTrayUploadClipboard.Click += new System.EventHandler(tsbClipboardUpload_Click);
		this.tsmiTrayUploadText.Image = ShareX.Properties.Resources.notebook;
		this.tsmiTrayUploadText.Name = "tsmiTrayUploadText";
		resources.ApplyResources(this.tsmiTrayUploadText, "tsmiTrayUploadText");
		this.tsmiTrayUploadText.Click += new System.EventHandler(tsmiUploadText_Click);
		this.tsmiTrayUploadURL.Image = ShareX.Properties.Resources.drive;
		this.tsmiTrayUploadURL.Name = "tsmiTrayUploadURL";
		resources.ApplyResources(this.tsmiTrayUploadURL, "tsmiTrayUploadURL");
		this.tsmiTrayUploadURL.Click += new System.EventHandler(tsmiUploadURL_Click);
		this.tsmiTrayUploadDragDrop.Image = ShareX.Properties.Resources.inbox;
		this.tsmiTrayUploadDragDrop.Name = "tsmiTrayUploadDragDrop";
		resources.ApplyResources(this.tsmiTrayUploadDragDrop, "tsmiTrayUploadDragDrop");
		this.tsmiTrayUploadDragDrop.Click += new System.EventHandler(tsbDragDropUpload_Click);
		this.tsmiTrayShortenURL.Image = ShareX.Properties.Resources.edit_scale;
		this.tsmiTrayShortenURL.Name = "tsmiTrayShortenURL";
		resources.ApplyResources(this.tsmiTrayShortenURL, "tsmiTrayShortenURL");
		this.tsmiTrayShortenURL.Click += new System.EventHandler(tsmiShortenURL_Click);
		this.tsmiTrayTweetMessage.Image = ShareX.Properties.Resources.Twitter;
		this.tsmiTrayTweetMessage.Name = "tsmiTrayTweetMessage";
		resources.ApplyResources(this.tsmiTrayTweetMessage, "tsmiTrayTweetMessage");
		this.tsmiTrayTweetMessage.Click += new System.EventHandler(tsmiTweetMessage_Click);
		this.tsmiTrayWorkflows.Image = ShareX.Properties.Resources.categories;
		this.tsmiTrayWorkflows.Name = "tsmiTrayWorkflows";
		resources.ApplyResources(this.tsmiTrayWorkflows, "tsmiTrayWorkflows");
		this.tsmiTrayTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[24]
		{
			this.tsmiTrayColorPicker, this.tsmiTrayScreenColorPicker, this.tsmiTrayRuler, this.tssTrayTools1, this.tsmiTrayImageEditor, this.tsmiTrayImageEffects, this.tsmiTrayImageViewer, this.tsmiTrayImageCombiner, this.tsmiTrayImageSplitter, this.tsmiTrayImageThumbnailer,
			this.tssTrayTools2, this.tsmiTrayVideoConverter, this.tsmiTrayVideoThumbnailer, this.tssTrayTools3, this.tsmiTrayOCR, this.tsmiTrayQRCode, this.tsmiTrayHashCheck, this.tsmiTrayIndexFolder, this.tssTrayTools4, this.tsmiTrayClipboardViewer,
			this.tsmiTrayBorderlessWindow, this.tsmiTrayInspectWindow, this.tsmiTrayMonitorTest, this.tsmiTrayDNSChanger
		});
		this.tsmiTrayTools.Image = ShareX.Properties.Resources.toolbox;
		this.tsmiTrayTools.Name = "tsmiTrayTools";
		resources.ApplyResources(this.tsmiTrayTools, "tsmiTrayTools");
		this.tsmiTrayColorPicker.Image = ShareX.Properties.Resources.color;
		this.tsmiTrayColorPicker.Name = "tsmiTrayColorPicker";
		resources.ApplyResources(this.tsmiTrayColorPicker, "tsmiTrayColorPicker");
		this.tsmiTrayColorPicker.Click += new System.EventHandler(tsmiColorPicker_Click);
		this.tsmiTrayScreenColorPicker.Image = ShareX.Properties.Resources.pipette;
		this.tsmiTrayScreenColorPicker.Name = "tsmiTrayScreenColorPicker";
		resources.ApplyResources(this.tsmiTrayScreenColorPicker, "tsmiTrayScreenColorPicker");
		this.tsmiTrayScreenColorPicker.Click += new System.EventHandler(tsmiScreenColorPicker_Click);
		this.tsmiTrayRuler.Image = ShareX.Properties.Resources.ruler_triangle;
		this.tsmiTrayRuler.Name = "tsmiTrayRuler";
		resources.ApplyResources(this.tsmiTrayRuler, "tsmiTrayRuler");
		this.tsmiTrayRuler.Click += new System.EventHandler(tsmiRuler_Click);
		this.tssTrayTools1.Name = "tssTrayTools1";
		resources.ApplyResources(this.tssTrayTools1, "tssTrayTools1");
		this.tsmiTrayImageEditor.Image = ShareX.Properties.Resources.image_pencil;
		this.tsmiTrayImageEditor.Name = "tsmiTrayImageEditor";
		resources.ApplyResources(this.tsmiTrayImageEditor, "tsmiTrayImageEditor");
		this.tsmiTrayImageEditor.Click += new System.EventHandler(tsmiImageEditor_Click);
		this.tsmiTrayImageEffects.Image = ShareX.Properties.Resources.image_saturation;
		this.tsmiTrayImageEffects.Name = "tsmiTrayImageEffects";
		resources.ApplyResources(this.tsmiTrayImageEffects, "tsmiTrayImageEffects");
		this.tsmiTrayImageEffects.Click += new System.EventHandler(tsmiImageEffects_Click);
		this.tsmiTrayImageViewer.Image = ShareX.Properties.Resources.images_flickr;
		this.tsmiTrayImageViewer.Name = "tsmiTrayImageViewer";
		resources.ApplyResources(this.tsmiTrayImageViewer, "tsmiTrayImageViewer");
		this.tsmiTrayImageViewer.Click += new System.EventHandler(tsmiImageViewer_Click);
		this.tsmiTrayImageCombiner.Image = ShareX.Properties.Resources.document_break;
		this.tsmiTrayImageCombiner.Name = "tsmiTrayImageCombiner";
		resources.ApplyResources(this.tsmiTrayImageCombiner, "tsmiTrayImageCombiner");
		this.tsmiTrayImageCombiner.Click += new System.EventHandler(tsmiImageCombiner_Click);
		this.tsmiTrayImageSplitter.Image = ShareX.Properties.Resources.image_split;
		this.tsmiTrayImageSplitter.Name = "tsmiTrayImageSplitter";
		resources.ApplyResources(this.tsmiTrayImageSplitter, "tsmiTrayImageSplitter");
		this.tsmiTrayImageSplitter.Click += new System.EventHandler(TsmiImageSplitter_Click);
		this.tsmiTrayImageThumbnailer.Image = ShareX.Properties.Resources.image_resize_actual;
		this.tsmiTrayImageThumbnailer.Name = "tsmiTrayImageThumbnailer";
		resources.ApplyResources(this.tsmiTrayImageThumbnailer, "tsmiTrayImageThumbnailer");
		this.tsmiTrayImageThumbnailer.Click += new System.EventHandler(tsmiImageThumbnailer_Click);
		this.tssTrayTools2.Name = "tssTrayTools2";
		resources.ApplyResources(this.tssTrayTools2, "tssTrayTools2");
		this.tsmiTrayVideoConverter.Image = ShareX.Properties.Resources.camcorder_pencil;
		this.tsmiTrayVideoConverter.Name = "tsmiTrayVideoConverter";
		resources.ApplyResources(this.tsmiTrayVideoConverter, "tsmiTrayVideoConverter");
		this.tsmiTrayVideoConverter.Click += new System.EventHandler(tsmiVideoConverter_Click);
		this.tsmiTrayVideoThumbnailer.Image = ShareX.Properties.Resources.images_stack;
		this.tsmiTrayVideoThumbnailer.Name = "tsmiTrayVideoThumbnailer";
		resources.ApplyResources(this.tsmiTrayVideoThumbnailer, "tsmiTrayVideoThumbnailer");
		this.tsmiTrayVideoThumbnailer.Click += new System.EventHandler(tsmiVideoThumbnailer_Click);
		this.tssTrayTools3.Name = "tssTrayTools3";
		resources.ApplyResources(this.tssTrayTools3, "tssTrayTools3");
		this.tsmiTrayOCR.Image = ShareX.Properties.Resources.edit_drop_cap;
		this.tsmiTrayOCR.Name = "tsmiTrayOCR";
		resources.ApplyResources(this.tsmiTrayOCR, "tsmiTrayOCR");
		this.tsmiTrayOCR.Click += new System.EventHandler(tsmiTrayOCR_Click);
		this.tsmiTrayQRCode.Image = ShareX.Properties.Resources.barcode_2d;
		this.tsmiTrayQRCode.Name = "tsmiTrayQRCode";
		resources.ApplyResources(this.tsmiTrayQRCode, "tsmiTrayQRCode");
		this.tsmiTrayQRCode.Click += new System.EventHandler(tsmiQRCode_Click);
		this.tsmiTrayHashCheck.Image = ShareX.Properties.Resources.application_task;
		this.tsmiTrayHashCheck.Name = "tsmiTrayHashCheck";
		resources.ApplyResources(this.tsmiTrayHashCheck, "tsmiTrayHashCheck");
		this.tsmiTrayHashCheck.Click += new System.EventHandler(tsmiHashCheck_Click);
		this.tsmiTrayIndexFolder.Image = ShareX.Properties.Resources.folder_tree;
		this.tsmiTrayIndexFolder.Name = "tsmiTrayIndexFolder";
		resources.ApplyResources(this.tsmiTrayIndexFolder, "tsmiTrayIndexFolder");
		this.tsmiTrayIndexFolder.Click += new System.EventHandler(tsmiIndexFolder_Click);
		this.tssTrayTools4.Name = "tssTrayTools4";
		resources.ApplyResources(this.tssTrayTools4, "tssTrayTools4");
		this.tsmiTrayClipboardViewer.Image = ShareX.Properties.Resources.clipboard_block;
		this.tsmiTrayClipboardViewer.Name = "tsmiTrayClipboardViewer";
		resources.ApplyResources(this.tsmiTrayClipboardViewer, "tsmiTrayClipboardViewer");
		this.tsmiTrayClipboardViewer.Click += new System.EventHandler(tsmiClipboardViewer_Click);
		this.tsmiTrayBorderlessWindow.Image = ShareX.Properties.Resources.application_resize_full;
		this.tsmiTrayBorderlessWindow.Name = "tsmiTrayBorderlessWindow";
		resources.ApplyResources(this.tsmiTrayBorderlessWindow, "tsmiTrayBorderlessWindow");
		this.tsmiTrayBorderlessWindow.Click += new System.EventHandler(tsmiBorderlessWindow_Click);
		this.tsmiTrayInspectWindow.Image = ShareX.Properties.Resources.application_search_result;
		this.tsmiTrayInspectWindow.Name = "tsmiTrayInspectWindow";
		resources.ApplyResources(this.tsmiTrayInspectWindow, "tsmiTrayInspectWindow");
		this.tsmiTrayInspectWindow.Click += new System.EventHandler(tsmiInspectWindow_Click);
		this.tsmiTrayMonitorTest.Image = ShareX.Properties.Resources.monitor;
		this.tsmiTrayMonitorTest.Name = "tsmiTrayMonitorTest";
		resources.ApplyResources(this.tsmiTrayMonitorTest, "tsmiTrayMonitorTest");
		this.tsmiTrayMonitorTest.Click += new System.EventHandler(tsmiMonitorTest_Click);
		this.tsmiTrayDNSChanger.Image = ShareX.Properties.Resources.network_ip;
		this.tsmiTrayDNSChanger.Name = "tsmiTrayDNSChanger";
		resources.ApplyResources(this.tsmiTrayDNSChanger, "tsmiTrayDNSChanger");
		this.tsmiTrayDNSChanger.Click += new System.EventHandler(tsmiDNSChanger_Click);
		this.tssTray1.Name = "tssTray1";
		resources.ApplyResources(this.tssTray1, "tssTray1");
		this.tsmiTrayAfterCaptureTasks.Image = ShareX.Properties.Resources.image_export;
		this.tsmiTrayAfterCaptureTasks.Name = "tsmiTrayAfterCaptureTasks";
		resources.ApplyResources(this.tsmiTrayAfterCaptureTasks, "tsmiTrayAfterCaptureTasks");
		this.tsmiTrayAfterUploadTasks.Image = ShareX.Properties.Resources.upload_cloud;
		this.tsmiTrayAfterUploadTasks.Name = "tsmiTrayAfterUploadTasks";
		resources.ApplyResources(this.tsmiTrayAfterUploadTasks, "tsmiTrayAfterUploadTasks");
		this.tsmiTrayDestinations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.tsmiTrayImageUploaders, this.tsmiTrayTextUploaders, this.tsmiTrayFileUploaders, this.tsmiTrayURLShorteners, this.tsmiTrayURLSharingServices, this.tssTrayDestinations1, this.tsmiTrayDestinationSettings, this.tsmiTrayCustomUploaderSettings });
		this.tsmiTrayDestinations.Image = ShareX.Properties.Resources.drive_globe;
		this.tsmiTrayDestinations.Name = "tsmiTrayDestinations";
		resources.ApplyResources(this.tsmiTrayDestinations, "tsmiTrayDestinations");
		this.tsmiTrayDestinations.DropDownOpened += new System.EventHandler(tsddbDestinations_DropDownOpened);
		this.tsmiTrayImageUploaders.Image = ShareX.Properties.Resources.image;
		this.tsmiTrayImageUploaders.Name = "tsmiTrayImageUploaders";
		resources.ApplyResources(this.tsmiTrayImageUploaders, "tsmiTrayImageUploaders");
		this.tsmiTrayTextUploaders.Image = ShareX.Properties.Resources.notebook;
		this.tsmiTrayTextUploaders.Name = "tsmiTrayTextUploaders";
		resources.ApplyResources(this.tsmiTrayTextUploaders, "tsmiTrayTextUploaders");
		this.tsmiTrayFileUploaders.Image = ShareX.Properties.Resources.application_block;
		this.tsmiTrayFileUploaders.Name = "tsmiTrayFileUploaders";
		resources.ApplyResources(this.tsmiTrayFileUploaders, "tsmiTrayFileUploaders");
		this.tsmiTrayURLShorteners.Image = ShareX.Properties.Resources.edit_scale;
		this.tsmiTrayURLShorteners.Name = "tsmiTrayURLShorteners";
		resources.ApplyResources(this.tsmiTrayURLShorteners, "tsmiTrayURLShorteners");
		this.tsmiTrayURLSharingServices.Image = ShareX.Properties.Resources.globe_share;
		this.tsmiTrayURLSharingServices.Name = "tsmiTrayURLSharingServices";
		resources.ApplyResources(this.tsmiTrayURLSharingServices, "tsmiTrayURLSharingServices");
		this.tssTrayDestinations1.Name = "tssTrayDestinations1";
		resources.ApplyResources(this.tssTrayDestinations1, "tssTrayDestinations1");
		this.tsmiTrayDestinationSettings.Image = ShareX.Properties.Resources.globe_pencil;
		this.tsmiTrayDestinationSettings.Name = "tsmiTrayDestinationSettings";
		resources.ApplyResources(this.tsmiTrayDestinationSettings, "tsmiTrayDestinationSettings");
		this.tsmiTrayDestinationSettings.Click += new System.EventHandler(tsmiDestinationSettings_Click);
		this.tsmiTrayCustomUploaderSettings.Image = ShareX.Properties.Resources.network_cloud;
		this.tsmiTrayCustomUploaderSettings.Name = "tsmiTrayCustomUploaderSettings";
		resources.ApplyResources(this.tsmiTrayCustomUploaderSettings, "tsmiTrayCustomUploaderSettings");
		this.tsmiTrayCustomUploaderSettings.Click += new System.EventHandler(tsmiCustomUploaderSettings_Click);
		this.tsmiTrayApplicationSettings.Image = ShareX.Properties.Resources.wrench_screwdriver;
		this.tsmiTrayApplicationSettings.Name = "tsmiTrayApplicationSettings";
		resources.ApplyResources(this.tsmiTrayApplicationSettings, "tsmiTrayApplicationSettings");
		this.tsmiTrayApplicationSettings.Click += new System.EventHandler(tsbApplicationSettings_Click);
		this.tsmiTrayTaskSettings.Image = ShareX.Properties.Resources.gear;
		this.tsmiTrayTaskSettings.Name = "tsmiTrayTaskSettings";
		resources.ApplyResources(this.tsmiTrayTaskSettings, "tsmiTrayTaskSettings");
		this.tsmiTrayTaskSettings.Click += new System.EventHandler(tsbTaskSettings_Click);
		this.tsmiTrayHotkeySettings.Image = ShareX.Properties.Resources.keyboard;
		this.tsmiTrayHotkeySettings.Name = "tsmiTrayHotkeySettings";
		resources.ApplyResources(this.tsmiTrayHotkeySettings, "tsmiTrayHotkeySettings");
		this.tsmiTrayHotkeySettings.Click += new System.EventHandler(tsbHotkeySettings_Click);
		this.tsmiTrayToggleHotkeys.Image = ShareX.Properties.Resources.keyboard__minus;
		this.tsmiTrayToggleHotkeys.Name = "tsmiTrayToggleHotkeys";
		resources.ApplyResources(this.tsmiTrayToggleHotkeys, "tsmiTrayToggleHotkeys");
		this.tsmiTrayToggleHotkeys.Click += new System.EventHandler(tsmiTrayToggleHotkeys_Click);
		this.tssTray2.Name = "tssTray2";
		resources.ApplyResources(this.tssTray2, "tssTray2");
		this.tsmiScreenshotsFolder.Image = ShareX.Properties.Resources.folder_open_image;
		this.tsmiScreenshotsFolder.Name = "tsmiScreenshotsFolder";
		resources.ApplyResources(this.tsmiScreenshotsFolder, "tsmiScreenshotsFolder");
		this.tsmiScreenshotsFolder.Click += new System.EventHandler(tsbScreenshotsFolder_Click);
		this.tsmiTrayHistory.Image = ShareX.Properties.Resources.application_blog;
		this.tsmiTrayHistory.Name = "tsmiTrayHistory";
		resources.ApplyResources(this.tsmiTrayHistory, "tsmiTrayHistory");
		this.tsmiTrayHistory.Click += new System.EventHandler(tsbHistory_Click);
		this.tsmiTrayImageHistory.Image = ShareX.Properties.Resources.application_icon_large;
		this.tsmiTrayImageHistory.Name = "tsmiTrayImageHistory";
		resources.ApplyResources(this.tsmiTrayImageHistory, "tsmiTrayImageHistory");
		this.tsmiTrayImageHistory.Click += new System.EventHandler(tsbImageHistory_Click);
		this.tssTray3.Name = "tssTray3";
		resources.ApplyResources(this.tssTray3, "tssTray3");
		this.tsmiRestartAsAdmin.Image = ShareX.Properties.Resources.uac;
		this.tsmiRestartAsAdmin.Name = "tsmiRestartAsAdmin";
		resources.ApplyResources(this.tsmiRestartAsAdmin, "tsmiRestartAsAdmin");
		this.tsmiRestartAsAdmin.Click += new System.EventHandler(tsmiRestartAsAdmin_Click);
		this.tsmiTrayRecentItems.Image = ShareX.Properties.Resources.clipboard_list;
		this.tsmiTrayRecentItems.Name = "tsmiTrayRecentItems";
		resources.ApplyResources(this.tsmiTrayRecentItems, "tsmiTrayRecentItems");
		this.tsmiOpenActionsToolbar.Image = ShareX.Properties.Resources.ui_toolbar__arrow;
		this.tsmiOpenActionsToolbar.Name = "tsmiOpenActionsToolbar";
		resources.ApplyResources(this.tsmiOpenActionsToolbar, "tsmiOpenActionsToolbar");
		this.tsmiOpenActionsToolbar.Click += new System.EventHandler(tsmiOpenActionsToolbar_Click);
		this.tsmiTrayShow.Image = ShareX.Properties.Resources.tick_button;
		this.tsmiTrayShow.Name = "tsmiTrayShow";
		resources.ApplyResources(this.tsmiTrayShow, "tsmiTrayShow");
		this.tsmiTrayShow.Click += new System.EventHandler(tsmiTrayShow_Click);
		this.tsmiTrayExit.Image = ShareX.Properties.Resources.cross_button;
		this.tsmiTrayExit.Name = "tsmiTrayExit";
		resources.ApplyResources(this.tsmiTrayExit, "tsmiTrayExit");
		this.tsmiTrayExit.Click += new System.EventHandler(tsmiTrayExit_Click);
		this.tsmiTrayExit.MouseDown += new System.Windows.Forms.MouseEventHandler(tsmiTrayExit_MouseDown);
		this.timerTraySingleClick.Tick += new System.EventHandler(timerTraySingleClick_Tick);
		this.pThumbnailView.BackColor = System.Drawing.Color.FromArgb(42, 47, 56);
		this.pThumbnailView.Controls.Add(this.lblThumbnailViewTip);
		this.pThumbnailView.Controls.Add(this.ucTaskThumbnailView);
		resources.ApplyResources(this.pThumbnailView, "pThumbnailView");
		this.pThumbnailView.Name = "pThumbnailView";
		resources.ApplyResources(this.lblThumbnailViewTip, "lblThumbnailViewTip");
		this.lblThumbnailViewTip.BackColor = System.Drawing.Color.Transparent;
		this.lblThumbnailViewTip.ForeColor = System.Drawing.Color.FromArgb(228, 228, 228);
		this.lblThumbnailViewTip.Name = "lblThumbnailViewTip";
		this.lblThumbnailViewTip.UseMnemonic = false;
		this.lblThumbnailViewTip.MouseUp += new System.Windows.Forms.MouseEventHandler(LblThumbnailViewTip_MouseUp);
		resources.ApplyResources(this.ucTaskThumbnailView, "ucTaskThumbnailView");
		this.ucTaskThumbnailView.BackColor = System.Drawing.Color.FromArgb(42, 47, 56);
		this.ucTaskThumbnailView.ClickAction = ShareX.ThumbnailViewClickAction.Default;
		this.ucTaskThumbnailView.Name = "ucTaskThumbnailView";
		this.ucTaskThumbnailView.ThumbnailSize = new System.Drawing.Size(200, 150);
		this.ucTaskThumbnailView.TitleLocation = ShareX.ThumbnailTitleLocation.Top;
		this.ucTaskThumbnailView.TitleVisible = true;
		this.ucTaskThumbnailView.ContextMenuRequested += new ShareX.TaskThumbnailView.TaskViewMouseEventHandler(UcTaskView_ContextMenuRequested);
		this.ucTaskThumbnailView.SelectedPanelChanged += new System.EventHandler(ucTaskThumbnailView_SelectedPanelChanged);
		this.ucTaskThumbnailView.KeyDown += new System.Windows.Forms.KeyEventHandler(lvUploads_KeyDown);
		this.ttMain.AutoPopDelay = 5000;
		this.ttMain.InitialDelay = 200;
		this.ttMain.OwnerDraw = true;
		this.ttMain.ReshowDelay = 100;
		this.ttMain.Draw += new System.Windows.Forms.DrawToolTipEventHandler(TtMain_Draw);
		resources.ApplyResources(this.pToolbars, "pToolbars");
		this.pToolbars.Controls.Add(this.tsMain);
		this.pToolbars.Name = "pToolbars";
		this.AllowDrop = true;
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.pThumbnailView);
		base.Controls.Add(this.scMain);
		base.Controls.Add(this.pToolbars);
		this.DoubleBuffered = true;
		base.Name = "MainForm";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MainForm_FormClosed);
		base.Shown += new System.EventHandler(MainForm_Shown);
		base.LocationChanged += new System.EventHandler(MainForm_LocationChanged);
		base.SizeChanged += new System.EventHandler(MainForm_SizeChanged);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(MainForm_DragDrop);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(MainForm_DragEnter);
		base.Resize += new System.EventHandler(MainForm_Resize);
		this.scMain.Panel1.ResumeLayout(false);
		this.scMain.Panel1.PerformLayout();
		this.scMain.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.scMain).EndInit();
		this.scMain.ResumeLayout(false);
		this.tsMain.ResumeLayout(false);
		this.tsMain.PerformLayout();
		this.cmsTaskInfo.ResumeLayout(false);
		this.cmsTray.ResumeLayout(false);
		this.pThumbnailView.ResumeLayout(false);
		this.pThumbnailView.PerformLayout();
		this.pToolbars.ResumeLayout(false);
		this.pToolbars.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
