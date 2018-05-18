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
    public class BandageStrip : ItemBase
    {
        public BandageStrip() : base(content.inventory.bandages_strip, Items_Res.BandageStrip_BandageStrip_ClothBandage, true, false)
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
                yield return Game.Ego.Say(Items_Res.A_cloth_bandage_I_should_use_this_to_dress_my_wound);
            }
        }

        IEnumerator UseScissorsScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.I_dont_need_to_cut_that_off_any_further);
            }
        }

        IEnumerator UseScript()
        {
            using (Game.CutsceneBlock())
            {
                Game.EnableSkipping();
                Game.Ego.Turn(Directions4.Up);

                yield return Game.Ego.StartUse();
                yield return Delay.Updates(100);
                Game.Ego.Get<Sprite>().SetImage(content.characters.ryan.sprite, 13, 8);
                Game.Ego.Inventory.RemoveItem(this);
                Game.Ego.Get<BloodDropEmitter>().Stop();
                yield return Game.Ego.StopUse();

                yield return Game.Ego.Say(Items_Res.That_should_be_enough);

                yield return Delay.Seconds(0.5f);

                Game.Ego.Inventory.Hide();

                yield return Game.Ego.GoTo(Tree.Basement.Door);
                yield return Game.Ego.StartUse();
                Game.PlaySoundEffect(content.audio.door_knob);
                yield return Delay.Seconds(1);
                yield return Game.Ego.StopUse();

                yield return Delay.Seconds(0.5f);
                yield return Game.Ego.StartUse();
                Game.PlaySoundEffect(content.audio.door_knock);
                yield return Delay.Seconds(1);
                yield return Game.Ego.StopUse();

                yield return Delay.Seconds(0.5f);

                yield return Game.Ego.Say(Items_Res.HELLO_IS_SOMEBODY_THERE);

                yield return Delay.Seconds(2.5f);

                yield return Game.Ego.StartUse();
                Game.PlaySoundEffect(content.audio.door_knock);
                yield return Delay.Seconds(1);
                yield return Game.Ego.StopUse();

                yield return Delay.Seconds(1.5f);

                yield return Game.Ego.Say(Items_Res.Ive_got_to_find_a_way_out_of_here);

                Tree.Basement.Door.Enabled = true;
                Tree.Basement.WindowLeft.Enabled = true;
                Tree.Basement.WindowRight.Enabled = true;

                Game.Ego.Inventory.Show();

                Game.StopSkipping();
            }
        }
    }

}
