using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;
using System.Collections.Generic;

namespace SessionSeven.GUI.Interaction
{
    [Serializable]
    public class LockedVerb : Verb
    {
        public Rectangle TextureRectangle { get; private set; }
        public Rectangle ScreenRectangle { get; private set; }
        public List<string> RandomText { get; private set; }
        private GetRandomTextFn RandomTextFn = null;

        public LockedVerb(string id, List<string> defaultTexts, Rectangle textureRectangle, Rectangle screenRectangle, string text, string preposition, bool ditransitive, GetRandomTextFn randomTextFn) : base(text, preposition, ditransitive)
        {
            Id = id;
            TextureRectangle = textureRectangle;
            ScreenRectangle = screenRectangle;
            RandomText = defaultTexts;
            RandomTextFn = randomTextFn;
        }

        public static LockedVerb Create(string id, List<string> defaultTexts, Rectangle rectangle, int offset, string text, string preposition = "", bool ditransitive = false, GetRandomTextFn randomTextFn = null)
        {
            var ScreenRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            ScreenRect.Offset(0, offset);

            return new LockedVerb(id, defaultTexts, rectangle, ScreenRect, text, preposition, ditransitive, randomTextFn);
        }

        public string GetRandomText(Randomizer randomizer, InteractionContext interactionContext)
        {
            if (null != RandomTextFn)
            {
                var Result = RandomTextFn(randomizer, interactionContext);
                if (!string.IsNullOrEmpty(Result))
                {
                    return Result;
                }
            }

            var Index = randomizer.CreateInt(RandomText.Count);
            return RandomText[Index];
        }
    }
}
