using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class DropForm : LayeredForm
{
	private static DropForm instance;

	private Bitmap backgroundImage;

	private bool isHovered;

	private TaskSettings taskSettings;

	private IContainer components;

	public int DropSize { get; set; }

	public int DropOffset { get; set; }

	public ContentAlignment DropAlignment { get; set; }

	public int DropOpacity { get; set; }

	public int DropHoverOpacity { get; set; }

	public static DropForm GetInstance(int size, int offset, ContentAlignment alignment, int opacity, int hoverOpacity, TaskSettings taskSettings = null)
	{
		if (instance == null || instance.IsDisposed)
		{
			instance = new DropForm(size, offset, alignment, opacity, hoverOpacity);
		}
		instance.taskSettings = taskSettings;
		return instance;
	}

	private DropForm(int size, int offset, ContentAlignment alignment, int opacity, int hoverOpacity)
	{
		InitializeComponent();
		DropSize = size.Clamp(10, 300);
		DropOffset = offset;
		DropAlignment = alignment;
		DropOpacity = opacity.Clamp(1, 255);
		DropHoverOpacity = hoverOpacity.Clamp(1, 255);
		backgroundImage = DrawDropImage(DropSize);
		base.Location = Helpers.GetPosition(DropAlignment, DropOffset, Screen.PrimaryScreen.WorkingArea.Size, backgroundImage.Size);
		SelectBitmap(backgroundImage, DropOpacity);
	}

	private Bitmap DrawDropImage(int size)
	{
		Bitmap bitmap = new Bitmap(size, size);
		Rectangle rectangle = new Rectangle(0, 0, size, size);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.FillRectangle(Brushes.CornflowerBlue, rectangle);
		graphics.DrawRectangleProper(Pens.Black, rectangle);
		using (Pen pen = new Pen(Color.WhiteSmoke, 5f)
		{
			Alignment = PenAlignment.Inset
		})
		{
			graphics.DrawRectangleProper(pen, rectangle.Offset(-1));
		}
		string dropForm_DrawDropImage_Drop_here = Resources.DropForm_DrawDropImage_Drop_here;
		using Font font = new Font("Arial", 20f, FontStyle.Bold);
		using StringFormat format = new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		};
		graphics.DrawString(dropForm_DrawDropImage_Drop_here, font, Brushes.Black, rectangle.LocationOffset(1), format);
		graphics.DrawString(dropForm_DrawDropImage_Drop_here, font, Brushes.White, rectangle, format);
		return bitmap;
	}

	private void DropForm_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			NativeMethods.ReleaseCapture();
			NativeMethods.SendMessage(base.Handle, 161u, (IntPtr)2, IntPtr.Zero);
		}
	}

	private void DropForm_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			Close();
		}
	}

	private void DropForm_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop, autoConvert: false) || e.Data.GetDataPresent(DataFormats.Bitmap, autoConvert: false) || e.Data.GetDataPresent(DataFormats.Text, autoConvert: false))
		{
			e.Effect = DragDropEffects.Copy;
			if (!isHovered)
			{
				SelectBitmap(backgroundImage, DropHoverOpacity);
				isHovered = true;
			}
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void DropForm_DragDrop(object sender, DragEventArgs e)
	{
		UploadManager.DragDropUpload(e.Data, taskSettings);
		if (isHovered)
		{
			SelectBitmap(backgroundImage, DropOpacity);
			isHovered = false;
		}
	}

	private void DropForm_DragLeave(object sender, EventArgs e)
	{
		if (isHovered)
		{
			SelectBitmap(backgroundImage, DropOpacity);
			isHovered = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		if (backgroundImage != null)
		{
			backgroundImage.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.AllowDrop = true;
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.Cursor = System.Windows.Forms.Cursors.SizeAll;
		this.Text = "DropForm";
		base.TopMost = true;
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(DropForm_MouseDown);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(DropForm_MouseUp);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(DropForm_DragEnter);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(DropForm_DragDrop);
		base.DragLeave += new System.EventHandler(DropForm_DragLeave);
	}
}
