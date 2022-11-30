using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.HistoryLib;
using ShareX.Properties;
using ShareX.UploadersLib;

namespace ShareX;

public static class TaskManager
{
	private static int lastIconStatus = -1;

	public static List<WorkerTask> Tasks { get; } = new List<WorkerTask>();


	public static TaskListView TaskListView { get; set; }

	public static TaskThumbnailView TaskThumbnailView { get; set; }

	public static RecentTaskManager RecentManager { get; } = new RecentTaskManager();


	public static bool IsBusy
	{
		get
		{
			if (Tasks.Count > 0)
			{
				return Tasks.Any((WorkerTask task) => task.IsBusy);
			}
			return false;
		}
	}

	public static void Start(WorkerTask task)
	{
		if (task != null)
		{
			Tasks.Add(task);
			UpdateMainFormTip();
			if (task.Status != TaskStatus.History)
			{
				task.StatusChanged += Task_StatusChanged;
				task.ImageReady += Task_ImageReady;
				task.UploadStarted += Task_UploadStarted;
				task.UploadProgressChanged += Task_UploadProgressChanged;
				task.UploadCompleted += Task_UploadCompleted;
				task.TaskCompleted += Task_TaskCompleted;
				task.UploadersConfigWindowRequested += Task_UploadersConfigWindowRequested;
			}
			TaskListView.AddItem(task);
			TaskThumbnailPanel taskThumbnailPanel = TaskThumbnailView.AddPanel(task);
			if (Program.Settings.TaskViewMode == TaskViewMode.ThumbnailView)
			{
				taskThumbnailPanel.UpdateThumbnail();
			}
			if (task.Status != TaskStatus.History)
			{
				StartTasks();
			}
		}
	}

	public static void Remove(WorkerTask task)
	{
		if (task != null)
		{
			task.Stop();
			Tasks.Remove(task);
			UpdateMainFormTip();
			TaskListView.RemoveItem(task);
			TaskThumbnailView.RemovePanel(task);
			task.Dispose();
		}
	}

	private static void StartTasks()
	{
		int num = Tasks.Count((WorkerTask x) => x.IsWorking);
		WorkerTask[] array = Tasks.Where((WorkerTask x) => x.Status == TaskStatus.InQueue).ToArray();
		if (array.Length != 0)
		{
			int num2 = ((Program.Settings.UploadLimit != 0) ? (Program.Settings.UploadLimit - num).Clamp(0, array.Length) : array.Length);
			for (int i = 0; i < num2; i++)
			{
				array[i].Start();
			}
		}
	}

	public static void StopAllTasks()
	{
		foreach (WorkerTask task in Tasks)
		{
			task?.Stop();
		}
	}

	public static void UpdateMainFormTip()
	{
		Label lblListViewTip = Program.MainForm.lblListViewTip;
		bool visible = (Program.MainForm.lblThumbnailViewTip.Visible = Program.Settings.ShowMainWindowTip && Tasks.Count == 0);
		lblListViewTip.Visible = visible;
	}

	private static void Task_StatusChanged(WorkerTask task)
	{
		DebugHelper.WriteLine("Task status: " + task.Status);
		ListViewItem listViewItem = TaskListView.FindItem(task);
		if (listViewItem != null)
		{
			listViewItem.SubItems[1].Text = task.Info.Status;
		}
		UpdateProgressUI();
	}

	private static void Task_ImageReady(WorkerTask task, Bitmap image)
	{
		TaskThumbnailPanel taskThumbnailPanel = TaskThumbnailView.FindPanel(task);
		if (taskThumbnailPanel != null)
		{
			taskThumbnailPanel.UpdateTitle();
			if (Program.Settings.TaskViewMode == TaskViewMode.ThumbnailView)
			{
				taskThumbnailPanel.UpdateThumbnail(image);
			}
		}
	}

