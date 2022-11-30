using System.Collections.Generic;
using ShareX.HelpersLib;

namespace ShareX;

public class HotkeysConfig : SettingsBase<HotkeysConfig>
{
	public List<HotkeySettings> Hotkeys = HotkeyManager.GetDefaultHotkeyList();
}
