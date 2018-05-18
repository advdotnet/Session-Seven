using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Title
{

    [Serializable]
    public class Scene : Location
    {
        public Scene() : base(content.rooms.title.scene)
        {
            Background.Get<Sprite>().SetRenderStage(RenderStage.PostBloom);

            DrawOrder = 20;
        }
    }
}
