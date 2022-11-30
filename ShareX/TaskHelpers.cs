using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.HistoryLib;
using ShareX.ImageEffectsLib;
using ShareX.IndexerLib;
using ShareX.MediaLib;
using ShareX.Properties;
using ShareX.ScreenCaptureLib;
using ShareX.UploadersLib;
using ShareX.UploadersLib.SharingServices;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;

namespace ShareX;

public static class TaskHelpers
{
	public static async Task ExecuteJob(HotkeyType job, CLICommand command = null)
	{
		await ExecuteJob(Program.DefaultTaskSettings, job, command);
	}

	public static async Task ExecuteJob(TaskSettings taskSettings)
	{
		await ExecuteJob(taskSettings, taskSettings.Job);
	}

	public static async Task ExecuteJob(TaskSettings taskSettings, HotkeyType job, CLICommand command = null)
	{
		if (job == HotkeyType.None)
		{
			return;
		}
		DebugHelper.WriteLine("Executing: " + job.GetLocalizedDescription());
		TaskSettings safeTaskSettings = TaskSettings.GetSafeTaskSettings(taskSettings);
		switch (job)
		{
		case HotkeyType.FileUpload:
			UploadManager.UploadFile(safeTaskSettings);
			break;
		case HotkeyType.FolderUpload:
			UploadManager.UploadFolder(safeTaskSettings);
			break;
		case HotkeyType.ClipboardUpload:
			UploadManager.ClipboardUpload(safeTaskSettings);
			break;
		case HotkeyType.ClipboardUploadWithContentViewer:
			UploadManager.ClipboardUploadWithContentViewer(safeTaskSettings);
			break;
		case HotkeyType.UploadText:
			UploadManager.ShowTextUploadDialog(safeTaskSettings);
			break;
		case HotkeyType.UploadURL:
			UploadManager.UploadURL(safeTaskSettings);
			break;
		case HotkeyType.DragDropUpload:
			OpenDropWindow(safeTaskSettings);
			break;
		case HotkeyType.ShortenURL:
			UploadManager.ShowShortenURLDialog(safeTaskSettings);
			break;
		case HotkeyType.TweetMessage:
			TweetMessage();
			break;
		case HotkeyType.StopUploads:
			TaskManager.StopAllTasks();
			break;
		case HotkeyType.PrintScreen:
			new CaptureFullscreen().Capture(safeTaskSettings);
			break;
		case HotkeyType.ActiveWindow:
			new CaptureActiveWindow().Capture(safeTaskSettings);
			break;
		case HotkeyType.ActiveMonitor:
			new CaptureActiveMonitor().Capture(safeTaskSettings);
			break;
		case HotkeyType.RectangleRegion:
			new CaptureRegion().Capture(safeTaskSettings);
			break;
		case HotkeyType.RectangleLight:
			new CaptureRegion(RegionCaptureType.Light).Capture(safeTaskSettings);
			break;
		case HotkeyType.RectangleTransparent:
			new CaptureRegion(RegionCaptureType.Transparent).Capture(safeTaskSettings);
			break;
		case HotkeyType.CustomRegion:
			new CaptureCustomRegion().Capture(safeTaskSettings);
			break;
		case HotkeyType.LastRegion:
			new CaptureLastRegion().Capture(safeTaskSettings);
			break;
		case HotkeyType.ScrollingCapture:
			OpenScrollingCapture(safeTaskSettings, forceSelection: true);
			break;
		case HotkeyType.AutoCapture:
			OpenAutoCapture(safeTaskSettings);
			break;
		case HotkeyType.StartAutoCapture:
			StartAutoCapture(safeTaskSettings);
			break;
		case HotkeyType.ScreenRecorder:
			StartScreenRecording(ScreenRecordOutput.FFmpeg, ScreenRecordStartMethod.Region, safeTaskSettings);
			break;
		case HotkeyType.ScreenRecorderActiveWindow:
			StartScreenRecording(ScreenRecordOutput.FFmpeg, ScreenRecordStartMethod.ActiveWindow, safeTaskSettings);
			break;
		case HotkeyType.ScreenRecorderCustomRegion:
			StartScreenRecording(ScreenRecordOutput.FFmpeg, ScreenRecordStartMethod.CustomRegion, safeTaskSettings);
			break;
		case HotkeyType.StartScreenRecorder:
			StartScreenRecording(ScreenRecordOutput.FFmpeg, ScreenRecordStartMethod.LastRegion, safeTaskSettings);
			break;
		case HotkeyType.ScreenRecorderGIF:
			StartScreenRecording(ScreenRecordOutput.GIF, ScreenRecordStartMethod.Region, safeTaskSettings);
			break;
		case HotkeyType.ScreenRecorderGIFActiveWindow:
			StartScreenRecording(ScreenRecordOutput.GIF, ScreenRecordStartMethod.ActiveWindow, safeTaskSettings);
			break;
		case HotkeyType.ScreenRecorderGIFCustomRegion:
			StartScreenRecording(ScreenRecordOutput.GIF, ScreenRecordStartMethod.CustomRegion, safeTaskSettings);
			break;
		case HotkeyType.StartScreenRecorderGIF:
			StartScreenRecording(ScreenRecordOutput.GIF, ScreenRecordStartMethod.LastRegion, safeTaskSettings);
			break;
		case HotkeyType.StopScreenRecording:
			StopScreenRecording();
			break;
		case HotkeyType.AbortScreenRecording:
			AbortScreenRecording();
			break;
		case HotkeyType.ColorPicker:
			ShowScreenColorPickerDialog(safeTaskSettings);
			break;
		case HotkeyType.ScreenColorPicker:
			OpenScreenColorPicker(safeTaskSettings);
			break;
		case HotkeyType.Ruler:
			OpenRuler(safeTaskSettings);
			break;
		case HotkeyType.ImageEditor:
			if (command != null && !string.IsNullOrEmpty(command.Parameter) && File.Exists(command.Parameter))
			{
				AnnotateImageFromFile(command.Parameter, safeTaskSettings);
			}
			else
			{
				OpenImageEditor(safeTaskSettings);
			}
			break;
		case HotkeyType.ImageEffects:
			if (command != null && !string.IsNullOrEmpty(command.Parameter) && File.Exists(command.Parameter))
			{
				OpenImageEffects(command.Parameter, taskSettings);
			}
			else
			{
				OpenImageEffects(taskSettings);
			}
			break;
		case HotkeyType.ImageViewer:
			if (command != null && !string.IsNullOrEmpty(command.Parameter) && File.Exists(command.Parameter))
			{
				OpenImageViewer(command.Parameter);
			}
			else
			{
				OpenImageViewer();
			}
			break;
		case HotkeyType.ImageCombiner:
			OpenImageCombiner(null, safeTaskSettings);
			break;
		case HotkeyType.ImageSplitter:
			OpenImageSplitter();
			break;
		case HotkeyType.ImageThumbnailer:
			OpenImageThumbnailer();
			break;
		case HotkeyType.VideoConverter:
			OpenVideoConverter(safeTaskSettings);
			break;
		case HotkeyType.VideoThumbnailer:
			OpenVideoThumbnailer(safeTaskSettings);
			break;
		case HotkeyType.OCR:
			await OCRImage(safeTaskSettings);
			break;
		case HotkeyType.QRCode:
			OpenQRCode();
			break;
		case HotkeyType.QRCodeDecodeFromScreen:
			OpenQRCodeDecodeFromScreen();
			break;
		case HotkeyType.HashCheck:
			OpenHashCheck();
			break;
		case HotkeyType.IndexFolder:
			UploadManager.IndexFolder();
			break;
		case HotkeyType.ClipboardViewer:
			OpenClipboardViewer();
			break;
		case HotkeyType.BorderlessWindow:
			OpenBorderlessWindow();
			break;
		case HotkeyType.InspectWindow:
			OpenInspectWindow();
			break;
		case HotkeyType.MonitorTest:
			OpenMonitorTest();
			break;
		case HotkeyType.DNSChanger:
			OpenDNSChanger();
			break;
		case HotkeyType.DisableHotkeys:
			ToggleHotkeys();
			break;
		case HotkeyType.OpenMainWindow:
			Program.MainForm.ForceActivate();
			break;
		case HotkeyType.OpenScreenshotsFolder:
			OpenScreenshotsFolder();
			break;
		case HotkeyType.OpenHistory:
			OpenHistory();
			break;
		case HotkeyType.OpenImageHistory:
			OpenImageHistory();
			break;
		case HotkeyType.ToggleActionsToolbar:
			ToggleActionsToolbar();
			break;
		case HotkeyType.ToggleTrayMenu:
			ToggleTrayMenu();
			break;
		case HotkeyType.ExitShareX:
			Program.MainForm.ForceClose();
			break;
		}
	}

