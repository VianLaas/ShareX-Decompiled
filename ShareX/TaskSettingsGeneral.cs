using System.Drawing;

namespace ShareX;

public class TaskSettingsGeneral
{
	public bool PlaySoundAfterCapture = true;

	public bool PlaySoundAfterUpload = true;

	public bool ShowToastNotificationAfterTaskCompleted = true;

	public float ToastWindowDuration = 3f;

	public float ToastWindowFadeDuration = 1f;

	public ContentAlignment ToastWindowPlacement = ContentAlignment.BottomRight;

	public Size ToastWindowSize = new Size(400, 300);

	public ToastClickAction ToastWindowLeftClickAction = ToastClickAction.OpenUrl;

	public ToastClickAction ToastWindowRightClickAction;

	public ToastClickAction ToastWindowMiddleClickAction = ToastClickAction.AnnotateImage;

	public bool ToastWindowAutoHide = true;

	public bool UseCustomCaptureSound;

	public string CustomCaptureSoundPath = "";

	public bool UseCustomTaskCompletedSound;

	public string CustomTaskCompletedSoundPath = "";

	public bool UseCustomErrorSound;

	public string CustomErrorSoundPath = "";

	public bool DisableNotifications;

	public bool DisableNotificationsOnFullscreen;

	public PopUpNotificationType PopUpNotification = PopUpNotificationType.ToastNotification;
}
