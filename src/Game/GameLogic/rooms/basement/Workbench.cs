using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	/// <summary>
	/// Workbench
	/// </summary>
	[Serializable]
	public class Workbench : Entity
	{
		public const int Z = 246;
		public Workbench()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.bench);

			Transform
				.Create(this)
				.SetPosition(609, 162)
				.SetZ(Z);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.workbench)
				.AddRectangle(416, 153, 225, 94);

			Interaction
				.Create(this)
				.SetPosition(510, 259)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Use, UseScript());
		}

		private IEnumerator UseScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_have_no_idea_on_what_to_work_at_the_moment);
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Its_my_workbench);
				yield return Game.Ego.Say(Basement_Res.I_told_Landon_a_while_ago_that_I_would_help_him_make_a_box_car_for_class_with_these_tools);
				yield return Game.Ego.Say(Basement_Res.I_still_havent_looked_up_how_to_make_one_Come_to_think_of_it_the_assignment_is_probably_over_by_now);
			}
		}
	}
}
