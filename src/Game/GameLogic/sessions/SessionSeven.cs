using Microsoft.Xna.Framework.Media;
using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.Office;
using STACK;
using STACK.Components;
using System.Collections;
using System.Collections.Generic;
using GlblRes = global::SessionSeven.Properties.Resources_Session_7;

namespace SessionSeven.Cutscenes
{

    public partial class Director : Entity
    {
        /// <summary>
        /// The last and final session
        /// </summary>
        /// <returns></returns>
        IEnumerator SessionSevenScript()
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

            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;

            yield return Delay.Seconds(1);

            RyanEyesClosed.Blinking = true;

            var FinalScore = GetFinalScore();

            switch (FinalScore)
            {
                case ScoreType.Jail:

                    Game.PlaySong(content.audio.session7a);
                    World.Get<AudioManager>().RepeatSong = true;

                    yield return Psychiatrist.Say(GlblRes.Weve_gotten_a_lot_of_good_work_done_today_Ryan);
                    yield return Delay.Seconds(0.25f);
                    yield return Psychiatrist.Say(GlblRes.I_appreciate_you_opening_up_to_me_as_much_as_you_have_I_understand_this_hasnt_been_easy_for_you);

                    yield return RyanVoice.Say(GlblRes.Yeah_well_it_was_good_just_to_see_a_familiar_face_Its_easier_to_talk_to_somebody_who_already_knows_my_history);

                    yield return Psychiatrist.Say(GlblRes.Of_course_Still_I_have_to_admit_I_was_surprised_to_get_this_call);
                    yield return Psychiatrist.Say(GlblRes.Im_sorry_its_come_to_this);

                    yield return RyanVoice.Say(GlblRes.The_trial_hasnt_happened_yet_I_still_have_a_chance);
                    yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
                    yield return Delay.Seconds(0.5f);
                    yield return RyanVoice.Say(GlblRes.I_assume_thats_why_youre_here_right);

                    yield return Psychiatrist.Say(GlblRes.Well_Id_like_to_say_Im_here_simply_to_support_you_but_yes);

                    if (KilledLandon)
                    {
                        yield return Psychiatrist.Say(GlblRes.Ryan_I_must_admit_to_you_things_arent_looking_good_for_you);
                        RyanEyesClosed.Blinking = false;
                        RyanEyesClosed.Visible = true;
                        yield return Delay.Seconds(1);
                        yield return Psychiatrist.Say(GlblRes.Your_son_is_dead_by_your_hands);
                        RyanEyesClosed.Visible = false;
                        yield return Psychiatrist.Say(GlblRes.You_may_only_have_been_trying_to_help_your_family_but_this_is_going_to_be_difficult_to_explain_away);
                        yield return Psychiatrist.Say(GlblRes.Still_its_important_that_you_came_clean_Ill_do_everything_I_can_to_help_you_with_my_testimony);
                        yield return Psychiatrist.Say(GlblRes.I_just_I_hope_it_will_be_enough);
                    }
                    else
                    {
                        yield return Psychiatrist.Say(GlblRes.I_have_hope_for_you_still_Ryan_I_really_do);
                        yield return Psychiatrist.Say(GlblRes.It_is_important_that_you_came_clean_I_think_with_this_testimony_we_can_prove_that_this_was_a_big_misunderstanding);
                        yield return Psychiatrist.Say(GlblRes.Just_be_as_honest_with_the_jury_as_you_were_with_me_just_now_and_everything_should_be_fine);
                        yield return Psychiatrist.Say(GlblRes.I_believe_this_will_be_alright_in_the_end);
                        yield return Psychiatrist.Say(GlblRes.Youre_a_good_man_You_can_come_back_from_this);
                        yield return Psychiatrist.Say(GlblRes.Its_Landon_who_has_to_face_the_real_consequences_from_this_Theyll_be_putting_him_away_any_day_now);
                    }

                    yield return Delay.Seconds(1);

                    yield return RyanVoice.Say(GlblRes.And_and_Cynthia);

                    yield return Delay.Seconds(0.5f);

                    yield return Psychiatrist.Say(GlblRes.Shes_in_just_about_the_same_boat_as_you);
                    yield return Psychiatrist.Say(GlblRes.Theres_a_chance_shell_be_released_because_she_was_technically_a_victim_of_domestic_abuse_but_yes_shell_also_have_to_stand_trial);

                    yield return RyanVoice.Say(GlblRes.I_see_Do_you_think_do_you_think_Ill_have_to_talk_to_her);

                    yield return Get<Scripts>().Start(CynthiaDialogScript());

                    yield return Psychiatrist.Say(GlblRes.Thats_all_for_today_Ryan_Your_handler_will_take_you_back_to_your_room);
                    yield return Psychiatrist.Say(GlblRes.Youve_been_through_a_lot_I_just_hope_we_can_work_through_it_all_and_get_you_feeling_better_soon);
                    yield return Psychiatrist.Say(GlblRes.Ill_see_you_tomorrow);

                    break;

                case ScoreType.Insanity:

                    Game.PlaySong(content.audio.session7b);
                    World.Get<AudioManager>().RepeatSong = true;

                    yield return Psychiatrist.Say(GlblRes.Alright_Ryan_I_think_we_got_a_lot_of_important_work_done_today);
                    yield return Delay.Seconds(0.25f);
                    yield return Psychiatrist.Say(GlblRes.Im_glad_youve_opened_up_to_me_so_much_about_what_happened);

                    yield return RyanVoice.Say(GlblRes.It_was_good_to_see_a_familiar_face_The_person_they_normally_send_me_to);

                    yield return Psychiatrist.Say(GlblRes.Yes_Ive_heard_Dr_Raj_can_be_a_bit_harsh);

                    yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
                    yield return RyanVoice.Say(GlblRes.He_doesnt_know_me_like_you_do_either_Its_hard_enough_to_tell_this_story_once_nevermind_a_second_time);

                    yield return Psychiatrist.Say(GlblRes.Of_course_I_think_youll_be_glad_to_hear_Im_taking_over_for_him_now);
                    yield return Psychiatrist.Say(GlblRes.Well_be_seeing_a_lot_more_of_each_other_Ryan);

                    yield return RyanVoice.Say(GlblRes.A_lot_more_huh_Does_that_mean_do_you_think_Ill_ever_get_out_of_here_Doctor);

                    if (KilledLandon)
                    {
                        yield return Psychiatrist.Say(GlblRes.Well_Ryan_I_have_to_be_honest_with_you_it_doesnt_look_good);
                        RyanEyesClosed.Blinking = false;
                        RyanEyesClosed.Visible = true;
                        yield return Delay.Seconds(1);
                        yield return Psychiatrist.Say(GlblRes.Your_son_is_dead_by_your_hands);
                        RyanEyesClosed.Visible = false;
                        yield return Psychiatrist.Say(GlblRes.Selfdefense_or_not_its_possible_youll_always_be_seen_as_a_threat_to_society);
                        yield return Psychiatrist.Say(GlblRes.Especially_seeing_the_traumatized_admittedly_manic_state_you_were_in_when_you_first_arrived);
                        yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                        yield return Psychiatrist.Say(GlblRes.Its_going_to_be_a_long_journey_for_you_I_can_help_you_make_the_best_of_it_but_as_for_your_freedom);
                        yield return Psychiatrist.Say(GlblRes.I_think_all_we_can_do_is_hope_for_the_best);
                        yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
                    }
                    else
                    {
                        yield return Psychiatrist.Say(GlblRes.Oh_I_think_so_These_places_arent_meant_for_permanent_residents_except_for_some_extreme_cases);
                        yield return Psychiatrist.Say(GlblRes.Its_not_as_if_youve_really_hurt_anyone);
                        yield return Psychiatrist.Say(GlblRes.But_seeing_how_traumatized_you_were_after_all_of_this_and_for_me_personally_knowing_your_very_unhealthy_relationships_with_Landon_and_Cynthia);
                        yield return Psychiatrist.Say(GlblRes.its_probably_for_the_best_that_you_get_this_time_to_really_heal_and_reflect_on_yourself);
                        yield return Psychiatrist.Say(GlblRes.Besides_itll_be_a_while_before_you_can_reach_out_to_Landon_at_all_considering_hes_in_a_separate_hospital);
                        yield return Psychiatrist.Say(GlblRes.He_wont_be_allowed_visitors_for_quite_some_time);
                    }

                    yield return Delay.Seconds(1);

                    yield return RyanVoice.Say(GlblRes.And_what_do_you_think_will_happen_to_Cynthia);

                    yield return Delay.Seconds(0.5f);

                    yield return Psychiatrist.Say(GlblRes.Well_for_now_shes_awaiting_trial);
                    yield return Psychiatrist.Say(GlblRes.Then_its_likely_shell_plead_that_she_was_forced_You_know_that_she_was_fearing_for_her_life_and_thats_why_she_helped_Landon_over_you);
                    yield return Psychiatrist.Say(GlblRes.And_perhaps_thats_true);

                    yield return RyanVoice.Say(GlblRes.Do_you_think_theyll_make_me_speak_to_her_in_court);

                    yield return Get<Scripts>().Start(CynthiaDialogScript());

                    yield return Psychiatrist.Say(GlblRes.Thats_all_for_today_Ryan_Your_handler_will_take_you_back_to_your_room);
                    yield return Psychiatrist.Say(GlblRes.Youve_been_through_a_lot_I_just_hope_we_can_work_through_it_all_and_get_you_feeling_better_soon);
                    yield return Psychiatrist.Say(GlblRes.Ill_see_you_tomorrow);

                    break;

                case ScoreType.Freedom:

                    Game.PlaySong(content.audio.session7c);
                    World.Get<AudioManager>().RepeatSong = true;

                    yield return RyanVoice.TransitionTo(RyanState.ArmsRaised);

                    yield return Psychiatrist.Say(GlblRes.Well_Id_say_we_got_a_lot_done_today_Ryan);
                    yield return Delay.Seconds(0.25f);
                    yield return Psychiatrist.Say(GlblRes.I_want_to_thank_you_again_for_opening_up_to_me);

                    yield return RyanVoice.Say(GlblRes.Ive_been_seeing_you_for_nearly_a_year_at_this_point_It_wouldnt_make_sense_to_stop_now);

                    yield return Psychiatrist.Say(GlblRes.No_especially_not_now_Its_very_important_that_youre_talking_to_someone_as_you_go_through_this_process);

                    yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
                    yield return RyanVoice.Say(GlblRes.But_you_do_think_Ill_come_out_okay);

                    yield return Psychiatrist.Say(GlblRes.Of_course_I_do_Ive_seen_you_go_through_quite_a_lot_but_Ive_also_seen_you_pull_through_it_with_grace);
                    yield return Psychiatrist.Say(GlblRes.If_anything_this_is_the_end_of_a_very_difficult_chapter_of_your_life_Things_can_only_get_better_for_you_now);

                    yield return Delay.Seconds(1);

                    yield return RyanVoice.Say(GlblRes.Well_its_almost_the_end_What_about_Cynthia);

                    yield return Delay.Seconds(0.5f);

                    yield return Psychiatrist.Say(GlblRes.You_mean_her_trial_Yes_theres_no_telling_how_that_will_go);
                    yield return Psychiatrist.Say(GlblRes.I_think_its_likely_shell_claim_she_was_forced_by_her_abusive_son);
                    yield return Psychiatrist.Say(GlblRes.Whether_that_will_be_enough_to_convince_the_judges_of_her_innocence_I_cant_say);

                    yield return RyanVoice.Say(GlblRes.Do_you_think_theyll_make_me_talk_to_her);

                    yield return Get<Scripts>().Start(CynthiaDialogScript());

                    yield return RyanVoice.Say(GlblRes.Yeah_and_Landon_Im_still_trying_to_figure_out_what_to_do_with_him);

                    yield return Psychiatrist.Say(GlblRes.Well_itll_be_some_time_before_hes_allowed_visitors_in_the_mental_ward);
                    yield return Psychiatrist.Say(GlblRes.Id_suggest_just_keeping_him_off_your_mind_as_much_as_possible_until_then);
                    yield return Psychiatrist.Say(GlblRes.Then_maybe_later_you_can_tackle_that_problem_as_it_comes);
                    yield return Psychiatrist.Say(GlblRes.And_of_course_Ryan_Im_here_for_you);

                    yield return RyanVoice.Say(GlblRes.I_guess_so_Ill_just_be_glad_when_I_can_put_this_all_behind_me);

                    yield return Psychiatrist.Say(GlblRes.And_you_will_But_I_think_thats_time_for_today_Ryan_Same_time_next_week);

                    yield return RyanVoice.Say(GlblRes.Right_Ill_see_you_next_Tuesday);

                    yield return Delay.Seconds(1f);

                    yield return Psychiatrist.Say(GlblRes.Sure_And_uh_Ryan);

                    yield return RyanVoice.Say(GlblRes.Yes_Doctor);

                    yield return Psychiatrist.Say(GlblRes.Enjoy_your_freedom);

                    yield return Delay.Seconds(1f);

                    yield return RyanVoice.Say(GlblRes.Thanks_Doc);

                    break;
            }

