using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.UploadersLib;

namespace ShareX;

public class UploadInfoManager
{
	private UploadInfoParser parser;

	public UploadInfoStatus[] SelectedItems { get; private set; }

	public UploadInfoStatus SelectedItem
	{
		get
		{
			if (IsItemSelected)
			{
				return SelectedItems[0];
			}
			return null;
		}
	}

	public bool IsItemSelected
	{
		get
		{
			if (SelectedItems != null)
			{
				return SelectedItems.Length != 0;
			}
			return false;
		}
	}

	public UploadInfoManager()
	{
		parser = new UploadInfoParser();
	}

	public void UpdateSelectedItems(IEnumerable<WorkerTask> tasks)
	{
		if (tasks != null && tasks.Count() > 0)
		{
			SelectedItems = (from x in tasks
				where x != null && x.Info != null
				select new UploadInfoStatus(x)).ToArray();
		}
		else
		{
			SelectedItems = null;
		}
	}

	private void CopyTexts(IEnumerable<string> texts)
	{
		if (texts != null && texts.Count() > 0)
		{
			string text = string.Join("\r\n", texts.ToArray());
			if (!string.IsNullOrEmpty(text))
			{
				ClipboardHelpers.CopyText(text);
			}
		}
	}

	public void OpenURL()
	{
		if (IsItemSelected && SelectedItem.IsURLExist)
		{
			URLHelpers.OpenURL(SelectedItem.Info.Result.URL);
		}
	}

	public void OpenShortenedURL()
	{
		if (IsItemSelected && SelectedItem.IsShortenedURLExist)
		{
			URLHelpers.OpenURL(SelectedItem.Info.Result.ShortenedURL);
		}
	}

	public void OpenThumbnailURL()
	{
		if (IsItemSelected && SelectedItem.IsThumbnailURLExist)
		{
			URLHelpers.OpenURL(SelectedItem.Info.Result.ThumbnailURL);
		}
	}

	public void OpenDeletionURL()
	{
		if (IsItemSelected && SelectedItem.IsDeletionURLExist)
		{
			URLHelpers.OpenURL(SelectedItem.Info.Result.DeletionURL);
		}
	}

	public void OpenFile()
	{
		if (IsItemSelected && SelectedItem.IsFileExist)
		{
			FileHelpers.OpenFile(SelectedItem.Info.FilePath);
		}
	}

	public void OpenThumbnailFile()
	{
		if (IsItemSelected && SelectedItem.IsThumbnailFileExist)
		{
			FileHelpers.OpenFile(SelectedItem.Info.ThumbnailFilePath);
		}
	}

	public void OpenFolder()
	{
		if (IsItemSelected && SelectedItem.IsFileExist)
		{
			FileHelpers.OpenFolderWithFile(SelectedItem.Info.FilePath);
		}
	}

	public void TryOpen()
	{
		if (IsItemSelected)
		{
			SelectedItem.Update();
			if (SelectedItem.IsShortenedURLExist)
			{
				URLHelpers.OpenURL(SelectedItem.Info.Result.ShortenedURL);
			}
			else if (SelectedItem.IsURLExist)
			{
				URLHelpers.OpenURL(SelectedItem.Info.Result.URL);
			}
			else if (SelectedItem.IsFilePathValid)
			{
				FileHelpers.OpenFile(SelectedItem.Info.FilePath);
			}
		}
	}

