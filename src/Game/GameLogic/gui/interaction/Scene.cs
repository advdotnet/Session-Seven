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

		private Entity _primary = null;
		private Entity _secondary = null;
		private LockedVerb _verb = Verbs.Walk;

		public void Reset()
		{
			_primary = null;
			_secondary = null;
			_verb = Verbs.Walk;
		}

		public LockedVerb SelectedVerb => _verb;
		public Entity SelectedPrimary => _primary;

		public void SelectVerb(LockedVerb verb)
		{
			_primary = null;
			_secondary = null;
			_verb = verb;
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
			var primaryEntityChanged = _primary != entity;

			if (null == entity)
			{
				return;
			}

			var combinableComponent = (_primary ?? entity).Get<Combinable>();
			var givableComponent = (_primary ?? entity).Get<Givable>();
			var combinable = null != combinableComponent && combinableComponent.IsCombinable;
			var givable = null != givableComponent && givableComponent.IsGivable;
			var canTriggerPrimaryAction = !_verb.Ditransitive || (!combinable && _verb.Equals(Verbs.Use)) || (!givable && _verb.Equals(Verbs.Give));

			if (null == _primary || canTriggerPrimaryAction)
			{
				_primary = entity;
			}
			else
			{
				if (_secondary != null && _secondary != entity)
				{
					Game.Ego.Stop();
					Reset();
					return;
				}
				_secondary = entity;
			}

			Tree.GUI.Interaction.ActionTextLabel.Color = Color.Gray;

			if (button == MouseButton.Right && null == _secondary)
			{
				SelectVerb(Verbs.Look);
				_primary = entity;
				TriggerInteraction(entity, Verbs.Look, Game.Ego, mouse, primaryEntityChanged);

				return;
			}

			if (_secondary != null || canTriggerPrimaryAction)
			{
				var getFor = !canTriggerPrimaryAction ? _primary : Game.Ego;

				// check for specific interaction (getFor -> entity)
				if (TriggerInteraction(entity, _verb, getFor, mouse, primaryEntityChanged))
				{
					return;
				}

				// interaction (entity -> any)
				if (TriggerInteraction(_primary, _verb, Any.Object, mouse, primaryEntityChanged))
				{
					return;
				}

				// any-> any
				Game.Ego.Stop();
				var randomText = _verb.GetRandomText(World.Get<Randomizer>(), new InteractionContext(Game.Ego, _primary, _secondary, _verb));
				Game.Ego.Say(randomText);
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
			var interaction = entity.Get<STACK.Components.Interaction>();

			if (interaction != null)
			{
				var interactions = interaction.GetInteractions().GetFor(getFor);

				if (interactions.TryGetValue(verb, out var fn))
				{
					if (entity is ItemBase)
					{
						Game.Ego.Stop();
					}
					else
					{
						Game.Ego.Get<Scripts>().Clear();
					}
					fn(new InteractionContext(Game.Ego, _primary, _secondary, verb));
					if (!World.Interactive)
					{
						UpdateLabel();
					}
					return true;
				}
			}

			if (Verbs.Walk.Equals(_verb))
			{
				if (entityChanged)
				{
					HandleWalk(interaction, entity, mouse);
				}

				return true;
			}

			return false;
		}

		private bool _doUpdateLabel = false;
		public readonly Rectangle ClickableRegion = new Rectangle(0, 0, Game.VIRTUAL_WIDTH, InteractionBar.HEIGHT);

		private bool IsNullOrInteractionbar(Entity entity)
		{
			return null == entity ||
				Tree.GUI.Interaction.InteractionBar == entity;
		}

		private void OnKeyUp(Keys obj)
		{
			var verb = KeyMapping.GetByKey(obj);
			if (null != verb)
			{
				Game.Ego.Stop();
				SelectVerb(verb);
			}
		}

		public void OnMouseUp(Vector2 position, MouseButton button)
		{
			var oum = World.Get<STACK.Components.Mouse>().ObjectUnderMouse;
			var isNullOrInteractionBar = IsNullOrInteractionbar(oum);

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
			else if (oum is VerbButton verbButton)
			{
				Game.Ego.Stop();
				SelectVerb(verbButton.Verb);
			}
			else if (oum == Tree.GUI.Interaction.ScrollUpButton)
			{
				Game.Ego.Inventory.ScrollBy(-1);
			}
			else if (oum == Tree.GUI.Interaction.ScrollDownButton)
			{
				Game.Ego.Inventory.ScrollBy(+1);
			}
			else
			{
				OnEntityClick(oum, position, button);
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
			_doUpdateLabel = true;
		}

		public override void OnUpdate()
		{
			Tree.GUI.Interaction.Verbs.HighlightedVerb = _verb.Equals(Verbs.Walk) ? null : _verb;

			if (Tree.GUI.Dialog.Menu.State != Dialog.DialogMenuState.Closed || (!World.Interactive && !_doUpdateLabel))
			{
				base.OnUpdate();

				return;
			}

			_doUpdateLabel = false;

			var pick = World.Get<STACK.Components.Mouse>().ObjectUnderMouse;
			var label = Tree.GUI.Interaction.ActionTextLabel;
			label.Color = Color.White;

			var combinableComponent = _primary?.Get<Combinable>();
			var givableComponent = _primary?.Get<Givable>();
			var combinable = null != combinableComponent && combinableComponent.IsCombinable;
			var givable = null != givableComponent && givableComponent.IsGivable;

			var pickedScrollButton = pick == Tree.GUI.Interaction.ScrollDownButton || pick == Tree.GUI.Interaction.ScrollUpButton;

			if (pick is VerbButton verbButton && World.Interactive)
			{
				label.ActionText = pick.Get<Hotspot>().Caption;
				Tree.GUI.Interaction.Verbs.HighlightedVerb = verbButton.Verb;
			}
			else if (_primary == null && !pickedScrollButton)
			{
				label.ActionText = _verb.CreateActionString(pick);
			}
			else if (_primary != null && (!_verb.Ditransitive || (!combinable && _verb.Equals(Verbs.Use)) || (!givable && _verb.Equals(Verbs.Give))))
			{
				// Action being executed
				label.Color = Color.Gray;
				label.ActionText = _verb.CreateActionString(_primary, !_verb.Ditransitive);
			}
			else if (_primary != null && _secondary != null)
			{
				label.Color = Color.Gray;
				label.ActionText = _verb.CreateActionString(_primary, true, _secondary);
			}
			else if (!pickedScrollButton)
			{
				label.ActionText = _verb.CreateActionString(_primary, true, pick);
			}
			else
			{
				label.ActionText = _verb.CreateActionString(_primary, true);
			}

			base.OnUpdate();
		}
	}
}
