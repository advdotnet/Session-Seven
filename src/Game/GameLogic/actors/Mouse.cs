using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using STACK;
using STACK.Components;
using STACK.Logging;
using System;
using System.Collections;

namespace SessionSeven.Actors
{

	[Serializable]
	public class Mouse : Entity
	{
		public static readonly Vector2 STARTPOSITION = new Vector2(783, 227);
		private const string _animationNameSnack = "snack";

		public Mouse()
		{
			Transform
				.Create(this)
				.SetPosition(STARTPOSITION)
				.SetSpeed(120)
				.SetOrientation(-Vector2.UnitX)
				.SetUpdateZWithPosition(true)
				.SetScale(1.0f);

			Sprite
				.Create(this)
				.SetEnableNormalMap(false)
				.SetImage(content.characters.mouse.sprite, 9, 7);

			SpriteTransformAnimation
				.Create(this)
				.SetSetFrameFn(SetFrame);

			SpriteCustomAnimation
				.Create(this)
				.SetGetFramesAction(GetCustomAnimationFrames);

			SpriteData
				.Create(this)
				.SetOrientationFlip(false)
				.SetOffset(-7, -13);

			Navigation
				.Create(this)
				.SetApplyScale(false)
				.SetApplyColoring(false)
				.SetRestrictPosition(true)
				.SetScale(1.0f);

			Scripts
				.Create(this);

			CameraLocked
				.Create(this)
				.SetEnabled(false);

			Visible = false;
			Enabled = false;
		}

		private int SetFrame(Transform transform, int step, int lastFrame)
		{
			if (transform.State.Has(State.Custom))
			{
				return lastFrame;
			}

			var scaledStep = step / 3;
			var result = lastFrame;
			var walking = transform.State.Has(State.Walking);

			// idle
			if (!walking)
			{
				result = (scaledStep % 9) + 1;
			}
			else
			{
				switch (transform.Orientation.ToDirection4())
				{
					case Directions4.Down:
						result = (scaledStep % 6) + (2 * 9) + 1;
						break;
					case Directions4.Right:
						result = -(scaledStep % 6) + (4 * 9) + 1 + 5;
						break;
					case Directions4.Up:
						result = (scaledStep % 6) + (1 * 9) + 1;
						break;
					case Directions4.Left:
						result = (scaledStep % 6) + (3 * 9) + 1;
						break;
				}
			}

			return result;
		}

		private void GetCustomAnimationFrames(Transform transform, string animation, Frames frames)
		{
			const byte columns = 9;
			const byte offset = (columns * 5) + 1;
			const byte totalFrames = columns * 2;

			frames.AddRange(offset, totalFrames);
			frames.AddDelay(3);

			if (_animationNameSnack != animation)
			{
				frames.Clear();
			}
		}

		public bool CollectedNuts { get; private set; }

		private const string _checkForNutsScriptId = "CHECKFORNUTSSCRIPT";

		public Script CheckForNuts()
		{
			Enabled = true;
			if (!Get<Scripts>().HasScript(_checkForNutsScriptId))
			{
				return StartScript(CheckForNutsScript(), _checkForNutsScriptId);
			}

			return Script.None;
		}

