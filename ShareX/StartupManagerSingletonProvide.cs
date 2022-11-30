namespace ShareX;

public static class StartupManagerSingletonProvider
{
	public static IStartupManager CurrentStartupManager { get; private set; }

	static StartupManagerSingletonProvider()
	{
		CurrentStartupManager = new DesktopStartupManager();
	}
}
