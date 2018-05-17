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
    public class Door : Entity
    {
        public Door()
        {
            Interaction
                .Create(this)
                .SetPosition(360, 239)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.door)
                .AddRectangle(318, 79, 65, 150);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.door, 3)
                .SetVisible(false);

            Transform
                .Create(this)
                .SetZ(1)
                .SetPosition(318, 80);

            Enabled = false;
        }

        public bool Drilled
        {
            get
            {
                return Get<Sprite>().Visible;
            }

            set
            {
                Get<Sprite>().SetVisible(value);
            }
        }

        const int OPENED_FRAME = 3;

        public bool Open
        {
            get
            {
                return Get<Sprite>().Visible && OPENED_FRAME == Get<Sprite>().CurrentFrame;
            }
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.Basement.DrillingMachine)
                    .Add(Verbs.Use, UseDrillingMachineScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Walk, WalkScript())
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Close, CloseScript())
                    .Add(Verbs.Use, OpenScript())
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Talk, TalkScript())
                .For(Tree.InventoryItems.Hammer)
                    .Add(Verbs.Use, UseHammerScript(), Game.Ego)
                .For(Tree.InventoryItems.DrawerKey)
                    .Add(Verbs.Use, UseDrawerKeyScript(), Game.Ego)
                .For(Tree.InventoryItems.Screwdriver)
                    .Add(Verbs.Use, UseScrewdriverScript(), Game.Ego)
                .For(Tree.InventoryItems.Paperclip)
                    .Add(Verbs.Use, UsePaperclipScript(), Game.Ego)
                .For(Tree.InventoryItems.Paperclips)
                    .Add(Verbs.Use, UsePaperclipScript(), Game.Ego)
                .For(Tree.InventoryItems.Scissors)
                    .Add(Verbs.Use, UseScissorsScript(), Game.Ego);
        }

        private IEnumerator UseHammerScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Open)
                {
                    yield return Game.Ego.Say(Basement_Res.The_door_is_open_already);
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    yield return Game.WaitForSoundEffect(content.audio.hammer_metal);
                    yield return Game.Ego.StopUse();
                    yield return Game.Ego.Say(Basement_Res.That_didnt_do_anything);
                }
            }
        }

        private IEnumerator UseScissorsScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Open)
                {
                    yield return Game.Ego.Say(Basement_Res.The_door_is_open_already);
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.I_cant_cut_the_door_open_with_a_pair_of_scissors);
                }
            }
        }

        IEnumerator UsePaperclipScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Open)
                {
                    yield return Game.Ego.Say(Basement_Res.The_door_is_open_already);
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.Cant_exert_enough_force_on_this_type_of_lock);
                }
            }
        }

        IEnumerator WalkScript()
        {
            yield return Game.Ego.GoTo(this);

            if (Open)
            {
                using (Game.CutsceneBlock())
                {
                    yield return Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.Seven);
                }
            }

            Tree.GUI.Interaction.Scene.Reset();
        }

        IEnumerator UseDrillingMachineScript()
        {
            yield return Game.Ego.GoTo(Tree.Basement.DrillingMachine);
            using (Game.CutsceneBlock())
            {
                if (!Tree.Basement.DrillingMachineCable.PluggedIn)
                {
                    yield return Game.Ego.Say(Basement_Res.No_power);
                }
                else
                {
                    if (!Drilled)
                    {
                        yield return Game.Ego.StartUse();
                        yield return Game.Ego.StopUse();

                        yield return Game.Ego.GoTo(this);

                        Tree.Basement.DrillingMachineUsed.Show();

                        yield return Game.Ego.StartUse();
                        yield return Game.WaitForSoundEffect(content.audio.drill);

                        if (Tree.Basement.DrillingMachine.BiMetalHoleSawInstalled)
                        {
                            Drilled = true;
                        }

                        yield return Game.Ego.StopUse();

                        Game.Ego.Turn(Directions4.Right);

                        yield return Game.Ego.StartUse();
                        Tree.Basement.DrillingMachineUsed.Hide();
                        yield return Game.Ego.StopUse();

                        if (!Tree.Basement.DrillingMachine.BiMetalHoleSawInstalled)
                        {
                            yield return Game.Ego.Say(Basement_Res.This_didnt_do_anything_besides_heating_the_drill_up_It_almost_glowed);
                        }
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.No_need_to_drill_any_more);
                    }
                }
            }

        }

        IEnumerator UseDrawerKeyScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Use();
                yield return Game.Ego.Say(Basement_Res.This_key_does_not_fit);
            }
        }

        IEnumerator UseScrewdriverScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.There_are_no_screws_visible);
            }
        }

        IEnumerator CloseScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Open)
                {
                    yield return Game.Ego.Say(Basement_Res.No_I_spent_so_much_time_trying_to_escape_from_here);
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.This_door_is_closed_already);
                }
            }
        }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Drilled)
                {
                    if (Open)
                    {
                        yield return Game.Ego.Say(Basement_Res.The_door_is_open_already);
                        yield break;
                    }
                    else
                    {
                        yield return Game.Ego.StartUse();
                        Game.PlaySoundEffect(content.audio.door_open);
                        yield return Delay.Seconds(0.5f);
                        Get<Sprite>().CurrentFrame = 2;
                        yield return Delay.Seconds(0.5f);
                        Get<Sprite>().CurrentFrame = 3;
                        yield return Game.Ego.StopUse();
                        yield break;
                    }
                }

                yield return Game.Ego.StartUse();
                Game.PlaySoundEffect(content.audio.door_knob);
                yield return Delay.Seconds(1);
                yield return Game.Ego.StopUse();

                yield return Delay.Seconds(0.5f);

                yield return Game.Ego.StartUse();
                Game.PlaySoundEffect(content.audio.door_knock);
                yield return Delay.Seconds(1);
                yield return Game.Ego.StopUse();

                yield return Delay.Seconds(1.5f);

                yield return Game.Ego.Say(Basement_Res.Argh_this_shouldnt_be_locked);
                yield return Delay.Seconds(.25f);
                yield return Game.Ego.Say(Basement_Res.Hello);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.This_is_a_massive_steel_door);
            }
        }

        IEnumerator TalkScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.HELLO_IS_ANYBODY_THERE);
                yield return Delay.Seconds(3);
            }
        }
    }
}
