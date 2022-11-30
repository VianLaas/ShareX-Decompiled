using System;
using Microsoft.Win32;
using ShareX.HelpersLib;

namespace ShareX;

public abstract class GenericStartupManager : IStartupManager
{
	public abstract string StartupTargetPath { get; }

	public StartupState State
	{
		get
		{
			if (ShortcutHelpers.CheckShortcut(Environment.SpecialFolder.Startup, "ShareX", StartupTargetPath))
			{
				if (Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\StartupFolder", "ShareX.lnk", null) is byte[] array && array.Length != 0 && array[0] == 3)
				{
					return StartupState.DisabledByUser;
				}
				return StartupState.Enabled;
			}
			return StartupState.Disabled;
		}
		set
		{
			if (value == StartupState.Enabled || value == StartupState.Disabled)
			{
				ShortcutHelpers.SetShortcut(value == StartupState.Enabled, Environment.SpecialFolder.Startup, "ShareX", StartupTargetPath, "-silent");
				return;
			}
			throw new NotSupportedException();
		}
	}
}
