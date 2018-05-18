using Microsoft.Xna.Framework;
using SessionSeven.Basement;
using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class RFIDAntennaCabinet : RFIDAntenna
    {
        protected override RFIDAntennaFloor AntennaFloor
        {
            get
            {
                return Tree.Basement.RFIDAntennaFloorCabinet;
            }
        }

        protected override RFIDAntenna OtherAntennaInv
        {
            get
            {
                return Tree.InventoryItems.RFIDAntennaShelf;
            }
        }
    }

    [Serializable]
    public class RFIDAntennaShelf : RFIDAntenna
    {
        protected override RFIDAntennaFloor AntennaFloor
        {
            get
            {
                return Tree.Basement.RFIDAntennaFloorShelf;
            }
        }

        protected override RFIDAntenna OtherAntennaInv
        {
            get
            {
                return Tree.InventoryItems.RFIDAntennaCabinet;
            }
        }
    }

    [Serializable]
    public abstract class RFIDAntenna : ItemBase
    {
        protected abstract RFIDAntenna OtherAntennaInv { get; }
        protected abstract RFIDAntennaFloor AntennaFloor { get; }

        bool SelectionAborted = false;

        public bool Placed { get; private set; }

        public bool HitCollider
        {
            get
            {
                return Placed && AntennaFloor.Get<TracerLine>().HitCollider;
            }
        }

        public RFIDAntenna() : base(content.inventory.rfidantenna, Items_Res.RFIDAntenna_RFIDAntenna_RFIDAntenna, true, false)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Use, UseScript(true, false))
                    .Add(Verbs.Look, LookScript());
        }

        public IEnumerator UseScript(bool reset, bool doCheck)
        {
            using (Game.CutsceneBlock(reset))
            {
                if (Tree.Basement.WoodenPanel.Uncovered)
                {
                    yield return Game.Ego.Say(Items_Res.I_triangulated_the_signal_already);
                    yield break;
                }

                // place
                yield return Game.Ego.StartScript(PlaceScript(AntennaFloor, RFIDAntennaFloor.POSITIONMODE_POSITION));

                if (AntennaFloor.WasPlacedTooClose)
                {
                    yield return Game.Ego.Say(Items_Res.I_need_to_place_the_other_antenna_with_more_distance_for_the_triangulation_to_work);
                }
                else
                {
                    if (!SelectionAborted)
                    {
                        Placed = true;
                        // rotate
                        yield return Game.Ego.StartScript(PlaceScript(AntennaFloor, RFIDAntennaFloor.POSITIONMODE_ROTATION));
                    }

                    if (!SelectionAborted)
                    {
                        if (HitCollider)
                        {
                            yield return Game.Ego.Say(Items_Res.The_antenna_is_receiving_a_signal_somewhere_along_that_direction);

                            if (null != OtherAntennaInv)
                            {
                                if (!OtherAntennaInv.Placed)
                                {
                                    yield return Game.Ego.Say(Items_Res.Now_the_other_antenna);

                                    yield return Game.Ego.StartScript(OtherAntennaInv.UseScript(false, true));
                                }

                                if (doCheck && OtherAntennaInv.HitCollider)
                                {
                                    Game.Ego.Turn(Tree.Basement.WoodenPanel);
                                    Tree.Basement.WoodenPanel.Uncover();
                                    yield return Game.Ego.Say(Items_Res.Seems_like_the_signal_is_coming_from_this_wooden_panel_near_the_workbench);
                                    yield return Game.Ego.Say(Items_Res.There_must_be_a_RFID_receiver_underneath_for_some_reason);
                                }
                            }
                            else
                            {
                                yield return Game.Ego.Say(Items_Res.I_need_another_antenna_to_triangulate_the_signal_and_get_its_position);
                            }
                        }
                        else
                        {
                            yield return Game.Ego.Say(Items_Res.No_signal_along_that_direction);
                        }
                    }
                }

                yield return Game.Ego.GoTo(AntennaFloor);
                yield return Game.Ego.StartUse();
                Placed = false;
                AntennaFloor.Visible = false;
                AntennaFloor.Enabled = false;
                yield return Game.Ego.StopUse();
            }
        }

        public IEnumerator LookScript(bool reset = true)
        {
            using (Game.CutsceneBlock(reset))
            {
                yield return Game.Ego.Say(Items_Res.This_is_a_longrange_RFID_antenna);
                yield return Game.Ego.Say(Items_Res.Must_be_Landons);
            }
        }

        IEnumerator PlaceScript(RFIDAntennaFloor antennaFloor, int mode)
        {
            SelectionAborted = false;
            var Selection = Tree.GUI.PositionSelection.Scene;

            Tree.GUI.Interaction.Scene.Interactive = false;
            Tree.GUI.Interaction.Scene.Visible = false;
            Game.Ego.Inventory.Hide();

            Tree.GUI.Mouse.Disable();

            Game.Ego.Get<CameraLocked>().Enabled = false;

            yield return Selection.StartSelectionScript(Game.Ego.Get<Scripts>(), antennaFloor, true, true, mode);

            Game.Ego.Get<CameraLocked>().Enabled = true;

            if (Selection.Aborted)
            {
                SelectionAborted = true;
            }
            else
            {
                if (RFIDAntennaFloor.POSITIONMODE_POSITION == mode)
                {
                    yield return Game.Ego.GoTo(antennaFloor);
                    yield return Game.Ego.Use();
                    antennaFloor.Visible = true;
                    antennaFloor.Enabled = true;
                    yield return Game.Ego.GoTo(antennaFloor.Get<Transform>().Position - new Vector2(0, 5));
                    Game.Ego.Turn(Directions4.Down);
                }
                else
                {
                    if (HitCollider)
                    {
                        antennaFloor.Get<TracerLine>().Visible = true;
                    }
                }
            }

            Tree.GUI.Mouse.Enable();

            Tree.GUI.Interaction.Scene.Interactive = true;
            Tree.GUI.Interaction.Scene.Visible = true;

            Game.Ego.Inventory.Show();
        }
    }
}
