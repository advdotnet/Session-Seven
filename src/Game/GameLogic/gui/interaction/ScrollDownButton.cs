using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI.Interaction
{
    [Serializable]
    public class ScrollDownButton : Entity
    {
        public static Rectangle ScreenRectangle = new Rectangle(290, Verbs.OFFSET + 17 + 46, 38, 46);
        public static Rectangle TextureRectangle = new Rectangle(290, 17 + 46, 38, 46);

        public ScrollDownButton()
        {
            HotspotRectangle
                .Create(this)
                .SetCaption(string.Empty)
                .AddRectangle(ScreenRectangle);

            Transform
                .Create(this)
                .SetZ(InteractionBar.Z + 1);

            Interactive = false;
        }
    }
}
