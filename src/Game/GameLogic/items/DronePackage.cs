using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class DronePackage : ItemBase
	{
		public DronePackage() : base(content.inventory.dronepackage, Items_Res.DronePackage_DronePackage_DronePackage, true, false)
		{

		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Open, OpenScript())
					.Add(Verbs.Use, UseScript())
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator OpenScript()
		{
			using (Game.CutsceneBlock())
			{
				Game.Ego.Turn(STACK.Components.Directions4.Up);
				yield return Game.Ego.StartUse();
				Game.Ego.Inventory.AddItem<Drone>();
				Game.Ego.Inventory.AddItem<RemoteControl>();
				Game.Ego.Inventory.RemoveItem(this);
				yield return Game.Ego.StopUse();
			}
		}

		private IEnumerator UseScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.I_should_open_the_package_first);
			}
		}

		public static IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.Aw_man_we_hardly_ever_used_this_thing);
				yield return Game.Ego.Say(Items_Res.It_was_pretty_fun_freaking_the_neighbors_dog_out_with_it_though);
				yield return Game.Ego.Say(Items_Res.Good_memories);
			}
		}
	}
}
