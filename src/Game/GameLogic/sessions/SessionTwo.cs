using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Dialog;
using SessionSeven.Office;
using STACK;
using STACK.Components;
using System.Collections;
using GlblRes = global::SessionSeven.Properties.Resources_Session_2;

namespace SessionSeven.Cutscenes
{

    public partial class Director : Entity
    {
        public const int SessionTwoDialogOneOptionOne = 1;
        public const int SessionTwoDialogOneOptionTwo = 2;
        public const int SessionTwoDialogOneOptionThree = 3;

        public int SessionTwoDialogOneOption { get; private set; }

        IEnumerator SessionTwoScript()
        {
            Game.StopSong();
            Game.PlaySoundEffect(content.audio.transition_2);

            Tree.Office.Scene.SetupLate();

            Tree.GUI.Interaction.Scene.Interactive = false;
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

            yield return Psychiatrist.Say(GlblRes.When_you_first_realized_you_were_trapped_what_did_you_do, 4f);

            Game.PlaySong(content.audio.session2);
            World.Get<AudioManager>().RepeatSong = true;

            yield return Delay.Seconds(0.3f);

            Game.StopSkipping();

            var Menu = Tree.GUI.Dialog.Menu;

            var RootDialog = ScoreOptions.Create()
                .Add(SessionTwoDialogOneOptionOne, GlblRes.I_just_lost_it_I_was_terrified, ScoreType.Insanity, 1)
                .Add(SessionTwoDialogOneOptionTwo, GlblRes.I_got_frustrated_I_tried_to_remember_what_I_was_doing_before_it_all_started, ScoreType.Jail, 1)
                .Add(SessionTwoDialogOneOptionThree, GlblRes.I_stayed_calm_I_wasnt_just_going_to_give_up, ScoreType.Freedom, 1);

            Menu.Open(RootDialog);

            yield return Menu.StartSelectionScript(Get<Scripts>());

            Game.EnableSkipping();

            var RootSelection = Menu.LastSelectedOption;
            ProcessScore(RootSelection);

            // store the choice for the therapy log item
            SessionTwoDialogOneOption = Menu.LastSelectedOption.ID;

            var NewRyanState = RyanState.ArmsCrossed;

            switch (RootSelection.ID)
            {
                case SessionTwoDialogOneOptionOne:
                case SessionTwoDialogOneOptionTwo:
                    NewRyanState = RyanState.ArmsCrossed;
                    break;
                case SessionTwoDialogOneOptionThree:
                    NewRyanState = RyanState.ArmsRaised;
                    break;
            }

            yield return RyanVoice.TransitionTo(NewRyanState);
            yield return RyanVoice.Say(RootSelection.Text);

            switch (RootSelection.ID)
            {
                case SessionTwoDialogOneOptionOne:
                    yield return RyanVoice.Say(GlblRes.I_was_just_trying_everything_I_could_to_get_out_of_there);
                    yield return RyanVoice.Say(GlblRes.But_then_I_wasnt_even_sure_how_Id_gotten_there);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.Nothing_was_making_sense_to_me_It_was_like_a_mad_blur_I_felt_like_banging_on_the_walls_most_of_the_time);
                    yield return RyanVoice.Say(GlblRes.I_think_I_was_hoping_somebody_would_hear_me_but_nobody_was_there);
                    yield return RyanVoice.Say(GlblRes.Neither_my_wife_or_my_son_was_answering_me_I_thought_maybe_they_were_in_danger_too_);

                    yield return Psychiatrist.Say(GlblRes.Dont_you_think_itd_be_less_confusing_if_you_started_with_how_you_got_there_in_the_first_place);

                    yield return RyanVoice.Say(GlblRes.If_only_I_knew);

                    break;

                case SessionTwoDialogOneOptionTwo:
                    yield return RyanVoice.Say(GlblRes.Before_I_was_there_you_know_How_did_it_come_to_this_And_where_was_my_family);
                    yield return RyanVoice.Say(GlblRes.I_was_trying_to_think_back_on_it_and_I_just_couldnt_remember);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.It_was_like_everything_leading_up_to_that_point_was_just_a_dream_and_I_couldnt_quite_piece_together_where_it_started_or_ended);
                    yield return RyanVoice.Say(GlblRes.And_no_matter_how_hard_I_tried_I_just_couldnt_wake_up);

