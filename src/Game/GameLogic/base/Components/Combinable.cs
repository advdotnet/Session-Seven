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
        private bool _IsCombinable;

        public bool IsCombinable
        {
            get { return _IsCombinable; }
            set { _IsCombinable = value; }
        }

        public Combinable()
        {
            _IsCombinable = true;
        }

        public static Combinable Create(Entity entity)
        {
            return entity.Add<Combinable>();
        }
    }
}
