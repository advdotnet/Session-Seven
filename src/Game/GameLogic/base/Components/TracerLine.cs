using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STACK;
using STACK.Components;
using STACK.Graphics;
using System;

namespace SessionSeven.Components
{
    /// <summary>
    /// Component that draws a line from the Transform's position to a target position.
    /// It also does a line segment circle collision check and changes the line color accordingly.
    /// </summary>
    [Serializable]
    public class TracerLine : Component, IDraw
    {
        public Color Color { get; set; }
        public Color HitColor { get; set; }
        public bool HitCollider { get; private set; }

        private Vector2 _Target;
        public Vector2 Target
        {
            get
            {
                return _Target;
            }
            set
            {
                if (value != _Target)
                {
                    _Target = value;
                    EvaluateCollision();
                }
            }
        }

        private Circle _Collider;
        /// <summary>
        /// (x,y) center position
        /// z radius
        /// </summary>
        public Circle Collider
        {
            get
            {
                return _Collider;
            }
            set
            {
                if (!value.Equals(_Collider))
                {
                    _Collider = value;
                    EvaluateCollision();
                }
            }
        }

        public TracerLine()
        {
            DrawOrder = 0;
            Visible = true;
        }

        private void EvaluateCollision()
        {
            HitCollider = _Collider.Intersects(From, ExtrapolateTarget());
        }

        private Vector2 From
        {
            get
            {
                var Transform = Get<Transform>();
                return Transform.Position;
            }
        }

        public bool Visible { get; set; }
        public float DrawOrder { get; set; }

        public void Draw(Renderer renderer)
        {
            if (renderer.Stage == RenderStage.Bloom)
            {
                var To = ExtrapolateTarget();

                DrawLine(renderer, From, To);
            }
        }

        public Vector2 ExtrapolateTarget()
        {
            var Diff = (Target - From);

            return Diff * Game.VIRTUAL_WIDTH * 3;
        }

        public Vector2 ExtrapolateSource()
        {
            var Diff = (From - Target);

            return Diff * Game.VIRTUAL_WIDTH * 3;
        }

        void DrawLine(Renderer renderer, Vector2 from, Vector2 to)
        {
            var Diff = to - from;
            var Angle = (float)Math.Atan2(Diff.Y, Diff.X);
            var Rectangle = new Rectangle((int)from.X, (int)from.Y, (int)Diff.Length(), 1);
            var DrawColor = HitCollider ? HitColor : Color;

            renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, Rectangle, null, DrawColor, Angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        public static TracerLine Create(Entity entity)
        {
            return entity.Add<TracerLine>();
        }

        public TracerLine SetColor(Color value) { Color = value; return this; }
        public TracerLine SetHitColor(Color value) { HitColor = value; return this; }
        public TracerLine SetTarget(Vector2 value) { Target = value; return this; }
        public TracerLine SetCollider(Circle value) { Collider = value; return this; }
        public TracerLine SetVisible(bool value) { Visible = value; return this; }
    }
}
