using System;
using ShareX.HelpersLib;

namespace ShareX;

public class RecentTask
{
	public string FilePath { get; set; }

	public string FileName
	{
		get
		{
			string filePath = "";
			if (!string.IsNullOrEmpty(FilePath))
			{
				filePath = FilePath;
			}
			else if (!string.IsNullOrEmpty(URL))
			{
				filePath = URL;
			}
			return FileHelpers.GetFileNameSafe(filePath);
		}
	}

	public string URL { get; set; }

	public string ThumbnailURL { get; set; }

	public string DeletionURL { get; set; }

	public string ShortenedURL { get; set; }

	public DateTime Time { get; set; }

	public string TrayMenuText
	{
		get
		{
			string arg = ToString().Truncate(50, "...", truncateFromRight: false);
			return $"[{Time:HH:mm:ss}] {arg}";
		}
	}

	public RecentTask()
	{
		Time = DateTime.Now;
	}

	public override string ToString()
	{
		string result = "";
		if (!string.IsNullOrEmpty(ShortenedURL))
		{
			result = ShortenedURL;
		}
		else if (!string.IsNullOrEmpty(URL))
		{
			result = URL;
		}
		else if (!string.IsNullOrEmpty(FilePath))
		{
			result = FilePath;
		}
		return result;
	}
}
