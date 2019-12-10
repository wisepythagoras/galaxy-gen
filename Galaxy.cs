using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace Galaxy
{
	public class Galaxy
	{
		// Private class variables.
		private PointF[] points;
		private int AmountOfStars = 180000;

		/// <summary>
		/// Initializes a new instance of the <see cref="Galaxy.Galaxy"/> class.
		/// </summary>
		public Galaxy ()
		{
		}

		/// <summary>
		/// Gets the points.
		/// </summary>
		/// <returns>The points.</returns>
		public PointF[] getPoints()
		{
			return points;
		}

		/// <summary>
		/// Generates the galactic body; based on this article/tutorial:
		/// http://itinerantgames.tumblr.com/post/78592276402/a-2d-procedural-galaxy-with-c
		/// </summary>
		public void GenerateGalacticBody(string name = "galaxy")
		{
			Random random = new Random ();

			int numArms = random.Next(2, 6);
			float armSeparationDistance = 2 * (float) Math.PI / numArms;
			float armOffsetMax = (float) (random.NextDouble () * (0.3 - 0.9) + 0.9); //0.9f;
			float rotationFactor = random.Next(2, 10); //5;
			float randomOffsetXY = (float) (random.NextDouble () * (0.02 - 0.06) + 0.06); //0.04f;

			PolarCords[] starPositions = new PolarCords[AmountOfStars];

			using (StreamWriter sw = File.CreateText ("galaxy.csv")) {
				for (int i = 0; i < AmountOfStars; i++) {
					// Choose a distance from the center of the galaxy.
					float distance = (float) (random.NextDouble () * (0.01 - 0.99) + 0.99);
					distance = (float) Math.Pow (distance, 2);

					// Choose an angle between 0 and 2 * PI.
					float angle = (float)random.NextDouble () * 2 * (float)Math.PI;
					float armOffset = (float)random.NextDouble () * armOffsetMax;
					armOffset = armOffset - armOffsetMax / 2;
					armOffset = armOffset * (1 / distance);

					float squaredArmOffset = (float)Math.Pow (armOffset, 2);
					if (armOffset < 0)
						squaredArmOffset = squaredArmOffset * -1;
					armOffset = squaredArmOffset;

					float rotation = distance * rotationFactor;

					angle = (int)(angle / armSeparationDistance) * armSeparationDistance + armOffset + rotation;

					// Convert polar coordinates to 2D cartesian coordinates.
					float starX = (float)Math.Cos (angle) * distance;
					float starY = (float)Math.Sin (angle) * distance;

					float randomOffsetX = (float) random.NextDouble () * randomOffsetXY;
					float randomOffsetY = (float) random.NextDouble () * randomOffsetXY;

					starX += randomOffsetX;
					starY += randomOffsetY;

					if (800 * (1.25f + starX) > 2000 || 800 * (1.25f + starY) > 2000 ||
						800 * (1.25f + starX) < 0 || 800 * (1.25f + starY) < 0) {
						Console.WriteLine (starX + "x" + starY);
					}

					starX = 800 * (1.25f + starX);
					starY = 800 * (1.25f + starY);

					sw.WriteLine (starX + "," + starY);

					// Now we can assign xy coords.
					starPositions [i].x = starX;
					starPositions [i].y = starY;
				}
			}

			// Save the galaxy as an image.
			Bitmap bmp = new Bitmap (2000, 2000);
			for (int i = 0; i < 2000; i++) {
				for (int j = 0; j < 2000; j++) {
					bmp.SetPixel (i, j, Color.Black);
				}
			}

			foreach(PolarCords cords in starPositions) {
				bmp.SetPixel (
					(int) cords.x,
					(int) cords.y,
					Color.AliceBlue
				);
			}

			bmp.Save(name + ".png", System.Drawing.Imaging.ImageFormat.Png);
		}

		public void DrawImage(string filename, int width, int height)
		{
			Bitmap bmp = new Bitmap (width, height);
			Brush brush = new SolidBrush (Color.FromArgb (20, 150, 200, 255));
			//File.Create (filename + ".dat");
			//Graphics graphics = Graphics.FromImage (Image.FromFile(filename));

			/*Image image;

			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				image = Image.FromStream(fs);
			}

			Graphics graphics = Graphics.FromImage (image);

			foreach (PointF point in points)
			{
				Point screenPoint = new Point((int)(point.X * (float)width), (int)(point.Y * (float)height));
				screenPoint.Offset(new Point(-2, -2));
				graphics.FillRectangle(brush, new Rectangle(screenPoint, new Size(4, 4)));
			}

			image.Save (filename, System.Drawing.Imaging.ImageFormat.Png);*/

			using (StreamWriter sw = File.CreateText (filename + ".dat"))
			{
				foreach (PointF point in points)
				{
					bmp.SetPixel (
						0,0,//(int)(Math.Abs(point.X) * (float)width),
						//(int)(Math.Abs(point.Y) * (float)height),
						Color.White
					);

					string data = (int)(Math.Abs (point.X) * (float)width) + "," +
					              (int)(Math.Abs (point.Y) * (float)height);

					// Debug
					if ((int)( (point.X) * (float)width) >= 1600 || (int)((point.X) * (float)width) < 0 ||
						(int)(Math.Abs (point.Y) * (float)height) >= 1600 || (int)((point.Y) * (float)height) < 0)
					{
						Console.Write (data);
						Console.WriteLine ("\t" + point.X + "x" + point.Y);
					}

					sw.WriteLine (data);
				}
			}

			bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
		}

		/// <summary>
		/// Render the specified g, width and height.
		/// </summary>
		/// <param name="g">The green component.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public void Render(Graphics g, int width, int height)
		{
			using (Brush brush = new SolidBrush(Color.FromArgb(20, 150, 200, 255)))
			{
				g.Clear(Color.Black);
				foreach (PointF point in points)
				{
					Point screenPoint = new Point((int)(point.X * (float)width), (int)(point.Y * (float)height));
					screenPoint.Offset(new Point(-2, -2));
					g.FillRectangle(brush, new Rectangle(screenPoint, new Size(4, 4)));
				}
				g.Flush();
			}
		}

		/// <summary>
		/// Generates the galaxy.
		/// </summary>
		/// <returns>The galaxy.</returns>
		/// <param name="numOfStars">Number of stars.</param>
		/// <param name="numOfArms">Number of arms.</param>
		/// <param name="spin">Spin.</param>
		/// <param name="armSpread">Arm spread.</param>
		/// <param name="starsAtCenterRatio">Stars at center ratio.</param>
		public PointF[] GenerateGalaxy(int numOfStars, int numOfArms, float spin, double armSpread, double starsAtCenterRatio)
		{
			List<PointF> result = new List<PointF>(numOfStars);

			for (int i = 0; i < numOfArms; i++)
			{
				result.AddRange(GenerateArm(numOfStars / numOfArms, (float)i / (float)numOfArms, spin, armSpread, starsAtCenterRatio));
			}

			points = result.ToArray ();
			return result.ToArray ();
		}

		/// <summary>
		/// Generates the arm.
		/// </summary>
		/// <returns>The arm.</returns>
		/// <param name="numOfStars">Number of stars.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="spin">Spin.</param>
		/// <param name="armSpread">Arm spread.</param>
		/// <param name="starsAtCenterRatio">Stars at center ratio.</param>
		public PointF[] GenerateArm(int numOfStars, float rotation, float spin, double armSpread, double starsAtCenterRatio)
		{
			PointF[] result = new PointF[numOfStars];
			Random r = new Random();

			for (int i = 0; i < numOfStars; i++)
			{
				double part = (double)i / (double)numOfStars;
				part = Math.Pow(part, starsAtCenterRatio);

				float distanceFromCenter = (float)part;
				double position = (part * spin + rotation) * Math.PI * 2;

				double xFluctuation = (Pow3Constrained(r.NextDouble()) - Pow3Constrained(r.NextDouble())) * armSpread;
				double yFluctuation = (Pow3Constrained(r.NextDouble()) - Pow3Constrained(r.NextDouble())) * armSpread;

				float resultX = (float)Math.Cos(position) * distanceFromCenter / 2 + 0.5f + (float)xFluctuation;
				float resultY = (float)Math.Sin(position) * distanceFromCenter / 2 + 0.5f + (float)yFluctuation;

				result[i] = new PointF(resultX, resultY);
			}

			return result;
		}

		/// <summary>
		/// Pow3s the constrained.
		/// </summary>
		/// <returns>The constrained.</returns>
		/// <param name="x">The x coordinate.</param>
		public static double Pow3Constrained(double x)
		{
			double value = Math.Pow(x - 0.5, 3) * 4 + 0.5d;
			return Math.Max(Math.Min(1, value), 0);
		}
	}
}

