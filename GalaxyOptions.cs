using System;

namespace Galaxy
{
	/// <summary>
	/// Galaxy options.
	/// </summary>
	public class GalaxyOptions
	{
		public string Name;
		public int AmountOfStars;
		public int NumberOfArms;
		public float MaxArmOffset;
		public float RotationFactor;
		public float RandomOffsetXY;
		public float ArmSeparationDistance;

		/// <summary>
		/// Initializes a new instance of the <see cref="Galaxy.GalaxyOptions"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="amountofstars">Amount of stars.</param>
		/// <param name="numberofarms">Number of arms.</param>
		/// <param name="maxarmoffset">Max armo ffset.</param>
		/// <param name="rotationfactor">Rotation factor.</param>
		/// <param name="randomoffsetxy">Random offset x-y.</param>
		public GalaxyOptions(string name, int amountofstars = 30000, int numberofarms = 4,
			float maxarmoffset = 0.9f, float rotationfactor = 5f, float randomoffsetxy = 0.04f)
		{
			this.Name = name;
			this.AmountOfStars = amountofstars;
			this.NumberOfArms = numberofarms;
			this.MaxArmOffset = maxarmoffset;
			this.RotationFactor = rotationfactor;
			this.RandomOffsetXY = randomoffsetxy;
			this.ArmSeparationDistance = 2 * (float)Math.PI / this.NumberOfArms;
		}
	}
}

