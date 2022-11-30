using System.Drawing;
using ShareX.HelpersLib;
using ShareX.ScreenCaptureLib;

namespace ShareX;

public class CaptureLastRegion : CaptureRegion
{
	protected override TaskMetadata Execute(TaskSettings taskSettings)
	{
		switch (CaptureRegion.lastRegionCaptureType)
		{
		default:
			if (RegionCaptureForm.LastRegionFillPath != null)
			{
				using (Bitmap bmp3 = TaskHelpers.GetScreenshot(taskSettings).CaptureFullscreen())
				{
					Rectangle resultArea;
					return new TaskMetadata(RegionCaptureTasks.ApplyRegionPathToImage(bmp3, RegionCaptureForm.LastRegionFillPath, out resultArea));
				}
			}
			return ExecuteRegionCapture(taskSettings);
		case RegionCaptureType.Light:
			if (!RegionCaptureLightForm.LastSelectionRectangle0Based.IsEmpty)
			{
				using (Bitmap bmp2 = TaskHelpers.GetScreenshot(taskSettings).CaptureFullscreen())
				{
					return new TaskMetadata(ImageHelpers.CropBitmap(bmp2, RegionCaptureLightForm.LastSelectionRectangle0Based));
				}
			}
			return ExecuteRegionCaptureLight(taskSettings);
		case RegionCaptureType.Transparent:
			if (!RegionCaptureTransparentForm.LastSelectionRectangle0Based.IsEmpty)
			{
				using (Bitmap bmp = TaskHelpers.GetScreenshot(taskSettings).CaptureFullscreen())
				{
					return new TaskMetadata(ImageHelpers.CropBitmap(bmp, RegionCaptureTransparentForm.LastSelectionRectangle0Based));
				}
			}
			return ExecuteRegionCaptureTransparent(taskSettings);
		}
	}
}
