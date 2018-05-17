using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class Paperclip : ItemBase
    {
        public Paperclip() : base(content.inventory.paperclip, Items_Res.Paperclip_Paperclip_PaperClip)
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
                yield return Game.Ego.Say(Items_Res.The_plastic_wrapping_is_missing);
            }
        }
    }
}
