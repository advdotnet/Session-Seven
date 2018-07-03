using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class CabinetToolbox : Entity
    {
        public CabinetToolbox()
        {
            Interaction
                .Create(this)
                .SetPosition(673, 249)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Transform
                .Create(this)
                .SetZ(3);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.toolbox)
                .AddRectangle(653, 149, 42, 29);

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
                yield return Game.Ego.Say(Basement_Res.The_toolbox_Landon_got_me_for_fathers_day_this_year);
                yield return Game.Ego.Say(Basement_Res.Its_nice_although_I_havent_found_a_reason_to_saw_anything_yet);
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.I_dont_want_to_carry_that_around_now);
            }
        }
    }
}
