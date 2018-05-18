using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class BatteryB : ItemBase
    {
        public BatteryB() : base(content.inventory.battery_b, Items_Res.BatteryA_BatteryA_Battery)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.A_battery);
            }
        }
    }
}
