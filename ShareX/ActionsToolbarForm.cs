using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class ActionsToolbarForm : Form
{
	private static ActionsToolbarForm instance;

	private IContainer components;

	private ToolStripEx tsMain;

	private ToolTip ttMain;

	private ContextMenuStrip cmsTitle;

	public static ActionsToolbarForm Instance
	{
		get
		{
			if (!IsInstanceActive)
			{
				instance = new ActionsToolbarForm();
			}
			return instance;
		}
	}

	public static bool IsInstanceActive
	{
		get
		{
			if (instance != null)
			{
				return !instance.IsDisposed;
			}
			return false;
		}
	}

	private ActionsToolbarForm()
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		this.AllowDrop = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.AutoSize = true;
		base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.BackColor = System.Drawing.SystemColors.ActiveBorder;
		base.ClientSize = new System.Drawing.Size(284, 261);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		this.Text = "ShareX - Actions toolbar";
		base.TopMost = ShareX.Program.Settings.ActionsToolbarStayTopMost;
		base.Shown += new System.EventHandler(ActionsToolbarForm_Shown);
		base.LocationChanged += new System.EventHandler(ActionsToolbarForm_LocationChanged);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(ActionsToolbarForm_DragEnter);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(ActionsToolbarForm_DragDrop);
		this.tsMain = new ShareX.HelpersLib.ToolStripEx
		{
			AutoSize = true,
			CanOverflow = false,
			ClickThrough = true,
			Dock = System.Windows.Forms.DockStyle.None,
			GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden,
			Location = new System.Drawing.Point(1, 1),
			Margin = new System.Windows.Forms.Padding(1),
			MinimumSize = new System.Drawing.Size(10, 30),
			Padding = new System.Windows.Forms.Padding(0, 1, 0, 0),
			Renderer = new ShareX.HelpersLib.ToolStripRoundedEdgeRenderer(),
			TabIndex = 0,
			ShowItemToolTips = false
		};
		this.tsMain.MouseLeave += new System.EventHandler(tsMain_MouseLeave);
		base.Controls.Add(this.tsMain);
		this.components = new System.ComponentModel.Container();
		this.ttMain = new System.Windows.Forms.ToolTip(this.components)
		{
			AutoPopDelay = 15000,
			InitialDelay = 300,
			ReshowDelay = 100,
			ShowAlways = true
		};
		this.cmsTitle = new System.Windows.Forms.ContextMenuStrip(this.components);
		System.Windows.Forms.ToolStripMenuItem toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem(ShareX.Properties.Resources.ActionsToolbar_Close);
		toolStripMenuItem.Click += new System.EventHandler(TsmiClose_Click);
		this.cmsTitle.Items.Add(toolStripMenuItem);
		this.cmsTitle.Items.Add(new System.Windows.Forms.ToolStripSeparator());
		System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem(ShareX.Properties.Resources.ActionsToolbar__LockPosition);
		toolStripMenuItem2.CheckOnClick = true;
		toolStripMenuItem2.Checked = ShareX.Program.Settings.ActionsToolbarLockPosition;
		toolStripMenuItem2.Click += new System.EventHandler(TsmiLock_Click);
		this.cmsTitle.Items.Add(toolStripMenuItem2);
		System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem(ShareX.Properties.Resources.ActionsToolbar_StayTopMost);
		toolStripMenuItem3.CheckOnClick = true;
		toolStripMenuItem3.Checked = ShareX.Program.Settings.ActionsToolbarStayTopMost;
		toolStripMenuItem3.Click += new System.EventHandler(TsmiTopMost_Click);
		this.cmsTitle.Items.Add(toolStripMenuItem3);
		System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem(ShareX.Properties.Resources.ActionsToolbar_OpenAtShareXStartup);
		toolStripMenuItem4.CheckOnClick = true;
		toolStripMenuItem4.Checked = ShareX.Program.Settings.ActionsToolbarRunAtStartup;
		toolStripMenuItem4.Click += new System.EventHandler(TsmiRunAtStartup_Click);
		this.cmsTitle.Items.Add(toolStripMenuItem4);
		this.cmsTitle.Items.Add(new System.Windows.Forms.ToolStripSeparator());
		System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem(ShareX.Properties.Resources.ActionsToolbar_Edit);
		toolStripMenuItem5.Click += new System.EventHandler(TsmiEdit_Click);
		this.cmsTitle.Items.Add(toolStripMenuItem5);
		this.UpdateToolbar(ShareX.Program.Settings.ActionsToolbarList);
		base.ResumeLayout(false);
		base.PerformLayout();
		this.UpdatePosition();
	}

	private void ActionsToolbarForm_Shown(object sender, EventArgs e)
	{
		this.ForceActivate();
	}

	private void ActionsToolbarForm_LocationChanged(object sender, EventArgs e)
	{
		CheckToolbarPosition();
	}

	private void ActionsToolbarForm_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop, autoConvert: false) || e.Data.GetDataPresent(DataFormats.Bitmap, autoConvert: false) || e.Data.GetDataPresent(DataFormats.Text, autoConvert: false))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void ActionsToolbarForm_DragDrop(object sender, DragEventArgs e)
	{
		UploadManager.DragDropUpload(e.Data);
	}

	private void CheckToolbarPosition()
	{
		Rectangle bounds = base.Bounds;
		Rectangle screenWorkingArea = CaptureHelpers.GetScreenWorkingArea();
		Point location = bounds.Location;
		if (bounds.Width < screenWorkingArea.Width)
		{
			if (bounds.X < screenWorkingArea.X)
			{
				location.X = screenWorkingArea.X;
			}
			else if (bounds.Right > screenWorkingArea.Right)
			{
				location.X = screenWorkingArea.Right - bounds.Width;
			}
		}
		if (bounds.Height < screenWorkingArea.Height)
		{
			if (bounds.Y < screenWorkingArea.Y)
			{
				location.Y = screenWorkingArea.Y;
			}
			else if (bounds.Bottom > screenWorkingArea.Bottom)
			{
				location.Y = screenWorkingArea.Bottom - bounds.Height;
			}
		}
		if (location != bounds.Location)
		{
			base.Location = location;
		}
		Program.Settings.ActionsToolbarPosition = location;
	}

	private void UpdatePosition()
	{
		Rectangle screenWorkingArea = CaptureHelpers.GetScreenWorkingArea();
		if (!Program.Settings.ActionsToolbarPosition.IsEmpty && screenWorkingArea.Contains(Program.Settings.ActionsToolbarPosition))
		{
			base.Location = Program.Settings.ActionsToolbarPosition;
			return;
		}
		Rectangle activeScreenWorkingArea = CaptureHelpers.GetActiveScreenWorkingArea();
		if (base.Width < activeScreenWorkingArea.Width)
		{
			base.Location = new Point(activeScreenWorkingArea.X + activeScreenWorkingArea.Width - base.Width, activeScreenWorkingArea.Y + activeScreenWorkingArea.Height - base.Height);
		}
		else
		{
			base.Location = activeScreenWorkingArea.Location;
		}
	}

	private void TsmiClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void TsmiLock_Click(object sender, EventArgs e)
	{
		Program.Settings.ActionsToolbarLockPosition = ((ToolStripMenuItem)sender).Checked;
	}

	private void TsmiTopMost_Click(object sender, EventArgs e)
	{
		Program.Settings.ActionsToolbarStayTopMost = ((ToolStripMenuItem)sender).Checked;
		base.TopMost = Program.Settings.ActionsToolbarStayTopMost;
	}

	private void TsmiRunAtStartup_Click(object sender, EventArgs e)
	{
		Program.Settings.ActionsToolbarRunAtStartup = ((ToolStripMenuItem)sender).Checked;
	}

	private void TsmiEdit_Click(object sender, EventArgs e)
	{
		using ActionsToolbarEditForm actionsToolbarEditForm = new ActionsToolbarEditForm(Program.Settings.ActionsToolbarList);
		if (Program.Settings.ActionsToolbarStayTopMost)
		{
			base.TopMost = false;
		}
		actionsToolbarEditForm.ShowDialog();
		if (Program.Settings.ActionsToolbarStayTopMost)
		{
			base.TopMost = true;
		}
		UpdateToolbar(Program.Settings.ActionsToolbarList);
		CheckToolbarPosition();
	}

	private void UpdateToolbar(List<HotkeyType> actions)
	{
		tsMain.SuspendLayout();
		tsMain.Items.Clear();
		ToolStripLabel toolStripLabel = new ToolStripLabel
		{
			Margin = new Padding(4, 0, 3, 0),
			Text = "ShareX",
			ToolTipText = Resources.ActionsToolbar_Tip
		};
		toolStripLabel.MouseDown += tslTitle_MouseDown;
		toolStripLabel.MouseEnter += tslTitle_MouseEnter;
		toolStripLabel.MouseLeave += tslTitle_MouseLeave;
		toolStripLabel.MouseUp += tslTitle_MouseUp;
		tsMain.Items.Add(toolStripLabel);
		foreach (HotkeyType action in actions)
		{
			if (action == HotkeyType.None)
			{
				ToolStripSeparator value = new ToolStripSeparator
				{
					Margin = new Padding(0)
				};
				tsMain.Items.Add(value);
				continue;
			}
			ToolStripButton toolStripButton = new ToolStripButton
			{
				Text = action.GetLocalizedDescription(),
				DisplayStyle = ToolStripItemDisplayStyle.Image,
				Image = TaskHelpers.FindMenuIcon(action)
			};
			toolStripButton.Click += async delegate
			{
				if (Program.Settings.ActionsToolbarStayTopMost)
				{
					base.TopMost = false;
				}
				await TaskHelpers.ExecuteJob(action);
				if (Program.Settings.ActionsToolbarStayTopMost)
				{
					base.TopMost = true;
				}
			};
			tsMain.Items.Add(toolStripButton);
		}
		foreach (ToolStripItem tsi in tsMain.Items)
		{
			tsi.MouseEnter += delegate
			{
				string caption = (string.IsNullOrEmpty(tsi.ToolTipText) ? tsi.Text : tsi.ToolTipText);
				ttMain.SetToolTip(tsMain, caption);
			};
			tsi.MouseLeave += tsMain_MouseLeave;
		}
		tsMain.ResumeLayout(performLayout: false);
		tsMain.PerformLayout();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void tsMain_MouseLeave(object sender, EventArgs e)
	{
		ttMain.RemoveAll();
	}

	private void tslTitle_MouseEnter(object sender, EventArgs e)
	{
		if (!Program.Settings.ActionsToolbarLockPosition)
		{
			Cursor = Cursors.SizeAll;
		}
	}

	private void tslTitle_MouseLeave(object sender, EventArgs e)
	{
		Cursor = Cursors.Default;
	}

	private void tslTitle_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && !Program.Settings.ActionsToolbarLockPosition)
		{
			NativeMethods.ReleaseCapture();
			Message m = Message.Create(base.Handle, 274, new IntPtr(61458L), IntPtr.Zero);
			DefWndProc(ref m);
		}
	}

	private void tslTitle_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			cmsTitle.Show(base.Location.X, base.Location.Y + base.Size.Height - 1);
		}
		else if (e.Button == MouseButtons.Middle)
		{
			Close();
		}
	}
}
