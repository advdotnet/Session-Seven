using SessionSeven.Actors;
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
    public class MedicalCabinet : Entity
    {
        public MedicalCabinet()
        {
            Interaction
                .Create(this)
                .SetPosition(792, 243)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.medical_cabinet)
                .AddRectangle(765, 81, 64, 51);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.medicalcabinet, 2)
                .SetVisible(false);

            Transform
                .Create(this)
                .SetZ(2)
                .SetPosition(771, 85);
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Pick, PickScript())
                    .Add(Verbs.Use, OpenScript())
                    .Add(Verbs.Close, CloseScript())
                    .Add(Verbs.Open, OpenScript());
        }

        IEnumerator CloseScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Its_shut_already);
            }
        }

        IEnumerator PickScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.Its_fixed_to_the_wall);
            }
        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.A_medical_cabinet_is_hanging_on_the_wall);
            }
        }

        public bool Opened { get; private set; }

        Sprite Sprite
        {
            get
            {
                return Get<Sprite>();
            }
        }

        IEnumerator OpenScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (Tree.Basement.CabinetRightDoor.Open)
                {
                    yield return Game.Ego.StartScript(Tree.Basement.CabinetRightDoor.CloseScript(false));
                    World.Interactive = false;
                    yield return Game.Ego.GoTo(this);
                }

                yield return Game.Ego.StartUse();

                Game.PlaySoundEffect(content.audio.drawer_open);
                yield return Delay.Seconds(0.5f);

                Sprite.Visible = true;

                Sprite.CurrentFrame = 1;
                yield return Delay.Seconds(0.25f);

                Sprite.CurrentFrame = 2;
                yield return Delay.Seconds(0.5f);

                yield return Game.Ego.StopUse();

                if (Opened)
                {
                    yield return Game.Ego.Say(Basement_Res.Nothing_of_interest);
                }
                else
                {
                    Opened = true;
                    Sprite.SetImage(content.rooms.basement.medicalcabinet_blood, 2);
                    Tree.Basement.MedicalCabinetBloodStain.Get<Sprite>().Visible = true;
                    yield return Game.Ego.Say(Basement_Res.There_are_some_bandages_and_scissors_in_it);

                    yield return Game.Ego.StartUse();
                    Game.Ego.Inventory.AddItem<InventoryItems.Scissors>();
                    Game.Ego.Inventory.AddItem<InventoryItems.Bandages>();
                    yield return Game.Ego.StopUse();

                    Game.Ego.Get<BloodDropEmitter>().ResetCommentCounter();
                }

                yield return Game.Ego.StartUse();
                Game.PlaySoundEffect(content.audio.drawer_close);

                yield return Delay.Seconds(0.75f);

                Sprite.CurrentFrame = 1;

                yield return Delay.Seconds(0.5f);
                Sprite.Visible = false;

                yield return Game.Ego.StopUse();

                yield return Delay.Seconds(0.5f);
            }
        }
    }
}
