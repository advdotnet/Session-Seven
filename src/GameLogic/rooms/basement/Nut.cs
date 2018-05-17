using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using SessionSeven.GUI.PositionSelection;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
    [Serializable]
    public class NutsOnFloor : Entity
    {
        [Serializable]
        public class Nut : Entity, IPositionable
        {
            public readonly Vector2 INTERACTIONOFFSET = new Vector2(5, 1);

            [NonSerialized]
            Path RestrictionPathFloor;

            public Nut(Vector2 position)
            {
                Sprite
                    .Create(this)
                    .SetImage(content.rooms.basement.nut);

                HotspotSprite
                    .Create(this)
                    .SetCaption(Basement_Res.nut)
                    .SetPixelPerfect(true);

                Interaction
                    .Create(this)
                    .SetDirection(Directions8.Down)
                    .SetPosition(position)
                    .SetGetInteractionsFn(GetInteractions);

                Transform
                    .Create(this)
                    .SetPosition(position)
                    .SetZ(position.Y);

                SpriteData
                    .Create(this)
                    .SetOffset(-3, -2);

                CameraLocked
                    .Create(this)
                    .SetEnabled(false);
            }

            public bool Placed = false;

            private Interactions GetInteractions()
            {
                return Interactions
                    .Create()
                    .For(Game.Ego)
                        .Add(Verbs.Pick, PickScript())
                        .Add(Verbs.Look, LookScript());
            }

            IEnumerator LookScript()
            {
                using (Game.CutsceneBlock())
                {
                    Game.Ego.Turn(this);
                    yield return Game.Ego.Say(Basement_Res.A_nut_is_lying_on_the_floor);
                }
            }

            IEnumerator PickScript()
            {
                using (Game.CutsceneBlock())
                {
                    Game.Ego.Turn(this);
                    yield return Game.Ego.Say(Basement_Res.I_have_plenty_more_of_them);
                }
            }

            public void BeginPosition(int mode)
            {
                Visible = true;
                Enabled = true;
                Get<SpriteData>().Color = new Color(0, 0, 1f, 0.3f);
                Get<CameraLocked>().Enabled = true;
            }

            public void EndPosition(int mode)
            {
                Get<SpriteData>().Color = Color.White;
                Get<CameraLocked>().Enabled = false;
                Visible = false;
                Enabled = false;
            }

            public void SetPosition(Vector2 position, int mode)
            {
                var TransformedPosition = DrawScene.Get<Camera>().TransformInverse(position);
                var NewPosition = TransformedPosition;

                if (null == RestrictionPathFloor)
                {
                    RestrictionPathFloor = CreateRestrictionPathFloor();
                }

                NewPosition = RestrictionPathFloor.GetClosestPoint(TransformedPosition);
                var Transform = Get<Transform>();
                Transform.Position = NewPosition;
                Get<Interaction>().Position = NewPosition;
                Transform.Z = NewPosition.Y;
            }

            Path CreateRestrictionPathFloor()
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

            public void Place()
            {
                Visible = true;
                Enabled = true;
                Placed = true;
            }
        }

        public List<Nut> Nuts = new List<Nut>();

        public NutsOnFloor()
        {

        }

        public Nut AddNut(int x, int y)
        {
            var Nut = new Nut(new Vector2(x, y));
            DrawScene.Push(Nut);
            Nuts.Add(Nut);
            return Nut;
        }

        /// <summary>
        /// Returns the first nut in the watch area
        /// </summary>
        /// <returns>null if there is none</returns>
        public Nut GetNutInWatchArea()
        {
            foreach (var Nut in Nuts)
            {
                if (Nut.Placed && Tree.Basement.MouseHole.WatchArea.Contains(Nut.Get<Transform>().Position))
                {
                    return Nut;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the closest nut to the given position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="withinDistance">if != -1 only the given distance is considered</param>
        /// <returns></returns>
        public Nut GetClosestNut(Vector2 position, float withinDistance = -1)
        {
            Nut Result = null;
            var ClosestDistance = float.MaxValue;

            foreach (var Nut in Nuts)
            {
                if (Nut.Placed)
                {
                    var Distance = (Nut.Get<Transform>().Position - position).Length();

                    if (Distance < ClosestDistance && (withinDistance == -1 || Distance <= withinDistance))
                    {
                        Result = Nut;
                        ClosestDistance = Distance;
                    }
                }
            }

            return Result;
        }

        public void RemoveNut(Nut nut)
        {
            DrawScene.Pop(nut);
            Nuts.Remove(nut);
        }
    }
}