	public void CopyURL()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsURLExist
				select x.Info.Result.URL);
		}
	}

	public void CopyShortenedURL()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsShortenedURLExist
				select x.Info.Result.ShortenedURL);
		}
	}

	public void CopyThumbnailURL()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsThumbnailURLExist
				select x.Info.Result.ThumbnailURL);
		}
	}

	public void CopyDeletionURL()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsDeletionURLExist
				select x.Info.Result.DeletionURL);
		}
	}

	public void CopyFile()
	{
		if (IsItemSelected && SelectedItem.IsFileExist)
		{
			ClipboardHelpers.CopyFile(SelectedItem.Info.FilePath);
		}
	}

	public void CopyImage()
	{
		if (IsItemSelected && SelectedItem.IsImageFile)
		{
			ClipboardHelpers.CopyImageFromFile(SelectedItem.Info.FilePath);
		}
	}

	public void CopyImageDimensions()
	{
		if (IsItemSelected && SelectedItem.IsImageFile)
		{
			Size imageFileDimensions = ImageHelpers.GetImageFileDimensions(SelectedItem.Info.FilePath);
			if (!imageFileDimensions.IsEmpty)
			{
				ClipboardHelpers.CopyText($"{imageFileDimensions.Width} x {imageFileDimensions.Height}");
			}
		}
	}

	public void CopyText()
	{
		if (IsItemSelected && SelectedItem.IsTextFile)
		{
			ClipboardHelpers.CopyTextFromFile(SelectedItem.Info.FilePath);
		}
	}

	public void CopyThumbnailFile()
	{
		if (IsItemSelected && SelectedItem.IsThumbnailFileExist)
		{
			ClipboardHelpers.CopyFile(SelectedItem.Info.ThumbnailFilePath);
		}
	}

	public void CopyThumbnailImage()
	{
		if (IsItemSelected && SelectedItem.IsThumbnailFileExist)
		{
			ClipboardHelpers.CopyImageFromFile(SelectedItem.Info.ThumbnailFilePath);
		}
	}

	public void CopyHTMLLink()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsURLExist
				select parser.Parse(x.Info, "<a href=\"$url\">$url</a>"));
		}
	}

	public void CopyHTMLImage()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsImageURL
				select parser.Parse(x.Info, "<img src=\"$url\">"));
		}
	}

	public void CopyHTMLLinkedImage()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsImageURL && x.IsThumbnailURLExist
				select parser.Parse(x.Info, "<a href=\"$url\"><img src=\"$thumbnailurl\"></a>"));
		}
	}

	public void CopyForumLink()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsURLExist
				select parser.Parse(x.Info, "[url]$url[/url]"));
		}
	}

	public void CopyForumImage()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsImageURL
				select parser.Parse(x.Info, "[img]$url[/img]"));
		}
	}

	public void CopyForumLinkedImage()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsImageURL && x.IsThumbnailURLExist
				select parser.Parse(x.Info, "[url=$url][img]$thumbnailurl[/img][/url]"));
		}
	}

	public void CopyMarkdownLink()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsURLExist
				select parser.Parse(x.Info, "[$url]($url)"));
		}
	}

	public void CopyMarkdownImage()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsImageURL
				select parser.Parse(x.Info, "![]($url)"));
		}
	}

	public void CopyMarkdownLinkedImage()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsImageURL && x.IsThumbnailURLExist
				select parser.Parse(x.Info, "[![]($thumbnailurl)]($url)"));
		}
	}

	public void CopyFilePath()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsFilePathValid
				select x.Info.FilePath);
		}
	}

	public void CopyFileName()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsFilePathValid
				select Path.GetFileNameWithoutExtension(x.Info.FilePath));
		}
	}

	public void CopyFileNameWithExtension()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsFilePathValid
				select Path.GetFileName(x.Info.FilePath));
		}
	}

	public void CopyFolder()
	{
		if (IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsFilePathValid
				select Path.GetDirectoryName(x.Info.FilePath));
		}
	}

	public void CopyCustomFormat(string format)
	{
		if (!string.IsNullOrEmpty(format) && IsItemSelected)
		{
			CopyTexts(from x in SelectedItems
				where x.IsURLExist
				select parser.Parse(x.Info, format));
		}
	}

	public void TryCopy()
	{
		if (!IsItemSelected)
		{
			return;
		}
		if (SelectedItem.IsShortenedURLExist)
		{
			CopyTexts(from x in SelectedItems
				where x.IsShortenedURLExist
				select x.Info.Result.ShortenedURL);
		}
		else if (SelectedItem.IsURLExist)
		{
			CopyTexts(from x in SelectedItems
				where x.IsURLExist
				select x.Info.Result.URL);
		}
		else if (SelectedItem.IsFilePathValid)
		{
			CopyTexts(from x in SelectedItems
				where x.IsFilePathValid
				select x.Info.FilePath);
		}
	}

	public void ShowImagePreview()
	{
		if (IsItemSelected && SelectedItem.IsImageFile)
		{
			ImageViewer.ShowImage(SelectedItem.Info.FilePath);
		}
	}

	public void ShowErrors()
	{
		if (IsItemSelected)
		{
			SelectedItem.Task.ShowErrorWindow();
		}
	}

	public void StopUpload()
	{
		if (!IsItemSelected)
		{
			return;
		}
		foreach (WorkerTask item in SelectedItems.Select((UploadInfoStatus x) => x.Task))
		{
			item?.Stop();
		}
	}

	public void Upload()
	{
		if (IsItemSelected && SelectedItem.IsFileExist)
		{
			UploadManager.UploadFile(SelectedItem.Info.FilePath);
		}
	}

	public void Download()
	{
		if (IsItemSelected && SelectedItem.IsFileURL)
		{
			UploadManager.DownloadFile(SelectedItem.Info.Result.URL);
		}
	}

	public void EditImage()
	{
		if (IsItemSelected && SelectedItem.IsImageFile)
		{
			TaskHelpers.AnnotateImageFromFile(SelectedItem.Info.FilePath);
		}
	}

	public void AddImageEffects()
	{
		if (IsItemSelected && SelectedItem.IsImageFile)
		{
			TaskHelpers.OpenImageEffects(SelectedItem.Info.FilePath);
		}
	}

	public void DeleteFiles()
	{
		if (!IsItemSelected)
		{
			return;
		}
		foreach (string item in SelectedItems.Select((UploadInfoStatus x) => x.Info.FilePath))
		{
			FileHelpers.DeleteFile(item, sendToRecycleBin: true);
		}
	}

	public void ShortenURL(UrlShortenerType urlShortener)
	{
		if (IsItemSelected && SelectedItem.IsURLExist)
		{
			UploadManager.ShortenURL(SelectedItem.Info.Result.ToString(), urlShortener);
		}
	}

	public void ShareURL(URLSharingServices urlSharingService)
	{
		if (IsItemSelected && SelectedItem.IsURLExist)
		{
			UploadManager.ShareURL(SelectedItem.Info.Result.ToString(), urlSharingService);
		}
	}

	public void SearchImageUsingGoogle()
	{
		if (IsItemSelected && SelectedItem.IsURLExist)
		{
			TaskHelpers.SearchImageUsingGoogle(SelectedItem.Info.Result.URL);
		}
	}

	public void SearchImageUsingBing()
	{
		if (IsItemSelected && SelectedItem.IsURLExist)
		{
			TaskHelpers.SearchImageUsingBing(SelectedItem.Info.Result.URL);
		}
	}

	public void ShowQRCode()
	{
		if (IsItemSelected && SelectedItem.IsURLExist)
		{
			new QRCodeForm(SelectedItem.Info.Result.URL).Show();
		}
	}

	public async Task OCRImage()
	{
		if (IsItemSelected && SelectedItem.IsImageFile)
		{
			await TaskHelpers.OCRImage(SelectedItem.Info.FilePath);
		}
	}

	public void CombineImages()
	{
		if (IsItemSelected)
		{
			IEnumerable<string> enumerable = from x in SelectedItems
				where x.IsImageFile
				select x.Info.FilePath;
			if (enumerable.Count() > 1)
			{
				TaskHelpers.OpenImageCombiner(enumerable);
			}
		}
	}

	public void CombineImages(Orientation orientation)
	{
		if (IsItemSelected)
		{
			IEnumerable<string> enumerable = from x in SelectedItems
				where x.IsImageFile
				select x.Info.FilePath;
			if (enumerable.Count() > 1)
			{
				TaskHelpers.CombineImages(enumerable, orientation);
			}
		}
	}

	public void ShowResponse()
	{
		if (IsItemSelected && SelectedItem.Info.Result != null)
		{
			ResponseForm.ShowInstance(SelectedItem.Info.Result);
		}
	}
}
