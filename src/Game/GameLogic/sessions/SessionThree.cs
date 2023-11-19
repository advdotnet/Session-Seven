using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Dialog;
using SessionSeven.Office;
using STACK;
using STACK.Components;
using System.Collections;
using System.Collections.Generic;
using GlblRes = global::SessionSeven.Properties.Resources_Session_3;

namespace SessionSeven.Cutscenes
{

	public partial class Director : Entity
	{
		public const int SessionThreeDialogOneOptionOne = 1;
		public const int SessionThreeDialogOneOptionTwo = 2;
		public const int SessionThreeDialogOneOptionThree = 3;

		public const int SessionThreeDialogTwoOptionOne = 1;
		public const int SessionThreeDialogTwoOptionTwo = 2;

		public const int SessionThreeDialogThreeOptionOne = 1;
		public const int SessionThreeDialogThreeOptionTwo = 2;
		public const int SessionThreeDialogThreeOptionThree = 3;

		public const int SessionThreeDialogFourOptionOne = 1;
		public const int SessionThreeDialogFourOptionTwo = 2;

		public int SessionThreeDialogThreeOption { get; private set; }

		public bool ForgaveCynthia => SessionThreeDialogThreeOptionTwo == SessionThreeDialogThreeOption;

		private IEnumerator SessionThreeScript()
		{
			Game.StopSong();
			Game.PlaySoundEffect(content.audio.transition_3);

			World.Get<AudioManager>().RepeatSong = true;

			Tree.Office.Scene.SetupEarly();
			Fader.Visible = false;
			Tree.GUI.Interaction.Scene.Interactive = false;

			Game.EnableSkipping();
			Tree.GUI.Interaction.Scene.Visible = false;
			Game.Ego.Inventory.Hide();

			Tree.Actors.Scene.Enabled = false;
			Tree.Basement.Scene.Interactive = false;
			Tree.Basement.Scene.Visible = false;

			Tree.Office.Scene.Visible = true;
			Tree.Office.Scene.Interactive = true;
			Tree.Office.Scene.Enabled = true;

			yield return Delay.Seconds(1);

			yield return Psychiatrist.Say(GlblRes.Lets_talk_about_something_less_painful_for_a_while);

			Game.PlaySong(content.audio.session3);
			World.Get<AudioManager>().RepeatSong = true;

			yield return Delay.Seconds(0.5f);

			yield return RyanVoice.Say(GlblRes.Sounds_good_to_me);

			yield return Delay.Seconds(0.5f);

			yield return Psychiatrist.Say(GlblRes.Good_to_hear_How_have_things_been_with_your_wife_since_all_of_this);

			Game.StopSkipping();

			var menu = Tree.GUI.Dialog.Menu;

			var rootDialog = ScoreOptions.Create()
				.Add(SessionThreeDialogOneOptionOne, GlblRes.I_thought_you_said_we_should_talk_about_something_LESS_painful, ScoreType.Insanity, 2)
				.Add(SessionThreeDialogOneOptionTwo, GlblRes.I_like_to_think_were_getting_better, ScoreType.Freedom, 2)
				.Add(SessionThreeDialogOneOptionThree, GlblRes.Honestly_I_still_havent_forgiven_her_for_all_this, ScoreType.Jail, 2);

			var subDialogOne = ScoreOptions.Create()
				.Add(SessionThreeDialogTwoOptionOne, GlblRes.Of_course, new Dictionary<ScoreType, int>()
				{
					{ ScoreType.Insanity, 1 },
					{ ScoreType.Jail, 1 }
				})
				.Add(SessionThreeDialogTwoOptionTwo, GlblRes.Maybe_not, ScoreType.Freedom, 1);

			menu.Open(rootDialog);

			yield return menu.StartSelectionScript(Get<Scripts>());
			var rootSelection = menu.LastSelectedOption;
			ProcessScore(rootSelection);

			Game.EnableSkipping();

			switch (rootSelection.ID)
			{
				case SessionThreeDialogOneOptionOne:

					yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
					yield return RyanVoice.Say(rootSelection.Text);
					yield return RyanVoice.Say(GlblRes.Cynthia_and_I_havent_been_on_particularly_good_ground_since_this_all_began);
					yield return RyanVoice.Say(GlblRes.Its_hard_to_reestablish_trust_in_somebody_that_constantly_seems_like_theyre_holding_something_back_);

					yield return Delay.Seconds(0.5f);

					yield return Psychiatrist.Say(GlblRes.And_you_do_think_that_shes_still_holding_something_back_from_you_Even_now);

					yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);

					Game.StopSkipping();

					menu.Open(subDialogOne);
					yield return menu.StartSelectionScript(Get<Scripts>());
					var subSelection1 = menu.LastSelectedOption;
					ProcessScore(subSelection1);

					Game.EnableSkipping();

					yield return RyanVoice.Say(subSelection1.Text);

					switch (subSelection1.ID)
					{
						case SessionThreeDialogTwoOptionOne:
							yield return RyanVoice.Say(GlblRes.She_has_to_be_You_dont_just_uproot_somebodys_life_like_that_with_such_little_reason);
							yield return RyanVoice.Say(GlblRes.Shes_still_hiding_something_from_me_even_now_I_know_she_is_She_didnt_even_want_me_coming_in_to_see_you);

							break;
						case SessionThreeDialogTwoOptionTwo:
							yield return RyanVoice.Say(GlblRes.I_I_dont_know_Honestly_I_just_think_this_is_going_to_take_time);
							yield return RyanVoice.Say(GlblRes.I_really_do_want_to_trust_her_again_Its_just_not_easy_);

							break;
					}

					break;

				case SessionThreeDialogOneOptionTwo:

					yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
					yield return RyanVoice.Say(rootSelection.Text);
					yield return RyanVoice.Say(GlblRes.Its_been_awhile_now_since_everything_went_down_Im_trying_my_best);
					yield return RyanVoice.Say(GlblRes.I_mean_its_hard_to_know_with_her_whether_or_not_shes_telling_the_truth_even_now_but_we_are_trying);
					yield return RyanVoice.Say(GlblRes.Yesterday_I_even_cooked_pasta_for_her);


					yield return Psychiatrist.Say(GlblRes.You_think_she_could_still_be_lying_to_you);

					yield return Delay.Seconds(2.5f);

					RyanEyesClosed.Blinking = false;
					RyanEyesClosed.Visible = true;
					yield return RyanVoice.Say(GlblRes.I_dont_know);
					RyanEyesClosed.Blinking = true;
					yield return RyanVoice.Say(GlblRes.Im_telling_myself_shes_not_that_weve_gotten_it_all_out_there_by_now_but_its_hard_to_say_for_sure);

					break;

				case SessionThreeDialogOneOptionThree:
					yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
					yield return RyanVoice.Say(rootSelection.Text);
					yield return RyanVoice.Say(GlblRes.I_know_you_keep_telling_me_I_should_just_move_on_but_its_just);
					yield return RyanVoice.Say(GlblRes.I_dont_know_if_I_ever_will_be_able_to_trust_her_again_Not_completely);
					yield return RyanVoice.Say(GlblRes.You_dont_just_move_on_from_something_like_that_you_know);


					yield return Psychiatrist.Say(GlblRes.But_do_you_really_think_shes_still_lying_to_you_Even_now);

					Game.StopSkipping();

					menu.Open(subDialogOne);
					yield return menu.StartSelectionScript(Get<Scripts>());
					var subSelection3 = menu.LastSelectedOption;
					ProcessScore(subSelection3);

					Game.EnableSkipping();

					yield return RyanVoice.Say(subSelection3.Text);

					switch (subSelection3.ID)
					{
						case SessionThreeDialogTwoOptionOne:
							yield return RyanVoice.Say(GlblRes.She_has_to_be_You_dont_just_uproot_somebodys_life_like_that_with_such_little_reason);
							yield return RyanVoice.Say(GlblRes.Shes_still_hiding_something_from_me_even_now_I_know_she_is_She_didnt_even_want_me_coming_in_to_see_you);

							break;
						case SessionThreeDialogTwoOptionTwo:
							yield return RyanVoice.Say(GlblRes.I_I_dont_know_Honestly_I_just_think_this_is_going_to_take_time);
							yield return RyanVoice.Say(GlblRes.I_really_do_want_to_trust_her_again_Its_just_not_easy_);

							break;
					}

					break;
			}

