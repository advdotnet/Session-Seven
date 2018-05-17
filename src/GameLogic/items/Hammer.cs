using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class Hammer : ItemBase
    {
        public Hammer()
            : base(content.inventory.hammer, Items_Res.Hammer_Hammer_Hammer)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Any.Object)
                    .Add(Verbs.Use, UseHammerWithScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.Its_a_tool_with_a_heavy_piece_of_metal_at_the_end_of_a_handle);
            }
        }

        IEnumerator UseHammerWithScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.If_all_you_have_is_a_hammer_everything_looks_like_a_nail);
            }
        }
    }

}
