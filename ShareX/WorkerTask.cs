using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.UploadersLib;

namespace ShareX;

public class WorkerTask : IDisposable
{
	public delegate void TaskEventHandler(WorkerTask task);

	public delegate void TaskImageEventHandler(WorkerTask task, Bitmap image);

	public delegate void UploaderServiceEventHandler(IUploaderService uploaderService);

	private ThreadWorker threadWorker;

	private GenericUploader uploader;

	private TaskReferenceHelper taskReferenceHelper;

	public TaskInfo Info { get; private set; }

	public TaskStatus Status { get; private set; }

	public bool IsBusy
	{
		get
		{
			if (Status != 0)
			{
				return IsWorking;
			}
			return true;
		}
	}

	public bool IsWorking
	{
		get
		{
			if (Status != TaskStatus.Preparing && Status != TaskStatus.Working)
			{
				return Status == TaskStatus.Stopping;
			}
			return true;
		}
	}

	public bool StopRequested { get; private set; }

	public bool RequestSettingUpdate { get; private set; }

	public bool EarlyURLCopied { get; private set; }

	public Stream Data { get; private set; }

	public Bitmap Image { get; private set; }

	public bool KeepImage { get; set; }

	public string Text { get; private set; }

	public event TaskEventHandler StatusChanged;

	public event TaskEventHandler UploadStarted;

	public event TaskEventHandler UploadProgressChanged;

	public event TaskEventHandler UploadCompleted;

	public event TaskEventHandler TaskCompleted;

	public event TaskImageEventHandler ImageReady;

	public event UploaderServiceEventHandler UploadersConfigWindowRequested;

	private WorkerTask(TaskSettings taskSettings)
	{
		Status = TaskStatus.InQueue;
		Info = new TaskInfo(taskSettings);
	}

	public static WorkerTask CreateHistoryTask(RecentTask recentTask)
	{
		WorkerTask workerTask = new WorkerTask(null);
		workerTask.Status = TaskStatus.History;
		workerTask.Info.FilePath = recentTask.FilePath;
		workerTask.Info.FileName = recentTask.FileName;
		workerTask.Info.Result.URL = recentTask.URL;
		workerTask.Info.Result.ThumbnailURL = recentTask.ThumbnailURL;
		workerTask.Info.Result.DeletionURL = recentTask.DeletionURL;
		workerTask.Info.Result.ShortenedURL = recentTask.ShortenedURL;
		workerTask.Info.TaskEndTime = recentTask.Time;
		return workerTask;
	}

	public static WorkerTask CreateDataUploaderTask(EDataType dataType, Stream stream, string fileName, TaskSettings taskSettings)
	{
		WorkerTask workerTask = new WorkerTask(taskSettings);
		workerTask.Info.Job = TaskJob.DataUpload;
		workerTask.Info.DataType = dataType;
		workerTask.Info.FileName = fileName;
		workerTask.Data = stream;
		return workerTask;
	}

	public static WorkerTask CreateFileUploaderTask(string filePath, TaskSettings taskSettings)
	{
		WorkerTask workerTask = new WorkerTask(taskSettings);
		workerTask.Info.FilePath = filePath;
		workerTask.Info.DataType = TaskHelpers.FindDataType(workerTask.Info.FilePath, taskSettings);
		if (workerTask.Info.TaskSettings.UploadSettings.FileUploadUseNamePattern)
		{
			string fileNameExtension = FileHelpers.GetFileNameExtension(workerTask.Info.FilePath);
			workerTask.Info.FileName = TaskHelpers.GetFileName(workerTask.Info.TaskSettings, fileNameExtension);
		}
		if (workerTask.Info.TaskSettings.AdvancedSettings.ProcessImagesDuringFileUpload && workerTask.Info.DataType == EDataType.Image)
		{
			workerTask.Info.Job = TaskJob.Job;
			workerTask.Image = ImageHelpers.LoadImage(workerTask.Info.FilePath);
		}
		else
		{
			workerTask.Info.Job = TaskJob.FileUpload;
			if (!workerTask.LoadFileStream())
			{
				return null;
			}
		}
		return workerTask;
	}

	public static WorkerTask CreateImageUploaderTask(TaskMetadata metadata, TaskSettings taskSettings, string customFileName = null)
	{
		WorkerTask workerTask = new WorkerTask(taskSettings);
		workerTask.Info.Job = TaskJob.Job;
		workerTask.Info.DataType = EDataType.Image;
		if (!string.IsNullOrEmpty(customFileName))
		{
			workerTask.Info.FileName = FileHelpers.AppendExtension(customFileName, "bmp");
		}
		else
		{
			workerTask.Info.FileName = TaskHelpers.GetFileName(taskSettings, "bmp", metadata);
		}
		workerTask.Info.Metadata = metadata;
		workerTask.Image = metadata.Image;
		return workerTask;
	}

