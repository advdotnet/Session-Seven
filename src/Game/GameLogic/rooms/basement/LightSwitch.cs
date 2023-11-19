using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class LightSwitch : Entity
	{
		public LightSwitch()
		{
			Interaction
				.Create(this)
				.SetPosition(289, 242)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.light_switch)
				.AddRectangle(290, 136, 11, 10);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Push, UseScript())
					.Add(Verbs.Use, UseScript())
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator UseScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.StartUse();
				Tree.Basement.Lights.Toggle();
				yield return Game.Ego.StopUse();
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.It_is_a_light_switch);
			}
		}
	}
}
