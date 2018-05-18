using Microsoft.Xna.Framework;
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
    public class WindowLeft : Window
    {
        protected override Rectangle GetHotspotRectangle() { return new Rectangle(37, 37, 31, 47); }
        protected override Vector2 GetInteractionPosition() { return new Vector2(92, 241); }
        protected override Directions8 GetInteractionDirection() { return Directions8.Left; }
    }

    [Serializable]
    public class WindowRight : Window
    {
        protected override Rectangle GetHotspotRectangle() { return new Rectangle(1069, 41, 31, 25); }
        protected override Vector2 GetInteractionPosition() { return new Vector2(986, 268); }
        protected override Directions8 GetInteractionDirection() { return Directions8.Right; }
    }

    [Serializable]
    public abstract class Window : Entity
    {
        public Window()
        {
            Interaction
                .Create(this)
                .SetPosition(GetInteractionPosition())
                .SetDirection(GetInteractionDirection())
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.window)
                .AddRectangle(GetHotspotRectangle());

            Enabled = false;
        }

        protected abstract Rectangle GetHotspotRectangle();
        protected abstract Vector2 GetInteractionPosition();
        protected abstract Directions8 GetInteractionDirection();

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.InventoryItems.Flashlight)
                    .Add(Verbs.Use, UseFlashlightScript(), Game.Ego)
                .For(Tree.InventoryItems.Drone)
                    .Add(Verbs.Use, UseDroneScript(), Game.Ego)
                .For(Tree.InventoryItems.Baton)
                    .Add(Verbs.Use, UseToolScript(), Game.Ego)
                .For(Tree.InventoryItems.BatonWithString)
                    .Add(Verbs.Use, UseToolScript(), Game.Ego)
                .For(Tree.InventoryItems.Hammer)
                    .Add(Verbs.Use, UseToolScript(), Game.Ego)
                .For(Tree.InventoryItems.Scissors)
                    .Add(Verbs.Use, UseToolScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, OpenScript())
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Push, OpenScript())
                    .Add(Verbs.Pull, OpenScript())
                    .Add(Verbs.Close, CloseScript())
                    .Add(Verbs.Talk, TalkScript())
                    .Add(Verbs.Use, OpenScript());
        }

        IEnumerator UseToolScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Those_burglar_bars_are_way_to_strong);
            }
        }

        IEnumerator UseFlashlightScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Nobody_will_see_this_in_broad_daylight);
            }
        }

        IEnumerator UseDroneScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.The_drone_wont_fit_through_the_burglar_bars);
            }
        }

        IEnumerator CloseScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Its_shut_already);
            }
        }

        IEnumerator TalkScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                var RandomInt = World.Get<Randomizer>().CreateInt(3);
                var Text = string.Empty;

                switch (RandomInt)
                {
                    case 0: Text = Basement_Res._HELLO; break;
                    case 1: Text = Basement_Res.ANYBODY; break;
                    default: Text = Basement_Res.HELP_ME; break;
                }

                yield return Game.Ego.Say(Text, 2f);
            }
        }

        public bool TriedOpen { get; private set; }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);

            if (!TriedOpen)
            {
                Game.EnableSkipping();
                Game.Ego.Inventory.Hide();
            }

            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Cant_get_through_the_burglar_bars);
                yield return Game.Ego.Say(Basement_Res.Thought_we_got_these_things_to_keep_people_out_not_in);
            }

            if (!TriedOpen)
            {
                Tree.Basement.WindowLeft.TriedOpen = true;
                Tree.Basement.WindowRight.TriedOpen = true;

                yield return Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.Two);
            }
        }
    }
}
