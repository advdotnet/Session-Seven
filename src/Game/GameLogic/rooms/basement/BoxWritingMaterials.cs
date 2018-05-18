using SessionSeven.GUI.Interaction;
using SessionSeven.InventoryItems;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class BoxWritingMaterials : Entity
    {
        public BoxWritingMaterials()
        {
            Interaction
                .Create(this)
                .SetPosition(205, 242)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.box)
                .AddRectangle(193, 200, 33, 17);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Open, OpenScript());

        }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Use();
                yield return Game.Ego.Say(Basement_Res.Lots_of_gnawed_off_paper_and_plastic);
                if (!Game.Ego.Inventory.HasItem<Paperclip>())
                {
                    yield return Game.Ego.Say(Basement_Res.Also_there_is_a_single_paper_clip_in_there);
                    yield return Game.Ego.StartUse();
                    Game.Ego.Inventory.AddItem<Paperclip>();
                    yield return Game.Ego.StopUse();
                }
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                Get<HotspotRectangle>().SetCaption(Basement_Res.box_with_writing_materials);
                yield return Game.Ego.Say(Basement_Res.Cynthia_stores_some_writing_materials_in_this_box);
                yield return Game.Ego.Say(Basement_Res.There_are_some_bite_marks_by_a_rat_or_something);
            }
        }
    }


}
