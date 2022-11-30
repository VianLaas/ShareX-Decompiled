using System.Collections.Generic;
using ShareX.HelpersLib;
using ShareX.ImageEffectsLib;

namespace ShareX;

public class TaskSettingsImage
{
	public EImageFormat ImageFormat;

	public PNGBitDepth ImagePNGBitDepth;

	public int ImageJPEGQuality = 90;

	public GIFQuality ImageGIFQuality;

	public bool ImageAutoUseJPEG = true;

	public int ImageAutoUseJPEGSize = 2048;

	public bool ImageAutoJPEGQuality;

	public FileExistAction FileExistAction;

	public List<ImageEffectPreset> ImageEffectPresets = new List<ImageEffectPreset> { ImageEffectPreset.GetDefaultPreset() };

	public int SelectedImageEffectPreset;

	public bool ShowImageEffectsWindowAfterCapture;

	public bool ImageEffectOnlyRegionCapture;

	public int ThumbnailWidth = 200;

	public int ThumbnailHeight;

	public string ThumbnailName = "-thumbnail";

	public bool ThumbnailCheckSize;
}
