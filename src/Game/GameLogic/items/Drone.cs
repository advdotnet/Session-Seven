using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class Drone : ItemBase
    {
        public Drone() : base(content.inventory.drone, Items_Res.Drone_Drone_Drone, true, true)
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
            yield return Game.Ego.StartScript(DronePackage.LookScript());
        }
    }
}
