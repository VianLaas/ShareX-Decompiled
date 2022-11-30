using System.Drawing;
using ShareX.HelpersLib;

namespace ShareX;

public class CaptureFullscreen : CaptureBase
{
	protected override TaskMetadata Execute(TaskSettings taskSettings)
	{
		Rectangle screenWorkingArea = CaptureHelpers.GetScreenWorkingArea();
		TaskMetadata taskMetadata = CreateMetadata(screenWorkingArea);
		taskMetadata.Image = TaskHelpers.GetScreenshot(taskSettings).CaptureFullscreen();
		return taskMetadata;
	}
}
