using STACK;
using STACK.Components;
using System;
using System.Collections;

namespace SessionSeven.Basement
{
	[Serializable]
	public class Receipt : Entity
	{
		public Receipt()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.receipt_anim, 11);

			Transform
				.Create(this)
				.SetPosition(673, 130)
				.SetZ(3);

			Enabled = false;
			Visible = false;
		}

		public IEnumerator FallDownScript()
		{
			const int delay = 6;

			for (var i = 0; i < Get<Sprite>().Columns; i++)
			{
				yield return Delay.Updates(delay);
				Get<Sprite>().CurrentFrame = i + 1;
			}
		}
	}
}
