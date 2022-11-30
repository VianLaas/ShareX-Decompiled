using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public static class LanguageHelper
{
	public static bool ChangeLanguage(SupportedLanguage language, params Form[] forms)
	{
		CultureInfo cultureInfo = ((language != 0) ? CultureInfo.GetCultureInfo(GetCultureName(language)) : CultureInfo.InstalledUICulture);
		if (!cultureInfo.Equals(Thread.CurrentThread.CurrentUICulture))
		{
			Helpers.SetDefaultUICulture(cultureInfo);
			DebugHelper.WriteLine("Language changed to: " + cultureInfo.DisplayName);
			foreach (Form form in forms)
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(form.GetType());
				ApplyResourceToControl(form, componentResourceManager, cultureInfo);
				componentResourceManager.ApplyResources(form, "$this", cultureInfo);
			}
			return true;
		}
		return false;
	}

	public static Image GetLanguageIcon(SupportedLanguage language)
	{
		return language switch
		{
			SupportedLanguage.Dutch => Resources.nl, 
			SupportedLanguage.English => Resources.us, 
			SupportedLanguage.French => Resources.fr, 
			SupportedLanguage.German => Resources.de, 
			SupportedLanguage.Hungarian => Resources.hu, 
			SupportedLanguage.Indonesian => Resources.id, 
			SupportedLanguage.Italian => Resources.it, 
			SupportedLanguage.Japanese => Resources.jp, 
			SupportedLanguage.Korean => Resources.kr, 
			SupportedLanguage.MexicanSpanish => Resources.mx, 
			SupportedLanguage.Persian => Resources.ir, 
			SupportedLanguage.Polish => Resources.pl, 
			SupportedLanguage.Portuguese => Resources.pt, 
			SupportedLanguage.PortugueseBrazil => Resources.br, 
			SupportedLanguage.Romanian => Resources.ro, 
			SupportedLanguage.Russian => Resources.ru, 
			SupportedLanguage.SimplifiedChinese => Resources.cn, 
			SupportedLanguage.Spanish => Resources.es, 
			SupportedLanguage.TraditionalChinese => Resources.tw, 
			SupportedLanguage.Turkish => Resources.tr, 
			SupportedLanguage.Ukrainian => Resources.ua, 
			SupportedLanguage.Vietnamese => Resources.vn, 
			_ => Resources.globe, 
		};
	}

	public static string GetCultureName(SupportedLanguage language)
	{
		return language switch
		{
			SupportedLanguage.Dutch => "nl-NL", 
			SupportedLanguage.French => "fr-FR", 
			SupportedLanguage.German => "de-DE", 
			SupportedLanguage.Hungarian => "hu-HU", 
			SupportedLanguage.Indonesian => "id-ID", 
			SupportedLanguage.Italian => "it-IT", 
			SupportedLanguage.Japanese => "ja-JP", 
			SupportedLanguage.Korean => "ko-KR", 
			SupportedLanguage.MexicanSpanish => "es-MX", 
			SupportedLanguage.Persian => "fa-IR", 
			SupportedLanguage.Polish => "pl-PL", 
			SupportedLanguage.Portuguese => "pt-PT", 
			SupportedLanguage.PortugueseBrazil => "pt-BR", 
			SupportedLanguage.Romanian => "ro-RO", 
			SupportedLanguage.Russian => "ru-RU", 
			SupportedLanguage.SimplifiedChinese => "zh-CN", 
			SupportedLanguage.Spanish => "es-ES", 
			SupportedLanguage.TraditionalChinese => "zh-TW", 
			SupportedLanguage.Turkish => "tr-TR", 
			SupportedLanguage.Ukrainian => "uk-UA", 
			SupportedLanguage.Vietnamese => "vi-VN", 
			_ => "en-US", 
		};
	}

	private static void ApplyResourceToControl(Control control, ComponentResourceManager resource, CultureInfo culture)
	{
		if (control is ToolStrip toolStrip)
		{
			ApplyResourceToToolStripItemCollection(toolStrip.Items, resource, culture);
		}
		else
		{
			foreach (Control control2 in control.Controls)
			{
				ApplyResourceToControl(control2, resource, culture);
			}
		}
		resource.ApplyResources(control, control.Name, culture);
	}

	private static void ApplyResourceToToolStripItemCollection(ToolStripItemCollection collection, ComponentResourceManager resource, CultureInfo culture)
	{
		foreach (ToolStripItem item in collection)
		{
			if (item is ToolStripDropDownItem toolStripDropDownItem)
			{
				ApplyResourceToToolStripItemCollection(toolStripDropDownItem.DropDownItems, resource, culture);
			}
			resource.ApplyResources(item, item.Name, culture);
		}
	}
}
