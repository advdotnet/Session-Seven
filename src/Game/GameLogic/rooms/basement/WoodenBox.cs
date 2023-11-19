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
	public class WoodenBox : Entity
	{
		public WoodenBox()
		{
			Interaction
				.Create(this)
				.SetPosition(649, 277)
				.SetDirection(Directions8.Left)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.wooden_box)
				.AddRectangle(615, 271, 14, 9);

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

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_should_take_it_first);
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.A_small_wooden_box_was_hidden_underneath_a_panel);
			}
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.StartUse();
				Enabled = false;
				Tree.Basement.WoodenPanel.Get<Sprite>().CurrentFrame = 3;
				Game.Ego.Inventory.AddItem<InventoryItems.WoodenBox>();
				Visible = false;
				yield return Game.Ego.StopUse();
			}
		}
	}


}
