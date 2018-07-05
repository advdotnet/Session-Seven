using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using STACK.Graphics;
using System;

namespace SessionSeven.GUI.PositionSelection
{
    /// <summary>
    /// Entity to display some instruction text.
    /// </summary>
    [Serializable]
    public class Label : Entity
    {
        public Label()
        {
            Text
                .Create(this)
                .SetFont(content.fonts.pixeloperator_outline_BMF)
                .SetAlign(Alignment.Top)
                .SetWidth(Game.VIRTUAL_WIDTH)
                .SetWordWrap(true);

            Transform
                .Create(this)
                .SetPosition(Game.VIRTUAL_WIDTH / 2, 50);

        }

        public override void OnBeginDraw(Renderer renderer)
        {
            if (renderer.Stage == RenderStage.PostBloom)
            {
                renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, new Rectangle(0, 0, Game.VIRTUAL_WIDTH, 18 * Get<Text>().Lines.Count), new Color(0, 0, 0, 0.5f));
            }

            base.OnBeginDraw(renderer);
        }

        public string LabelText
        {
            get
            {
                return Get<Text>().Lines.Count == 1 ? Get<Text>().Lines[0].Text : string.Empty;
            }
            set
            {
                if (Get<Text>().Lines.Count != 1 || Get<Text>().Lines[0].Text != value)
                {
                    Get<Text>().Set(value, -1, Position);
                }
            }
        }

        public Color Color
        {
            get
            {
                return Get<Text>().Color;
            }
            set
            {
                Get<Text>().Color = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return Get<Transform>().Position;
            }
            set
            {
                Get<Transform>().SetPosition(value);
                Get<Text>().SetPosition(value);
            }
        }
    }
}
