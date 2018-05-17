using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.GUI.Interaction
{
    /// <summary>
    /// Entity to display teh current action text like "Goto object" or "Use object1 with object2".
    /// </summary>
    [Serializable]
    public class ActionTextLabel : Entity
    {
        public ActionTextLabel()
        {
            Text
                .Create(this)
                .SetFont(content.fonts.pixeloperator_BMF)
                .SetAlign(Alignment.Top)
                .SetWidth(Game.VIRTUAL_WIDTH)
                .SetWordWrap(false);

            Transform
                .Create(this)
                .SetZ(InteractionBar.Z + 3)
                .SetPosition(Game.VIRTUAL_WIDTH / 2, 338);

        }

        public string ActionText
        {
            get
            {
                return Get<Text>().Lines.Count == 1 ? Get<Text>().Lines[0].Text : string.Empty;
            }
            set
            {
                if (Get<Text>().Lines.Count != 1 || Get<Text>().Lines[0].Text != value)
                {
                    Get<Text>().Set(value, -1, Position);
                }
            }
        }

        public Color Color
        {
            get
            {
                return Get<Text>().Color;
            }
            set
            {
                Get<Text>().Color = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return Get<Transform>().Position;
            }
            set
            {
                Get<Transform>().SetPosition(value);
                Get<Text>().SetPosition(value);
            }
        }
    }
}
