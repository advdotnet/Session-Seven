using Microsoft.Xna.Framework;
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
    public class CabinetRightDoor : Entity
    {
        public bool Open { get; private set; }
        public bool TookDrone { get; private set; }

        private readonly Rectangle CLOSEDHOTSPOT = new Rectangle(704, 70, 49, 160);
        private readonly Rectangle OPENEDHOTSPOT = new Rectangle(748, 73, 49, 147);

        public CabinetRightDoor()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.cabinetright, 2, 1)
                .SetFrame(1)
                .SetVisible(false);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.right_cabinet_door)
                .SetRectangle(CLOSEDHOTSPOT);

            Transform
                .Create(this)
                .SetPosition(703, 72)
                .SetZ(2);

            Interaction
                .Create(this)
                .SetPosition(703, 247)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            Enabled = false;

            Open = false;
            TookDrone = false;
        }


        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Push, MoveScript())
                    .Add(Verbs.Pull, MoveScript())
                    .Add(Verbs.Close, CloseScript())
                    .Add(Verbs.Use, OnUse);
        }

        public void TakeDrone()
        {
            TookDrone = true;
            Get<Sprite>().CurrentFrame = 2;
        }

        Script OnUse(InteractionContext context)
        {
            IEnumerator Script;

            if (Open)
            {
                Script = CloseScript();
            }
            else
            {
                Script = OpenScript();
            }

            return Game.Ego.StartScript(Script);
        }

        IEnumerator MoveScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.There_is_no_way_I_could_move_that_cabinet_all_by_myself);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Our_old_cabinet);
            }
        }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {

                if (Open)
                {
                    yield return Game.Ego.Say(Basement_Res.It_is_open_already);
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    yield return Game.WaitForSoundEffect(content.audio.cabinet_open);

                    Get<Sprite>().Visible = true;
                    Get<Sprite>().CurrentFrame = (TookDrone) ? 2 : 1;
                    Get<HotspotRectangle>().SetRectangle(OPENEDHOTSPOT);
                    if (!TookDrone)
                    {
                        Tree.Basement.DronePackage.Enabled = true;
                    }
                    Tree.Basement.CabinetSupplies.Enabled = true;
                    Tree.Basement.CabinetScrapbooksRight.Enabled = true;
                    Open = true;
                    yield return Game.Ego.StopUse();
                }
            }
        }

        public IEnumerator CloseScript(bool reset = true)
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock(true, reset))
            {

                if (!Open)
                {
                    yield return Game.Ego.Say(Basement_Res.It_is_closed_already);
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    yield return Game.WaitForSoundEffect(content.audio.cabinet_close);

                    Get<Sprite>().Visible = false;
                    Open = false;
                    Get<HotspotRectangle>().SetRectangle(CLOSEDHOTSPOT);
                    Tree.Basement.DronePackage.Enabled = false;
                    Tree.Basement.CabinetSupplies.Enabled = false;
                    Tree.Basement.CabinetScrapbooksRight.Enabled = false;
                    yield return Game.Ego.StopUse();
                }
            }
        }
    }
}
