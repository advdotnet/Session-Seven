using Microsoft.Xna.Framework;
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
    public class CabinetLeftDoor : Entity
    {
        private readonly Rectangle OPENEDHOTSPOT = new Rectangle(636, 73, 14, 152);
        private readonly Rectangle CLOSEDHOTSPOT = new Rectangle(650, 70, 54, 160);

        private bool FirstOpen = false;

        public CabinetLeftDoor()
        {
            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.left_cabinet_door)
                .SetRectangle(CLOSEDHOTSPOT);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.cabinetleft)
                .SetVisible(false);

            Transform
                .Create(this)
                .SetPosition(637, 73)
                .SetZ(2);

            Interaction
                .Create(this)
                .SetPosition(703, 247)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Enabled = false;
            Open = false;
        }


        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.InventoryItems.Hammer)
                    .Add(Verbs.Use, UseHammerScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Push, MoveScript())
                    .Add(Verbs.Pull, MoveScript())
                    .Add(Verbs.Close, CloseScript())
                    .Add(Verbs.Use, OnUse);
        }

        public bool Open
        {
            get
            {
                return Get<Sprite>().Visible;
            }
            private set
            {
                Get<Sprite>().Visible = value;
            }
        }

        Script OnUse(InteractionContext context)
        {
            IEnumerator Script;

            if (Open)
            {
                Script = CloseScript();
            }
            else
            {
                Script = OpenScript();
            }

            return Game.Ego.StartScript(Script);
        }

        IEnumerator UseHammerScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Tree.Basement.CabinetLock.Visible)
                {
                    yield return Game.Ego.StartUse();
                    yield return Game.WaitForSoundEffect(content.audio.hammer_wood);
                    yield return Game.Ego.StopUse();
                    yield return Game.Ego.Say(Basement_Res.That_didnt_do_anything);
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.I_picked_the_lock_already);
                }
            }
        }

        IEnumerator MoveScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.There_is_no_way_I_could_move_that_cabinet_all_by_myself);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Tree.Basement.CabinetLock.Visible)
                {
                    yield return Game.Ego.Say(Basement_Res.This_cabinet_door_has_a_lock_on_it);
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.The_left_cabinet_door);
                }
            }
        }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Open)
                {
                    yield return Game.Ego.Say(Basement_Res.It_is_open_already);
                }
                else
                {
                    if (Tree.Basement.CabinetLock.Visible)
                    {
                        yield return Game.Ego.Say(Basement_Res.Cant_open_the_door_as_long_as_it_is_locked);
                    }
                    else
                    {
                        yield return Game.Ego.StartUse();
                        yield return Game.WaitForSoundEffect(content.audio.cabinet_open);

                        Open = true;
                        Get<HotspotRectangle>().SetRectangle(OPENEDHOTSPOT);

                        Tree.Basement.CabinetSawKit.Visible = !Game.Ego.Inventory.HasItem<SawKit>();
                        Tree.Basement.CabinetSawKit.Enabled = !Game.Ego.Inventory.HasItem<SawKit>();

                        if (!FirstOpen)
                        {
                            Tree.Basement.Receipt.Visible = true;
                        }

                        yield return Game.Ego.StopUse();

                        if (!FirstOpen)
                        {
                            yield return Game.Ego.StartScript(ReceiptScript(), "receipt");
                            FirstOpen = true;
                        }
                    }
                }
            }
        }

        private IEnumerator ReceiptScript()
        {
            Game.Ego.Inventory.Hide();
            yield return Game.Ego.StartScript(Tree.Basement.Receipt.FallDownScript(), "receipt_fall_down");

            yield return Delay.Seconds(1.5f);
            yield return Game.Ego.GoTo((int)Game.Ego.Get<Transform>().Position.X + 10, (int)Game.Ego.Get<Transform>().Position.Y);
            Game.Ego.Turn(Directions4.Left);
            yield return Game.Ego.Say(Basement_Res.Whats_that);

            yield return Game.Ego.StartUse();
            Tree.Basement.Receipt.Visible = false;
            yield return Game.Ego.StopUse();
            yield return Delay.Seconds(1f);
            yield return Game.Ego.Say(Basement_Res.This_is_an_old_receipt_for_new_tools_we_bought_when_moving_in);
            yield return Delay.Seconds(1f);
            yield return Game.Ego.GoTo(Tree.Basement.MissingTool);
            yield return Game.Ego.Say(Basement_Res.Lets_see);
            yield return Delay.Seconds(1f);

            yield return Game.Ego.Say(Basement_Res.One_hammer_Check_Got_it_with_me);
            yield return Game.Ego.Say(Basement_Res.Two_bench_vises_Check);
            yield return Game.Ego.Say(Basement_Res.Drilling_machine_Check);
            yield return Game.Ego.Say(Basement_Res.Three_screwdrivers_Check_Got_one_of_them_with_me);
            yield return Game.Ego.Say(Basement_Res.Four_wrenches);
            yield return Game.Ego.Say(Basement_Res.Wait_a_minute);

            Tree.Basement.MissingTool.Get<HotspotRectangle>().SetCaption(Basement_Res.missing_wrench);

            yield return Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.Six);
        }

        public IEnumerator CloseScript(bool reset = true)
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock(true, reset))
            {

                if (!Open)
                {
                    yield return Game.Ego.Say(Basement_Res.It_is_closed_already);
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    yield return Game.WaitForSoundEffect(content.audio.cabinet_close);

                    Open = false;
                    Get<HotspotRectangle>().SetRectangle(CLOSEDHOTSPOT);
                    Tree.Basement.CabinetSawKit.Visible = false;
                    Tree.Basement.CabinetSawKit.Enabled = false;

                    yield return Game.Ego.StopUse();
                }
            }
        }
    }
}
