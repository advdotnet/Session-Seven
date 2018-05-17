using STACK;
using System;

namespace SessionSeven.Components
{
    /// <summary>
    /// Entity can be given to other entities by using the "GIVE" verb.
    /// </summary>
    [Serializable]
    public class Givable : Component
    {
        public static Givable Create(Entity entity)
        {
            return entity.Add<Givable>();
        }
    }    
}
