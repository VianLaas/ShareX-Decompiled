using System;
using System.Threading;
using ShareX.HelpersLib;

namespace ShareX;

public class CaptureWindow : CaptureBase
{
	public IntPtr WindowHandle { get; private set; }

	public CaptureWindow(IntPtr windowHandle)
	{
		WindowHandle = windowHandle;
		base.AllowAutoHideForm = WindowHandle != Program.MainForm.Handle;
	}

	protected override TaskMetadata Execute(TaskSettings taskSettings)
	{
		WindowInfo windowInfo = new WindowInfo(WindowHandle);
		if (windowInfo.IsMinimized)
		{
			windowInfo.Restore();
		}
		windowInfo.Activate();
		Thread.Sleep(250);
		TaskMetadata taskMetadata = new TaskMetadata();
		taskMetadata.UpdateInfo(windowInfo);
		if (taskSettings.CaptureSettings.CaptureTransparent && !taskSettings.CaptureSettings.CaptureClientArea)
		{
			taskMetadata.Image = TaskHelpers.GetScreenshot(taskSettings).CaptureWindowTransparent(WindowHandle);
		}
		else
		{
			taskMetadata.Image = TaskHelpers.GetScreenshot(taskSettings).CaptureWindow(WindowHandle);
		}
		return taskMetadata;
	}
}
