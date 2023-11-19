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

		private Vector2 _target;
		public Vector2 Target
		{
			get => _target;
			set
			{
				if (value != _target)
				{
					_target = value;
					EvaluateCollision();
				}
			}
		}

		private Circle _collider;
		/// <summary>
		/// (x,y) center position
		/// z radius
		/// </summary>
		public Circle Collider
		{
			get => _collider;
			set
			{
				if (!value.Equals(_collider))
				{
					_collider = value;
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
			HitCollider = _collider.Intersects(From, ExtrapolateTarget());
		}

		private Vector2 From
		{
			get
			{
				var transform = Get<Transform>();
				return transform.Position;
			}
		}

		public bool Visible { get; set; }
		public float DrawOrder { get; set; }

		public void Draw(Renderer renderer)
		{
			if (renderer.Stage == RenderStage.Bloom)
			{
				var to = ExtrapolateTarget();

				DrawLine(renderer, From, to);
			}
		}

		public Vector2 ExtrapolateTarget()
		{
			var diff = Target - From;

			return diff * Game.VIRTUAL_WIDTH * 3;
		}

		public Vector2 ExtrapolateSource()
		{
			var diff = From - Target;

			return diff * Game.VIRTUAL_WIDTH * 3;
		}

		private void DrawLine(Renderer renderer, Vector2 from, Vector2 to)
		{
			var diff = to - from;
			var angle = (float)Math.Atan2(diff.Y, diff.X);
			var rectangle = new Rectangle((int)from.X, (int)from.Y, (int)diff.Length(), 1);
			var drawColor = HitCollider ? HitColor : Color;

			renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, rectangle, null, drawColor, angle, new Vector2(0, 0), SpriteEffects.None, 0);
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
