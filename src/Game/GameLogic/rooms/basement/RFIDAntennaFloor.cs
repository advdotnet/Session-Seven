using Microsoft.Xna.Framework;
using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.PositionSelection;
using STACK;
using STACK.Components;
using System;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{

	[Serializable]
	public class RFIDAntennaFloorShelf : RFIDAntennaFloor { }

	[Serializable]
	public class RFIDAntennaFloorCabinet : RFIDAntennaFloor { }

	[Serializable]
	public abstract class RFIDAntennaFloor : Entity, IPositionable
	{
		public const int POSITIONMODE_POSITION = 0;
		public const int POSITIONMODE_ROTATION = 1;

		public RFIDAntennaFloor()
		{
			CameraLocked
				.Create(this)
				.SetEnabled(false);

			Transform
				.Create(this)
				.SetPosition(400, 290)
				.SetUpdateZWithPosition(false)
				.SetZ(-1);

			SpriteData
				.Create(this)
				.SetOffset(-13, -6);

			Scripts
				.Create(this);

			TracerLine
				.Create(this)
				.SetCollider(WoodenPanel.Collider)
				.SetColor(Color.Red)
				.SetHitColor(Color.LightGreen)
				.SetVisible(false);

			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.rfidantenna);

			Enabled = false;
			Visible = false;
		}

		public static Path CreateRestrictionPathFloor()
		{
			var points = new PathVertex[12];

			points[0] = new PathVertex(181, 248);
			points[1] = new PathVertex(109, 348);
			points[2] = new PathVertex(282, 396);
			points[3] = new PathVertex(872, 392);
			points[4] = new PathVertex(1042, 347);
			points[5] = new PathVertex(983, 278);
			points[6] = new PathVertex(814, 276);
			points[7] = new PathVertex(791, 256);
			points[8] = new PathVertex(660, 252);
			points[9] = new PathVertex(650, 274);
			points[10] = new PathVertex(387, 278);
			points[11] = new PathVertex(358, 248);

			var indices = new int[30];
			indices[0] = 0; indices[1] = 1; indices[2] = 2;
			indices[3] = 2; indices[4] = 3; indices[5] = 4;
			indices[6] = 2; indices[7] = 4; indices[8] = 5;
			indices[9] = 2; indices[10] = 5; indices[11] = 6;
			indices[12] = 2; indices[13] = 6; indices[14] = 9;
			indices[15] = 2; indices[16] = 10; indices[17] = 11;
			indices[18] = 0; indices[19] = 2; indices[20] = 11;
			indices[21] = 2; indices[22] = 9; indices[23] = 10;
			indices[24] = 7; indices[25] = 8; indices[26] = 9;
			indices[27] = 6; indices[28] = 7; indices[29] = 9;

			return new Path(points, indices, 1.0f, 0.6f);
		}

		[NonSerialized]
		private Path _restrictionPathFloor;
		private const int _allowedDistanceSquared = 20 * 20;

		public void SetPosition(Vector2 position, int mode)
		{
			var transformedPosition = DrawScene.Get<Camera>().TransformInverse(position);

			if (POSITIONMODE_POSITION == mode)
			{
				if (null == _restrictionPathFloor)
				{
					_restrictionPathFloor = CreateRestrictionPathFloor();
				}

				var newPosition = _restrictionPathFloor.GetClosestPoint(transformedPosition);
				if (null != _lineSegmentTarget && null != _lineSegmentSource)
				{
					var closestTarget = _lineSegmentTarget.GetClosestPoint(newPosition);
					var closestSource = _lineSegmentSource.GetClosestPoint(newPosition);
					WasPlacedTooClose = (closestTarget - newPosition).LengthSquared() <= _allowedDistanceSquared ||
						(closestSource - newPosition).LengthSquared() <= _allowedDistanceSquared;
					if (!WasPlacedTooClose)
					{
						Get<SpriteData>().Color = new Color(0, 0, 1f, 0.3f);
					}
					else
					{
						Get<SpriteData>().Color = new Color(1f, 0, 0, 0.3f);
					}
				}

				Get<Transform>().Scale = _restrictionPathFloor.GetScale(newPosition.Y);
				Get<Transform>().Position = newPosition;
			}
			else
			{
				Get<TracerLine>().Target = transformedPosition;
			}
		}

		public bool WasPlacedTooClose { get; private set; }

		public void EndPosition(int mode)
		{
			Get<SpriteData>().Color = Color.White;
			Get<CameraLocked>().Enabled = false;
			if (POSITIONMODE_POSITION == mode)
			{
				Visible = false;
				Enabled = false;
			}
			Tree.GUI.PositionSelection.Label.Visible = false;
		}

		public void BeginPosition(int mode)
		{
			Tree.GUI.PositionSelection.Label.Visible = true;

			if (POSITIONMODE_ROTATION == mode)
			{
				Tree.GUI.PositionSelection.Label.LabelText = Basement_Res.Move_the_mouse_to_rotate_Left_click_to_confirm_Right_click_to_abort;
				Get<TracerLine>().SetVisible(true);
			}
			else
			{
				Tree.GUI.PositionSelection.Label.LabelText = Basement_Res.Left_click_to_place_the_antenna_Right_click_to_abort;
				Get<SpriteData>().Color = new Color(0, 0, 1f, 0.3f);
				Get<TracerLine>().SetVisible(false);
				WasPlacedTooClose = false;
			}

			Visible = true;
			Enabled = true;

			Get<CameraLocked>().Enabled = true;
			_lineSegmentTarget = null;
			_lineSegmentSource = null;
			if (OtherAntennaWasPlacedAndHit())
			{
				var position = OtherAntenna.Get<Transform>().Position;
				var source = OtherAntenna.Get<TracerLine>().ExtrapolateSource();
				var target = OtherAntenna.Get<TracerLine>().ExtrapolateTarget();
				_lineSegmentTarget = new StarFinder.LineSegment(position, target);
				_lineSegmentSource = new StarFinder.LineSegment(position, source);
			}
		}

		private bool OtherAntennaWasPlacedAndHit()
		{
			var other = OtherAntenna;
			return other != null &&
				other.Visible &&
				other.Enabled &&
				other.Get<TracerLine>().Visible &&
				other.Get<TracerLine>().HitCollider;
		}

		private RFIDAntennaFloor OtherAntenna
		{
			get
			{
				if (Tree.Basement.RFIDAntennaFloorCabinet == this)
				{
					return Tree.Basement.RFIDAntennaFloorShelf;
				}
				return Tree.Basement.RFIDAntennaFloorCabinet;
			}
		}

		private StarFinder.LineSegment _lineSegmentTarget = null;
		private StarFinder.LineSegment _lineSegmentSource = null;
	}
}
