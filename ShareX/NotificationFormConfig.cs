using System;
using System.Drawing;

namespace ShareX;

public class NotificationFormConfig : IDisposable
{
	public int Duration { get; set; }

	public int FadeDuration { get; set; }

	public ContentAlignment Placement { get; set; }

	public int Offset { get; set; } = 5;


	public Size Size { get; set; }

	public bool IsValid
	{
		get
		{
			if ((Duration > 0 || FadeDuration > 0) && Size.Width > 0)
			{
				return Size.Height > 0;
			}
			return false;
		}
	}

	public Color BackgroundColor { get; set; } = Color.FromArgb(50, 50, 50);


	public Color BorderColor { get; set; } = Color.FromArgb(40, 40, 40);


	public int TextPadding { get; set; } = 10;


	public Font TextFont { get; set; } = new Font("Arial", 11f);


	public Color TextColor { get; set; } = Color.FromArgb(210, 210, 210);


	public Font TitleFont { get; set; } = new Font("Arial", 11f, FontStyle.Bold);


	public Color TitleColor { get; set; } = Color.FromArgb(240, 240, 240);


	public Bitmap Image { get; set; }

	public string Title { get; set; }

	public string Text { get; set; }

	public string FilePath { get; set; }

	public string URL { get; set; }

	public ToastClickAction LeftClickAction { get; set; }

	public ToastClickAction RightClickAction { get; set; }

	public ToastClickAction MiddleClickAction { get; set; }

	public void Dispose()
	{
		TextFont?.Dispose();
		TitleFont?.Dispose();
		Image?.Dispose();
	}
}
