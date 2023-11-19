using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class Hazelnuts : Entity
	{
		public Hazelnuts()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.hazelnuts);

			HotspotSprite
				.Create(this)
				.SetCaption(Basement_Res.hazelnuts)
				.SetPixelPerfect(true);

			Transform
				.Create(this)
				.SetPosition(31, 201)
				.SetZ(263);

			Interaction
				.Create(this)
				.SetPosition(127, 276)
				.SetDirection(Directions8.Left)
				.SetGetInteractionsFn(GetInteractions);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Open, OpenScript())
					.Add(Verbs.Pick, PickScript());
		}

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Better_take_them_first);
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			yield return Game.Ego.StartScript(InventoryItems.Hazelnuts.LookScript());
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Who_knows_how_long_Im_locked_in_here);
				yield return Game.Ego.StartUse();
				Game.Ego.Inventory.AddItem<InventoryItems.Hazelnuts>();
				Enabled = false;
				Visible = false;
				yield return Game.Ego.StopUse();
			}
		}
	}
}
