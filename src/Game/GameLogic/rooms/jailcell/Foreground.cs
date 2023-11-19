using STACK;
using STACK.Components;
using System;

namespace SessionSeven.JailCell
{
	/// <summary>
	/// Boxes in the foreground
	/// </summary>
	[Serializable]
	public class Foreground : Entity
	{
		public Foreground()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.jailcell.foreground);

			DrawOrder = 400;
		}
	}
}
