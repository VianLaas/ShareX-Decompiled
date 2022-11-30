using System.Drawing;
using ShareX.HelpersLib;

namespace ShareX;

public class CaptureActiveMonitor : CaptureBase
{
	protected override TaskMetadata Execute(TaskSettings taskSettings)
	{
		Rectangle activeScreenWorkingArea = CaptureHelpers.GetActiveScreenWorkingArea();
		TaskMetadata taskMetadata = CreateMetadata(activeScreenWorkingArea);
		taskMetadata.Image = TaskHelpers.GetScreenshot(taskSettings).CaptureActiveMonitor();
		return taskMetadata;
	}
}
