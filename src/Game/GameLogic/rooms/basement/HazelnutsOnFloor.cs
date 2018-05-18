using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class HazelnutsOnFloor : Entity
    {
        public HazelnutsOnFloor()
        {
            Interaction
               .Create(this)
               .SetPosition(178, 254)
               .SetDirection(Directions8.Left)
               .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.empty_bag_of_hazelnuts)
                .AddRectangle(139, 242, 27, 13);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Pick, PickScript())
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.I_dont_want_to_carry_that_empty_bag_around);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.An_empty_bag_of_hazelnuts_is_lying_on_the_floor);
                yield return Game.Ego.Say(Basement_Res.Looks_like_an_animal_was_chewing_on_it);
            }
        }
    }
}
