using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class Hammer : Entity
    {
        public Hammer()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.hammer);

            HotspotSprite
                .Create(this)
                .SetCaption(Basement_Res.hammer)
                .SetPixelPerfect(false);

            Transform
                .Create(this)
                .SetPosition(470, 99)
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
                yield return Game.Ego.Say(Basement_Res.A_hammer_is_hanging_on_the_tool_bar);
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);

            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();
                Game.Ego.Inventory.AddItem<InventoryItems.Hammer>();
                Enabled = false;
                Visible = false;
                yield return Game.Ego.StopUse();
            }
        }
    }
}
