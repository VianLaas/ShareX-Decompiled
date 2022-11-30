using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ShareX.HelpersLib;
using ShareX.HistoryLib;
using ShareX.UploadersLib;

namespace ShareX;

public class TaskInfo
{
	private string filePath;

	public TaskSettings TaskSettings { get; set; }

	public string Status { get; set; }

	public TaskJob Job { get; set; }

	public bool IsUploadJob
	{
		get
		{
			switch (Job)
			{
			case TaskJob.Job:
				return TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.UploadImageToHost);
			case TaskJob.DataUpload:
			case TaskJob.FileUpload:
			case TaskJob.TextUpload:
			case TaskJob.ShortenURL:
			case TaskJob.ShareURL:
			case TaskJob.DownloadUpload:
				return true;
			default:
				return false;
			}
		}
	}

	public ProgressManager Progress { get; set; }

	public string FilePath
	{
		get
		{
			return filePath;
		}
		set
		{
			filePath = value;
			if (string.IsNullOrEmpty(filePath))
			{
				FileName = "";
			}
			else
			{
				FileName = Path.GetFileName(filePath);
			}
		}
	}

	public string FileName { get; set; }

	public string ThumbnailFilePath { get; set; }

	public EDataType DataType { get; set; }

	public TaskMetadata Metadata { get; set; }

	public EDataType UploadDestination
	{
		get
		{
			if ((DataType == EDataType.Image && TaskSettings.ImageDestination == ImageDestination.FileUploader) || (DataType == EDataType.Text && TaskSettings.TextDestination == TextDestination.FileUploader))
			{
				return EDataType.File;
			}
			return DataType;
		}
	}

	public string UploaderHost
	{
		get
		{
			if (IsUploadJob)
			{
				switch (UploadDestination)
				{
				case EDataType.Image:
					return TaskSettings.ImageDestination.GetLocalizedDescription();
				case EDataType.Text:
					return TaskSettings.TextDestination.GetLocalizedDescription();
				case EDataType.File:
					return DataType switch
					{
						EDataType.Image => TaskSettings.ImageFileDestination.GetLocalizedDescription(), 
						EDataType.Text => TaskSettings.TextFileDestination.GetLocalizedDescription(), 
						_ => TaskSettings.FileDestination.GetLocalizedDescription(), 
					};
				case EDataType.URL:
					if (Job == TaskJob.ShareURL)
					{
						return TaskSettings.URLSharingServiceDestination.GetLocalizedDescription();
					}
					return TaskSettings.URLShortenerDestination.GetLocalizedDescription();
				}
			}
			return "";
		}
	}

	public DateTime TaskStartTime { get; set; }

	public DateTime TaskEndTime { get; set; }

	public TimeSpan TaskDuration => TaskEndTime - TaskStartTime;

	public Stopwatch UploadDuration { get; set; }

	public UploadResult Result { get; set; }

	public TaskInfo(TaskSettings taskSettings)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		TaskSettings = taskSettings;
		Metadata = new TaskMetadata();
		Result = new UploadResult();
	}

	public Dictionary<string, string> GetTags()
	{
		if (Metadata != null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(Metadata.WindowTitle))
			{
				dictionary.Add("WindowTitle", Metadata.WindowTitle);
			}
			if (!string.IsNullOrEmpty(Metadata.ProcessName))
			{
				dictionary.Add("ProcessName", Metadata.ProcessName);
			}
			if (dictionary.Count > 0)
			{
				return dictionary;
			}
		}
		return null;
	}

	public override string ToString()
	{
		string text = Result.ToString();
		if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(FilePath))
		{
			text = FilePath;
		}
		return text;
	}

	public HistoryItem GetHistoryItem()
	{
		return new HistoryItem
		{
			FileName = FileName,
			FilePath = FilePath,
			DateTime = TaskEndTime,
			Type = DataType.ToString(),
			Host = UploaderHost,
			URL = Result.URL,
			ThumbnailURL = Result.ThumbnailURL,
			DeletionURL = Result.DeletionURL,
			ShortenedURL = Result.ShortenedURL,
			Tags = GetTags()
		};
	}
}
