using SessionSeven.Actors;
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
    public class Bandages : ItemBase
    {
        public Bandages() : base(content.inventory.bandages, Items_Res.Bandages_Bandages_ClothBandageRoll, true, false)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Use, UseScript())
                    .Add(Verbs.Look, LookScript())
                .For(Tree.InventoryItems.Scissors)
                    .Add(Verbs.Use, UseScissorsScript(), Game.Ego);
        }

        public Script Use()
        {
            return Game.Ego.StartScript(UseScript());
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.A_cloth_bandage_roll_This_is_exactly_what_I_need_right_now);
            }
        }

        IEnumerator UseScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.I_should_better_cut_some_part_off);
                yield return Game.Ego.Say(Items_Res.Cant_use_the_whole_thing);
            }
        }

        IEnumerator UseScissorsScript()
        {
            using (Game.CutsceneBlock())
            {
                Game.Ego.Turn(Directions4.Up);

                yield return Game.Ego.StartUse();

                Game.Ego.Inventory.RemoveItem(this);
                Game.Ego.Inventory.AddItem<BandagesCut>();
                Game.PlaySoundEffect(content.audio.scissors_cut);
                Game.Ego.Inventory.AddItem<BandageStrip>();

                yield return Game.Ego.StopUse();

                Game.Ego.Get<BloodDropEmitter>().ResetCommentCounter();
            }
        }
    }

}
