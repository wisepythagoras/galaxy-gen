using System;
using System.Drawing;
using Galaxy;

namespace Galaxy
{
	class MainClass
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args)
		{
			Console.WriteLine("Creating galaxy");

			// GalaxyOptions myGalaxyOptions = new GalaxyOptions ("My Huge Galaxy", 18000000, 10, 1.65f, 5, 0.08f);
			GalaxyOptions myGalaxyOptions =  new GalaxyOptions("Galaxy", 200000, 4, 1.05f, 5, 0.05f);
			// GalaxyOptions myGalaxyOptions = new GalaxyOptions ("My Galaxy", 180000, 6, 1.35f, 5, 0.08f);
			GalaxyGenerator myGalaxy = new GalaxyGenerator(myGalaxyOptions, LongRandom(10, 1234567, new Random()));
			myGalaxy.GenerateGalacticBody();
			myGalaxy.DrawGalaxy(4000);
		}

		/// <summary>
		/// Longs the random. Taken from here, but modified:
		/// http://stackoverflow.com/questions/6651554/random-number-in-long-range-is-this-the-way#answer-6651661
		/// </summary>
		/// <returns>The random.</returns>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		/// <param name="rand">Rand.</param>
		private static int LongRandom(int min, int max, Random rand)
		{
			int result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
			result = (result << 32);
			result = result | rand.Next((Int32)min, (Int32)max);
			return result;
		}
	}
}
