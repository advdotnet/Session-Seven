using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using SessionSeven.InventoryItems;
using STACK;
using STACK.Components;
using STACK.Graphics;
using System;
using System.Collections.Generic;
using GlblRes = global::SessionSeven.Properties.Resources;

namespace SessionSeven.GUI.Interaction
{
    public delegate string GetRandomTextFn(Randomizer randomizer, InteractionContext interactionContext);

    [Serializable]
    public class LockedVerb : Verb
    {
        public Rectangle TextureRectangle { get; private set; }
        public Rectangle ScreenRectangle { get; private set; }
        public List<string> RandomText { get; private set; }
        private GetRandomTextFn RandomTextFn = null;

        public LockedVerb(List<string> defaultTexts, Rectangle textureRectangle, Rectangle screenRectangle, string text, string preposition, bool ditransitive, GetRandomTextFn randomTextFn) : base(text, preposition, ditransitive)
        {
            TextureRectangle = textureRectangle;
            ScreenRectangle = screenRectangle;
            RandomText = defaultTexts;
            RandomTextFn = randomTextFn;
        }

        public static LockedVerb Create(List<string> defaultTexts, Rectangle rectangle, int offset, string text, string preposition = "", bool ditransitive = false, GetRandomTextFn randomTextFn = null)
        {
            var ScreenRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            ScreenRect.Offset(0, offset);

            return new LockedVerb(defaultTexts, rectangle, ScreenRect, text, preposition, ditransitive, randomTextFn);
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

    [Serializable]
    public class Verbs : Entity
    {
        public const int OFFSET = 288;
        public static LockedVerb Walk = LockedVerb.Create(RandomTexts.Default, Rectangle.Empty, OFFSET, GlblRes.Walk_to);
        public static LockedVerb Give = LockedVerb.Create(RandomTexts.Give, new Rectangle(0, 17, 92, 30), OFFSET, GlblRes.Give, GlblRes.to, true);
        public static LockedVerb Open = LockedVerb.Create(RandomTexts.Open, new Rectangle(0, 17 + 30, 92, 32), OFFSET, GlblRes.Open);
        public static LockedVerb Close = LockedVerb.Create(RandomTexts.Close, new Rectangle(0, 17 + 62, 92, 33), OFFSET, GlblRes.Close);
        public static LockedVerb Pick = LockedVerb.Create(RandomTexts.Pick, new Rectangle(92, 17, 110, 30), OFFSET, GlblRes.Pick_up, "", false, PickRandomTextFn);
        public static LockedVerb Look = LockedVerb.Create(RandomTexts.Look, new Rectangle(92, 17 + 30, 110, 32), OFFSET, GlblRes.Look_at);
        public static LockedVerb Talk = LockedVerb.Create(RandomTexts.Talk, new Rectangle(92, 17 + 62, 110, 33), OFFSET, GlblRes.Talk_to);
        public static LockedVerb Use = LockedVerb.Create(RandomTexts.Use, new Rectangle(202, 17, 88, 30), OFFSET, GlblRes.Use, GlblRes.with, true);
        public static LockedVerb Push = LockedVerb.Create(RandomTexts.Move, new Rectangle(202, 17 + 30, 88, 32), OFFSET, GlblRes.Push);
        public static LockedVerb Pull = LockedVerb.Create(RandomTexts.Move, new Rectangle(202, 17 + 62, 88, 33), OFFSET, GlblRes.Pull);

        public static List<LockedVerb> All = new List<LockedVerb>() { Give, Open, Close, Pick, Look, Talk, Use, Push, Pull };

        public Verbs()
        {
            Sprite
                .Create(this)
                .SetRenderStage(RenderStage.PostBloom)
                .SetImage(GlblRes.uiverbs);

            SpriteData
                .Create(this)
                .SetOffset(0, OFFSET);
        }

        private static string PickRandomTextFn(Randomizer randomizer, InteractionContext interactionContext)
        {
            if (interactionContext.Primary is ItemBase)
            {
                var Index = randomizer.CreateInt(RandomTexts.PickInventory.Count);
                return RandomTexts.PickInventory[Index];
            }

            return null;
        }

        public LockedVerb HighlightedVerb { get; set; }

        public override void OnDraw(Renderer renderer)
        {
            base.OnDraw(renderer);
            if (null != HighlightedVerb)
            {
                var Texture = Tree.GUI.Interaction.VerbsHighlight.Get<Sprite>().Texture;
                renderer.SpriteBatch.Draw(Texture, HighlightedVerb.ScreenRectangle, HighlightedVerb.TextureRectangle, Color.White);
            }

            var Pick = World.Get<STACK.Components.Mouse>().ObjectUnderMouse;

            if (Pick == Tree.GUI.Interaction.ScrollUpButton && Game.Ego.Inventory.CanScrollUp && World.Interactive)
            {
                var Texture = Tree.GUI.Interaction.VerbsHighlight.Get<Sprite>().Texture;
                renderer.SpriteBatch.Draw(Texture, ScrollUpButton.ScreenRectangle, ScrollUpButton.TextureRectangle, Color.White);
            }
            else if (Pick == Tree.GUI.Interaction.ScrollDownButton && Game.Ego.Inventory.CanScrollDown && World.Interactive)
            {
                var Texture = Tree.GUI.Interaction.VerbsHighlight.Get<Sprite>().Texture;
                renderer.SpriteBatch.Draw(Texture, ScrollDownButton.ScreenRectangle, ScrollDownButton.TextureRectangle, Color.White);
            }
        }
    }

    [Serializable]
    public class VerbsHighlight : Entity
    {
        public VerbsHighlight()
        {
            Sprite
                .Create(this)
                .SetRenderStage(RenderStage.PostBloom)
                .SetImage(GlblRes.uiverbshighlight);

            SpriteData
                .Create(this)
                .SetOffset(0, Verbs.OFFSET);

            Visible = false;
        }
    }

    [Serializable]
    public class InteractionBar : Entity
    {
        public const float Z = 1;

        public InteractionBar()
        {
            HotspotRectangle
                .Create(this)
                .SetCaption("")
                .SetRectangle(0, 288, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT - 288);

            Transform
                .Create(this)
                .SetZ(Z);
        }
    }

    [Serializable]
    public class ScrollUpButton : Entity
    {
        public static Rectangle ScreenRectangle = new Rectangle(290, Verbs.OFFSET + 17, 38, 46);
        public static Rectangle TextureRectangle = new Rectangle(290, 17, 38, 46);

        public ScrollUpButton()
        {
            HotspotRectangle
                .Create(this)
                .SetCaption("scroll up")
                .AddRectangle(ScreenRectangle);

            Transform
                .Create(this)
                .SetZ(InteractionBar.Z + 1);

            Interactive = false;
        }
    }

    [Serializable]
    public class ScrollDownButton : Entity
    {
        public static Rectangle ScreenRectangle = new Rectangle(290, Verbs.OFFSET + 17 + 46, 38, 46);
        public static Rectangle TextureRectangle = new Rectangle(290, 17 + 46, 38, 46);

        public ScrollDownButton()
        {
            HotspotRectangle
                .Create(this)
                .SetCaption("scroll down")
                .AddRectangle(ScreenRectangle);

            Transform
                .Create(this)
                .SetZ(InteractionBar.Z + 1);

            Interactive = false;
        }
    }

    [Serializable]
    public abstract class VerbButton : Entity
    {
        public LockedVerb Verb { get; private set; }

        public VerbButton(LockedVerb verb)
        {
            Verb = verb;

            HotspotRectangle
                .Create(this)
                .SetCaption(verb.Text)
                .AddRectangle(verb.ScreenRectangle);

            Transform
                .Create(this)
                .SetZ(InteractionBar.Z + 1);
        }

        public override void OnLoadContent(ContentLoader content)
        {
            // Restore static references from savegame
            foreach (var Verb in Verbs.All)
            {
                if (this.Verb.Equals(Verb))
                {
                    this.Verb = Verb;
                }
            }

            base.OnLoadContent(content);
        }
    }

    [Serializable]
    public class GiveButton : VerbButton
    {
        public GiveButton() : base(Verbs.Give) { }
    }

    [Serializable]
    public class OpenButton : VerbButton
    {
        public OpenButton() : base(Verbs.Open) { }
    }

    [Serializable]
    public class CloseButton : VerbButton
    {
        public CloseButton() : base(Verbs.Close) { }
    }

    [Serializable]
    public class PickButton : VerbButton
    {
        public PickButton() : base(Verbs.Pick) { }
    }

    [Serializable]
    public class LookButton : VerbButton
    {
        public LookButton() : base(Verbs.Look) { }
    }

    [Serializable]
    public class TalkButton : VerbButton
    {
        public TalkButton() : base(Verbs.Talk) { }
    }

    [Serializable]
    public class UseButton : VerbButton
    {
        public UseButton() : base(Verbs.Use) { }
    }

    [Serializable]
    public class PushButton : VerbButton
    {
        public PushButton() : base(Verbs.Push) { }
    }

    [Serializable]
    public class PullButton : VerbButton
    {
        public PullButton() : base(Verbs.Pull) { }
    }
}
