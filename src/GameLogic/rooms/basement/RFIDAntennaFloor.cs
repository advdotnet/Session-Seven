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
            var Points = new PathVertex[12];

            Points[0] = new PathVertex(181, 248);
            Points[1] = new PathVertex(109, 348);
            Points[2] = new PathVertex(282, 396);
            Points[3] = new PathVertex(872, 392);
            Points[4] = new PathVertex(1042, 347);
            Points[5] = new PathVertex(983, 278);
            Points[6] = new PathVertex(814, 276);
            Points[7] = new PathVertex(791, 256);
            Points[8] = new PathVertex(660, 252);
            Points[9] = new PathVertex(650, 274);
            Points[10] = new PathVertex(387, 278);
            Points[11] = new PathVertex(358, 248);

            var Indices = new int[30];
            Indices[0] = 0; Indices[1] = 1; Indices[2] = 2;
            Indices[3] = 2; Indices[4] = 3; Indices[5] = 4;
            Indices[6] = 2; Indices[7] = 4; Indices[8] = 5;
            Indices[9] = 2; Indices[10] = 5; Indices[11] = 6;
            Indices[12] = 2; Indices[13] = 6; Indices[14] = 9;
            Indices[15] = 2; Indices[16] = 10; Indices[17] = 11;
            Indices[18] = 0; Indices[19] = 2; Indices[20] = 11;
            Indices[21] = 2; Indices[22] = 9; Indices[23] = 10;
            Indices[24] = 7; Indices[25] = 8; Indices[26] = 9;
            Indices[27] = 6; Indices[28] = 7; Indices[29] = 9;

            return new Path(Points, Indices, 1.0f, 0.6f);
        }

        [NonSerialized]
        Path RestrictionPathFloor;

        const int ALLOWEDDISTANCESQUARED = 20 * 20;

        public void SetPosition(Vector2 position, int mode)
        {
            var TransformedPosition = DrawScene.Get<Camera>().TransformInverse(position);

            if (POSITIONMODE_POSITION == mode)
            {
                var NewPosition = TransformedPosition;

                if (null == RestrictionPathFloor)
                {
                    RestrictionPathFloor = CreateRestrictionPathFloor();
                }

                NewPosition = RestrictionPathFloor.GetClosestPoint(TransformedPosition);

                if (null != LineSegmentTarget && null != LineSegmentSource)
                {
                    var ClosestTarget = LineSegmentTarget.GetClosestPoint(NewPosition);
                    var ClosestSource = LineSegmentSource.GetClosestPoint(NewPosition);
                    WasPlacedTooClose = (ClosestTarget - NewPosition).LengthSquared() <= ALLOWEDDISTANCESQUARED ||
                        (ClosestSource - NewPosition).LengthSquared() <= ALLOWEDDISTANCESQUARED;
                    if (!WasPlacedTooClose)
                    {
                        Get<SpriteData>().Color = new Color(0, 0, 1f, 0.3f);
                    }
                    else
                    {
                        Get<SpriteData>().Color = new Color(1f, 0, 0, 0.3f);
                    }
                }

                Get<Transform>().Scale = RestrictionPathFloor.GetScale(NewPosition.Y);
                Get<Transform>().Position = NewPosition;
            }
            else
            {
                Get<TracerLine>().Target = TransformedPosition;
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
            }

            Visible = true;
            Enabled = true;

            Get<CameraLocked>().Enabled = true;
            LineSegmentTarget = null;
            LineSegmentSource = null;
            if (OtherAntennaWasPlacedAndHit())
            {
                var Position = OtherAntenna.Get<Transform>().Position;
                var Source = OtherAntenna.Get<TracerLine>().ExtrapolateSource();
                var Target = OtherAntenna.Get<TracerLine>().ExtrapolateTarget();
                LineSegmentTarget = new StarFinder.LineSegment(Position, Target);
                LineSegmentSource = new StarFinder.LineSegment(Position, Source);
            }
        }

        private bool OtherAntennaWasPlacedAndHit()
        {
            var Other = OtherAntenna;
            return Other != null &&
                Other.Visible &&
                Other.Enabled &&
                Other.Get<TracerLine>().Visible &&
                Other.Get<TracerLine>().HitCollider;
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

        private StarFinder.LineSegment LineSegmentTarget = null;
        private StarFinder.LineSegment LineSegmentSource = null;
    }
}
