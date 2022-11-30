using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class EasterEggAboutAnimation : IDisposable
{
	private EasterEggBounce easterEggBounce;

	private int direction;

	public Canvas Canvas { get; private set; }

	public bool IsPaused { get; set; }

	public Size Size { get; set; } = new Size(200, 200);


	public int Step { get; set; } = 10;


	public int MinStep { get; set; } = 3;


	public int MaxStep { get; set; } = 35;


	public int Speed { get; set; } = 1;


	public Color Color { get; set; } = new HSB(0.0, 1.0, 0.9);


	public int ClickCount { get; private set; }

	public EasterEggAboutAnimation(Canvas canvas, Form form)
	{
		Canvas = canvas;
		Canvas.MouseDown += Canvas_MouseDown;
		Canvas.Draw += Canvas_Draw;
		easterEggBounce = new EasterEggBounce(form);
	}

	public void Start()
	{
		direction = Speed;
		Canvas.Start(50);
	}

	private void Canvas_MouseDown(object sender, MouseEventArgs e)
	{
		IsPaused = !IsPaused;
		if (!easterEggBounce.IsWorking)
		{
			ClickCount++;
			if (ClickCount >= 10)
			{
				easterEggBounce.ApplyGravity = e.Button == MouseButtons.Left;
				easterEggBounce.Start();
			}
		}
		else
		{
			easterEggBounce.Stop();
		}
	}

	private void Canvas_Draw(Graphics g)
	{
		g.SetHighQuality();
		int num = Size.Width / 2;
		int num2 = Size.Height / 2;
		using (Matrix matrix = new Matrix())
		{
			matrix.RotateAt(45f, new PointF(num, num2));
			g.Transform = matrix;
		}
		using (Pen pen = new Pen(Color, 2f))
		{
			for (int i = 0; i <= num; i += Step)
			{
				g.DrawLine(pen, i, num2, num, num2 - i);
				g.DrawLine(pen, num, i, num + i, num2);
				g.DrawLine(pen, Size.Width - i, num2, num, num2 + i);
				g.DrawLine(pen, num, Size.Height - i, num - i, num2);
			}
		}
		if (!IsPaused)
		{
			if (Step + Speed > MaxStep)
			{
				direction = -Speed;
			}
			else if (Step - Speed < MinStep)
			{
				direction = Speed;
			}
			Step += direction;
			HSB hSB = Color;
			if (hSB.Hue >= 1.0)
			{
				hSB.Hue = 0.0;
			}
			else
			{
				hSB.Hue += 0.01;
			}
			Color = hSB;
		}
	}

	public void Dispose()
	{
		if (easterEggBounce != null)
		{
			easterEggBounce.Dispose();
		}
	}
}
