using STACK;
using STACK.Components;
using System;

namespace SessionSeven.JailCell
{

    [Serializable]
    public class Scene : Location
    {
        public Scene() : base(content.rooms.jailcell.scene)
        {
            this.AutoAddEntities();

            ScenePath
                .Create(this)
                .SetPathFile(content.rooms.jailcell.path);

            DrawOrder = 22;
        }
    }
}
