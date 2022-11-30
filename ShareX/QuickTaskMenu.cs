using System;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class QuickTaskMenu
{
	public delegate void TaskInfoSelectedEventHandler(QuickTaskInfo taskInfo);

	public TaskInfoSelectedEventHandler TaskInfoSelected;

	public void ShowMenu()
	{
		ContextMenuStrip cms = new ContextMenuStrip
		{
			Font = new Font("Arial", 10f),
			AutoClose = false
		};
		cms.KeyUp += delegate(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				cms.Close();
			}
		};
		ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(Resources.QuickTaskMenu_ShowMenu_Continue);
		toolStripMenuItem.Image = Resources.control;
		toolStripMenuItem.Click += delegate
		{
			cms.Close();
			OnTaskInfoSelected(null);
		};
		cms.Items.Add(toolStripMenuItem);
		cms.Items.Add(new ToolStripSeparator());
		if (Program.Settings != null && Program.Settings.QuickTaskPresets != null && Program.Settings.QuickTaskPresets.Count > 0)
		{
			foreach (QuickTaskInfo quickTaskPreset in Program.Settings.QuickTaskPresets)
			{
				if (quickTaskPreset.IsValid)
				{
					ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem
					{
						Text = quickTaskPreset.ToString().Replace("&", "&&"),
						Tag = quickTaskPreset
					};
					toolStripMenuItem2.Image = FindSuitableIcon(quickTaskPreset);
					toolStripMenuItem2.Click += delegate(object sender, EventArgs e)
					{
						QuickTaskInfo taskInfo = ((ToolStripMenuItem)sender).Tag as QuickTaskInfo;
						cms.Close();
						OnTaskInfoSelected(taskInfo);
					};
					cms.Items.Add(toolStripMenuItem2);
				}
				else
				{
					cms.Items.Add(new ToolStripSeparator());
				}
			}
			cms.Items[0].Select();
			cms.Items.Add(new ToolStripSeparator());
		}
		ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(Resources.QuickTaskMenu_ShowMenu_Edit_this_menu___);
		toolStripMenuItem3.Image = Resources.pencil;
		toolStripMenuItem3.Click += delegate
		{
			cms.Close();
			new QuickTaskMenuEditorForm().ShowDialog();
		};
		cms.Items.Add(toolStripMenuItem3);
		cms.Items.Add(new ToolStripSeparator());
		ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(Resources.QuickTaskMenu_ShowMenu_Cancel);
		toolStripMenuItem4.Image = Resources.cross;
		toolStripMenuItem4.Click += delegate
		{
			cms.Close();
		};
		cms.Items.Add(toolStripMenuItem4);
		if (ShareXResources.UseCustomTheme)
		{
			ShareXResources.ApplyCustomThemeToContextMenuStrip(cms);
		}
		Point cursorPosition = CaptureHelpers.GetCursorPosition();
		cursorPosition.Offset(-10, -10);
		cms.Show(cursorPosition);
		cms.Focus();
	}

	protected void OnTaskInfoSelected(QuickTaskInfo taskInfo)
	{
		TaskInfoSelected?.Invoke(taskInfo);
	}

	public Image FindSuitableIcon(QuickTaskInfo taskInfo)
	{
		if (taskInfo.AfterCaptureTasks.HasFlag(AfterCaptureTasks.UploadImageToHost))
		{
			return Resources.upload_cloud;
		}
		if (taskInfo.AfterCaptureTasks.HasFlag(AfterCaptureTasks.CopyImageToClipboard) || taskInfo.AfterCaptureTasks.HasFlag(AfterCaptureTasks.CopyFileToClipboard))
		{
			return Resources.clipboard;
		}
		if (taskInfo.AfterCaptureTasks.HasFlag(AfterCaptureTasks.SaveImageToFile) || taskInfo.AfterCaptureTasks.HasFlag(AfterCaptureTasks.SaveImageToFileWithDialog))
		{
			return Resources.disk_black;
		}
		return Resources.image;
	}
}
