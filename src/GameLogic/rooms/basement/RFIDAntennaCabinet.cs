using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;

namespace SessionSeven.Basement
{
    [Serializable]
    public class RFIDAntennaCabinet : Entity
    {
        public readonly Rectangle Collider = new Rectangle(684, 37, 27, 30);

        public RFIDAntennaCabinet()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.rfidantennacabinet, 5);

            HotspotSprite
                .Create(this)
                .SetCaption("cardboard")
                .SetPixelPerfect(true);

            Transform
                .Create(this)
                .SetPosition(688, 49)
                .SetZ(1);

            Interaction
                .Create(this)
                .SetPosition(696, 248)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Scripts
                .Create(this);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.InventoryItems.Drone)
                    .Add(Verbs.Use, UseDroneScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Pick, PickScript());
        }

        bool UsedDrone = false;

        public Script StartFallDownScript()
        {
            return Get<Scripts>().Start(FallDownScript());
        }

        public bool FellDown
        {
            get
            {
                return Get<Sprite>().CurrentFrame > 1;
            }
        }

        IEnumerator FallDownScript()
        {
            yield return Delay.Seconds(0.1f);
            Get<Sprite>().CurrentFrame = 2;
            yield return Delay.Seconds(0.1f);
            Get<Sprite>().CurrentFrame = 3;
            yield return Delay.Seconds(0.1f);
            Get<Sprite>().CurrentFrame = 4;
            yield return Delay.Seconds(0.1f);
            Get<Sprite>().CurrentFrame = 5;
        }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say("I should take it, first.");
            }
        }

        IEnumerator UseDroneScript()
        {
            Game.Ego.Turn(this);
            using (Game.CutsceneBlock())
            {
                if (FellDown)
                {
                    yield return Game.Ego.Say("I should be able to reach it now.");
                    yield return Game.Ego.Say("No need fiddling around with the drone.");
                    yield break;

                }
                if (!UsedDrone)
                {
                    yield return Game.Ego.Say("Maybe I can make the drone bump into the cardboard.");
                    yield return Delay.Seconds(0.5f);
                    UsedDrone = true;
                }
                yield return Game.Ego.GoTo(802, 262);
                Game.Ego.Turn(Directions4.Down);
                yield return Game.Ego.StartUse();
                Game.Ego.Inventory.RemoveItem<InventoryItems.Drone>();
                Tree.Basement.Drone.PlaceOnFloor();
                yield return Game.Ego.StopUse();
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say("A cardboard without any inscription is sitting on a box on the top of the cabinet.");

                if (Game.Ego.Inventory.HasItem<InventoryItems.RFIDAntennaBoxShelf>() ||
                    Game.Ego.Inventory.HasItem<InventoryItems.RFIDAntennaShelf>())
                {
                    yield return Game.Ego.Say("Looks exactly like the one I got from the shelf.");
                }
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);

            using (Game.CutsceneBlock())
            {
                if (!FellDown)
                {
                    yield return Game.Ego.Say("Can't reach it with bare hands.");
                }
                else
                {
                    yield return Game.Ego.Say("Now I can only just reach it.");
                    yield return Game.Ego.StartUse();
                    Enabled = false;
                    Game.Ego.Inventory.AddItem<InventoryItems.RFIDAntennaBoxCabinet>();
                    Visible = false;
                    yield return Game.Ego.StopUse();
                }
            }
        }
    }
}
