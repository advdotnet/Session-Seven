using System;

namespace SessionSeven.GUI.Dialog
{
	/// <summary>
	/// Simple dialog options
	/// </summary>
	[Serializable]
	public class Options : BaseOptions<BaseOption>
	{
		private Options() { }

		public static Options Create() => new Options();

		public Options Add(int id, string text)
		{
			if (id == -1)
			{
				throw new ArgumentException("Option.ID must not equal -1.");
			}

			OptionSet.Add(new BaseOption(id, text));

			return this;
		}

		public Options Add(int id, string text, Func<bool> predicate)
		{
			if (predicate())
			{
				Add(id, text);
			}

			return this;
		}

		public Options RemoveByID(int id)
		{
			for (var i = 0; i < OptionSet.Count; i++)
			{
				if (id == OptionSet[i].ID)
				{
					OptionSet.RemoveAt(i);
					return this;
				}
			}

			return this;
		}
	}
}
