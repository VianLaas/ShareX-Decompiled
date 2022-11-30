using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.HistoryLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;
using ShareX.UploadersLib;
using ShareX.UploadersLib.FileUploaders;

namespace ShareX;

internal static class SettingManager
{
	private const string ApplicationConfigFileName = "ApplicationConfig.json";

	private const string UploadersConfigFileName = "UploadersConfig.json";

	private const string HotkeysConfigFileName = "HotkeysConfig.json";

	private static ManualResetEvent uploadersConfigResetEvent = new ManualResetEvent(initialState: false);

	private static ManualResetEvent hotkeysConfigResetEvent = new ManualResetEvent(initialState: false);

	private static string ApplicationConfigFilePath
	{
		get
		{
			if (Program.Sandbox)
			{
				return null;
			}
			return Path.Combine(Program.PersonalFolder, "ApplicationConfig.json");
		}
	}

	private static string UploadersConfigFilePath
	{
		get
		{
			if (Program.Sandbox)
			{
				return null;
			}
			string path = ((Settings == null || string.IsNullOrEmpty(Settings.CustomUploadersConfigPath)) ? Program.PersonalFolder : FileHelpers.ExpandFolderVariables(Settings.CustomUploadersConfigPath));
			return Path.Combine(path, "UploadersConfig.json");
		}
	}

	private static string HotkeysConfigFilePath
	{
		get
		{
			if (Program.Sandbox)
			{
				return null;
			}
			string path = ((Settings == null || string.IsNullOrEmpty(Settings.CustomHotkeysConfigPath)) ? Program.PersonalFolder : FileHelpers.ExpandFolderVariables(Settings.CustomHotkeysConfigPath));
			return Path.Combine(path, "HotkeysConfig.json");
		}
	}

	public static string BackupFolder => Path.Combine(Program.PersonalFolder, "Backup");

	private static ApplicationConfig Settings
	{
		get
		{
			return Program.Settings;
		}
		set
		{
			Program.Settings = value;
		}
	}

	private static TaskSettings DefaultTaskSettings
	{
		get
		{
			return Program.DefaultTaskSettings;
		}
		set
		{
			Program.DefaultTaskSettings = value;
		}
	}

	private static UploadersConfig UploadersConfig
	{
		get
		{
			return Program.UploadersConfig;
		}
		set
		{
			Program.UploadersConfig = value;
		}
	}

	private static HotkeysConfig HotkeysConfig
	{
		get
		{
			return Program.HotkeysConfig;
		}
		set
		{
			Program.HotkeysConfig = value;
		}
	}

	public static void LoadInitialSettings()
	{
		LoadApplicationConfig();
		Task.Run(delegate
		{
			LoadUploadersConfig();
			uploadersConfigResetEvent.Set();
			LoadHotkeysConfig();
			hotkeysConfigResetEvent.Set();
		});
	}

	public static void WaitUploadersConfig()
	{
		if (UploadersConfig == null)
		{
			uploadersConfigResetEvent.WaitOne();
		}
	}

	public static void WaitHotkeysConfig()
	{
		if (HotkeysConfig == null)
		{
			hotkeysConfigResetEvent.WaitOne();
		}
	}

	public static void LoadApplicationConfig(bool fallbackSupport = true)
	{
		Settings = SettingsBase<ApplicationConfig>.Load(ApplicationConfigFilePath, BackupFolder, fallbackSupport);
		Settings.CreateBackup = true;
		Settings.CreateWeeklyBackup = true;
		Settings.SettingsSaveFailed += Settings_SettingsSaveFailed;
		DefaultTaskSettings = Settings.DefaultTaskSettings;
		ApplicationConfigBackwardCompatibilityTasks();
		MigrateHistoryFile();
	}

	private static void Settings_SettingsSaveFailed(Exception e)
	{
		string text = ((!(e is UnauthorizedAccessException) && !(e is FileNotFoundException)) ? e.Message : Resources.YourAntiVirusSoftwareOrTheControlledFolderAccessFeatureInWindowsCouldBeBlockingShareX);
		TaskHelpers.ShowNotificationTip(text, "ShareX - " + Resources.FailedToSaveSettings, 5000);
	}

