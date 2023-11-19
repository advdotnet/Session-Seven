using System.Collections.Generic;

namespace SessionSeven.GUI.Dialog
{
	public interface IDialogOptions
	{
		BaseOption this[int index]
		{
			get;
		}

		int Count { get; }

		IEnumerable<string> SelectTexts();
	}
}
