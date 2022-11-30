using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class TaskListView
{
	public MyListView ListViewControl { get; private set; }

	public TaskListView(MyListView listViewControl)
	{
		ListViewControl = listViewControl;
	}

	public ListViewItem AddItem(WorkerTask task)
	{
		TaskInfo info = task.Info;
		if (task.Status != TaskStatus.History)
		{
			DebugHelper.WriteLine("Task in queue. Job: {0}, Type: {1}, Host: {2}", info.Job, info.UploadDestination, info.UploaderHost);
		}
		ListViewItem listViewItem = new ListViewItem();
		listViewItem.Tag = task;
		listViewItem.Text = info.FileName;
		if (task.Status == TaskStatus.History)
		{
			listViewItem.SubItems.Add(Resources.TaskManager_CreateListViewItem_History);
			listViewItem.SubItems.Add(task.Info.TaskEndTime.ToString());
		}
		else
		{
			listViewItem.SubItems.Add(Resources.TaskManager_CreateListViewItem_In_queue);
			listViewItem.SubItems.Add("");
		}
		listViewItem.SubItems.Add("");
		listViewItem.SubItems.Add("");
		listViewItem.SubItems.Add("");
		if (task.Status == TaskStatus.History)
		{
			listViewItem.SubItems.Add(task.Info.ToString());
			listViewItem.ImageIndex = 4;
		}
		else
		{
			listViewItem.SubItems.Add("");
			listViewItem.ImageIndex = 3;
		}
		if (Program.Settings.ShowMostRecentTaskFirst)
		{
			ListViewControl.Items.Insert(0, listViewItem);
		}
		else
		{
			ListViewControl.Items.Add(listViewItem);
		}
		listViewItem.EnsureVisible();
		ListViewControl.FillLastColumn();
		return listViewItem;
	}

	public void RemoveItem(WorkerTask task)
	{
		ListViewItem listViewItem = FindItem(task);
		if (listViewItem != null)
		{
			ListViewControl.Items.Remove(listViewItem);
		}
	}

	public ListViewItem FindItem(WorkerTask task)
	{
		foreach (ListViewItem item in ListViewControl.Items)
		{
			if (item.Tag is WorkerTask workerTask && workerTask == task)
			{
				return item;
			}
		}
		return null;
	}
}
