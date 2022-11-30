using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShareX.HelpersLib;

namespace ShareX;

public class WatchFolderManager : IDisposable
{
	public List<WatchFolder> WatchFolders { get; private set; }

	public void UpdateWatchFolders()
	{
		if (WatchFolders != null)
		{
			UnregisterAllWatchFolders();
		}
		WatchFolders = new List<WatchFolder>();
		foreach (WatchFolderSettings watchFolder in Program.DefaultTaskSettings.WatchFolderList)
		{
			AddWatchFolder(watchFolder, Program.DefaultTaskSettings);
		}
		foreach (HotkeySettings hotkey in Program.HotkeysConfig.Hotkeys)
		{
			foreach (WatchFolderSettings watchFolder2 in hotkey.TaskSettings.WatchFolderList)
			{
				AddWatchFolder(watchFolder2, hotkey.TaskSettings);
			}
		}
	}

	private WatchFolder FindWatchFolder(WatchFolderSettings watchFolderSetting)
	{
		return WatchFolders.FirstOrDefault((WatchFolder watchFolder) => watchFolder.Settings == watchFolderSetting);
	}

	private bool IsExist(WatchFolderSettings watchFolderSetting)
	{
		return FindWatchFolder(watchFolderSetting) != null;
	}

	public void AddWatchFolder(WatchFolderSettings watchFolderSetting, TaskSettings taskSettings)
	{
		if (IsExist(watchFolderSetting))
		{
			return;
		}
		if (!taskSettings.WatchFolderList.Contains(watchFolderSetting))
		{
			taskSettings.WatchFolderList.Add(watchFolderSetting);
		}
		WatchFolder watchFolder = new WatchFolder();
		watchFolder.Settings = watchFolderSetting;
		watchFolder.TaskSettings = taskSettings;
		watchFolder.FileWatcherTrigger += delegate(string origPath)
		{
			TaskSettings safeTaskSettings = TaskSettings.GetSafeTaskSettings(taskSettings);
			string text = origPath;
			if (watchFolderSetting.MoveFilesToScreenshotsFolder)
			{
				string screenshotsFolder = TaskHelpers.GetScreenshotsFolder(safeTaskSettings);
				string fileName = Path.GetFileName(origPath);
				text = TaskHelpers.HandleExistsFile(screenshotsFolder, fileName, safeTaskSettings);
				FileHelpers.CreateDirectoryFromFilePath(text);
				File.Move(origPath, text);
			}
			UploadManager.UploadFile(text, safeTaskSettings);
		};
		WatchFolders.Add(watchFolder);
		if (taskSettings.WatchFolderEnabled)
		{
			watchFolder.Enable();
		}
	}

	public void RemoveWatchFolder(WatchFolderSettings watchFolderSetting)
	{
		using WatchFolder watchFolder = FindWatchFolder(watchFolderSetting);
		if (watchFolder != null)
		{
			watchFolder.TaskSettings.WatchFolderList.Remove(watchFolderSetting);
			WatchFolders.Remove(watchFolder);
		}
	}

	public void UpdateWatchFolderState(WatchFolderSettings watchFolderSetting)
	{
		WatchFolder watchFolder = FindWatchFolder(watchFolderSetting);
		if (watchFolder != null)
		{
			if (watchFolder.TaskSettings.WatchFolderEnabled)
			{
				watchFolder.Enable();
			}
			else
			{
				watchFolder.Dispose();
			}
		}
	}

	public void UnregisterAllWatchFolders()
	{
		if (WatchFolders == null)
		{
			return;
		}
		foreach (WatchFolder watchFolder in WatchFolders)
		{
			watchFolder?.Dispose();
		}
	}

	public void Dispose()
	{
		UnregisterAllWatchFolders();
	}
}
