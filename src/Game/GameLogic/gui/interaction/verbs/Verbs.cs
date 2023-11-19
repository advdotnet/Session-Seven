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
	[Serializable]
	public class Verbs : Entity
	{
		public const int OFFSET = InteractionBar.HEIGHT;

		public static readonly LockedVerb Walk = LockedVerb.Create("walk", RandomTexts.Default, Rectangle.Empty, OFFSET, GlblRes.Walk_to);
		public static readonly LockedVerb Give = LockedVerb.Create("give", RandomTexts.Give, new Rectangle(0, 17, 92, 30), OFFSET, GlblRes.Give, GlblRes.to, true);
		public static readonly LockedVerb Open = LockedVerb.Create("open", RandomTexts.Open, new Rectangle(0, 17 + 30, 92, 32), OFFSET, GlblRes.Open);
		public static readonly LockedVerb Close = LockedVerb.Create("close", RandomTexts.Close, new Rectangle(0, 17 + 62, 92, 33), OFFSET, GlblRes.Close);
		public static readonly LockedVerb Pick = LockedVerb.Create("pick", RandomTexts.Pick, new Rectangle(92, 17, 110, 30), OFFSET, GlblRes.Pick_up, string.Empty, false, PickRandomTextFn);
		public static readonly LockedVerb Look = LockedVerb.Create("look", RandomTexts.Look, new Rectangle(92, 17 + 30, 110, 32), OFFSET, GlblRes.Look_at);
		public static readonly LockedVerb Talk = LockedVerb.Create("talk", RandomTexts.Talk, new Rectangle(92, 17 + 62, 110, 33), OFFSET, GlblRes.Talk_to);
		public static readonly LockedVerb Use = LockedVerb.Create("use", RandomTexts.Use, new Rectangle(202, 17, 88, 30), OFFSET, GlblRes.Use, GlblRes.with, true);
		public static readonly LockedVerb Push = LockedVerb.Create("push", RandomTexts.Move, new Rectangle(202, 17 + 30, 88, 32), OFFSET, GlblRes.Push);
		public static readonly LockedVerb Pull = LockedVerb.Create("pull", RandomTexts.Move, new Rectangle(202, 17 + 62, 88, 33), OFFSET, GlblRes.Pull);

		public static readonly List<LockedVerb> All = new List<LockedVerb>() { Give, Open, Close, Pick, Look, Talk, Use, Push, Pull };

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
				var index = randomizer.CreateInt(RandomTexts.PickInventory.Count);
				return RandomTexts.PickInventory[index];
			}

			return null;
		}

		public LockedVerb HighlightedVerb { get; set; }

		public override void OnDraw(Renderer renderer)
		{
			base.OnDraw(renderer);

			if (null != HighlightedVerb)
			{
				var texture = Tree.GUI.Interaction.VerbsHighlight.Get<Sprite>().Texture;
				renderer.SpriteBatch.Draw(texture, HighlightedVerb.ScreenRectangle, HighlightedVerb.TextureRectangle, Color.White);
			}

			if (!World.Interactive)
			{
				return;
			}

			var pick = World.Get<STACK.Components.Mouse>().ObjectUnderMouse;

			if (pick == Tree.GUI.Interaction.ScrollUpButton && Game.Ego.Inventory.CanScrollUp)
			{
				var texture = Tree.GUI.Interaction.VerbsHighlight.Get<Sprite>().Texture;
				renderer.SpriteBatch.Draw(texture, ScrollUpButton.ScreenRectangle, ScrollUpButton.TextureRectangle, Color.White);
			}
			else if (pick == Tree.GUI.Interaction.ScrollDownButton && Game.Ego.Inventory.CanScrollDown)
			{
				var texture = Tree.GUI.Interaction.VerbsHighlight.Get<Sprite>().Texture;
				renderer.SpriteBatch.Draw(texture, ScrollDownButton.ScreenRectangle, ScrollDownButton.TextureRectangle, Color.White);
			}
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
