using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class Boiler : Entity
	{
		public Boiler()
		{
			Interaction
				.Create(this)
				.SetPosition(994, 268)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.boiler)
				.AddRectangle(961, 32, 62, 131);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript());

		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.The_boiler_that_keeps_our_hot_water_running);
				yield return Delay.Seconds(0.5f);
				yield return Game.Ego.Say(Basement_Res.Man_I_could_use_a_hot_shower_right_about_now);
			}
		}
	}
}
