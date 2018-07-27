using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI
{
    [Serializable]
    public class ScreenText : Entity
    {
        public ScreenText()
        {
            Text
                .Create(this)
                .SetColor(Color.White)
                .SetAlign(Alignment.Center)
                .SetFont(content.fonts.pixeloperator_outline_BMF)
                .SetFadeDuration(3f)
                .SetHeight(Game.VIRTUAL_HEIGHT)
                .SetWidth(Game.VIRTUAL_WIDTH - 50);

            Transform
                .Create(this)
                .SetPosition(Game.VIRTUAL_WIDTH / 2, Game.VIRTUAL_HEIGHT / 2);

            Scripts
                .Create(this);

            DrawOrder = 2;
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

        public Script ShowText(string text, float duration = 0)
        {
            return Get<Scripts>().Say(text, duration);
        }
    }
}
