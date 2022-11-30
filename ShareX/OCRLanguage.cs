namespace ShareX;

public class OCRLanguage
{
	public string DisplayName { get; set; }

	public string LanguageTag { get; set; }

	public OCRLanguage(string displayName, string languageTag)
	{
		DisplayName = displayName;
		LanguageTag = languageTag;
	}

	public override string ToString()
	{
		return DisplayName;
	}
}
