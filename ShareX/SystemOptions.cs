using System;
using Microsoft.Win32;
using ShareX.HelpersLib;

namespace ShareX;

public static class SystemOptions
{
	private const string RegistryPath = "SOFTWARE\\ShareX";

	public static bool DisableUpdateCheck { get; private set; }

	public static bool DisableUpload { get; private set; }

	public static string PersonalPath { get; private set; }

	public static void UpdateSystemOptions()
	{
		DisableUpdateCheck = GetSystemOptionBoolean("DisableUpdateCheck");
		DisableUpload = GetSystemOptionBoolean("DisableUpload");
		PersonalPath = GetSystemOptionString("PersonalPath");
	}

	private static bool GetSystemOptionBoolean(string name)
	{
		object value = RegistryHelpers.GetValue("SOFTWARE\\ShareX", name, RegistryHive.LocalMachine);
		if (value != null)
		{
			try
			{
				return Convert.ToBoolean(value);
			}
			catch
			{
			}
		}
		value = RegistryHelpers.GetValue("SOFTWARE\\ShareX", name);
		if (value != null)
		{
			try
			{
				return Convert.ToBoolean(value);
			}
			catch
			{
			}
		}
		return false;
	}

	private static string GetSystemOptionString(string name)
	{
		string valueString = RegistryHelpers.GetValueString("SOFTWARE\\ShareX", name, RegistryHive.LocalMachine);
		if (valueString == null)
		{
			valueString = RegistryHelpers.GetValueString("SOFTWARE\\ShareX", name);
		}
		return valueString;
	}
}
