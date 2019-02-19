using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class Envelope : ItemBase
    {
        public Envelope() : base(content.inventory.envelope, Items_Res.Envelope_Envelope_Envelope)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator LookScript(bool reset = true)
        {
            using (Game.CutsceneBlock(reset, reset))
            {
                yield return Game.Ego.Say(Items_Res.The_envelope_is_from_Landons_old_school);
                LookedAt = true;
            }
        }

        public bool ReadLetter { get; private set; }
        public bool LookedAt { get; private set; }

        public Script Open()
        {
            return Game.Ego.StartScript(OpenScript());
        }



        IEnumerator OpenScript()
        {
            using (Game.CutsceneBlock(true, false))
            {
                if (!LookedAt)
                {
                    yield return Game.Ego.StartScript(LookScript(false));
                }

                if (!ReadLetter)
                {
                    yield return Game.Ego.Say(Items_Res.Lets_see);
                }

            }

            Tree.Letter.Scene.Enabled = true;
            Tree.Letter.Scene.Visible = true;
            Tree.GUI.Interaction.Scene.Interactive = false;

            Game.StopSkipping();

            while (Tree.Letter.Scene.Enabled)
            {
                yield return 1;
            }

            var StartSession = Game.Ego.Inventory.HasItem<Blanket>() &&
                Tree.InventoryItems.Blanket.LookedAt &&
                !Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Three);

            using (Game.CutsceneBlock())
            {
                if (StartSession)
                {
                    Game.PlayBasementEndSong();
                }
                else if (!ReadLetter)
                {
                    Game.PlaySoundEffect(content.audio.puzzle);
                }

                if (!ReadLetter)
                {
                    yield return Delay.Seconds(1.5f);
                    yield return Game.Ego.Say(Items_Res.Whats_this_about);
                    yield return Game.Ego.Say(Items_Res.I_knew_he_was_having_problems_in_his_old_school_but_this);
                    yield return Game.Ego.Say(Items_Res.I_thought_we_changed_schools_because_he_was_getting_bullied_Why_would_Cynthia_keep_this_letter_a_secret_from_me);

                    ReadLetter = true;
                }

                Tree.GUI.Interaction.Scene.Interactive = true;
            }

            if (StartSession)
            {
                yield return Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.Three);
            }
        }
    }
}
