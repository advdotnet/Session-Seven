using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class TherapyLog : ItemBase
	{
		public TherapyLog() : base(content.inventory.therapylog, Items_Res.TherapyLog_TherapyLog_TherapyLog)
		{

		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				Game.EnableSkipping();

				yield return Game.Ego.Say(Items_Res.According_to_the_date_this_therapy_session_happened_some_months_ago);
				yield return Delay.Seconds(0.3f);
				yield return Game.Ego.Say(Items_Res.It_reads);
				yield return Delay.Seconds(0.5f);
				yield return Game.Ego.Say(Items_Res.Mr_Psychiatrist__When_you_first_realized_you_were_trapped_what_did_you_do_);

				switch (Tree.Cutscenes.Director.SessionTwoDialogOneOption)
				{
					case Cutscenes.Director.SessionTwoDialogOneOptionOne:
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__I_just_lost_it_I_was_terrified_);
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__I_was_just_trying_everything_I_could_to_get_out_of_there_);
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__But_then_I_wasnt_even_sure_how_Id_gotten_there_);
						break;
					case Cutscenes.Director.SessionTwoDialogOneOptionTwo:
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__I_got_frustrated_I_tried_to_remember_what_I_was_doing_before_it_all_started_);
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__Before_I_was_there_you_know_How_did_it_come_to_this_And_where_was_my_family_);
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__I_was_trying_to_think_back_on_it_and_I_just_couldnt_remember_);
						break;
					case Cutscenes.Director.SessionTwoDialogOneOptionThree:
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__I_stayed_calm_I_wasnt_just_going_to_give_up_);
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__I_did_my_best_to_figure_out_what_was_going_on_I_wasnt_just_going_to_sit_down_and_take_it_);
						yield return Game.Ego.Say(Items_Res.Mr_Schmidt__Especially_not_if_my_family_was_in_danger_somewhere_else_);
						break;
				}

				yield return Delay.Seconds(1f);

				yield return Game.Ego.Say(Items_Res.It_stops_here_The_other_page_seems_to_be_missing);

				Game.StopSkipping();
			}
		}
	}
}
