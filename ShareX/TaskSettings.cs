using System.Collections.Generic;
using Newtonsoft.Json;
using ShareX.HelpersLib;
using ShareX.UploadersLib;

namespace ShareX;

public class TaskSettings
{
	public string Description = "";

	public HotkeyType Job;

	public bool UseDefaultAfterCaptureJob = true;

	public AfterCaptureTasks AfterCaptureJob = AfterCaptureTasks.CopyImageToClipboard | AfterCaptureTasks.SaveImageToFile | AfterCaptureTasks.UploadImageToHost;

	public bool UseDefaultAfterUploadJob = true;

	public AfterUploadTasks AfterUploadJob = AfterUploadTasks.CopyURLToClipboard;

	public bool UseDefaultDestinations = true;

	public ImageDestination ImageDestination;

	public FileDestination ImageFileDestination;

	public TextDestination TextDestination;

	public FileDestination TextFileDestination;

	public FileDestination FileDestination;

	public UrlShortenerType URLShortenerDestination;

	public URLSharingServices URLSharingServiceDestination = URLSharingServices.Twitter;

	public bool OverrideFTP;

	public int FTPIndex;

	public bool OverrideCustomUploader;

	public int CustomUploaderIndex;

	public bool OverrideScreenshotsFolder;

	public string ScreenshotsFolder = "";

	public bool UseDefaultGeneralSettings = true;

	public TaskSettingsGeneral GeneralSettings = new TaskSettingsGeneral();

	public bool UseDefaultImageSettings = true;

	public TaskSettingsImage ImageSettings = new TaskSettingsImage();

	public bool UseDefaultCaptureSettings = true;

	public TaskSettingsCapture CaptureSettings = new TaskSettingsCapture();

	public bool UseDefaultUploadSettings = true;

	public TaskSettingsUpload UploadSettings = new TaskSettingsUpload();

	public bool UseDefaultActions = true;

	public List<ExternalProgram> ExternalPrograms = new List<ExternalProgram>();

	public bool UseDefaultToolsSettings = true;

	public TaskSettingsTools ToolsSettings = new TaskSettingsTools();

	public bool UseDefaultAdvancedSettings = true;

	public TaskSettingsAdvanced AdvancedSettings = new TaskSettingsAdvanced();

	public bool WatchFolderEnabled;

	public List<WatchFolderSettings> WatchFolderList = new List<WatchFolderSettings>();

	[JsonIgnore]
	public TaskSettings TaskSettingsReference { get; private set; }

	[JsonIgnore]
	public bool IsSafeTaskSettings => TaskSettingsReference != null;

	[JsonIgnore]
	public TaskSettingsImage ImageSettingsReference
	{
		get
		{
			if (UseDefaultImageSettings)
			{
				return Program.DefaultTaskSettings.ImageSettings;
			}
			return TaskSettingsReference.ImageSettings;
		}
	}

	[JsonIgnore]
	public TaskSettingsCapture CaptureSettingsReference
	{
		get
		{
			if (UseDefaultCaptureSettings)
			{
				return Program.DefaultTaskSettings.CaptureSettings;
			}
			return TaskSettingsReference.CaptureSettings;
		}
	}

	[JsonIgnore]
	public TaskSettingsTools ToolsSettingsReference
	{
		get
		{
			if (UseDefaultToolsSettings)
			{
				return Program.DefaultTaskSettings.ToolsSettings;
			}
			return TaskSettingsReference.ToolsSettings;
		}
	}

	public bool IsUsingDefaultSettings
	{
		get
		{
			if (UseDefaultAfterCaptureJob && UseDefaultAfterUploadJob && UseDefaultDestinations && !OverrideFTP && !OverrideCustomUploader && !OverrideScreenshotsFolder && UseDefaultGeneralSettings && UseDefaultImageSettings && UseDefaultCaptureSettings && UseDefaultUploadSettings && UseDefaultActions && UseDefaultToolsSettings && UseDefaultAdvancedSettings)
			{
				return !WatchFolderEnabled;
			}
			return false;
		}
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(Description))
		{
			return Job.GetLocalizedDescription();
		}
		return Description;
	}

	public static TaskSettings GetDefaultTaskSettings()
	{
		TaskSettings taskSettings = new TaskSettings();
		taskSettings.SetDefaultSettings();
		taskSettings.TaskSettingsReference = Program.DefaultTaskSettings;
		return taskSettings;
	}

	public static TaskSettings GetSafeTaskSettings(TaskSettings taskSettings)
	{
		TaskSettings taskSettings2;
		if (taskSettings.IsUsingDefaultSettings && Program.DefaultTaskSettings != null)
		{
			taskSettings2 = Program.DefaultTaskSettings.Copy();
			taskSettings2.Description = taskSettings.Description;
			taskSettings2.Job = taskSettings.Job;
		}
		else
		{
			taskSettings2 = taskSettings.Copy();
			taskSettings2.SetDefaultSettings();
		}
		taskSettings2.TaskSettingsReference = taskSettings;
		return taskSettings2;
	}

	private void SetDefaultSettings()
	{
		if (Program.DefaultTaskSettings != null)
		{
			TaskSettings taskSettings = Program.DefaultTaskSettings.Copy();
			if (UseDefaultAfterCaptureJob)
			{
				AfterCaptureJob = taskSettings.AfterCaptureJob;
			}
			if (UseDefaultAfterUploadJob)
			{
				AfterUploadJob = taskSettings.AfterUploadJob;
			}
			if (UseDefaultDestinations)
			{
				ImageDestination = taskSettings.ImageDestination;
				ImageFileDestination = taskSettings.ImageFileDestination;
				TextDestination = taskSettings.TextDestination;
				TextFileDestination = taskSettings.TextFileDestination;
				FileDestination = taskSettings.FileDestination;
				URLShortenerDestination = taskSettings.URLShortenerDestination;
				URLSharingServiceDestination = taskSettings.URLSharingServiceDestination;
			}
			if (UseDefaultGeneralSettings)
			{
				GeneralSettings = taskSettings.GeneralSettings;
			}
			if (UseDefaultImageSettings)
			{
				ImageSettings = taskSettings.ImageSettings;
			}
			if (UseDefaultCaptureSettings)
			{
				CaptureSettings = taskSettings.CaptureSettings;
			}
			if (UseDefaultUploadSettings)
			{
				UploadSettings = taskSettings.UploadSettings;
			}
			if (UseDefaultActions)
			{
				ExternalPrograms = taskSettings.ExternalPrograms;
			}
			if (UseDefaultToolsSettings)
			{
				ToolsSettings = taskSettings.ToolsSettings;
			}
			if (UseDefaultAdvancedSettings)
			{
				AdvancedSettings = taskSettings.AdvancedSettings;
			}
		}
	}

	public FileDestination GetFileDestinationByDataType(EDataType dataType)
	{
		return dataType switch
		{
			EDataType.Image => ImageFileDestination, 
			EDataType.Text => TextFileDestination, 
			_ => FileDestination, 
		};
	}
}
