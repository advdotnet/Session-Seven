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
    public class Shelf : Entity
    {
        public const float Z = 1;

        public Shelf()
        {
            Interaction
                .Create(this)
                .SetPosition(1020, 295)
                .SetDirection(Directions8.Right)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.shelf)
                .AddRectangle(1028, 107, 81, 167);

            Transform
                .Create(this)
                .SetZ(Z);

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

        bool FirstLook = true;

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.This_bookcase_is_mostly_made_up_of_my_and_Cynthias_old_college_textbooks);
                if (FirstLook)
                {
                    Tree.Basement.ShelfBox.Enabled = true;
                    Tree.Basement.ShelfEngineeringBooks.Enabled = true;
                    Tree.Basement.ShelfRFIDBook.Enabled = true;
                    Tree.Basement.ShelfBlanketFlashlight.Enabled = true;
                    Tree.Basement.RFIDAntennaShelf.Enabled = true;
                    Tree.Basement.ShelfComicBox.Enabled = true;
                    FirstLook = false;
                }
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.This_is_to_big_to_carry_around);
            }
        }
    }
}
