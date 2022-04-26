using System;
using System.Windows.Forms;

namespace GravityBalls
{
	public class WorldModel
	{
		public double BallX;
		public double BallY;
		public double BallRadius;

		public double PlayerX;
		public double PlayerY;
		public double PlayerHeight = 50;
		public double PlayerWidth = 20;
		public double PlayerSpeed = 10;
		public int PlayerPoint = 0;

		public double EnemyX;
		public double EnemyY;
		public double EnemyHeight = 50;
		public double EnemyWidth = 20;
		public double EnemySpeed = 2;
		public int EnemyPoint = 0;

		public double WorldWidth;
		public double WorldHeight;
		public double BallVx = 100;
		public double BallVy = 100;
		public double Resistance = 0.2;
		public double G = 500;
		public double Force = 300000;

		public void SimulateTimeframe(double dt, EventArgs e)
		{
			MoveBall(dt);
			ApplyWallsBouncing();
			ApplyPlayerBouncing();
			ApplyEnemyMove();
			ApplyEnemyBouncing();
			ApplyPointsCount();
			//	ApplyAirResistance(dt);
			//	ApplyGravity(dt);
			//	ApplyCursorRepulsion(dt);
		}

		private void MoveBall(double dt)
		{
			BallX += BallVx * dt;
			BallY += BallVy * dt;

			BallX = Math.Max(BallRadius, Math.Min(BallX, WorldWidth - BallRadius));
			BallY = Math.Max(BallRadius, Math.Min(BallY, WorldHeight - BallRadius));
		}

		private void ApplyWallsBouncing()
		{
			if (BallY + BallRadius >= WorldHeight || BallY - BallRadius <= 0)
				BallVy = -BallVy;
			if (BallX + BallRadius >= WorldWidth || BallX - BallRadius <= 0)
				BallVx = -BallVx;
		}

		private void ApplyPlayerBouncing()
		{
			if (BallX + BallRadius >= PlayerX 
				&& BallY + BallRadius >= PlayerY 
				&& BallY - BallRadius <= PlayerY + PlayerHeight)
				BallVx = -BallVx;
		}

		private void ApplyEnemyBouncing()
		{
			if (BallX - BallRadius <= EnemyX + EnemyWidth
				&& BallY + BallRadius >= EnemyY
				&& BallY - BallRadius <= EnemyY + EnemyHeight)
				BallVx = -BallVx;
		}

		private void ApplyPointsCount()
        {
			if (BallX - BallRadius == 0)
				StartNewRound(true);
			if (BallX + BallRadius == WorldWidth)
				StartNewRound(false);

        }

		private void ApplyEnemyMove() 
		{
			if (EnemyY + EnemyHeight / 2 < BallY)
				EnemyY += EnemySpeed;
			if (EnemyY + EnemyHeight / 2 > BallY)
				EnemyY -= EnemySpeed;
		}
		private void StartNewRound(bool isPlayer)
		{
			BallX = WorldWidth / 2;
			BallY = WorldHeight / 2;
			var rnd = new Random().Next(2);
			if (rnd == 0)
            {
				BallVx = -BallVx;
            }
			if (rnd == 1)
            {
				BallVx = BallVx;
            }				
			if (isPlayer)
            {
				PlayerPoint++;
            }
			else
            {
				EnemyPoint++;
            }
		}

		private void ApplyAirResistance(double dt)
		{
			BallVx = BallVx - BallVx * Resistance * dt;
			BallVy = BallVy - BallVy * Resistance * dt;
		}

		private void ApplyGravity(double dt)
		{
			BallVy += G * dt;
		}

		private void ApplyCursorRepulsion(double dt)
		{
			var cursorX = Cursor.Position.X;
			var cursorY = Cursor.Position.Y;
			var dx = BallX - cursorX;
			var dy = BallY - cursorY;
			var d = Math.Sqrt(dx * dx + dy * dy);
			var f = Force / (d * d);

			BallVx += dx * f * dt;
			BallVy += dy * f * dt;
		}

		
	}
}