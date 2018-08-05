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
    public class SocketsCenter : Entity
    {
        public SocketsCenter()
        {
            Interaction
                .Create(this)
                .SetPosition(543, 262)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.sockets)
                .AddRectangle(538, 197, 13, 15);

            Transform
                .Create(this)
                .SetZ(Workbench.Z + 1);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                .For(Tree.Basement.DrillingMachine)
                    .Add(Verbs.Use, LookScript(), Game.Ego)
                .For(Tree.Basement.DrillingMachineCable)
                    .Add(Verbs.Use, LookScript(), Game.Ego);
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.These_sockets_have_been_defective_since_we_moved_in);
                yield return Game.Ego.Say(Basement_Res.I_never_found_the_time_to_deal_with_them);
            }
        }
    }
}
