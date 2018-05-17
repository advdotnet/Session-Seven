using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{
    [Serializable]
    public class ModemLight : Entity
    {
        public ModemLight()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.modemlight, 2);

            Transform
                .Create(this)
                .SetZ(3)
                .SetPosition(241, 214);

        }

        byte counter = 0;
        const int TOGGLEDELAY = 23;

        public override void OnUpdate()
        {
            counter++;

            if (counter > TOGGLEDELAY)
            {
                counter = 0;
                ToggleLight();
            }

            base.OnUpdate();
        }

        private void ToggleLight()
        {
            Get<Sprite>().CurrentFrame = Get<Sprite>().CurrentFrame == 1 ? 2 : 1;
        }
    }
}
