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
                .SetCaption("toolbox")
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
                yield return Game.Ego.Say("The toolbox Landon got me for father's day this year.");
                yield return Game.Ego.Say("It's nice, although I haven't found a reason to saw anything yet.");
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