            RyanEyesClosed.Blinking = false;
            yield return Delay.Seconds(2f);
            Game.StopSkipping();
            yield return FadeOutScript();

            Get<Scripts>().Start(FadeInScript());
            Tree.Actors.Scene.Enabled = true;
            switch (FinalScore)
            {
                case ScoreType.Freedom:
                    Game.Ego.EnterScene(Tree.SunSet.Scene);
                    Game.Ego.Get<Transform>().Position = new Microsoft.Xna.Framework.Vector2(403, 344);
                    yield return Game.WaitForSoundEffect(content.audio.birds);
                    yield return FadeOutScript();
                    break;
                case ScoreType.Insanity:
                    Game.Ego.EnterScene(Tree.PaddedCell.Scene);
                    Game.Ego.Get<Transform>().Position = new Microsoft.Xna.Framework.Vector2(268, 349);
                    Game.Ego.Get<Transform>().Speed = 70;
                    Game.Ego.SetWalkingPace(7);
                    yield return Game.Ego.GoTo(0, 349);
                    yield return Game.Ego.GoTo(640, 349);
                    yield return Game.Ego.GoTo(0, 349);
                    var InnerScript = Get<Scripts>().Start(FadeOutScript());
                    yield return Game.Ego.GoTo(640, 349);
                    yield return Script.WaitFor(InnerScript);
                    break;
                default:
                    Game.Ego.EnterScene(Tree.JailCell.Scene);
                    Game.Ego.Get<Transform>().Position = new Microsoft.Xna.Framework.Vector2(344, 334);
                    Game.Ego.Get<Transform>().Speed = 70;
                    Game.Ego.SetWalkingPace(7);
                    yield return Game.Ego.GoTo(0, 334);
                    yield return Game.Ego.GoTo(640, 334);
                    var InnerScriptJail = Get<Scripts>().Start(FadeOutScript());
                    yield return Game.Ego.GoTo(0, 334);
                    yield return Game.Ego.GoTo(640, 334);
                    yield return Script.WaitFor(InnerScriptJail);

                    break;
            }

