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
    public class SocketsLeft : Entity
    {
        public SocketsLeft()
        {
            Interaction
                .Create(this)
                .SetPosition(233, 251)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.sockets)
                .AddRectangle(229, 198, 11, 12);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                .For(Tree.Basement.DrillingMachine)
                    .Add(Verbs.Use, UseDrillingMachineCableScript(), Game.Ego)
                .For(Tree.Basement.DrillingMachineCable)
                    .Add(Verbs.Use, UseDrillingMachineCableScript(), Game.Ego);
        }

        IEnumerator UseDrillingMachineCableScript()
        {
            yield return Game.Ego.GoTo(Tree.Basement.DrillingMachineCable);
            using (Game.CutsceneBlock())
            {
                if (!Tree.Basement.DrillingMachineCable.PluggedIn)
                {
                    yield return Game.Ego.Use();
                    yield return Game.Ego.GoTo(this);
                    yield return Game.Ego.StartUse();
                    Game.PlaySoundEffect(content.audio.socket_plugin_in);
                    Tree.Basement.DrillingMachineCable.PluggedIn = true;
                    yield return Game.Ego.StopUse();
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.Its_already_plugged_in);
                }
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.These_sockets_are_working_actually);
            }
        }
    }
}
