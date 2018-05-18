using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class ShelfSingleBook : Entity
    {
        public ShelfSingleBook()
        {
            Interaction
                .Create(this)
                .SetPosition(1020, 295)
                .SetDirection(Directions8.Right)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.notebook)
                .AddRectangle(1049, 183, 3, 24);

            Transform
                .Create(this)
                .SetZ(Shelf.Z + 2);

            Enabled = false;
        }

        Interactions GetInteractions()
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
                yield return Game.Ego.Say(Basement_Res.An_old_notebook_from_university);
                yield return Game.Ego.Use();
                yield return Game.Ego.Say(Basement_Res.The_first_page_reads);
                yield return Game.Ego.Say(Basement_Res._There_was_a_time_that_the_pieces_fit_but_I_watched_them_fall_away_);
                yield return Game.Ego.Say(Basement_Res._Mildewed_and_smoldering_strangled_by_our_coveting_);

            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.I_dont_need_that);
            }
        }
    }
}
