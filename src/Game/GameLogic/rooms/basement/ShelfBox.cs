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
    public class ShelfBox : Entity
    {
        public ShelfBox()
        {
            Interaction
                .Create(this)
                .SetPosition(1020, 295)
                .SetDirection(Directions8.Right)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.box)
                .AddRectangle(1065, 142, 41, 30);

            Transform
                .Create(this)
                .SetZ(Shelf.Z + 1);

            Enabled = false;
        }

        Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Pick, PickScript());
        }

        private int OpenedCount = 0;
        private bool LookedAt = false;

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();

                var TrackNumber = Math.Min(3, OpenedCount + 1);

                yield return Game.WaitForSoundEffect(content.audio._path_ + "rustling" + TrackNumber);
                yield return Game.Ego.StopUse();

                switch (OpenedCount)
                {
                    case 0:
                        yield return Game.Ego.Say(Basement_Res.There_are_numerous_guarantee_cards_in_there);
                        break;
                    case 1:
                        yield return Game.Ego.Say(Basement_Res.Also_in_the_box_are_some_old_drug_prescriptions_for_me);
                        yield return Game.Ego.Say(Basement_Res.What_were_they_about);
                        yield return Game.Ego.StartUse();
                        Game.Ego.Inventory.AddItem<DrugPrescriptionRyan>();
                        yield return Game.Ego.StopUse();
                        yield return Tree.InventoryItems.DrugPrescriptionRyan.LookAt();
                        break;
                    case 2:
                        yield return Game.Ego.Say(Basement_Res.Even_more_guarantee_cards);
                        yield return Game.Ego.Say(Basement_Res.And_a_battery);
                        yield return Game.Ego.StartUse();
                        Game.Ego.Inventory.AddItem<BatteryB>();
                        yield return Game.Ego.StopUse();
                        break;
                    case 3:
                        yield return Game.Ego.Say(Basement_Res.Whats_that_at_the_bottom);
                        yield return Game.Ego.Say(Basement_Res.A_therapy_session_log);
                        yield return Game.Ego.Say(Basement_Res.Cynthia_must_have_been_keeping_them);
                        yield return Game.Ego.StartUse();
                        Game.Ego.Inventory.AddItem<TherapyLog>();
                        yield return Game.Ego.StopUse();
                        break;
                    default:
                        yield return Game.Ego.Say(Basement_Res.Nothing_of_interest);
                        break;
                }

                OpenedCount++;
            }
        }

        IEnumerator LookScript()
        {
            if (!LookedAt)
            {
                yield return Game.Ego.GoTo(this);
                using (Game.CutsceneBlock(false, false))
                {
                    yield return Game.Ego.Say(Basement_Res.This_box_is_for_stuff_I_am_not_sure_about_whether_I_still_need_it_or_not);
                }
                LookedAt = true;
            }

            yield return Game.Ego.StartScript(OpenScript());
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.I_dont_need_to_carry_that_whole_box_around);
            }
        }
    }
}
