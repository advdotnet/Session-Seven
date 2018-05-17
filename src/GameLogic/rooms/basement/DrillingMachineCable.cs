using SessionSeven.Components;
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
    public class DrillingMachineCable : Entity
    {
        public DrillingMachineCable()
        {
            Interaction
                .Create(this)
                .SetPosition(408, 261)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.cable)
                .AddRectangle(387, 224, 28, 13);

            Transform
                .Create(this)
                .SetPosition(234, 203)
                .SetZ(-1.5f);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.cable)
                .SetVisible(false);

            Combinable
                .Create(this);

            Enabled = false;
        }

        public bool PluggedIn
        {
            get
            {
                return Get<Sprite>().Visible;
            }
            set
            {
                Interactive = !value;
                Get<Sprite>().Visible = value;
                Tree.Basement.DrillingMachineCableTop.Get<Sprite>().Visible = value;
                Enabled = !value;
            }
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
                yield return Game.Ego.Say(Basement_Res.My_drilling_machines_cable);
            }
        }
    }

    [Serializable]
    public class DrillingMachineCableTop : Entity
    {
        public DrillingMachineCableTop()
        {
            Transform
                .Create(this)
                .SetPosition(234, 203)
                .SetZ(1f);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.cabletop)
                .SetVisible(false);
        }
    }
}