	public static ImageData PrepareImage(Image img, TaskSettings taskSettings)
	{
		ImageData imageData = new ImageData();
		imageData.ImageStream = SaveImageAsStream(img, taskSettings.ImageSettings.ImageFormat, taskSettings);
		imageData.ImageFormat = taskSettings.ImageSettings.ImageFormat;
		if (taskSettings.ImageSettings.ImageAutoUseJPEG && taskSettings.ImageSettings.ImageFormat != EImageFormat.JPEG && imageData.ImageStream.Length > taskSettings.ImageSettings.ImageAutoUseJPEGSize * 1000)
		{
			imageData.ImageStream.Dispose();
			using (Bitmap img2 = ImageHelpers.FillBackground(img, Color.White))
			{
				if (taskSettings.ImageSettings.ImageAutoJPEGQuality)
				{
					imageData.ImageStream = ImageHelpers.SaveJPEGAutoQuality(img2, taskSettings.ImageSettings.ImageAutoUseJPEGSize * 1000, 2, 70);
				}
				else
				{
					imageData.ImageStream = ImageHelpers.SaveJPEG(img2, taskSettings.ImageSettings.ImageJPEGQuality);
				}
			}
			imageData.ImageFormat = EImageFormat.JPEG;
		}
		return imageData;
	}

	public static string CreateThumbnail(Bitmap bmp, string folder, string fileName, TaskSettings taskSettings)
	{
		if ((taskSettings.ImageSettings.ThumbnailWidth > 0 || taskSettings.ImageSettings.ThumbnailHeight > 0) && (!taskSettings.ImageSettings.ThumbnailCheckSize || (bmp.Width > taskSettings.ImageSettings.ThumbnailWidth && bmp.Height > taskSettings.ImageSettings.ThumbnailHeight)))
		{
			string fileName2 = Path.GetFileNameWithoutExtension(fileName) + taskSettings.ImageSettings.ThumbnailName + ".jpg";
			string text = HandleExistsFile(folder, fileName2, taskSettings);
			if (!string.IsNullOrEmpty(text))
			{
				using (Bitmap bmp2 = (Bitmap)bmp.Clone())
				{
					using Bitmap img = new Resize(taskSettings.ImageSettings.ThumbnailWidth, taskSettings.ImageSettings.ThumbnailHeight).Apply(bmp2);
					using Bitmap img2 = ImageHelpers.FillBackground(img, Color.White);
					ImageHelpers.SaveJPEG(img2, text, 90);
					return text;
				}
			}
		}
		return null;
	}

	public static MemoryStream SaveImageAsStream(Image img, EImageFormat imageFormat, TaskSettings taskSettings)
	{
		return SaveImageAsStream(img, imageFormat, taskSettings.ImageSettings.ImagePNGBitDepth, taskSettings.ImageSettings.ImageJPEGQuality, taskSettings.ImageSettings.ImageGIFQuality);
	}

	public static MemoryStream SaveImageAsStream(Image img, EImageFormat imageFormat, PNGBitDepth pngBitDepth = PNGBitDepth.Automatic, int jpegQuality = 90, GIFQuality gifQuality = GIFQuality.Default)
	{
		MemoryStream memoryStream = new MemoryStream();
		switch (imageFormat)
		{
		case EImageFormat.PNG:
			ImageHelpers.SavePNG(img, memoryStream, pngBitDepth);
			if (Program.Settings.PNGStripColorSpaceInformation)
			{
				using (memoryStream)
				{
					return ImageHelpers.PNGStripColorSpaceInformation(memoryStream);
				}
			}
			break;
		case EImageFormat.JPEG:
		{
			using Bitmap img2 = ImageHelpers.FillBackground(img, Color.White);
			ImageHelpers.SaveJPEG(img2, memoryStream, jpegQuality);
			return memoryStream;
		}
		case EImageFormat.GIF:
			ImageHelpers.SaveGIF(img, memoryStream, gifQuality);
			break;
		case EImageFormat.BMP:
			img.Save(memoryStream, ImageFormat.Bmp);
			break;
		case EImageFormat.TIFF:
			img.Save(memoryStream, ImageFormat.Tiff);
			break;
		}
		return memoryStream;
	}

	public static void SaveImageAsFile(Bitmap bmp, TaskSettings taskSettings, bool overwriteFile = false)
	{
		using ImageData imageData = PrepareImage(bmp, taskSettings);
		string screenshotsFolder = GetScreenshotsFolder(taskSettings);
		string fileName = GetFileName(taskSettings, imageData.ImageFormat.GetDescription(), bmp);
		string text = Path.Combine(screenshotsFolder, fileName);
		if (!overwriteFile)
		{
			text = HandleExistsFile(text, taskSettings);
		}
		if (!string.IsNullOrEmpty(text))
		{
			imageData.Write(text);
			DebugHelper.WriteLine("Image saved to file: " + text);
		}
	}

	public static string GetFileName(TaskSettings taskSettings, string extension, Bitmap bmp)
	{
		TaskMetadata metadata = new TaskMetadata(bmp);
		return GetFileName(taskSettings, extension, metadata);
	}

	public static string GetFileName(TaskSettings taskSettings, string extension = null, TaskMetadata metadata = null)
	{
		NameParser nameParser = new NameParser(NameParserType.FileName)
		{
			AutoIncrementNumber = Program.Settings.NameParserAutoIncrementNumber,
			MaxNameLength = taskSettings.AdvancedSettings.NamePatternMaxLength,
			MaxTitleLength = taskSettings.AdvancedSettings.NamePatternMaxTitleLength,
			CustomTimeZone = (taskSettings.UploadSettings.UseCustomTimeZone ? taskSettings.UploadSettings.CustomTimeZone : null)
		};
		if (metadata != null)
		{
			if (metadata.Image != null)
			{
				nameParser.ImageWidth = metadata.Image.Width;
				nameParser.ImageHeight = metadata.Image.Height;
			}
			nameParser.WindowText = metadata.WindowTitle;
			nameParser.ProcessName = metadata.ProcessName;
		}
		string text = ((string.IsNullOrEmpty(taskSettings.UploadSettings.NameFormatPatternActiveWindow) || string.IsNullOrEmpty(nameParser.WindowText)) ? nameParser.Parse(taskSettings.UploadSettings.NameFormatPattern) : nameParser.Parse(taskSettings.UploadSettings.NameFormatPatternActiveWindow));
		Program.Settings.NameParserAutoIncrementNumber = nameParser.AutoIncrementNumber;
		if (!string.IsNullOrEmpty(extension))
		{
			text = text + "." + extension.TrimStart('.');
		}
		return text;
	}

	public static string GetScreenshotsFolder(TaskSettings taskSettings = null, TaskMetadata metadata = null)
	{
		NameParser nameParser = new NameParser(NameParserType.FilePath);
		if (metadata != null)
		{
			if (metadata.Image != null)
			{
				nameParser.ImageWidth = metadata.Image.Width;
				nameParser.ImageHeight = metadata.Image.Height;
			}
			nameParser.WindowText = metadata.WindowTitle;
			nameParser.ProcessName = metadata.ProcessName;
		}
		string path;
		if (taskSettings != null && taskSettings.OverrideScreenshotsFolder && !string.IsNullOrEmpty(taskSettings.ScreenshotsFolder))
		{
			path = nameParser.Parse(taskSettings.ScreenshotsFolder);
		}
		else
		{
			string pattern = ((string.IsNullOrEmpty(Program.Settings.SaveImageSubFolderPatternWindow) || string.IsNullOrEmpty(nameParser.WindowText)) ? Program.Settings.SaveImageSubFolderPattern : Program.Settings.SaveImageSubFolderPatternWindow);
			string path2 = nameParser.Parse(pattern);
			path = Path.Combine(Program.ScreenshotsParentFolder, path2);
		}
		return FileHelpers.GetAbsolutePath(path);
	}

