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
    public class FamiliyPortrait : Entity
    {
        public FamiliyPortrait()
        {
            Interaction
                .Create(this)
                .SetPosition(203, 251)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.familyportraitshattered)
                .SetVisible(false);

            Transform
                .Create(this)
                .SetZ(4)
                .SetPosition(202, 89);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.family_portrait)
                .AddRectangle(201, 88, 51, 40);

            Enabled = false;
        }

        bool Shattered
        {
            get { return Get<Sprite>().Visible; }
            set { Get<Sprite>().Visible = value; }
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Tree.InventoryItems.Crowbar)
                    .Add(Verbs.Use, ShatterScript(), Game.Ego)
                .For(Tree.InventoryItems.Hammer)
                    .Add(Verbs.Use, ShatterScript(), Game.Ego)
                .For(Tree.InventoryItems.Baton)
                    .Add(Verbs.Use, ShatterScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Pick, PickScript())
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Push, MoveScript())
                    .Add(Verbs.Pull, MoveScript());
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Its_fine_where_it_is);
            }
        }

        IEnumerator ShatterScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Shattered)
                {
                    yield return Game.Ego.Say(Basement_Res.The_picture_frames_glass_is_shattered_already);
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    Game.PlaySoundEffect(content.audio.glass_shatter);
                    Game.Ego.Get<Score>().Add(ScoreType.Insanity, 1);
                    Shattered = true;
                    yield return Game.Ego.StopUse();
                }
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Its_an_old_photograph_of_Cynthia_Landon_and_me);

                yield return Delay.Seconds(0.3f);
                if (Game.Ego.Get<Score>().HasScore())
                {
                    switch (Game.Ego.Get<Score>().GetScoreTypeResult())
                    {
                        case ScoreType.Freedom:
                            switch (World.Get<Randomizer>().CreateInt(2))
                            {
                                case 0:
                                    yield return Game.Ego.Say(Basement_Res.Those_were_good_times);
                                    break;
                                case 1:
                                    yield return Game.Ego.Say(Basement_Res.We_look_happy_there);
                                    break;
                            }
                            break;
                        case ScoreType.Insanity:
                            yield return Game.Ego.Say(Basement_Res.I_hate_how_my_smile_doesnt_look_real_in_this_picture);
                            break;
                        case ScoreType.Jail:
                            yield return Game.Ego.Say(Basement_Res.Sometimes_I_wonder_why_Landon_prefers_to_stand_next_to_Cynthia_when_taking_pictures);
                            break;
                    }
                }

                if (Shattered)
                {
                    yield return Game.Ego.Say(Basement_Res.The_picture_frames_glass_is_shattered);
                }
            }
        }

        IEnumerator MoveScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.There_is_nothing_behind);
            }
        }
    }
}
