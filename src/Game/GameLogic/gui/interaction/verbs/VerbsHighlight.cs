using STACK;
using STACK.Components;
using System;
using GlblRes = global::SessionSeven.Properties.Resources;

namespace SessionSeven.GUI.Interaction
{
    [Serializable]
    public class VerbsHighlight : Entity
    {
        public VerbsHighlight()
        {
            Sprite
                .Create(this)
                .SetRenderStage(RenderStage.PostBloom)
                .SetImage(GlblRes.uiverbshighlight);

            SpriteData
                .Create(this)
                .SetOffset(0, Verbs.OFFSET);

            Visible = false;
        }
    }
}
