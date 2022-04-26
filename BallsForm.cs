using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GravityBalls
{
	public class BallsForm : Form
	{
		private Timer timer;
		private WorldModel world;
		private Font font;
		private WorldModel CreateWorldModel()
		{
			var w = new WorldModel
			{
				WorldHeight = ClientSize.Height,
				WorldWidth = ClientSize.Width,
				BallRadius = 10,
			};
			w.PlayerY = w.WorldHeight / 2;
			w.PlayerX = w.WorldWidth - 30;
			w.EnemyX = 30;
			w.EnemyY = w.WorldHeight / 2;
			w.BallX = w.WorldHeight / 2;
			w.BallY = w.BallRadius;
			return w;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			world.WorldHeight = ClientSize.Height;
			world.WorldWidth = ClientSize.Width;
			world.PlayerX = world.WorldWidth - 30;
			world.EnemyX = 30;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			DoubleBuffered = true;
			BackColor = Color.Black;
			world = CreateWorldModel();
			
			font = new Font(FontFamily.GenericSansSerif, 12.0F, FontStyle.Bold);

			timer = new Timer { Interval = 30 };
			timer.Tick += TimerOnTick;
			timer.Start();

			world.WorldHeight = ClientSize.Height;
			world.WorldWidth = ClientSize.Width;
		}

		private void TimerOnTick(object sender, EventArgs eventArgs)
		{
			world.SimulateTimeframe(timer.Interval / 1000d, eventArgs);
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			var g = e.Graphics;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.FillEllipse(Brushes.GreenYellow,
				(float)(world.BallX - world.BallRadius),
				(float)(world.BallY - world.BallRadius),
				2 * (float)world.BallRadius,
				2 * (float)world.BallRadius);
			g.FillRectangle(Brushes.White, 
				(float)(world.PlayerX), 
				(float)(world.PlayerY), 
				(float)(world.PlayerWidth), 
				(float)(world.PlayerHeight));
			g.FillRectangle(Brushes.White,
				(float)(world.EnemyX),
				(float)(world.EnemyY),
				(float)(world.EnemyWidth),
				(float)(world.EnemyHeight));
			var points = string.Format("{0} | {1}", world.EnemyPoint, world.PlayerPoint);
			g.DrawString(points, font, Brushes.White, 
				(float) world.WorldWidth / 2, 
				(float) 10);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Text = string.Format("Cursor ({0}, {1})", e.X, e.Y);
		}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
			if (e.KeyCode == Keys.W)
				world.PlayerY -= world.PlayerSpeed;
			if (e.KeyCode == Keys.S)
				world.PlayerY += world.PlayerSpeed;
		}


        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BallsForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "BallsForm";
            this.Load += new System.EventHandler(this.BallsForm_Load);
            this.ResumeLayout(false);

        }

        private void BallsForm_Load(object sender, EventArgs e)
        {

        }
    }
}