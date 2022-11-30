using System.Windows.Forms;

namespace ShareX;

public class DesktopStartupManager : GenericStartupManager
{
	public override string StartupTargetPath => Application.ExecutablePath;
}
