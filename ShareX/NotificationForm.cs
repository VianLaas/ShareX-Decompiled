using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class NotificationForm : LayeredForm
{
	private static NotificationForm instance;

	private bool isMouseInside;

	private bool isDurationEnd;

	private int fadeInterval = 50;

	private float opacityDecrement;

	private int urlPadding = 3;

	private int titleSpace = 3;

	private Size titleRenderSize;

	private Size textRenderSize;

	private Size totalRenderSize;

	private bool isMouseDragging;

	private Point dragStart;

	private float opacity = 255f;

	private Bitmap buffer;

	private Graphics gBuffer;

	private Timer tDuration;

	private Timer tOpacity;

	private IContainer components;

	public NotificationFormConfig Config { get; private set; }

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams obj = base.CreateParams;
			obj.ExStyle |= 128;
			return obj;
		}
	}

	private NotificationForm()
	{
		InitializeComponent();
	}

	public static void Show(NotificationFormConfig config)
	{
		if (!config.IsValid)
		{
			return;
		}
		if (config.Image == null)
		{
			config.Image = ImageHelpers.LoadImage(config.FilePath);
		}
		if (config.Image != null || !string.IsNullOrEmpty(config.Text))
		{
			if (instance == null || instance.IsDisposed)
			{
				instance = new NotificationForm();
				instance.LoadConfig(config);
				NativeMethods.ShowWindow(instance.Handle, 8);
			}
			else
			{
				instance.LoadConfig(config);
			}
		}
	}

	public static void CloseActiveForm()
	{
		if (instance != null && !instance.IsDisposed)
		{
			instance.Close();
		}
	}

	public void LoadConfig(NotificationFormConfig config)
	{
		Config?.Dispose();
		buffer?.Dispose();
		gBuffer?.Dispose();
		Config = config;
		opacityDecrement = (float)fadeInterval / (float)Config.FadeDuration * 255f;
		if (Config.Image != null)
		{
			Config.Image = ImageHelpers.ResizeImageLimit(Config.Image, Config.Size);
			Config.Size = new Size(Config.Image.Width + 2, Config.Image.Height + 2);
		}
		else if (!string.IsNullOrEmpty(Config.Text))
		{
			Size proposedSize = Config.Size.Offset(-Config.TextPadding * 2);
			textRenderSize = TextRenderer.MeasureText(Config.Text, Config.TextFont, proposedSize, TextFormatFlags.EndEllipsis | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
			textRenderSize = new Size(textRenderSize.Width, Math.Min(textRenderSize.Height, proposedSize.Height));
			totalRenderSize = textRenderSize;
			if (!string.IsNullOrEmpty(Config.Title))
			{
				titleRenderSize = TextRenderer.MeasureText(Config.Title, Config.TitleFont, Config.Size.Offset(-Config.TextPadding * 2), TextFormatFlags.EndEllipsis);
				totalRenderSize = new Size(Math.Max(textRenderSize.Width, titleRenderSize.Width), titleRenderSize.Height + titleSpace + textRenderSize.Height);
			}
			Config.Size = new Size(totalRenderSize.Width + Config.TextPadding * 2, totalRenderSize.Height + Config.TextPadding * 2 + 2);
		}
		buffer = new Bitmap(Config.Size.Width, Config.Size.Height);
		gBuffer = Graphics.FromImage(buffer);
		Point position = Helpers.GetPosition(Config.Placement, Config.Offset, Screen.PrimaryScreen.WorkingArea.Size, Config.Size);
		NativeMethods.SetWindowPos(base.Handle, (IntPtr)(-1), position.X + Screen.PrimaryScreen.WorkingArea.X, position.Y + Screen.PrimaryScreen.WorkingArea.Y, Config.Size.Width, Config.Size.Height, SetWindowPosFlags.SWP_NOACTIVATE);
		tDuration.Stop();
		tOpacity.Stop();
		opacity = 255f;
		Render(updateBuffer: true);
		if (Config.Duration <= 0)
		{
			DurationEnd();
			return;
		}
		tDuration.Interval = Config.Duration;
		tDuration.Start();
	}

	private void UpdateBuffer()
	{
		Rectangle rectangle = new Rectangle(0, 0, buffer.Width, buffer.Height);
		gBuffer.Clear(Config.BackgroundColor);
		if (Config.Image != null)
		{
			gBuffer.DrawImage(Config.Image, 1, 1, Config.Image.Width, Config.Image.Height);
			if (isMouseInside && !string.IsNullOrEmpty(Config.URL))
			{
				Rectangle rect = new Rectangle(0, 0, rectangle.Width, 40);
				using (SolidBrush brush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
				{
					gBuffer.FillRectangle(brush, rect);
				}
				TextRenderer.DrawText(gBuffer, Config.URL, Config.TextFont, rect.Offset(-urlPadding), Color.White, TextFormatFlags.EndEllipsis);
			}
		}
		else if (!string.IsNullOrEmpty(Config.Text))
		{
			Rectangle bounds2;
			if (!string.IsNullOrEmpty(Config.Title))
			{
				Rectangle bounds = new Rectangle(Config.TextPadding, Config.TextPadding, titleRenderSize.Width + 2, titleRenderSize.Height + 2);
				TextRenderer.DrawText(gBuffer, Config.Title, Config.TitleFont, bounds, Config.TitleColor, TextFormatFlags.EndEllipsis);
				bounds2 = new Rectangle(Config.TextPadding, Config.TextPadding + bounds.Height + titleSpace, textRenderSize.Width + 2, textRenderSize.Height + 2);
			}
			else
			{
				bounds2 = new Rectangle(Config.TextPadding, Config.TextPadding, textRenderSize.Width + 2, textRenderSize.Height + 2);
			}
			TextRenderer.DrawText(gBuffer, Config.Text, Config.TextFont, bounds2, Config.TextColor, TextFormatFlags.EndEllipsis | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
		}
		using Pen pen = new Pen(Config.BorderColor);
		gBuffer.DrawRectangleProper(pen, rectangle);
	}

	private void Render(bool updateBuffer)
	{
		if (updateBuffer)
		{
			UpdateBuffer();
		}
		SelectBitmap(buffer, (int)opacity);
	}

	private void DurationEnd()
	{
		isDurationEnd = true;
		tDuration.Stop();
		if (!isMouseInside)
		{
			StartFade();
		}
	}

	private void StartFade()
	{
		if (Config.FadeDuration <= 0)
		{
			Close();
			return;
		}
		opacity = 255f;
		Render(updateBuffer: false);
		tOpacity.Interval = fadeInterval;
		tOpacity.Start();
	}

	private void tDuration_Tick(object sender, EventArgs e)
	{
		DurationEnd();
	}

	private void tOpacity_Tick(object sender, EventArgs e)
	{
		if (opacity > opacityDecrement)
		{
			opacity -= opacityDecrement;
			Render(updateBuffer: false);
		}
		else
		{
			Close();
		}
	}

	private void NotificationForm_MouseEnter(object sender, EventArgs e)
	{
		isMouseInside = true;
		tOpacity.Stop();
		if (!base.IsDisposed)
		{
			opacity = 255f;
			Render(updateBuffer: true);
		}
	}

	private void NotificationForm_MouseLeave(object sender, EventArgs e)
	{
		isMouseInside = false;
		isMouseDragging = false;
		Render(updateBuffer: true);
		if (isDurationEnd)
		{
			StartFade();
		}
	}

	private void NotificationForm_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			dragStart = e.Location;
			isMouseDragging = true;
		}
	}

	private void NotificationForm_MouseMove(object sender, MouseEventArgs e)
	{
		if (isMouseDragging)
		{
			int num = 20;
			if (!new Rectangle(dragStart.X - num, dragStart.Y - num, num * 2, num * 2).Contains(e.Location) && !string.IsNullOrEmpty(Config.FilePath) && File.Exists(Config.FilePath))
			{
				IDataObject data = new DataObject(DataFormats.FileDrop, new string[1] { Config.FilePath });
				DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move);
				isMouseDragging = false;
			}
		}
	}

	private void NotificationForm_MouseUp(object sender, MouseEventArgs e)
	{
		isMouseDragging = false;
	}

	private void NotificationForm_MouseClick(object sender, MouseEventArgs e)
	{
		tDuration.Stop();
		Close();
		ToastClickAction action = ToastClickAction.CloseNotification;
		if (e.Button == MouseButtons.Left)
		{
			action = Config.LeftClickAction;
		}
		else if (e.Button == MouseButtons.Right)
		{
			action = Config.RightClickAction;
		}
		else if (e.Button == MouseButtons.Middle)
		{
			action = Config.MiddleClickAction;
		}
		ExecuteAction(action);
	}

	private void ExecuteAction(ToastClickAction action)
	{
		switch (action)
		{
		case ToastClickAction.AnnotateImage:
			if (!string.IsNullOrEmpty(Config.FilePath) && FileHelpers.IsImageFile(Config.FilePath))
			{
				TaskHelpers.AnnotateImageFromFile(Config.FilePath);
			}
			break;
		case ToastClickAction.CopyImageToClipboard:
			if (!string.IsNullOrEmpty(Config.FilePath))
			{
				ClipboardHelpers.CopyImageFromFile(Config.FilePath);
			}
			break;
		case ToastClickAction.CopyFile:
			if (!string.IsNullOrEmpty(Config.FilePath))
			{
				ClipboardHelpers.CopyFile(Config.FilePath);
			}
			break;
		case ToastClickAction.CopyFilePath:
			if (!string.IsNullOrEmpty(Config.FilePath))
			{
				ClipboardHelpers.CopyText(Config.FilePath);
			}
			break;
		case ToastClickAction.CopyUrl:
			if (!string.IsNullOrEmpty(Config.URL))
			{
				ClipboardHelpers.CopyText(Config.URL);
			}
			break;
		case ToastClickAction.OpenFile:
			if (!string.IsNullOrEmpty(Config.FilePath))
			{
				FileHelpers.OpenFile(Config.FilePath);
			}
			break;
		case ToastClickAction.OpenFolder:
			if (!string.IsNullOrEmpty(Config.FilePath))
			{
				FileHelpers.OpenFolderWithFile(Config.FilePath);
			}
			break;
		case ToastClickAction.OpenUrl:
			if (!string.IsNullOrEmpty(Config.URL))
			{
				URLHelpers.OpenURL(Config.URL);
			}
			break;
		case ToastClickAction.Upload:
			if (!string.IsNullOrEmpty(Config.FilePath))
			{
				UploadManager.UploadFile(Config.FilePath);
			}
			break;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		Config?.Dispose();
		buffer?.Dispose();
		gBuffer?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.tDuration = new System.Windows.Forms.Timer(this.components);
		this.tOpacity = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.tDuration.Tick += new System.EventHandler(tDuration_Tick);
		this.tOpacity.Tick += new System.EventHandler(tOpacity_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(400, 300);
		this.Cursor = System.Windows.Forms.Cursors.Hand;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "NotificationForm";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		this.Text = "NotificationForm";
		base.MouseClick += new System.Windows.Forms.MouseEventHandler(NotificationForm_MouseClick);
		base.MouseEnter += new System.EventHandler(NotificationForm_MouseEnter);
		base.MouseLeave += new System.EventHandler(NotificationForm_MouseLeave);
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(NotificationForm_MouseDown);
		base.MouseMove += new System.Windows.Forms.MouseEventHandler(NotificationForm_MouseMove);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(NotificationForm_MouseUp);
		base.ResumeLayout(false);
	}
}
