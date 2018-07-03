using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class Screwdriver : Entity
    {
        public Screwdriver()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.screwdriver);

            HotspotSprite
                .Create(this)
                .SetCaption(Basement_Res.screwdriver)
                .SetPixelPerfect(false);

            Transform
                .Create(this)
                .SetPosition(505, 101)
                .SetZ(ToolBar.Z + 1);

            Interaction
                .Create(this)
                .SetPosition(517, 264)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Pick, PickScript());
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.A_quality_screwdriver_that_does_not_break_on_the_first_screw);
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);

            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();
                Game.Ego.Inventory.AddItem<InventoryItems.Screwdriver>();
                Enabled = false;
                Visible = false;
                yield return Game.Ego.StopUse();
            }
        }
    }
}
