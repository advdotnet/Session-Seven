using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class Folders : Entity
    {
        public Folders()
        {
            Interaction
                .Create(this)
                .SetPosition(205, 242)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.folders)
                .AddRectangle(189, 148, 63, 45);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.There_are_all_sorts_of_different_colored_folders_full_of_documents_for_the_new_house_old_tax_files_and_various_papers_for_Landon);
                yield return Game.Ego.Say(Basement_Res.I_didnt_realize_how_much_Cynthia_was_handling_on_her_own_while_I_was_away_at_work);
            }
        }
    }
}
