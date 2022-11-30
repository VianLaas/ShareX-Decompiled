using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class TaskThumbnailView : UserControl
{
	public delegate void TaskViewMouseEventHandler(object sender, MouseEventArgs e);

	private bool titleVisible = true;

	private ThumbnailTitleLocation titleLocation;

	private Size thumbnailSize = new Size(200, 150);

	private ThumbnailViewClickAction clickAction;

	private IContainer components;

	private FlowLayoutPanel flpMain;

	public List<TaskThumbnailPanel> Panels { get; private set; }

	public List<TaskThumbnailPanel> SelectedPanels { get; private set; }

	public TaskThumbnailPanel SelectedPanel
	{
		get
		{
			if (SelectedPanels.Count > 0)
			{
				return SelectedPanels[SelectedPanels.Count - 1];
			}
			return null;
		}
	}

	public bool TitleVisible
	{
		get
		{
			return titleVisible;
		}
		set
		{
			if (titleVisible == value)
			{
				return;
			}
			titleVisible = value;
			foreach (TaskThumbnailPanel panel in Panels)
			{
				panel.TitleVisible = titleVisible;
			}
		}
	}

	public ThumbnailTitleLocation TitleLocation
	{
		get
		{
			return titleLocation;
		}
		set
		{
			if (titleLocation == value)
			{
				return;
			}
			titleLocation = value;
			foreach (TaskThumbnailPanel panel in Panels)
			{
				panel.TitleLocation = titleLocation;
			}
		}
	}

	public Size ThumbnailSize
	{
		get
		{
			return thumbnailSize;
		}
		set
		{
			if (!(thumbnailSize != value))
			{
				return;
			}
			thumbnailSize = value;
			foreach (TaskThumbnailPanel panel in Panels)
			{
				panel.ThumbnailSize = thumbnailSize;
			}
			UpdateAllThumbnails(forceUpdate: true);
		}
	}

	public ThumbnailViewClickAction ClickAction
	{
		get
		{
			return clickAction;
		}
		set
		{
			if (clickAction == value)
			{
				return;
			}
			clickAction = value;
			foreach (TaskThumbnailPanel panel in Panels)
			{
				panel.ClickAction = clickAction;
			}
		}
	}

	public event TaskViewMouseEventHandler ContextMenuRequested;

	public event EventHandler SelectedPanelChanged;

	public TaskThumbnailView()
	{
		Panels = new List<TaskThumbnailPanel>();
		SelectedPanels = new List<TaskThumbnailPanel>();
		InitializeComponent();
		UpdateTheme();
	}

	protected override Point ScrollToControl(Control activeControl)
	{
		return base.AutoScrollPosition;
	}

	public void UpdateTheme()
	{
		if (ShareXResources.UseCustomTheme)
		{
			BackColor = ShareXResources.Theme.BackgroundColor;
		}
		else
		{
			BackColor = SystemColors.Window;
		}
		foreach (TaskThumbnailPanel panel in Panels)
		{
			panel.UpdateTheme();
		}
	}

	private TaskThumbnailPanel CreatePanel(WorkerTask task)
	{
		TaskThumbnailPanel panel = new TaskThumbnailPanel(task);
		panel.ThumbnailSize = ThumbnailSize;
		panel.ClickAction = ClickAction;
		panel.TitleVisible = TitleVisible;
		panel.TitleLocation = TitleLocation;
		panel.MouseEnter += Panel_MouseEnter;
		panel.MouseDown += delegate(object sender, MouseEventArgs e)
		{
			Panel_MouseDown(e, panel);
		};
		panel.MouseUp += Panel_MouseUp;
		panel.ImagePreviewRequested += Panel_ImagePreviewRequested;
		return panel;
	}

	public TaskThumbnailPanel AddPanel(WorkerTask task)
	{
		TaskThumbnailPanel taskThumbnailPanel = CreatePanel(task);
		Panels.Add(taskThumbnailPanel);
		flpMain.Controls.Add(taskThumbnailPanel);
		flpMain.Controls.SetChildIndex(taskThumbnailPanel, 0);
		return taskThumbnailPanel;
	}

	public void RemovePanel(WorkerTask task)
	{
		TaskThumbnailPanel taskThumbnailPanel = FindPanel(task);
		if (taskThumbnailPanel != null)
		{
			Panels.Remove(taskThumbnailPanel);
			SelectedPanels.Remove(taskThumbnailPanel);
			flpMain.Controls.Remove(taskThumbnailPanel);
			taskThumbnailPanel.Dispose();
		}
	}

	public TaskThumbnailPanel FindPanel(WorkerTask task)
	{
		return Panels.FirstOrDefault((TaskThumbnailPanel x) => x.Task == task);
	}

	public void UpdateAllThumbnails(bool forceUpdate = false)
	{
		foreach (TaskThumbnailPanel panel in Panels)
		{
			if (forceUpdate || !panel.ThumbnailExists)
			{
				panel.UpdateThumbnail();
			}
		}
	}

	public void UnselectAllPanels(TaskThumbnailPanel ignorePanel = null)
	{
		SelectedPanels.Clear();
		foreach (TaskThumbnailPanel panel in Panels)
		{
			if (panel != ignorePanel)
			{
				panel.Selected = false;
			}
		}
		OnSelectedPanelChanged();
	}

	protected void OnContextMenuRequested(object sender, MouseEventArgs e)
	{
		this.ContextMenuRequested?.Invoke(sender, e);
	}

	protected void OnSelectedPanelChanged()
	{
		this.SelectedPanelChanged?.Invoke(this, EventArgs.Empty);
	}

	private void Panel_MouseEnter(object sender, EventArgs e)
	{
		if (NativeMethods.GetForegroundWindow() == base.ParentForm.Handle && !flpMain.Focused)
		{
			flpMain.Focus();
		}
	}

	private void Panel_MouseDown(object sender, MouseEventArgs e)
	{
		Panel_MouseDown(e, null);
	}

	private void Panel_MouseDown(MouseEventArgs e, TaskThumbnailPanel panel)
	{
		if (panel == null)
		{
			UnselectAllPanels();
		}
		else if (Control.ModifierKeys == Keys.Control)
		{
			if (panel.Selected)
			{
				panel.Selected = false;
				SelectedPanels.Remove(panel);
			}
			else
			{
				panel.Selected = true;
				SelectedPanels.Add(panel);
			}
		}
		else if (Control.ModifierKeys == Keys.Shift)
		{
			if (SelectedPanels.Count > 0)
			{
				TaskThumbnailPanel start = SelectedPanels[0];
				UnselectAllPanels();
				foreach (TaskThumbnailPanel item in Panels.Range(start, panel))
				{
					item.Selected = true;
					SelectedPanels.Add(item);
				}
			}
			else
			{
				panel.Selected = true;
				SelectedPanels.Add(panel);
			}
		}
		else if (!panel.Selected || e.Button == MouseButtons.Left)
		{
			UnselectAllPanels(panel);
			panel.Selected = true;
			SelectedPanels.Add(panel);
		}
		OnSelectedPanelChanged();
	}

	private void Panel_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			OnContextMenuRequested(sender, e);
		}
	}

	private void Panel_ImagePreviewRequested(TaskThumbnailPanel panel)
	{
		string[] files = Panels.Select((TaskThumbnailPanel x) => x.Task.Info.FilePath).Reverse().ToArray();
		int imageIndex = Panels.Count - Panels.IndexOf(panel) - 1;
		ImageViewer.ShowImage(files, imageIndex);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		OnKeyDown(new KeyEventArgs(keyData));
		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
		base.SuspendLayout();
		this.flpMain.AutoSize = true;
		this.flpMain.Dock = System.Windows.Forms.DockStyle.Top;
		this.flpMain.Location = new System.Drawing.Point(0, 0);
		this.flpMain.Name = "flpMain";
		this.flpMain.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
		this.flpMain.Size = new System.Drawing.Size(242, 8);
		this.flpMain.TabIndex = 0;
		this.flpMain.MouseDown += new System.Windows.Forms.MouseEventHandler(Panel_MouseDown);
		this.flpMain.MouseEnter += new System.EventHandler(Panel_MouseEnter);
		this.flpMain.MouseUp += new System.Windows.Forms.MouseEventHandler(Panel_MouseUp);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		this.BackColor = System.Drawing.Color.FromArgb(42, 47, 56);
		base.Controls.Add(this.flpMain);
		base.Name = "TaskThumbnailView";
		base.Size = new System.Drawing.Size(242, 228);
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(Panel_MouseDown);
		base.MouseEnter += new System.EventHandler(Panel_MouseEnter);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(Panel_MouseUp);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
