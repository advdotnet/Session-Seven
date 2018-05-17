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
    public class DrawerLeft : Entity
    {
        public DrawerLeft()
        {
            Interaction
                .Create(this)
                .SetPosition(146, 307)
                .SetDirection(Directions8.Left)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.drawer)
                .AddRectangle(81, 210, 21, 26);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.drawerleft, 4, 1)
                .SetFrame(1);

            Transform
                .Create(this)
                .SetPosition(83, 212)
                .SetZ(264);

            Enabled = false;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Open, OpenScript())
                .For(Tree.InventoryItems.DrawerKey)
                    .Add(Verbs.Use, UseDrawerKeyScript(), Game.Ego);
        }

        IEnumerator UseDrawerKeyScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.This_drawer_is_unlocked_already);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.The_drawer_belongs_to_my_wifes_old_desk);
            }
        }

        const float ANIMATIONDELAY = 0.15f;

        private void SetFrame(byte state)
        {
            Get<Sprite>().CurrentFrame = state;
        }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();
                Game.PlaySoundEffect(content.audio.drawer_open);
                yield return Delay.Seconds(ANIMATIONDELAY * 2);
                SetFrame(2);
                yield return Delay.Seconds(ANIMATIONDELAY);
                SetFrame(3);
                yield return Delay.Seconds(ANIMATIONDELAY);
                SetFrame(4);
                yield return Game.Ego.StopUse();
                yield return Game.Ego.Say(Basement_Res.Its_empty);
                yield return Game.Ego.StartUse();

                Game.PlaySoundEffect(content.audio.drawer_close);
                yield return Delay.Seconds(ANIMATIONDELAY * 2);
                SetFrame(3);
                yield return Delay.Seconds(ANIMATIONDELAY * 2);
                SetFrame(2);
                yield return Delay.Seconds(ANIMATIONDELAY * 2);
                SetFrame(1);
                yield return Game.Ego.StopUse();
            }
        }
    }
}
