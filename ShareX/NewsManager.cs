using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using Newtonsoft.Json;
using ShareX.HelpersLib;

namespace ShareX;

public class NewsManager
{
	public List<NewsItem> NewsItems { get; private set; } = new List<NewsItem>();


	public DateTime LastReadDate { get; set; }

	public bool IsUnread => UnreadCount > 0;

	public int UnreadCount
	{
		get
		{
			if (NewsItems == null)
			{
				return 0;
			}
			return NewsItems.Count((NewsItem x) => x.IsUnread);
		}
	}

	public void UpdateNews()
	{
		try
		{
			NewsItems = GetNews();
		}
		catch (Exception exception)
		{
			DebugHelper.WriteException(exception);
		}
	}

	public void UpdateUnread()
	{
		if (NewsItems == null)
		{
			return;
		}
		foreach (NewsItem newsItem in NewsItems)
		{
			newsItem.IsUnread = newsItem.DateTime > LastReadDate;
		}
	}

	private List<NewsItem> GetNews()
	{
		using (WebClient webClient = new WebClient())
		{
			webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			webClient.Headers.Add(HttpRequestHeader.UserAgent, ShareXResources.UserAgent);
			webClient.Proxy = HelpersOptions.CurrentProxy.GetWebProxy();
			string address = URLHelpers.CombineURL("https://getsharex.com", "news.json");
			string value = webClient.DownloadString(address);
			if (!string.IsNullOrEmpty(value))
			{
				JsonSerializerSettings settings = new JsonSerializerSettings
				{
					DateTimeZoneHandling = DateTimeZoneHandling.Local
				};
				return JsonConvert.DeserializeObject<List<NewsItem>>(value, settings);
			}
		}
		return null;
	}

	private void ExportNews(List<NewsItem> newsItems)
	{
		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			DateTimeZoneHandling = DateTimeZoneHandling.Utc,
			Formatting = Formatting.Indented,
			NullValueHandling = NullValueHandling.Ignore
		};
		string contents = JsonConvert.SerializeObject(newsItems, settings);
		File.WriteAllText("news.json", contents);
	}
}
