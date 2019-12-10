using System;
using System.IO;
using System.Drawing;

namespace Galaxy
{
	/// <summary>
	/// Galaxy generator.
	/// </summary>
	public class GalaxyGenerator
	{
		// Private class variables

		private int Seed;
		private GalaxyOptions Options;
		private PolarCords[] StarPositions;
		private Random random;

		/// <summary>
		/// Initializes a new instance of the <see cref="Galaxy.GalaxyGenerator"/> class.
		/// </summary>
		public GalaxyGenerator (GalaxyOptions options, int seed = 0)
		{
			this.Options = options;
			this.Seed = seed;
			this.StarPositions = new PolarCords[Options.AmountOfStars];
			
			if (this.Seed != 0) {
				this.random = new Random (this.Seed);
			}
			else {
				this.random = new Random ();
			}
		}

		/// <summary>
		/// Gets the positions of the stars.
		/// </summary>
		/// <returns>The star positions.</returns>
		public PolarCords[] GetStarPositions()
		{
			return this.StarPositions;
		}

		/// <summary>
		/// Generates the galactic body. Based on this:
		/// http://itinerantgames.tumblr.com/post/78592276402/a-2d-procedural-galaxy-with-c
		/// </summary>
		public void GenerateGalacticBody()
		{
			/*int numArms = random.Next(2, 6);
			float armSeparationDistance = 2 * (float) Math.PI / numArms;
			float armOffsetMax = (float) (random.NextDouble () * (0.3 - 0.9) + 0.9); //0.9f;
			float rotationFactor = random.Next(2, 10); //5;
			float randomOffsetXY = (float) (random.NextDouble () * (0.02 - 0.06) + 0.06); //0.04f;
			*/

			for (int i = 0; i < Options.AmountOfStars; i++) {
				// Choose a distance from the center of the galaxy.
				float distance = (float) (this.random.NextDouble () * (0.01 - 0.99) + 0.99);
				distance = (float) Math.Pow (distance, 2);

				// Choose an angle between 0 and 2 * PI.
				float angle = (float) this.random.NextDouble () * 2 * (float)Math.PI;
				float armOffset = (float) this.random.NextDouble () * Options.MaxArmOffset;

				armOffset = armOffset - Options.MaxArmOffset / 2;
				armOffset = armOffset * (1 / distance);

				float squaredArmOffset = (float) Math.Pow (armOffset, 2);

				// Take the positive value for the offset.
				if (armOffset < 0) {
					squaredArmOffset = Math.Abs (squaredArmOffset); // * -1;
				}

				armOffset = squaredArmOffset;

				float rotation = distance * Options.RotationFactor;

				// Compute the angle of the arms.
				angle = (int) (
					(this.random.NextDouble() * (0.71 - 0.99) + 0.99) * // Not necessary to be here.
					angle / Options.ArmSeparationDistance) *
					Options.ArmSeparationDistance + armOffset + rotation;

				// Convert polar coordinates to 2D cartesian coordinates.
				float starX = (float)Math.Cos (angle) * distance;
				float starY = (float)Math.Sin (angle) * distance;

				float randomOffsetX = (float) random.NextDouble () * Options.RandomOffsetXY;
				float randomOffsetY = (float) random.NextDouble () * Options.RandomOffsetXY;

				// Apply the random offset.
				starX += randomOffsetX;
				starY += randomOffsetY;

				// Amplify the results so that they can be visible to the human eye.
				starX = 1.25f + starX;
				starY = 1.25f + starY;

				// Assign the proper x and y coordinates.
				this.StarPositions[i].x = starX;
				this.StarPositions[i].y = starY;
			}
		}

		/// <summary>
		/// Draws the galaxy into an image.
		/// </summary>
		public void DrawGalaxy(int widthHeight = 2000)
		{
			int width = widthHeight;
			int height = widthHeight;

			// Save the galaxy as an image.
			Bitmap bmp = new Bitmap (width, height);

			// Default the background to black,
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					bmp.SetPixel (i, j, Color.Black);
				}
			}

			int factor = (int) (widthHeight / 1.25 / 2);

			// Set the positions of the stars.
			foreach(PolarCords cords in this.StarPositions) {
				bmp.SetPixel (
					(int) (factor * cords.x),
					(int) (factor * cords.y),
					Color.AliceBlue
				);
			}

			// Save the image.
			bmp.Save(Options.Name + ".png", System.Drawing.Imaging.ImageFormat.Png);
		}
	}
}
