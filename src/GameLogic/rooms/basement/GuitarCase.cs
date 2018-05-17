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
    public class GuitarCase : Entity
    {
        public const float Z = 1;

        public GuitarCase()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.guitarcaseopen)
                .SetVisible(false);

            Transform
                .Create(this)
                .SetPosition(128, 129)
                .SetZ(Z);

            Interaction
                .Create(this)
                .SetPosition(163, 243)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.guitar_case)
                .AddRectangle(144, 134, 39, 93);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Use, UseScript())
                    .Add(Verbs.Close, CloseScript())
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator UseScript()
        {
            if (Get<Sprite>().Visible)
            {
                yield return Game.Ego.StartScript(CloseScript());
            }
            else
            {
                yield return Game.Ego.StartScript(OpenScript());
            }
        }

        IEnumerator CloseScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (!Get<Sprite>().Visible)
                {
                    yield return Game.Ego.Say(Basement_Res.The_guitar_case_is_closed_already);
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    Game.PlaySoundEffect(content.audio.guitar_case_close);
                    Get<Sprite>().Visible = false;
                    Tree.Basement.Guitar.Visible = false;
                    yield return Game.Ego.StopUse();
                }
            }
        }

        bool Opened = false;

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Get<Sprite>().Visible)
                {
                    yield return Game.Ego.Say(Basement_Res.The_guitar_case_is_open_already);
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    Game.PlaySoundEffect(content.audio.guitar_case_open);
                    Get<Sprite>().Visible = true;
                    Tree.Basement.Guitar.Visible = true;
                    if (!Opened)
                    {
                        yield return Delay.Seconds(0.5f);
                        Game.PlaySoundEffect(content.audio.guitar_case_close);
                        Tree.Basement.GuitarStrings.Visible = true;
                    }
                    yield return Game.Ego.StopUse();
                }

                if (!Opened)
                {
                    yield return Game.Ego.Say(Basement_Res.A_guitar_string_package_dropped_out);
                    Opened = true;
                }
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Its_my_old_guitar_I_used_to_play_every_night_as_Landon_was_going_to_bed);
                yield return Game.Ego.Say(Basement_Res.I_didnt_know_it_was_down_here_now);
            }
        }
    }
}
