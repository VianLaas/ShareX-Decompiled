using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public static class IntegrationHelpers
{
	private static readonly string ApplicationPath = "\"" + Application.ExecutablePath + "\"";

	private static readonly string ShellExtMenuName = "ShareX";

	private static readonly string ShellExtMenuFiles = "Software\\Classes\\*\\shell\\" + ShellExtMenuName;

	private static readonly string ShellExtMenuFilesCmd = ShellExtMenuFiles + "\\command";

	private static readonly string ShellExtMenuDirectory = "Software\\Classes\\Directory\\shell\\" + ShellExtMenuName;

	private static readonly string ShellExtMenuDirectoryCmd = ShellExtMenuDirectory + "\\command";

	private static readonly string ShellExtDesc = Resources.IntegrationHelpers_UploadWithShareX;

	private static readonly string ShellExtIcon = ApplicationPath + ",0";

	private static readonly string ShellExtPath = ApplicationPath + " \"%1\"";

	private static readonly string ShellExtEditName = "ShareXImageEditor";

	private static readonly string ShellExtEditImage = "Software\\Classes\\SystemFileAssociations\\image\\shell\\" + ShellExtEditName;

	private static readonly string ShellExtEditImageCmd = ShellExtEditImage + "\\command";

	private static readonly string ShellExtEditDesc = Resources.IntegrationHelpers_EditWithShareX;

	private static readonly string ShellExtEditIcon = ApplicationPath + ",0";

	private static readonly string ShellExtEditPath = ApplicationPath + " -ImageEditor \"%1\"";

	private static readonly string ShellCustomUploaderExtensionPath = "Software\\Classes\\.sxcu";

	private static readonly string ShellCustomUploaderExtensionValue = "ShareX.sxcu";

	private static readonly string ShellCustomUploaderAssociatePath = "Software\\Classes\\" + ShellCustomUploaderExtensionValue;

	private static readonly string ShellCustomUploaderAssociateValue = "ShareX custom uploader";

	private static readonly string ShellCustomUploaderIconPath = ShellCustomUploaderAssociatePath + "\\DefaultIcon";

	private static readonly string ShellCustomUploaderIconValue = ApplicationPath + ",0";

	private static readonly string ShellCustomUploaderCommandPath = ShellCustomUploaderAssociatePath + "\\shell\\open\\command";

	private static readonly string ShellCustomUploaderCommandValue = ApplicationPath + " -CustomUploader \"%1\"";

	private static readonly string ShellImageEffectExtensionPath = "Software\\Classes\\.sxie";

	private static readonly string ShellImageEffectExtensionValue = "ShareX.sxie";

	private static readonly string ShellImageEffectAssociatePath = "Software\\Classes\\" + ShellImageEffectExtensionValue;

	private static readonly string ShellImageEffectAssociateValue = "ShareX image effect";

	private static readonly string ShellImageEffectIconPath = ShellImageEffectAssociatePath + "\\DefaultIcon";

	private static readonly string ShellImageEffectIconValue = ApplicationPath + ",0";

	private static readonly string ShellImageEffectCommandPath = ShellImageEffectAssociatePath + "\\shell\\open\\command";

	private static readonly string ShellImageEffectCommandValue = ApplicationPath + " -ImageEffect \"%1\"";

	private static readonly string ChromeNativeMessagingHosts = "SOFTWARE\\Google\\Chrome\\NativeMessagingHosts\\com.getsharex.sharex";

	private static readonly string FirefoxNativeMessagingHosts = "SOFTWARE\\Mozilla\\NativeMessagingHosts\\ShareX";

	public static bool CheckShellContextMenuButton()
	{
		try
		{
			return RegistryHelpers.CheckStringValue(ShellExtMenuFilesCmd, null, ShellExtPath) && RegistryHelpers.CheckStringValue(ShellExtMenuDirectoryCmd, null, ShellExtPath);
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		return false;
	}

	public static void CreateShellContextMenuButton(bool create)
	{
		try
		{
			if (create)
			{
				UnregisterShellContextMenuButton();
				RegisterShellContextMenuButton();
			}
			else
			{
				UnregisterShellContextMenuButton();
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	private static void RegisterShellContextMenuButton()
	{
		RegistryHelpers.CreateRegistry(ShellExtMenuFiles, ShellExtDesc);
		RegistryHelpers.CreateRegistry(ShellExtMenuFiles, "Icon", ShellExtIcon);
		RegistryHelpers.CreateRegistry(ShellExtMenuFilesCmd, ShellExtPath);
		RegistryHelpers.CreateRegistry(ShellExtMenuDirectory, ShellExtDesc);
		RegistryHelpers.CreateRegistry(ShellExtMenuDirectory, "Icon", ShellExtIcon);
		RegistryHelpers.CreateRegistry(ShellExtMenuDirectoryCmd, ShellExtPath);
	}

	private static void UnregisterShellContextMenuButton()
	{
		RegistryHelpers.RemoveRegistry(ShellExtMenuFiles, recursive: true);
		RegistryHelpers.RemoveRegistry(ShellExtMenuDirectory, recursive: true);
	}

	public static bool CheckEditShellContextMenuButton()
	{
		try
		{
			return RegistryHelpers.CheckStringValue(ShellExtEditImageCmd, null, ShellExtEditPath);
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		return false;
	}

	public static void CreateEditShellContextMenuButton(bool create)
	{
		try
		{
			if (create)
			{
				UnregisterEditShellContextMenuButton();
				RegisterEditShellContextMenuButton();
			}
			else
			{
				UnregisterEditShellContextMenuButton();
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	private static void RegisterEditShellContextMenuButton()
	{
		RegistryHelpers.CreateRegistry(ShellExtEditImage, ShellExtEditDesc);
		RegistryHelpers.CreateRegistry(ShellExtEditImage, "Icon", ShellExtEditIcon);
		RegistryHelpers.CreateRegistry(ShellExtEditImageCmd, ShellExtEditPath);
	}

	private static void UnregisterEditShellContextMenuButton()
	{
		RegistryHelpers.RemoveRegistry(ShellExtEditImage, recursive: true);
	}

	public static bool CheckCustomUploaderExtension()
	{
		try
		{
			return RegistryHelpers.CheckStringValue(ShellCustomUploaderExtensionPath, null, ShellCustomUploaderExtensionValue) && RegistryHelpers.CheckStringValue(ShellCustomUploaderCommandPath, null, ShellCustomUploaderCommandValue);
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		return false;
	}

	public static void CreateCustomUploaderExtension(bool create)
	{
		try
		{
			if (create)
			{
				UnregisterCustomUploaderExtension();
				RegisterCustomUploaderExtension();
			}
			else
			{
				UnregisterCustomUploaderExtension();
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	private static void RegisterCustomUploaderExtension()
	{
		RegistryHelpers.CreateRegistry(ShellCustomUploaderExtensionPath, ShellCustomUploaderExtensionValue);
		RegistryHelpers.CreateRegistry(ShellCustomUploaderAssociatePath, ShellCustomUploaderAssociateValue);
		RegistryHelpers.CreateRegistry(ShellCustomUploaderIconPath, ShellCustomUploaderIconValue);
		RegistryHelpers.CreateRegistry(ShellCustomUploaderCommandPath, ShellCustomUploaderCommandValue);
		NativeMethods.SHChangeNotify(HChangeNotifyEventID.SHCNE_ASSOCCHANGED, HChangeNotifyFlags.SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
	}

	private static void UnregisterCustomUploaderExtension()
	{
		RegistryHelpers.RemoveRegistry(ShellCustomUploaderExtensionPath);
		RegistryHelpers.RemoveRegistry(ShellCustomUploaderAssociatePath, recursive: true);
	}

	public static bool CheckImageEffectExtension()
	{
		try
		{
			return RegistryHelpers.CheckStringValue(ShellImageEffectExtensionPath, null, ShellImageEffectExtensionValue) && RegistryHelpers.CheckStringValue(ShellImageEffectCommandPath, null, ShellImageEffectCommandValue);
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		return false;
	}

	public static void CreateImageEffectExtension(bool create)
	{
		try
		{
			if (create)
			{
				UnregisterImageEffectExtension();
				RegisterImageEffectExtension();
			}
			else
			{
				UnregisterImageEffectExtension();
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	private static void RegisterImageEffectExtension()
	{
		RegistryHelpers.CreateRegistry(ShellImageEffectExtensionPath, ShellImageEffectExtensionValue);
		RegistryHelpers.CreateRegistry(ShellImageEffectAssociatePath, ShellImageEffectAssociateValue);
		RegistryHelpers.CreateRegistry(ShellImageEffectIconPath, ShellImageEffectIconValue);
		RegistryHelpers.CreateRegistry(ShellImageEffectCommandPath, ShellImageEffectCommandValue);
		NativeMethods.SHChangeNotify(HChangeNotifyEventID.SHCNE_ASSOCCHANGED, HChangeNotifyFlags.SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
	}

	private static void UnregisterImageEffectExtension()
	{
		RegistryHelpers.RemoveRegistry(ShellImageEffectExtensionPath);
		RegistryHelpers.RemoveRegistry(ShellImageEffectAssociatePath, recursive: true);
	}

	public static bool CheckChromeExtensionSupport()
	{
		try
		{
			return RegistryHelpers.CheckStringValue(ChromeNativeMessagingHosts, null, Program.ChromeHostManifestFilePath) && File.Exists(Program.ChromeHostManifestFilePath);
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		return false;
	}

	public static void CreateChromeExtensionSupport(bool create)
	{
		try
		{
			if (create)
			{
				UnregisterChromeExtensionSupport();
				RegisterChromeExtensionSupport();
			}
			else
			{
				UnregisterChromeExtensionSupport();
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	private static void CreateChromeHostManifest(string filePath)
	{
		FileHelpers.CreateDirectoryFromFilePath(filePath);
		ChromeManifest chromeManifest = new ChromeManifest();
		chromeManifest.name = "com.getsharex.sharex";
		chromeManifest.description = "ShareX";
		chromeManifest.path = Program.NativeMessagingHostFilePath;
		chromeManifest.type = "stdio";
		chromeManifest.allowed_origins = new string[1] { "chrome-extension://nlkoigbdolhchiicbonbihbphgamnaoc/" };
		string contents = JsonConvert.SerializeObject(chromeManifest, Formatting.Indented);
		File.WriteAllText(filePath, contents, Encoding.UTF8);
	}

	private static void RegisterChromeExtensionSupport()
	{
		CreateChromeHostManifest(Program.ChromeHostManifestFilePath);
		RegistryHelpers.CreateRegistry(ChromeNativeMessagingHosts, Program.ChromeHostManifestFilePath);
	}

	private static void UnregisterChromeExtensionSupport()
	{
		if (File.Exists(Program.ChromeHostManifestFilePath))
		{
			File.Delete(Program.ChromeHostManifestFilePath);
		}
		RegistryHelpers.RemoveRegistry(ChromeNativeMessagingHosts);
	}

	public static bool CheckFirefoxAddonSupport()
	{
		try
		{
			return RegistryHelpers.CheckStringValue(FirefoxNativeMessagingHosts, null, Program.FirefoxHostManifestFilePath) && File.Exists(Program.FirefoxHostManifestFilePath);
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		return false;
	}

	public static void CreateFirefoxAddonSupport(bool create)
	{
		try
		{
			if (create)
			{
				UnregisterFirefoxAddonSupport();
				RegisterFirefoxAddonSupport();
			}
			else
			{
				UnregisterFirefoxAddonSupport();
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	private static void CreateFirefoxHostManifest(string filePath)
	{
		FileHelpers.CreateDirectoryFromFilePath(filePath);
		FirefoxManifest firefoxManifest = new FirefoxManifest();
		firefoxManifest.name = "ShareX";
		firefoxManifest.description = "ShareX";
		firefoxManifest.path = Program.NativeMessagingHostFilePath;
		firefoxManifest.type = "stdio";
		firefoxManifest.allowed_extensions = new string[1] { "firefox@getsharex.com" };
		string contents = JsonConvert.SerializeObject(firefoxManifest, Formatting.Indented);
		File.WriteAllText(filePath, contents, Encoding.UTF8);
	}

	private static void RegisterFirefoxAddonSupport()
	{
		CreateFirefoxHostManifest(Program.FirefoxHostManifestFilePath);
		RegistryHelpers.CreateRegistry(FirefoxNativeMessagingHosts, Program.FirefoxHostManifestFilePath);
	}

	private static void UnregisterFirefoxAddonSupport()
	{
		if (File.Exists(Program.FirefoxHostManifestFilePath))
		{
			File.Delete(Program.FirefoxHostManifestFilePath);
		}
		RegistryHelpers.RemoveRegistry(FirefoxNativeMessagingHosts);
	}

	public static bool CheckSendToMenuButton()
	{
		return ShortcutHelpers.CheckShortcut(Environment.SpecialFolder.SendTo, "ShareX", Application.ExecutablePath);
	}

	public static bool CreateSendToMenuButton(bool create)
	{
		return ShortcutHelpers.SetShortcut(create, Environment.SpecialFolder.SendTo, "ShareX", Application.ExecutablePath);
	}

	public static bool CheckSteamShowInApp()
	{
		return File.Exists(Program.SteamInAppFilePath);
	}

	public static void SteamShowInApp(bool showInApp)
	{
		string steamInAppFilePath = Program.SteamInAppFilePath;
		try
		{
			if (showInApp)
			{
				FileHelpers.CreateEmptyFile(steamInAppFilePath);
			}
			else if (File.Exists(steamInAppFilePath))
			{
				File.Delete(steamInAppFilePath);
			}
		}
		catch (Exception ex)
		{
			DebugHelper.WriteException(ex);
			ex.ShowError();
			return;
		}
		MessageBox.Show(Resources.ApplicationSettingsForm_cbSteamShowInApp_CheckedChanged_For_settings_to_take_effect_ShareX_needs_to_be_reopened_from_Steam_, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	public static void Uninstall()
	{
		StartupManagerSingletonProvider.CurrentStartupManager.State = StartupState.Disabled;
		CreateShellContextMenuButton(create: false);
		CreateEditShellContextMenuButton(create: false);
		CreateCustomUploaderExtension(create: false);
		CreateImageEffectExtension(create: false);
		CreateSendToMenuButton(create: false);
	}
}
