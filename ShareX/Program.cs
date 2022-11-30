using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.UploadersLib;

namespace ShareX;

internal static class Program
{
	public const string Name = "ShareX";

	public const ShareXBuild Build = ShareXBuild.Release;

	private const string PersonalPathConfigFileName = "PersonalPath.cfg";

	public static readonly string DefaultPersonalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ShareX");

	public static readonly string PortablePersonalFolder = FileHelpers.GetAbsolutePath("ShareX");

	private static readonly string CurrentPersonalPathConfigFilePath = Path.Combine(DefaultPersonalFolder, "PersonalPath.cfg");

	private static readonly string PreviousPersonalPathConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ShareX", "PersonalPath.cfg");

	private static readonly string PortableCheckFilePath = FileHelpers.GetAbsolutePath("Portable");

	public static readonly string NativeMessagingHostFilePath = FileHelpers.GetAbsolutePath("ShareX_NativeMessagingHost.exe");

	public static readonly string SteamInAppFilePath = FileHelpers.GetAbsolutePath("Steam");

	public const string HistoryFileName = "History.json";

	public const string HistoryFileNameOld = "History.xml";

	public const string LogsFolderName = "Logs";

	private static string PersonalPathDetectionMethod;

	private static bool closeSequenceStarted;

	private static bool restartRequested;

	private static bool restartAsAdmin;

