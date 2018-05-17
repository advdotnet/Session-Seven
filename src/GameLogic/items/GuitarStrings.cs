using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class GuitarStrings : ItemBase
    {
        public GuitarStrings() : base(content.inventory.strings, Items_Res.GuitarStrings_GuitarStrings_Strings, true, true)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.InventoryItems.Baton)
                    .Add(Verbs.Use, UseStringsWithBatonScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, OnLook());
        }

        IEnumerator UseStringsWithBatonScript()
        {
            if (null != Tree.InventoryItems.Baton)
            {
                yield return Tree.InventoryItems.Baton.UseStringsScript();
            }
        }

        IEnumerator OnLook()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.These_are_my_replacement_guitar_strings);
            }
        }
    }

}
