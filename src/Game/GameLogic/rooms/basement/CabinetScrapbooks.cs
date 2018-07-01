using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;

namespace SessionSeven.Basement
{
    [Serializable]
    public class CabinetScrapbooks : Entity
    {
        public CabinetScrapbooks()
        {
            Interaction
                .Create(this)
                .SetPosition(703, 247)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Transform
                .Create(this)
                .SetZ(3);

            HotspotRectangle
                .Create(this)
                .SetCaption("scrapbooks")
                .AddRectangle(703, 80, 43, 26);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Pick, PickScript());
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say("Cynthia used to be really into making scrapbooks for every year when Landon was growing up.");
                yield return Game.Ego.Say("I guess I hadn't really noticed she wasn't doing it any more.");
                yield return Game.Ego.Say("I can't quite bring myself to look through them right now.");
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say("I can't quite bring myself to look through them right now.");
            }
        }
    }
}
