using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI
{
    [Serializable]
    public class Mouse : Entity
    {
        public Mouse()
        {
            Transform
                .Create(this);

            Sprite
                .Create(this)
                .SetImage(content.ui.cursor)
                .SetRenderStage(RenderStage.PostBloom);

            SpriteData
                .Create(this)
                .SetOffset(-20, -20);

            MouseFollower
                .Create(this);

            InteractiveVisibility
                .Create(this);
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
            Visible = false;
        }
    }
}
