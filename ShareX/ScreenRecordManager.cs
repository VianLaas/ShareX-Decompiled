using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;

namespace ShareX;

public static class ScreenRecordManager
{
	private static ScreenRecorder screenRecorder;

	private static ScreenRecordForm recordForm;

	public static bool IsRecording { get; private set; }

	public static void StartStopRecording(ScreenRecordOutput outputType, ScreenRecordStartMethod startMethod, TaskSettings taskSettings)
	{
		if (IsRecording)
		{
			if (recordForm != null && !recordForm.IsDisposed)
			{
				recordForm.StartStopRecording();
			}
		}
		else
		{
			StartRecording(outputType, taskSettings, startMethod);
		}
	}

	public static void StopRecording()
	{
		if (IsRecording && screenRecorder != null)
		{
			screenRecorder.StopRecording();
		}
	}

	public static void AbortRecording()
	{
		if (IsRecording && recordForm != null && !recordForm.IsDisposed)
		{
			recordForm.AbortRecording();
		}
	}

	private static void StartRecording(ScreenRecordOutput outputType, TaskSettings taskSettings, ScreenRecordStartMethod startMethod = ScreenRecordStartMethod.Region)
	{
		if (outputType == ScreenRecordOutput.GIF)
		{
			taskSettings.CaptureSettings.FFmpegOptions.VideoCodec = FFmpegVideoCodec.gif;
		}
		if (taskSettings.CaptureSettings.FFmpegOptions.IsAnimatedImage)
		{
			taskSettings.CaptureSettings.ScreenRecordTwoPassEncoding = true;
		}
		int fps;
		if (taskSettings.CaptureSettings.FFmpegOptions.VideoCodec == FFmpegVideoCodec.gif)
		{
			fps = taskSettings.CaptureSettings.GIFFPS;
		}
		else
		{
			fps = taskSettings.CaptureSettings.ScreenRecordFPS;
		}
		DebugHelper.WriteLine("Starting screen recording. Video encoder: \"{0}\", Audio encoder: \"{1}\", FPS: {2}", taskSettings.CaptureSettings.FFmpegOptions.VideoCodec.GetDescription(), taskSettings.CaptureSettings.FFmpegOptions.AudioCodec.GetDescription(), fps);
		if (!TaskHelpers.CheckFFmpeg(taskSettings))
		{
			return;
		}
		if (!taskSettings.CaptureSettings.FFmpegOptions.IsSourceSelected)
		{
			MessageBox.Show(Resources.FFmpeg_FFmpeg_video_and_audio_source_both_can_t_be__None__, "ShareX - " + Resources.FFmpeg_FFmpeg_error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (taskSettings.GeneralSettings.ToastWindowAutoHide)
		{
			NotificationForm.CloseActiveForm();
		}
		Rectangle captureRectangle = Rectangle.Empty;
		TaskMetadata metadata = new TaskMetadata();
		switch (startMethod)
		{
		case ScreenRecordStartMethod.Region:
		{
			if (taskSettings.CaptureSettings.ScreenRecordTransparentRegion)
			{
				RegionCaptureTasks.GetRectangleRegionTransparent(out captureRectangle);
				break;
			}
			RegionCaptureTasks.GetRectangleRegion(out captureRectangle, out var windowInfo, taskSettings.CaptureSettings.SurfaceOptions);
			metadata.UpdateInfo(windowInfo);
			break;
		}
		case ScreenRecordStartMethod.ActiveWindow:
			if (taskSettings.CaptureSettings.CaptureClientArea)
			{
				captureRectangle = CaptureHelpers.GetActiveWindowClientRectangle();
			}
			else
			{
				captureRectangle = CaptureHelpers.GetActiveWindowRectangle();
			}
			break;
		case ScreenRecordStartMethod.CustomRegion:
			captureRectangle = taskSettings.CaptureSettings.CaptureCustomRegion;
			break;
		case ScreenRecordStartMethod.LastRegion:
			captureRectangle = Program.Settings.ScreenRecordRegion;
			break;
		}
		Rectangle screenBounds = CaptureHelpers.GetScreenBounds();
		captureRectangle = Rectangle.Intersect(captureRectangle, screenBounds);
		if (taskSettings.CaptureSettings.FFmpegOptions.IsEvenSizeRequired)
		{
			captureRectangle = CaptureHelpers.EvenRectangleSize(captureRectangle);
		}
		if (IsRecording || !captureRectangle.IsValid() || screenRecorder != null)
		{
			return;
		}
		Program.Settings.ScreenRecordRegion = captureRectangle;
		IsRecording = true;
		string path = "";
		bool abortRequested = false;
		float duration = (taskSettings.CaptureSettings.ScreenRecordFixedDuration ? taskSettings.CaptureSettings.ScreenRecordDuration : 0f);
		recordForm = new ScreenRecordForm(captureRectangle)
		{
			ActivateWindow = (startMethod == ScreenRecordStartMethod.Region),
			Duration = duration,
			AskConfirmationOnAbort = taskSettings.CaptureSettings.ScreenRecordAskConfirmationOnAbort
		};
		recordForm.StopRequested += StopRecording;
		recordForm.Show();
		int delay;
		Task.Run(delegate
		{
			try
			{
				string extension2 = ((!taskSettings.CaptureSettings.ScreenRecordTwoPassEncoding) ? taskSettings.CaptureSettings.FFmpegOptions.Extension : "mp4");
				string screenshotsFolder = TaskHelpers.GetScreenshotsFolder(taskSettings, metadata);
				string fileName2 = TaskHelpers.GetFileName(taskSettings, extension2, metadata);
				path = TaskHelpers.HandleExistsFile(screenshotsFolder, fileName2, taskSettings);
				if (string.IsNullOrEmpty(path))
				{
					abortRequested = true;
				}
				if (!abortRequested)
				{
					recordForm.ChangeState(ScreenRecordState.BeforeStart);
					if (taskSettings.CaptureSettings.ScreenRecordAutoStart)
					{
						delay = (int)(taskSettings.CaptureSettings.ScreenRecordStartDelay * 1000f);
						if (delay > 0)
						{
							recordForm.InvokeSafe(delegate
							{
								recordForm.StartCountdown(delay);
							});
							recordForm.RecordResetEvent.WaitOne(delay);
						}
					}
					else
					{
						recordForm.RecordResetEvent.WaitOne();
					}
					if (recordForm.IsAbortRequested)
					{
						abortRequested = true;
					}
					if (!abortRequested)
					{
						ScreenRecordingOptions options = new ScreenRecordingOptions
						{
							IsRecording = true,
							IsLossless = taskSettings.CaptureSettings.ScreenRecordTwoPassEncoding,
							FFmpeg = taskSettings.CaptureSettings.FFmpegOptions,
							FPS = fps,
							Duration = duration,
							OutputPath = path,
							CaptureArea = captureRectangle,
							DrawCursor = taskSettings.CaptureSettings.ScreenRecordShowCursor
						};
						Screenshot screenshot = TaskHelpers.GetScreenshot(taskSettings);
						screenshot.CaptureCursor = taskSettings.CaptureSettings.ScreenRecordShowCursor;
						screenRecorder = new ScreenRecorder(ScreenRecordOutput.FFmpeg, options, screenshot, captureRectangle);
						screenRecorder.RecordingStarted += ScreenRecorder_RecordingStarted;
						screenRecorder.EncodingProgressChanged += ScreenRecorder_EncodingProgressChanged;
						recordForm.ChangeState(ScreenRecordState.AfterStart);
						screenRecorder.StartRecording();
						if (recordForm.IsAbortRequested)
						{
							abortRequested = true;
						}
					}
				}
			}
			catch (Exception exception)
			{
				DebugHelper.WriteException(exception);
			}
			if (taskSettings.CaptureSettings.ScreenRecordTwoPassEncoding && !abortRequested && screenRecorder != null && File.Exists(path))
			{
				recordForm.ChangeState(ScreenRecordState.Encoding);
				path = ProcessTwoPassEncoding(path, metadata, taskSettings);
			}
			if (recordForm != null)
			{
				recordForm.InvokeSafe(delegate
				{
					recordForm.Close();
					recordForm.Dispose();
					recordForm = null;
				});
			}
			if (screenRecorder != null)
			{
				screenRecorder.Dispose();
				screenRecorder = null;
				if (abortRequested && !string.IsNullOrEmpty(path) && File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}).ContinueInCurrentContext(delegate
		{
			if (!abortRequested && !string.IsNullOrEmpty(path) && File.Exists(path) && TaskHelpers.ShowAfterCaptureForm(taskSettings, out var fileName, null, path))
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
					string extension = Path.GetExtension(path);
					if (!fileNameWithoutExtension.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
					{
						path = FileHelpers.RenameFile(path, fileName + extension);
					}
				}
				TaskManager.Start(WorkerTask.CreateFileJobTask(path, metadata, taskSettings, fileName));
			}
			abortRequested = false;
			IsRecording = false;
		});
	}

	private static void ScreenRecorder_RecordingStarted()
	{
		recordForm.ChangeState(ScreenRecordState.AfterRecordingStart);
	}

	private static void ScreenRecorder_EncodingProgressChanged(int progress)
	{
		recordForm.ChangeStateProgress(progress);
	}

	private static string ProcessTwoPassEncoding(string input, TaskMetadata metadata, TaskSettings taskSettings, bool deleteInputFile = true)
	{
		string screenshotsFolder = TaskHelpers.GetScreenshotsFolder(taskSettings, metadata);
		string fileName = TaskHelpers.GetFileName(taskSettings, taskSettings.CaptureSettings.FFmpegOptions.Extension, metadata);
		string text = Path.Combine(screenshotsFolder, fileName);
		try
		{
			if (taskSettings.CaptureSettings.FFmpegOptions.VideoCodec == FFmpegVideoCodec.gif)
			{
				screenRecorder.FFmpegEncodeAsGIF(input, text);
			}
			else
			{
				screenRecorder.FFmpegEncodeVideo(input, text);
			}
		}
		finally
		{
			if (deleteInputFile && !input.Equals(text, StringComparison.InvariantCultureIgnoreCase) && File.Exists(input))
			{
				File.Delete(input);
			}
		}
		return text;
	}
}
