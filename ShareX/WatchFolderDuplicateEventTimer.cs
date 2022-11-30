using System.Diagnostics;

namespace ShareX;

public class WatchFolderDuplicateEventTimer
{
	private const int expireTime = 1000;

	private Stopwatch timer;

	private string path;

	public bool IsElapsed => timer.ElapsedMilliseconds >= 1000;

	public WatchFolderDuplicateEventTimer(string path)
	{
		timer = Stopwatch.StartNew();
		this.path = path;
	}

	public bool IsDuplicateEvent(string path)
	{
		int num;
		if (path == this.path)
		{
			num = ((!IsElapsed) ? 1 : 0);
			if (num != 0)
			{
				timer = Stopwatch.StartNew();
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}
}
