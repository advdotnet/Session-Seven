using Microsoft.Xna.Framework;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class CabinetScrapbooksLeft : CabinetScrapbooksBase
    {
        protected override Rectangle Hotspot
        {
            get { return new Rectangle(649, 79, 49, 26); }
        }

        protected override Vector2 InteractionPosition
        {
            get { return new Vector2(703, 247); }
        }
    }

    [Serializable]
    public class CabinetScrapbooksRight : CabinetScrapbooksBase
    {
        protected override Rectangle Hotspot
        {
            get { return new Rectangle(703, 80, 43, 26); }
        }

        protected override Vector2 InteractionPosition
        {
            get { return new Vector2(703, 247); }
        }
    }

    [Serializable]
    public abstract class CabinetScrapbooksBase : Entity
    {
        protected abstract Vector2 InteractionPosition { get; }
        protected abstract Rectangle Hotspot { get; }

        public CabinetScrapbooksBase()
        {
            Interaction
                .Create(this)
                .SetPosition(InteractionPosition)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Transform
                .Create(this)
                .SetZ(3);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.scrapbooks)
                .AddRectangle(Hotspot);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Use, LookScript())
                    .Add(Verbs.Pick, PickScript());
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Cynthia_used_to_be_really_into_making_scrapbooks_for_every_year_when_Landon_was_growing_up);
                yield return Game.Ego.Say(Basement_Res.I_guess_I_hadnt_really_noticed_she_wasnt_doing_it_any_more);
                yield return Game.Ego.Say(Basement_Res.I_cant_quite_bring_myself_to_look_through_them_right_now);
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.I_cant_quite_bring_myself_to_look_through_them_right_now);
            }
        }
    }
}
