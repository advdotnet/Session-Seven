using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI.Interaction
{
	[Serializable]
	public class InteractionBar : Entity
	{
		public const float Z = 1;
		public const int HEIGHT = 288;

		public InteractionBar()
		{
			HotspotRectangle
				.Create(this)
				.SetCaption(string.Empty)
				.SetRectangle(0, HEIGHT, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT - HEIGHT);

			Transform
				.Create(this)
				.SetZ(Z);
		}
	}
}
