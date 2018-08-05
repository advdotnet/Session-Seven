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
        private bool _IsGivable;

        public bool IsGivable
        {
            get { return _IsGivable; }
            set { _IsGivable = value; }
        }

        public Givable()
        {
            _IsGivable = true;
        }

        public static Givable Create(Entity entity)
        {
            return entity.Add<Givable>();
        }
    }
}
