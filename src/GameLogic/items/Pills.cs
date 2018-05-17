using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class Pills : ItemBase
    {
        public Pills()
            : base(content.inventory.pills, Items_Res.Pills_Pills_Antidepressants, true, false)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Use, UseScript())
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator UseScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.Im_not_taking_this_kind_of_medicine);
            }
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Items_Res.Landons_antidepressants_They_have_not_yet_been_opened);
                yield return Game.Ego.Say(Items_Res.I_thought_he_was_taking_them_regularly);
            }
        }
    }

}
