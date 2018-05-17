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
        private Texture2D Texture;
        [NonSerialized]
        static Rectangle SHADOW_TL = new Rectangle(0, 0, 20, 20);
        [NonSerialized]
        static Rectangle SHADOW_RT = new Rectangle(108, 0, 20, 20);
        [NonSerialized]
        static Rectangle SHADOW_LB = new Rectangle(0, 108, 20, 20);
        [NonSerialized]
        static Rectangle SHADOW_RB = new Rectangle(108, 108, 20, 20);
        [NonSerialized]
        static Rectangle SHADOW_T = new Rectangle(20, 0, 88, 20);
        [NonSerialized]
        static Rectangle SHADOW_B = new Rectangle(20, 108, 88, 20);
        [NonSerialized]
        static Rectangle SHADOW_L = new Rectangle(0, 20, 20, 88);
        [NonSerialized]
        static Rectangle SHADOW_R = new Rectangle(108, 20, 20, 88);

        public ShadowRectangle()
        {

        }

        public void LoadContent(ContentLoader content)
        {
            Texture = content.Load<Texture2D>(SessionSeven.content.ui.shadow);
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
            spriteBatch.Draw(Texture, new Rectangle(rect.X - width, rect.Y - width, width, width), SHADOW_TL, color);
            spriteBatch.Draw(Texture, new Rectangle(rect.X + rect.Width, rect.Y - width, width, width), SHADOW_RT, color);
            spriteBatch.Draw(Texture, new Rectangle(rect.X - width, rect.Y + rect.Height, width, width), SHADOW_LB, color);
            spriteBatch.Draw(Texture, new Rectangle(rect.X + rect.Width, rect.Y + rect.Height, width, width), SHADOW_RB, color);

            // t, b, l, r
            spriteBatch.Draw(Texture, new Rectangle(rect.X, rect.Y - width, rect.Width, width), SHADOW_T, color);
            spriteBatch.Draw(Texture, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, width), SHADOW_B, color);
            spriteBatch.Draw(Texture, new Rectangle(rect.X - width, rect.Y, width, rect.Height), SHADOW_L, color);
            spriteBatch.Draw(Texture, new Rectangle(rect.X + rect.Width, rect.Y, width, rect.Height), SHADOW_R, color);
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
