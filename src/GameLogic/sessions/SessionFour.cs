using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Dialog;
using SessionSeven.Office;
using STACK;
using STACK.Components;
using System.Collections;
using System.Collections.Generic;
using GlblRes = global::SessionSeven.Properties.Resources_Session_4;

namespace SessionSeven.Cutscenes
{

    public partial class Director : Entity
    {
        IEnumerator SessionFourScript()
        {
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

            yield return Psychiatrist.Say(GlblRes.I_think_we_could_both_benefit_from_learning_a_little_bit_more_about_yourself);

            yield return Delay.Seconds(1);

            yield return RyanVoice.Say(GlblRes.Like_what);

            yield return Psychiatrist.Say(GlblRes.Well_what_do_you_do_for_work);

            yield return RyanVoice.Say(GlblRes.Im_a_mechanical_engineer);

            yield return Psychiatrist.Say(GlblRes.And_you_like_your_work);

            Game.StopSkipping();

            var Menu = Tree.GUI.Dialog.Menu;

            var FirstDialog = ScoreOptions.Create()
                .Add(1, GlblRes.Cant_complain, ScoreType.Freedom, 1)
                .Add(2, GlblRes.Its_awful, new Dictionary<ScoreType, int> {
                    { ScoreType.Jail, 1 },
                    { ScoreType.Insanity, 1}
                });

            Menu.Open(FirstDialog);

            yield return Menu.StartSelectionScript(Get<Scripts>());

            Game.EnableSkipping();

            var FirstSelection = Menu.LastSelectedOption;
            ProcessScore(FirstSelection);

            if (FirstSelection.ID == 2)
            {
                yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
            }
            else
            {
                yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
            }


            yield return RyanVoice.Say(FirstSelection.Text);

            if (FirstSelection.ID == 1)
            {
                yield return Psychiatrist.Say(GlblRes.Thats_good_Liking_what_you_do_is_important);
            }
            else
            {
                yield return Psychiatrist.Say(GlblRes.Im_sorry_to_hear_that);
            }

            yield return Psychiatrist.Say(GlblRes.So_were_you_upset_about_having_to_change_offices_to_be_with_your_son);

            Game.StopSkipping();

            var SecondDialog = ScoreOptions.Create()
                .Add(1, GlblRes.Yes_I_dont_handle_change_well, ScoreType.Insanity, 1)
                .Add(2, GlblRes.Yes_I_had_it_good_where_I_was, ScoreType.Jail, 1)
                .Add(3, GlblRes.No_It_doesnt_matter_to_me_if_Im_with_my_family, ScoreType.Freedom, 1);

            Menu.Open(SecondDialog);

            yield return Menu.StartSelectionScript(Get<Scripts>());

            Game.EnableSkipping();

            var SecondSelection = Menu.LastSelectedOption;
            ProcessScore(SecondSelection);

            if (SecondSelection.ID == 1 || SecondSelection.ID == 2)
            {
                yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);
            }
            else
            {
                yield return RyanVoice.TransitionTo(RyanState.HandsIntertwined);
            }

            yield return RyanVoice.Say(SecondSelection.Text);

            switch (SecondSelection.ID)
            {
                case 1:
                    yield return Psychiatrist.Say(GlblRes.I_see_Perhaps_this_time_however_it_was_for_the_best);
                    break;
                case 2:
                    yield return Psychiatrist.Say(GlblRes.Thats_unfortunate_But_I_wonder_if_you_cant_make_something_better_for_yourself_where_you_are_now);
                    break;
                case 3:
                    yield return Psychiatrist.Say(GlblRes.Good_to_hear_Your_family_is_your_support_system);
                    break;
            }

            yield return Delay.Seconds(1);

            yield return Psychiatrist.Say(GlblRes.Anyways_work_isnt_the_only_thing_that_makes_up_a_man_What_would_you_say_you_do_for_fun);

            yield return RyanVoice.TransitionTo(RyanState.Neutral);

            yield return RyanVoice.Say(GlblRes.Oh_I_dont_know_I_play_the_guitar);

            yield return Psychiatrist.Say(GlblRes.And_do_you_find_that_playing_helps_you_when_you_are_agitated);

            yield return RyanVoice.TransitionTo(RyanState.ArmsCrossed);

            yield return RyanVoice.Say(GlblRes.Do_you_have_to_call_it_that);

            yield return Delay.Seconds(1);

            yield return Psychiatrist.Say(GlblRes.Im_sorry_is_there_something_else_you_would_prefer_I_call_your_episodes);

            yield return Delay.Seconds(0.25f);

            Psychiatrist.StartWriting();

            var Sound = Game.PlaySoundEffect(content.audio.scribble_long);

            yield return RyanVoice.Say(GlblRes.Well_I_I_dont_know_It_doesnt_matter_Besides_I_havent_had_a_problem_like_that_in_a_long_time);

            yield return Script.WaitFor(Sound);

            Psychiatrist.StopWriting();

            yield return Psychiatrist.Say(GlblRes.Is_that_true_Youre_taking_your_medications_regularly);

            yield return RyanVoice.Say(GlblRes.Dont_you_believe_me);

            RyanVoice.TransitionTo(RyanState.Neutral);

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
