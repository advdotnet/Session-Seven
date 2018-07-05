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
                .For(Any.Object)
                    .Add(Verbs.Use, UseScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator UseScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.I_dont_see_the_point_in_making_the_drone_fly_to_that);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.StartScript(DronePackage.LookScript());
        }
    }
}
