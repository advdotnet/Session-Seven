using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class Crowbar : Entity
    {
        public Crowbar()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.crowbar);

            HotspotRectangle
                .Create(this)
                .SetRectangle(845, 176, 23, 19)
                .SetCaption(Basement_Res.crowbar);

            Transform
                .Create(this)
                .SetPosition(845, 176)
                .SetZ(1);

            Interaction
                .Create(this)
                .SetPosition(814, 241)
                .SetDirection(Directions8.Right)
                .SetGetInteractionsFn(GetInteractions);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Use, UseScript())
                    .Add(Verbs.Pick, PickScript());
        }

        IEnumerator UseScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.I_should_take_it_first);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.My_good_old_crowbar);
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.You_will_never_know_when_this_will_come_in_handy);
                yield return Game.Ego.StartUse();
                Game.Ego.Inventory.AddItem<InventoryItems.Crowbar>();
                Enabled = false;
                Visible = false;
                yield return Game.Ego.StopUse();
            }
        }
    }
}