			yield return Psychiatrist.Say(GlblRes.Well_alright_thats_fair_But_tell_me_this_Ryan_Do_you_still_love_her);

			var loveLineDialog = ScoreOptions.Create()
				.Add(SessionThreeDialogThreeOptionOne, GlblRes.Can_I_really_love_somebody_I_dont_even_fully_trust, ScoreType.Insanity, 2)
				.Add(SessionThreeDialogThreeOptionTwo, GlblRes.Shes_my_wife_of_course_I_do, ScoreType.Freedom, 2)
				.Add(SessionThreeDialogThreeOptionThree, GlblRes.Not_anymore_I_cant, ScoreType.Jail, 2);

			Game.StopSkipping();

			menu.Open(loveLineDialog);

			yield return menu.StartSelectionScript(Get<Scripts>());

			var loveLineSelection = menu.LastSelectedOption;
			SessionThreeDialogThreeOption = menu.LastSelectedOption.ID;
			ProcessScore(loveLineSelection);

			Game.EnableSkipping();

			switch (loveLineSelection.ID)
			{
				case SessionThreeDialogThreeOptionOne:

					yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
					yield return RyanVoice.Say(loveLineSelection.Text);
					yield return RyanVoice.Say(GlblRes.I_want_to_love_her_of_course_I_do_but_how_can_I_when_Im_too_busy_analyzing_everything_she_does);
					yield return RyanVoice.Say(GlblRes.Everything_she_says_to_me_every_time_she_leaves_the_house_for_groceries_or_whatever_I_have_to_try_and_figure_out_if_shes_being_honest_with_me_or_not);
					yield return RyanVoice.Say(GlblRes.Its_maddening);

					yield return Psychiatrist.Say(GlblRes.Have_you_tried_talking_to_her_about_this);

					yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
					yield return RyanVoice.Say(GlblRes.No_of_course_not_Not_yet_But_I_swear_to_god_its_driving_me_insane);
					yield return RyanVoice.Say(GlblRes.Just_last_week_she_said_she_was_going_out_to_the_gym_Its_been_years_since_she_last_went_to_the_gym);
					yield return RyanVoice.Say(GlblRes.I_thought_I_could_let_it_go_but_I_couldnt);

					yield return Psychiatrist.Say(GlblRes.So_you_didnt_let_her_go);

					yield return RyanVoice.Say(GlblRes.Oh_no_I_let_her_go_She_walked_out_of_the_house_got_into_the_car_and_drove_off_down_the_street_before);

					yield return Psychiatrist.Say(GlblRes.Before_what);

					yield return RyanVoice.Say(GlblRes.Well_Im_not_proud_of_it_but_I_followed_her);
					yield return RyanVoice.Say(GlblRes.I_stayed_clear_back_so_she_wouldnt_recognize_me_and_I_followed_her_all_the_way_to_where_she_was_going);

					yield return Psychiatrist.Say(GlblRes.And);

					yield return RyanVoice.Say(GlblRes.She_went_straight_to_the_gym);

					yield return Psychiatrist.Say(GlblRes.But_you_still_dont_trust_her);

					yield return RyanVoice.Say(GlblRes.No_I_think_this_was_just_a_rare_instance_of_honesty_for_her);
					yield return RyanVoice.Say(GlblRes.And_who_knows_Maybe_she_even_knew_I_was_following_her_after_all);

					yield return Psychiatrist.Say(GlblRes.And_you_dont_think_things_will_ever_be_the_same_now);

					var loveLineSubDialog = ScoreOptions.Create()
						.Add(SessionThreeDialogFourOptionOne, GlblRes.No_Never_again, ScoreType.Insanity, 1)
						.Add(SessionThreeDialogFourOptionTwo, GlblRes.Maybe_some_day, ScoreType.Freedom, 1);

					Game.StopSkipping();

					menu.Open(loveLineSubDialog);

					yield return menu.StartSelectionScript(Get<Scripts>());
					var loveLineSubSelection = menu.LastSelectedOption;
					ProcessScore(loveLineSubSelection);

					Game.EnableSkipping();

					switch (loveLineSubSelection.ID)
					{
						case SessionThreeDialogFourOptionOne:

							yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
							yield return RyanVoice.Say(loveLineSubSelection.Text);
							yield return RyanVoice.Say(GlblRes.Any_love_I_had_for_her_is_just_getting_slowly_destroyed_by_what_she_did);
							yield return RyanVoice.TransitionTo(RyanState.Neutral);
							yield return RyanVoice.Say(GlblRes.I_just_dont_think_I_can_get_over_this);

							break;

						case SessionThreeDialogFourOptionTwo:

							yield return RyanVoice.Say(loveLineSubSelection.Text);
							yield return RyanVoice.Say(GlblRes.I_know_there_was_a_point_where_Cynthia_and_I_were_well_we_were_really_happy);
							yield return RyanVoice.Say(GlblRes.She_was_my_best_friend_and_I_would_have_trusted_her_with_my_life_We_did_everything_together_Sometimes_I_think_back_on_that);
							yield return RyanVoice.Say(GlblRes.We_had_one_day_not_long_before_Landon_was_born_when_we_were_supposed_to_be_out_getting_some_decorations_for_our_baby_shower);
							yield return RyanVoice.Say(GlblRes.but_we_just_got_so_tired_of_all_the_family_and_the_friends_and_it_was_such_a_nice_sunny_day_outside);
							yield return RyanVoice.Say(GlblRes.We_ended_up_just_pulling_off_the_highway_at_this_lake_just_me_and_her_and_lying_in_the_grass_for_hours_on_our_picnic_blanket_just_soaking_up_the_sun_together_until_it_went_down);
							yield return RyanVoice.Say(GlblRes.It_was_beautiful_I_miss_that);
							RyanVoice.TransitionTo(RyanState.Neutral);
							yield return Psychiatrist.Say(GlblRes.And_you_think_you_could_get_that_back_one_day);

							yield return Delay.Seconds(2.5f);

							yield return RyanVoice.Say(GlblRes.I_dont_know_I_really_hope_so);

							break;
					}


					break;

				case SessionThreeDialogThreeOptionTwo:

					yield return RyanVoice.TransitionTo(RyanState.ArmsRaised);
					yield return RyanVoice.Say(loveLineSelection.Text);
					yield return RyanVoice.Say(GlblRes.I_cant_just_stop_loving_her_Shes_been_my_best_friend_since_college);
					yield return RyanVoice.Say(GlblRes.We_have_a_son_together_He_he_needs_us_both);

					yield return Psychiatrist.Say(GlblRes.And_you_plan_to_be_there_for_him);

					yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
					yield return RyanVoice.Say(GlblRes.The_two_arent_exactly_mutually_exclusive_I_dont_know_I_dont_really_want_to_talk_about_him_right_now);

					yield return Psychiatrist.Say(GlblRes.Of_course_Im_sorry_Well_save_that_for_another_session);
					yield return Psychiatrist.Say(GlblRes.But_you_do_think_you_will_be_staying_with_Cynthia_as_of_now);

					yield return RyanVoice.Say(GlblRes.I_dont_think_I_have_much_of_a_choice_Besides_I_think_one_day_well_get_back_to_who_we_were);

					yield return Psychiatrist.Say(GlblRes.And_who_were_you_then);

					yield return RyanVoice.Say(GlblRes.The_kind_of_people_that_would_sneak_out_of_our_own_wedding_reception_just_to_make_out_and_get_something_greasy_to_eat);
					yield return RyanVoice.Say(GlblRes.People_who_went_camping_and_rode_bikes_together_and_watched_shitty_movies_together_just_to_make_fun_of_them);
					yield return RyanVoice.Say(GlblRes.I_liked_those_people);
					RyanVoice.TransitionTo(RyanState.Neutral);
					yield return Psychiatrist.Say(GlblRes.What_happened_to_them);

					yield return Delay.Seconds(1.5f);

					yield return RyanVoice.Say(GlblRes.Do_I_really_even_have_to_tell_you);

					break;

				case SessionThreeDialogThreeOptionThree:

					yield return RyanVoice.Say(loveLineSelection.Text);
					yield return RyanVoice.Say(GlblRes.I_cant_imagine_loving_somebody_I_dont_even_trust_Especially_not_after_what_shes_done_Its_not_possible_for_me);

					yield return Psychiatrist.Say(GlblRes.You_dont_have_any_good_memories_with_her_Something_to_convince_you_otherwise_);

					yield return RyanVoice.Say(GlblRes.Of_course_I_have_good_memories_I_married_the_woman_for_gods_sakes);
					yield return RyanVoice.Say(GlblRes.I_just_dont_think_theres_even_a_chance_well_go_back_to_the_way_things_were_Not_now);

					yield return Psychiatrist.Say(GlblRes.Have_you_told_her_you_feel_this_way);

					yield return RyanVoice.Say(GlblRes.No_Not_yet_Theres_still_too_much_at_stake_I_have_to_think_about_Landon_too_here);
					RyanVoice.TransitionTo(RyanState.Neutral);

					yield return Psychiatrist.Say(GlblRes.And_what_are_your_thoughts_on_Landon_as_of_now);

					yield return RyanVoice.Say(GlblRes.I_think_Id_prefer_we_save_that_for_another_session_Doctor_);

					break;
			}

			Tree.Basement.Scene.Visible = true;

			Tree.Office.Scene.Enabled = false;
			Tree.Office.Scene.Visible = false;

			Tree.Cutscenes.Scene.Visible = false;
			Tree.Cutscenes.Scene.Enabled = false;

			World.Interactive = true;
			Tree.GUI.Interaction.Scene.Visible = true;
			Game.Ego.Inventory.Show();

			Tree.Basement.Scene.Interactive = true;
			Tree.Actors.Scene.Enabled = true;

			Tree.GUI.Interaction.Scene.Interactive = true;

			Game.StopSkipping();
		}
	}
}
