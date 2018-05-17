using STACK;
using System;

namespace SessionSeven.Components
{
    /// <summary>
    /// Entity can be combined with other entities by the "USE" verb.
    /// </summary>
    [Serializable]
    public class Combinable : Component
    {
        public static Combinable Create(Entity entity)
        {
            return entity.Add<Combinable>();
        }
    }    
}
