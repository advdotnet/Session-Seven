using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{
    [Serializable]
    public class DrillingMachineUsed : Entity
    {
        public DrillingMachineUsed()
        {
            Transform
                .Create(this)
                .SetPosition(364, 154)
                .SetZ(1f);

            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.drillused)
                .SetVisible(false);
        }

        public void Show()
        {
            Get<Sprite>().Visible = true;
            Tree.Basement.DrillingMachine.Visible = false;
        }

        public void Hide()
        {
            Get<Sprite>().Visible = false;
            Tree.Basement.DrillingMachine.Visible = true;
        }
    }


}
