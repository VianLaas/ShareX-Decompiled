using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ShareX.HelpersLib;

namespace ShareX;

public static class CleanupManager
{
	public static void Cleanup()
	{
		try
		{
			CleanupAppTempFolder();
			if (Program.Settings != null)
			{
				int keepFileCount = Math.Max(Program.Settings.CleanupKeepFileCount, 0);
				if (Program.Settings.AutoCleanupBackupFiles)
				{
					CleanupFolder(SettingManager.BackupFolder, "ApplicationConfig-*.json", keepFileCount);
					CleanupFolder(SettingManager.BackupFolder, "HotkeysConfig-*.json", keepFileCount);
					CleanupFolder(SettingManager.BackupFolder, "UploadersConfig-*.json", keepFileCount);
					CleanupFolder(SettingManager.BackupFolder, "History-*.json", keepFileCount);
				}
				if (Program.Settings.AutoCleanupLogFiles)
				{
					CleanupFolder(Program.LogsFolder, "ShareX-Log-*.txt", keepFileCount);
				}
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	public static void CleanupAsync()
	{
		Task.Run(delegate
		{
			Cleanup();
		});
	}

	private static void CleanupFolder(string folderPath, string fileNamePattern, int keepFileCount)
	{
		foreach (FileInfo item in (from f in new DirectoryInfo(folderPath).GetFiles(fileNamePattern)
			orderby (f.LastWriteTime.Year > 1601) ? f.LastWriteTime : f.CreationTime descending
			select f).Skip(keepFileCount))
		{
			item.Delete();
			DebugHelper.WriteLine("File deleted: " + item.FullName);
		}
	}

	private static void CleanupAppTempFolder()
	{
		string tempPath = Path.GetTempPath();
		if (!string.IsNullOrEmpty(tempPath))
		{
			string text = Path.Combine(tempPath, "ShareX");
			if (Directory.Exists(text))
			{
				Directory.Delete(text, recursive: true);
				DebugHelper.WriteLine("ShareX temp folder cleaned: " + text);
			}
		}
	}
}
