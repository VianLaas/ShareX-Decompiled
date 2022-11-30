using System;
using System.IO;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class ImageData : IDisposable
{
	public MemoryStream ImageStream { get; set; }

	public EImageFormat ImageFormat { get; set; }

	public bool Write(string filePath)
	{
		try
		{
			if (ImageStream != null && !string.IsNullOrEmpty(filePath))
			{
				return ImageStream.WriteToFile(filePath);
			}
		}
		catch (Exception ex)
		{
			DebugHelper.WriteException(ex);
			string text = Resources.ImageData_Write_Error_Message + "\r\n\"" + filePath + "\"";
			if (ex is UnauthorizedAccessException || ex is FileNotFoundException)
			{
				text = text + "\r\n\r\n" + Resources.YourAntiVirusSoftwareOrTheControlledFolderAccessFeatureInWindowsCouldBeBlockingShareX;
			}
			MessageBox.Show(text, "ShareX - " + Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return false;
	}

	public void Dispose()
	{
		ImageStream?.Dispose();
	}
}
