using Microsoft.Xna.Framework;
using SessionSeven.Actors;
using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using GlblRes = global::SessionSeven.Properties.Resources_Session_1;

namespace SessionSeven.Cutscenes
{

    public partial class Director : Entity
    {
        AudioManager Audio { get { return World.Get<AudioManager>(); } }

        IEnumerator SessionOneScript()
        {
            var BloomSettings = new BloomSettings("SessionSeven", 0.75f, 1.5f, 0.8f, 1, 1, 1);

            World.Get<RenderSettings>().BloomSettings = BloomSettings;
            Game.Ego.Inventory.Hide();
            Tree.Office.Scene.SetupEarly();

            Game.EnableSkipping();
            Game.EnableTextSkipping(false);

            Game.PlaySong(content.audio.intro);

            Tree.GUI.Interaction.Scene.Visible = false;

            Tree.Actors.Scene.Enabled = false;
            Tree.Basement.Scene.Interactive = false;
            Tree.Basement.Scene.Visible = false;
            Tree.Actors.Scene.Enabled = false;
            Tree.GUI.Mouse.Visible = true;
            Tree.Office.Scene.Visible = true;

            Tree.Office.Scene.Interactive = true;
            Tree.Office.Scene.Enabled = true;
            World.Get<RenderSettings>().BloomEnabled = true;
            Game.Ego.Enabled = false;
            Game.Ego.Visible = false;

            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;

            Psychiatrist.StartWriting();

            var Sound = Game.PlaySoundEffect(content.audio.scribble_short);
            var FadeScript = Get<Scripts>().Start(FadeInScript());
            int Frames = 0;

            while (Sound.IsPlaying())
            {
                yield return 1;
                Frames++;
                if (Frames > 90)
                {
                    RyanEyesClosed.Blinking = true;
                }
            }

            RyanEyesClosed.Blinking = true;

            Psychiatrist.StopWriting();

            yield return STACK.Script.WaitFor(FadeScript);

            yield return Delay.Seconds(0.3f);

            yield return Psychiatrist.Say(GlblRes.And_what_was_life_like_for_you_there_Ryan, 4f);
            yield return Psychiatrist.Say(GlblRes.How_did_things_go, 3f);

            Game.PlaySoundEffect(content.audio.transition_1);
            Tree.Actors.Scene.Enabled = true;

            Tree.Basement.RyanLying.Visible = true;
            Tree.Basement.RyanLying.Enabled = true;
            Tree.Basement.RyanLying.State = Basement.RyanLyingState.EyesClosed;
            Tree.Basement.Scene.Visible = true;
            Tree.Office.Scene.Visible = false;
            Tree.Office.Scene.Enabled = false;

            Fader.Color = Color.Black;
            Fader.Visible = true;

            Sound = Tree.Basement.RyanLying.PlayWhiningSound();

            World.Get<RenderSettings>().BloomSettings = new BloomSettings("temp", 0, 8, 3.5f, 1, 1, 0);
            World.Get<RenderSettings>().BloomEnabled = true;

            const int loops2 = 500;
            bool eyesOpen = false;

            for (int j = 0; j < loops2; j++)
            {
                Fader.Color = new Color(255, 255, 255, Math.Max(0, 200 - j));

                World.Get<RenderSettings>().BloomSettings.BloomThreshold += 0.75f / loops2;
                World.Get<RenderSettings>().BloomSettings.BlurAmount -= 6.5f / loops2;
                World.Get<RenderSettings>().BloomSettings.BloomIntensity -= 2.7f / loops2; // -> 1
                World.Get<RenderSettings>().BloomSettings.BaseSaturation += 1.0f / loops2;

                switch (j)
                {
                    case 300: eyesOpen = true; break;
                    case 380: eyesOpen = false; break;
                    case 400: eyesOpen = true; break;
                }

                Tree.Basement.RyanLying.State = eyesOpen ? Basement.RyanLyingState.EyesOpen : Basement.RyanLyingState.EyesClosed;

                yield return 1;
            }

            World.Get<RenderSettings>().BloomSettings = BloomSettings;

            Fader.Color = Color.Black;
            Fader.Visible = false;

            yield return STACK.Script.WaitFor(Sound);

            Tree.Basement.Scene.Visible = false;
            Tree.Office.Scene.Visible = true;
            Tree.Office.Scene.Enabled = true;

            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;

            yield return Delay.Seconds(1);

            RyanEyesClosed.Blinking = true;

            yield return RyanVoice.TransitionTo(Office.RyanState.HandsIntertwined);

            yield return RyanVoice.Say(GlblRes.Do_you_have_to_ask_It_was_horrible, 4.6f);

            Tree.Basement.RyanLying.State = Basement.RyanLyingState.SittingEyesClosed;
            Tree.Basement.RyanLying.Get<Transform>().SetPosition(259 + 22, 178);
            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;
            yield return Delay.Seconds(0.75f);
            RyanEyesClosed.Blinking = true;

            yield return RyanVoice.Say(GlblRes.It_wasnt_a_life_It_wasnt_living, 4.5f);

            Tree.Basement.Scene.Visible = true;
            Tree.Office.Scene.Visible = false;
            Tree.Office.Scene.Enabled = false;

            Tree.Basement.RyanLying.State = Basement.RyanLyingState.SittingEyesClosed;
            yield return Delay.Seconds(0.5f);
            Tree.Basement.RyanLying.State = Basement.RyanLyingState.SittingEyesOpen;
            yield return Delay.Seconds(0.3f);
            Tree.Basement.RyanLying.State = Basement.RyanLyingState.SittingEyesClosed;
            yield return Delay.Seconds(0.2f);
            Tree.Basement.RyanLying.State = Basement.RyanLyingState.SittingEyesOpen;
            yield return Delay.Seconds(1.3f);

            Tree.Basement.Scene.Visible = false;
            Tree.Office.Scene.Visible = true;
            Tree.Office.Scene.Enabled = true;

            yield return Delay.Seconds(2);

            yield return Psychiatrist.Say(GlblRes.And_what_was_the_worst_part_about_it_would_you_say, 4f);

            yield return Delay.Seconds(0.3f);


            Tree.Basement.Scene.Visible = true;
            Tree.Office.Scene.Visible = false;
            Tree.Office.Scene.Enabled = false;

            Tree.Actors.RyanVoice.Visible = true;
            yield return Tree.Actors.RyanVoice.Say(GlblRes.Hello, 1.6f);
            yield return Delay.Seconds(0.5f);
            yield return Tree.Actors.RyanVoice.Say(GlblRes.Cynthia, 1.8f);
            Tree.Actors.RyanVoice.Visible = false;

            yield return Delay.Seconds(2);

            Tree.Basement.Scene.Visible = false;
            Tree.Office.Scene.Visible = true;
            Tree.Office.Scene.Enabled = true;

            yield return RyanVoice.Say(GlblRes.The_not_knowing_Definitely, 3.8f);

            yield return RyanVoice.TransitionTo(Office.RyanState.Neutral);

            Tree.Office.Therapist.StartWriting();
            Sound = Game.PlaySoundEffect(content.audio.scribble_long);
            var Script = RyanVoice.Say(GlblRes.Not_knowing_what_was_going_to_happen_next_or_who_was_going_to_be_waiting_for_me_not_knowing_what_would_happen_to_me_or_my_family, 14.6f);

            yield return Script.WaitFor(Sound);

            Tree.Office.Therapist.StopWriting();

            yield return Script.WaitFor(Script);

            Tree.Basement.Scene.Visible = true;
            Tree.Office.Scene.Visible = false;
            Tree.Office.Scene.Enabled = false;

            Game.Ego.Visible = true;
            Game.Ego.Enabled = true;
            Tree.Basement.RyanLying.Visible = false;
            Tree.Basement.RyanLying.Enabled = false;

            yield return Game.Ego.GoTo(400, 250);
            yield return Game.Ego.Say(GlblRes.HelloExclamation, 1.8f);
            yield return Game.Ego.GoTo(150, 250);
            yield return Game.Ego.Say(GlblRes.Help_Help_me, 2.4f);

            Tree.Basement.Scene.Visible = false;
            Tree.Office.Scene.Visible = true;
            Tree.Office.Scene.Enabled = true;

            yield return RyanVoice.Say(GlblRes.Not_knowing_how_the_hell_I_was_going_to_get_out_of_there_or_if_I_ever_would_living_in_a_constant_state_of_fear, 12.5f);
            yield return Delay.Seconds(0.25f);
            RyanEyesClosed.Blinking = false;
            RyanEyesClosed.Visible = true;
            yield return Delay.Seconds(0.25f);
            yield return RyanVoice.Say(GlblRes.That_by_far_was_the_worst_part_of_it, 4.9f);

            Game.Ego.Get<Transform>().SetPosition(100, 250);
            Tree.Basement.Scene.Visible = true;
            Tree.Office.Scene.Visible = false;
            Tree.Office.Scene.Enabled = false;
            RyanEyesClosed.Blinking = true;

            // Blood dropping
            Game.Ego.Get<BloodDropEmitter>().Start();

            yield return Delay.Seconds(1.5f);

            FadeScript = Get<Scripts>().Start(FadeOutScript());

            yield return Game.Ego.Say(GlblRes.HelloExclamation, 1.8f);
            Script = Game.Ego.GoTo(500, 250);

            yield return STACK.Script.WaitFor(Script, FadeScript);

            yield return Delay.Seconds(1);

            Game.StopSkipping();
            Game.EnableSkipping();

            Game.PlaySoundEffect(content.audio.title);
            yield return Delay.Seconds(0.5f);
            Tree.Title.Scene.Visible = true;

            yield return FadeInScript();

            yield return Delay.Seconds(4);

            Game.Ego.Get<Transform>().SetPosition(300, 250);

            yield return FadeOutScript();

            Tree.Title.Scene.Visible = false;
            Game.Ego.Turn(Directions4.Down);

            yield return FadeInScript();

            Tree.Office.Scene.Enabled = false;
            Tree.Office.Scene.Visible = false;

            Tree.Office.Scene.Background.Visible = true;

            Tree.Basement.Scene.Interactive = true;
            Game.EnableTextSkipping(true);

            yield return Game.Ego.Say(GlblRes.Ouch_How_the_hell_did_this_happen);
            yield return Game.Ego.Say(GlblRes.I_need_to_bandage_my_hand_I_think_we_have_a_medicine_cabinet_down_here_somewhere);

            Tree.Cutscenes.Scene.Visible = false;
            Tree.Cutscenes.Scene.Enabled = false;

            Tree.GUI.Interaction.Scene.Visible = true;
            Game.Ego.Inventory.Show();

            Game.StopSkipping();

            Game.PlaySong(content.audio.basement);
            World.Get<AudioManager>().RepeatSong = true;

            World.Interactive = true;
        }
    }
}