	public static void LoadUploadersConfig(bool fallbackSupport = true)
	{
		UploadersConfig = SettingsBase<UploadersConfig>.Load(UploadersConfigFilePath, BackupFolder, fallbackSupport);
		UploadersConfig.CreateBackup = true;
		UploadersConfig.CreateWeeklyBackup = true;
		UploadersConfig.SupportDPAPIEncryption = true;
		UploadersConfigBackwardCompatibilityTasks();
	}

	public static void LoadHotkeysConfig(bool fallbackSupport = true)
	{
		HotkeysConfig = SettingsBase<HotkeysConfig>.Load(HotkeysConfigFilePath, BackupFolder, fallbackSupport);
		HotkeysConfig.CreateBackup = true;
		HotkeysConfig.CreateWeeklyBackup = true;
		HotkeysConfigBackwardCompatibilityTasks();
	}

	public static void LoadAllSettings()
	{
		LoadApplicationConfig();
		LoadUploadersConfig();
		LoadHotkeysConfig();
	}

	private static void ApplicationConfigBackwardCompatibilityTasks()
	{
		if (Settings.IsUpgradeFrom("11.4.1"))
		{
			RegionCaptureOptions surfaceOptions = DefaultTaskSettings.CaptureSettings.SurfaceOptions;
			surfaceOptions.AnnotationOptions = new AnnotationOptions();
			surfaceOptions.LastRegionTool = ShapeType.RegionRectangle;
			surfaceOptions.LastAnnotationTool = ShapeType.DrawingRectangle;
		}
		if (Settings.IsUpgradeFrom("11.5.0") && File.Exists(Program.ChromeHostManifestFilePath))
		{
			IntegrationHelpers.CreateChromeExtensionSupport(create: true);
		}
		if (Settings.IsUpgradeFrom("13.0.2"))
		{
			Settings.UseCustomTheme = Settings.UseDarkTheme;
		}
		if (Settings.IsUpgradeFrom("13.3.1") && Settings.Themes != null)
		{
			Settings.Themes.Add(ShareXTheme.NordDarkTheme);
			Settings.Themes.Add(ShareXTheme.NordLightTheme);
			Settings.Themes.Add(ShareXTheme.DraculaTheme);
		}
		if (Settings.IsUpgradeFrom("13.4.0"))
		{
			DefaultTaskSettings.GeneralSettings.ShowToastNotificationAfterTaskCompleted = DefaultTaskSettings.GeneralSettings.PopUpNotification != PopUpNotificationType.None;
		}
	}

	private static void MigrateHistoryFile()
	{
		if (!File.Exists(Program.HistoryFilePathOld))
		{
			return;
		}
		if (!File.Exists(Program.HistoryFilePath))
		{
			DebugHelper.WriteLine("Migrating XML history file \"" + Program.HistoryFilePathOld + "\" to JSON history file \"" + Program.HistoryFilePath + "\"");
			List<HistoryItem> historyItems = new HistoryManagerXML(Program.HistoryFilePathOld).GetHistoryItems();
			if (historyItems.Count > 0)
			{
				new HistoryManagerJSON(Program.HistoryFilePath).AppendHistoryItems(historyItems);
			}
		}
		FileHelpers.MoveFile(Program.HistoryFilePathOld, BackupFolder);
	}

