using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Dialog;
using SessionSeven.Office;
using STACK;
using STACK.Components;
using System.Collections;
using GlblRes = global::SessionSeven.Properties.Resources_Session_6;

namespace SessionSeven.Cutscenes
{

    public partial class Director : Entity
    {
        public const int SessionSixDialogOptionOne = 1;
        public const int SessionSixDialogOptionTwo = 2;
        public const int SessionSixDialogOptionThree = 3;

        private int SessionSixDialogOneOption;

        public bool KilledLandon
        {
            get
            {
                return SessionSixDialogOptionOne == SessionSixDialogOneOption;
            }
        }

        IEnumerator SessionSixScript()
        {
            RemoveMouseRemains();

            Game.StopSong();
            Game.PlaySoundEffect(content.audio.transition_4);
            Tree.Office.Scene.SetupLate();
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

            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;

            yield return Delay.Seconds(1);

            RyanEyesClosed.Blinking = true;

            yield return Psychiatrist.Say(GlblRes.Are_you_ready_to_tell_me_what_happened_that_night);

            Game.PlaySong(content.audio.session6);
            World.Get<AudioManager>().RepeatSong = true;

            yield return Delay.Seconds(0.5f);
            yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);

            yield return RyanVoice.Say(GlblRes.I_I_dont_know);

            yield return Psychiatrist.Say(GlblRes.Im_only_here_to_help_you_Ryan_You_know_that_That_has_always_been_my_goal_with_you_);

            yield return RyanVoice.TransitionTo(RyanState.Neutral);

            yield return RyanVoice.Say(GlblRes.I_guess_thats_true);

            yield return Psychiatrist.Say(GlblRes.It_is_So_why_dont_you_start_with_what_you_saw_when_you_came_home);

            yield return Delay.Seconds(0.5f);
            yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
            yield return Delay.Seconds(0.5f);
            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;

            yield return Delay.Seconds(2.5f);

            RyanEyesClosed.Blinking = true;
            yield return Delay.Seconds(0.5f);
            yield return RyanVoice.Say(GlblRes.I_saw_her_Cynthia_cowering_on_the_floor_of_the_kitchen);
            yield return RyanVoice.Say(GlblRes.Her_lip_was_bleeding_and_one_of_her_eyes_was_turning_black_on_the_spot);
            yield return RyanVoice.Say(GlblRes.I_ran_straight_to_her_and_asked_her_what_happened_I_thought_wed_had_a_break_in);

            yield return Psychiatrist.Say(GlblRes.And_what_did_she_say_to_you);

            yield return RyanVoice.Say(GlblRes.At_first_she_wasnt_making_any_sense_She_kept_saying_things_like_Im_alright_Im_alright_it_wasnt_his_fault_and_I_was_asking_her_who_is_he);
            yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
            yield return RyanVoice.Say(GlblRes.Then_I_thought_maybe_she_was_cheating_on_me_again_That_shed_brought_somebody_home_and_theyd_attacked_her);

            yield return Psychiatrist.Say(GlblRes.But_thats_not_what_happened);

            yield return Delay.Seconds(0.5f);
            yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);

            yield return RyanVoice.Say(GlblRes.No_No_its_not_I_asked_her_who_who_did_this__and_she_kept_crying_and_telling_me_to_keep_my_voice_down_and_thats);

            yield return Psychiatrist.Say(GlblRes.Thats_when_you_realized_he_was_in_the_house_with_you);

            yield return Delay.Seconds(1.5f);

            yield return RyanVoice.Say(GlblRes.Yes_I_heard_him_blasting_music_from_his_room_Hed_just_left_her_to_bleed_on_the_floor);

            yield return Psychiatrist.Say(GlblRes.So_what_did_you_do_next);

            yield return RyanVoice.Say(GlblRes.I_helped_Cynthia_into_a_chair_and_got_her_some_ice_and_then_I_told_her_Id_be_right_back_She_was_scared_out_of_her_mind_I_think);
            yield return RyanVoice.Say(GlblRes.She_didnt_want_me_to_go_up_there_She_said_he_had_some_kind_of_a_weapon);
            yield return RyanVoice.Say(GlblRes.That_hed_threatened_to_kill_us_both_with_it_if_we_bothered_him_I_told_her_I_was_just_going_to_get_some_painkillers_from_our_bedroom);
            yield return RyanVoice.Say(GlblRes.I_dont_know_if_she_believed_me_Then_I_went_upstairs_His_door_was_slightly_open);
            yield return RyanVoice.Say(GlblRes.I_could_see_him_in_his_room_before_he_could_see_me);

