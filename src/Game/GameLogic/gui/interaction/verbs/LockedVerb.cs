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
		private readonly GetRandomTextFn _randomTextFn = null;

		public LockedVerb(string id, List<string> defaultTexts, Rectangle textureRectangle, Rectangle screenRectangle, string text, string preposition, bool ditransitive, GetRandomTextFn randomTextFn) : base(text, preposition, ditransitive)
		{
			Id = id;
			TextureRectangle = textureRectangle;
			ScreenRectangle = screenRectangle;
			RandomText = defaultTexts;
			_randomTextFn = randomTextFn;
		}

		public static LockedVerb Create(string id, List<string> defaultTexts, Rectangle rectangle, int offset, string text, string preposition = "", bool ditransitive = false, GetRandomTextFn randomTextFn = null)
		{
			var screenRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
			screenRect.Offset(0, offset);

			return new LockedVerb(id, defaultTexts, rectangle, screenRect, text, preposition, ditransitive, randomTextFn);
		}

		public string GetRandomText(Randomizer randomizer, InteractionContext interactionContext)
		{
			if (null != _randomTextFn)
			{
				var result = _randomTextFn(randomizer, interactionContext);
				if (!string.IsNullOrEmpty(result))
				{
					return result;
				}
			}

			var index = randomizer.CreateInt(RandomText.Count);
			return RandomText[index];
		}
	}
}
