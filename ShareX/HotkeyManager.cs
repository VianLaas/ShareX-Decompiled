using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class HotkeyManager
{
	public delegate void HotkeyTriggerEventHandler(HotkeySettings hotkeySetting);

	public delegate void HotkeysToggledEventHandler(bool hotkeysEnabled);

	public HotkeyTriggerEventHandler HotkeyTrigger;

	public HotkeysToggledEventHandler HotkeysToggledTrigger;

	private HotkeyForm hotkeyForm;

	public List<HotkeySettings> Hotkeys { get; private set; }

	public bool IgnoreHotkeys { get; set; }

	public HotkeyManager(HotkeyForm form)
	{
		hotkeyForm = form;
		hotkeyForm.HotkeyPress += hotkeyForm_HotkeyPress;
		hotkeyForm.FormClosed += delegate
		{
			hotkeyForm.InvokeSafe(delegate
			{
				UnregisterAllHotkeys(removeFromList: false);
			});
		};
	}

	public void UpdateHotkeys(List<HotkeySettings> hotkeys, bool showFailedHotkeys)
	{
		if (Hotkeys != null)
		{
			UnregisterAllHotkeys();
		}
		Hotkeys = hotkeys;
		RegisterAllHotkeys();
		if (showFailedHotkeys)
		{
			ShowFailedHotkeys();
		}
	}

	private void hotkeyForm_HotkeyPress(ushort id, Keys key, Modifiers modifier)
	{
		if (!IgnoreHotkeys && (!Program.Settings.DisableHotkeysOnFullscreen || !CaptureHelpers.IsActiveWindowFullscreen()))
		{
			HotkeySettings hotkeySettings = Hotkeys.Find((HotkeySettings x) => x.HotkeyInfo.ID == id);
			if (hotkeySettings != null)
			{
				OnHotkeyTrigger(hotkeySettings);
			}
		}
	}

	protected void OnHotkeyTrigger(HotkeySettings hotkeySetting)
	{
		HotkeyTrigger?.Invoke(hotkeySetting);
	}

	public void RegisterHotkey(HotkeySettings hotkeySetting)
	{
		if (!Program.Settings.DisableHotkeys || hotkeySetting.TaskSettings.Job == HotkeyType.DisableHotkeys)
		{
			UnregisterHotkey(hotkeySetting, removeFromList: false);
			if (hotkeySetting.HotkeyInfo.Status != 0 && hotkeySetting.HotkeyInfo.IsValidHotkey)
			{
				hotkeyForm.RegisterHotkey(hotkeySetting.HotkeyInfo);
				if (hotkeySetting.HotkeyInfo.Status == HotkeyStatus.Registered)
				{
					DebugHelper.WriteLine("Hotkey registered: " + hotkeySetting);
				}
				else if (hotkeySetting.HotkeyInfo.Status == HotkeyStatus.Failed)
				{
					DebugHelper.WriteLine("Hotkey register failed: " + hotkeySetting);
				}
			}
		}
		if (!Hotkeys.Contains(hotkeySetting))
		{
			Hotkeys.Add(hotkeySetting);
		}
	}

	public void RegisterAllHotkeys()
	{
		HotkeySettings[] array = Hotkeys.ToArray();
		foreach (HotkeySettings hotkeySetting in array)
		{
			RegisterHotkey(hotkeySetting);
		}
	}

	public void UnregisterHotkey(HotkeySettings hotkeySetting, bool removeFromList = true)
	{
		if (hotkeySetting.HotkeyInfo.Status == HotkeyStatus.Registered)
		{
			hotkeyForm.UnregisterHotkey(hotkeySetting.HotkeyInfo);
			if (hotkeySetting.HotkeyInfo.Status == HotkeyStatus.NotConfigured)
			{
				DebugHelper.WriteLine("Hotkey unregistered: " + hotkeySetting);
			}
			else if (hotkeySetting.HotkeyInfo.Status == HotkeyStatus.Failed)
			{
				DebugHelper.WriteLine("Hotkey unregister failed: " + hotkeySetting);
			}
		}
		if (removeFromList)
		{
			Hotkeys.Remove(hotkeySetting);
		}
	}

	public void UnregisterAllHotkeys(bool removeFromList = true, bool temporary = false)
	{
		if (Hotkeys == null)
		{
			return;
		}
		HotkeySettings[] array = Hotkeys.ToArray();
		foreach (HotkeySettings hotkeySettings in array)
		{
			if (!temporary || hotkeySettings.TaskSettings.Job != HotkeyType.DisableHotkeys)
			{
				UnregisterHotkey(hotkeySettings, removeFromList);
			}
		}
	}

	public void ToggleHotkeys(bool hotkeysDisabled)
	{
		if (!hotkeysDisabled)
		{
			RegisterAllHotkeys();
		}
		else
		{
			UnregisterAllHotkeys(removeFromList: false, temporary: true);
		}
		HotkeysToggledTrigger?.Invoke(hotkeysDisabled);
	}

	public void ShowFailedHotkeys()
	{
		List<HotkeySettings> list = Hotkeys.Where((HotkeySettings x) => x.HotkeyInfo.Status == HotkeyStatus.Failed).ToList();
		if (list.Count <= 0)
		{
			return;
		}
		string arg = string.Join("\r\n", list.Select((HotkeySettings x) => x.TaskSettings.ToString() + ": " + x.HotkeyInfo.ToString()));
		string arg2 = ((list.Count > 1) ? Resources.HotkeyManager_ShowFailedHotkeys_hotkeys : Resources.HotkeyManager_ShowFailedHotkeys_hotkey);
		string text = string.Format(Resources.HotkeyManager_ShowFailedHotkeys_Unable_to_register_hotkey, arg2, arg);
		string[] processNames = new string[9] { "ShareX", "OneDrive", "Dropbox", "Greenshot", "ScreenshotCaptor", "FSCapture", "Snagit32", "puush", "Lightshot" };
		int ignoreProcess = Process.GetCurrentProcess().Id;
		List<string> list2 = (from x in Process.GetProcesses()
			where x.Id != ignoreProcess && !string.IsNullOrEmpty(x.ProcessName) && processNames.Any((string x2) => x.ProcessName.Equals(x2, StringComparison.InvariantCultureIgnoreCase))
			select $"{x.MainModule.FileVersionInfo.ProductName} ({x.MainModule.ModuleName})").Distinct().ToList();
		if (list2 != null && list2.Count > 0)
		{
			text = text + "\r\n\r\n" + Resources.HotkeyManager_ShowFailedHotkeys_These_applications_could_be_conflicting_ + "\r\n\r\n" + string.Join("\r\n", list2);
		}
		MessageBox.Show(text, "ShareX - " + Resources.HotkeyManager_ShowFailedHotkeys_Hotkey_registration_failed, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
	}

	public void ResetHotkeys()
	{
		UnregisterAllHotkeys();
		Hotkeys.AddRange(GetDefaultHotkeyList());
		RegisterAllHotkeys();
		if (Program.Settings.DisableHotkeys)
		{
			TaskHelpers.ToggleHotkeys();
		}
	}

	public static List<HotkeySettings> GetDefaultHotkeyList()
	{
		return new List<HotkeySettings>
		{
			new HotkeySettings(HotkeyType.RectangleRegion, Keys.Snapshot | Keys.Control),
			new HotkeySettings(HotkeyType.PrintScreen, Keys.Snapshot),
			new HotkeySettings(HotkeyType.ActiveWindow, Keys.Snapshot | Keys.Alt),
			new HotkeySettings(HotkeyType.ScreenRecorder, Keys.Snapshot | Keys.Shift),
			new HotkeySettings(HotkeyType.ScreenRecorderGIF, Keys.Snapshot | Keys.Shift | Keys.Control)
		};
	}
}
