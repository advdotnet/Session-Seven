using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using SessionSeven.InventoryItems;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class DrillingMachine : Entity
    {
        public DrillingMachine()
        {
            Interaction
                .Create(this)
                .SetPosition(408, 261)
                .SetDirection(Directions8.Up)
                .SetGetInteractionsFn(GetInteractions);

            HotspotRectangle
                .Create(this)
                .SetCaption(Basement_Res.drilling_machine)
                .AddRectangle(411, 154, 21, 22);

            Transform
                .Create(this)
                .SetPosition(426, 152)
                .SetZ(Workbench.Z + 1);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.drillholesaw)
                .SetVisible(false);

            Combinable
                .Create(this);

            Enabled = false;
        }

        public bool BiMetalHoleSawInstalled
        {
            get
            {
                return Get<Sprite>().Visible;
            }
            set
            {
                Get<Sprite>().SetVisible(value);
            }
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Any.Object)
                    .Add(Verbs.Use, UseDrillingMachineWith)
                .For(Tree.InventoryItems.SawKit)
                    .Add(Verbs.Use, UseSawKitScript(), Game.Ego)
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());

        }

        IEnumerator UseSawKitScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                if (BiMetalHoleSawInstalled)
                {
                    yield return Game.Ego.Say(Basement_Res.I_attached_already_one_of_them);
                }
                else
                {
                    yield return Game.Ego.StartUse();
                    Game.PlaySoundEffect(content.audio.drill_click);
                    BiMetalHoleSawInstalled = true;
                    yield return Game.Ego.StopUse();
                }
            }
        }

        Script UseDrillingMachineWith(InteractionContext context)
        {
            return Game.Ego.StartScript(UseDrillingMachineWithScript(context));
        }

        IEnumerator UseDrillingMachineWithScript(InteractionContext context)
        {
            Game.Ego.Turn(this);
            using (Game.CutsceneBlock())
            {
                if (!Tree.Basement.DrillingMachineCable.PluggedIn)
                {
                    yield return Game.Ego.Say(Basement_Res.No_power);
                }
                else
                {
                    if (context.Secondary is ItemBase)
                    {
                        yield return Game.Ego.Say(Basement_Res.I_cant_drill_on_that_while_holding_it_in_my_hands);
                    }
                    else
                    {
                        yield return Game.Ego.Say(Basement_Res.I_dont_need_to_drill_that);
                    }
                }
            }

        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say(Basement_Res.My_drilling_machine);
            }
        }
    }
}
