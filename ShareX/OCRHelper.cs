using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ShareX.HelpersLib;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace ShareX;

public static class OCRHelper
{
	private const string SupportedVersion = "10.0.18362.0";

	public static bool IsSupported => Helpers.OSVersion >= new Version("10.0.18362.0");

	public static OCRLanguage[] AvailableLanguages
	{
		get
		{
			ThrowIfNotSupported();
			return OcrEngine.AvailableRecognizerLanguages.Select((Language x) => new OCRLanguage(x.DisplayName, x.LanguageTag)).ToArray();
		}
	}

	public static void ThrowIfNotSupported()
	{
		if (!IsSupported)
		{
			throw new Exception(string.Format("Optical character recognition feature is only available with Windows version {0} or newer.", "10.0.18362.0"));
		}
	}

	public static async Task<string> OCR(Bitmap bmp, string languageTag = "en", float scaleFactor = 1f, bool singleLine = false)
	{
		ThrowIfNotSupported();
		scaleFactor = Math.Max(scaleFactor, 1f);
		return await Task.Run(async delegate
		{
			using Bitmap bmpClone = (Bitmap)bmp.Clone();
			using Bitmap bmpScaled = ImageHelpers.ResizeImage(bmpClone, (int)((float)bmpClone.Width * scaleFactor), (int)((float)bmpClone.Height * scaleFactor));
			return await OCRInternal(bmpScaled, languageTag, singleLine);
		});
	}

	private static async Task<string> OCRInternal(Bitmap bmp, string languageTag, bool singleLine = false)
	{
		return "Some String";
	}
}
