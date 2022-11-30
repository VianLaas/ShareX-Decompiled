using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class HotkeySettings
{
	public HotkeyInfo HotkeyInfo { get; set; }

	public TaskSettings TaskSettings { get; set; }

	public HotkeySettings()
	{
		HotkeyInfo = new HotkeyInfo();
	}

	public HotkeySettings(HotkeyType job, Keys hotkey = Keys.None)
		: this()
	{
		TaskSettings = TaskSettings.GetDefaultTaskSettings();
		TaskSettings.Job = job;
		HotkeyInfo = new HotkeyInfo(hotkey);
	}

	public override string ToString()
	{
		if (HotkeyInfo != null && TaskSettings != null)
		{
			return $"Hotkey: {HotkeyInfo}, Description: {TaskSettings}, Job: {TaskSettings.Job}";
		}
		return "";
	}
}
