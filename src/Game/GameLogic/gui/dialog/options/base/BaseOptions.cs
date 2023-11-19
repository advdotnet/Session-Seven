using System;
using System.Collections.Generic;
using System.Linq;

namespace SessionSeven.GUI.Dialog
{
	[Serializable]
	public abstract class BaseOptions<T> : IDialogOptions where T : BaseOption
	{
		protected List<T> OptionSet = new List<T>(3);

		public int Count => OptionSet.Count;

		public BaseOption this[int index] => OptionSet[index];

		public IEnumerable<string> SelectTexts() => OptionSet.Select(o => o.Text);
	}
}