	private static void Task_UploadStarted(WorkerTask task)
	{
		TaskInfo info = task.Info;
		string text = $"Upload started. File name: {info.FileName}";
		if (!string.IsNullOrEmpty(info.FilePath))
		{
			text = text + ", File path: " + info.FilePath;
		}
		DebugHelper.WriteLine(text);
		ListViewItem listViewItem = TaskListView.FindItem(task);
		if (listViewItem != null)
		{
			listViewItem.Text = info.FileName;
			listViewItem.SubItems[1].Text = info.Status;
			listViewItem.ImageIndex = 0;
		}
		TaskThumbnailPanel taskThumbnailPanel = TaskThumbnailView.FindPanel(task);
		if (taskThumbnailPanel != null)
		{
			taskThumbnailPanel.UpdateStatus();
			taskThumbnailPanel.ProgressVisible = true;
		}
	}

	private static void Task_UploadProgressChanged(WorkerTask task)
	{
		if (task.Status != TaskStatus.Working)
		{
			return;
		}
		TaskInfo info = task.Info;
		ListViewItem listViewItem = TaskListView.FindItem(task);
		if (listViewItem != null)
		{
			listViewItem.SubItems[1].Text = $"{info.Progress.Percentage:0.0}%";
			if (info.Progress.CustomProgressText != null)
			{
				listViewItem.SubItems[2].Text = info.Progress.CustomProgressText;
				listViewItem.SubItems[3].Text = "";
			}
			else
			{
				listViewItem.SubItems[2].Text = $"{info.Progress.Position.ToSizeString(Program.Settings.BinaryUnits)} / {info.Progress.Length.ToSizeString(Program.Settings.BinaryUnits)}";
				if (info.Progress.Speed > 0.0)
				{
					listViewItem.SubItems[3].Text = ((long)info.Progress.Speed).ToSizeString(Program.Settings.BinaryUnits) + "/s";
				}
			}
			listViewItem.SubItems[4].Text = Helpers.ProperTimeSpan(info.Progress.Elapsed);
			listViewItem.SubItems[5].Text = Helpers.ProperTimeSpan(info.Progress.Remaining);
		}
		TaskThumbnailView.FindPanel(task)?.UpdateProgress();
		UpdateProgressUI();
	}

