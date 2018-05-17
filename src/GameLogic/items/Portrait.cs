using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class Portrait : ItemBase
    {
        public Portrait() : base(content.inventory.portrait, Items_Res.Portrait_Portrait_Portrait)
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
                yield return Game.Ego.Say(Items_Res.It_is_a_portrait_of_my_son_Landon_when_he_was_six_years_old);
                yield return Game.Ego.Say(Items_Res.He_looks_happy, 1f);

                if (!Game.Ego.Inventory.HasItem<DrawerKey>())
                {
                    yield return Delay.Seconds(1.5f);
                    yield return Game.Ego.Say(Items_Res.Hey_Theres_a_small_key_glued_to_the_back_of_this);
                    Game.Ego.Inventory.AddItem<DrawerKey>();
                }
            }
        }
    }
}
