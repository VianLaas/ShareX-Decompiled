using System;
using System.Drawing;
using ShareX.HelpersLib;

namespace ShareX;

public class TaskMetadata : IDisposable
{
	private const int WindowInfoMaxLength = 255;

	private string windowTitle;

	private string processName;

	public Bitmap Image { get; set; }

	public string WindowTitle
	{
		get
		{
			return windowTitle;
		}
		set
		{
			windowTitle = value.Truncate(255);
		}
	}

	public string ProcessName
	{
		get
		{
			return processName;
		}
		set
		{
			processName = value.Truncate(255);
		}
	}

	public TaskMetadata()
	{
	}

	public TaskMetadata(Bitmap image)
	{
		Image = image;
	}

	public void UpdateInfo(WindowInfo windowInfo)
	{
		if (windowInfo != null)
		{
			WindowTitle = windowInfo.Text;
			ProcessName = windowInfo.ProcessName;
		}
	}

	public void Dispose()
	{
		Image?.Dispose();
	}
}
