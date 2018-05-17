using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class Crates : Entity
    {
        public Crates()
        {
            Interaction
                .Create(this)
                .SetPosition(908, 269)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.crates)
                .AddRectangle(827, 120, 147, 134);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.InventoryItems.Hammer)
                    .Add(Verbs.Use, UseHammerScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Push, PushScript());
        }

        IEnumerator UseHammerScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();
                yield return Game.WaitForSoundEffect(content.audio.hammer_wood);
                yield return Game.Ego.StopUse();
            }
        }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.They_are_empty_We_just_kept_them_if_we_need_to_move_again);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Man_moving_makes_a_mess);
            }
        }

        IEnumerator PushScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.I_dont_see_the_point_in_that_There_is_nothing_underneath);
            }
        }
    }


}