            yield return Delay.Seconds(0.5f);

            yield return Psychiatrist.Say(GlblRes.And_what_was_he_doing);

            yield return RyanVoice.Say(GlblRes.Just_sitting_On_the_edge_of_his_bed_Perfectly_still_Just_staring_at_the_wall);
            yield return RyanVoice.Say(GlblRes.Youd_never_have_known_what_hed_just_done_To_his_own_mother);
            yield return RyanVoice.Say(GlblRes.There_was_something_in_his_lap_but_I_couldnt_quite_see_it_I_figured_that_was_his_weapon);
            yield return RyanVoice.Say(GlblRes.I_had_to_do_something_right_then_and_there_He_was_putting_us_all_in_danger);

            yield return Psychiatrist.Say(GlblRes.Did_you_call_the_police);

            yield return RyanVoice.Say(GlblRes.I_would_have_had_to_leave_the_house_to_do_it_and_I_didnt_want_to_leave_Cynthia_alone_I_didnt_know_what_he_was_going_to_do_next);

            yield return Psychiatrist.Say(GlblRes.So_what_did_you_do);

            Game.StopSkipping();

            var Menu = Tree.GUI.Dialog.Menu;

            var SecondDialog = ScoreOptions.Create()
                .Add(SessionSixDialogOptionOne, GlblRes.I_jumped_him_from_behind, ScoreType.Jail, 1)
                .Add(SessionSixDialogOptionTwo, GlblRes.I_tried_to_talk_him_down_so_that_we_could_get_him_help_, ScoreType.Freedom, 5)
                .Add(SessionSixDialogOptionThree, GlblRes.I_went_back_to_my_room_and_started_packing_for_Cynthia_and_me_to_get_away_, ScoreType.Insanity, 5);

            Menu.Open(SecondDialog);

            yield return Menu.StartSelectionScript(Get<Scripts>());

            Game.EnableSkipping();

            var FirstSelection = Menu.LastSelectedOption;
            // store the choice for later
            SessionSixDialogOneOption = FirstSelection.ID;

            ProcessScore(FirstSelection);

            yield return RyanVoice.Say(FirstSelection.Text);

            switch (FirstSelection.ID)
            {
                case SessionSixDialogOptionOne:
                    yield return RyanVoice.Say(GlblRes.I_didnt_really_have_a_choice_I_just_knew_I_needed_to_stop_him);

                    yield return Delay.Seconds(0.5f);

                    yield return Psychiatrist.Say(GlblRes.So_you_attacked_him);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.You_have_to_understand_me_I_didnt_want_to_hurt_him_I_just_needed_him_to_drop_whatever_he_had);
                    yield return RyanVoice.Say(GlblRes.I_thought_maybe_once_he_saw_that_he_wasnt_going_to_be_able_to_hurt_us_hed_sit_down_and_listen_to_reason);
                    yield return RyanVoice.Say(GlblRes.Then_we_could_put_him_in_a_hospital_or_something_for_a_while_I_didnt_think_I_never_thought_that);

                    yield return Psychiatrist.Say(GlblRes.Tell_me_how_you_did_it);

