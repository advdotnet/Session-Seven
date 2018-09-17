using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class DrugPrescriptionRyan : ItemBase
    {
        public DrugPrescriptionRyan() : base(content.inventory.drugprescriptionryan, Items_Res.DrugPrescriptionRyan_DrugPrescriptionRyan_DrugPrescription)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());
        }

        public Script LookAt()
        {
            return Game.Ego.StartScript(LookScript());
        }

        public bool LookedAt { get; private set; }

        public IEnumerator LookScript()
        {
            var StartSession = Game.Ego.Inventory.HasItem<GuitarStrings>();

            if (StartSession)
            {
                Game.PlaySong(content.audio.basementend);
                World.Get<AudioManager>().RepeatSong = false;
            }
            else
            {
                Game.PlaySoundEffect(content.audio.puzzle);
            }

            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.This_is_an_old_xanax_prescription_which_was_issued_for_me_some_years_ago);

                LookedAt = true;
            }

            if (StartSession)
            {
                yield return Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.Four);
            }
        }
    }
}
