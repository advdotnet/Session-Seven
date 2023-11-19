using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;
using System.Collections;

namespace SessionSeven.Actors
{
	[Serializable]
	public class RyanVoice : Entity
	{
		public RyanVoice()
		{
			Transform
				.Create(this)
				.SetPosition((Game.VIRTUAL_WIDTH * 3 / 4) - 120, 150)
				.SetAbsolute(true);

			Text
				.Create(this)
				.SetColor(Color.White)
				.SetFont(content.fonts.pixeloperator_outline_BMF)
				.SetWidth(600);

			Scripts
				.Create(this);
		}

		public Script Say(string text, float duration = 0)
		{
			return Get<Scripts>().Start(SayWrapper(text, duration));
		}

		private IEnumerator SayWrapper(string text, float duration = 0)
		{
			Tree.Basement.RyanLying.Get<Transform>().State = State.Talking;
			var sayScript = Get<Scripts>().Say(text, duration);
			yield return Script.WaitFor(sayScript);
			Tree.Basement.RyanLying.Get<Transform>().State = State.Idle;
		}
	}
}
