using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.ScreenCaptureLib;

namespace ShareX;

public class CaptureRegion : CaptureBase
{
	protected static RegionCaptureType lastRegionCaptureType;

	public RegionCaptureType RegionCaptureType { get; protected set; }

	public CaptureRegion()
	{
	}

	public CaptureRegion(RegionCaptureType regionCaptureType)
	{
		RegionCaptureType = regionCaptureType;
	}

	protected override TaskMetadata Execute(TaskSettings taskSettings)
	{
		return RegionCaptureType switch
		{
			RegionCaptureType.Light => ExecuteRegionCaptureLight(taskSettings), 
			RegionCaptureType.Transparent => ExecuteRegionCaptureTransparent(taskSettings), 
			_ => ExecuteRegionCapture(taskSettings), 
		};
	}

	protected TaskMetadata ExecuteRegionCapture(TaskSettings taskSettings)
	{
		RegionCaptureMode mode = ((!taskSettings.AdvancedSettings.RegionCaptureDisableAnnotation) ? RegionCaptureMode.Annotation : RegionCaptureMode.Default);
		Screenshot screenshot = TaskHelpers.GetScreenshot(taskSettings);
		screenshot.CaptureCursor = false;
		Bitmap canvas = ((!taskSettings.CaptureSettings.SurfaceOptions.ActiveMonitorMode) ? screenshot.CaptureFullscreen() : screenshot.CaptureActiveMonitor());
		CursorData cursorData = null;
		if (taskSettings.CaptureSettings.ShowCursor)
		{
			cursorData = new CursorData();
		}
		using (RegionCaptureForm regionCaptureForm = new RegionCaptureForm(mode, taskSettings.CaptureSettingsReference.SurfaceOptions, canvas))
		{
			if (cursorData != null && cursorData.IsVisible)
			{
				regionCaptureForm.AddCursor(cursorData.ToBitmap(), regionCaptureForm.PointToClient(cursorData.DrawPosition));
			}
			regionCaptureForm.ShowDialog();
			Bitmap resultImage = regionCaptureForm.GetResultImage();
			if (resultImage != null)
			{
				TaskMetadata taskMetadata = new TaskMetadata(resultImage);
				if (regionCaptureForm.IsImageModified)
				{
					base.AllowAnnotation = false;
				}
				if (regionCaptureForm.Result == RegionResult.Region)
				{
					WindowInfo windowInfo = regionCaptureForm.GetWindowInfo();
					taskMetadata.UpdateInfo(windowInfo);
				}
				lastRegionCaptureType = RegionCaptureType.Default;
				return taskMetadata;
			}
		}
		return null;
	}

	protected TaskMetadata ExecuteRegionCaptureLight(TaskSettings taskSettings)
	{
		Screenshot screenshot = TaskHelpers.GetScreenshot(taskSettings);
		Bitmap canvas = ((!taskSettings.CaptureSettings.SurfaceOptions.ActiveMonitorMode) ? screenshot.CaptureFullscreen() : screenshot.CaptureActiveMonitor());
		bool activeMonitorMode = taskSettings.CaptureSettings.SurfaceOptions.ActiveMonitorMode;
		using (RegionCaptureLightForm regionCaptureLightForm = new RegionCaptureLightForm(canvas, activeMonitorMode))
		{
			if (regionCaptureLightForm.ShowDialog() == DialogResult.OK)
			{
				Bitmap areaImage = regionCaptureLightForm.GetAreaImage();
				if (areaImage != null)
				{
					lastRegionCaptureType = RegionCaptureType.Light;
					return new TaskMetadata(areaImage);
				}
			}
		}
		return null;
	}

	protected TaskMetadata ExecuteRegionCaptureTransparent(TaskSettings taskSettings)
	{
		using (RegionCaptureTransparentForm regionCaptureTransparentForm = new RegionCaptureTransparentForm(taskSettings.CaptureSettings.SurfaceOptions.ActiveMonitorMode))
		{
			if (regionCaptureTransparentForm.ShowDialog() == DialogResult.OK)
			{
				Screenshot screenshot = TaskHelpers.GetScreenshot(taskSettings);
				Bitmap areaImage = regionCaptureTransparentForm.GetAreaImage(screenshot);
				if (areaImage != null)
				{
					lastRegionCaptureType = RegionCaptureType.Transparent;
					return new TaskMetadata(areaImage);
				}
			}
		}
		return null;
	}
}
