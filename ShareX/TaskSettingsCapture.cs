using System.Drawing;
using ShareX.ScreenCaptureLib;

namespace ShareX;

public class TaskSettingsCapture
{
	public bool ShowCursor = true;

	public decimal ScreenshotDelay;

	public bool CaptureTransparent;

	public bool CaptureShadow = true;

	public int CaptureShadowOffset = 100;

	public bool CaptureClientArea;

	public bool CaptureAutoHideTaskbar;

	public Rectangle CaptureCustomRegion = new Rectangle(0, 0, 0, 0);

	public RegionCaptureOptions SurfaceOptions = new RegionCaptureOptions();

	public FFmpegOptions FFmpegOptions = new FFmpegOptions(Program.DefaultFFmpegFilePath);

	public int ScreenRecordFPS = 30;

	public int GIFFPS = 15;

	public bool ScreenRecordShowCursor = true;

	public bool ScreenRecordAutoStart = true;

	public float ScreenRecordStartDelay;

	public bool ScreenRecordFixedDuration;

	public float ScreenRecordDuration = 3f;

	public bool ScreenRecordTwoPassEncoding;

	public bool ScreenRecordAskConfirmationOnAbort;

	public bool ScreenRecordTransparentRegion;

	public ScrollingCaptureOptions ScrollingCaptureOptions = new ScrollingCaptureOptions();

	public OCROptions OCROptions = new OCROptions();
}
