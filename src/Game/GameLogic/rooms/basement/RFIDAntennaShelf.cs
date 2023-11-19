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
	public class RFIDAntennaShelf : Entity
	{
		public RFIDAntennaShelf()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.rfidantennashelf);

			HotspotSprite
				.Create(this)
				.SetCaption(Basement_Res.cardboard)
				.SetPixelPerfect(false);

			Transform
				.Create(this)
				.SetPosition(1038, 114)
				.SetZ(Shelf.Z + 2);

			Interaction
				.Create(this)
				.SetPosition(1015, 281)
				.SetDirection(Directions8.Right)
				.SetGetInteractionsFn(GetInteractions);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Open, OpenScript())
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Pick, PickScript());
		}

		private bool _lookedAt = false;

		private IEnumerator LookScript(bool reset = true)
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock(reset))
			{
				yield return Game.Ego.Say(Basement_Res.A_cardboard_without_any_inscription);
				yield return Game.Ego.Say(Basement_Res.I_wonder_who_put_it_here_and_what_is_inside);
				_lookedAt = true;
			}
		}

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_should_take_it_first);
			}
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);

			using (Game.CutsceneBlock())
			{
				if (!_lookedAt)
				{
					yield return Game.Ego.StartScript(LookScript(false));
				}
				yield return Game.Ego.StartUse();
				Game.Ego.Inventory.AddItem<RFIDAntennaBoxShelf>();
				Visible = false;
				Enabled = false;
				yield return Game.Ego.StopUse();
			}
		}
	}
}
