using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;

namespace SessionSeven.Basement
{
    [Serializable]
    public class Boiler : Entity
    {
        public Boiler()
        {
            Interaction
                .Create(this)
                .SetPosition(994, 268)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption("boiler")
                .AddRectangle(961, 32, 62, 131);

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
                yield return Game.Ego.Say("The boiler that keeps our hot water running.");
                yield return Delay.Seconds(0.5f);
                yield return Game.Ego.Say("Man, I could use a hot shower right about now.");
            }
        }
    }
}
