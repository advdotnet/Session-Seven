using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class LandonPortrait : Entity
    {
        public LandonPortrait()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.image);

            HotspotSprite
                .Create(this)
                .SetCaption(Basement_Res.portrait)
                .SetPixelPerfect(true);

            Transform
                .Create(this)
                .SetPosition(49, 177)
                .SetZ(263);

            Interaction
                .Create(this)
                .SetPosition(145, 281)
                .SetDirection(Directions8.Left)
                .SetGetInteractionsFn(GetInteractions);

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

        bool LookedAt = false;

        IEnumerator LookScript(bool reset = true)
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock(true, reset))
            {
                yield return Game.Ego.Say(Basement_Res.Weird_is_this_one_of_Landons_school_pictures_Whats_it_doing_down_here);
                LookedAt = true;
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);

            if (!LookedAt)
            {
                yield return Game.Ego.StartScript(LookScript(false));
            }

            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Might_as_well_keep_it_with_me_Maybe_its_good_luck);
                yield return Game.Ego.StartUse();
                Game.Ego.Inventory.AddItem<InventoryItems.Portrait>();
                Enabled = false;
                Visible = false;
                yield return Game.Ego.StopUse();
            }
        }
    }
}
