using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Office
{
	[Serializable]
	public class Therapist : Entity
	{
		public Therapist()
		{
			Transform
				.Create(this)
				.SetPosition(440, 200)
				.SetZ(2)
				.SetAbsolute(true);

			Text
				.Create(this)
				.SetColor(Color.OrangeRed)
				.SetFont(content.fonts.pixeloperator_outline_BMF)
				.SetWidth(300);

			Sprite
				.Create(this)
				.SetImage(content.rooms.office.therapist, 5, 1);

			SpriteTransformAnimation
				.Create(this)
				.SetSetFrameFn(SetFrame);

			SpriteData
				.Create(this)
				.SetOffset(-87, 45);

			Scripts
				.Create(this);
		}

		public Script Say(string text, float duration = 0)
		{
			return Get<Scripts>().Say(text, duration);
		}

		private int SetFrame(Transform transform, int step, int lastFrame)
		{
			// 1,2 talk
			// 3,4,5 write

			var scaledStep = step / 10;

			if (transform.State == State.Idle)
			{
				return 1;
			}

			if (transform.State.Has(State.Talking))
			{
				if (lastFrame < 2 && World.Get<Randomizer>().CreateInt(10) > 4)
				{
					return lastFrame;
				}

				var result = scaledStep % 2;

				return 1 + result;
			}

			// writing
			return 3 + (scaledStep % 3);
		}

		public void StartWriting()
		{
			Get<Transform>().SetState(State.Custom);
		}

		public void StopWriting()
		{
			Get<Transform>().SetState(State.Idle);
		}
	}
}
