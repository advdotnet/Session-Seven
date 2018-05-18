using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class CrowbarBroken : ItemBase
    {
        public CrowbarBroken() : base(content.inventory.crowbar_broken, Items_Res.CrowbarBroken_CrowbarBroken_BrokenCrowbar)
        {
        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Any.Object)
                    .Add(Verbs.Use, UseCrowbarWith)
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());
        }

        Script UseCrowbarWith(InteractionContext context)
        {
            return Game.Ego.StartScript(UseCrowbarWithScript(context));
        }

        IEnumerator UseCrowbarWithScript(InteractionContext context)
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.The_two_broken_parts_are_utterly_useless);
            }
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.Unreliable_garbage);
            }
        }
    }
}
