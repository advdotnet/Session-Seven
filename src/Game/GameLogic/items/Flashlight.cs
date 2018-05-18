using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class Flashlight : ItemBase
    {
        public Flashlight() : base(content.inventory.flashlight, Items_Res.Flashlight_Flashlight_Flashlight, false, true)
        {
            BatteryCompartment
                .Create(this);
        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Any.Object)
                    .Add(Verbs.Use, UseAnyScript(), Game.Ego)
                .For(Tree.InventoryItems.BatteryA)
                    .Add(Verbs.Use, Get<BatteryCompartment>().InstallBatteryAScript(true), Game.Ego)
                .For(Tree.InventoryItems.BatteryB)
                    .Add(Verbs.Use, Get<BatteryCompartment>().InstallBatteryBScript(true), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Open, Get<BatteryCompartment>().OpenScript())
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator UseAnyScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.It_is_sufficiently_bright);
            }
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.A_regular_flashlight);
                yield return Game.Ego.Say(Items_Res.The_battery_compartment_has_place_for_two_batteries);

                var Description = Get<BatteryCompartment>().GetDescriptionString();

                yield return Game.Ego.Say(Description);
            }
        }
    }
}