	public static bool ShowAfterCaptureForm(TaskSettings taskSettings, out string fileName, TaskMetadata metadata = null, string filePath = null)
	{
		fileName = null;
		if (taskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.ShowAfterCaptureWindow))
		{
			AfterCaptureForm afterCaptureForm = null;
			try
			{
				afterCaptureForm = (string.IsNullOrEmpty(filePath) ? new AfterCaptureForm(metadata, taskSettings) : new AfterCaptureForm(filePath, taskSettings));
				if (afterCaptureForm.ShowDialog() == DialogResult.Cancel)
				{
					metadata?.Dispose();
					return false;
				}
				fileName = afterCaptureForm.FileName;
			}
			finally
			{
				afterCaptureForm.Dispose();
			}
		}
		return true;
	}

	public static void PrintImage(Image img)
	{
		if (Program.Settings.DontShowPrintSettingsDialog)
		{
			using (PrintHelper printHelper = new PrintHelper(img))
			{
				printHelper.Settings = Program.Settings.PrintSettings;
				printHelper.Print();
				return;
			}
		}
		using PrintForm printForm = new PrintForm(img, Program.Settings.PrintSettings);
		printForm.ShowDialog();
	}

	public static Bitmap ApplyImageEffects(Bitmap bmp, TaskSettingsImage taskSettingsImage)
	{
		if (bmp != null)
		{
			bmp = ImageHelpers.NonIndexedBitmap(bmp);
			if (taskSettingsImage.ShowImageEffectsWindowAfterCapture)
			{
				using ImageEffectsForm imageEffectsForm = new ImageEffectsForm(bmp, taskSettingsImage.ImageEffectPresets, taskSettingsImage.SelectedImageEffectPreset);
				imageEffectsForm.ShowDialog();
				taskSettingsImage.SelectedImageEffectPreset = imageEffectsForm.SelectedPresetIndex;
			}
			if (taskSettingsImage.ImageEffectPresets.IsValidIndex(taskSettingsImage.SelectedImageEffectPreset))
			{
				using (bmp)
				{
					return taskSettingsImage.ImageEffectPresets[taskSettingsImage.SelectedImageEffectPreset].ApplyEffects(bmp);
				}
			}
		}
		return bmp;
	}

	public static void AddDefaultExternalPrograms(TaskSettings taskSettings)
	{
		if (taskSettings.ExternalPrograms == null)
		{
			taskSettings.ExternalPrograms = new List<ExternalProgram>();
		}
		AddExternalProgramFromRegistry(taskSettings, "Paint", "mspaint.exe");
		AddExternalProgramFromRegistry(taskSettings, "Paint.NET", "PaintDotNet.exe");
		AddExternalProgramFromRegistry(taskSettings, "Adobe Photoshop", "Photoshop.exe");
		AddExternalProgramFromRegistry(taskSettings, "IrfanView", "i_view32.exe");
		AddExternalProgramFromRegistry(taskSettings, "XnView", "xnview.exe");
	}

	private static void AddExternalProgramFromRegistry(TaskSettings taskSettings, string name, string fileName)
	{
		if (taskSettings.ExternalPrograms.Exists((ExternalProgram x) => x.Name == name))
		{
			return;
		}
		try
		{
			string text = RegistryHelpers.SearchProgramPath(fileName);
			if (!string.IsNullOrEmpty(text))
			{
				ExternalProgram item = new ExternalProgram(name, text);
				taskSettings.ExternalPrograms.Add(item);
			}
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	public static string HandleExistsFile(string folder, string fileName, TaskSettings taskSettings)
	{
		return HandleExistsFile(Path.Combine(folder, fileName), taskSettings);
	}

	public static string HandleExistsFile(string filePath, TaskSettings taskSettings)
	{
		if (File.Exists(filePath))
		{
			switch (taskSettings.ImageSettings.FileExistAction)
			{
			case FileExistAction.Ask:
			{
				using FileExistForm fileExistForm = new FileExistForm(filePath);
				fileExistForm.ShowDialog();
				filePath = fileExistForm.FilePath;
				return filePath;
			}
			case FileExistAction.UniqueName:
				filePath = FileHelpers.GetUniqueFilePath(filePath);
				break;
			case FileExistAction.Cancel:
				filePath = "";
				break;
			}
		}
		return filePath;
	}

	public static void OpenDropWindow(TaskSettings taskSettings = null)
	{
		DropForm.GetInstance(Program.Settings.DropSize, Program.Settings.DropOffset, Program.Settings.DropAlignment, Program.Settings.DropOpacity, Program.Settings.DropHoverOpacity, taskSettings).ForceActivate();
	}

	public static void StartScreenRecording(ScreenRecordOutput outputType, ScreenRecordStartMethod startMethod, TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		ScreenRecordManager.StartStopRecording(outputType, startMethod, taskSettings);
	}

	public static void StopScreenRecording()
	{
		ScreenRecordManager.StopRecording();
	}

	public static void AbortScreenRecording()
	{
		ScreenRecordManager.AbortRecording();
	}

	public static void OpenScrollingCapture(TaskSettings taskSettings = null, bool forceSelection = false)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		ScrollingCaptureForm scrollingCaptureForm = new ScrollingCaptureForm(taskSettings.CaptureSettingsReference.ScrollingCaptureOptions, taskSettings.CaptureSettings.SurfaceOptions, forceSelection);
		scrollingCaptureForm.ImageProcessRequested += delegate(Bitmap img)
		{
			UploadManager.RunImageTask(img, taskSettings);
		};
		scrollingCaptureForm.Show();
	}

	public static void OpenAutoCapture(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		AutoCaptureForm.Instance.TaskSettings = taskSettings;
		AutoCaptureForm.Instance.ForceActivate();
	}

	public static void StartAutoCapture(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (!AutoCaptureForm.IsRunning)
		{
			AutoCaptureForm instance = AutoCaptureForm.Instance;
			instance.TaskSettings = taskSettings;
			instance.Show();
			instance.Execute();
		}
	}

	public static void OpenScreenshotsFolder()
	{
		string screenshotsFolder = GetScreenshotsFolder();
		if (Directory.Exists(screenshotsFolder))
		{
			FileHelpers.OpenFolder(screenshotsFolder);
		}
		else
		{
			FileHelpers.OpenFolder(Program.ScreenshotsParentFolder);
		}
	}

	public static void OpenHistory()
	{
		new HistoryForm(Program.HistoryFilePath, Program.Settings.HistorySettings, delegate(string filePath)
		{
			UploadManager.UploadFile(filePath);
		}, delegate(string filePath)
		{
			AnnotateImageFromFile(filePath);
		}).Show();
	}

	public static void OpenImageHistory()
	{
		new ImageHistoryForm(Program.HistoryFilePath, Program.Settings.ImageHistorySettings, delegate(string filePath)
		{
			UploadManager.UploadFile(filePath);
		}, delegate(string filePath)
		{
			AnnotateImageFromFile(filePath);
		}).Show();
	}

	public static void OpenDebugLog()
	{
		DebugForm form = DebugForm.GetFormInstance(DebugHelper.Logger);
		if (!form.HasUploadRequested)
		{
			form.UploadRequested += delegate(string text)
			{
				if (MessageBox.Show(form, Resources.MainForm_UploadDebugLogWarning, "ShareX", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
				{
					UploadManager.UploadText(text);
				}
			};
		}
		form.ForceActivate();
	}

	public static void ShowScreenColorPickerDialog(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		taskSettings.CaptureSettings.SurfaceOptions.ScreenColorPickerInfoText = taskSettings.ToolsSettings.ScreenColorPickerInfoText;
		RegionCaptureTasks.ShowScreenColorPickerDialog(taskSettings.CaptureSettingsReference.SurfaceOptions);
	}

	public static void OpenScreenColorPicker(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		taskSettings.CaptureSettings.SurfaceOptions.ScreenColorPickerInfoText = taskSettings.ToolsSettings.ScreenColorPickerInfoText;
		PointInfo pointInfo = RegionCaptureTasks.GetPointInfo(taskSettings.CaptureSettings.SurfaceOptions);
		if (pointInfo != null)
		{
			string text = ((Control.ModifierKeys != Keys.Control) ? taskSettings.ToolsSettings.ScreenColorPickerFormat : taskSettings.ToolsSettings.ScreenColorPickerFormatCtrl);
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = CodeMenuEntryPixelInfo.Parse(text, pointInfo.Color, pointInfo.Position);
				ClipboardHelpers.CopyText(text2);
				ShowNotificationTip(string.Format(Resources.TaskHelpers_OpenQuickScreenColorPicker_Copied_to_clipboard___0_, text2), "ShareX - " + Resources.ScreenColorPicker);
			}
		}
	}

	public static void OpenHashCheck()
	{
		new HashCheckForm().Show();
	}

	public static void OpenDirectoryIndexer(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		IndexerSettings indexerSettings = taskSettings.ToolsSettingsReference.IndexerSettings;
		indexerSettings.BinaryUnits = Program.Settings.BinaryUnits;
		DirectoryIndexerForm directoryIndexerForm = new DirectoryIndexerForm(indexerSettings);
		directoryIndexerForm.UploadRequested += delegate(string source)
		{
			WorkerTask workerTask = WorkerTask.CreateTextUploaderTask(source, taskSettings);
			workerTask.Info.FileName = Path.ChangeExtension(workerTask.Info.FileName, indexerSettings.Output.ToString().ToLowerInvariant());
			TaskManager.Start(workerTask);
		};
		directoryIndexerForm.Show();
	}

	public static void OpenImageCombiner(IEnumerable<string> imageFiles = null, TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		ImageCombinerForm imageCombinerForm = new ImageCombinerForm(taskSettings.ToolsSettingsReference.ImageCombinerOptions, imageFiles);
		imageCombinerForm.ProcessRequested += delegate(Bitmap bmp)
		{
			UploadManager.RunImageTask(bmp, taskSettings);
		};
		imageCombinerForm.Show();
	}

	public static void CombineImages(IEnumerable<string> imageFiles, Orientation orientation, TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		Bitmap bitmap = ImageHelpers.CombineImages(imageFiles, orientation, taskSettings.ToolsSettings.ImageCombinerOptions.Alignment, taskSettings.ToolsSettings.ImageCombinerOptions.Space, taskSettings.ToolsSettings.ImageCombinerOptions.AutoFillBackground);
		if (bitmap != null)
		{
			UploadManager.RunImageTask(bitmap, taskSettings);
		}
	}

	public static void OpenImageSplitter()
	{
		new ImageSplitterForm().Show();
	}

	public static void OpenImageThumbnailer()
	{
		new ImageThumbnailerForm().Show();
	}

	public static void OpenVideoConverter(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (CheckFFmpeg(taskSettings))
		{
			new VideoConverterForm(taskSettings.CaptureSettings.FFmpegOptions.FFmpegPath, taskSettings.ToolsSettingsReference.VideoConverterOptions).Show();
		}
	}

	public static void OpenVideoThumbnailer(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (!CheckFFmpeg(taskSettings))
		{
			return;
		}
		taskSettings.ToolsSettingsReference.VideoThumbnailOptions.DefaultOutputDirectory = GetScreenshotsFolder(taskSettings);
		VideoThumbnailerForm videoThumbnailerForm = new VideoThumbnailerForm(taskSettings.CaptureSettings.FFmpegOptions.FFmpegPath, taskSettings.ToolsSettingsReference.VideoThumbnailOptions);
		videoThumbnailerForm.ThumbnailsTaken += delegate(List<VideoThumbnailInfo> thumbnails)
		{
			if (taskSettings.ToolsSettingsReference.VideoThumbnailOptions.UploadThumbnails)
			{
				foreach (VideoThumbnailInfo thumbnail in thumbnails)
				{
					UploadManager.UploadFile(thumbnail.FilePath, taskSettings);
				}
			}
		};
		videoThumbnailerForm.Show();
	}

	public static void OpenBorderlessWindow(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		new BorderlessWindowForm(taskSettings.ToolsSettingsReference.BorderlessWindowSettings).Show();
	}

	public static void OpenInspectWindow()
	{
		new InspectWindowForm().Show();
	}

	public static void OpenClipboardViewer()
	{
		new ClipboardViewerForm().Show();
	}

	public static void OpenImageEditor(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		using EditorStartupForm editorStartupForm = new EditorStartupForm(taskSettings.CaptureSettingsReference.SurfaceOptions);
		if (editorStartupForm.ShowDialog() == DialogResult.OK)
		{
			AnnotateImageAsync(editorStartupForm.Image, editorStartupForm.ImageFilePath, taskSettings);
		}
	}

	public static void AnnotateImageFromFile(string filePath, TaskSettings taskSettings = null)
	{
		if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
		{
			if (taskSettings == null)
			{
				taskSettings = TaskSettings.GetDefaultTaskSettings();
			}
			AnnotateImageAsync(ImageHelpers.LoadImage(filePath), filePath, taskSettings);
		}
		else
		{
			MessageBox.Show("File does not exist:" + Environment.NewLine + filePath, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	public static void AnnotateImageAsync(Bitmap bmp, string filePath, TaskSettings taskSettings)
	{
		ThreadWorker threadWorker = new ThreadWorker();
		threadWorker.DoWork += delegate
		{
			bmp = AnnotateImage(bmp, filePath, taskSettings);
		};
		threadWorker.Completed += delegate
		{
			if (bmp != null)
			{
				UploadManager.RunImageTask(bmp, taskSettings);
			}
		};
		threadWorker.Start(ApartmentState.STA);
	}

	public static Bitmap AnnotateImage(Bitmap bmp, string filePath, TaskSettings taskSettings, bool taskMode = false)
	{
		if (bmp != null)
		{
			bmp = ImageHelpers.NonIndexedBitmap(bmp);
			using (bmp)
			{
				int mode = (taskMode ? 6 : 5);
				RegionCaptureOptions surfaceOptions = taskSettings.CaptureSettingsReference.SurfaceOptions;
				using RegionCaptureForm regionCaptureForm = new RegionCaptureForm((RegionCaptureMode)mode, surfaceOptions, bmp);
				regionCaptureForm.ImageFilePath = filePath;
				regionCaptureForm.SaveImageRequested += delegate(Bitmap output, string newFilePath)
				{
					using (output)
					{
						if (string.IsNullOrEmpty(newFilePath))
						{
							string screenshotsFolder2 = GetScreenshotsFolder(taskSettings);
							string fileName2 = GetFileName(taskSettings, taskSettings.ImageSettings.ImageFormat.GetDescription(), output);
							newFilePath = Path.Combine(screenshotsFolder2, fileName2);
						}
						ImageHelpers.SaveImage(output, newFilePath);
						return newFilePath;
					}
				};
				regionCaptureForm.SaveImageAsRequested += delegate(Bitmap output, string newFilePath)
				{
					using (output)
					{
						if (string.IsNullOrEmpty(newFilePath))
						{
							string screenshotsFolder = GetScreenshotsFolder(taskSettings);
							string fileName = GetFileName(taskSettings, taskSettings.ImageSettings.ImageFormat.GetDescription(), output);
							newFilePath = Path.Combine(screenshotsFolder, fileName);
						}
						newFilePath = ImageHelpers.SaveImageFileDialog(output, newFilePath);
						return newFilePath;
					}
				};
				regionCaptureForm.CopyImageRequested += delegate(Bitmap output)
				{
					Program.MainForm.InvokeSafe(delegate
					{
						ClipboardHelpers.CopyImage(output);
					});
				};
				regionCaptureForm.UploadImageRequested += delegate(Bitmap output)
				{
					Program.MainForm.InvokeSafe(delegate
					{
						UploadManager.UploadImage(output, taskSettings);
					});
				};
				regionCaptureForm.PrintImageRequested += delegate(Bitmap output)
				{
					Program.MainForm.InvokeSafe(delegate
					{
						using (output)
						{
							PrintImage(output);
						}
					});
				};
				regionCaptureForm.ShowDialog();
				switch (regionCaptureForm.Result)
				{
				case RegionResult.Close:
				case RegionResult.AnnotateCancelTask:
					return null;
				case RegionResult.Region:
				case RegionResult.AnnotateRunAfterCaptureTasks:
					return regionCaptureForm.GetResultImage();
				case RegionResult.Fullscreen:
				case RegionResult.AnnotateContinueTask:
					return (Bitmap)regionCaptureForm.Canvas.Clone();
				case RegionResult.LastRegion:
				case RegionResult.Monitor:
				case RegionResult.ActiveMonitor:
					break;
				}
			}
		}
		return null;
	}

	public static void OpenImageEffects(TaskSettings taskSettings = null)
	{
		OpenImageEffects(ImageHelpers.OpenImageFileDialog(), taskSettings);
	}

	public static void OpenImageEffects(string filePath, TaskSettings taskSettings = null)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return;
		}
		Bitmap bitmap = ImageHelpers.LoadImage(filePath);
		if (bitmap == null)
		{
			return;
		}
		bitmap = ImageHelpers.NonIndexedBitmap(bitmap);
		if (taskSettings == null)
		{
			taskSettings = Program.DefaultTaskSettings;
		}
		using ImageEffectsForm imageEffectsForm = new ImageEffectsForm(bitmap, taskSettings.ImageSettings.ImageEffectPresets, taskSettings.ImageSettings.SelectedImageEffectPreset);
		imageEffectsForm.EnableToolMode(delegate(Bitmap x)
		{
			UploadManager.RunImageTask(x, taskSettings);
		}, filePath);
		imageEffectsForm.ShowDialog();
	}

	public static ImageEffectsForm OpenImageEffectsSingleton(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = Program.DefaultTaskSettings;
		}
		bool num = !ImageEffectsForm.IsInstanceActive;
		ImageEffectsForm imageEffectsForm = ImageEffectsForm.GetFormInstance(taskSettings.ImageSettings.ImageEffectPresets, taskSettings.ImageSettings.SelectedImageEffectPreset);
		if (num)
		{
			imageEffectsForm.FormClosed += delegate
			{
				taskSettings.ImageSettings.SelectedImageEffectPreset = imageEffectsForm.SelectedPresetIndex;
			};
			imageEffectsForm.Show();
		}
		else
		{
			imageEffectsForm.ForceActivate();
		}
		return imageEffectsForm;
	}

	public static void OpenImageViewer()
	{
		OpenImageViewer(ImageHelpers.OpenImageFileDialog());
	}

	public static void OpenImageViewer(string filePath)
	{
		if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
		{
			string[] files = Directory.GetFiles(Path.GetDirectoryName(filePath));
			if (files != null && files.Length != 0)
			{
				int imageIndex = Array.IndexOf(files, filePath);
				ImageViewer.ShowImage(files, imageIndex);
			}
		}
	}

	public static void OpenMonitorTest()
	{
		using MonitorTestForm monitorTestForm = new MonitorTestForm();
		monitorTestForm.ShowDialog();
	}

	public static void OpenDNSChanger()
	{
		if (Helpers.IsAdministrator())
		{
			new DNSChangerForm().Show();
		}
		else
		{
			RunShareXAsAdmin("-dnschanger");
		}
	}

	public static void RunShareXAsAdmin(string arguments = null)
	{
		try
		{
			using Process process = new Process();
			ProcessStartInfo processStartInfo2 = (process.StartInfo = new ProcessStartInfo
			{
				FileName = Application.ExecutablePath,
				Arguments = arguments,
				UseShellExecute = true,
				Verb = "runas"
			});
			process.Start();
		}
		catch
		{
		}
	}

	public static void OpenQRCode()
	{
		QRCodeForm.EncodeClipboard().Show();
	}

	public static void OpenQRCodeDecodeFromScreen()
	{
		QRCodeForm.OpenFormDecodeFromScreen();
	}

	public static void OpenRuler(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		RegionCaptureTasks.ShowScreenRuler(taskSettings.CaptureSettings.SurfaceOptions);
	}

	public static void SearchImageUsingGoogle(string url)
	{
		new GoogleImageSearchSharingService().CreateSharer(null, null).ShareURL(url);
	}

	public static void SearchImageUsingBing(string url)
	{
		new BingVisualSearchSharingService().CreateSharer(null, null).ShareURL(url);
	}

	public static async Task OCRImage(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		using Bitmap bmp = RegionCaptureTasks.GetRegionImage(taskSettings.CaptureSettings.SurfaceOptions);
		await OCRImage(bmp, taskSettings);
	}

	public static async Task OCRImage(string filePath, TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
		{
			using Bitmap bmp = ImageHelpers.LoadImage(filePath);
			await OCRImage(bmp, taskSettings, filePath);
		}
	}

	public static async Task OCRImage(Bitmap bmp, TaskSettings taskSettings = null, string filePath = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		await OCRImage(bmp, taskSettings.CaptureSettingsReference.OCROptions, filePath);
	}

	private static async Task OCRImage(Bitmap bmp, OCROptions options, string filePath = null)
	{
		try
		{
			OCRHelper.ThrowIfNotSupported();
			if (bmp == null)
			{
				return;
			}
			if (!options.Silent)
			{
				using (OCRForm oCRForm = new OCRForm(bmp, options))
				{
					oCRForm.ShowDialog();
					if (!string.IsNullOrEmpty(oCRForm.Result) && !string.IsNullOrEmpty(filePath))
					{
						File.WriteAllText(Path.ChangeExtension(filePath, "txt"), oCRForm.Result, Encoding.UTF8);
					}
					return;
				}
			}
			await AsyncOCRImage(bmp, options, filePath);
		}
		catch (Exception e)
		{
			e.ShowError(fullError: false);
		}
	}

	private static async Task AsyncOCRImage(Bitmap bmp, OCROptions options, string filePath = null)
	{
		ShowNotificationTip(Resources.OCRForm_AutoProcessing);
		string result = null;
		if (bmp != null)
		{
			result = await OCRHelper.OCR(bmp, options.Language, options.ScaleFactor, options.SingleLine);
		}
		if (!string.IsNullOrEmpty(result))
		{
			Program.MainForm.InvokeSafe(delegate
			{
				ClipboardHelpers.CopyText(result);
			});
			if (!string.IsNullOrEmpty(filePath))
			{
				File.WriteAllText(Path.ChangeExtension(filePath, "txt"), result, Encoding.UTF8);
			}
			ShowNotificationTip(Resources.OCRForm_AutoComplete);
		}
		else
		{
			ShowNotificationTip(Resources.OCRForm_AutoCompleteFail);
		}
	}

	public static void TweetMessage()
	{
		if (!IsUploadAllowed())
		{
			return;
		}
		if (Program.UploadersConfig != null && Program.UploadersConfig.TwitterOAuthInfoList != null)
		{
			OAuthInfo twitterOAuth = Program.UploadersConfig.TwitterOAuthInfoList.ReturnIfValidIndex(Program.UploadersConfig.TwitterSelectedAccount);
			if (twitterOAuth != null && OAuthInfo.CheckOAuth(twitterOAuth))
			{
				Task.Run(delegate
				{
					using TwitterTweetForm twitterTweetForm = new TwitterTweetForm(twitterOAuth);
					if (twitterTweetForm.ShowDialog() == DialogResult.OK && twitterTweetForm.IsTweetSent)
					{
						ShowNotificationTip(Resources.TaskHelpers_TweetMessage_Tweet_successfully_sent_);
					}
				});
				return;
			}
		}
		MessageBox.Show(Resources.TaskHelpers_TweetMessage_Unable_to_find_valid_Twitter_account_, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	public static EDataType FindDataType(string filePath, TaskSettings taskSettings)
	{
		if (FileHelpers.CheckExtension(filePath, taskSettings.AdvancedSettings.ImageExtensions))
		{
			return EDataType.Image;
		}
		if (FileHelpers.CheckExtension(filePath, taskSettings.AdvancedSettings.TextExtensions))
		{
			return EDataType.Text;
		}
		return EDataType.File;
	}

	public static bool ToggleHotkeys()
	{
		bool num = !Program.Settings.DisableHotkeys;
		ToggleHotkeys(num);
		return num;
	}

	public static void ToggleHotkeys(bool disableHotkeys)
	{
		Program.Settings.DisableHotkeys = disableHotkeys;
		Program.HotkeyManager.ToggleHotkeys(disableHotkeys);
		Program.MainForm.UpdateToggleHotkeyButton();
		ShowNotificationTip(disableHotkeys ? Resources.TaskHelpers_ToggleHotkeys_Hotkeys_disabled_ : Resources.TaskHelpers_ToggleHotkeys_Hotkeys_enabled_);
	}

	public static bool CheckFFmpeg(TaskSettings taskSettings)
	{
		string text = taskSettings.CaptureSettings.FFmpegOptions.FFmpegPath;
		if (string.IsNullOrEmpty(text))
		{
			text = Program.DefaultFFmpegFilePath;
		}
		if (!File.Exists(text))
		{
			if (MessageBox.Show(string.Format(Resources.FFmpeg_does_not_exist, text), "ShareX - " + Resources.FFmpeg_Missing + " ffmpeg.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
			{
				return false;
			}
			switch (FFmpegGitHubDownloader.DownloadFFmpeg(async: false, DownloaderForm_InstallRequested))
			{
			case DialogResult.OK:
			{
				FFmpegOptions fFmpegOptions = Program.DefaultTaskSettings.CaptureSettings.FFmpegOptions;
				FFmpegOptions fFmpegOptions2 = taskSettings.TaskSettingsReference.CaptureSettings.FFmpegOptions;
				string text2 = (taskSettings.CaptureSettings.FFmpegOptions.CLIPath = Program.DefaultFFmpegFilePath);
				string text5 = (fFmpegOptions.CLIPath = (fFmpegOptions2.CLIPath = text2));
				break;
			}
			case DialogResult.Cancel:
				return false;
			}
		}
		return true;
	}

	private static void DownloaderForm_InstallRequested(string filePath)
	{
		if (FFmpegGitHubDownloader.ExtractFFmpeg(filePath, Program.ToolsFolder))
		{
			MessageBox.Show(Resources.FFmpeg_FFmpeg_successfully_downloaded, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			MessageBox.Show(Resources.FFmpeg_Download_of_FFmpeg_failed, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PlayCaptureSound(TaskSettings taskSettings)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (taskSettings.GeneralSettings.UseCustomCaptureSound && !string.IsNullOrEmpty(taskSettings.GeneralSettings.CustomCaptureSoundPath))
		{
			Helpers.PlaySoundAsync(taskSettings.GeneralSettings.CustomCaptureSoundPath);
		}
		else
		{
			Helpers.PlaySoundAsync(Resources.CaptureSound);
		}
	}

	public static void PlayTaskCompleteSound(TaskSettings taskSettings)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (taskSettings.GeneralSettings.UseCustomTaskCompletedSound && !string.IsNullOrEmpty(taskSettings.GeneralSettings.CustomTaskCompletedSoundPath))
		{
			Helpers.PlaySoundAsync(taskSettings.GeneralSettings.CustomTaskCompletedSoundPath);
		}
		else
		{
			Helpers.PlaySoundAsync(Resources.TaskCompletedSound);
		}
	}

	public static void PlayErrorSound(TaskSettings taskSettings)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		if (taskSettings.GeneralSettings.UseCustomErrorSound && !string.IsNullOrEmpty(taskSettings.GeneralSettings.CustomErrorSoundPath))
		{
			Helpers.PlaySoundAsync(taskSettings.GeneralSettings.CustomErrorSoundPath);
		}
		else
		{
			Helpers.PlaySoundAsync(Resources.ErrorSound);
		}
	}

	public static void OpenUploadersConfigWindow(IUploaderService uploaderService = null)
	{
		SettingManager.WaitUploadersConfig();
		bool num = !UploadersConfigForm.IsInstanceActive;
		UploadersConfigForm formInstance = UploadersConfigForm.GetFormInstance(Program.UploadersConfig);
		if (num)
		{
			formInstance.FormClosed += delegate
			{
				SettingManager.SaveUploadersConfigAsync();
			};
			if (uploaderService != null)
			{
				formInstance.NavigateToTabPage(uploaderService.GetUploadersConfigTabPage(formInstance));
			}
			formInstance.Show();
		}
		else
		{
			if (uploaderService != null)
			{
				formInstance.NavigateToTabPage(uploaderService.GetUploadersConfigTabPage(formInstance));
			}
			formInstance.ForceActivate();
		}
	}

	public static void OpenCustomUploaderSettingsWindow()
	{
		SettingManager.WaitUploadersConfig();
		bool num = !CustomUploaderSettingsForm.IsInstanceActive;
		CustomUploaderSettingsForm formInstance = CustomUploaderSettingsForm.GetFormInstance(Program.UploadersConfig);
		if (num)
		{
			formInstance.FormClosed += delegate
			{
				SettingManager.SaveUploadersConfigAsync();
			};
			formInstance.Show();
		}
		else
		{
			formInstance.ForceActivate();
		}
	}

	public static Image FindMenuIcon<T>(T value) where T : Enum
	{
		if (value is AfterCaptureTasks)
		{
			object obj = value;
			AfterCaptureTasks afterCaptureTasks = (AfterCaptureTasks)((obj is AfterCaptureTasks) ? obj : null);
			switch (afterCaptureTasks)
			{
			default:
				throw new Exception("Icon missing for after capture task: " + afterCaptureTasks);
			case AfterCaptureTasks.ShowQuickTaskMenu:
				return Resources.ui_menu_blue;
			case AfterCaptureTasks.ShowAfterCaptureWindow:
				return Resources.application_text_image;
			case AfterCaptureTasks.AddImageEffects:
				return Resources.image_saturation;
			case AfterCaptureTasks.AnnotateImage:
				return Resources.image_pencil;
			case AfterCaptureTasks.CopyImageToClipboard:
				return Resources.clipboard_paste_image;
			case AfterCaptureTasks.SendImageToPrinter:
				return Resources.printer;
			case AfterCaptureTasks.SaveImageToFile:
				return Resources.disk;
			case AfterCaptureTasks.SaveImageToFileWithDialog:
				return Resources.disk_rename;
			case AfterCaptureTasks.SaveThumbnailImageToFile:
				return Resources.disk_small;
			case AfterCaptureTasks.PerformActions:
				return Resources.application_terminal;
			case AfterCaptureTasks.CopyFileToClipboard:
				return Resources.clipboard_block;
			case AfterCaptureTasks.CopyFilePathToClipboard:
				return Resources.clipboard_list;
			case AfterCaptureTasks.ShowInExplorer:
				return Resources.folder_stand;
			case AfterCaptureTasks.ScanQRCode:
				if (!ShareXResources.IsDarkTheme)
				{
					return Resources.barcode_2d;
				}
				return Resources.barcode_2d_white;
			case AfterCaptureTasks.DoOCR:
				if (!ShareXResources.IsDarkTheme)
				{
					return Resources.edit_drop_cap;
				}
				return Resources.edit_drop_cap_white;
			case AfterCaptureTasks.ShowBeforeUploadWindow:
				return Resources.application__arrow;
			case AfterCaptureTasks.UploadImageToHost:
				return Resources.upload_cloud;
			case AfterCaptureTasks.DeleteFile:
				return Resources.bin;
			}
		}
		if (value is AfterUploadTasks)
		{
			object obj2 = value;
			AfterUploadTasks afterUploadTasks = (AfterUploadTasks)((obj2 is AfterUploadTasks) ? obj2 : null);
			switch (afterUploadTasks)
			{
			default:
				throw new Exception("Icon missing for after upload task: " + afterUploadTasks);
			case AfterUploadTasks.ShowAfterUploadWindow:
				return Resources.application_browser;
			case AfterUploadTasks.UseURLShortener:
				if (!ShareXResources.IsDarkTheme)
				{
					return Resources.edit_scale;
				}
				return Resources.edit_scale_white;
			case AfterUploadTasks.ShareURL:
				return Resources.globe_share;
			case AfterUploadTasks.CopyURLToClipboard:
				return Resources.clipboard_paste_document_text;
			case AfterUploadTasks.OpenURL:
				return Resources.globe__arrow;
			case AfterUploadTasks.ShowQRCode:
				if (!ShareXResources.IsDarkTheme)
				{
					return Resources.barcode_2d;
				}
				return Resources.barcode_2d_white;
			}
		}
		if (value is HotkeyType)
		{
			object obj3 = value;
			HotkeyType hotkeyType = (HotkeyType)((obj3 is HotkeyType) ? obj3 : null);
			switch (hotkeyType)
			{
			default:
				throw new Exception("Icon missing for hotkey type: " + hotkeyType);
			case HotkeyType.None:
				return null;
			case HotkeyType.FileUpload:
				return Resources.folder_open_document;
			case HotkeyType.FolderUpload:
				return Resources.folder;
			case HotkeyType.ClipboardUpload:
				return Resources.clipboard;
			case HotkeyType.ClipboardUploadWithContentViewer:
				return Resources.clipboard_task;
			case HotkeyType.UploadText:
				return Resources.notebook;
			case HotkeyType.UploadURL:
				return Resources.drive;
			case HotkeyType.DragDropUpload:
				return Resources.inbox;
			case HotkeyType.ShortenURL:
				if (!ShareXResources.IsDarkTheme)
				{
					return Resources.edit_scale;
				}
				return Resources.edit_scale_white;
			case HotkeyType.TweetMessage:
				return Resources.Twitter;
			case HotkeyType.StopUploads:
				return Resources.cross_button;
			case HotkeyType.PrintScreen:
				return Resources.layer_fullscreen;
			case HotkeyType.ActiveWindow:
				return Resources.application_blue;
			case HotkeyType.ActiveMonitor:
				return Resources.monitor;
			case HotkeyType.RectangleRegion:
				return Resources.layer_shape;
			case HotkeyType.RectangleLight:
				return Resources.Rectangle;
			case HotkeyType.RectangleTransparent:
				return Resources.layer_transparent;
			case HotkeyType.CustomRegion:
				return Resources.layer__arrow;
			case HotkeyType.LastRegion:
				return Resources.layers;
			case HotkeyType.ScrollingCapture:
				return Resources.ui_scroll_pane_image;
			case HotkeyType.AutoCapture:
				return Resources.clock;
			case HotkeyType.StartAutoCapture:
				return Resources.clock__arrow;
			case HotkeyType.ScreenRecorder:
				return Resources.camcorder_image;
			case HotkeyType.ScreenRecorderActiveWindow:
				return Resources.camcorder__arrow;
			case HotkeyType.ScreenRecorderCustomRegion:
				return Resources.camcorder__arrow;
			case HotkeyType.StartScreenRecorder:
				return Resources.camcorder__arrow;
			case HotkeyType.ScreenRecorderGIF:
				return Resources.film;
			case HotkeyType.ScreenRecorderGIFActiveWindow:
				return Resources.film__arrow;
			case HotkeyType.ScreenRecorderGIFCustomRegion:
				return Resources.film__arrow;
			case HotkeyType.StartScreenRecorderGIF:
				return Resources.film__arrow;
			case HotkeyType.StopScreenRecording:
				return Resources.camcorder__minus;
			case HotkeyType.AbortScreenRecording:
				return Resources.camcorder__exclamation;
			case HotkeyType.ColorPicker:
				return Resources.color;
			case HotkeyType.ScreenColorPicker:
				return Resources.pipette;
			case HotkeyType.Ruler:
				return Resources.ruler_triangle;
			case HotkeyType.ImageEditor:
				return Resources.image_pencil;
			case HotkeyType.ImageEffects:
				return Resources.image_saturation;
			case HotkeyType.ImageViewer:
				return Resources.images_flickr;
			case HotkeyType.ImageCombiner:
				return Resources.document_break;
			case HotkeyType.ImageSplitter:
				return Resources.image_split;
			case HotkeyType.ImageThumbnailer:
				return Resources.image_resize_actual;
			case HotkeyType.VideoConverter:
				return Resources.camcorder_pencil;
			case HotkeyType.VideoThumbnailer:
				return Resources.images_stack;
			case HotkeyType.OCR:
				if (!ShareXResources.IsDarkTheme)
				{
					return Resources.edit_drop_cap;
				}
				return Resources.edit_drop_cap_white;
			case HotkeyType.QRCode:
				if (!ShareXResources.IsDarkTheme)
				{
					return Resources.barcode_2d;
				}
				return Resources.barcode_2d_white;
			case HotkeyType.QRCodeDecodeFromScreen:
				if (!ShareXResources.IsDarkTheme)
				{
					return Resources.barcode_2d;
				}
				return Resources.barcode_2d_white;
			case HotkeyType.HashCheck:
				return Resources.application_task;
			case HotkeyType.IndexFolder:
				return Resources.folder_tree;
			case HotkeyType.ClipboardViewer:
				return Resources.clipboard_block;
			case HotkeyType.BorderlessWindow:
				return Resources.application_resize_full;
			case HotkeyType.InspectWindow:
				return Resources.application_search_result;
			case HotkeyType.MonitorTest:
				return Resources.monitor;
			case HotkeyType.DNSChanger:
				return Resources.network_ip;
			case HotkeyType.DisableHotkeys:
				return Resources.keyboard__minus;
			case HotkeyType.OpenMainWindow:
				return Resources.application_home;
			case HotkeyType.OpenScreenshotsFolder:
				return Resources.folder_open_image;
			case HotkeyType.OpenHistory:
				return Resources.application_blog;
			case HotkeyType.OpenImageHistory:
				return Resources.application_icon_large;
			case HotkeyType.ToggleActionsToolbar:
				return Resources.ui_toolbar__arrow;
			case HotkeyType.ToggleTrayMenu:
				return Resources.ui_menu_blue;
			case HotkeyType.ExitShareX:
				return Resources.cross;
			}
		}
		return null;
	}

	public static Image FindMenuIcon<T>(int index) where T : Enum
	{
		return FindMenuIcon(Helpers.GetEnumFromIndex<T>(index));
	}

	public static Screenshot GetScreenshot(TaskSettings taskSettings = null)
	{
		if (taskSettings == null)
		{
			taskSettings = TaskSettings.GetDefaultTaskSettings();
		}
		return new Screenshot
		{
			CaptureCursor = taskSettings.CaptureSettings.ShowCursor,
			CaptureClientArea = taskSettings.CaptureSettings.CaptureClientArea,
			RemoveOutsideScreenArea = true,
			CaptureShadow = taskSettings.CaptureSettings.CaptureShadow,
			ShadowOffset = taskSettings.CaptureSettings.CaptureShadowOffset,
			AutoHideTaskbar = taskSettings.CaptureSettings.CaptureAutoHideTaskbar
		};
	}

	public static void ImportCustomUploader(string filePath)
	{
		if (Program.UploadersConfig == null)
		{
			return;
		}
		try
		{
			CustomUploaderItem customUploaderItem = JsonHelpers.DeserializeFromFile<CustomUploaderItem>(filePath);
			if (customUploaderItem == null)
			{
				return;
			}
			bool flag = false;
			if (customUploaderItem.DestinationType == CustomUploaderDestinationType.None)
			{
				if (MessageBox.Show($"Would you like to add \"{customUploaderItem}\" custom uploader?", "ShareX - Custom uploader confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				List<string> list = new List<string>();
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.ImageUploader))
				{
					list.Add("images");
				}
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.TextUploader))
				{
					list.Add("texts");
				}
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.FileUploader))
				{
					list.Add("files");
				}
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.URLShortener) || customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.URLSharingService))
				{
					list.Add("urls");
				}
				string arg = string.Join("/", list);
				switch (MessageBox.Show($"Would you like to set \"{customUploaderItem}\" as the active custom uploader for {arg}?", "ShareX - Custom uploader confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
				{
				case DialogResult.Yes:
					flag = true;
					break;
				case DialogResult.Cancel:
					return;
				}
			}
			customUploaderItem.CheckBackwardCompatibility();
			Program.UploadersConfig.CustomUploadersList.Add(customUploaderItem);
			if (flag)
			{
				int num = Program.UploadersConfig.CustomUploadersList.Count - 1;
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.ImageUploader))
				{
					Program.UploadersConfig.CustomImageUploaderSelected = num;
					Program.DefaultTaskSettings.ImageDestination = ImageDestination.CustomImageUploader;
				}
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.TextUploader))
				{
					Program.UploadersConfig.CustomTextUploaderSelected = num;
					Program.DefaultTaskSettings.TextDestination = TextDestination.CustomTextUploader;
				}
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.FileUploader))
				{
					Program.UploadersConfig.CustomFileUploaderSelected = num;
					Program.DefaultTaskSettings.FileDestination = FileDestination.CustomFileUploader;
				}
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.URLShortener))
				{
					Program.UploadersConfig.CustomURLShortenerSelected = num;
					Program.DefaultTaskSettings.URLShortenerDestination = UrlShortenerType.CustomURLShortener;
				}
				if (customUploaderItem.DestinationType.HasFlag(CustomUploaderDestinationType.URLSharingService))
				{
					Program.UploadersConfig.CustomURLSharingServiceSelected = num;
					Program.DefaultTaskSettings.URLSharingServiceDestination = URLSharingServices.CustomURLSharingService;
				}
				Program.MainForm.UpdateCheckStates();
				Program.MainForm.UpdateUploaderMenuNames();
			}
			if (CustomUploaderSettingsForm.IsInstanceActive)
			{
				CustomUploaderSettingsForm.CustomUploaderUpdateTab();
			}
		}
		catch (Exception ex)
		{
			DebugHelper.WriteException(ex);
			ex.ShowError();
		}
	}

	public static void ImportImageEffect(string filePath)
	{
		string text = null;
		try
		{
			text = ImageEffectPackager.ExtractPackage(filePath, Program.ImageEffectsFolder);
		}
		catch (Exception e)
		{
			e.ShowError(fullError: false);
		}
		if (!string.IsNullOrEmpty(text))
		{
			OpenImageEffectsSingleton(Program.DefaultTaskSettings)?.ImportImageEffect(text);
			if (!Program.DefaultTaskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.AddImageEffects) && MessageBox.Show(Resources.WouldYouLikeToEnableImageEffects, "ShareX", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Program.DefaultTaskSettings.AfterCaptureJob = Program.DefaultTaskSettings.AfterCaptureJob.Add<AfterCaptureTasks>(AfterCaptureTasks.AddImageEffects);
				Program.MainForm.UpdateCheckStates();
			}
		}
	}

	public static void HandleNativeMessagingInput(string filePath)
	{
		if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
		{
			return;
		}
		NativeMessagingInput nativeMessagingInput = null;
		try
		{
			nativeMessagingInput = JsonHelpers.DeserializeFromFile<NativeMessagingInput>(filePath);
			File.Delete(filePath);
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
		if (nativeMessagingInput != null)
		{
			if (!string.IsNullOrEmpty(nativeMessagingInput.URL) && URLHelpers.IsValidURL(nativeMessagingInput.URL))
			{
				UploadManager.DownloadAndUploadFile(nativeMessagingInput.URL);
			}
			else if (!string.IsNullOrEmpty(nativeMessagingInput.Text))
			{
				UploadManager.UploadText(nativeMessagingInput.Text);
			}
		}
	}

	public static void OpenActionsToolbar()
	{
		ActionsToolbarForm.Instance.ForceActivate();
	}

	public static void ToggleActionsToolbar()
	{
		if (ActionsToolbarForm.IsInstanceActive)
		{
			ActionsToolbarForm.Instance.Close();
		}
		else
		{
			ActionsToolbarForm.Instance.ForceActivate();
		}
	}

	public static async Task DownloadAppVeyorBuild()
	{
		AppVeyorUpdateChecker updateChecker = new AppVeyorUpdateChecker
		{
			IsBeta = Program.Dev,
			IsPortable = Program.Portable,
			Proxy = HelpersOptions.CurrentProxy.GetWebProxy(),
			Branch = "develop"
		};
		await updateChecker.CheckUpdateAsync();
		if (updateChecker.Status == UpdateStatus.UpdateAvailable)
		{
			updateChecker.DownloadUpdate();
		}
	}

	public static Image CreateQRCode(string text, int width, int height)
	{
		if (CheckQRCodeContent(text))
		{
			try
			{
				return new BarcodeWriter
				{
					Format = BarcodeFormat.QR_CODE,
					Options = new QrCodeEncodingOptions
					{
						Width = width,
						Height = height,
						CharacterSet = "UTF-8"
					},
					Renderer = new BitmapRenderer()
				}.Write(text);
			}
			catch (Exception e)
			{
				e.ShowError();
			}
		}
		return null;
	}

	public static Image CreateQRCode(string text, int size)
	{
		return CreateQRCode(text, size, size);
	}

	public static string[] BarcodeScan(Bitmap bmp, bool scanQRCodeOnly = false)
	{
		try
		{
			BarcodeReader barcodeReader = new BarcodeReader
			{
				AutoRotate = true,
				Options = new DecodingOptions
				{
					TryHarder = true,
					TryInverted = true
				}
			};
			if (scanQRCodeOnly)
			{
				barcodeReader.Options.PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE };
			}
			Result[] array = barcodeReader.DecodeMultiple(bmp);
			if (array != null)
			{
				return (from x in array
					where x != null && !string.IsNullOrEmpty(x.Text)
					select x.Text).ToArray();
			}
		}
		catch (Exception e)
		{
			e.ShowError();
		}
		return null;
	}

	public static bool CheckQRCodeContent(string content)
	{
		if (!string.IsNullOrEmpty(content))
		{
			return Encoding.UTF8.GetByteCount(content) <= 2952;
		}
		return false;
	}

	public static void ShowBalloonTip(string text, ToolTipIcon icon, int timeout, string title = "ShareX", BalloonTipAction clickAction = null)
	{
		if (Program.MainForm != null && !Program.MainForm.IsDisposed && Program.MainForm.niTray != null && Program.MainForm.niTray.Visible)
		{
			Program.MainForm.niTray.Tag = clickAction;
			Program.MainForm.niTray.ShowBalloonTip(timeout, title, text, icon);
		}
	}

	public static void ShowNotificationTip(string text, string title = "ShareX", int duration = -1)
	{
		if (duration < 0)
		{
			duration = (int)(Program.DefaultTaskSettings.GeneralSettings.ToastWindowDuration * 1000f);
		}
		NotificationFormConfig toastConfig = new NotificationFormConfig
		{
			Duration = duration,
			FadeDuration = (int)(Program.DefaultTaskSettings.GeneralSettings.ToastWindowFadeDuration * 1000f),
			Placement = Program.DefaultTaskSettings.GeneralSettings.ToastWindowPlacement,
			Size = Program.DefaultTaskSettings.GeneralSettings.ToastWindowSize,
			Title = title,
			Text = text
		};
		Program.MainForm.InvokeSafe(delegate
		{
			NotificationForm.Show(toastConfig);
		});
	}

	public static void ToggleTrayMenu()
	{
		ContextMenuStrip contextMenuStrip = Program.MainForm.niTray.ContextMenuStrip;
		if (contextMenuStrip != null && !contextMenuStrip.IsDisposed)
		{
			if (contextMenuStrip.Visible)
			{
				contextMenuStrip.Close();
				return;
			}
			NativeMethods.SetForegroundWindow(contextMenuStrip.Handle);
			contextMenuStrip.Show(Cursor.Position);
		}
	}

	public static bool IsUploadAllowed()
	{
		if (SystemOptions.DisableUpload)
		{
			MessageBox.Show(Resources.YourSystemAdminDisabledTheUploadFeature, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		if (Program.Settings.DisableUpload)
		{
			MessageBox.Show(Resources.ThisFeatureWillNotWorkWhenDisableUploadOptionIsEnabled, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		return true;
	}
}
