using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class CabinetSupplies : Entity
	{
		public CabinetSupplies()
		{
			Interaction
				.Create(this)
				.SetPosition(703, 247)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			Transform
				.Create(this)
				.SetZ(3);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.arts_and_crafts_supplies)
				.AddRectangle(703, 149, 44, 28);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Pick, PickScript());
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Cynthia_took_control_of_this_cabinet_compartment_for_her_arts_and_crafts_supplies_a_while_ago);
				yield return Game.Ego.Say(Basement_Res.Its_full_of_all_sorts_of_glitter_glues_paint_and_what_not);
				yield return Game.Ego.Say(Basement_Res.It_doesnt_look_like_shes_touched_any_of_it_in_a_while_though);
			}
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_dont_want_to_carry_that_around_now);
			}
		}
	}
}
