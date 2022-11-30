using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.IndexerLib;
using ShareX.Properties;
using ShareX.UploadersLib;

namespace ShareX;

public static class UploadManager
{
	public static void UploadFile(string filePath, TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (!string.IsNullOrEmpty(filePath))
		{
			if (File.Exists(filePath))
			{
				TaskManager.Start(WorkerTask.CreateFileUploaderTask(filePath, taskSettings));
			}
			else if (Directory.Exists(filePath))
			{
				UploadFile(Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories), taskSettings);
			}
		}
	}

	public static void UploadFile(string[] files, TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (files != null && files.Length != 0 && (files.Length <= 10 || IsUploadConfirmed(files.Length)))
		{
			for (int i = 0; i < files.Length; i++)
			{
				UploadFile(files[i], taskSettings);
			}
		}
	}

	private static bool IsUploadConfirmed(int length)
	{
		if (Program.Settings.ShowMultiUploadWarning)
		{
			using (MyMessageBox myMessageBox = new MyMessageBox(string.Format(Resources.UploadManager_IsUploadConfirmed_Are_you_sure_you_want_to_upload__0__files_, length), "ShareX - " + Resources.UploadManager_IsUploadConfirmed_Upload_files, MessageBoxButtons.YesNo, Resources.UploadManager_IsUploadConfirmed_Don_t_show_this_message_again_))
			{
				myMessageBox.ShowDialog();
				Program.Settings.ShowMultiUploadWarning = !myMessageBox.IsChecked;
				return myMessageBox.DialogResult == DialogResult.Yes;
			}
		}
		return true;
	}

	public static void UploadFile(TaskSettings taskSettings = null)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Title = "ShareX - " + Resources.UploadManager_UploadFile_File_upload;
		if (!string.IsNullOrEmpty(Program.Settings.FileUploadDefaultDirectory) && Directory.Exists(Program.Settings.FileUploadDefaultDirectory))
		{
			openFileDialog.InitialDirectory = Program.Settings.FileUploadDefaultDirectory;
		}
		else
		{
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		}
		openFileDialog.Multiselect = true;
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			if (!string.IsNullOrEmpty(openFileDialog.FileName))
			{
				Program.Settings.FileUploadDefaultDirectory = Path.GetDirectoryName(openFileDialog.FileName);
			}
			UploadFile(openFileDialog.FileNames, taskSettings);
		}
	}

	public static void UploadFolder(TaskSettings taskSettings = null)
	{
		using FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
		folderSelectDialog.Title = "ShareX - " + Resources.UploadManager_UploadFolder_Folder_upload;
		if (!string.IsNullOrEmpty(Program.Settings.FileUploadDefaultDirectory) && Directory.Exists(Program.Settings.FileUploadDefaultDirectory))
		{
			folderSelectDialog.InitialDirectory = Program.Settings.FileUploadDefaultDirectory;
		}
		else
		{
			folderSelectDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		}
		if (folderSelectDialog.ShowDialog() && !string.IsNullOrEmpty(folderSelectDialog.FileName))
		{
			Program.Settings.FileUploadDefaultDirectory = folderSelectDialog.FileName;
			UploadFile(folderSelectDialog.FileName, taskSettings);
		}
	}

	public static void ProcessImageUpload(Bitmap bmp, TaskSettings taskSettings)
	{
		if (bmp != null)
		{
			if (!taskSettings.AdvancedSettings.ProcessImagesDuringClipboardUpload)
			{
				taskSettings.AfterCaptureJob = AfterCaptureTasks.UploadImageToHost;
			}
			RunImageTask(bmp, taskSettings);
		}
	}

	public static void ProcessTextUpload(string text, TaskSettings taskSettings)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		string url = text.Trim();
		if (URLHelpers.IsValidURL(url))
		{
			if (taskSettings.UploadSettings.ClipboardUploadURLContents)
			{
				DownloadAndUploadFile(url, taskSettings);
				return;
			}
			if (taskSettings.UploadSettings.ClipboardUploadShortenURL)
			{
				ShortenURL(url, taskSettings);
				return;
			}
			if (taskSettings.UploadSettings.ClipboardUploadShareURL)
			{
				ShareURL(url, taskSettings);
				return;
			}
		}
		if (taskSettings.UploadSettings.ClipboardUploadAutoIndexFolder && text.Length <= 260 && Directory.Exists(text))
		{
			IndexFolder(text, taskSettings);
		}
		else
		{
			UploadText(text, taskSettings, allowCustomText: true);
		}
	}

	public static void ProcessFilesUpload(string[] files, TaskSettings taskSettings)
	{
		if (files != null && files.Length != 0)
		{
			UploadFile(files, taskSettings);
		}
	}

	public static void ClipboardUpload(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (ClipboardHelpers.ContainsImage())
		{
			ProcessImageUpload(ClipboardHelpers.GetImage(), taskSettings);
		}
		else if (ClipboardHelpers.ContainsText())
		{
			ProcessTextUpload(ClipboardHelpers.GetText(), taskSettings);
		}
		else if (ClipboardHelpers.ContainsFileDropList())
		{
			ProcessFilesUpload(ClipboardHelpers.GetFileDropList(), taskSettings);
		}
	}

	public static void ClipboardUploadWithContentViewer(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		using ClipboardUploadForm clipboardUploadForm = new ClipboardUploadForm(taskSettings);
		clipboardUploadForm.ShowDialog();
	}

	public static void ClipboardUploadMainWindow(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (Program.Settings.ShowClipboardContentViewer)
		{
			using (ClipboardUploadForm clipboardUploadForm = new ClipboardUploadForm(taskSettings, showCheckBox: true))
			{
				clipboardUploadForm.ShowDialog();
				Program.Settings.ShowClipboardContentViewer = !clipboardUploadForm.DontShowThisWindow;
				return;
			}
		}
		ClipboardUpload(taskSettings);
	}

	public static void ShowTextUploadDialog(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		using TextUploadForm textUploadForm = new TextUploadForm();
		if (textUploadForm.ShowDialog() == DialogResult.OK)
		{
			string content = textUploadForm.Content;
			if (!string.IsNullOrEmpty(content))
			{
				UploadText(content, taskSettings);
			}
		}
	}

	public static void DragDropUpload(IDataObject data, TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (data.GetDataPresent(DataFormats.FileDrop, autoConvert: false))
		{
			UploadFile(data.GetData(DataFormats.FileDrop, autoConvert: false) as string[], taskSettings);
		}
		else if (data.GetDataPresent(DataFormats.Bitmap, autoConvert: false))
		{
			RunImageTask(data.GetData(DataFormats.Bitmap, autoConvert: false) as Bitmap, taskSettings);
		}
		else if (data.GetDataPresent(DataFormats.Text, autoConvert: false))
		{
			UploadText(data.GetData(DataFormats.Text, autoConvert: false) as string, taskSettings, allowCustomText: true);
		}
	}

	public static void UploadURL(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		string inputText = null;
		string text = ClipboardHelpers.GetText(checkContainsText: true);
		if (URLHelpers.IsValidURL(text))
		{
			inputText = text;
		}
		string inputText2 = InputBox.GetInputText("ShareX - " + Resources.UploadManager_UploadURL_URL_to_download_from_and_upload, inputText);
		if (!string.IsNullOrEmpty(inputText2))
		{
			DownloadAndUploadFile(inputText2, taskSettings);
		}
	}

	public static void ShowShortenURLDialog(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		string inputText = null;
		string text = ClipboardHelpers.GetText(checkContainsText: true);
		if (URLHelpers.IsValidURL(text))
		{
			inputText = text;
		}
		string inputText2 = InputBox.GetInputText("ShareX - " + Resources.UploadManager_ShowShortenURLDialog_ShortenURL, inputText, Resources.UploadManager_ShowShortenURLDialog_Shorten);
		if (!string.IsNullOrEmpty(inputText2))
		{
			ShortenURL(inputText2, taskSettings);
		}
	}

	public static void RunImageTask(Bitmap bmp, TaskSettings taskSettings, bool skipQuickTaskMenu = false, bool skipAfterCaptureWindow = false)
	{
		RunImageTask(new TaskMetadata(bmp), taskSettings, skipQuickTaskMenu, skipAfterCaptureWindow);
	}

	public static void RunImageTask(TaskMetadata metadata, TaskSettings taskSettings, bool skipQuickTaskMenu = false, bool skipAfterCaptureWindow = false)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (metadata == null || metadata.Image == null || taskSettings == null)
		{
			return;
		}
		if (!skipQuickTaskMenu && taskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.ShowQuickTaskMenu))
		{
			QuickTaskMenu quickTaskMenu = new QuickTaskMenu();
			quickTaskMenu.TaskInfoSelected = (QuickTaskMenu.TaskInfoSelectedEventHandler)Delegate.Combine(quickTaskMenu.TaskInfoSelected, (QuickTaskMenu.TaskInfoSelectedEventHandler)delegate(QuickTaskInfo taskInfo)
			{
				if (taskInfo == null)
				{
					RunImageTask(metadata, taskSettings, skipQuickTaskMenu: true);
				}
				else if (taskInfo.IsValid)
				{
					taskSettings.AfterCaptureJob = taskInfo.AfterCaptureTasks;
					taskSettings.AfterUploadJob = taskInfo.AfterUploadTasks;
					RunImageTask(metadata, taskSettings, skipQuickTaskMenu: true);
				}
			});
			quickTaskMenu.ShowMenu();
		}
		else
		{
			string fileName = null;
			if (skipAfterCaptureWindow || TaskHelpers.ShowAfterCaptureForm(taskSettings, out fileName, metadata))
			{
				TaskManager.Start(WorkerTask.CreateImageUploaderTask(metadata, taskSettings, fileName));
			}
		}
	}

	public static void UploadImage(Bitmap bmp, TaskSettings taskSettings = null)
	{
		if (bmp != null)
		{
			if (taskSettings == null)
			{
				taskSettings = TaskSettings.GetDefaultTaskSettings();
			}
			if (taskSettings.IsSafeTaskSettings)
			{
				taskSettings.UseDefaultAfterCaptureJob = false;
				taskSettings.AfterCaptureJob = AfterCaptureTasks.UploadImageToHost;
			}
			RunImageTask(bmp, taskSettings);
		}
	}

	public static void UploadImage(Bitmap bmp, ImageDestination imageDestination, FileDestination imageFileDestination, TaskSettings taskSettings = null)
	{
		if (bmp != null)
		{
			if (taskSettings == null)
			{
				taskSettings = TaskSettings.GetDefaultTaskSettings();
			}
			if (taskSettings.IsSafeTaskSettings)
			{
				taskSettings.UseDefaultAfterCaptureJob = false;
				taskSettings.AfterCaptureJob = AfterCaptureTasks.UploadImageToHost;
				taskSettings.UseDefaultDestinations = false;
				taskSettings.ImageDestination = imageDestination;
				taskSettings.ImageFileDestination = imageFileDestination;
			}
			RunImageTask(bmp, taskSettings);
		}
	}

	public static void UploadText(string text, TaskSettings taskSettings = null, bool allowCustomText = false)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (allowCustomText)
		{
			string textCustom = taskSettings.AdvancedSettings.TextCustom;
			if (!string.IsNullOrEmpty(textCustom))
			{
				if (taskSettings.AdvancedSettings.TextCustomEncodeInput)
				{
					text = HttpUtility.HtmlEncode(text);
				}
				text = textCustom.Replace("%input", text);
			}
		}
		TaskManager.Start(WorkerTask.CreateTextUploaderTask(text, taskSettings));
	}

	public static void UploadImageStream(Stream stream, string fileName, TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (stream != null && stream.Length > 0 && !string.IsNullOrEmpty(fileName))
		{
			TaskManager.Start(WorkerTask.CreateDataUploaderTask(EDataType.Image, stream, fileName, taskSettings));
		}
	}

	public static void ShortenURL(string url, TaskSettings taskSettings = null)
	{
		if (!string.IsNullOrEmpty(url))
		{
			if (taskSettings == null)
			{
				taskSettings = TaskSettings.GetDefaultTaskSettings();
			}
			TaskManager.Start(WorkerTask.CreateURLShortenerTask(url, taskSettings));
		}
	}

	public static void ShortenURL(string url, UrlShortenerType urlShortener)
	{
		if (!string.IsNullOrEmpty(url))
		{
			TaskSettings defaultTaskSettings = TaskSettings.GetDefaultTaskSettings();
			defaultTaskSettings.URLShortenerDestination = urlShortener;
			TaskManager.Start(WorkerTask.CreateURLShortenerTask(url, defaultTaskSettings));
		}
	}

	public static void ShareURL(string url, TaskSettings taskSettings = null)
	{
		if (!string.IsNullOrEmpty(url))
		{
			if (taskSettings == null)
			{
				taskSettings = TaskSettings.GetDefaultTaskSettings();
			}
			TaskManager.Start(WorkerTask.CreateShareURLTask(url, taskSettings));
		}
	}

	public static void ShareURL(string url, URLSharingServices urlSharingService)
	{
		if (!string.IsNullOrEmpty(url))
		{
			TaskSettings defaultTaskSettings = TaskSettings.GetDefaultTaskSettings();
			defaultTaskSettings.URLSharingServiceDestination = urlSharingService;
			TaskManager.Start(WorkerTask.CreateShareURLTask(url, defaultTaskSettings));
		}
	}

	public static void DownloadFile(string url, TaskSettings taskSettings = null)
	{
		DownloadFile(url, upload: false, taskSettings);
	}

	public static void DownloadAndUploadFile(string url, TaskSettings taskSettings = null)
	{
		DownloadFile(url, upload: true, taskSettings);
	}

	private static void DownloadFile(string url, bool upload, TaskSettings taskSettings = null)
	{
		if (!string.IsNullOrEmpty(url))
		{
			if (taskSettings == null)
			{
				taskSettings = TaskSettings.GetDefaultTaskSettings();
			}
			WorkerTask workerTask = WorkerTask.CreateDownloadTask(url, upload, taskSettings);
			if (workerTask != null)
			{
				TaskManager.Start(workerTask);
			}
		}
	}

	public static void IndexFolder(TaskSettings taskSettings = null)
	{
		using FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
		if (folderSelectDialog.ShowDialog())
		{
			IndexFolder(folderSelectDialog.FileName, taskSettings);
		}
	}

	public static void IndexFolder(string folderPath, TaskSettings taskSettings = null)
	{
		if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
		{
			return;
		}
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		taskSettings.ToolsSettings.IndexerSettings.BinaryUnits = Program.Settings.BinaryUnits;
		string source = null;
		Task.Run(delegate
		{
			source = Indexer.Index(folderPath, taskSettings.ToolsSettings.IndexerSettings);
		}).ContinueInCurrentContext(delegate
		{
			if (!string.IsNullOrEmpty(source))
			{
				WorkerTask workerTask = WorkerTask.CreateTextUploaderTask(source, taskSettings);
				workerTask.Info.FileName = Path.ChangeExtension(workerTask.Info.FileName, taskSettings.ToolsSettings.IndexerSettings.Output.ToString().ToLower());
				TaskManager.Start(workerTask);
			}
		});
	}
}
