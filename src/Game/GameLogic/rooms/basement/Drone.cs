using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using STACK.Graphics;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{

	[Serializable]
	public enum DroneCommand
	{
		Forward,
		Left,
		Right,
		Off,
		On
	}

	[Serializable]
	public class Drone : Entity
	{
		public readonly Vector2 INITIAL_POSITION = new Vector2(802, 265);
		private const int _initialZ = 263;
		private Directions4 _orientation = Directions4.Down;

		private Directions4 Orientation
		{
			get => _orientation;
			set
			{
				_orientation = value;
				Get<Transform>().SetOrientation(value.ToVector2());
			}
		}

		private readonly Projection2D _projectionFloor = new Projection2D().SetQuadliteral(new Vector2(0 - 40, 375), new Vector2(1103 + 40, 375), new Vector2(999, 227), new Vector2(104, 227));
		private readonly Projection2D _projectionCeiling = new Projection2D().SetQuadliteral(new Vector2(14, 10), new Vector2(1072, 10), new Vector2(985, 45), new Vector2(110, 45));

		//public override void OnDraw(Renderer renderer)
		//{
		//    base.OnDraw(renderer);

		//    if (renderer.Stage == RenderStage.PostBloom && Flying)
		//    {
		//        var Col = new Color(Color.White, 0.25f);

		//        renderer.PrimitivesRenderer.DrawTriangle(new Vector2(0, 375 - 40), new Vector2(1103 + 40, 375), new Vector2(999, 227), Col);
		//        renderer.PrimitivesRenderer.DrawTriangle(new Vector2(0, 375 - 40), new Vector2(999, 227), new Vector2(104, 227), Col);

		//        Col = new Color(Color.Blue, 0.25f);

		//        renderer.PrimitivesRenderer.DrawTriangle(new Vector2(110, 45), new Vector2(985, 45), new Vector2(1072, 10), Col);
		//        renderer.PrimitivesRenderer.DrawTriangle(new Vector2(110, 45), new Vector2(1072, 10), new Vector2(14, 10), Col);

		//        Col = Color.Red;

		//        renderer.PrimitivesRenderer.DrawRectangle(Transform.Position - new Vector2(1, 1), Transform.Position + new Vector2(1, 1), Col);

		//        Col = Color.Blue;

		//        renderer.PrimitivesRenderer.DrawRectangle(DisplacedPosition - new Vector2(1, 1), DisplacedPosition + new Vector2(1, 1), Col);

		//        RestrictionPathCeiling.Draw(renderer.PrimitivesRenderer.DrawTriangle, renderer.PrimitivesRenderer.DrawLine, World.Get<Mouse>().Position);

		//        Col = new Color(Color.Bisque, 0.25f);

		//        renderer.PrimitivesRenderer.DrawRectangle(Tree.Basement.RFIDAntennaCabinet.Collider, Col);
		//    }
		//}

		/// <summary>
		/// Places the drone on the initial floor position, sets its scale and makes it visible & enabled.
		/// </summary>
		public void PlaceOnFloor()
		{
			Transform.SetPosition(INITIAL_POSITION);
			var ceilingPosition = GetCeilingPosition();
			var scale = RestrictionPathCeiling.GetScale(ceilingPosition.Y);
			Transform.SetScale(scale);
			Get<Interaction>().SetPosition(INITIAL_POSITION);
			Transform.SetZ(_initialZ);
			Orientation = Directions4.Down;
			Visible = true;
			Enabled = true;
		}

		public Drone()
		{
			Sprite
				.Create(this)
				.SetGetPositionFn(GetSpritePosition)
				.SetImage(content.rooms.basement.drone, 5, 4);

			SpriteTransformAnimation
				.Create(this)
				.SetSetFrameFn(SetFrame);

			CameraLocked
				.Create(this)
				.SetEnabled(false);

			Transform
				.Create(this)
				.SetPosition(INITIAL_POSITION)
				.SetDirection(Directions4.Down)
				.SetUpdateZWithPosition(false)
				.SetZ(_initialZ);

			SpriteData
				.Create(this)
				.SetOffset(-42, -16);

			HotspotSprite
				.Create(this)
				.SetCaption(Basement_Res.drone)
				.SetPixelPerfect(false);

			Interaction
				.Create(this)
				.SetDirection(Directions8.Down)
				.SetGetInteractionsFn(GetInteractions);

			Scripts
				.Create(this);

			DroneDisplacement
				.Create(this);

			Flying = false;
			Enabled = false;
			Visible = false;
		}

		public override void OnDraw(Renderer renderer)
		{
			base.OnDraw(renderer);
		}

		private Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Use, UseScript())
					.Add(Verbs.Pick, PickScript())
					.Add(Verbs.Look, LookScript());
		}

		private void FaceDrone()
		{
			var diffVector = Transform.Position - Game.Ego.Get<Transform>().Position;
			if (Vector2.Zero != diffVector)
			{
				Game.Ego.Turn(diffVector.ToDirection4());
			}
		}

		private IEnumerator UseScript()
		{
			using (Game.CutsceneBlock())
			{
				FaceDrone();
				yield return Game.Ego.Say(Basement_Res.The_drone_is_operated_by_using_its_remote_control);
			}
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				FaceDrone();
				yield return Game.Ego.Say(Basement_Res.Its_sitting_on_the_floor_ready_to_ascend);
			}
		}

		public IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(Transform.Position);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.StartUse();
				Game.Ego.Inventory.AddItem<InventoryItems.Drone>();
				Visible = false;
				yield return Game.Ego.StopUse();

				Enabled = false;
				Crashed = false;
			}
		}


		public bool Flying { get; private set; }

		private Vector2 _unitPosition;
		private bool _reachedCeiling = false;
		private int _lastScaledStep = -1;

		private int SetFrame(Transform transform, int step, int lastFrame)
		{
			if (!Flying)
			{
				return GetStandingFrame();
			}

			var scaledStep = step / 3;

			if (scaledStep != _lastScaledStep)
			{
				_lastScaledStep = scaledStep;
				var flyingFrames = GetFlyingFrames();

				return flyingFrames.GetRandomExcluding(World.Get<Randomizer>(), lastFrame);
			}

			return lastFrame;
		}

		private readonly Frames _framesFlyingDown = Frames.Create(2, 3, 4, 5);
		private readonly Frames _framesFlyingUp = Frames.Create(7, 8, 9, 10);
		private readonly Frames _framesFlyingRight = Frames.Create(12, 13, 14, 15);
		private readonly Frames _framesFlyingLeft = Frames.Create(17, 18, 19, 20);

		private Frames GetFlyingFrames()
		{
			switch (Orientation)
			{
				case Directions4.Down: return _framesFlyingDown;
				case Directions4.Up: return _framesFlyingUp;
				case Directions4.Right: return _framesFlyingRight;
				default: return _framesFlyingLeft;
			}
		}

		private int GetStandingFrame()
		{
			switch (Orientation)
			{
				case Directions4.Down: return 1;
				case Directions4.Up: return 6;
				case Directions4.Right: return 11;
				default: return 16;
			}
		}

		private Transform Transform => Get<Transform>();

		[NonSerialized]
		private SoundEffectInstance _flyingSoundEffect;

		public IEnumerator ExecuteCommand(DroneCommand command)
		{
			if (!Flying && command != DroneCommand.On)
			{
				Game.PlaySoundEffect(content.audio.drone_failure);
				yield break;
			}

			switch (command)
			{
				case DroneCommand.Forward:
					yield return Get<Scripts>().GoTo(Forward());
					break;

				case DroneCommand.Left:
					TurnLeft();
					break;

				case DroneCommand.Right:
					TurnRight();
					break;

				case DroneCommand.Off:
					StopFlyingSound();
					Game.PlaySoundEffect(content.audio.drone_shutdown);
					yield return Get<Scripts>().GoTo(StopEngine());
					Game.PlaySoundEffect(content.audio.drone_landing);
					break;

				case DroneCommand.On:
					PlayFlyingSound();
					yield return Get<Scripts>().GoTo(StartEngine());
					Get<DroneDisplacement>().Reset();
					_reachedCeiling = true;
					break;
			}

			// if collision
			while (Get<Scripts>().ScriptCollection.Count > 0)
			{
				yield return 1;
			}
		}

		public bool Crashed { get; private set; }

		private Vector2 GetSpritePosition() => _displacedPosition + new Vector2(0, 10);

		private Vector2 _displacedPosition = Vector2.Zero;

		private void PlayFlyingSound()
		{
			_flyingSoundEffect = Game.PlaySoundEffect(content.audio.drone_flying, true);
		}

		private void StopFlyingSound()
		{
			_flyingSoundEffect?.Stop();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (Flying && null == _flyingSoundEffect)
			{
				PlayFlyingSound();
			}

			if (Flying && _reachedCeiling)
			{
				_unitPosition = _projectionCeiling.TransformInverse(Transform.Position);

				var displacedUnitPosition = _unitPosition + (Get<DroneDisplacement>().GetDisplacement() / new Vector2(500f, 80f));
				_displacedPosition = _projectionCeiling.Transform(displacedUnitPosition);

				var scale = RestrictionPathCeiling.GetScale(Transform.Position.Y);
				Transform.SetScale(scale);

				var droneCollided = !RestrictionPathCeiling.Contains(_displacedPosition);

				if (Tree.Basement.RFIDAntennaCabinet.Enabled && Tree.Basement.RFIDAntennaCabinet.Collider.Contains(_displacedPosition) && !Tree.Basement.RFIDAntennaCabinet.FellDown)
				{
					droneCollided = true;
					Tree.Basement.RFIDAntennaCabinet.StartFallDownScript();
				}

				if (droneCollided)
				{
					StopFlyingSound();
					Game.PlaySoundEffect(content.audio.drone_shutdown);
					Crashed = true;
					Get<Scripts>().Clear();
					Get<Scripts>().Start(LandScript());

					// Decrease displacement amplitude = difficulty
					Get<DroneDisplacement>().SetAmplitude(Get<DroneDisplacement>().Amplitude - 2);

					if (Tree.GUI.Dialog.Menu.State == GUI.Dialog.DialogMenuState.Open ||
						Tree.GUI.Dialog.Menu.State == GUI.Dialog.DialogMenuState.Opening)
					{
						Tree.GUI.Dialog.Menu.Close();
					}
				}
			}
			else
			{
				_displacedPosition = Transform.Position;
			}
		}

		private IEnumerator LandScript()
		{
			var landingPosition = StopEngine();
			Transform.SetZ(landingPosition.Y);
			yield return Get<Scripts>().GoTo(landingPosition);
			Game.PlaySoundEffect(content.audio.drone_landing);
		}

		private Vector2 StartEngine()
		{
			Flying = true;
			Crashed = false;
			_reachedCeiling = false;
			_unitPosition = _projectionFloor.TransformInverse(Transform.Position);
			var ceilingPosition = _projectionCeiling.Transform(_unitPosition);
			return ceilingPosition;
		}

		private Vector2 GetCeilingPosition()
		{
			var unitSpacePosition = _projectionFloor.TransformInverse(Transform.Position);
			return _projectionCeiling.Transform(unitSpacePosition);
		}

		private Vector2 StopEngine()
		{
			Flying = false;
			_reachedCeiling = false;
			_unitPosition = _projectionCeiling.TransformInverse(Transform.Position);
			var floorPosition = _projectionFloor.Transform(_unitPosition);
			floorPosition = DrawScene.Get<ScenePath>().Path.GetClosestPoint(floorPosition);
			return floorPosition;
		}

		private void TurnLeft()
		{
			var result = Directions4.None;

			switch (Orientation)
			{
				case Directions4.Down: result = Directions4.Right; break;
				case Directions4.Right: result = Directions4.Up; break;
				case Directions4.Up: result = Directions4.Left; break;
				case Directions4.Left: result = Directions4.Down; break;
			}

			Orientation = result;
		}

		private void TurnRight()
		{
			var result = Directions4.None;

			switch (Orientation)
			{
				case Directions4.Down: result = Directions4.Left; break;
				case Directions4.Left: result = Directions4.Up; break;
				case Directions4.Up: result = Directions4.Right; break;
				case Directions4.Right: result = Directions4.Down; break;
			}

			Orientation = result;
		}

		private Vector2 Forward()
		{
			var targetUnitPosition = _unitPosition + Vector2.Divide(Orientation.ToVector2() * new Vector2(1, -1), new Vector2(30, 7));
			return _projectionCeiling.Transform(targetUnitPosition);
		}

		private Path CreateRestrictionPathCeiling()
		{
			const int offsetY = 10;
			var points = new PathVertex[10];

			points[0] = new PathVertex(663, 33 + offsetY);
			points[1] = new PathVertex(641 - 50, 18 + offsetY);
			points[2] = new PathVertex(650 - 40, 3 + offsetY);
			points[3] = new PathVertex(1055, 3 + offsetY);
			points[4] = new PathVertex(951, 23 + offsetY);
			points[5] = new PathVertex(886, 21 + offsetY);
			points[6] = new PathVertex(867, 32 + offsetY);
			points[7] = new PathVertex(848, 4 + offsetY);
			points[8] = new PathVertex(862, 15 + offsetY);
			points[9] = new PathVertex(877, 3 + offsetY);

			var indices = new int[24];
			indices[0] = 0; indices[1] = 1; indices[2] = 2;
			indices[3] = 3; indices[4] = 4; indices[5] = 5;
			indices[6] = 5; indices[7] = 6; indices[8] = 8;
			indices[9] = 2; indices[10] = 7; indices[11] = 8;
			indices[12] = 0; indices[13] = 2; indices[14] = 6;
			indices[15] = 2; indices[16] = 6; indices[17] = 8;
			indices[18] = 5; indices[19] = 8; indices[20] = 9;
			indices[21] = 3; indices[22] = 5; indices[23] = 9;

			return new Path(points, indices, 0.6f, 0.9f);
		}

		[NonSerialized]
		private Path _restrictionPathCeiling;

		public Path RestrictionPathCeiling => _restrictionPathCeiling ?? (_restrictionPathCeiling = CreateRestrictionPathCeiling());
	}
}
