using SessionSeven.Components;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class Cactus : Entity
    {
        public Cactus()
        {
            Interaction
                .Create(this)
                .SetPosition(813, 256)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.cactus)
                .AddRectangle(799, 155, 31, 78);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Talk, TalkScript())
                    .Add(Verbs.Use, TouchScript())
                    .Add(Verbs.Pull, TouchScript())
                    .Add(Verbs.Push, TouchScript());
        }

        IEnumerator TalkScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Talking_to_plants_is_Cynthias_job);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Has_Cynthia_been_watering_this_regularly);
                yield return Game.Ego.Say(Basement_Res.Cacti_are_so_durable);
            }
        }

        int TouchCounter = 0;

        IEnumerator TouchScript()
        {
            const int COUNTER_INSANITY_THRESHOLD = 3;

            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Use();
                yield return Game.Ego.Say(Basement_Res.Ouch);
                TouchCounter++;

                if (COUNTER_INSANITY_THRESHOLD == TouchCounter)
                {
                    Game.Ego.Get<Score>().Add(ScoreType.Insanity, 1);
                }
            }
        }
    }
}