                    yield return Psychiatrist.Say(GlblRes.Well_I_think_the_beginning_is_a_good_place_to_start_How_did_you_think_you_got_there);

                    yield return RyanVoice.Say(GlblRes.Thats_just_it_I_didnt_know);

                    break;
                case SessionTwoDialogOneOptionThree:
                    yield return RyanVoice.Say(GlblRes.I_did_my_best_to_figure_out_what_was_going_on_I_wasnt_just_going_to_sit_down_and_take_it);
                    yield return RyanVoice.TransitionTo(RyanState.Neutral);
                    yield return RyanVoice.Say(GlblRes.Especially_not_if_my_family_was_in_danger_somewhere_else);
                    yield return RyanVoice.Say(GlblRes.I_looked_everywhere_for_clues_anything_I_could_find_that_might_help_me_fix_the_mess_Id_gotten_myself_into);
                    yield return RyanVoice.Say(GlblRes.I_guess_that_started_with_figuring_out_how_Id_gotten_there_in_the_first_place);


                    yield return Psychiatrist.Say(GlblRes.Well_why_dont_we_start_there);

                    yield return RyanVoice.Say(GlblRes.Well_that_implies_that_I_had_any_idea_at_all);

                    break;
            }

            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;
            yield return Delay.Seconds(0.5f);
            RyanEyesClosed.Blinking = true;

            Tree.Basement.Scene.Visible = true;
            Tree.Office.Scene.Visible = false;

            Tree.Office.Scene.Enabled = false;
            Tree.Office.Scene.Visible = false;

            Tree.Cutscenes.Scene.Visible = false;
            Tree.Cutscenes.Scene.Enabled = false;

            World.Interactive = true;

            Game.Ego.Inventory.Show();

            Tree.Basement.Scene.Interactive = true;
            Tree.Actors.Scene.Enabled = true;

            Tree.Basement.Crates.Enabled = true;
            Tree.Basement.WindowLeft.Enabled = true;
            Tree.Basement.WindowRight.Enabled = true;
            Tree.Basement.LightSwitch.Enabled = true;
            Tree.Basement.Desk.Enabled = true;
            Tree.Basement.FamiliyPortrait.Enabled = true;
            Tree.Basement.BloodOnFloor.Enabled = true;
            Tree.Basement.LandonPortrait.Enabled = true;
            Tree.Basement.DrawerRight.Enabled = true;
            Tree.Basement.DrawerLeft.Enabled = true;
            Tree.Basement.CabinetRightDoor.Enabled = true;
            Tree.Basement.ToolBar.Enabled = true;
            Tree.Basement.GuitarCase.Enabled = true;
            Tree.Basement.Crowbar.Enabled = true;
            Tree.Basement.MouseHole.Enabled = true;
            Tree.Basement.Hazelnuts.Enabled = true;
            Tree.Basement.SocketsLeft.Enabled = true;
            Tree.Basement.SocketsCenter.Enabled = true;
            Tree.Basement.Shelf.Enabled = true;
            Tree.Basement.ToDoBoard.Enabled = true;
            Tree.Basement.BoxWritingMaterials.Enabled = true;
            Tree.Basement.BoxScrews.Enabled = true;
            Tree.Basement.CrumbsLeft.Enabled = true;
            Tree.Basement.CrumbsRight.Enabled = true;
            Tree.Basement.RFIDAntennaCabinet.Enabled = true;
            Tree.Basement.CabinetLock.Enabled = true;
            Tree.Basement.CabinetLeftDoor.Enabled = true;
            Tree.Basement.Workbench.Enabled = true;
            Tree.Basement.Carpet.Enabled = true;
            Tree.Basement.Cactus.Enabled = true;
            Tree.Basement.DrillingMachine.Enabled = true;
            Tree.Basement.DrillingMachineCable.Enabled = true;
            Tree.Basement.HazelnutsOnFloor.Enabled = true;

            Tree.GUI.Interaction.Scene.Interactive = true;
            Tree.GUI.Interaction.Scene.Visible = true;

            World.Get<AudioManager>().RepeatSong = false;

            Game.StopSkipping();
        }
    }
}
