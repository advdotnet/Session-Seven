using System;

namespace SessionSeven.GUI.Dialog
{
	[Serializable]
	public class BaseOption
	{
		public readonly int ID;
		public readonly string Text;

		public BaseOption(int id, string text)
		{
			ID = id;
			Text = text;
		}

		public static readonly BaseOption None = new BaseOption(0, string.Empty);
	}
}
