using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class ShelfBlanketFlashlight : Entity
	{
		private const int _blanketFrame = 1;
		private const int _flashlightFrame = 2;

		public ShelfBlanketFlashlight()
		{
			Interaction
				.Create(this)
				.SetPosition(1020, 295)
				.SetDirection(Directions8.Right)
				.SetGetInteractionsFn(GetInteractions);

			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.blanketflashlight, 2)
				.SetFrame(_blanketFrame);

			HotspotSprite
				.Create(this)
				.SetCaption(Basement_Res.blanket)
				.SetPixelPerfect(false);

			Transform
				.Create(this)
				.SetPosition(1070, 246)
				.SetZ(Shelf.Z + 2);

			Enabled = false;
		}

		private Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Pick, PickScript());
		}

		public bool LookedAt { get; private set; }

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (_blanketFrame == Get<Sprite>().CurrentFrame)
				{
					yield return Game.Ego.Say(Basement_Res.Looks_like_our_old_picnic_blanket);
				}
				else if (_flashlightFrame == Get<Sprite>().CurrentFrame)
				{
					yield return Game.Ego.Say(Basement_Res.Below_the_blanket_was_a_flashlight);
				}
			}
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.StartUse();

				if (_blanketFrame == Get<Sprite>().CurrentFrame)
				{
					Get<Sprite>().CurrentFrame = _flashlightFrame;
					Get<HotspotSprite>().SetCaption(Basement_Res.flashlight);
					Game.Ego.Inventory.AddItem<InventoryItems.Blanket>();
				}
				else if (_flashlightFrame == Get<Sprite>().CurrentFrame)
				{
					Visible = false;
					Enabled = false;
					Game.Ego.Inventory.AddItem<InventoryItems.Flashlight>();
				}

				yield return Game.Ego.StopUse();

				if (Game.Ego.Inventory.HasItem<InventoryItems.Blanket>() && Visible)
				{
					yield return Tree.InventoryItems.Blanket.LookAt();
				}
			}
		}
	}
}