	public static string VersionText
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			Version version = Version.Parse(Application.ProductVersion);
			stringBuilder.Append(version.Major + "." + version.Minor);
			if (version.Build > 0 || version.Revision > 0)
			{
				stringBuilder.Append("." + version.Build);
			}
			if (version.Revision > 0)
			{
				stringBuilder.Append("." + version.Revision);
			}
			if (Dev)
			{
				stringBuilder.Append(" Dev");
			}
			if (Portable)
			{
				stringBuilder.Append(" Portable");
			}
			return stringBuilder.ToString();
		}
	}

	public static string Title
	{
		get
		{
			string text = "ShareX " + VersionText;
			if (Settings != null && Settings.DevMode)
			{
				string text2 = ShareXBuild.Release.ToString();
				if (IsAdmin)
				{
					text2 += ", Admin";
				}
				text = text + " (" + text2 + ")";
			}
			return text;
		}
	}

	public static string TitleShort
	{
		get
		{
			if (Settings != null && Settings.DevMode)
			{
				return Title;
			}
			return "ShareX";
		}
	}

	public static bool Dev { get; } = false;


	public static bool MultiInstance { get; private set; }

	public static bool Portable { get; private set; }

	public static bool SilentRun { get; private set; }

	public static bool Sandbox { get; private set; }

	public static bool IsAdmin { get; private set; }

	public static bool SteamFirstTimeConfig { get; private set; }

	public static bool IgnoreHotkeyWarning { get; private set; }

	public static bool PuushMode { get; private set; }

	internal static ApplicationConfig Settings { get; set; }

	internal static TaskSettings DefaultTaskSettings { get; set; }

	internal static UploadersConfig UploadersConfig { get; set; }

	internal static HotkeysConfig HotkeysConfig { get; set; }

	internal static MainForm MainForm { get; private set; }

	internal static Stopwatch StartTimer { get; private set; }

	internal static HotkeyManager HotkeyManager { get; set; }

	internal static WatchFolderManager WatchFolderManager { get; set; }

	internal static GitHubUpdateManager UpdateManager { get; private set; }

	internal static ShareXCLIManager CLI { get; private set; }

	private static string PersonalPathConfigFilePath
	{
		get
		{
			string absolutePath = FileHelpers.GetAbsolutePath("PersonalPath.cfg");
			if (File.Exists(absolutePath))
			{
				return absolutePath;
			}
			return CurrentPersonalPathConfigFilePath;
		}
	}

	private static string CustomPersonalPath { get; set; }

	public static string PersonalFolder
	{
		get
		{
			if (!string.IsNullOrEmpty(CustomPersonalPath))
			{
				return FileHelpers.ExpandFolderVariables(CustomPersonalPath);
			}
			return DefaultPersonalFolder;
		}
	}

	public static string HistoryFilePath
	{
		get
		{
			if (Sandbox)
			{
				return null;
			}
			return Path.Combine(PersonalFolder, "History.json");
		}
	}

	public static string HistoryFilePathOld
	{
		get
		{
			if (Sandbox)
			{
				return null;
			}
			return Path.Combine(PersonalFolder, "History.xml");
		}
	}

	public static string LogsFolder => Path.Combine(PersonalFolder, "Logs");

	public static string LogsFilePath
	{
		get
		{
			string path = $"ShareX-Log-{DateTime.Now:yyyy-MM}.txt";
			return Path.Combine(LogsFolder, path);
		}
	}

	public static string RequestLogsFilePath => Path.Combine(LogsFolder, "ShareX-Request-Logs.txt");

	public static string ScreenshotsParentFolder
	{
		get
		{
			if (Settings != null && Settings.UseCustomScreenshotsPath)
			{
				string customScreenshotsPath = Settings.CustomScreenshotsPath;
				string customScreenshotsPath2 = Settings.CustomScreenshotsPath2;
				if (!string.IsNullOrEmpty(customScreenshotsPath))
				{
					customScreenshotsPath = FileHelpers.ExpandFolderVariables(customScreenshotsPath);
					if (string.IsNullOrEmpty(customScreenshotsPath2) || Directory.Exists(customScreenshotsPath))
					{
						return customScreenshotsPath;
					}
				}
				if (!string.IsNullOrEmpty(customScreenshotsPath2))
				{
					customScreenshotsPath2 = FileHelpers.ExpandFolderVariables(customScreenshotsPath2);
					if (Directory.Exists(customScreenshotsPath2))
					{
						return customScreenshotsPath2;
					}
				}
			}
			return Path.Combine(PersonalFolder, "Screenshots");
		}
	}

	public static string ToolsFolder => Path.Combine(PersonalFolder, "Tools");

	public static string ImageEffectsFolder => Path.Combine(PersonalFolder, "ImageEffects");

	public static string ScreenRecorderCacheFilePath => Path.Combine(PersonalFolder, "ScreenRecorder.avi");

	public static string DefaultFFmpegFilePath => Path.Combine(ToolsFolder, "ffmpeg.exe");

	public static string ChromeHostManifestFilePath => Path.Combine(ToolsFolder, "Chrome-host-manifest.json");

	public static string FirefoxHostManifestFilePath => Path.Combine(ToolsFolder, "Firefox-host-manifest.json");

	[STAThread]
	private static void Main(string[] args)
	{
		Application.ThreadException += Application_ThreadException;
		Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		StartTimer = Stopwatch.StartNew();
		CLI = new ShareXCLIManager(args);
		CLI.ParseCommands();
		if (CheckAdminTasks())
		{
			return;
		}
		SystemOptions.UpdateSystemOptions();
		UpdatePersonalPath();
		DebugHelper.Init(LogsFilePath);
		MultiInstance = CLI.IsCommandExist("multi", "m");
		using (new ApplicationInstanceManager(!MultiInstance, args, SingleInstanceCallback))
		{
			using (new TimerResolutionManager())
			{
				Run();
			}
		}
		if (restartRequested)
		{
			DebugHelper.WriteLine("ShareX restarting.");
			if (restartAsAdmin)
			{
				TaskHelpers.RunShareXAsAdmin("-silent");
			}
			else
			{
				Process.Start(Application.ExecutablePath);
			}
		}
	}

	private static void Run()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		DebugHelper.WriteLine("ShareX starting.");
		DebugHelper.WriteLine("Version: " + VersionText);
		DebugHelper.WriteLine("Build: " + ShareXBuild.Release);
		DebugHelper.WriteLine("Command line: " + Environment.CommandLine);
		DebugHelper.WriteLine("Personal path: " + PersonalFolder);
		if (!string.IsNullOrEmpty(PersonalPathDetectionMethod))
		{
			DebugHelper.WriteLine("Personal path detection method: " + PersonalPathDetectionMethod);
		}
		DebugHelper.WriteLine("Operating system: " + Helpers.GetOperatingSystemProductName(includeBit: true));
		IsAdmin = Helpers.IsAdministrator();
		DebugHelper.WriteLine("Running as elevated process: " + IsAdmin);
		SilentRun = CLI.IsCommandExist("silent", "s");
		IgnoreHotkeyWarning = CLI.IsCommandExist("NoHotkeys");
		CreateParentFolders();
		RegisterExtensions();
		CheckPuushMode();
		DebugWriteFlags();
		SettingManager.LoadInitialSettings();
		Uploader.UpdateServicePointManager();
		UpdateManager = new GitHubUpdateManager("ShareX", "ShareX", Dev, Portable);
		LanguageHelper.ChangeLanguage(Settings.Language);
		CleanupManager.CleanupAsync();
		Helpers.TryFixHandCursor();
		DebugHelper.WriteLine("MainForm init started.");
		MainForm = new MainForm();
		DebugHelper.WriteLine("MainForm init finished.");
		Application.Run(MainForm);
		CloseSequence();
	}

	public static void CloseSequence()
	{
		if (!closeSequenceStarted)
		{
			closeSequenceStarted = true;
			DebugHelper.Logger.AsyncWrite = false;
			DebugHelper.WriteLine("ShareX closing.");
			if (WatchFolderManager != null)
			{
				WatchFolderManager.Dispose();
			}
			SettingManager.SaveAllSettings();
			DebugHelper.WriteLine("ShareX closed.");
		}
	}

	public static void Restart(bool asAdmin = false)
	{
		restartRequested = true;
		restartAsAdmin = asAdmin;
		Application.Exit();
	}

	private static void SingleInstanceCallback(object sender, InstanceCallbackEventArgs args)
	{
		if (WaitFormLoad(5000))
		{
			MainForm.InvokeSafe(async delegate
			{
				await UseCommandLineArgs(args.CommandLineArgs);
			});
		}
	}

	private static bool WaitFormLoad(int wait)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		while (stopwatch.ElapsedMilliseconds < wait)
		{
			if (MainForm != null && MainForm.IsReady)
			{
				return true;
			}
			Thread.Sleep(10);
		}
		return false;
	}

	private static async Task UseCommandLineArgs(string[] args)
	{
		if (args == null || args.Length < 1)
		{
			if (MainForm.niTray != null && MainForm.niTray.Visible)
			{
				MainForm.niTray.Visible = false;
				MainForm.niTray.Visible = true;
			}
			MainForm.ForceActivate();
		}
		else if (MainForm.Visible)
		{
			MainForm.ForceActivate();
		}
		CLIManager cLIManager = new CLIManager(args);
		cLIManager.ParseCommands();
		await CLI.UseCommandLineArgs(cLIManager.Commands);
	}

	private static void UpdatePersonalPath()
	{
		Sandbox = CLI.IsCommandExist("sandbox");
		if (Sandbox)
		{
			return;
		}
		if (CLI.IsCommandExist("portable", "p"))
		{
			Portable = true;
			CustomPersonalPath = PortablePersonalFolder;
			PersonalPathDetectionMethod = "Portable CLI flag";
		}
		else if (File.Exists(PortableCheckFilePath))
		{
			Portable = true;
			CustomPersonalPath = PortablePersonalFolder;
			PersonalPathDetectionMethod = "Portable file (" + PortableCheckFilePath + ")";
		}
		else if (!string.IsNullOrEmpty(SystemOptions.PersonalPath))
		{
			CustomPersonalPath = SystemOptions.PersonalPath;
			PersonalPathDetectionMethod = "Registry";
		}
		else
		{
			MigratePersonalPathConfig();
			string text = ReadPersonalPathConfig();
			if (!string.IsNullOrEmpty(text))
			{
				CustomPersonalPath = FileHelpers.GetAbsolutePath(text);
				PersonalPathDetectionMethod = "PersonalPath.cfg file (" + PersonalPathConfigFilePath + ")";
			}
		}
		if (Directory.Exists(PersonalFolder))
		{
			return;
		}
		try
		{
			Directory.CreateDirectory(PersonalFolder);
		}
		catch (Exception value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{0} \"{1}\"", Resources.Program_Run_Unable_to_create_folder_, PersonalFolder);
			stringBuilder.AppendLine();
			if (!string.IsNullOrEmpty(PersonalPathDetectionMethod))
			{
				stringBuilder.AppendLine("Personal path detection method: " + PersonalPathDetectionMethod);
			}
			stringBuilder.AppendLine();
			stringBuilder.Append(value);
			MessageBox.Show(stringBuilder.ToString(), "ShareX - " + Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			CustomPersonalPath = "";
		}
	}

	private static void CreateParentFolders()
	{
		if (!Sandbox && Directory.Exists(PersonalFolder))
		{
			FileHelpers.CreateDirectory(SettingManager.BackupFolder);
			FileHelpers.CreateDirectory(ImageEffectsFolder);
			FileHelpers.CreateDirectory(LogsFolder);
			FileHelpers.CreateDirectory(ScreenshotsParentFolder);
			FileHelpers.CreateDirectory(ToolsFolder);
		}
	}

	private static void RegisterExtensions()
	{
		if (!Portable)
		{
			if (!IntegrationHelpers.CheckCustomUploaderExtension())
			{
				IntegrationHelpers.CreateCustomUploaderExtension(create: true);
			}
			if (!IntegrationHelpers.CheckImageEffectExtension())
			{
				IntegrationHelpers.CreateImageEffectExtension(create: true);
			}
		}
	}

	public static void UpdateHelpersSpecialFolders()
	{
		HelpersOptions.ShareXSpecialFolders = new Dictionary<string, string> { { "ShareXImageEffects", ImageEffectsFolder } };
	}

	private static void MigratePersonalPathConfig()
	{
		if (!File.Exists(PreviousPersonalPathConfigFilePath))
		{
			return;
		}
		try
		{
			if (!File.Exists(CurrentPersonalPathConfigFilePath))
			{
				FileHelpers.CreateDirectoryFromFilePath(CurrentPersonalPathConfigFilePath);
				File.Move(PreviousPersonalPathConfigFilePath, CurrentPersonalPathConfigFilePath);
			}
			File.Delete(PreviousPersonalPathConfigFilePath);
			Directory.Delete(Path.GetDirectoryName(PreviousPersonalPathConfigFilePath));
		}
		catch (Exception e)
		{
			e.ShowError();
		}
	}

	public static string ReadPersonalPathConfig()
	{
		if (File.Exists(PersonalPathConfigFilePath))
		{
			return File.ReadAllText(PersonalPathConfigFilePath, Encoding.UTF8).Trim();
		}
		return "";
	}

	public static bool WritePersonalPathConfig(string path)
	{
		path = ((path != null) ? path.Trim() : "");
		if (!string.IsNullOrEmpty(path) || File.Exists(PersonalPathConfigFilePath))
		{
			string value = ReadPersonalPathConfig();
			if (!path.Equals(value, StringComparison.InvariantCultureIgnoreCase))
			{
				try
				{
					FileHelpers.CreateDirectoryFromFilePath(PersonalPathConfigFilePath);
					File.WriteAllText(PersonalPathConfigFilePath, path, Encoding.UTF8);
					return true;
				}
				catch (UnauthorizedAccessException exception)
				{
					DebugHelper.WriteException(exception);
					MessageBox.Show(string.Format(Resources.Program_WritePersonalPathConfig_Cant_access_to_file, PersonalPathConfigFilePath), "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				catch (Exception ex)
				{
					DebugHelper.WriteException(ex);
					ex.ShowError();
				}
			}
		}
		return false;
	}

	private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
	{
		OnError(e.Exception);
	}

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		OnError((Exception)e.ExceptionObject);
	}

	private static void OnError(Exception e)
	{
		using ErrorForm errorForm = new ErrorForm(e.Message, $"{e}\r\n\r\n{Title}", LogsFilePath, "https://github.com/ShareX/ShareX/issues?q=is%3Aissue");
		errorForm.ShowDialog();
	}

	private static bool CheckAdminTasks()
	{
		if (CLI.IsCommandExist("dnschanger"))
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Helpers.TryFixHandCursor();
			Application.Run(new DNSChangerForm());
			return true;
		}
		return false;
	}

	private static bool CheckUninstall()
	{
		if (CLI.IsCommandExist("uninstall"))
		{
			try
			{
				IntegrationHelpers.Uninstall();
			}
			catch
			{
			}
			return true;
		}
		return false;
	}

	private static bool CheckPuushMode()
	{
		PuushMode = File.Exists(FileHelpers.GetAbsolutePath("puush"));
		return PuushMode;
	}

	private static void DebugWriteFlags()
	{
		List<string> list = new List<string>();
		if (Dev)
		{
			list.Add("Dev");
		}
		if (MultiInstance)
		{
			list.Add("MultiInstance");
		}
		if (Portable)
		{
			list.Add("Portable");
		}
		if (SilentRun)
		{
			list.Add("SilentRun");
		}
		if (Sandbox)
		{
			list.Add("Sandbox");
		}
		if (SteamFirstTimeConfig)
		{
			list.Add("SteamFirstTimeConfig");
		}
		if (IgnoreHotkeyWarning)
		{
			list.Add("IgnoreHotkeyWarning");
		}
		if (SystemOptions.DisableUpdateCheck)
		{
			list.Add("DisableUpdateCheck");
		}
		if (SystemOptions.DisableUpload)
		{
			list.Add("DisableUpload");
		}
		if (PuushMode)
		{
			list.Add("PuushMode");
		}
		string text = string.Join(", ", list);
		if (!string.IsNullOrEmpty(text))
		{
			DebugHelper.WriteLine("Flags: " + text);
		}
	}
}
