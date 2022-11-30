using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class RecentTaskManager
{
	private int maxCount = 10;

	private static readonly object itemsLock = new object();

	public int MaxCount
	{
		get
		{
			return maxCount;
		}
		set
		{
			maxCount = value.Clamp(1, 100);
			lock (itemsLock)
			{
				while (Tasks.Count > maxCount)
				{
					Tasks.Dequeue();
				}
				UpdateTrayMenu();
			}
		}
	}

	public Queue<RecentTask> Tasks { get; private set; }

	public RecentTaskManager()
	{
		Tasks = new Queue<RecentTask>();
	}

	public void InitItems()
	{
		lock (itemsLock)
		{
			MaxCount = Program.Settings.RecentTasksMaxCount;
			if (Program.Settings.RecentTasks != null)
			{
				Tasks = new Queue<RecentTask>(Program.Settings.RecentTasks.Take(MaxCount));
			}
			UpdateTrayMenu();
			UpdateMainWindowList();
		}
	}

	public void Add(WorkerTask task)
	{
		if (!string.IsNullOrEmpty(task.Info.ToString()))
		{
			RecentTask task2 = new RecentTask
			{
				FilePath = task.Info.FilePath,
				URL = task.Info.Result.URL,
				ThumbnailURL = task.Info.Result.ThumbnailURL,
				DeletionURL = task.Info.Result.DeletionURL,
				ShortenedURL = task.Info.Result.ShortenedURL
			};
			Add(task2);
		}
		if (Program.Settings.RecentTasksSave)
		{
			Program.Settings.RecentTasks = Tasks.ToArray();
		}
		else
		{
			Program.Settings.RecentTasks = null;
		}
	}

	public void Add(RecentTask task)
	{
		lock (itemsLock)
		{
			while (Tasks.Count >= MaxCount)
			{
				Tasks.Dequeue();
			}
			Tasks.Enqueue(task);
			UpdateTrayMenu();
		}
	}

	public void Clear()
	{
		lock (itemsLock)
		{
			Tasks.Clear();
			Program.Settings.RecentTasks = null;
			UpdateTrayMenu();
		}
	}

	private void UpdateTrayMenu()
	{
		ToolStripMenuItem tsmiTrayRecentItems = Program.MainForm.tsmiTrayRecentItems;
		if (Program.Settings.RecentTasksSave && Program.Settings.RecentTasksShowInTrayMenu && Tasks.Count > 0)
		{
			tsmiTrayRecentItems.Visible = true;
			tsmiTrayRecentItems.DropDownItems.Clear();
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(Resources.RecentManager_UpdateRecentMenu_Left_click_to_copy_URL_to_clipboard__Right_click_to_open_URL_);
			toolStripMenuItem.Enabled = false;
			tsmiTrayRecentItems.DropDownItems.Add(toolStripMenuItem);
			tsmiTrayRecentItems.DropDownItems.Add(new ToolStripSeparator());
			{
				foreach (RecentTask task in Tasks)
				{
					ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem();
					toolStripMenuItem2.Text = task.TrayMenuText;
					string link = task.ToString();
					toolStripMenuItem2.ToolTipText = link;
					toolStripMenuItem2.MouseUp += delegate(object sender, MouseEventArgs e)
					{
						if (e.Button == MouseButtons.Left)
						{
							ClipboardHelpers.CopyText(link);
						}
						else if (e.Button == MouseButtons.Right)
						{
							URLHelpers.OpenURL(link);
						}
					};
					if (Program.Settings.RecentTasksTrayMenuMostRecentFirst)
					{
						tsmiTrayRecentItems.DropDownItems.Insert(2, toolStripMenuItem2);
					}
					else
					{
						tsmiTrayRecentItems.DropDownItems.Add(toolStripMenuItem2);
					}
				}
				return;
			}
		}
		tsmiTrayRecentItems.Visible = false;
	}

	private void UpdateMainWindowList()
	{
		if (Program.Settings.RecentTasksSave && Program.Settings.RecentTasksShowInMainWindow && Tasks.Count > 0)
		{
			TaskManager.AddRecentTasksToMainWindow();
		}
	}
}