            Tree.Actors.Scene.Enabled = false;
            Tree.Title.Scene.Visible = true;

            yield return FadeInScript();

            Game.StopSkipping();

            Game.PlaySong(content.audio.credits);
            World.Get<AudioManager>().RepeatSong = false;

            yield return Delay.Seconds(4);

            yield return FadeOutScript();

            Game.EnableTextSkipping(false);

            foreach (var CreditText in GetCreditTexts(true))
            {
                yield return Tree.GUI.ScreenText.ShowText(CreditText, 5f);
                yield return Delay.Seconds(0.5f);
            }

            if (MediaState.Playing == MediaPlayer.State)
            {
                while (MediaPlayer.Volume > 0.0f)
                {
                    yield return Delay.Seconds(0.1f);
                    MediaPlayer.Volume -= .05f;
                }
                MediaPlayer.Stop();
                MediaPlayer.Volume = 1f;
            }

            Game.StopSkipping();
            Game.Engine.Pause();
            Game.Engine.Game.UnloadWorld();
            Game.MainMenu.Show();
            Game.Engine.Renderer.GUIManager.ShowSoftwareCursor = true;
        }

        IEnumerator CynthiaDialogScript()
        {
            if (!ForgaveCynthia)
            {
                yield return Psychiatrist.Say(GlblRes.I_mean_even_after_she_cheated_you_felt_like_you_could_never_trust_her_again_right);
                yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
                yield return Psychiatrist.Say(GlblRes.So_for_your_sake_I_hope_they_dont);
            }
            else
            {
                yield return Psychiatrist.Say(GlblRes.Well_after_her_adultery_issue_you_still_wanted_to_make_things_work_right);
                yield return Psychiatrist.Say(GlblRes.You_have_quite_a_few_good_memories_with_her);
                yield return Psychiatrist.Say(GlblRes.And_she_was_very_scared_this_time_around_Who_knows);
                yield return Psychiatrist.Say(GlblRes.Maybe_talking_to_her_again_with_all_of_the_truth_laid_out_now_wouldnt_be_such_a_bad_thing);
            }

            yield return RyanVoice.TransitionTo(RyanState.Neutral);
        }

