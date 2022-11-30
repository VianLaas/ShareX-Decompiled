using System;

namespace ShareX;

[Flags]
public enum AfterCaptureTasks
{
	None = 0,
	ShowQuickTaskMenu = 1,
	ShowAfterCaptureWindow = 2,
	AddImageEffects = 4,
	AnnotateImage = 8,
	CopyImageToClipboard = 0x10,
	SendImageToPrinter = 0x20,
	SaveImageToFile = 0x40,
	SaveImageToFileWithDialog = 0x80,
	SaveThumbnailImageToFile = 0x100,
	PerformActions = 0x200,
	CopyFileToClipboard = 0x400,
	CopyFilePathToClipboard = 0x800,
	ShowInExplorer = 0x1000,
	ScanQRCode = 0x2000,
	DoOCR = 0x4000,
	ShowBeforeUploadWindow = 0x8000,
	UploadImageToHost = 0x10000,
	DeleteFile = 0x20000
}
