using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI
{
    [Serializable]
    public class Fader : Entity
    {
        public Fader()
        {
            Sprite
                .Create(this)
                .SetRenderStage(RenderStage.PostBloom)
                .SetImage(Sprite.WHITEPIXELIMAGE);

            SpriteData
                .Create(this)
                .SetColor(Color.Black)
                .SetScale(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT);

            DrawOrder = 1;
        }

        public Color Color
        {
            get
            {
                return Get<SpriteData>().Color;
            }
            set
            {
                Get<SpriteData>().Color = value;
            }
        }
    }
}
