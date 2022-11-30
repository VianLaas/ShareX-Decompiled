using System.Drawing;

namespace ShareX;

public class CaptureMonitor : CaptureBase
{
	public Rectangle MonitorRectangle { get; private set; }

	public CaptureMonitor(Rectangle monitorRectangle)
	{
		MonitorRectangle = monitorRectangle;
	}

	protected override TaskMetadata Execute(TaskSettings taskSettings)
	{
		TaskMetadata taskMetadata = CreateMetadata(MonitorRectangle);
		taskMetadata.Image = TaskHelpers.GetScreenshot().CaptureRectangle(MonitorRectangle);
		return taskMetadata;
	}
}
