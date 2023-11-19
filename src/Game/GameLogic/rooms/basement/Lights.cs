using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{
	[Serializable]
	public class Lights : Entity
	{
		public Lights()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.lightsoff);

			DrawOrder = 1;

			Visible = false;
		}

		public void SwitchOn()
		{
			Game.PlaySoundEffect(content.audio.light_on);
			Visible = false;
		}

		public void SwitchOff()
		{
			Game.PlaySoundEffect(content.audio.light_off);
			Visible = true;
		}

		public void Toggle()
		{
			if (!Visible)
			{
				SwitchOff();
			}
			else
			{
				SwitchOn();
			}
		}
	}
}
