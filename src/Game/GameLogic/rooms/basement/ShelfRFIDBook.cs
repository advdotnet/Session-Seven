using SessionSeven.GUI.Interaction;
using SessionSeven.InventoryItems;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class ShelfRFIDBook : Entity
	{
		public ShelfRFIDBook()
		{
			Interaction
				.Create(this)
				.SetPosition(1020, 295)
				.SetDirection(Directions8.Right)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.book)
				.AddRectangle(1064, 113, 32, 11);

			Transform
				.Create(this)
				.SetPosition(1064, 114)
				.SetZ(Shelf.Z + 1);

			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.rfidbook)
				.SetVisible(false); // sprite is shown when taken

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Use, PickScript())
					.Add(Verbs.Pick, PickScript());
		}

		private IEnumerator LookScript(bool reset = true)
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock(true, reset))
			{
				yield return Game.Ego.Say(Basement_Res.This_books_title_is_RFID_Applied);
				yield return Game.Ego.Say(Basement_Res.I_wonder_what_its_doing_on_the_top_shelf_compartment);
				yield return Game.Ego.Say(Basement_Res.Landon_was_interested_in_RFID_technology_for_some_reason_some_time_ago);
				yield return Game.Ego.Say(Basement_Res.Maybe_he_put_it_up_there);
				Get<HotspotRectangle>().SetCaption(Basement_Res.RFID_Applied);
				LookedAt = true;
			}
		}

		public bool LookedAt { get; private set; }

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);

			if (!LookedAt)
			{
				yield return Game.Ego.StartScript(LookScript(false));
			}

			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.StartUse();
				Enabled = false;
				Get<Sprite>().SetVisible(true);
				Game.Ego.Inventory.AddItem<RFIDBook>();
				yield return Game.Ego.StopUse();
			}
		}
	}


}
