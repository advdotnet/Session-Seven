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
    public class ToolBar : Entity
    {
        public const float Z = 1;

        public ToolBar()
        {
            Interaction
                .Create(this)
                .SetPosition(517, 264)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.tools)
                .AddRectangle(461, 95, 119, 45);

            Transform
                .Create(this)
                .SetZ(Z);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Pick, LookScript());
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.My_old_tools);
                yield return Game.Ego.Say(Basement_Res.Hang_on_is_one_of_them_missing);
                yield return Game.Ego.Say(Basement_Res.Damn_Ive_told_Landon_a_thousand_times_to_bring_back_anything_he_borrows);

                Tree.Basement.MissingTool.Enabled = true;
                Tree.Basement.Hammer.Enabled = true;
                Tree.Basement.Screwdriver.Enabled = true;
                this.Enabled = false;
            }
        }
    }
}