        public static IEnumerable<string> GetCreditTexts(bool ending)
        {
            var NL = System.Environment.NewLine;
            var NLNL = NL + NL;

            yield return GlblRes.Writing + NLNL + GlblRes.Jennifer_Jussel;
            yield return GlblRes.Music + NLNL + GlblRes.Damian_Potenzoni;
            yield return GlblRes.Coding + NLNL + GlblRes.Jonas_Jelli;
            yield return GlblRes.Graphics + NLNL + GlblRes.Jeremy_Carver__Jonas_Jelli;
            if (ending)
            {
                yield return "Beta Testing" + NLNL + "";
                yield return "Special Thanks" + NLNL + "Jennifer:";
                yield return "Special Thanks" + NLNL + "Damian: I would like to thank my family and friends for the continuous support. Thanks to everyone involved in Session Seven for making me a part of the experience, you guys rock. Big thanks to my wife, without her, I couldn't make this possible. Love, D.";
                yield return "Special Thanks" + NLNL + "Jonas: My wife, family and friends and everybody contributing to making this game!";
                yield return "Special Thanks" + NLNL + "Jeremy: Amanda & Christian Carver, Maria Smith, Pierre Bezuidenhout, Dawid Frederik Strauss";
            }
            yield return GlblRes.Thank_you_for_playing;
            yield return GlblRes.Devoted_to_all_point__click_adventure_fans;
            yield return GlblRes.wwwsessionsevencom;
        }

        /// <summary>
        /// Returns the game score for the final Session.
        /// </summary>
        /// <returns></returns>
        public ScoreType GetFinalScore()
        {
            var ScoreComponent = Game.Ego.Get<Score>();
            var Result = ScoreComponent.GetScoreTypeResult();

            // If Landon was killed, Freedom Ending is not possible
            if (KilledLandon)
            {
                var JailPoints = ScoreComponent.GetScore(ScoreType.Jail);
                var InsanityPoints = ScoreComponent.GetScore(ScoreType.Insanity);

                Result = JailPoints >= InsanityPoints ? ScoreType.Jail : ScoreType.Insanity;
            }

            return Result;
        }
    }
}
