using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class CabinetSupplies : Entity
    {
        public CabinetSupplies()
        {
            Interaction
                .Create(this)
                .SetPosition(703, 247)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Transform
                .Create(this)
                .SetZ(3);

            HotspotRectangle
                .Create(this)
                .SetCaption("arts and crafts supplies")
                .AddRectangle(703, 149, 44, 28);

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
                yield return Game.Ego.Say("Cynthia took control of this cabinet compartment for her arts and crafts supplies a while ago.");
                yield return Game.Ego.Say("It's full of all sorts of glitter glues, paint, and what not.");
                yield return Game.Ego.Say("It doesn't look like she's touched any of it in a while, though.");
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