	public static WorkerTask CreateTextUploaderTask(string text, TaskSettings taskSettings)
	{
		WorkerTask workerTask = new WorkerTask(taskSettings);
		workerTask.Info.Job = TaskJob.TextUpload;
		workerTask.Info.DataType = EDataType.Text;
		workerTask.Info.FileName = TaskHelpers.GetFileName(taskSettings, taskSettings.AdvancedSettings.TextFileExtension);
		workerTask.Text = text;
		return workerTask;
	}

	public static WorkerTask CreateURLShortenerTask(string url, TaskSettings taskSettings)
	{
		WorkerTask workerTask = new WorkerTask(taskSettings);
		workerTask.Info.Job = TaskJob.ShortenURL;
		workerTask.Info.DataType = EDataType.URL;
		workerTask.Info.FileName = string.Format(Resources.UploadTask_CreateURLShortenerTask_Shorten_URL___0__, taskSettings.URLShortenerDestination.GetLocalizedDescription());
		workerTask.Info.Result.URL = url;
		return workerTask;
	}

	public static WorkerTask CreateShareURLTask(string url, TaskSettings taskSettings)
	{
		WorkerTask workerTask = new WorkerTask(taskSettings);
		workerTask.Info.Job = TaskJob.ShareURL;
		workerTask.Info.DataType = EDataType.URL;
		workerTask.Info.FileName = string.Format(Resources.UploadTask_CreateShareURLTask_Share_URL___0__, taskSettings.URLSharingServiceDestination.GetLocalizedDescription());
		workerTask.Info.Result.URL = url;
		return workerTask;
	}

	public static WorkerTask CreateFileJobTask(string filePath, TaskMetadata metadata, TaskSettings taskSettings, string customFileName = null)
	{
		WorkerTask workerTask = new WorkerTask(taskSettings);
		workerTask.Info.FilePath = filePath;
		workerTask.Info.DataType = TaskHelpers.FindDataType(workerTask.Info.FilePath, taskSettings);
		if (!string.IsNullOrEmpty(customFileName))
		{
			string fileNameExtension = FileHelpers.GetFileNameExtension(workerTask.Info.FilePath);
			workerTask.Info.FileName = FileHelpers.AppendExtension(customFileName, fileNameExtension);
		}
		else if (workerTask.Info.TaskSettings.UploadSettings.FileUploadUseNamePattern)
		{
			string fileNameExtension2 = FileHelpers.GetFileNameExtension(workerTask.Info.FilePath);
			workerTask.Info.FileName = TaskHelpers.GetFileName(workerTask.Info.TaskSettings, fileNameExtension2);
		}
		workerTask.Info.Metadata = metadata;
		workerTask.Info.Job = TaskJob.Job;
		if (workerTask.Info.IsUploadJob && !workerTask.LoadFileStream())
		{
			return null;
		}
		return workerTask;
	}

	public static WorkerTask CreateDownloadTask(string url, bool upload, TaskSettings taskSettings)
	{
		WorkerTask workerTask = new WorkerTask(taskSettings);
		workerTask.Info.Job = (upload ? TaskJob.DownloadUpload : TaskJob.Download);
		string path = URLHelpers.URLDecode(url, 10);
		path = URLHelpers.GetFileName(path);
		path = FileHelpers.SanitizeFileName(path);
		if (workerTask.Info.TaskSettings.UploadSettings.FileUploadUseNamePattern)
		{
			string fileNameExtension = FileHelpers.GetFileNameExtension(path);
			path = TaskHelpers.GetFileName(workerTask.Info.TaskSettings, fileNameExtension);
		}
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		workerTask.Info.FileName = path;
		workerTask.Info.DataType = TaskHelpers.FindDataType(workerTask.Info.FileName, taskSettings);
		workerTask.Info.Result.URL = url;
		return workerTask;
	}

	public void Start()
	{
		if (Status == TaskStatus.InQueue && !StopRequested)
		{
			Info.TaskStartTime = DateTime.Now;
			threadWorker = new ThreadWorker();
			Prepare();
			threadWorker.DoWork += ThreadDoWork;
			threadWorker.Completed += ThreadCompleted;
			threadWorker.Start(ApartmentState.STA);
		}
	}

	private void Prepare()
	{
		Status = TaskStatus.Preparing;
		TaskJob job = Info.Job;
		if (job == TaskJob.Job || job == TaskJob.TextUpload)
		{
			Info.Status = Resources.UploadTask_Prepare_Preparing;
		}
		else
		{
			Info.Status = Resources.UploadTask_Prepare_Starting;
		}
		OnStatusChanged();
	}

