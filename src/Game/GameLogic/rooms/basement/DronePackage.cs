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
	public class DronePackage : Entity
	{
		public DronePackage()
		{
			Interaction
				.Create(this)
				.SetPosition(703, 247)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			Transform
				.Create(this)
				.SetZ(3);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.drone_package)
				.AddRectangle(709, 190, 36, 22);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Pick, PickScript())
					.Add(Verbs.Open, OpenScript());
		}

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_should_take_it_out_of_the_cabinet_first);
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			yield return Game.Ego.StartScript(InventoryItems.DronePackage.LookScript());
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.StartUse();
				Game.Ego.Inventory.AddItem<InventoryItems.DronePackage>();
				Enabled = false;
				Tree.Basement.CabinetRightDoor.TakeDrone();
				yield return Game.Ego.StopUse();
			}
		}
	}
}
