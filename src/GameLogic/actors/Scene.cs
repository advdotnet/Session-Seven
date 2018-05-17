using System;

namespace SessionSeven.Actors
{

    [Serializable]
    public class Scene : STACK.Scene
    {
        public Scene(Inventory playerInventory)
        {
            Enabled = false;
            Visible = true;

            Push(new RyanVoice(),
                 new Ryan(playerInventory),
                 new Mouse());
        }
    }
}