	private static void UploadersConfigBackwardCompatibilityTasks()
	{
		if (UploadersConfig.IsUpgradeFrom("11.6.0"))
		{
			if (UploadersConfig.DropboxURLType == DropboxURLType.Direct)
			{
				UploadersConfig.DropboxUseDirectLink = true;
			}
			if (!string.IsNullOrEmpty(UploadersConfig.AmazonS3Settings.Endpoint))
			{
				bool flag = false;
				foreach (AmazonS3Endpoint endpoint in AmazonS3.Endpoints)
				{
					if (endpoint.Region != null && endpoint.Region.Equals(UploadersConfig.AmazonS3Settings.Endpoint, StringComparison.InvariantCultureIgnoreCase))
					{
						UploadersConfig.AmazonS3Settings.Endpoint = endpoint.Endpoint;
						UploadersConfig.AmazonS3Settings.Region = endpoint.Region;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					UploadersConfig.AmazonS3Settings.Endpoint = "";
				}
			}
		}
		if (UploadersConfig.CustomUploadersList == null)
		{
			return;
		}
		foreach (CustomUploaderItem customUploaders in UploadersConfig.CustomUploadersList)
		{
			customUploaders.CheckBackwardCompatibility();
		}
	}

	private static void HotkeysConfigBackwardCompatibilityTasks()
	{
		if (!HotkeysConfig.IsUpgradeFrom("13.1.1"))
		{
			return;
		}
		foreach (TaskSettings item in HotkeysConfig.Hotkeys.Select((HotkeySettings x) => x.TaskSettings))
		{
			if (item != null && !string.IsNullOrEmpty(item.AdvancedSettings.CapturePath))
			{
				item.OverrideScreenshotsFolder = true;
				item.ScreenshotsFolder = item.AdvancedSettings.CapturePath;
				item.AdvancedSettings.CapturePath = "";
			}
		}
	}

	public static void SaveAllSettings()
	{
		if (Settings != null)
		{
			Settings.Save(ApplicationConfigFilePath);
		}
		if (UploadersConfig != null)
		{
			UploadersConfig.Save(UploadersConfigFilePath);
		}
		if (HotkeysConfig != null)
		{
			HotkeysConfig.Save(HotkeysConfigFilePath);
		}
	}

	public static void SaveApplicationConfigAsync()
	{
		if (Settings != null)
		{
			Settings.SaveAsync(ApplicationConfigFilePath);
		}
	}

	public static void SaveUploadersConfigAsync()
	{
		if (UploadersConfig != null)
		{
			UploadersConfig.SaveAsync(UploadersConfigFilePath);
		}
	}

	public static void SaveHotkeysConfigAsync()
	{
		if (HotkeysConfig != null)
		{
			HotkeysConfig.SaveAsync(HotkeysConfigFilePath);
		}
	}

	public static void SaveAllSettingsAsync()
	{
		SaveApplicationConfigAsync();
		SaveUploadersConfigAsync();
		SaveHotkeysConfigAsync();
	}

	public static void ResetSettings()
	{
		if (File.Exists(ApplicationConfigFilePath))
		{
			File.Delete(ApplicationConfigFilePath);
		}
		LoadApplicationConfig(fallbackSupport: false);
		if (File.Exists(UploadersConfigFilePath))
		{
			File.Delete(UploadersConfigFilePath);
		}
		LoadUploadersConfig(fallbackSupport: false);
		if (File.Exists(HotkeysConfigFilePath))
		{
			File.Delete(HotkeysConfigFilePath);
		}
		LoadHotkeysConfig(fallbackSupport: false);
	}

	public static bool Export(string archivePath, bool settings, bool history)
	{
		MemoryStream memoryStream = null;
		MemoryStream memoryStream2 = null;
		MemoryStream memoryStream3 = null;
		try
		{
			List<ZipEntryInfo> list = new List<ZipEntryInfo>();
			if (settings)
			{
				memoryStream = Settings.SaveToMemoryStream();
				list.Add(new ZipEntryInfo(memoryStream, "ApplicationConfig.json"));
				memoryStream2 = UploadersConfig.SaveToMemoryStream();
				list.Add(new ZipEntryInfo(memoryStream2, "UploadersConfig.json"));
				memoryStream3 = HotkeysConfig.SaveToMemoryStream();
				list.Add(new ZipEntryInfo(memoryStream3, "HotkeysConfig.json"));
			}
			if (history)
			{
				list.Add(new ZipEntryInfo(Program.HistoryFilePath));
			}
			ZipManager.Compress(archivePath, list);
			return true;
		}
		catch (Exception ex)
		{
			DebugHelper.WriteException(ex);
			MessageBox.Show("Error while exporting backup:\r\n" + ex, "ShareX - Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			memoryStream?.Dispose();
			memoryStream2?.Dispose();
			memoryStream3?.Dispose();
		}
		return false;
	}

	public static bool Import(string archivePath)
	{
		try
		{
			ZipManager.Extract(archivePath, Program.PersonalFolder, retainDirectoryStructure: true, (ZipArchiveEntry entry) => FileHelpers.CheckExtension(entry.Name, new string[2] { "json", "xml" }), 1000000000L);
			return true;
		}
		catch (Exception ex)
		{
			DebugHelper.WriteException(ex);
			MessageBox.Show("Error while importing backup:\r\n" + ex, "ShareX - Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return false;
	}
}
