using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class BloodOnFloor : Entity
	{
		public BloodOnFloor()
		{
			Interaction
				.Create(this)
				.SetWalkToClickPosition(true)
				.SetPosition(360, 260)
				.SetDirection(Directions8.Left)
				.SetGetInteractionsFn(GetInteractions);

			Transform
				.Create(this)
				.SetZ(Carpet.Z + 1);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.bloodstains)
				.AddRectangle(263, 250, 11, 8)
				.AddRectangle(332, 256, 16, 9);

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
				yield return Game.Ego.Say(Basement_Res.How_the_hell_did_that_happen, 2f);
			}
		}
	}
}
