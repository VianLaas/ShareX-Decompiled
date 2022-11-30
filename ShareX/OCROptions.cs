using System.Collections.Generic;
using System.Linq;

namespace ShareX;

public class OCROptions
{
	public string Language { get; set; } = "en";


	public float ScaleFactor { get; set; } = 2f;


	public bool SingleLine { get; set; }

	public bool Silent { get; set; }

	public bool AutoCopy { get; set; }

	public List<ServiceLink> ServiceLinks { get; set; } = DefaultServiceLinks;


	public int SelectedServiceLink { get; set; }

	public static List<ServiceLink> DefaultServiceLinks => new List<ServiceLink>
	{
		new ServiceLink("Google Translate", "https://translate.google.com/?sl=auto&tl=en&text={0}&op=translate"),
		new ServiceLink("Google Search", "https://www.google.com/search?q={0}"),
		new ServiceLink("Google Images", "https://www.google.com/search?q={0}&tbm=isch"),
		new ServiceLink("Bing", "https://www.bing.com/search?q={0}"),
		new ServiceLink("DuckDuckGo", "https://duckduckgo.com/?q={0}"),
		new ServiceLink("DeepL", "https://www.deepl.com/translator#auto/en/{0}")
	};

	public bool IsDefaultServiceLinks()
	{
		if (ServiceLinks != null && ServiceLinks.Count > 0)
		{
			List<ServiceLink> defaultServiceLinks = DefaultServiceLinks;
			return ServiceLinks.All((ServiceLink x) => defaultServiceLinks.Any((ServiceLink y) => x.Name == y.Name));
		}
		return false;
	}
}
