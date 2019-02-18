using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.InventoryItems;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI.Interaction
{
    [Serializable]
    public class Scene : STACK.Scene
    {
        public Scene()
        {
            this.AutoAddEntities();

            Enabled = true;
            Visible = true;
            Interactive = true;
            DrawOrder = 122;

            InputDispatcher
                .Create(this)
                .SetOnMouseUpFn(OnMouseUp)
                .SetOnKeyUpFn(OnKeyUp)
                .SetOnMouseScrollFn(OnMouseScroll);

            Reset();
        }

        Entity Primary = null;
        Entity Secondary = null;
        LockedVerb Verb = Verbs.Walk;

        public void Reset()
        {
            Primary = null;
            Secondary = null;
            Verb = Verbs.Walk;
        }

        public LockedVerb SelectedVerb { get { return Verb; } }
        public Entity SelectedPrimary { get { return Primary; } }

        public void SelectVerb(LockedVerb verb)
        {
            Primary = null;
            Secondary = null;
            Verb = verb;
        }

        public void Show()
        {
            Enabled = true;
            Visible = true;
        }

        public void Hide()
        {
            Enabled = false;
            Visible = false;
        }

        public void OnEntityClick(Entity entity, Vector2 mouse, MouseButton button)
        {
            var PrimaryEntityChanged = (Primary != entity);

            if (null == entity)
            {
                return;
            }

            var CombinableComponent = (Primary ?? entity).Get<Combinable>();
            var GivableComponent = (Primary ?? entity).Get<Givable>();
            var Combinable = (null != CombinableComponent && CombinableComponent.IsCombinable);
            var Givable = (null != GivableComponent && GivableComponent.IsGivable);
            var CanTriggerPrimaryAction = !Verb.Ditransitive || (!Combinable && Verb.Equals(Verbs.Use)) || (!Givable && Verb.Equals(Verbs.Give));

            if (null == Primary || CanTriggerPrimaryAction)
            {
                Primary = entity;
            }
            else
            {
                if (Secondary != null && Secondary != entity)
                {
                    Game.Ego.Stop();
                    Reset();
                    return;
                }
                Secondary = entity;
            }

            Tree.GUI.Interaction.ActionTextLabel.Color = Color.Gray;

            if (button == MouseButton.Right && null == Secondary)
            {
                SelectVerb(Verbs.Look);
                Primary = entity;
                TriggerInteraction(entity, Verbs.Look, Game.Ego, mouse, PrimaryEntityChanged);

                return;
            }

            if (Secondary != null || CanTriggerPrimaryAction)
            {
                var getFor = !CanTriggerPrimaryAction ? Primary : Game.Ego;

                // check for specific interaction (getFor -> entity)
                if (TriggerInteraction(entity, Verb, getFor, mouse, PrimaryEntityChanged))
                {
                    return;
                }

                // interaction (entity -> any)
                if (TriggerInteraction(Primary, Verb, Any.Object, mouse, PrimaryEntityChanged))
                {
                    return;
                }

                // any-> any
                Game.Ego.Stop();
                var RandomText = Verb.GetRandomText(World.Get<Randomizer>(), new InteractionContext(Game.Ego, Primary, Secondary, Verb));
                Game.Ego.Say(RandomText);
                Reset();
            }
        }

        private void HandleWalk(STACK.Components.Interaction interaction, Entity entity, Vector2 mouse)
        {
            Game.Ego.Get<Scripts>().Clear();
            Game.Ego.Get<Text>().Clear();
            Game.Ego.Get<Transform>().RemoveState(State.Talking);
            Game.Ego.Get<SpriteCustomAnimation>().StopAnimation();

            if (entity is ItemBase)
            {
                Game.Ego.Get<Transform>().State = State.Idle;
                Reset();
                return;
            }

            if (interaction != null && !interaction.WalkToClickPosition)
            {
                Game.Ego.GoTo(interaction.Position, interaction.Direction, Reset);
            }
            else
            {
                Game.Ego.GoTo(Vector2.Transform(mouse, entity.DrawScene.Get<Camera>().TransformationInverse), Directions8.None, Reset);
            }
        }

        private bool TriggerInteraction(Entity entity, Verb verb, object getFor, Vector2 mouse, bool entityChanged)
        {
            var Interaction = entity.Get<STACK.Components.Interaction>();

            if (Interaction != null)
            {
                var Interactions = Interaction.GetInteractions().GetFor(getFor);
                InteractionFn Fn;

                if (Interactions.TryGetValue(verb, out Fn))
                {
                    if (entity is ItemBase)
                    {
                        Game.Ego.Stop();
                    }
                    else
                    {
                        Game.Ego.Get<Scripts>().Clear();
                    }
                    Fn(new InteractionContext(Game.Ego, Primary, Secondary, verb));
                    if (!World.Interactive)
                    {
                        DoUpdateLabel = true;
                    }
                    return true;
                }
            }

            if (Verbs.Walk.Equals(Verb))
            {
                if (entityChanged)
                {
                    HandleWalk(Interaction, entity, mouse);
                }

                return true;
            }

            return false;
        }

        bool DoUpdateLabel = false;
        public readonly Rectangle ClickableRegion = new Rectangle(0, 0, Game.VIRTUAL_WIDTH, InteractionBar.HEIGHT);

        private bool IsNullOrInteractionbar(Entity entity)
        {
            return (null == entity ||
                Tree.GUI.Interaction.InteractionBar == entity);
        }

        private void OnKeyUp(Keys obj)
        {
            var Verb = KeyMapping.GetByKey(obj);
            if (null != Verb)
            {
                Game.Ego.Stop();
                SelectVerb(Verb);
            }
        }

        public void OnMouseUp(Vector2 position, MouseButton button)
        {
            var OUM = World.Get<STACK.Components.Mouse>().ObjectUnderMouse;
            var isNullOrInteractionBar = IsNullOrInteractionbar(OUM);

            if (isNullOrInteractionBar)
            {
                if (MouseButton.Left == button)
                {
                    if (!Game.Ego.Inventory.Visible || ClickableRegion.Contains(position))
                    {
                        Game.Ego.Get<Scripts>().Clear();
                        Game.Ego.GoTo(Vector2.Transform(position, Game.Ego.DrawScene.Get<Camera>().TransformationInverse), Directions8.None, Reset);
                        Reset();
                    }
                }
                else
                {
                    Game.Ego.Stop();
                    Reset();
                }
            }
            else if (OUM is VerbButton)
            {
                Game.Ego.Stop();
                SelectVerb(((VerbButton)OUM).Verb);
            }
            else if (OUM == Tree.GUI.Interaction.ScrollUpButton)
            {
                Game.Ego.Inventory.ScrollBy(-1);
            }
            else if (OUM == Tree.GUI.Interaction.ScrollDownButton)
            {
                Game.Ego.Inventory.ScrollBy(+1);
            }
            else
            {
                OnEntityClick(OUM, position, button);
            }
        }

        private void OnMouseScroll(Vector2 position, int diff)
        {
            if (diff < 0)
            {
                Game.Ego.Inventory.ScrollBy(+1);
            }
            else
            {
                Game.Ego.Inventory.ScrollBy(-1);
            }
        }

        public void UpdateLabel()
        {
            DoUpdateLabel = true;
        }

        public override void OnUpdate()
        {
            Tree.GUI.Interaction.Verbs.HighlightedVerb = (Verb.Equals(Verbs.Walk) ? null : Verb);

            if (Tree.GUI.Dialog.Menu.State != Dialog.DialogMenuState.Closed || (!World.Interactive && !DoUpdateLabel))
            {
                base.OnUpdate();

                return;
            }

            DoUpdateLabel = false;

            var Pick = World.Get<STACK.Components.Mouse>().ObjectUnderMouse;
            var Label = Tree.GUI.Interaction.ActionTextLabel;
            Label.Color = Color.White;

            var CombinableComponent = Primary?.Get<Combinable>();
            var GivableComponent = Primary?.Get<Givable>();
            var Combinable = (null != CombinableComponent && CombinableComponent.IsCombinable);
            var Givable = (null != GivableComponent && GivableComponent.IsGivable);

            var PickedScrollButton = (Pick == Tree.GUI.Interaction.ScrollDownButton || Pick == Tree.GUI.Interaction.ScrollUpButton);

            if (Pick is VerbButton && World.Interactive)
            {
                Label.ActionText = Pick.Get<Hotspot>().Caption;
                Tree.GUI.Interaction.Verbs.HighlightedVerb = ((VerbButton)Pick).Verb;
            }
            else if (Primary == null && !PickedScrollButton)
            {
                Label.ActionText = Verb.CreateActionString(Pick);
            }
            else if (Primary != null && (!Verb.Ditransitive || (!Combinable && Verb.Equals(Verbs.Use)) || (!Givable && Verb.Equals(Verbs.Give))))
            {
                // Action being executed
                Label.Color = Color.Gray;
                Label.ActionText = Verb.CreateActionString(Primary, !Verb.Ditransitive);
            }
            else if (Primary != null && Secondary != null)
            {
                Label.Color = Color.Gray;
                Label.ActionText = Verb.CreateActionString(Primary, true, Secondary);
            }
            else if (!PickedScrollButton)
            {
                Label.ActionText = Verb.CreateActionString(Primary, true, Pick);
            }
            else
            {
                Label.ActionText = Verb.CreateActionString(Primary, true);
            }

            base.OnUpdate();
        }
    }
}
