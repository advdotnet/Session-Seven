using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{
    /// <summary>
    /// Boxes in the foreground
    /// </summary>
    [Serializable]
    public class Foreground : Entity
    {
        public Foreground()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.foreground);

            Transform
                .Create(this)
                .SetZ(400);
        }
    }
}
