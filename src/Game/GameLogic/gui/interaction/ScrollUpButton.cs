using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI.Interaction
{
	[Serializable]
	public class ScrollUpButton : Entity
	{
		public static readonly Rectangle ScreenRectangle = new Rectangle(290, Verbs.OFFSET + 17, 38, 46);
		public static readonly Rectangle TextureRectangle = new Rectangle(290, 17, 38, 46);

		public ScrollUpButton()
		{
			HotspotRectangle
				.Create(this)
				.SetCaption(string.Empty)
				.AddRectangle(ScreenRectangle);

			Transform
				.Create(this)
				.SetZ(InteractionBar.Z + 1);

			Interactive = false;
		}
	}
}
