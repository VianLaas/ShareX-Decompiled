using System;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class EasterEggBounce : IDisposable
{
	private Timer timer;

	private Point velocity;

	public Form Form { get; private set; }

	public bool IsWorking { get; private set; }

	public Rectangle BounceRectangle { get; set; }

	public int Speed { get; set; } = 20;


	public bool ApplyGravity { get; set; } = true;


	public int GravityPower { get; set; } = 3;


	public int BouncePower { get; set; } = 50;


	public EasterEggBounce(Form form)
	{
		Form = form;
		timer = new Timer();
		timer.Interval = 20;
		timer.Tick += bounceTimer_Tick;
		BounceRectangle = CaptureHelpers.GetScreenWorkingArea();
	}

	public void Start()
	{
		if (!IsWorking)
		{
			IsWorking = true;
			velocity = new Point(RandomFast.Pick<int>(-Speed, Speed), ApplyGravity ? GravityPower : RandomFast.Pick<int>(-Speed, Speed));
			timer.Start();
		}
	}

	public void Stop()
	{
		if (IsWorking)
		{
			if (timer != null)
			{
				timer.Stop();
			}
			IsWorking = false;
		}
	}

	private void bounceTimer_Tick(object sender, EventArgs e)
	{
		if (Form == null || Form.IsDisposed)
		{
			return;
		}
		int num = Form.Left + velocity.X;
		int num2 = BounceRectangle.X + BounceRectangle.Width - Form.Width - 1;
		if (num <= BounceRectangle.X)
		{
			num = BounceRectangle.X;
			velocity.X = Speed;
		}
		else if (num >= num2)
		{
			num = num2;
			velocity.X = -Speed;
		}
		int num3 = Form.Top + velocity.Y;
		int num4 = BounceRectangle.Y + BounceRectangle.Height - Form.Height - 1;
		if (ApplyGravity)
		{
			if (num3 >= num4)
			{
				num3 = num4;
				velocity.Y = -BouncePower + RandomFast.Next(-10, 10);
			}
			else
			{
				velocity.Y += GravityPower;
			}
		}
		else if (num3 <= BounceRectangle.Y)
		{
			num3 = BounceRectangle.Y;
			velocity.Y = Speed;
		}
		else if (num3 >= num4)
		{
			num3 = num4;
			velocity.Y = -Speed;
		}
		Form.Location = new Point(num, num3);
	}

	public void Dispose()
	{
		Stop();
		if (timer != null)
		{
			timer.Dispose();
		}
	}
}
