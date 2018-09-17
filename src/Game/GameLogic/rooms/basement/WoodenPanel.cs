using Microsoft.Xna.Framework;
using SessionSeven.Components;
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
    public class WoodenPanel : Entity
    {
        public const int Z = 1;
        /// <summary>
        /// Colliding circle, z equals radius
        /// </summary>
        public static readonly Circle Collider = new Circle(new Vector2(621, 275), 8);

        public bool Uncovered { get; private set; }

        public WoodenPanel()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.woodenpanel, 3)
                .SetVisible(false);

            HotspotRectangle
                .Create(this)
                .SetRectangle(541, 271, 88, 9)
                .SetCaption(Basement_Res.wooden_panel);

            Transform
                .Create(this)
                .SetPosition(540, 258)
                .SetZ(Z);

            Interaction
                .Create(this)
                .SetPosition(649, 277)
                .SetWalkToClickPosition(true)
                .SetDirection(Directions8.Left)
                .SetGetInteractionsFn(GetInteractions);

            Enabled = false;
            Visible = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.InventoryItems.Hammer)
                    .Add(Verbs.Use, UseHammerScript(), Game.Ego)
                .For(Tree.InventoryItems.Screwdriver)
                    .Add(Verbs.Use, UseScrewdriverScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Use, PickScript())
                    .Add(Verbs.Open, PickScript())
                    .Add(Verbs.Push, PickScript())
                    .Add(Verbs.Pull, PickScript())
                    .Add(Verbs.Pick, PickScript());
        }

        IEnumerator UseHammerScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                var Sprite = Get<Sprite>();
                if (!Sprite.Visible)
                {
                    yield return Game.Ego.StartUse();
                    yield return Game.WaitForSoundEffect(content.audio.hammer_wood);
                    yield return Game.Ego.StopUse();
                    yield return Game.Ego.Say(Basement_Res.Cant_break_it_with_the_hammer);
                }
                else if (Sprite.Visible && Sprite.CurrentFrame == 1)
                {
                    yield return Game.Ego.StartUse();
                    Game.PlaySoundEffect(content.audio.hammer_once);
                    yield return Delay.Seconds(0.25f);
                    Game.PlaySoundEffect(content.audio.open_panel);
                    Sprite.CurrentFrame = 2;
                    Enabled = false;
                    Tree.Basement.WoodenBox.Enabled = true;
                    yield return Game.Ego.StopUse();
                    Game.PlaySoundEffect(content.audio.puzzle);
                    yield return Delay.Seconds(1);
                    yield return Game.Ego.Say(Basement_Res.Where_does_this_horrible_smell_come_from);
                }
                else if (Sprite.Visible && Sprite.CurrentFrame > 1)
                {
                    yield return Game.Ego.Say(Basement_Res.I_levered_the_wooden_panel_out_already);
                }
            }
        }

        IEnumerator UseScrewdriverScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Get<Sprite>().Visible)
                {
                    yield return Game.Ego.Say(Basement_Res.I_levered_the_wooden_panel_out_already);
                    yield break;
                }

                yield return Game.Ego.Say(Basement_Res.I_put_the_screwdriver_in_the_small_gap_between_the_planks);
                yield return Game.Ego.StartUse();
                Game.Ego.Inventory.RemoveItem<InventoryItems.Screwdriver>();
                Game.PlaySoundEffect(content.audio.screwdriver_wood);
                Get<HotspotRectangle>().SetCaption(Basement_Res.screwdriver);
                Get<Sprite>().Visible = true;
                Get<HotspotRectangle>().SetRectangle(579, 258, 15, 22);
                yield return Game.Ego.StopUse();
            }
        }

        public void Uncover()
        {
            Uncovered = true;
            Visible = true;
            Enabled = true;
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (!Get<Sprite>().Visible)
                {
                    yield return Game.Ego.Say(Basement_Res.Both_RFID_antennas_received_a_signal_from_this_wooden_panel);
                }
                else
                {
                    switch (Get<Sprite>().CurrentFrame)
                    {
                        case 1:
                            yield return Game.Ego.Say(Basement_Res.A_screwdriver_is_put_in_the_gap_seperating_the_panels);
                            break;
                        case 2:
                            yield return Game.Ego.Say(Basement_Res.I_levered_out_the_wooden_panel_A_small_wooden_box_is_underneath);
                            break;
                        case 3:
                            yield return Game.Ego.Say(Basement_Res.I_levered_out_the_wooden_panel);
                            break;
                    }
                }
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Get<Sprite>().Visible && 1 == Get<Sprite>().CurrentFrame)
                {
                    yield return Game.Ego.StartUse();
                    Game.Ego.Inventory.AddItem<InventoryItems.Screwdriver>();
                    Get<HotspotRectangle>().SetCaption(Basement_Res.wooden_panel);
                    Get<Sprite>().Visible = false;
                    Get<HotspotRectangle>().SetRectangle(541, 271, 88, 9);
                    yield return Game.Ego.StopUse();
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    yield return Delay.Seconds(1);
                    yield return Game.Ego.StopUse();
                    yield return Game.Ego.Say(Basement_Res.Cant_move_it_with_bare_hands);
                }
            }
        }
    }
}
