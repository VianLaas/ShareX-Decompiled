using System;
using System.Collections.Generic;
using ShareX.UploadersLib;

namespace ShareX;

public class TaskSettingsUpload
{
	public bool UseCustomTimeZone;

	public TimeZoneInfo CustomTimeZone = TimeZoneInfo.Utc;

	public string NameFormatPattern = "%ra{10}";

	public string NameFormatPatternActiveWindow = "%pn_%ra{10}";

	public bool FileUploadUseNamePattern;

	public bool FileUploadReplaceProblematicCharacters;

	public bool URLRegexReplace;

	public string URLRegexReplacePattern = "^https?://(.+)$";

	public string URLRegexReplaceReplacement = "https://$1";

	public bool ClipboardUploadURLContents;

	public bool ClipboardUploadShortenURL;

	public bool ClipboardUploadShareURL;

	public bool ClipboardUploadAutoIndexFolder;

	public List<UploaderFilter> UploaderFilters = new List<UploaderFilter>();
}
