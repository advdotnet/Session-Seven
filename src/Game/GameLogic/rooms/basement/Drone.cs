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
        private const int INITIAL_Z = 263;

        Directions4 _Orientation = Directions4.Down;
        Directions4 Orientation
        {
            get
            {
                return _Orientation;
            }
            set
            {
                _Orientation = value;
                Get<Transform>().SetOrientation(value.ToVector2());
            }
        }
        Projection2D ProjectionFloor = new Projection2D().SetQuadliteral(new Vector2(0 - 40, 375), new Vector2(1103 + 40, 375), new Vector2(999, 227), new Vector2(104, 227));
        Projection2D ProjectionCeiling = new Projection2D().SetQuadliteral(new Vector2(14, 10), new Vector2(1072, 10), new Vector2(985, 45), new Vector2(110, 45));

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
            var CeilingPosition = GetCeilingPosition(INITIAL_POSITION);
            var Scale = RestrictionPathCeiling.GetScale(CeilingPosition.Y);
            Transform.SetScale(Scale);
            Get<Interaction>().SetPosition(INITIAL_POSITION);
            Transform.SetZ(INITIAL_Z);
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
                .SetZ(INITIAL_Z);

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

        Interactions GetInteractions()
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
            var DiffVector = Transform.Position - Game.Ego.Get<Transform>().Position;
            if (Vector2.Zero != DiffVector)
            {
                Game.Ego.Turn(DiffVector.ToDirection4());
            }
        }

        IEnumerator UseScript()
        {
            using (Game.CutsceneBlock())
            {
                FaceDrone();
                yield return Game.Ego.Say(Basement_Res.The_drone_is_operated_by_using_its_remote_control);
            }
        }

        IEnumerator LookScript()
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

        Vector2 UnitPosition;
        bool ReachedCeiling = false;
        private int LastScaledStep = -1;

        private int SetFrame(Transform transform, int step, int lastFrame)
        {
            if (!Flying)
            {
                return GetStandingFrame();
            }

            var scaledStep = step / 3;

            if (scaledStep != LastScaledStep)
            {
                LastScaledStep = scaledStep;
                var FlyingFrames = GetFlyingFrames();

                return FlyingFrames.GetRandomExcluding(World.Get<Randomizer>(), lastFrame);
            }

            return lastFrame;
        }

        Frames FramesFlyingDown = Frames.Create(2, 3, 4, 5);
        Frames FramesFlyingUp = Frames.Create(7, 8, 9, 10);
        Frames FramesFlyingRight = Frames.Create(12, 13, 14, 15);
        Frames FramesFlyingLeft = Frames.Create(17, 18, 19, 20);

        Frames GetFlyingFrames()
        {
            switch (Orientation)
            {
                case Directions4.Down: return FramesFlyingDown;
                case Directions4.Up: return FramesFlyingUp;
                case Directions4.Right: return FramesFlyingRight;
                default: return FramesFlyingLeft;
            }
        }

        int GetStandingFrame()
        {
            switch (Orientation)
            {
                case Directions4.Down: return 1;
                case Directions4.Up: return 6;
                case Directions4.Right: return 11;
                default: return 16;
            }
        }

        private Transform Transform
        {
            get
            {
                return Get<Transform>();
            }
        }

        [NonSerialized]
        SoundEffectInstance FlyingSoundEffect;

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
                    ReachedCeiling = true;
                    break;
            }

            // if collision
            while (Get<Scripts>().ScriptCollection.Count > 0)
            {
                yield return 1;
            }
        }

        public bool Crashed { get; private set; }

        Vector2 GetSpritePosition()
        {
            return DisplacedPosition + new Vector2(0, 10);
        }

        private Vector2 DisplacedPosition = Vector2.Zero;

        private void PlayFlyingSound()
        {
            FlyingSoundEffect = Game.PlaySoundEffect(content.audio.drone_flying, true);
        }

        private void StopFlyingSound()
        {
            FlyingSoundEffect?.Stop();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Flying && null == FlyingSoundEffect)
            {
                PlayFlyingSound();
            }

            if (Flying && ReachedCeiling)
            {
                UnitPosition = ProjectionCeiling.TransformInverse(Transform.Position);

                var DisplacedUnitPosition = UnitPosition + Get<DroneDisplacement>().GetDisplacement() / new Vector2(500f, 80f);
                DisplacedPosition = ProjectionCeiling.Transform(DisplacedUnitPosition);

                var Scale = RestrictionPathCeiling.GetScale(Transform.Position.Y);
                Transform.SetScale(Scale);

                var DroneCollided = !RestrictionPathCeiling.Contains(DisplacedPosition);

                if (Tree.Basement.RFIDAntennaCabinet.Enabled && Tree.Basement.RFIDAntennaCabinet.Collider.Contains(DisplacedPosition) && !Tree.Basement.RFIDAntennaCabinet.FellDown)
                {
                    DroneCollided = true;
                    Tree.Basement.RFIDAntennaCabinet.StartFallDownScript();
                }

                if (DroneCollided)
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
                DisplacedPosition = Transform.Position;
            }
        }

        private IEnumerator LandScript()
        {
            var LandingPosition = StopEngine();
            Transform.SetZ(LandingPosition.Y);
            yield return Get<Scripts>().GoTo(LandingPosition);
            Game.PlaySoundEffect(content.audio.drone_landing);
        }

        private Vector2 StartEngine()
        {
            Flying = true;
            Crashed = false;
            ReachedCeiling = false;
            UnitPosition = ProjectionFloor.TransformInverse(Transform.Position);
            var CeilingPosition = ProjectionCeiling.Transform(UnitPosition);
            return CeilingPosition;
        }

        private Vector2 GetCeilingPosition(Vector2 floorPosition)
        {
            var UnitSpacePosition = ProjectionFloor.TransformInverse(Transform.Position);
            return ProjectionCeiling.Transform(UnitSpacePosition);
        }

        private Vector2 StopEngine()
        {
            Flying = false;
            ReachedCeiling = false;
            UnitPosition = ProjectionCeiling.TransformInverse(Transform.Position);
            var FloorPosition = ProjectionFloor.Transform(UnitPosition);
            FloorPosition = DrawScene.Get<ScenePath>().Path.GetClosestPoint(FloorPosition);
            return FloorPosition;
        }

        private void TurnLeft()
        {
            Directions4 Result = Directions4.None;

            switch (Orientation)
            {
                case Directions4.Down: Result = Directions4.Right; break;
                case Directions4.Right: Result = Directions4.Up; break;
                case Directions4.Up: Result = Directions4.Left; break;
                case Directions4.Left: Result = Directions4.Down; break;
            }

            Orientation = Result;
        }

        private void TurnRight()
        {
            Directions4 Result = Directions4.None;

            switch (Orientation)
            {
                case Directions4.Down: Result = Directions4.Left; break;
                case Directions4.Left: Result = Directions4.Up; break;
                case Directions4.Up: Result = Directions4.Right; break;
                case Directions4.Right: Result = Directions4.Down; break;
            }

            Orientation = Result;
        }

        private Vector2 Forward()
        {
            var TargetUnitPosition = UnitPosition + Vector2.Divide(Orientation.ToVector2() * new Vector2(1, -1), new Vector2(30, 7));
            return ProjectionCeiling.Transform(TargetUnitPosition);
        }

        private void Crash()
        {
            Flying = false;
        }

        Path CreateRestrictionPathCeiling()
        {
            const int OffsetY = 10;
            var Points = new PathVertex[10];

            Points[0] = new PathVertex(663, 33 + OffsetY);
            Points[1] = new PathVertex(641 - 50, 18 + OffsetY);
            Points[2] = new PathVertex(650 - 40, 3 + OffsetY);
            Points[3] = new PathVertex(1055, 3 + OffsetY);
            Points[4] = new PathVertex(951, 23 + OffsetY);
            Points[5] = new PathVertex(886, 21 + OffsetY);
            Points[6] = new PathVertex(867, 32 + OffsetY);
            Points[7] = new PathVertex(848, 4 + OffsetY);
            Points[8] = new PathVertex(862, 15 + OffsetY);
            Points[9] = new PathVertex(877, 3 + OffsetY);

            int[] Indices = new int[24];
            Indices[0] = 0; Indices[1] = 1; Indices[2] = 2;
            Indices[3] = 3; Indices[4] = 4; Indices[5] = 5;
            Indices[6] = 5; Indices[7] = 6; Indices[8] = 8;
            Indices[9] = 2; Indices[10] = 7; Indices[11] = 8;
            Indices[12] = 0; Indices[13] = 2; Indices[14] = 6;
            Indices[15] = 2; Indices[16] = 6; Indices[17] = 8;
            Indices[18] = 5; Indices[19] = 8; Indices[20] = 9;
            Indices[21] = 3; Indices[22] = 5; Indices[23] = 9;

            return new Path(Points, Indices, 0.6f, 0.9f);
        }

        [NonSerialized]
        Path _RestrictionPathCeiling;

        public Path RestrictionPathCeiling
        {
            get
            {
                return _RestrictionPathCeiling ?? (_RestrictionPathCeiling = CreateRestrictionPathCeiling());
            }
        }
    }
}
