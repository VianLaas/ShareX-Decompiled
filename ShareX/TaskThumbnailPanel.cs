using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class TaskThumbnailPanel : UserControl
{
	public delegate void TaskThumbnailPanelEventHandler(TaskThumbnailPanel panel);

	private bool selected;

	private string title;

	private bool titleVisible = true;

	private ThumbnailTitleLocation titleLocation;

	private int progress;

	private bool progressVisible;

	private Size thumbnailSize;

	private Rectangle dragBoxFromMouseDown;

	private IContainer components;

	private TaskRoundedCornerPanel pThumbnail;

	private BlackStyleLabel lblTitle;

	private BlackStyleProgressBar pbProgress;

	private PictureBox pbThumbnail;

	private ToolTip ttMain;

	private BlackStyleLabel lblError;

	public WorkerTask Task { get; private set; }

	public bool Selected
	{
		get
		{
			return selected;
		}
		set
		{
			if (selected != value)
			{
				selected = value;
				pThumbnail.Selected = selected;
			}
		}
	}

	public string Title
	{
		get
		{
			return title;
		}
		set
		{
			title = value;
			if (lblTitle.Text != title)
			{
				lblTitle.Text = title;
			}
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
			if (titleVisible != value)
			{
				titleVisible = value;
				lblTitle.Visible = titleVisible;
				UpdateLayout();
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
			if (titleLocation != value)
			{
				titleLocation = value;
				pThumbnail.StatusLocation = value;
				UpdateLayout();
			}
		}
	}

	public int Progress
	{
		get
		{
			return progress;
		}
		set
		{
			progress = value;
			if (pbProgress.Value != progress)
			{
				pbProgress.Value = progress;
			}
		}
	}

	public bool ProgressVisible
	{
		get
		{
			return progressVisible;
		}
		set
		{
			progressVisible = value;
			pbProgress.Visible = progressVisible;
		}
	}

	public bool ThumbnailExists { get; private set; }

	public Size ThumbnailSize
	{
		get
		{
			return thumbnailSize;
		}
		set
		{
			if (thumbnailSize != value)
			{
				thumbnailSize = value;
				UpdateLayout();
			}
		}
	}

	public ThumbnailViewClickAction ClickAction { get; set; }

	public new event EventHandler MouseEnter
	{
		add
		{
			base.MouseEnter += value;
			lblTitle.MouseEnter += value;
			pThumbnail.MouseEnter += value;
			pbThumbnail.MouseEnter += value;
			pbProgress.MouseEnter += value;
		}
		remove
		{
			base.MouseEnter -= value;
			lblTitle.MouseEnter -= value;
			pThumbnail.MouseEnter -= value;
			pbThumbnail.MouseEnter -= value;
			pbProgress.MouseEnter -= value;
		}
	}

	public new event MouseEventHandler MouseDown
	{
		add
		{
			base.MouseDown += value;
			lblTitle.MouseDown += value;
			pThumbnail.MouseDown += value;
			pbThumbnail.MouseDown += value;
			pbProgress.MouseDown += value;
		}
		remove
		{
			base.MouseDown -= value;
			lblTitle.MouseDown -= value;
			pThumbnail.MouseDown -= value;
			pbThumbnail.MouseDown -= value;
			pbProgress.MouseDown -= value;
		}
	}

	public new event MouseEventHandler MouseUp
	{
		add
		{
			base.MouseUp += value;
			lblTitle.MouseUp += value;
			pThumbnail.MouseUp += value;
			pbThumbnail.MouseUp += value;
			pbProgress.MouseUp += value;
		}
		remove
		{
			base.MouseUp -= value;
			lblTitle.MouseUp -= value;
			pThumbnail.MouseUp -= value;
			pbThumbnail.MouseUp -= value;
			pbProgress.MouseUp -= value;
		}
	}

	public event TaskThumbnailPanelEventHandler ImagePreviewRequested;

	public TaskThumbnailPanel(WorkerTask task)
	{
		Task = task;
		InitializeComponent();
		UpdateTheme();
		UpdateTitle();
	}

	protected void OnImagePreviewRequested()
	{
		this.ImagePreviewRequested?.Invoke(this);
	}

	public void UpdateTheme()
	{
		if (ShareXResources.UseCustomTheme)
		{
			lblTitle.ForeColor = ShareXResources.Theme.TextColor;
			lblTitle.TextShadowColor = ShareXResources.Theme.DarkBackgroundColor;
			pThumbnail.PanelColor = ShareXResources.Theme.DarkBackgroundColor;
			ttMain.BackColor = ShareXResources.Theme.BackgroundColor;
			ttMain.ForeColor = ShareXResources.Theme.TextColor;
		}
		else
		{
			lblTitle.ForeColor = SystemColors.ControlText;
			lblTitle.TextShadowColor = Color.Transparent;
			pThumbnail.PanelColor = SystemColors.ControlLight;
			ttMain.BackColor = SystemColors.Window;
			ttMain.ForeColor = SystemColors.ControlText;
		}
	}

	public void UpdateTitle()
	{
		Title = Task.Info?.FileName;
		if (Task.Info != null && !string.IsNullOrEmpty(Task.Info.ToString()))
		{
			lblTitle.Cursor = Cursors.Hand;
			ttMain.SetToolTip(lblTitle, Task.Info.ToString());
		}
		else
		{
			lblTitle.Cursor = Cursors.Default;
			ttMain.SetToolTip(lblTitle, null);
		}
	}

	private void UpdateLayout()
	{
		lblTitle.Width = pThumbnail.Padding.Horizontal + ThumbnailSize.Width;
		pThumbnail.Size = new Size(pThumbnail.Padding.Horizontal + ThumbnailSize.Width, pThumbnail.Padding.Vertical + ThumbnailSize.Height);
		int num = pThumbnail.Height;
		if (TitleVisible)
		{
			num += lblTitle.Height + 2;
		}
		base.Size = new Size(pThumbnail.Width, num);
		if (TitleLocation == ThumbnailTitleLocation.Top)
		{
			lblTitle.Location = new Point(0, 0);
			if (TitleVisible)
			{
				pThumbnail.Location = new Point(0, lblTitle.Height + 2);
			}
			else
			{
				pThumbnail.Location = new Point(0, 0);
			}
			lblError.Location = new Point((base.ClientSize.Width - lblError.Width) / 2, 1);
		}
		else
		{
			pThumbnail.Location = new Point(0, 0);
			lblTitle.Location = new Point(0, pThumbnail.Height + 2);
			lblError.Location = new Point((base.ClientSize.Width - lblError.Width) / 2, pThumbnail.Height - lblError.Height - 1);
		}
	}

	public void UpdateThumbnail(Bitmap bmp = null)
	{
		ClearThumbnail();
		if (ThumbnailSize.IsEmpty || Task.Info == null)
		{
			return;
		}
		try
		{
			string filePath = Task.Info.FilePath;
			if (ClickAction != ThumbnailViewClickAction.Select && !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
			{
				pbThumbnail.Cursor = Cursors.Hand;
			}
			Bitmap bitmap = CreateThumbnail(filePath, bmp);
			if (bitmap != null)
			{
				pbThumbnail.Image = bitmap;
				ThumbnailExists = true;
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	private Bitmap CreateThumbnail(string filePath, Bitmap bmp = null)
	{
		if (bmp != null)
		{
			return ImageHelpers.ResizeImage(bmp, ThumbnailSize, allowEnlarge: false);
		}
		if (string.IsNullOrEmpty(filePath))
		{
			filePath = Task.Info.FileName;
		}
		else if (File.Exists(filePath))
		{
			using Bitmap bitmap = ImageHelpers.LoadImage(filePath);
			if (bitmap != null)
			{
				return ImageHelpers.ResizeImage(bitmap, ThumbnailSize, allowEnlarge: false);
			}
		}
		if (!string.IsNullOrEmpty(filePath))
		{
			using (Icon icon = NativeMethods.GetJumboFileIcon(filePath, jumboSize: false))
			{
				using Bitmap bmp2 = icon.ToBitmap();
				return ImageHelpers.ResizeImage(bmp2, ThumbnailSize, allowEnlarge: false);
			}
		}
		return null;
	}

	public void UpdateProgress()
	{
		if (Task.Info != null)
		{
			Progress = (int)Task.Info.Progress.Percentage;
		}
	}

	public void UpdateStatus()
	{
		if (Task.Info != null)
		{
			pThumbnail.UpdateStatusColor(Task.Status);
			lblError.Visible = Task.Status == TaskStatus.Failed;
		}
		UpdateTitle();
	}

	public void ClearThumbnail()
	{
		Image image = pbThumbnail.Image;
		pbThumbnail.Image = null;
		if (image != null && image != pbThumbnail.ErrorImage && image != pbThumbnail.InitialImage)
		{
			image.Dispose();
		}
		pbThumbnail.Cursor = Cursors.Default;
		ThumbnailExists = false;
	}

	private void ExecuteClickAction(ThumbnailViewClickAction clickAction, TaskInfo info)
	{
		if (info == null)
		{
			return;
		}
		string filePath = info.FilePath;
		switch (clickAction)
		{
		case ThumbnailViewClickAction.Default:
			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
			{
				break;
			}
			if (FileHelpers.IsImageFile(filePath))
			{
				pbThumbnail.Enabled = false;
				try
				{
					OnImagePreviewRequested();
					break;
				}
				finally
				{
					pbThumbnail.Enabled = true;
				}
			}
			if (FileHelpers.IsTextFile(filePath) || FileHelpers.IsVideoFile(filePath) || MessageBox.Show("Would you like to open this file?\r\n\r\n" + filePath, Resources.ShareXConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				FileHelpers.OpenFile(filePath);
			}
			break;
		case ThumbnailViewClickAction.OpenImageViewer:
			if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath) && FileHelpers.IsImageFile(filePath))
			{
				pbThumbnail.Enabled = false;
				try
				{
					OnImagePreviewRequested();
					break;
				}
				finally
				{
					pbThumbnail.Enabled = true;
				}
			}
			break;
		case ThumbnailViewClickAction.OpenFile:
			if (!string.IsNullOrEmpty(filePath))
			{
				FileHelpers.OpenFile(filePath);
			}
			break;
		case ThumbnailViewClickAction.OpenFolder:
			if (!string.IsNullOrEmpty(filePath))
			{
				FileHelpers.OpenFolderWithFile(filePath);
			}
			break;
		case ThumbnailViewClickAction.OpenURL:
			if (info.Result != null)
			{
				URLHelpers.OpenURL(info.Result.ToString());
			}
			break;
		case ThumbnailViewClickAction.EditImage:
			if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath) && FileHelpers.IsImageFile(filePath))
			{
				TaskHelpers.AnnotateImageFromFile(filePath);
			}
			break;
		case ThumbnailViewClickAction.Select:
			break;
		}
	}

	private void LblTitle_MouseClick(object sender, MouseEventArgs e)
	{
		if (Control.ModifierKeys != 0 || e.Button != MouseButtons.Left || Task.Info == null)
		{
			return;
		}
		if (Task.Info.Result != null)
		{
			string text = Task.Info.Result.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				URLHelpers.OpenURL(text);
				return;
			}
		}
		if (!string.IsNullOrEmpty(Task.Info.FilePath))
		{
			FileHelpers.OpenFile(Task.Info.FilePath);
		}
	}

	private void lblError_MouseClick(object sender, MouseEventArgs e)
	{
		if (Control.ModifierKeys == Keys.None && e.Button == MouseButtons.Left)
		{
			Task.ShowErrorWindow();
		}
	}

	private void PbThumbnail_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			Size size = new Size(10, 10);
			dragBoxFromMouseDown = new Rectangle(new Point(e.X - size.Width / 2, e.Y - size.Height / 2), size);
		}
	}

	private void PbThumbnail_MouseUp(object sender, MouseEventArgs e)
	{
		dragBoxFromMouseDown = Rectangle.Empty;
	}

	private void PbThumbnail_MouseClick(object sender, MouseEventArgs e)
	{
		if (Control.ModifierKeys == Keys.None && e.Button == MouseButtons.Left)
		{
			ExecuteClickAction(ClickAction, Task.Info);
		}
	}

	private void pbThumbnail_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (Control.ModifierKeys == Keys.None && e.Button == MouseButtons.Left && ClickAction == ThumbnailViewClickAction.Select)
		{
			ExecuteClickAction(ThumbnailViewClickAction.OpenFile, Task.Info);
		}
	}

	private void PbThumbnail_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
		{
			if (Task.Info != null && !string.IsNullOrEmpty(Task.Info.FilePath) && File.Exists(Task.Info.FilePath))
			{
				Program.MainForm.AllowDrop = false;
				IDataObject data = new DataObject(DataFormats.FileDrop, new string[1] { Task.Info.FilePath });
				dragBoxFromMouseDown = Rectangle.Empty;
				pbThumbnail.DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move);
				Program.MainForm.AllowDrop = true;
			}
			else
			{
				dragBoxFromMouseDown = Rectangle.Empty;
			}
		}
	}

	private void TtMain_Draw(object sender, DrawToolTipEventArgs e)
	{
		e.DrawBackground();
		e.DrawBorder();
		e.DrawText();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		ClearThumbnail();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.TaskThumbnailPanel));
		this.ttMain = new System.Windows.Forms.ToolTip(this.components);
		this.lblTitle = new ShareX.HelpersLib.BlackStyleLabel();
		this.pThumbnail = new ShareX.TaskRoundedCornerPanel();
		this.lblError = new ShareX.HelpersLib.BlackStyleLabel();
		this.pbProgress = new ShareX.HelpersLib.BlackStyleProgressBar();
		this.pbThumbnail = new System.Windows.Forms.PictureBox();
		this.pThumbnail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbThumbnail).BeginInit();
		base.SuspendLayout();
		this.ttMain.AutoPopDelay = 5000;
		this.ttMain.InitialDelay = 200;
		this.ttMain.OwnerDraw = true;
		this.ttMain.ReshowDelay = 100;
		this.ttMain.Draw += new System.Windows.Forms.DrawToolTipEventHandler(TtMain_Draw);
		this.lblTitle.AutoEllipsis = true;
		this.lblTitle.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(this.lblTitle, "lblTitle");
		this.lblTitle.ForeColor = System.Drawing.Color.White;
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTitle.MouseClick += new System.Windows.Forms.MouseEventHandler(LblTitle_MouseClick);
		this.pThumbnail.BackColor = System.Drawing.Color.Transparent;
		this.pThumbnail.Controls.Add(this.lblError);
		this.pThumbnail.Controls.Add(this.pbProgress);
		this.pThumbnail.Controls.Add(this.pbThumbnail);
		componentResourceManager.ApplyResources(this.pThumbnail, "pThumbnail");
		this.pThumbnail.Name = "pThumbnail";
		this.pThumbnail.PanelColor = System.Drawing.Color.FromArgb(28, 32, 38);
		this.pThumbnail.Radius = 5f;
		this.pThumbnail.Selected = false;
		this.pThumbnail.StatusLocation = ShareX.ThumbnailTitleLocation.Top;
		componentResourceManager.ApplyResources(this.lblError, "lblError");
		this.lblError.BackColor = System.Drawing.Color.FromArgb(180, 0, 0);
		this.lblError.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblError.ForeColor = System.Drawing.Color.White;
		this.lblError.Name = "lblError";
		this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblError.MouseClick += new System.Windows.Forms.MouseEventHandler(lblError_MouseClick);
		componentResourceManager.ApplyResources(this.pbProgress, "pbProgress");
		this.pbProgress.ForeColor = System.Drawing.Color.White;
		this.pbProgress.Name = "pbProgress";
		this.pbProgress.ShowPercentageText = true;
		this.pbThumbnail.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(this.pbThumbnail, "pbThumbnail");
		this.pbThumbnail.Name = "pbThumbnail";
		this.pbThumbnail.TabStop = false;
		this.pbThumbnail.MouseClick += new System.Windows.Forms.MouseEventHandler(PbThumbnail_MouseClick);
		this.pbThumbnail.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(pbThumbnail_MouseDoubleClick);
		this.pbThumbnail.MouseDown += new System.Windows.Forms.MouseEventHandler(PbThumbnail_MouseDown);
		this.pbThumbnail.MouseMove += new System.Windows.Forms.MouseEventHandler(PbThumbnail_MouseMove);
		this.pbThumbnail.MouseUp += new System.Windows.Forms.MouseEventHandler(PbThumbnail_MouseUp);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.Transparent;
		base.Controls.Add(this.pThumbnail);
		base.Controls.Add(this.lblTitle);
		base.Name = "TaskThumbnailPanel";
		this.pThumbnail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pbThumbnail).EndInit();
		base.ResumeLayout(false);
	}
}
