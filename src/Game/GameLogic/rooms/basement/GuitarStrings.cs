using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using SessionSeven.InventoryItems;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class GuitarStrings : Entity
    {
        public GuitarStrings()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.guitarstringsfloor);

            Transform
                .Create(this)
                .SetPosition(162, 229);

            Interaction
                .Create(this)
                .SetPosition(166, 256)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotSprite
                .Create(this)
                .SetPixelPerfect(false)
                .SetCaption(Basement_Res.guitar_strings);

            Visible = false;
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
                var StartSession = Game.Ego.Inventory.HasItem<DrugPrescriptionRyan>() && Tree.InventoryItems.DrugPrescriptionRyan.LookedAt;

                if (!StartSession)
                {
                    Game.PlaySoundEffect(content.audio.puzzle);
                }

                yield return Game.Ego.StartUse();
                Game.Ego.Inventory.AddItem<InventoryItems.GuitarStrings>();
                this.Visible = false;
                this.Enabled = false;
                yield return Game.Ego.StopUse();

                if (StartSession)
                {
                    yield return Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.Four);
                }
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.A_guitar_string_package_that_dropped_out_when_I_opened_the_guitar_case);
            }
        }
    }


}
