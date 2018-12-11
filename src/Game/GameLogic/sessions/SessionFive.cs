using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Dialog;
using STACK;
using STACK.Components;
using System.Collections;
using GlblRes = global::SessionSeven.Properties.Resources_Session_5;

namespace SessionSeven.Cutscenes
{

    public partial class Director : Entity
    {
        IEnumerator SessionFiveScript()
        {
            Game.StopSong();
            Game.PlaySoundEffect(content.audio.transition_4);
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

            yield return Psychiatrist.Say(GlblRes.Now_Ryan_I_know_weve_been_avoiding_this_subject_for_a_while_but);

            Game.PlaySong(content.audio.choice);
            World.Get<AudioManager>().RepeatSong = true;

            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;

            yield return Delay.Seconds(2.25f);

            RyanEyesClosed.Visible = false;

            yield return RyanVoice.TransitionTo(Office.RyanState.HandsIntertwined);

            yield return Delay.Seconds(0.5f);

            yield return RyanVoice.Say(GlblRes.I_know_I_know_We_should_talk_about_him_We_should);

            yield return Psychiatrist.Say(GlblRes.Im_glad_you_think_so_too_We_can_start_simple_How_have_things_been_since_the_diagnosis);

            yield return RyanVoice.Say(GlblRes.Oh_I_dont_know_Odd_I_guess_I_just_dont_know_if_I_believe_it);

            yield return Psychiatrist.Say(GlblRes.Aspergers_can_present_itself_in_many_different_ways_Ryan);

            yield return RyanVoice.Say(GlblRes.Sure_I_know_it_can_its_just_youve_heard_the_way_I_describe_him_Does_it_sound_like_Aspergers_to_you);

            yield return Psychiatrist.Say(GlblRes.Well_his_antisocial_behavior_certainly_could_be_a_sign_Youve_told_me_he_has_lots_of_trouble_making_friends);

            yield return RyanVoice.Say(GlblRes.Yeah_he_does_At_least_I_think_he_does_He_never_brings_anybody_home_anyways_Do_you_think_hell_ever_get_any_better_about_that);

            yield return Psychiatrist.Say(GlblRes.Its_possible_Perhaps_the_antidepressants_will_help_with_that_as_well);
            yield return Psychiatrist.Say(GlblRes.Many_depressed_people_feel_that_socializing_just_isnt_worth_it_whether_theyre_on_the_spectrum_or_not);
            yield return Delay.Seconds(2);
            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;
            yield return Psychiatrist.Say(GlblRes.You_ought_to_know_that);

            yield return Delay.Seconds(0.75f);
            RyanEyesClosed.Visible = false;
            yield return Delay.Seconds(0.25f);

            yield return RyanVoice.Say(GlblRes.Thats_true_I_dont_know_I_just_cant_shake_the_feeling_that_Im_Im_missing_something_Something_big_about_his_behavior);
            yield return RyanVoice.Say(GlblRes.And_its_not_that_I_dont_trust_well_your_people_but_if_his_own_father_doesnt_know_him_how_can_a_psychiatrist);

            yield return Psychiatrist.Say(GlblRes.You_may_be_right_Ryan_But_for_the_time_being_the_diagnosis_should_be_a_good_thing);
            yield return Psychiatrist.Say(GlblRes.Its_your_first_step_to_getting_Landon_help_Dont_you_think_so);

            yield return RyanVoice.TransitionTo(Office.RyanState.Neutral);

            Game.StopSkipping();

            var Menu = Tree.GUI.Dialog.Menu;

            var FirstDialog = ScoreOptions.Create()
                .Add(1, GlblRes.Of_course_I_just_want_whats_best_for_my_son, ScoreType.Freedom, 1)
                .Add(2, GlblRes.I_really_dont_think_hes_sick_Not_like_that, ScoreType.Jail, 1)
                .Add(3, GlblRes.I_think_Im_afraid_of_him, ScoreType.Insanity, 1);

            Menu.Open(FirstDialog);

            yield return Menu.StartSelectionScript(Get<Scripts>());

            Game.EnableSkipping();

            var Selection = Menu.LastSelectedOption;
            ProcessScore(Selection);

            Game.PlaySong(content.audio.session5);

            yield return RyanVoice.Say(Selection.Text);

            switch (Selection.ID)
            {
                case 1:
                    yield return RyanVoice.Say(GlblRes.Ive_always_wanted_whats_best_for_Landon);
                    yield return RyanVoice.Say(GlblRes.Im_not_100_convinced_thats_changing_schools_pumping_him_with_pills_and_starting_all_over_but_if_thats_what_his_mother_and_his_psychiatrist_are_saying_I_guess_thats_for_the_best);

                    yield return Psychiatrist.Say(GlblRes.Its_good_that_you_have_his_best_interests_at_heart);
                    yield return Psychiatrist.Say(GlblRes.I_know_that_as_a_parent_with_mental_illness_it_can_be_difficult_to_handle_when_your_child_is_also_diagnosed);

                    yield return RyanVoice.TransitionTo(Office.RyanState.HandsIntertwined);

                    yield return RyanVoice.Say(GlblRes.Its_hard_not_to_feel_guilty_you_know_Like_maybe_if_Id_done_something_differently);

                    yield return Psychiatrist.Say(GlblRes.Mental_illness_is_like_any_other_chronic_disease_You_cant_have_done_anything_to_prevent_it);


                    yield return RyanVoice.Say(GlblRes.Okay_but_maybe_if_Id_been_around_more_often_or_spent_more_time_with_him);
                    yield return RyanVoice.Say(GlblRes.Im_always_off_at_work_and_his_mother_isnt_the_most_accessible_of_people);
                    yield return RyanVoice.Say(GlblRes.Maybe_if_Id_been_there_he_wouldnt_be_having_such_a_rough_time);

                    yield return Psychiatrist.Say(GlblRes.Maybe_but_that_doesnt_mean_youve_lost_everything_Ryan_You_can_still_make_time_for_him_now);

                    yield return RyanVoice.TransitionTo(Office.RyanState.Neutral);

                    yield return RyanVoice.Say(GlblRes.What_if_its_too_late_Hes_so_attached_to_his_mother_now_He_barely_speaks_to_me);
                    yield return RyanVoice.Say(GlblRes.Ive_tried_reaching_out_to_him_a_few_times_but_he_always_goes_off_and_hides_in_his_room_to_play_on_his_computer_or_whatever_it_is_he_does_up_there);
                    yield return Delay.Seconds(0.25f);
                    yield return RyanVoice.Say(GlblRes.He_isnt_interested_in_bonding);

                    yield return Psychiatrist.Say(GlblRes.Teenagers_are_difficult_mentally_ill_or_not_Youve_got_to_try_Ryan_For_your_family_You_know_how_important_it_is);

                    yield return RyanVoice.Say(GlblRes.He_wouldnt_even_tell_me_what_those_bullies_were_doing_to_him_at_school_Or_who_they_were_He_just_said_he_needed_to_get_away_from_them);
                    yield return RyanVoice.Say(GlblRes.I_still_dont_know_why_why_he_did_what_he_did);

                    yield return Psychiatrist.Say(GlblRes.To_the_bullies);

                    yield return RyanVoice.TransitionTo(Office.RyanState.HandsIntertwined);
                    yield return RyanVoice.Say(GlblRes.Yes_Like_to_purposefully_hurt_somebody_the_way_he_did_that_one_kid_the_kid_had_to_have_done_something_pretty_awful_to_him);

                    yield return Psychiatrist.Say(GlblRes.How_bad_were_his_injuries_exactly);

                    yield return RyanVoice.Say(GlblRes.Hes_just_getting_out_of_the_hospital_today_They_thought_he_might_have_needed_plastic_surgery_for_his_face);
                    yield return RyanVoice.Say(GlblRes.And_Landon_still_hasnt_said_a_thing_Just_that_he_had_it_coming_He_didnt_even_seem_sorry_about_it);

                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return Delay.Seconds(3);
                    RyanEyesClosed.Blinking = true;

                    yield return Psychiatrist.Say(GlblRes.Well_its_difficult_to_talk_about_the_people_that_have_hurt_us_especially_for_a_teenager);
                    yield return Psychiatrist.Say(GlblRes.If_you_really_want_to_connect_youll_just_need_to_try_harder_to_reach_out_Maybe_hed_like_a_trip_to_the_movies_Or_the_park);

                    yield return RyanVoice.TransitionTo(Office.RyanState.Neutral);

                    yield return RyanVoice.Say(GlblRes.Maybe_Who_knows_what_he_likes_anymore);
                    break;

                case 2:
                    yield return RyanVoice.Say(GlblRes.Landon_has_always_been_troubled_as_long_as_I_can_remember);

                    yield return Psychiatrist.Say(GlblRes.What_do_you_mean_by_troubled);

                    yield return RyanVoice.Say(GlblRes.I_mean_hes_always_been_difficult_to_reach_Even_when_he_was_a_baby);
                    yield return RyanVoice.Say(GlblRes.All_he_did_was_stare_Stare_or_cry_when_he_was_hungry_It_was_unnerving);

                    yield return Delay.Seconds(1);

                    yield return Psychiatrist.Say(GlblRes.You_mean_to_say_he_was_slow_to_show_emotion);

                    yield return RyanVoice.Say(GlblRes.Well_yes_but_it_was_more_than_that_I_mean_theres_something_from_when_he_was_younger_and_at_the_time_I_thought_maybe_I_was_imagining_it_but_now);

                    yield return Psychiatrist.Say(GlblRes.What_are_you_talking_about);

                    yield return Delay.Seconds(2);

                    yield return RyanVoice.Say(GlblRes.I_was_the_first_one_to_ever_see_him_smile);

                    yield return Psychiatrist.Say(GlblRes.Well_thats_wonderful);

                    yield return RyanVoice.TransitionTo(Office.RyanState.HandsIntertwined);

                    yield return RyanVoice.Say(GlblRes.No_no_it_wasnt_wonderful_It_was_unsettling);
                    yield return RyanVoice.Say(GlblRes.Most_babies_their_first_smile_happens_because_they_hear_their_mothers_voice_or_somebody_makes_a_silly_face_at_them);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return Delay.Seconds(1);
                    yield return RyanVoice.Say(GlblRes.Not_Landon);
                    RyanEyesClosed.Blinking = true;

                    yield return Psychiatrist.Say(GlblRes.What_was_it_for_Landon);

                    yield return RyanVoice.Say(GlblRes.Cynthia_was_just_out_getting_the_mail_when_it_happened_I_was_in_the_kitchen_getting_breakfast_ready_I_was_going_to_make_pancakes);
                    yield return RyanVoice.Say(GlblRes.Anyways_I_opened_up_one_of_the_cabinets_higher_up_trying_to_find_a_good_frying_pan);
                    yield return RyanVoice.Say(GlblRes.I_couldnt_quite_reach_the_pan_I_wanted_so_I_set_Landon_down_on_the_counter_for_just_a_minute_and_reached_up);
                    yield return RyanVoice.Say(GlblRes.The_pan_came_crashing_down_right_next_to_Landon_I_panicked_and_threw_my_hand_out);
                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.Say(GlblRes.I_hit_the_pan_out_of_the_way_in_time_but_not_before_it_broke_two_of_my_fingers);
                    yield return Delay.Seconds(1);
                    yield return RyanVoice.Say(GlblRes.It_made_the_most_awful_sound);

                    yield return Psychiatrist.Say(GlblRes.And_Landon_cried);

                    yield return RyanVoice.Say(GlblRes.No_no_thats_just_it_Most_babies_would_have_started_screaming_the_second_the_loud_sounds_started_But_not_Landon);
                    yield return RyanVoice.Say(GlblRes.He_was_making_a_funny_sound_and_fidgeting_a_little_bit_and_for_a_second_I_thought_maybe_hed_somehow_been_hurt_or_was_getting_ready_to_cry_but_no);
                    yield return RyanVoice.Say(GlblRes.He_was_giggling_It_was_the_first_time_Id_ever_seen_him_smile_or_laugh_And_he_just_kept_going);
                    yield return RyanVoice.Say(GlblRes.Im_standing_there_screaming_in_pain_and_hes_laughing_He_thought_it_was_hilarious);

                    yield return RyanVoice.TransitionTo(Office.RyanState.Neutral);

                    yield return Psychiatrist.Say(GlblRes.Maybe_you_were_making_a_funny_face);

                    yield return RyanVoice.Say(GlblRes.Thats_what_Cynthia_thought_when_I_told_her_but_I_dont_think_thats_it_I_think_he_genuinely_liked_it);

                    yield return Psychiatrist.Say(GlblRes.The_loud_noises);

                    yield return RyanVoice.Say(GlblRes.The_screaming_The_pain_Sometimes_I_wonder_if_he_still_does);
                    break;

                case 3:
                    yield return RyanVoice.Say(GlblRes.That_psychiatrist_hes_seeing_doesnt_know_Landon_like_I_do_He_hasnt_seen_what_Ive_seen_him_doing);

                    yield return Psychiatrist.Say(GlblRes.What_did_you_see);

                    yield return RyanVoice.Say(GlblRes.Its_hard_to_explain_And_Im_not_really_proud_of_how_I_found_it);

                    yield return Psychiatrist.Say(GlblRes.Found_what_Ryan);

                    yield return RyanVoice.TransitionTo(Office.RyanState.HandsIntertwined);

                    yield return RyanVoice.Say(GlblRes.I_You_have_to_understand_that_he_never_talks_to_me_And_after_everything_thats_happened_with_the_bullying_and_the_switching_schools_and_the_secrets);
                    yield return RyanVoice.Say(GlblRes.I_just_wanted_to_understand);
                    yield return RyanVoice.Say(GlblRes.See_pretty_much_every_weekend_when_he_thinks_nobody_is_looking_Landon_goes_off_into_the_belt_of_forest_behind_our_home_and_doesnt_come_back_for_a_couple_of_hours);
                    yield return RyanVoice.Say(GlblRes.I_had_no_idea_what_he_was_doing_back_there_I_thought_maybe_he_was_meeting_a_girl_or_maybe_doing_drugs_with_some_friends_I_didnt_know_about);
                    yield return RyanVoice.Say(GlblRes.I_dont_know_that_I_really_would_have_minded_if_that_was_what_he_was_doing_back_there_because_at_least_that_would_mean_hes_spending_time_with_someone);
                    yield return RyanVoice.Say(GlblRes.But_its_the_kind_of_thing_I_think_a_father_should_know_about);

                    yield return Psychiatrist.Say(GlblRes.Sure);

                    yield return RyanVoice.Say(GlblRes.So_one_Saturday_afternoon_I_decided_to_follow_after_him_I_waited_until_he_left_the_house_and_scaled_over_the_back_fence_and_then_I_went);
                    yield return RyanVoice.Say(GlblRes.He_walked_for_about_twenty_minutes_back_into_the_forest_and_then_he_stopped_at_this_little_clearing_in_the_trees);
                    yield return RyanVoice.Say(GlblRes.I_hid_behind_one_of_the_big_oaks_while_he_bent_down_over_something);
                    yield return RyanVoice.Say(GlblRes.He_wasnt_meeting_anybody_after_all_He_just_seemed_to_be_working_away_at_something_like_he_was_really_focused);
                    yield return RyanVoice.Say(GlblRes.I_felt_bad_watching_him_but_I_also_didnt_want_to_interrupt_So_I_just_watched);

                    yield return Psychiatrist.Say(GlblRes.And_then);

                    yield return RyanVoice.Say(GlblRes.After_a_while_he_seemed_to_get_bored_He_got_up_and_left_After_I_was_sure_he_wasnt_going_to_come_back_I_got_up_so_I_could_get_a_closer_look_at_his_project);
                    yield return RyanVoice.Say(GlblRes.From_far_away_it_looked_like_like_maybe_dolls_or_something__Like_he_was_playing_with_dolls_or_stuffed_animals);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.Say(GlblRes.But_then);
                    yield return Delay.Seconds(0.75f);
                    yield return RyanVoice.Say(GlblRes.then_I_got_closer_and);
                    yield return Delay.Seconds(1);
                    yield return RyanVoice.Say(GlblRes.and_I_realized);
                    yield return Delay.Seconds(1.5f);

                    yield return Psychiatrist.Say(GlblRes.They_werent_dolls);
                    RyanEyesClosed.Blinking = true;

                    yield return RyanVoice.Say(GlblRes.No_Not_dolls_Animals_Corpses_Bloody_open_bodies_of_squirrels_and_rabbits_baking_in_the_sun_I_almost_threw_up);

                    yield return Psychiatrist.Say(GlblRes.But_what_was_he_doing_with_them);

                    yield return RyanVoice.Say(GlblRes.I_couldnt_tell_you_He_had_a_bunch_of_sticks_and_sharpened_rocks_around_and_hed_pulled_the_guts_out_of_some_of_the_poor_little_creatures);
                    yield return RyanVoice.Say(GlblRes.It_was_like_he_was_examining_them_Playing_surgeon);

                    yield return Psychiatrist.Say(GlblRes.Ryan_I_cant_lie_to_you_Thats_thats_concerning_behavior_Is_he_still_doing_that);

                    yield return RyanVoice.Say(GlblRes.I_think_thats_the_most_disturbing_part_about_all_of_this_for_me_strangely_enough);

                    yield return RyanVoice.TransitionTo(Office.RyanState.Neutral);

                    yield return RyanVoice.Say(GlblRes.He_stopped_That_same_day_that_I_went_after_him_he_quit_going_out_on_the_weekends);
                    yield return RyanVoice.Say(GlblRes.Started_staying_at_home_in_his_room_like_he_does_mosts_school_nights);
                    yield return Delay.Seconds(1);
                    yield return RyanVoice.Say(GlblRes.Its_like_he_knew_hed_been_followed);

                    yield return Psychiatrist.Say(GlblRes.Perhaps_Cynthia_told_him);

                    yield return RyanVoice.Say(GlblRes.Maybe_but_I_dont_think_so_I_dont_think_she_knew_Id_gone_out);

                    yield return Psychiatrist.Say(GlblRes.But_he_never_said_anything_to_you_about_it);

                    yield return RyanVoice.Say(GlblRes.No_never);
                    yield return Delay.Seconds(0.25f);
                    yield return RyanVoice.Say(GlblRes.And_to_be_honest_with_you);
                    yield return Delay.Seconds(1);
                    yield return RyanVoice.Say(GlblRes.I_havent_been_able_to_sleep_quite_the_same_since);

                    break;
            }

            Tree.Basement.Scene.Visible = true;

            Tree.Office.Scene.Enabled = false;
            Tree.Office.Scene.Visible = false;

            Tree.Cutscenes.Scene.Visible = false;
            Tree.Cutscenes.Scene.Enabled = false;

            Tree.GUI.Interaction.Scene.Visible = true;
            Game.Ego.Inventory.Show();

            Tree.Basement.Scene.Interactive = true;
            Tree.Actors.Scene.Enabled = true;

            Tree.GUI.Interaction.Scene.Interactive = true;

            World.Interactive = true;
            Game.StopSkipping();
        }
    }
}
