using System;

namespace ShareX;

[Flags]
public enum AfterUploadTasks
{
	None = 0,
	ShowAfterUploadWindow = 1,
	UseURLShortener = 2,
	ShareURL = 4,
	CopyURLToClipboard = 8,
	OpenURL = 0x10,
	ShowQRCode = 0x20
}
