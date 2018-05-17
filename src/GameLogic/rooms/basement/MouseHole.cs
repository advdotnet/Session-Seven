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
    public class MouseHole : Entity
    {
        [NonSerialized]
        Path _WatchArea;
        public bool FedMouse { get; private set; }
        public bool SawMouse { get; private set; }

        public Path WatchArea
        {
            get
            {
                return _WatchArea;
            }
        }

        private int UpdatesSinceLastBlink = 0;

        public MouseHole()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.eyes, 2)
                .SetFrame(2);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.mouse_hole)
                .SetRectangle(779, 216, 11, 10);

            Transform
                .Create(this)
                .SetPosition(780, 217)
                .SetZ(1);

            Interaction
                .Create(this)
                .SetPosition(784, 243)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Enabled = false;
        }

        public override void OnInitialize(bool restore)
        {
            base.OnInitialize(restore);
            _WatchArea = CreateWatchArea();
        }

        public bool EyesVisible
        {
            get
            {
                return Get<Sprite>().CurrentFrame == 1;
            }
            set
            {
                Get<Sprite>().CurrentFrame = value ? 1 : 2;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!Game.Ego.Inventory.HasItem<InventoryItems.Hazelnuts>())
            {
                EyesVisible = false;
                return;
            }

            var PlayerPosition = Game.Ego.Get<Transform>().Position;

            EyesVisible = WatchArea.Contains(PlayerPosition) && UpdatesSinceLastBlink > 0 && !Game.Mouse.Visible;

            var RandomThreshold = 250 + World.Get<Randomizer>().CreateInt(100) +
                World.Get<Randomizer>().CreateInt(100) +
                World.Get<Randomizer>().CreateInt(100);

            if (UpdatesSinceLastBlink > RandomThreshold)
            {
                EyesVisible = false;
                var BlinkDuration = World.Get<Randomizer>().CreateInt(3, 10);
                UpdatesSinceLastBlink = -BlinkDuration;
            }

            if (Tree.Basement.Drone.Flying)
            {
                EyesVisible = false;
            }

            UpdatesSinceLastBlink++;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.InventoryItems.Flashlight)
                    .Add(Verbs.Use, UseFlashlightScript(), Game.Ego)
                .For(Tree.InventoryItems.Hammer)
                    .Add(Verbs.Use, AttackScript(), Game.Ego)
                .For(Tree.InventoryItems.Crowbar)
                    .Add(Verbs.Use, AttackScript(), Game.Ego)
                .For(Tree.InventoryItems.Scissors)
                    .Add(Verbs.Use, AttackScript(), Game.Ego)
                .For(Tree.InventoryItems.Paperclips)
                    .Add(Verbs.Use, GivePaperclipsScript(), Game.Ego)
                    .Add(Verbs.Give, GivePaperclipsScript(), Game.Ego)
                .For(Tree.InventoryItems.Paperclip)
                    .Add(Verbs.Use, GivePaperclipScript(), Game.Ego)
                    .Add(Verbs.Give, GivePaperclipScript(), Game.Ego)
                .For(Tree.InventoryItems.Hazelnuts)
                    .Add(Verbs.Use, FeedScript(), Game.Ego)
                    .Add(Verbs.Give, FeedScript(), Game.Ego)
                .For(Tree.InventoryItems.BatonWithString)
                    .Add(Verbs.Use, UseBatonWithStringScript(), Game.Ego)
                .For(Tree.InventoryItems.Baton)
                    .Add(Verbs.Use, UseBatonScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Talk, TalkScript())
                    .Add(Verbs.Pick, PickScript());
        }

        IEnumerator GivePaperclipScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (!Tree.Actors.Mouse.Visible)
                {
                    if (SawMouse)
                    {
                        yield return Game.Ego.Use();
                        yield return Delay.Seconds(1);
                        yield return Game.Ego.Say(Basement_Res.It_does_not_seem_to_be_interested);
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.What);
                    }
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.What);
                }
            }
        }

        IEnumerator GivePaperclipsScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.After_all_the_effort_I_wont_put_them_back);
            }
        }

        IEnumerator UseBatonWithStringScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (GotItem)
                {
                    yield return Game.Ego.Say(Basement_Res.There_is_nothing_but_crumbs_in_it);
                    yield break;
                }

                if (!Tree.Actors.Mouse.Visible)
                {
                    if (SawMouse)
                    {
                        yield return Game.Ego.Say(Basement_Res.Not_as_long_as_the_rat_is_in_there);
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.Why_would_I_do_that);
                    }
                }
                else
                {
                    if (SawItem)
                    {

                        yield return Game.Ego.StartUse();
                        if (Tree.Actors.Mouse.Visible)
                        {
                            yield return Delay.Seconds(1);
                            if (Tree.Actors.Mouse.Visible)
                            {
                                Game.Ego.Inventory.AddItem<InventoryItems.Paperclips>();
                                yield return Game.Ego.StopUse();

                                yield return Game.Ego.Say(Basement_Res.Here_we_go);
                                yield return Game.Ego.Say(Basement_Res.A_package_of_paper_clips);
                                yield return Game.Ego.Say(Basement_Res.The_rat_must_love_nibbling_off_the_plastic_wrapping);
                                yield break;
                            }
                        }
                        yield return Game.Ego.StopUse();
                        yield return Game.Ego.Say(Basement_Res.JESUS);
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.Why_would_I_do_that);
                    }
                }
            }
        }

        IEnumerator UseBatonScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (!Tree.Actors.Mouse.Visible)
                {
                    if (SawMouse)
                    {
                        yield return Game.Ego.Say(Basement_Res.Not_as_long_as_the_rat_is_in_there);
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.Why_would_I_do_that);
                    }
                }
                else
                {
                    if (SawItem)
                    {
                        yield return Game.Ego.StartUse();
                        yield return Delay.Seconds(1);
                        yield return Game.Ego.StopUse();

                        if (!Tree.InventoryItems.Baton.Expanded)
                        {
                            yield return Game.Ego.Say(Basement_Res.The_baton_is_too_short_to_reach_the_item_in_the_back);
                        }
                        else
                        {
                            yield return Game.Ego.Say(Basement_Res.I_could_reach_the_item_in_the_back_but_I_couldnt_pull_it_out);
                            yield return Game.Ego.Say(Basement_Res.I_need_a_sling_or_something);
                        }
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.Why_would_I_do_that);
                    }
                }
            }
        }

        IEnumerator UseFlashlightScript()
        {
            if (Tree.InventoryItems.Flashlight.Get<BatteryCompartment>().GetBatteriesCount() != 2)
            {
                Tree.GUI.Interaction.Scene.Reset();
                yield return Game.Ego.Say(Basement_Res.No_power);
                yield break;
            }

            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();
                yield return Delay.Seconds(1);
                bool MouseWasVisible = Tree.Actors.Mouse.Visible;
                if (!SawMouse && !MouseWasVisible)
                {
                    yield return Game.Ego.Say(Basement_Res.JESUS);
                    SawMouse = true;
                }
                yield return Game.Ego.StopUse();

                if (!MouseWasVisible)
                {
                    yield return Game.Ego.Say(Basement_Res.There_is_a_huge_rat_in_there);
                }
                else
                {
                    if (GotItem)
                    {
                        yield return Game.Ego.Say(Basement_Res.Theres_nothing_but_crumbs_in_it);
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.There_are_more_crumbs_in_there_as_well_as_a_small_item_some_way_in);
                        SawItem = true;
                    }
                }
            }
        }

        public bool SawItem { get; private set; }
        public bool GotItem { get; private set; }

        IEnumerator TalkScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (!Game.Ego.Inventory.HasItem<InventoryItems.Hazelnuts>())
                {
                    yield return Game.Ego.Say(Basement_Res.Anybody_home);
                }
                else
                {
                    if (SawMouse)
                    {
                        if (Tree.InventoryItems.Hazelnuts.SawMouseCollectNuts)
                        {
                            yield return Game.Ego.Say(Basement_Res.Are_you_still_hungry);
                        }
                        else
                        {
                            yield return Game.Ego.Say(Basement_Res.Hello_there_little_critter);
                        }
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.Anybody_home);
                    }
                }
            }
        }

        private bool PetKiller = false;

        IEnumerator AttackScript()
        {
            Game.Ego.Turn(this);
            using (Game.CutsceneBlock())
            {
                if (SawMouse)
                {
                    yield return Game.Ego.Say(Basement_Res.I_am_not_a_pet_killer);
                    if (!PetKiller)
                    {
                        Game.Ego.Get<Score>().Add(ScoreType.Jail, 1);
                        PetKiller = true;
                    }
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.Why);
                }
            }
        }

        IEnumerator FeedScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Tree.Actors.Mouse.Visible)
                {
                    yield return Game.Ego.Say(Basement_Res.Nobody_home);
                    yield break;
                }

                // disable entity to control visibility manually
                Tree.Basement.MouseHole.Enabled = false;

                if (!FedMouse)
                {
                    yield return Game.Ego.Say(Basement_Res.You_like_these);
                }
                else
                {
                    var Text = string.Empty;

                    switch (World.Get<Randomizer>().CreateInt(3))
                    {
                        case 0: Text = Basement_Res.Want_some_more; break;
                        case 1: Text = Basement_Res.Still_hungry; break;
                        default: Text = Basement_Res.Have_you_had_enough; break;
                    }

                    yield return Game.Ego.Say(Text);
                }
                yield return Game.Ego.StartUse();
                var Nut = Tree.Basement.NutsOnFloor.AddNut(783, 225);
                yield return Game.Ego.StopUse();
                yield return Delay.Seconds(0.5f);
                yield return Game.Ego.GoTo(660, 280);
                Game.Ego.Turn(this);
                yield return Delay.Seconds(2.5f);

                Tree.Basement.MouseHole.EyesVisible = true;
                yield return Delay.Seconds(0.5f);
                Tree.Basement.NutsOnFloor.RemoveNut(Nut);
                yield return Delay.Seconds(1.5f);
                Game.PlaySoundEffect(content.audio.mouse_eat);
                Tree.Basement.MouseHole.EyesVisible = false;
                yield return Delay.Seconds(0.5f);

                if (!FedMouse)
                {
                    yield return Game.Ego.Say(Basement_Res.He_took_it);
                }

                Tree.Basement.MouseHole.Enabled = true;

                FedMouse = true;
            }
        }

        IEnumerator LookScript()
        {
            Game.Ego.Turn(this);
            using (Game.CutsceneBlock())
            {
                if (!Game.Ego.Inventory.HasItem<InventoryItems.Hazelnuts>())
                {
                    yield return Game.Ego.Say(Basement_Res.We_have_has_problems_with_rats_since_we_moved_in_here_four_years_ago);
                    yield return Game.Ego.Say(Basement_Res.I_hate_these_little_critters);
                }
                else
                {
                    if (Tree.Actors.Mouse.Visible)
                    {
                        yield return Game.Ego.Say(Basement_Res.Its_too_dark_for_me_too_see_anything_inside);
                    }
                    else
                    {
                        if (EyesVisible)
                        {
                            if (Tree.InventoryItems.Hazelnuts.SawMouseCollectNuts)
                            {
                                yield return Game.Ego.Say(Basement_Res.This_is_a_huuge_rat_in_there);
                            }
                            else
                            {
                                if (World.Get<Randomizer>().CreateInt(2) == 0)
                                {
                                    yield return Game.Ego.Say(Basement_Res.Are_there_eyes_staring_at_me);
                                }
                                else
                                {
                                    yield return Game.Ego.Say(Basement_Res.Is_somebody_home);
                                }
                            }

                            SawMouse = true;
                        }
                        else
                        {
                            yield return Game.Ego.Say(Basement_Res.I_feel_like_something_is_watching_me);
                        }
                    }
                }
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (SawMouse)
                {
                    yield return Game.Ego.Say(Basement_Res.Better_not);
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.Who_knows_whats_lurking_in_there);
                }
            }
        }

        Path CreateWatchArea()
        {
            var Points = new PathVertex[7];

            Points[0] = new PathVertex(475, 399);
            Points[1] = new PathVertex(1108, 353);
            Points[2] = new PathVertex(871, 251);
            Points[3] = new PathVertex(821, 218);
            Points[4] = new PathVertex(752, 232);
            Points[5] = new PathVertex(752, 216);
            Points[6] = new PathVertex(1109, 399);

            var Indices = new int[15];
            Indices[0] = 3; Indices[1] = 4; Indices[2] = 5;
            Indices[3] = 0; Indices[4] = 1; Indices[5] = 6;
            Indices[6] = 2; Indices[7] = 3; Indices[8] = 4;
            Indices[9] = 0; Indices[10] = 1; Indices[11] = 4;
            Indices[12] = 1; Indices[13] = 2; Indices[14] = 4;

            return new Path(Points, Indices);
        }
    }
}
