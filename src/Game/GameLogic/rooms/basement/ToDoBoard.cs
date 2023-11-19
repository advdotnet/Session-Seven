using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class ToDoBoard : Entity
	{
		public ToDoBoard()
		{
			Interaction
				.Create(this)
				.SetPosition(127, 276)
				.SetDirection(Directions8.Left)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.bulletin_board)
				.AddRectangle(3, 98, 61, 69);

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
				yield return Game.Ego.Say(Basement_Res.This_bulletin_board_is_covered_in_reminders_for_meetings_with_the_realtor_the_bank_and_Landons_teachers_as_well_as_an_alphabetized_shopping_list_of_Cynthias);
				yield return Game.Ego.Say(Basement_Res.Landon_drew_something_in_the_corner_but_its_too_faded_now_to_make_out_what_its_supposed_to_be);
			}
		}
	}
}
