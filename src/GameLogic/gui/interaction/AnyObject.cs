using System;

namespace SessionSeven.GUI.Interaction
{
    /// <summary>
    /// Represents any object in interactions.
    /// </summary>
    [Serializable]
    public class Any
    {
        public static Any Object = new Any();

        private Any() { }

        public override bool Equals(object obj)
        {
            var item = obj as Any;

            if (item == null)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return "Any Object";
        }
    }
}
