using System.Drawing;

namespace ShareX;

public class CaptureCustomRegion : CaptureBase
{
	protected override TaskMetadata Execute(TaskSettings taskSettings)
	{
		Rectangle captureCustomRegion = taskSettings.CaptureSettings.CaptureCustomRegion;
		TaskMetadata taskMetadata = CreateMetadata(captureCustomRegion);
		taskMetadata.Image = TaskHelpers.GetScreenshot(taskSettings).CaptureRectangle(captureCustomRegion);
		return taskMetadata;
	}
}
