using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;

namespace SessionSeven.Basement
{
    [Serializable]
    public class Folders : Entity
    {
        public Folders()
        {
            Interaction
                .Create(this)
                .SetPosition(205, 242)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption("folders")
                .AddRectangle(189, 148, 63, 45);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say("There are all sorts of different colored folders, full of documents for the new house, old tax files, and various papers for Landon.");
                yield return Game.Ego.Say("I didn't realize how much Cynthia was handling on her own while I was away at work...");
            }
        }
    }
}