	private static void Task_UploadCompleted(WorkerTask task)
	{
		TaskInfo info = task.Info;
		if (info != null && info.Result != null && !info.Result.IsError)
		{
			string text = info.Result.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = "Upload completed. URL: " + text;
				if (info.UploadDuration != null)
				{
					text2 += $", Duration: {info.UploadDuration.ElapsedMilliseconds} ms";
				}
				DebugHelper.WriteLine(text2);
			}
		}
		TaskThumbnailPanel taskThumbnailPanel = TaskThumbnailView.FindPanel(task);
		if (taskThumbnailPanel != null)
		{
			taskThumbnailPanel.ProgressVisible = false;
		}
	}

	private static void Task_TaskCompleted(WorkerTask task)
	{
		try
		{
			if (task == null)
			{
				return;
			}
			task.KeepImage = false;
			if (task.RequestSettingUpdate)
			{
				Program.MainForm.UpdateCheckStates();
			}
			TaskInfo info = task.Info;
			if (info == null || info.Result == null)
			{
				return;
			}
			TaskThumbnailPanel taskThumbnailPanel = TaskThumbnailView.FindPanel(task);
			if (taskThumbnailPanel != null)
			{
				taskThumbnailPanel.UpdateStatus();
				taskThumbnailPanel.ProgressVisible = false;
			}
			ListViewItem listViewItem = TaskListView.FindItem(task);
			if (task.Status == TaskStatus.Stopped)
			{
				DebugHelper.WriteLine("Task stopped. File name: " + info.FileName);
				if (listViewItem != null)
				{
					listViewItem.Text = info.FileName;
					listViewItem.SubItems[1].Text = info.Status;
					listViewItem.ImageIndex = 2;
				}
			}
			else if (task.Status == TaskStatus.Failed)
			{
				string text = string.Join("\r\n\r\n", info.Result.Errors.ToArray());
				DebugHelper.WriteLine("Task failed. File name: " + info.FileName + ", Errors:\r\n" + text);
				if (listViewItem != null)
				{
					listViewItem.SubItems[1].Text = info.Status;
					listViewItem.SubItems[6].Text = "";
					listViewItem.ImageIndex = 1;
				}
				if (!info.TaskSettings.GeneralSettings.DisableNotifications)
				{
					if (info.TaskSettings.GeneralSettings.PlaySoundAfterUpload)
					{
						TaskHelpers.PlayErrorSound(info.TaskSettings);
					}
					if (info.Result.Errors.Count > 0)
					{
						string text2 = info.Result.Errors[0];
						if (info.TaskSettings.GeneralSettings.ShowToastNotificationAfterTaskCompleted && !string.IsNullOrEmpty(text2) && (!info.TaskSettings.GeneralSettings.DisableNotificationsOnFullscreen || !CaptureHelpers.IsActiveWindowFullscreen()))
						{
							TaskHelpers.ShowNotificationTip(text2, "ShareX - " + Resources.TaskManager_task_UploadCompleted_Error, 5000);
						}
					}
				}
			}
			else
			{
				DebugHelper.WriteLine($"Task completed. File name: {info.FileName}, Duration: {(long)info.TaskDuration.TotalMilliseconds} ms");
				string text3 = info.ToString();
				if (listViewItem != null)
				{
					listViewItem.Text = info.FileName;
					listViewItem.SubItems[1].Text = info.Status;
					listViewItem.ImageIndex = 2;
					if (!string.IsNullOrEmpty(text3))
					{
						listViewItem.SubItems[6].Text = text3;
					}
				}
				if (!task.StopRequested && !string.IsNullOrEmpty(text3))
				{
					if (Program.Settings.HistorySaveTasks && (!Program.Settings.HistoryCheckURL || !string.IsNullOrEmpty(info.Result.URL) || !string.IsNullOrEmpty(info.Result.ShortenedURL)))
					{
						AppendHistoryItemAsync(info.GetHistoryItem());
					}
					RecentManager.Add(task);
					if (!info.TaskSettings.GeneralSettings.DisableNotifications && info.Job != TaskJob.ShareURL)
					{
						if (info.TaskSettings.GeneralSettings.PlaySoundAfterUpload)
						{
							TaskHelpers.PlayTaskCompleteSound(info.TaskSettings);
						}
						if (!string.IsNullOrEmpty(info.TaskSettings.AdvancedSettings.BalloonTipContentFormat))
						{
							text3 = new UploadInfoParser().Parse(info, info.TaskSettings.AdvancedSettings.BalloonTipContentFormat);
						}
						if (info.TaskSettings.GeneralSettings.ShowToastNotificationAfterTaskCompleted && !string.IsNullOrEmpty(text3) && (!info.TaskSettings.GeneralSettings.DisableNotificationsOnFullscreen || !CaptureHelpers.IsActiveWindowFullscreen()))
						{
							task.KeepImage = true;
							NotificationForm.Show(new NotificationFormConfig
							{
								Duration = (int)(info.TaskSettings.GeneralSettings.ToastWindowDuration * 1000f),
								FadeDuration = (int)(info.TaskSettings.GeneralSettings.ToastWindowFadeDuration * 1000f),
								Placement = info.TaskSettings.GeneralSettings.ToastWindowPlacement,
								Size = info.TaskSettings.GeneralSettings.ToastWindowSize,
								LeftClickAction = info.TaskSettings.GeneralSettings.ToastWindowLeftClickAction,
								RightClickAction = info.TaskSettings.GeneralSettings.ToastWindowRightClickAction,
								MiddleClickAction = info.TaskSettings.GeneralSettings.ToastWindowMiddleClickAction,
								FilePath = info.FilePath,
								Image = task.Image,
								Title = "ShareX - " + Resources.TaskManager_task_UploadCompleted_ShareX___Task_completed,
								Text = text3,
								URL = text3
							});
							if (info.TaskSettings.AfterUploadJob.HasFlag(AfterUploadTasks.ShowAfterUploadWindow) && info.IsUploadJob)
							{
								NativeMethods.ShowWindow(new AfterUploadForm(info).Handle, 8);
							}
						}
					}
				}
			}
			if (listViewItem != null)
			{
				listViewItem.EnsureVisible();
				if (Program.Settings.AutoSelectLastCompletedTask)
				{
					TaskListView.ListViewControl.SelectSingle(listViewItem);
				}
			}
		}
		finally
		{
			if (!IsBusy && Program.CLI.IsCommandExist("AutoClose"))
			{
				Application.Exit();
			}
			else
			{
				StartTasks();
				UpdateProgressUI();
				if (Program.Settings.SaveSettingsAfterTaskCompleted && !IsBusy)
				{
					SettingManager.SaveAllSettingsAsync();
				}
			}
		}
	}

	private static void Task_UploadersConfigWindowRequested(IUploaderService uploaderService)
	{
		TaskHelpers.OpenUploadersConfigWindow(uploaderService);
	}

	public static void UpdateProgressUI()
	{
		bool flag = false;
		double num = 0.0;
		IEnumerable<WorkerTask> source = Tasks.Where((WorkerTask x) => x != null && x.Status == TaskStatus.Working && x.Info != null);
		if (source.Count() > 0)
		{
			flag = true;
			source = source.Where((WorkerTask x) => x.Info.Progress != null);
			if (source.Count() > 0)
			{
				num = source.Average((WorkerTask x) => x.Info.Progress.Percentage);
			}
		}
		if (flag)
		{
			Program.MainForm.Text = $"{Program.Title} - {num:0.0}%";
			UpdateTrayIcon((int)num);
			TaskbarManager.SetProgressValue(Program.MainForm, (int)num);
		}
		else
		{
			Program.MainForm.Text = Program.Title;
			UpdateTrayIcon();
			TaskbarManager.SetProgressState(Program.MainForm, TaskbarProgressBarStatus.NoProgress);
		}
	}

	public static void UpdateTrayIcon(int progress = -1)
	{
		if (!Program.Settings.TrayIconProgressEnabled || !Program.MainForm.niTray.Visible || lastIconStatus == progress)
		{
			return;
		}
		Icon icon;
		if (progress >= 0)
		{
			try
			{
				icon = Helpers.GetProgressIcon(progress);
			}
			catch (Exception exception)
			{
				DebugHelper.WriteException(exception);
				progress = -1;
				if (lastIconStatus == progress)
				{
					return;
				}
				icon = ShareXResources.Icon;
			}
		}
		else
		{
			icon = ShareXResources.Icon;
		}
		using (Icon icon2 = Program.MainForm.niTray.Icon)
		{
			Program.MainForm.niTray.Icon = icon;
			icon2.DisposeHandle();
		}
		lastIconStatus = progress;
	}

	public static void AddTestTasks(int count)
	{
		for (int i = 0; i < count; i++)
		{
			Start(WorkerTask.CreateHistoryTask(new RecentTask
			{
				FilePath = "..\\..\\..\\ShareX.HelpersLib\\Resources\\ShareX_Logo.png"
			}));
		}
	}

	public static void TestTrayIcon()
	{
		Timer timer = new Timer();
		timer.Interval = 50;
		int i = 0;
		timer.Tick += delegate
		{
			if (i > 99)
			{
				timer.Stop();
				UpdateTrayIcon();
			}
			else
			{
				UpdateTrayIcon(i++);
			}
		};
		timer.Start();
	}

	private static void AppendHistoryItemAsync(HistoryItem historyItem)
	{
		Task.Run(delegate
		{
			HistoryManagerJSON historyManagerJSON = new HistoryManagerJSON(Program.HistoryFilePath);
			historyManagerJSON.BackupFolder = SettingManager.BackupFolder;
			historyManagerJSON.CreateBackup = false;
			historyManagerJSON.CreateWeeklyBackup = true;
			historyManagerJSON.AppendHistoryItem(historyItem);
		});
	}

	public static void AddRecentTasksToMainWindow()
	{
		if (TaskListView.ListViewControl.Items.Count != 0)
		{
			return;
		}
		foreach (RecentTask task in RecentManager.Tasks)
		{
			Start(WorkerTask.CreateHistoryTask(task));
		}
	}
}
