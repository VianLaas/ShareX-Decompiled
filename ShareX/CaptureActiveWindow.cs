namespace ShareX;

public class CaptureActiveWindow : CaptureBase
{
	protected override TaskMetadata Execute(TaskSettings taskSettings)
	{
		TaskMetadata taskMetadata = CreateMetadata();
		if (taskSettings.CaptureSettings.CaptureTransparent && !taskSettings.CaptureSettings.CaptureClientArea)
		{
			taskMetadata.Image = TaskHelpers.GetScreenshot(taskSettings).CaptureActiveWindowTransparent();
		}
		else
		{
			taskMetadata.Image = TaskHelpers.GetScreenshot(taskSettings).CaptureActiveWindow();
		}
		return taskMetadata;
	}
}
