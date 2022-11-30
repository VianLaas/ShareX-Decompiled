using System;
using Newtonsoft.Json;

namespace ShareX;

public class NewsItem
{
	public DateTime DateTime { get; set; }

	public string Text { get; set; }

	public string URL { get; set; }

	[JsonIgnore]
	public bool IsUnread { get; set; }
}
