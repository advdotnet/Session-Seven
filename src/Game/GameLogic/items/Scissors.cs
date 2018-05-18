using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class Scissors : ItemBase
    {
        public Scissors() : base(content.inventory.scissors, Items_Res.Scissors_Scissors_Scissors)
        {
        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Any.Object)
                    .Add(Verbs.Use, UseAnyScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                .For(Tree.InventoryItems.Bandages)
                    .Add(Verbs.Use, OnUseBandages);
        }

        private IEnumerator UseAnyScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.I_dont_want_to_cut_that);
            }
        }

        private Script OnUseBandages(InteractionContext context)
        {
            return Tree.InventoryItems.Bandages.Use();
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.One_pair_of_bandage_scissors);
            }
        }
    }
}