	public void Stop()
	{
		StopRequested = true;
		switch (Status)
		{
		case TaskStatus.InQueue:
			OnTaskCompleted();
			break;
		case TaskStatus.Preparing:
		case TaskStatus.Working:
			if (uploader != null)
			{
				uploader.StopUpload();
			}
			Status = TaskStatus.Stopping;
			Info.Status = Resources.UploadTask_Stop_Stopping;
			OnStatusChanged();
			break;
		}
	}

	public void ShowErrorWindow()
	{
		if (Info == null || Info.Result == null || !Info.Result.IsError)
		{
			return;
		}
		string text = Info.Result.ErrorsToString();
		if (!string.IsNullOrEmpty(text))
		{
			using (ErrorForm errorForm = new ErrorForm(Resources.UploadInfoManager_ShowErrors_Upload_errors, text, Program.LogsFilePath, "https://github.com/ShareX/ShareX/issues?q=is%3Aissue", unhandledException: false))
			{
				errorForm.ShowDialog();
			}
		}
	}

	private void ThreadDoWork()
	{
		CreateTaskReferenceHelper();
		try
		{
			StopRequested = !DoThreadJob();
			OnImageReady();
			if (!StopRequested)
			{
				if (Info.IsUploadJob && TaskHelpers.IsUploadAllowed())
				{
					DoUploadJob();
				}
				else
				{
					Info.Result.IsURLExpected = false;
				}
			}
		}
		finally
		{
			KeepImage = Image != null && Info.TaskSettings.GeneralSettings.ShowToastNotificationAfterTaskCompleted;
			Dispose();
			if (EarlyURLCopied && (StopRequested || Info.Result == null || string.IsNullOrEmpty(Info.Result.URL)) && ClipboardHelpers.ContainsText())
			{
				ClipboardHelpers.Clear();
			}
			if ((Info.Job == TaskJob.Job || (Info.Job == TaskJob.FileUpload && Info.TaskSettings.AdvancedSettings.UseAfterCaptureTasksDuringFileUpload)) && Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.DeleteFile) && !string.IsNullOrEmpty(Info.FilePath) && File.Exists(Info.FilePath))
			{
				File.Delete(Info.FilePath);
			}
		}
		if (!StopRequested && Info.Result != null && Info.Result.IsURLExpected && !Info.Result.IsError)
		{
			if (string.IsNullOrEmpty(Info.Result.URL))
			{
				AddErrorMessage(Resources.UploadTask_ThreadDoWork_URL_is_empty_);
			}
			else
			{
				DoAfterUploadJobs();
			}
		}
	}

	private void CreateTaskReferenceHelper()
	{
		taskReferenceHelper = new TaskReferenceHelper
		{
			DataType = Info.DataType,
			OverrideFTP = Info.TaskSettings.OverrideFTP,
			FTPIndex = Info.TaskSettings.FTPIndex,
			OverrideCustomUploader = Info.TaskSettings.OverrideCustomUploader,
			CustomUploaderIndex = Info.TaskSettings.CustomUploaderIndex,
			TextFormat = Info.TaskSettings.AdvancedSettings.TextFormat
		};
	}

	private void DoUploadJob()
	{
		if (Program.Settings.ShowUploadWarning)
		{
			bool num = !FirstTimeUploadForm.ShowForm();
			Program.Settings.ShowUploadWarning = false;
			if (num)
			{
				Program.DefaultTaskSettings.AfterCaptureJob = Program.DefaultTaskSettings.AfterCaptureJob.Remove<AfterCaptureTasks>(AfterCaptureTasks.UploadImageToHost);
				foreach (HotkeySettings hotkey in Program.HotkeysConfig.Hotkeys)
				{
					if (hotkey.TaskSettings != null)
					{
						hotkey.TaskSettings.AfterCaptureJob = hotkey.TaskSettings.AfterCaptureJob.Remove<AfterCaptureTasks>(AfterCaptureTasks.UploadImageToHost);
					}
				}
				Info.TaskSettings.AfterCaptureJob = Info.TaskSettings.AfterCaptureJob.Remove<AfterCaptureTasks>(AfterCaptureTasks.UploadImageToHost);
				Info.Result.IsURLExpected = false;
				RequestSettingUpdate = true;
				return;
			}
		}
		if (Program.Settings.ShowLargeFileSizeWarning > 0)
		{
			long num2 = (Program.Settings.BinaryUnits ? (Program.Settings.ShowLargeFileSizeWarning * 1024 * 1024) : (Program.Settings.ShowLargeFileSizeWarning * 1000 * 1000));
			if (Data != null && Data.Length > num2)
			{
				using MyMessageBox myMessageBox = new MyMessageBox(Resources.UploadTask_DoUploadJob_You_are_attempting_to_upload_a_large_file, "ShareX", MessageBoxButtons.YesNo, Resources.UploadManager_IsUploadConfirmed_Don_t_show_this_message_again_);
				myMessageBox.ShowDialog();
				if (myMessageBox.IsChecked)
				{
					Program.Settings.ShowLargeFileSizeWarning = 0;
				}
				if (myMessageBox.DialogResult == DialogResult.No)
				{
					Stop();
				}
			}
		}
		if (StopRequested)
		{
			return;
		}
		SettingManager.WaitUploadersConfig();
		Status = TaskStatus.Working;
		Info.Status = Resources.UploadTask_DoUploadJob_Uploading;
		TaskbarManager.SetProgressState(Program.MainForm, TaskbarProgressBarStatus.Normal);
		bool flag = false;
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.ShowBeforeUploadWindow))
		{
			using BeforeUploadForm beforeUploadForm = new BeforeUploadForm(Info);
			flag = beforeUploadForm.ShowDialog() != DialogResult.OK;
		}
		if (!flag)
		{
			OnUploadStarted();
			bool flag2 = DoUpload(Data, Info.FileName);
			if (flag2 && Program.Settings.MaxUploadFailRetry > 0)
			{
				int num3 = 1;
				while (!StopRequested && flag2 && num3 <= Program.Settings.MaxUploadFailRetry)
				{
					DebugHelper.WriteLine("Upload failed. Retrying upload.");
					flag2 = DoUpload(Data, Info.FileName, num3);
					num3++;
				}
			}
			if (!flag2)
			{
				OnUploadCompleted();
			}
		}
		else
		{
			Info.Result.IsURLExpected = false;
		}
	}

	private bool DoUpload(Stream data, string fileName, int retry = 0)
	{
		bool flag = false;
		if (retry > 0)
		{
			if (Program.Settings.UseSecondaryUploaders)
			{
				Info.TaskSettings.ImageDestination = Program.Settings.SecondaryImageUploaders[retry - 1];
				Info.TaskSettings.ImageFileDestination = Program.Settings.SecondaryFileUploaders[retry - 1];
				Info.TaskSettings.TextDestination = Program.Settings.SecondaryTextUploaders[retry - 1];
				Info.TaskSettings.TextFileDestination = Program.Settings.SecondaryFileUploaders[retry - 1];
				Info.TaskSettings.FileDestination = Program.Settings.SecondaryFileUploaders[retry - 1];
			}
			else
			{
				Thread.Sleep(1000);
			}
		}
		SSLBypassHelper sSLBypassHelper = null;
		try
		{
			if (HelpersOptions.AcceptInvalidSSLCertificates)
			{
				sSLBypassHelper = new SSLBypassHelper();
			}
			if (!CheckUploadFilters(data, fileName))
			{
				switch (Info.UploadDestination)
				{
				case EDataType.Image:
					Info.Result = UploadImage(data, fileName);
					break;
				case EDataType.Text:
					Info.Result = UploadText(data, fileName);
					break;
				case EDataType.File:
					Info.Result = UploadFile(data, fileName);
					break;
				}
			}
			StopRequested |= taskReferenceHelper.StopRequested;
		}
		catch (Exception ex)
		{
			if (!StopRequested)
			{
				DebugHelper.WriteException(ex);
				flag = true;
				AddErrorMessage(ex.ToString());
			}
		}
		finally
		{
			sSLBypassHelper?.Dispose();
			if (Info.Result == null)
			{
				Info.Result = new UploadResult();
			}
			if (uploader != null)
			{
				AddErrorMessage(uploader.Errors.ToArray());
			}
			flag |= Info.Result.IsError;
		}
		return flag;
	}

	private void AddErrorMessage(params string[] errorMessages)
	{
		if (Info.Result == null)
		{
			Info.Result = new UploadResult();
		}
		Info.Result.Errors.AddRange(errorMessages);
	}

	private bool DoThreadJob()
	{
		if (Info.IsUploadJob && Info.TaskSettings.AdvancedSettings.AutoClearClipboard)
		{
			ClipboardHelpers.Clear();
		}
		if (Info.Job == TaskJob.Download || Info.Job == TaskJob.DownloadUpload)
		{
			if (!DownloadFromURL(Info.Job == TaskJob.DownloadUpload))
			{
				return false;
			}
			if (Info.Job == TaskJob.Download)
			{
				return true;
			}
		}
		if (Info.Job == TaskJob.Job)
		{
			if (!DoAfterCaptureJobs())
			{
				return false;
			}
			DoFileJobs();
		}
		else if (Info.Job == TaskJob.TextUpload && !string.IsNullOrEmpty(Text))
		{
			DoTextJobs();
		}
		else if (Info.Job == TaskJob.FileUpload && Info.TaskSettings.AdvancedSettings.UseAfterCaptureTasksDuringFileUpload)
		{
			DoFileJobs();
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.DoOCR))
		{
			DoOCR();
		}
		if (Info.IsUploadJob && Data != null && Data.CanSeek)
		{
			Data.Position = 0L;
		}
		return true;
	}

	private bool DoAfterCaptureJobs()
	{
		if (Image == null)
		{
			return true;
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.AddImageEffects))
		{
			Image = TaskHelpers.ApplyImageEffects(Image, Info.TaskSettings.ImageSettingsReference);
			if (Image == null)
			{
				DebugHelper.WriteLine("Error: Applying image effects resulted empty image.");
				return false;
			}
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.AnnotateImage))
		{
			Image = TaskHelpers.AnnotateImage(Image, null, Info.TaskSettings, taskMode: true);
			if (Image == null)
			{
				return false;
			}
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.CopyImageToClipboard))
		{
			ClipboardHelpers.CopyImage(Image, Info.FileName);
			DebugHelper.WriteLine("Image copied to clipboard.");
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.SendImageToPrinter))
		{
			TaskHelpers.PrintImage(Image);
		}
		Info.Metadata.Image = Image;
		if (Info.TaskSettings.AfterCaptureJob.HasFlagAny<AfterCaptureTasks>(AfterCaptureTasks.SaveImageToFile, AfterCaptureTasks.SaveImageToFileWithDialog, AfterCaptureTasks.DoOCR, AfterCaptureTasks.UploadImageToHost))
		{
			ImageData imageData = TaskHelpers.PrepareImage(Image, Info.TaskSettings);
			Data = imageData.ImageStream;
			Info.FileName = Path.ChangeExtension(Info.FileName, imageData.ImageFormat.GetDescription());
			if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.SaveImageToFile))
			{
				string text = TaskHelpers.HandleExistsFile(TaskHelpers.GetScreenshotsFolder(Info.TaskSettings, Info.Metadata), Info.FileName, Info.TaskSettings);
				if (!string.IsNullOrEmpty(text))
				{
					Info.FilePath = text;
					imageData.Write(Info.FilePath);
					DebugHelper.WriteLine("Image saved to file: " + Info.FilePath);
				}
			}
			if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.SaveImageToFileWithDialog))
			{
				using SaveFileDialog saveFileDialog = new SaveFileDialog();
				string text2 = null;
				text2 = ((string.IsNullOrEmpty(HelpersOptions.LastSaveDirectory) || !Directory.Exists(HelpersOptions.LastSaveDirectory)) ? TaskHelpers.GetScreenshotsFolder(Info.TaskSettings, Info.Metadata) : HelpersOptions.LastSaveDirectory);
				bool flag;
				do
				{
					saveFileDialog.InitialDirectory = text2;
					saveFileDialog.FileName = Info.FileName;
					saveFileDialog.DefaultExt = Path.GetExtension(Info.FileName).Substring(1);
					saveFileDialog.Filter = string.Format("*{0}|*{0}|All files (*.*)|*.*", Path.GetExtension(Info.FileName));
					saveFileDialog.Title = Resources.UploadTask_DoAfterCaptureJobs_Choose_a_folder_to_save + " " + Path.GetFileName(Info.FileName);
					if (saveFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog.FileName))
					{
						Info.FilePath = saveFileDialog.FileName;
						HelpersOptions.LastSaveDirectory = Path.GetDirectoryName(Info.FilePath);
						flag = imageData.Write(Info.FilePath);
						if (flag)
						{
							DebugHelper.WriteLine("Image saved to file with dialog: " + Info.FilePath);
						}
						continue;
					}
					break;
				}
				while (!flag);
			}
			if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.SaveThumbnailImageToFile))
			{
				string fileName;
				string folder;
				if (!string.IsNullOrEmpty(Info.FilePath))
				{
					fileName = Path.GetFileName(Info.FilePath);
					folder = Path.GetDirectoryName(Info.FilePath);
				}
				else
				{
					fileName = Info.FileName;
					folder = TaskHelpers.GetScreenshotsFolder(Info.TaskSettings, Info.Metadata);
				}
				Info.ThumbnailFilePath = TaskHelpers.CreateThumbnail(Image, folder, fileName, Info.TaskSettings);
				if (!string.IsNullOrEmpty(Info.ThumbnailFilePath))
				{
					DebugHelper.WriteLine("Thumbnail saved to file: " + Info.ThumbnailFilePath);
				}
			}
		}
		return true;
	}

	private void DoFileJobs()
	{
		if (string.IsNullOrEmpty(Info.FilePath) || !File.Exists(Info.FilePath))
		{
			return;
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.PerformActions) && Info.TaskSettings.ExternalPrograms != null)
		{
			IEnumerable<ExternalProgram> enumerable = Info.TaskSettings.ExternalPrograms.Where((ExternalProgram x) => x.IsActive);
			if (enumerable.Count() > 0)
			{
				bool flag = false;
				string fileName = Info.FileName;
				foreach (ExternalProgram item in enumerable)
				{
					string text = item.Run(Info.FilePath);
					if (!string.IsNullOrEmpty(text))
					{
						flag = true;
						Info.FilePath = text;
						if (Data != null)
						{
							Data.Dispose();
						}
						item.DeletePendingInputFile();
					}
				}
				if (flag)
				{
					string fileNameExtension = FileHelpers.GetFileNameExtension(Info.FilePath);
					Info.FileName = FileHelpers.ChangeFileNameExtension(fileName, fileNameExtension);
					LoadFileStream();
				}
			}
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.CopyFileToClipboard))
		{
			ClipboardHelpers.CopyFile(Info.FilePath);
		}
		else if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.CopyFilePathToClipboard))
		{
			ClipboardHelpers.CopyText(Info.FilePath);
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.ShowInExplorer))
		{
			FileHelpers.OpenFolderWithFile(Info.FilePath);
		}
		if (Info.TaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.ScanQRCode) && Info.DataType == EDataType.Image)
		{
			QRCodeForm.OpenFormDecodeFromFile(Info.FilePath).ShowDialog();
		}
	}

	private void DoTextJobs()
	{
		if (Info.TaskSettings.AdvancedSettings.TextTaskSaveAsFile)
		{
			string text = TaskHelpers.HandleExistsFile(TaskHelpers.GetScreenshotsFolder(Info.TaskSettings), Info.FileName, Info.TaskSettings);
			if (!string.IsNullOrEmpty(text))
			{
				Info.FilePath = text;
				FileHelpers.CreateDirectoryFromFilePath(Info.FilePath);
				File.WriteAllText(Info.FilePath, Text, Encoding.UTF8);
				DebugHelper.WriteLine("Text saved to file: " + Info.FilePath);
			}
		}
		byte[] bytes = Encoding.UTF8.GetBytes(Text);
		Data = new MemoryStream(bytes);
	}

	private void DoAfterUploadJobs()
	{
		try
		{
			if (Info.TaskSettings.UploadSettings.URLRegexReplace)
			{
				Info.Result.URL = Regex.Replace(Info.Result.URL, Info.TaskSettings.UploadSettings.URLRegexReplacePattern, Info.TaskSettings.UploadSettings.URLRegexReplaceReplacement);
			}
			if (Info.TaskSettings.AdvancedSettings.ResultForceHTTPS)
			{
				Info.Result.ForceHTTPS();
			}
			if (Info.Job != TaskJob.ShareURL && (Info.TaskSettings.AfterUploadJob.HasFlag(AfterUploadTasks.UseURLShortener) || Info.Job == TaskJob.ShortenURL || (Info.TaskSettings.AdvancedSettings.AutoShortenURLLength > 0 && Info.Result.URL.Length > Info.TaskSettings.AdvancedSettings.AutoShortenURLLength)))
			{
				UploadResult uploadResult = ShortenURL(Info.Result.URL);
				if (uploadResult != null)
				{
					Info.Result.ShortenedURL = uploadResult.ShortenedURL;
					Info.Result.Errors.AddRange(uploadResult.Errors);
				}
			}
			if (Info.Job != TaskJob.ShortenURL && (Info.TaskSettings.AfterUploadJob.HasFlag(AfterUploadTasks.ShareURL) || Info.Job == TaskJob.ShareURL))
			{
				UploadResult uploadResult2 = ShareURL(Info.Result.ToString());
				if (uploadResult2 != null)
				{
					Info.Result.Errors.AddRange(uploadResult2.Errors);
				}
				if (Info.Job == TaskJob.ShareURL)
				{
					Info.Result.IsURLExpected = false;
				}
			}
			if (Info.TaskSettings.AfterUploadJob.HasFlag(AfterUploadTasks.CopyURLToClipboard))
			{
				string text = (string.IsNullOrEmpty(Info.TaskSettings.AdvancedSettings.ClipboardContentFormat) ? Info.Result.ToString() : new UploadInfoParser().Parse(Info, Info.TaskSettings.AdvancedSettings.ClipboardContentFormat));
				if (!string.IsNullOrEmpty(text))
				{
					ClipboardHelpers.CopyText(text);
				}
			}
			if (Info.TaskSettings.AfterUploadJob.HasFlag(AfterUploadTasks.OpenURL))
			{
				string url = (string.IsNullOrEmpty(Info.TaskSettings.AdvancedSettings.OpenURLFormat) ? Info.Result.ToString() : new UploadInfoParser().Parse(Info, Info.TaskSettings.AdvancedSettings.OpenURLFormat));
				URLHelpers.OpenURL(url);
			}
			if (Info.TaskSettings.AfterUploadJob.HasFlag(AfterUploadTasks.ShowQRCode))
			{
				threadWorker.InvokeAsync(delegate
				{
					new QRCodeForm(Info.Result.ToString()).Show();
				});
			}
		}
		catch (Exception ex)
		{
			DebugHelper.WriteException(ex);
			AddErrorMessage(ex.ToString());
		}
	}

	public UploadResult UploadData(IGenericUploaderService service, Stream stream, string fileName)
	{
		if (!service.CheckConfig(Program.UploadersConfig))
		{
			return GetInvalidConfigResult(service);
		}
		uploader = service.CreateUploader(Program.UploadersConfig, taskReferenceHelper);
		if (uploader != null)
		{
			uploader.BufferSize = (int)Math.Pow(2.0, Program.Settings.BufferSizePower) * 1024;
			uploader.ProgressChanged += uploader_ProgressChanged;
			if (Info.TaskSettings.AfterUploadJob.HasFlag(AfterUploadTasks.CopyURLToClipboard) && Info.TaskSettings.AdvancedSettings.EarlyCopyURL)
			{
				uploader.EarlyURLCopyRequested += delegate(string url)
				{
					ClipboardHelpers.CopyText(url);
					EarlyURLCopied = true;
				};
			}
			fileName = URLHelpers.RemoveBidiControlCharacters(fileName);
			if (Info.TaskSettings.UploadSettings.FileUploadReplaceProblematicCharacters)
			{
				fileName = URLHelpers.ReplaceReservedCharacters(fileName, "_");
			}
			Info.UploadDuration = Stopwatch.StartNew();
			UploadResult result = uploader.Upload(stream, fileName);
			Info.UploadDuration.Stop();
			return result;
		}
		return null;
	}

	private bool CheckUploadFilters(Stream stream, string fileName)
	{
		if (Info.TaskSettings.UploadSettings.UploaderFilters != null && !string.IsNullOrEmpty(fileName) && stream != null)
		{
			UploaderFilter uploaderFilter = Info.TaskSettings.UploadSettings.UploaderFilters.FirstOrDefault((UploaderFilter x) => x.IsValidFilter(fileName));
			if (uploaderFilter != null)
			{
				IGenericUploaderService uploaderService = uploaderFilter.GetUploaderService();
				if (uploaderService != null)
				{
					Info.Result = UploadData(uploaderService, stream, fileName);
					return true;
				}
			}
		}
		return false;
	}

	public UploadResult UploadImage(Stream stream, string fileName)
	{
		ImageUploaderService service = UploaderFactory.ImageUploaderServices[Info.TaskSettings.ImageDestination];
		return UploadData(service, stream, fileName);
	}

	public UploadResult UploadText(Stream stream, string fileName)
	{
		TextUploaderService service = UploaderFactory.TextUploaderServices[Info.TaskSettings.TextDestination];
		return UploadData(service, stream, fileName);
	}

	public UploadResult UploadFile(Stream stream, string fileName)
	{
		FileUploaderService service = UploaderFactory.FileUploaderServices[Info.TaskSettings.GetFileDestinationByDataType(Info.DataType)];
		return UploadData(service, stream, fileName);
	}

	public UploadResult ShortenURL(string url)
	{
		URLShortenerService uRLShortenerService = UploaderFactory.URLShortenerServices[Info.TaskSettings.URLShortenerDestination];
		if (!uRLShortenerService.CheckConfig(Program.UploadersConfig))
		{
			return GetInvalidConfigResult(uRLShortenerService);
		}
		return uRLShortenerService.CreateShortener(Program.UploadersConfig, taskReferenceHelper)?.ShortenURL(url);
	}

	public UploadResult ShareURL(string url)
	{
		if (!string.IsNullOrEmpty(url))
		{
			URLSharingService uRLSharingService = UploaderFactory.URLSharingServices[Info.TaskSettings.URLSharingServiceDestination];
			if (!uRLSharingService.CheckConfig(Program.UploadersConfig))
			{
				return GetInvalidConfigResult(uRLSharingService);
			}
			URLSharer uRLSharer = uRLSharingService.CreateSharer(Program.UploadersConfig, taskReferenceHelper);
			if (uRLSharer != null)
			{
				return uRLSharer.ShareURL(url);
			}
		}
		return null;
	}

	private UploadResult GetInvalidConfigResult(IUploaderService uploaderService)
	{
		UploadResult uploadResult = new UploadResult();
		string text = string.Format(Resources.WorkerTask_GetInvalidConfigResult__0__configuration_is_invalid_or_missing__Please_check__Destination_settings__window_to_configure_it_, uploaderService.ServiceName);
		DebugHelper.WriteLine(text);
		uploadResult.Errors.Add(text);
		OnUploadersConfigWindowRequested(uploaderService);
		return uploadResult;
	}

	private bool DownloadFromURL(bool upload)
	{
		string address = Info.Result.URL.Trim();
		Info.Result.URL = "";
		string screenshotsFolder = TaskHelpers.GetScreenshotsFolder(Info.TaskSettings);
		Info.FilePath = TaskHelpers.HandleExistsFile(screenshotsFolder, Info.FileName, Info.TaskSettings);
		if (!string.IsNullOrEmpty(Info.FilePath))
		{
			Info.Status = Resources.UploadTask_DownloadAndUpload_Downloading;
			OnStatusChanged();
			try
			{
				FileHelpers.CreateDirectoryFromFilePath(Info.FilePath);
				using (WebClient webClient = new WebClient())
				{
					webClient.Headers.Add(HttpRequestHeader.UserAgent, ShareXResources.UserAgent);
					webClient.Proxy = HelpersOptions.CurrentProxy.GetWebProxy();
					webClient.DownloadFile(address, Info.FilePath);
				}
				if (upload)
				{
					LoadFileStream();
				}
				return true;
			}
			catch (Exception ex)
			{
				DebugHelper.WriteException(ex);
				MessageBox.Show(string.Format(Resources.UploadManager_DownloadAndUploadFile_Download_failed, ex), "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		return false;
	}

	private void DoOCR()
	{
		if (Image != null && Info.DataType == EDataType.Image)
		{
			TaskHelpers.OCRImage(Image, Info.TaskSettings).GetAwaiter().GetResult();
		}
	}

	private bool LoadFileStream()
	{
		try
		{
			Data = new FileStream(Info.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		catch (Exception e)
		{
			e.ShowError();
			return false;
		}
		return true;
	}

	private void ThreadCompleted()
	{
		OnTaskCompleted();
	}

	private void uploader_ProgressChanged(ProgressManager progress)
	{
		if (progress != null)
		{
			Info.Progress = progress;
			OnUploadProgressChanged();
		}
	}

	private void OnStatusChanged()
	{
		if (this.StatusChanged != null)
		{
			threadWorker.InvokeAsync(delegate
			{
				this.StatusChanged(this);
			});
		}
	}

	private void OnImageReady()
	{
		if (this.ImageReady == null)
		{
			return;
		}
		Bitmap image = null;
		if (Program.Settings.TaskViewMode == TaskViewMode.ThumbnailView && Image != null)
		{
			image = (Bitmap)Image.Clone();
		}
		threadWorker.InvokeAsync(delegate
		{
			using (image)
			{
				this.ImageReady(this, image);
			}
		});
	}

	private void OnUploadStarted()
	{
		if (this.UploadStarted != null)
		{
			threadWorker.InvokeAsync(delegate
			{
				this.UploadStarted(this);
			});
		}
	}

	private void OnUploadCompleted()
	{
		if (this.UploadCompleted != null)
		{
			threadWorker.InvokeAsync(delegate
			{
				this.UploadCompleted(this);
			});
		}
	}

	private void OnUploadProgressChanged()
	{
		if (this.UploadProgressChanged != null)
		{
			threadWorker.InvokeAsync(delegate
			{
				this.UploadProgressChanged(this);
			});
		}
	}

	private void OnTaskCompleted()
	{
		Info.TaskEndTime = DateTime.Now;
		if (StopRequested)
		{
			Status = TaskStatus.Stopped;
			Info.Status = Resources.UploadTask_OnUploadCompleted_Stopped;
		}
		else if (Info.Result.IsError)
		{
			Status = TaskStatus.Failed;
			Info.Status = Resources.TaskManager_task_UploadCompleted_Error;
		}
		else
		{
			Status = TaskStatus.Completed;
			Info.Status = Resources.UploadTask_OnUploadCompleted_Done;
		}
		this.TaskCompleted?.Invoke(this);
		Dispose();
	}

	private void OnUploadersConfigWindowRequested(IUploaderService uploaderService)
	{
		if (this.UploadersConfigWindowRequested != null)
		{
			threadWorker.InvokeAsync(delegate
			{
				this.UploadersConfigWindowRequested(uploaderService);
			});
		}
	}

	public void Dispose()
	{
		if (Data != null)
		{
			Data.Dispose();
			Data = null;
		}
		if (!KeepImage && Image != null)
		{
			Image.Dispose();
			Image = null;
		}
	}
}
