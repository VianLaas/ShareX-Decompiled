using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ShareX.HelpersLib;

namespace ShareX;

public class WatchFolder : IDisposable
{
	public delegate void FileWatcherTriggerEventHandler(string path);

	private SynchronizationContext context;

	private FileSystemWatcher fileWatcher;

	private List<WatchFolderDuplicateEventTimer> timers = new List<WatchFolderDuplicateEventTimer>();

	public WatchFolderSettings Settings { get; set; }

	public TaskSettings TaskSettings { get; set; }

	public event FileWatcherTriggerEventHandler FileWatcherTrigger;

	public virtual void Enable()
	{
		Dispose();
		string text = FileHelpers.ExpandFolderVariables(Settings.FolderPath);
		if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
		{
			context = SynchronizationContext.Current ?? new SynchronizationContext();
			fileWatcher = new FileSystemWatcher(text);
			if (!string.IsNullOrEmpty(Settings.Filter))
			{
				fileWatcher.Filter = Settings.Filter;
			}
			fileWatcher.IncludeSubdirectories = Settings.IncludeSubdirectories;
			fileWatcher.Created += fileWatcher_Created;
			fileWatcher.EnableRaisingEvents = true;
		}
	}

	protected void OnFileWatcherTrigger(string path)
	{
		this.FileWatcherTrigger?.Invoke(path);
	}

	private async void fileWatcher_Created(object sender, FileSystemEventArgs e)
	{
		CleanElapsedTimers();
		string path = e.FullPath;
		foreach (WatchFolderDuplicateEventTimer timer in timers)
		{
			if (timer.IsDuplicateEvent(path))
			{
				return;
			}
		}
		timers.Add(new WatchFolderDuplicateEventTimer(path));
		int successCount = 0;
		long previousSize = -1L;
		await Helpers.WaitWhileAsync(delegate
		{
			if (!FileHelpers.IsFileLocked(path))
			{
				long fileSize = FileHelpers.GetFileSize(path);
				if (fileSize > 0 && fileSize == previousSize)
				{
					successCount++;
				}
				previousSize = fileSize;
				return successCount < 4;
			}
			previousSize = -1L;
			return true;
		}, 250, 5000, delegate
		{
			context.Post(delegate
			{
				OnFileWatcherTrigger(path);
			}, null);
		}, 1000);
	}

	protected void CleanElapsedTimers()
	{
		for (int i = 0; i < timers.Count; i++)
		{
			if (timers[i].IsElapsed)
			{
				timers.Remove(timers[i]);
			}
		}
	}

	public void Dispose()
	{
		if (fileWatcher != null)
		{
			fileWatcher.Dispose();
		}
	}
}
