using Microsoft.Xna.Framework;
using SessionSeven.Components;
using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Office
{
    [Serializable]
    public class RyanEyesClosed : Entity
    {
        readonly Vector2 DEFAULT_POSITION = new Vector2(289, 231);

        bool _Blinking = true;
        public bool Blinking
        {
            get
            {
                return _Blinking;
            }
            set
            {
                Visible = false;
                if (value && _Blinking != value)
                {
                    Get<RandomCountdown>().Reset();
                }
                _Blinking = value;
            }
        }

        public RyanEyesClosed()
        {
            Transform
                .Create(this)
                .SetPosition(DEFAULT_POSITION)
                .SetZ(2)
                .SetAbsolute(true);

            Sprite
                .Create(this)
                .SetImage(content.rooms.office.ryaneyes, 2)
                .SetFrame(1);

            RandomCountdown
                .Create(this)
                .SetDuration(7)
                .SetMinUpdates(300)
                .SetMaxUpdates(500);

            DrawOrder = Ryan.PRIORITY + 1;
        }

        public override void OnUpdate()
        {
            if (Blinking)
            {
                Visible = Get<RandomCountdown>().Action;
            }

            var RyanCurrentFrame = Tree.Office.Ryan.Get<Sprite>().CurrentFrame;

            switch (RyanCurrentFrame)
            {
                case 1:
                    Get<Transform>().SetPosition(290, 232);
                    Get<Sprite>().CurrentFrame = 1;
                    break;
                case 2:
                    Get<Transform>().SetPosition(288, 232);
                    Get<Sprite>().CurrentFrame = 1;
                    break;

                case 3:
                case 4:
                case 5:
                    Get<Transform>().SetPosition(287, 233);
                    Get<Sprite>().CurrentFrame = 2;
                    break;

                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:

                case 17:
                case 18:
                case 19:
                    Get<Transform>().SetPosition(DEFAULT_POSITION);
                    Get<Sprite>().CurrentFrame = 1;
                    break;

                case 14:
                case 15:
                case 16:
                    Get<Transform>().SetPosition(286, 231);
                    Get<Sprite>().CurrentFrame = 1;
                    break;
            }

            base.OnUpdate();
        }
    }
}
