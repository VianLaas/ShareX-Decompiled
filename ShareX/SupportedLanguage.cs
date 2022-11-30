using System.ComponentModel;

namespace ShareX;

public enum SupportedLanguage
{
	Automatic,
	[Description("Nederlands (Dutch)")]
	Dutch,
	[Description("English")]
	English,
	[Description("Français (French)")]
	French,
	[Description("Deutsch (German)")]
	German,
	[Description("Magyar (Hungarian)")]
	Hungarian,
	[Description("Bahasa Indonesia (Indonesian)")]
	Indonesian,
	[Description("Italiano (Italian)")]
	Italian,
	[Description("日本語 (Japanese)")]
	Japanese,
	[Description("한국어 (Korean)")]
	Korean,
	[Description("Español mexicano (Mexican Spanish)")]
	MexicanSpanish,
	[Description("فارسی (Persian)")]
	Persian,
	[Description("Polski (Polish)")]
	Polish,
	[Description("Português (Portuguese)")]
	Portuguese,
	[Description("Português-Brasil (Portuguese-Brazil)")]
	PortugueseBrazil,
	[Description("Română (Romanian)")]
	Romanian,
	[Description("Русский (Russian)")]
	Russian,
	[Description("简体中文 (Simplified Chinese)")]
	SimplifiedChinese,
	[Description("Español (Spanish)")]
	Spanish,
	[Description("繁體中文 (Traditional Chinese)")]
	TraditionalChinese,
	[Description("Türkçe (Turkish)")]
	Turkish,
	[Description("Українська (Ukrainian)")]
	Ukrainian,
	[Description("Tiếng Việt (Vietnamese)")]
	Vietnamese
}
