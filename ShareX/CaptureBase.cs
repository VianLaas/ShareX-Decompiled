using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using ShareX.HelpersLib;

namespace ShareX;

public abstract class CaptureBase
{
	public bool AllowAutoHideForm { get; set; } = true;


	public bool AllowAnnotation { get; set; } = true;


	public void Capture(bool autoHideForm)
	{
		Capture(null, autoHideForm);
	}

	public void Capture(TaskSettings taskSettings = null, bool autoHideForm = false)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (taskSettings.GeneralSettings.ToastWindowAutoHide)
		{
			NotificationForm.CloseActiveForm();
		}
		if (taskSettings.CaptureSettings.ScreenshotDelay > 0m)
		{
			Task.Delay((int)(taskSettings.CaptureSettings.ScreenshotDelay * 1000m)).ContinueInCurrentContext(delegate
			{
				CaptureInternal(taskSettings, autoHideForm);
			});
		}
		else
		{
			CaptureInternal(taskSettings, autoHideForm);
		}
	}

	protected abstract TaskMetadata Execute(TaskSettings taskSettings);

	private void CaptureInternal(TaskSettings taskSettings, bool autoHideForm)
	{
		if (autoHideForm && AllowAutoHideForm)
		{
			Program.MainForm.Hide();
			Thread.Sleep(250);
		}
		TaskMetadata metadata = null;
		try
		{
			AllowAnnotation = true;
			metadata = Execute(taskSettings);
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		finally
		{
			if (autoHideForm && AllowAutoHideForm)
			{
				Program.MainForm.ForceActivate();
			}
			AfterCapture(metadata, taskSettings);
		}
	}

	private void AfterCapture(TaskMetadata metadata, TaskSettings taskSettings)
	{
		if (metadata != null && metadata.Image != null)
		{
			if (taskSettings.GeneralSettings.PlaySoundAfterCapture)
			{
				TaskHelpers.PlayCaptureSound(taskSettings);
			}
			if (taskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.AnnotateImage) && !AllowAnnotation)
			{
				taskSettings.AfterCaptureJob = taskSettings.AfterCaptureJob.Remove<AfterCaptureTasks>(AfterCaptureTasks.AnnotateImage);
			}
			if (taskSettings.ImageSettings.ImageEffectOnlyRegionCapture && GetType() != typeof(CaptureRegion) && GetType() != typeof(CaptureLastRegion))
			{
				taskSettings.AfterCaptureJob = taskSettings.AfterCaptureJob.Remove<AfterCaptureTasks>(AfterCaptureTasks.AddImageEffects);
			}
			UploadManager.RunImageTask(metadata, taskSettings);
		}
	}

	protected TaskMetadata CreateMetadata()
	{
		return CreateMetadata(Rectangle.Empty, null);
	}

	protected TaskMetadata CreateMetadata(Rectangle insideRect)
	{
		return CreateMetadata(insideRect, "explorer");
	}

	protected TaskMetadata CreateMetadata(Rectangle insideRect, string ignoreProcess)
	{
		TaskMetadata taskMetadata = new TaskMetadata();
		WindowInfo windowInfo = new WindowInfo(NativeMethods.GetForegroundWindow());
		if ((ignoreProcess == null || !windowInfo.ProcessName.Equals(ignoreProcess, StringComparison.InvariantCultureIgnoreCase)) && (insideRect.IsEmpty || windowInfo.Rectangle.Contains(insideRect)))
		{
			taskMetadata.UpdateInfo(windowInfo);
		}
		return taskMetadata;
	}
}
