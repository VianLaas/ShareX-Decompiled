using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using ShareX.HelpersLib;

namespace ShareX;

public class TaskSettingsAdvanced
{
	public string CapturePath;

	[Category("General")]
	[DefaultValue(false)]
	[Description("Allow after capture tasks for image files by loading them as bitmap when files are handled during file upload, clipboard file upload, drag && drop file upload, watch folder and other image file tasks.")]
	public bool ProcessImagesDuringFileUpload { get; set; }

	[Category("General")]
	[DefaultValue(false)]
	[Description("Use after capture tasks for clipboard image upload.")]
	public bool ProcessImagesDuringClipboardUpload { get; set; }

	[Category("General")]
	[DefaultValue(true)]
	[Description("Allows file related after capture tasks (\"Perform actions\", \"Copy file to clipboard\" etc.) to be used when doing file upload.")]
	public bool UseAfterCaptureTasksDuringFileUpload { get; set; }

	[Category("General")]
	[DefaultValue(true)]
	[Description("Save text as file for tasks such as\u00a0clipboard text upload, drag and drop text upload, index folder etc.")]
	public bool TextTaskSaveAsFile { get; set; }

	[Category("General")]
	[DefaultValue(false)]
	[Description("If task contains upload job then this setting will clear clipboard when task start.")]
	public bool AutoClearClipboard { get; set; }

	[Category("Capture")]
	[DefaultValue(false)]
	[Description("Disable annotation support in region capture.")]
	public bool RegionCaptureDisableAnnotation { get; set; }

	[Category("Upload")]
	[Description("Files with these file extensions will be uploaded using image uploader.")]
	[Editor("System.Windows.Forms.Design.StringCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public List<string> ImageExtensions { get; set; }

	[Category("Upload")]
	[Description("Files with these file extensions will be uploaded using text uploader.")]
	[Editor("System.Windows.Forms.Design.StringCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public List<string> TextExtensions { get; set; }

	[Category("Upload")]
	[DefaultValue(false)]
	[Description("Copy URL before start upload. Only works for FTP, FTPS, SFTP, Amazon S3, Google Cloud Storage and Azure Storage.")]
	public bool EarlyCopyURL { get; set; }

	[Category("Upload text")]
	[DefaultValue("txt")]
	[Description("File extension when saving text to the local hard disk.")]
	public string TextFileExtension { get; set; }

	[Category("Upload text")]
	[DefaultValue("text")]
	[Description("Text format e.g. csharp, cpp, etc.")]
	public string TextFormat { get; set; }

	[Category("Upload text")]
	[DefaultValue("")]
	[Description("Custom text input. Use %input for text input. Example you can create web page with your text in it.")]
	[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
	public string TextCustom { get; set; }

	[Category("Upload text")]
	[DefaultValue(true)]
	[Description("HTML encode custom text input.")]
	public bool TextCustomEncodeInput { get; set; }

	[Category("After upload")]
	[DefaultValue(false)]
	[Description("If result URL starts with \"http://\" then replace it with \"https://\".")]
	public bool ResultForceHTTPS { get; set; }

	[Category("After upload")]
	[DefaultValue("$result")]
	[Description("Clipboard content format after uploading. Supported variables: $result, $url, $shorturl, $thumbnailurl, $deletionurl, $filepath, $filename, $filenamenoext, $folderpath, $foldername, $uploadtime and other variables such as %y-%mo-%d etc.")]
	public string ClipboardContentFormat { get; set; }

	[Category("After upload")]
	[DefaultValue("$result")]
	[Description("Balloon tip content format after uploading. Supported variables: $result, $url, $shorturl, $thumbnailurl, $deletionurl, $filepath, $filename, $filenamenoext, $folderpath, $foldername, $uploadtime and other variables such as %y-%mo-%d etc.")]
	public string BalloonTipContentFormat { get; set; }

	[Category("After upload")]
	[DefaultValue("$result")]
	[Description("After upload task \"Open URL\" format. Supported variables: $result, $url, $shorturl, $thumbnailurl, $deletionurl, $filepath, $filename, $filenamenoext, $folderpath, $foldername, $uploadtime and other variables such as %y-%mo-%d etc.")]
	public string OpenURLFormat { get; set; }

	[Category("After upload")]
	[DefaultValue(0)]
	[Description("Automatically shorten URL if the URL is longer than the specified number of characters. 0 means automatic URL shortening is not active.")]
	public int AutoShortenURLLength { get; set; }

	[Category("After upload")]
	[DefaultValue(false)]
	[Description("After upload form will be automatically closed after 60 seconds.")]
	public bool AutoCloseAfterUploadForm { get; set; }

	[Category("Name pattern")]
	[DefaultValue(100)]
	[Description("Maximum name pattern length for file name.")]
	public int NamePatternMaxLength { get; set; }

	[Category("Name pattern")]
	[DefaultValue(50)]
	[Description("Maximum name pattern title (%t) length for file name.")]
	public int NamePatternMaxTitleLength { get; set; }

	public TaskSettingsAdvanced()
	{
		this.ApplyDefaultPropertyValues();
		ImageExtensions = FileHelpers.ImageFileExtensions.ToList();
		TextExtensions = FileHelpers.TextFileExtensions.ToList();
	}
}
