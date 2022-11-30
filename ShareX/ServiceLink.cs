using ShareX.HelpersLib;

namespace ShareX;

public class ServiceLink
{
	public string Name { get; set; }

	public string URL { get; set; }

	public ServiceLink(string name, string url)
	{
		Name = name;
		URL = url;
	}

	public string GenerateLink(string input)
	{
		if (!string.IsNullOrEmpty(input))
		{
			string arg = URLHelpers.URLEncode(input);
			return string.Format(URL, arg);
		}
		return null;
	}

	public void OpenLink(string input)
	{
		string text = GenerateLink(input);
		if (!string.IsNullOrEmpty(text))
		{
			URLHelpers.OpenURL(text);
		}
	}

	public override string ToString()
	{
		return Name;
	}
}