		private IEnumerator CheckForNutsScript()
		{
			Get<Transform>().Position = STARTPOSITION;
			Turn(Directions4.Down);

			if (!Tree.InventoryItems.Hazelnuts.SawMouseCollectNuts)
			{
				Game.Ego.Get<CameraLocked>().Enabled = false;
				Get<CameraLocked>().Enabled = true;
			}

			// check if there is a nut in the area the rat can observe
			var nextTargetNut = Tree.Basement.NutsOnFloor.GetNutInWatchArea();

			if (null != nextTargetNut)
			{
				Visible = true;
			}

			while (null != nextTargetNut)
			{
				yield return Delay.Seconds(0.5f);
				var position = nextTargetNut.Get<Transform>().Position + nextTargetNut.INTERACTIONOFFSET;
				yield return GoTo(position);
				yield return Delay.Seconds(0.75f);
				CollectedNuts = true;

				if (Tree.Basement.NutsOnFloor.Nuts.Contains(nextTargetNut))
				{
					Tree.Basement.NutsOnFloor.RemoveNut(nextTargetNut);

					Game.PlaySoundEffect(content.audio.mouse_eat);
					yield return PlayAnimation(_animationNameSnack);

					yield return Delay.Seconds(0.75f);
				}

				// check if there are other nuts
				nextTargetNut = Tree.Basement.NutsOnFloor.GetClosestNut(Get<Transform>().Position);

				if (null == nextTargetNut)
				{
					var goToMouseHoleScript = Get<Scripts>().Start(GoToMouseHole(), _goToMouseHoleScriptId);
					while (!goToMouseHoleScript.Done)
					{
						yield return 1;
						nextTargetNut = Tree.Basement.NutsOnFloor.GetClosestNut(Get<Transform>().Position);
						if (null != nextTargetNut)
						{
							Get<Scripts>().Remove(_goToMouseHoleScriptId);
							Get<Navigation>().RestrictPosition = true;
						}
					}
				}
			}

			yield return Get<Scripts>().Start(GoToMouseHole(), _goToMouseHoleScriptId);

			if (!Tree.InventoryItems.Hazelnuts.SawMouseCollectNuts)
			{
				Game.Ego.Get<CameraLocked>().Enabled = true;
				Get<CameraLocked>().Enabled = false;
			}

			Visible = false;
		}

		private const string _goToMouseHoleScriptId = "gotomouseholescriptid";

		private IEnumerator GoToMouseHole()
		{
			if ((Get<Transform>().Position - STARTPOSITION).Length() > 2)
			{
				yield return GoTo(STARTPOSITION + new Vector2(0, 15));
				Get<Navigation>().RestrictPosition = false;
				yield return GoTo(STARTPOSITION);
				Get<Navigation>().RestrictPosition = true;
			}
		}

		public void Turn(Directions4 direction)
		{
			Get<Transform>().Turn(direction);
		}

		/// <summary>
		/// Makes the player face the given entity based on its Transform's position.
		/// </summary>
		/// <param name="entity"></param>
		public void Turn(Entity entity)
		{
			var transform = entity.Get<Transform>();
			if (null == transform)
			{
				Log.WriteLine("Turn to entity <" + entity.ID + ">: no Transform component avaiable.", LogLevel.Warning);
				return;
			}

			var diffVector = transform.Position - Get<Transform>().Position;

			Turn(diffVector.ToDirection4());
		}

		public Script StartScript(IEnumerator script, string name = "")
		{
			return Get<Scripts>().Start(script, name);
		}

		public Script Say(string text, float duration = 0)
		{
			return Get<Scripts>().Say(text, duration);
		}

		private Script PlayAnimation(string animation, bool looped = false)
		{
			return Get<Scripts>().PlayAnimation(animation, looped);
		}

		public Script GoTo(int x, int y, Directions8 direction = Directions8.None, Action cb = null)
		{
			return GoTo(new Vector2(x, y), direction, cb);
		}

		public Script GoTo(Vector2 position, Directions8 direction = Directions8.None, Action cb = null)
		{
			return Get<Scripts>().GoTo(position, direction, cb);
		}

		public Script GoTo(Interaction interaction, Action cb = null)
		{
			return Get<Scripts>().GoTo(interaction.Position, interaction.Direction, cb);
		}

		public Script GoTo(Entity entity, Directions8 direction = Directions8.None, Action cb = null)
		{
			var interaction = entity.Get<Interaction>();
			if (null != interaction)
			{
				return GoTo(interaction, cb);
			}

			var transform = entity.Get<Transform>();
			if (null != transform)
			{
				return GoTo(transform.Position, direction, cb);
			}

			Log.WriteLine("GoTo entity <" + entity.ID + ">: no Interaction and no Transform component avaiable.", LogLevel.Warning);

			return Script.None;
		}

		public void Stop()
		{
			Get<Scripts>().Clear();
			Get<Transform>().State = State.Idle;
			Get<SpriteCustomAnimation>().StopAnimation();
		}
	}
}
