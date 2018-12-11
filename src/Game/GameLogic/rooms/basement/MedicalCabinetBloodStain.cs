using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{
    [Serializable]
    public class MedicalCabinetBloodStain : Entity
    {
        public MedicalCabinetBloodStain()
        {
            Sprite
                .Create(this)
                .SetImage(content.rooms.basement.medicalcabinet_blood_stain)
                .SetVisible(false);

            Transform
                .Create(this)
                .SetZ(1)
                .SetPosition(772, 105);
        }
    }
}
