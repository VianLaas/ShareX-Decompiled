using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class TaskRoundedCornerPanel : RoundedCornerPanel
{
	private bool selected;

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
				Invalidate();
			}
		}
	}

	public Color StatusColor { get; private set; } = Color.Transparent;


	public ThumbnailTitleLocation StatusLocation { get; set; }

	public void UpdateStatusColor(TaskStatus status)
	{
		Color statusColor = StatusColor;
		switch (status)
		{
		case TaskStatus.Stopped:
		case TaskStatus.Completed:
			StatusColor = Color.CornflowerBlue;
			break;
		case TaskStatus.Failed:
			StatusColor = Color.Red;
			break;
		case TaskStatus.History:
			StatusColor = Color.Transparent;
			break;
		default:
			StatusColor = Color.PaleGreen;
			break;
		}
		if (statusColor != StatusColor)
		{
			Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		Graphics graphics = e.Graphics;
		if (Selected)
		{
			graphics.PixelOffsetMode = PixelOffsetMode.Default;
			using Pen pen = new Pen(ShareXResources.Theme.TextColor)
			{
				DashStyle = DashStyle.Dot
			};
			graphics.DrawRoundedRectangle(pen, base.ClientRectangle, base.Radius);
		}
		if (StatusColor.A <= 0)
		{
			return;
		}
		graphics.PixelOffsetMode = PixelOffsetMode.Half;
		int num = ((StatusLocation != 0) ? base.ClientRectangle.Height : 0);
		using LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, base.ClientRectangle.Width, 1), Color.Black, Color.Black, LinearGradientMode.Horizontal);
		ColorBlend colorBlend = new ColorBlend();
		colorBlend.Positions = new float[4] { 0f, 0.3f, 0.7f, 1f };
		colorBlend.Colors = new Color[4]
		{
			Color.Transparent,
			StatusColor,
			StatusColor,
			Color.Transparent
		};
		linearGradientBrush.InterpolationColors = colorBlend;
		using Pen pen2 = new Pen(linearGradientBrush);
		graphics.DrawLine(pen2, new Point(0, num), new Point(base.ClientRectangle.Width - 1, num));
	}
}
