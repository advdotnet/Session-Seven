using Microsoft.Xna.Framework.Input;
using System;

namespace SessionSeven.GUI.Interaction
{
    [Serializable]
    public static class KeyMapping
    {

        public static LockedVerb GetByKey(Keys key)
        {
            switch (key)
            {
                case Keys.Q:
                case Keys.NumPad7: return Verbs.Give;

                case Keys.W:
                case Keys.NumPad8: return Verbs.Pick;

                case Keys.E:
                case Keys.NumPad9: return Verbs.Use;

                case Keys.A:
                case Keys.NumPad4: return Verbs.Open;

                case Keys.S:
                case Keys.NumPad5: return Verbs.Look;

                case Keys.D:
                case Keys.NumPad6: return Verbs.Push;

                case Keys.Y:
                case Keys.Z:
                case Keys.NumPad1: return Verbs.Close;

                case Keys.X:
                case Keys.NumPad2: return Verbs.Talk;

                case Keys.C:
                case Keys.NumPad3: return Verbs.Pull;

                default: return null;
            }
        }
    }
}
