using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI.Interaction
{
    [Serializable]
    public abstract class VerbButton : Entity
    {
        public LockedVerb Verb { get; private set; }

        public VerbButton(LockedVerb verb)
        {
            Verb = verb;

            HotspotRectangle
                .Create(this)
                .SetCaption(verb.Text)
                .AddRectangle(verb.ScreenRectangle);

            Transform
                .Create(this)
                .SetZ(InteractionBar.Z + 1);
        }

        public override void OnLoadContent(ContentLoader content)
        {
            // Restore static references from savegame
            foreach (var Verb in Verbs.All)
            {
                if (this.Verb.Equals(Verb))
                {
                    this.Verb = Verb;
                }
            }

            base.OnLoadContent(content);
        }
    }
}