                    yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                    yield return RyanVoice.Say(GlblRes.Dont_make_it_sound_like_I_knew_what_I_was_doing_I_didnt_I_really_didnt);
                    yield return RyanVoice.Say(GlblRes.I_just_got_so_so_angry_all_of_a_sudden_I_came_up_behind_him_and_I_hit_him_hard_across_the_back_of_the_head);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.It_surprised_him_He_fell_forward_and_I_used_the_opportunity_to_grab_what_he_had_in_his_lap);
                    yield return RyanVoice.Say(GlblRes.It_was_just_a_wrench_It_wasnt_even_that_heavy);

                    yield return Psychiatrist.Say(GlblRes.What_happened_after_he_fell);

                    yield return Delay.Seconds(1.0f);
                    yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);

                    yield return RyanVoice.Say(GlblRes.I_tried_to_hold_his_head_down_to_the_ground_but_he_was_stronger_than_I_thought);
                    yield return RyanVoice.Say(GlblRes.He_got_right_back_up_and_turned_around_and_I_realized_in_that_moment_that_whoever_I_was_looking_at_wasnt_my_son);
                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.Say(GlblRes.Not_really_It_was_like_a_monster_had_taken_over);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return Delay.Seconds(1.0f);
                    yield return RyanVoice.Say(GlblRes.There_wasnt_a_trace_of_humanity_left_in_him);
                    RyanEyesClosed.Blinking = true;

                    yield return Psychiatrist.Say(GlblRes.Did_he_hurt_you_too);

                    yield return RyanVoice.Say(GlblRes.Yes);
                    yield return RyanVoice.Say(GlblRes.It_only_took_him_a_second_to_realize_what_had_happened_and_who_had_hit_him_and_then_he_was_right_back_up_and_punching_me_as_hard_as_he_could_in_the_face_without_a_word);
                    yield return RyanVoice.Say(GlblRes.He_knocked_the_air_straight_out_of_me_Put_me_flat_on_my_back);

                    yield return Delay.Seconds(1.0f);
                    yield return Psychiatrist.Say(GlblRes.And_did_he_say_anything_to_you_after_that);

                    yield return RyanVoice.Say(GlblRes.He_told_me_I_really_ought_to_mind_my_own_business);
                    yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                    yield return RyanVoice.Say(GlblRes.I_was_enraged);
                    yield return RyanVoice.Say(GlblRes.I_yelled_at_him_to_stop_but_he_didnt_he_just_kept_hitting_me_hitting_me_so_hard_I_could_barely_see_and_my_nose_was_bleeding_and_I_called_out_for_Cynthia_but_I_dont_think_she_could_hear_us);
                    yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);

                    yield return Psychiatrist.Say(GlblRes.And_then);

                    yield return Delay.Seconds(1.0f);

                    yield return RyanVoice.Say(GlblRes.And_then_It_was_like_my_body_acted_on_its_own_I_didnt_even_think_about_it);
                    yield return RyanVoice.Say(GlblRes.I_think_it_was_self_defense_and_I_know_I_was_crying_and_I_just_swung_the_damn_wrench_up_to_where_I_thought_his_face_might_be_above_mine_and_and);
                    yield return Delay.Seconds(3.0f);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return RyanVoice.Say(GlblRes.Oh_god_the_sound_was_awful);
                    yield return Delay.Seconds(3.0f);
                    RyanEyesClosed.Blinking = true;

                    yield return Psychiatrist.Say(GlblRes.And_then_he_fell);

                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.Say(GlblRes.Yes_but_not_right_away_It_took_him_a_second_Like_he_was_thinking_about_it);
                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.Say(GlblRes.He_got_all_quiet_and_I_looked_him_in_the_eyes_and_he_just_looked_surprised);
                    yield return Delay.Seconds(1.0f);
                    yield return RyanVoice.Say(GlblRes.Not_even_sad_or_upset_Just_surprised);
                    yield return Delay.Seconds(2.0f);
                    yield return RyanVoice.Say(GlblRes.And_then_he_fell_on_top_of_me_like_a_rag_doll_and_I_just_knew);
                    yield return Delay.Seconds(3.0f);
                    yield return RyanVoice.Say(GlblRes.I_knew_he_was_gone);
                    yield return Delay.Seconds(2.0f);

                    yield return Psychiatrist.Say(GlblRes.But_if_you_killed_him_then_how_did_you);

                    yield return RyanVoice.Say(GlblRes.It_was_Cynthia);
                    yield return RyanVoice.Say(GlblRes.I_was_trying_to_get_up_and_I_turned_around_and_she_was_there_holding_one_of_the_heavy_ceramic_vases_her_mother_gave_to_us);
                    yield return RyanVoice.Say(GlblRes.I_thought_she_was_going_to_faint);
                    yield return RyanVoice.Say(GlblRes.I_got_up_to_help_her_to_run_over_to_her_but_then_she_lifted_the_vase_up_above_her_head);
                    yield return RyanVoice.Say(GlblRes.And_thats_the_last_thing_I_remember);

                    yield return Psychiatrist.Say(GlblRes.Thats_all_you_remember_It_was_Cynthia);

                    yield return RyanVoice.Say(GlblRes.I_didnt_believe_it_myself_I_couldnt_have);
                    yield return RyanVoice.Say(GlblRes.It_took_me_ages_of_searching_around_in_that_basement_to_remember_even_that_far);
                    yield return RyanVoice.Say(GlblRes.When_I_woke_up_in_that_basement_I_thought_I_thought_maybe_somebody_had_come_in_or_I_had_fallen_down);
                    yield return RyanVoice.Say(GlblRes.I_thought_my_family_was_in_danger);
                    yield return Delay.Seconds(1.0f);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return RyanVoice.Say(GlblRes.But_I_was_wrong);
                    yield return Delay.Seconds(1.0f);
                    yield return RyanVoice.Say(GlblRes.It_was_them);
                    yield return Delay.Seconds(1.0f);
                    yield return RyanVoice.Say(GlblRes.They_hurt_me);
                    yield return Delay.Seconds(1.0f);
                    yield return RyanVoice.Say(GlblRes.My_own_family);
                    yield return Delay.Seconds(1.0f);
                    yield return RyanVoice.Say(GlblRes.My_own_wife_imprisoned_me);
                    yield return Delay.Seconds(3.5f);
                    yield return RyanVoice.Say(GlblRes.She_left_me_to_die);
                    yield return Delay.Seconds(2.5f);
                    RyanEyesClosed.Blinking = true;

                    yield return Psychiatrist.Say(GlblRes.You_killed_her_only_son_Ryan_She_was_terrified);

                    yield return RyanVoice.Say(GlblRes.Yeah_well_well_see_what_the_jury_thinks_about_that);
                    yield return RyanVoice.Say(GlblRes.I_was_only_trying_to_protect_her_I_was_trying_to_protect_us);

                    yield return Psychiatrist.Say(GlblRes.From_the_son_you_raised);

                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.Yeah_I_mean_I_guess_thats_the_truly_scary_part_about_all_this_isnt_it);
                    yield return RyanVoice.Say(GlblRes.He_was_our_son_He_slept_in_our_house_We_took_care_of_him_when_he_was_sick);
                    yield return RyanVoice.Say(GlblRes.We_gave_up_our_lives_for_him_I_switched_jobs);
                    yield return RyanVoice.Say(GlblRes.His_mother_tucked_him_in_at_night_for_years);

                    yield return Psychiatrist.Say(GlblRes.Psychopathy_is_very_difficult_to_understand);

                    yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                    yield return RyanVoice.Say(GlblRes.I_dont_think_I_want_to_understand_I_dont_want_to_know_what_was_happening_in_his_head);
                    yield return RyanVoice.Say(GlblRes.I_dont_want_to_know_what_he_was_thinking_when_he_hit_herOr_when_he_attacked_me);
                    yield return RyanVoice.Say(GlblRes.Or_when_I_when_we_looked_at_each_other);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);

                    yield return Psychiatrist.Say(GlblRes.Perhaps_we_should_end_here_We_can_always_pick_up_later);
                    yield return Delay.Seconds(2.5f);

                    yield return RyanVoice.Say(GlblRes.I_just_need_a_moment);
                    break;

                case SessionSixDialogOptionTwo:
                    yield return RyanVoice.Say(GlblRes.I_couldnt_just_abandon_him_Hes_my_son);
                    yield return RyanVoice.Say(GlblRes.I_thought_maybe_if_I_could_just_talk_to_him_father_to_son_he_would_listen);
                    yield return RyanVoice.Say(GlblRes.Then_we_could_get_him_the_help_he_needed);

                    yield return Delay.Seconds(0.5f);

                    yield return Psychiatrist.Say(GlblRes.And_how_did_that_go);

                    yield return Delay.Seconds(1.0f);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.Say(GlblRes.Not_how_it_should_have);
                    RyanEyesClosed.Blinking = true;
                    yield return RyanVoice.Say(GlblRes.I_knew_we_hadnt_been_talking_much_lately_but_I_really_thought_he_would_care_about_what_his_own_father_had_to_say);
                    yield return RyanVoice.Say(GlblRes.I_knocked_on_the_doorframe_so_that_I_wouldnt_surprise_him_and_the_way_he_looked_around_at_me_I_almost_felt_scared);
                    yield return RyanVoice.Say(GlblRes.He_didnt_look_like_he_regretted_anything_at_all_He_just_looked_empty);
                    yield return RyanVoice.Say(GlblRes.Empty_and_maybe_a_little_annoyed_that_I_was_there);
                    yield return RyanVoice.Say(GlblRes.It_was_like_all_of_a_sudden_I_realized_that_this_person_Id_been_looking_at_all_his_life_wasnt_who_I_thought_he_was);

                    yield return Psychiatrist.Say(GlblRes.But_who_was_he);

                    yield return Delay.Seconds(0.5f);

                    yield return RyanVoice.Say(GlblRes.I_dont_know_I_just_dont_know_Somebody_soulless);
                    yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                    yield return RyanVoice.Say(GlblRes.He_asked_me_what_I_was_doing_I_told_him_I_just_wanted_to_talk);
                    yield return RyanVoice.Say(GlblRes.He_didnt_move_or_say_anything_so_I_just_went_ahead_and_sat_down_next_to_him_on_the_bed);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return RyanVoice.Say(GlblRes.I_swear_I_could_feel_the_irritated_energy_coming_off_of_him_when_I_did_It_was_cold);
                    RyanEyesClosed.Blinking = true;
                    yield return RyanVoice.Say(GlblRes.I_asked_him_what_happened_I_asked_him_if_he_knew_what_state_his_mother_was_in_downstairs);

                    yield return Psychiatrist.Say(GlblRes.Did_he_admit_to_hurting_her);

                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.No_not_really_But_he_didnt_deny_it_either);
                    yield return RyanVoice.Say(GlblRes.He_just_said_he_was_sure_shed_be_alright_Honestly_it_broke_my_heart);
                    yield return RyanVoice.Say(GlblRes.To_see_my_son_do_that_to_his_own_mother_and_feel_nothing_It_was_painful);
                    yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
                    yield return RyanVoice.Say(GlblRes.I_couldnt_help_it_I_started_cryingI_couldnt_understand_what_was_happening_I_just_wanted_it_to_be_a_nightmare);
                    yield return RyanVoice.Say(GlblRes.I_wanted_to_wake_up_I_wanted_Landon_to_snap_out_of_it);
                    yield return RyanVoice.Say(GlblRes.I_told_him_you_cant_really_mean_that_and_the_look_on_his_face_when_he_heard_my_voice_breaking);
                    yield return RyanVoice.Say(GlblRes.I_knew_immediately_that_I_wasnt_safe);
                    yield return Delay.Seconds(1.5f);
                    yield return RyanVoice.Say(GlblRes.He_pulled_the_wrench_out_from_under_his_leg_before_I_could_do_anything);

                    yield return Psychiatrist.Say(GlblRes.A_wrench);

                    yield return RyanVoice.Say(GlblRes.The_weapon_Cynthia_had_mentioned);
                    yield return RyanVoice.Say(GlblRes.The_last_thing_I_remember_was_him_raising_it_over_his_head_and_it_swinging_towards_me);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return RyanVoice.Say(GlblRes.The_next_thing_I_knew_I_woke_up_in_the_basement);
                    RyanEyesClosed.Blinking = true;

                    yield return Psychiatrist.Say(GlblRes.And_thats_all_you_remember_It_was_Landon_all_along);

                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.I_didnt_want_to_believe_it);
                    yield return RyanVoice.Say(GlblRes.It_took_me_ages_of_wandering_around_in_that_basement_to_remember_it_for_myself_completely);
                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.Say(GlblRes.All_that_time_I_thought_my_family_was_in_danger);
                    yield return Delay.Seconds(1.5f);
                    yield return RyanVoice.Say(GlblRes.I_thought_maybe_theyd_been_taken_away_and_thats_why_they_werent_helping_me);
                    yield return Delay.Seconds(2.5f);
                    yield return RyanVoice.Say(GlblRes.And_all_that_time_Id_just_been_abandoned);
                    yield return Delay.Seconds(1.5f);

                    yield return Psychiatrist.Say(GlblRes.Cynthia_too);

                    yield return RyanVoice.Say(GlblRes.She_must_have_left_with_him_after_he_threw_me_in_the_basement);
                    yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                    yield return RyanVoice.Say(GlblRes.Shes_always_been_so_attached_to_him_Hes_her_little_boy_She_obsesses_over_him);
                    yield return RyanVoice.Say(GlblRes.She_was_willing_to_move_for_him_She_didnt_care_how_it_made_me_feel_);
                    yield return RyanVoice.Say(GlblRes.Wed_been_growing_apart_and_then_to_have_her_baby_do_something_like_that_Running_off_with_him_only_made_sense_to_her);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return Delay.Seconds(1.5f);
                    yield return RyanVoice.Say(GlblRes.She_probably_wanted_to_protect_him);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return Delay.Seconds(1.5f);
                    yield return RyanVoice.Say(GlblRes.Ugh_I);
                    RyanEyesClosed.Blinking = true;

                    yield return Psychiatrist.Say(GlblRes.Are_you_alright_Ryan);

                    yield return Delay.Seconds(2.5f);

                    yield return RyanVoice.Say(GlblRes.Its_just_sickening);

                    yield return Psychiatrist.Say(GlblRes.How_about_we_take_a_short_break_You_can_catch_your_breath);
                    break;

                case SessionSixDialogOptionThree:
                    yield return RyanVoice.Say(GlblRes.It_didnt_feel_like_I_had_a_choice_I_didnt_want_anybody_to_get_hurt_any_more);
                    yield return RyanVoice.Say(GlblRes.Especially_not_my_family_I_thought_we_could_just_pack_up_and_escape);
                    yield return RyanVoice.Say(GlblRes.Maybe_go_stay_in_a_hotel_for_the_night);
                    yield return RyanVoice.Say(GlblRes.I_thought_once_we_were_safely_out_of_there_we_could_call_the_police_to_confront_Landon);

                    yield return Delay.Seconds(0.5f);

                    yield return Psychiatrist.Say(GlblRes.You_didnt_think_you_should_confront_him_yourself);

                    yield return RyanVoice.Say(GlblRes.No_we_were_past_that_He_was_violent_I_felt_like_I_hardly_even_knew_him_anymore);
                    yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                    yield return RyanVoice.Say(GlblRes.Son_or_not_hed_become_a_stranger_I_didnt_know_what_I_could_have_said_or_done);
                    yield return RyanVoice.Say(GlblRes.I_just_kept_thinking_about_Cynthia_too_and_the_way_shed_looked_so_pathetic_and_hurt);
                    yield return RyanVoice.Say(GlblRes.The_way_she_kept_making_excuses_for_him_I_knew_the_only_way_to_save_her_would_be_to_get_her_far_away_from_him);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.So_I_went_back_to_our_room_and_started_packing_for_both_of_us);

                    yield return Psychiatrist.Say(GlblRes.You_didnt_consult_her_first);

                    yield return Delay.Seconds(0.25f);
                    yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
                    yield return Delay.Seconds(0.25f);
                    yield return RyanVoice.Say(GlblRes.She_never_would_have_agreed_I_knew_she_wouldnt_have_agreed);
                    yield return RyanVoice.Say(GlblRes.So_I_just_brought_the_bag_to_her_downstairs_and_told_her_we_were_leaving_She_was_so_beaten);
                    yield return RyanVoice.Say(GlblRes.I_thought_she_would_leave_with_me_right_away_That_she_would_finally_see_sense);
                    yield return RyanVoice.Say(GlblRes.But_she_was_absolutely_brokenI_found_her_still_sitting_on_the_floor_trying_to_act_like_nothing_was_wrong);
                    yield return RyanVoice.Say(GlblRes.I_grabbed_her_arm_and_told_her_come_on_were_leaving_but_she_just_kept_shaking_her_head);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return Delay.Seconds(1.5f);
                    yield return Psychiatrist.Say(GlblRes.So_you_tried_to_force_her);

                    yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                    yield return RyanVoice.Say(GlblRes.I_didnt_know_what_else_I_could_do);
                    yield return RyanVoice.Say(GlblRes.She_wasnt_listening_to_common_sense);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.There_was_no_telling_how_much_time_we_had_before_Landon_realized_something_was_happening);
                    yield return RyanVoice.Say(GlblRes.I_ended_up_trying_to_drag_her_out_with_me_but_she_wouldnt_cooperate_She_started_screaming_and_yelling_telling_me_she_couldnt_leave_her_baby);
                    yield return RyanVoice.Say(GlblRes.I_tried_to_tell_her_that_didnt_make_any_sense_but_she_just_wouldnt_listen_She_was_sobbing);
                    yield return RyanVoice.Say(GlblRes.Inconsolable_I_was_trying_to_get_her_to_stop_telling_her_she_needed_to_leave_that_Landon_would_hear_her_but);

                    yield return Psychiatrist.Say(GlblRes.But_she_wouldnt);

                    yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
                    yield return RyanVoice.Say(GlblRes.And_Landon_must_have_heard);
                    yield return RyanVoice.Say(GlblRes.Next_thing_I_knew_Cynthia_was_looking_up_at_him_and_he_was_standing_behind_me_watching_us_both);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return RyanVoice.Say(GlblRes.He_was_holding_a_wrench_He_didnt_even_look_at_me_He_just_told_Cynthia_to_come_to_him);
                    RyanEyesClosed.Blinking = true;
                    yield return RyanVoice.Say(GlblRes.And_she_she_did_She_shook_herself_out_of_my_grip_and_went_to_him_after_only_about_a_minute);
                    yield return Delay.Seconds(1f);
                    yield return RyanVoice.Say(GlblRes.She_kept_muttering_about_how_he_was_her_baby_and_she_was_sorry);
                    yield return Delay.Seconds(1.5f);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return Delay.Seconds(1f);
                    yield return RyanVoice.Say(GlblRes.The_last_thing_I_remember_is_her_screaming_while_he_swung_the_wrench_at_my_head);
                    RyanEyesClosed.Blinking = true;
                    yield return Delay.Seconds(1f);

                    yield return Psychiatrist.Say(GlblRes.And_thats_it_It_was_both_of_them_working_together);
                    yield return Delay.Seconds(1f);

                    yield return RyanVoice.Say(GlblRes.I_didnt_want_to_believe_it);
                    yield return Delay.Seconds(2f);
                    yield return RyanVoice.Say(GlblRes.My_own_family_And_all_this_time_I_thought_maybe_they_were_hurt_But_theyd_just_run_away);
                    yield return Delay.Seconds(1f);
                    RyanEyesClosed.Blinking = false;
                    RyanEyesClosed.Visible = true;
                    yield return Delay.Seconds(1f);
                    yield return RyanVoice.Say(GlblRes.My_wife_My_son);
                    RyanEyesClosed.Blinking = true;

                    yield return Psychiatrist.Say(GlblRes.Are_you_alright_Ryan);

                    yield return Delay.Seconds(2.5f);

                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.I_just_need_a_moment);
                    break;
            }

            Game.StopSkipping();

            Tree.Actors.Scene.Enabled = true;

            var FinalScore = GetFinalScore();
            var EgoPositionInBasement = Game.Ego.Get<Transform>().Position;
            var EgoOrientationInBasement = Game.Ego.Get<Transform>().Orientation;

            Game.Ego.Turn(Directions4.Down);

            switch (FinalScore)
            {
                case ScoreType.Freedom:
                    Game.Ego.EnterScene(Tree.SunSet.Scene);
                    Game.Ego.Get<Transform>().Position = new Microsoft.Xna.Framework.Vector2(403, 344);
                    break;
                case ScoreType.Insanity:
                    Game.Ego.EnterScene(Tree.PaddedCell.Scene);
                    Game.Ego.Get<Transform>().Position = new Microsoft.Xna.Framework.Vector2(268, 329);
                    break;
                default:
                    Game.Ego.EnterScene(Tree.JailCell.Scene);
                    Game.Ego.Get<Transform>().Position = new Microsoft.Xna.Framework.Vector2(344, 354);
                    break;
            }

            yield return Delay.Seconds(2);
            Game.Ego.Turn(Directions4.Left);
            yield return Delay.Seconds(1);
            Game.Ego.Turn(Directions4.Up);
            yield return Delay.Seconds(1);

            Game.Ego.EnterScene(Tree.Basement.Scene);
            Game.Ego.Get<Transform>().SetPosition(EgoPositionInBasement);
            Game.Ego.Get<Transform>().SetOrientation(EgoOrientationInBasement);

            Tree.Basement.Scene.Visible = true;

            Tree.Office.Scene.Enabled = false;
            Tree.Office.Scene.Visible = false;

            Tree.Cutscenes.Scene.Visible = false;
            Tree.Cutscenes.Scene.Enabled = false;

            Tree.GUI.Interaction.Scene.Visible = true;
            Game.Ego.Inventory.Show();

            Tree.Basement.Scene.Interactive = true;

            Tree.GUI.Interaction.Scene.Interactive = true;

            World.Interactive = true;
            Game.StopSkipping();
        }

        private void RemoveMouseRemains()
        {
            Tree.Basement.NutsOnFloor.RemoveAllNuts();
        }
    }
}
