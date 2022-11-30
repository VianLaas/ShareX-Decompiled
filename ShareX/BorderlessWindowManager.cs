using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class BorderlessWindowManager
{
	public static bool MakeWindowBorderless(string windowTitle, bool useWorkingArea = false)
	{
		if (!string.IsNullOrEmpty(windowTitle))
		{
			IntPtr intPtr = SearchWindow(windowTitle);
			if (!(intPtr == IntPtr.Zero))
			{
				MakeWindowBorderless(intPtr, useWorkingArea);
				return true;
			}
			MessageBox.Show(Resources.UnableToFindAWindowWithSpecifiedWindowTitle, "ShareX", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		return false;
	}

	public static void MakeWindowBorderless(IntPtr hWnd, bool useWorkingArea = false)
	{
		WindowInfo windowInfo = new WindowInfo(hWnd);
		if (windowInfo.IsMinimized)
		{
			windowInfo.Restore();
		}
		WindowStyles style = windowInfo.Style;
		style = (windowInfo.Style = style & ~(WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU | WindowStyles.WS_THICKFRAME | WindowStyles.WS_TABSTOP));
		WindowStyles exStyle = windowInfo.ExStyle;
		exStyle = (windowInfo.ExStyle = exStyle & ~(WindowStyles.WS_GROUP | WindowStyles.WS_EX_DLGMODALFRAME | WindowStyles.WS_EX_CLIENTEDGE));
		Screen screen = Screen.FromHandle(hWnd);
		Rectangle rect = ((!useWorkingArea) ? screen.Bounds : screen.WorkingArea);
		SetWindowPosFlags setWindowPosFlags = SetWindowPosFlags.SWP_DRAWFRAME | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOZORDER;
		if (rect.IsEmpty)
		{
			setWindowPosFlags |= SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE;
		}
		windowInfo.SetWindowPos(rect, setWindowPosFlags);
	}

	private static IntPtr SearchWindow(string windowTitle)
	{
		IntPtr intPtr = NativeMethods.FindWindow(null, windowTitle);
		if (intPtr == IntPtr.Zero)
		{
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.MainWindowTitle.Contains(windowTitle, StringComparison.InvariantCultureIgnoreCase))
				{
					return process.MainWindowHandle;
				}
			}
		}
		return intPtr;
	}
}
