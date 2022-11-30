using ShareX.IndexerLib;
using ShareX.MediaLib;

namespace ShareX;

public class TaskSettingsTools
{
	public string ScreenColorPickerFormat = "$hex";

	public string ScreenColorPickerFormatCtrl = "$r255, $g255, $b255";

	public string ScreenColorPickerInfoText = "RGB: $r255, $g255, $b255$nHex: $hex$nX: $x Y: $y";

	public IndexerSettings IndexerSettings = new IndexerSettings();

	public ImageCombinerOptions ImageCombinerOptions = new ImageCombinerOptions();

	public VideoConverterOptions VideoConverterOptions = new VideoConverterOptions();

	public VideoThumbnailOptions VideoThumbnailOptions = new VideoThumbnailOptions();

	public BorderlessWindowSettings BorderlessWindowSettings = new BorderlessWindowSettings();
}
