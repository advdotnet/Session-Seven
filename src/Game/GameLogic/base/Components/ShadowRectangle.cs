using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STACK;
using STACK.Graphics;
using System;

namespace SessionSeven.Components
{
	[Serializable]
	public class ShadowRectangle : Component, IContent
	{
		[NonSerialized]
		private Texture2D _texture;
		[NonSerialized]
		private static Rectangle _shadowTL = new Rectangle(0, 0, 20, 20);
		[NonSerialized]
		private static Rectangle _shadowRT = new Rectangle(108, 0, 20, 20);
		[NonSerialized]
		private static Rectangle _shadowLB = new Rectangle(0, 108, 20, 20);
		[NonSerialized]
		private static Rectangle _shadowRB = new Rectangle(108, 108, 20, 20);
		[NonSerialized]
		private static Rectangle _shadowT = new Rectangle(20, 0, 88, 20);
		[NonSerialized]
		private static Rectangle _shadowB = new Rectangle(20, 108, 88, 20);
		[NonSerialized]
		private static Rectangle _shadowL = new Rectangle(0, 20, 20, 88);
		[NonSerialized]
		private static Rectangle _shadowR = new Rectangle(108, 20, 20, 88);

		public ShadowRectangle()
		{

		}

		public void LoadContent(ContentLoader content)
		{
			_texture = content.Load<Texture2D>(SessionSeven.content.ui.shadow);
		}

		public void UnloadContent()
		{

		}

		public static ShadowRectangle Create(World addTo)
		{
			return addTo.Add<ShadowRectangle>();
		}

		public void DrawShadow(SpriteBatch spriteBatch, Rectangle rect, int width, Color color)
		{
			// shadow
			// lt, rt, lb, rb                   
			spriteBatch.Draw(_texture, new Rectangle(rect.X - width, rect.Y - width, width, width), _shadowTL, color);
			spriteBatch.Draw(_texture, new Rectangle(rect.X + rect.Width, rect.Y - width, width, width), _shadowRT, color);
			spriteBatch.Draw(_texture, new Rectangle(rect.X - width, rect.Y + rect.Height, width, width), _shadowLB, color);
			spriteBatch.Draw(_texture, new Rectangle(rect.X + rect.Width, rect.Y + rect.Height, width, width), _shadowRB, color);

			// t, b, l, r
			spriteBatch.Draw(_texture, new Rectangle(rect.X, rect.Y - width, rect.Width, width), _shadowT, color);
			spriteBatch.Draw(_texture, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, width), _shadowB, color);
			spriteBatch.Draw(_texture, new Rectangle(rect.X - width, rect.Y, width, rect.Height), _shadowL, color);
			spriteBatch.Draw(_texture, new Rectangle(rect.X + rect.Width, rect.Y, width, rect.Height), _shadowR, color);
		}

		public void DrawBorder(Renderer renderer, Rectangle rect, int width, Color color)
		{
			renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, new Rectangle(rect.X, rect.Y, rect.Width, width), color);
			renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, new Rectangle(rect.X, rect.Y + rect.Height - width, rect.Width, width), color);
			renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, new Rectangle(rect.X, rect.Y, width, rect.Height), color);
			renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, new Rectangle(rect.X + rect.Width - width, rect.Y, width, rect.Height), color);
		}
	}
}
