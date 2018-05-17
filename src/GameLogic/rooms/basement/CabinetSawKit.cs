using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class CabinetSawKit : Entity
    {
        public CabinetSawKit()
        {
            HotspotSprite
                .Create(this)
                .SetCaption(Basement_Res.bimetal_hole_saw_kit)
                .SetPixelPerfect(false);

            Interaction
                .Create(this)
                .SetPosition(703, 247)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Transform
                .Create(this)
                .SetPosition(671, 199)
                .SetZ(3);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.sawkit);

            Enabled = false;
            Visible = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Open, UseScript())
                    .Add(Verbs.Pick, PickScript())
                    .Add(Verbs.Use, UseScript());
        }

        IEnumerator UseScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.I_better_take_that_with_me_first);
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();
                Visible = false;
                Game.Ego.Inventory.AddItem<InventoryItems.SawKit>();
                Enabled = false;
                yield return Game.Ego.StopUse();
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.A_bimetal_hole_saw_kit);
            }
        }
    }
}
