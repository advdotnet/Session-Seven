using SessionSeven.Components;
using System;
using System.Collections.Generic;

namespace SessionSeven.GUI.Dialog
{
	[Serializable]
	public class ScoreOption : BaseOption
	{
		public readonly Dictionary<ScoreType, int> ScoreSet;

		public ScoreOption(int id, string text, Dictionary<ScoreType, int> scoreSet) : base(id, text)
		{
			ScoreSet = scoreSet;
		}

		public static new readonly ScoreOption None = new ScoreOption(0, string.Empty, null);
	}
}
