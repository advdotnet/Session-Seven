using System;

namespace SessionSeven.GUI.Interaction
{
	/// <summary>
	/// Represents any object in interactions.
	/// </summary>
	[Serializable]
	public class Any
	{
		public static readonly Any Object = new Any();

		private Any() { }

		public override bool Equals(object obj)
		{
			if (!(obj is Any))
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode() => 0;

		public override string ToString() => "Any Object";
	}
}
