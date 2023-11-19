using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Dialog;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class RemoteControl : ItemBase
	{

		public RemoteControl() : base(content.inventory.remotecontrol, Items_Res.RemoteControl_RemoteControl_RemoteControl, true, false)
		{
			BatteryCompartment
				.Create(this);
		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Tree.InventoryItems.BatteryA)
					.Add(Verbs.Use, Get<BatteryCompartment>().InstallBatteryAScript(true), Game.Ego)
				.For(Tree.InventoryItems.BatteryB)
					.Add(Verbs.Use, Get<BatteryCompartment>().InstallBatteryBScript(true), Game.Ego)
				.For(Game.Ego)
					.Add(Verbs.Use, UseScript())
					.Add(Verbs.Open, Get<BatteryCompartment>().OpenScript())
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.Its_a_handheld_controller_for_the_drone);
				yield return Game.Ego.Say(Items_Res.The_battery_compartment_has_place_for_two_batteries);

				var description = Get<BatteryCompartment>().GetDescriptionString();

				yield return Game.Ego.Say(description);
			}
		}

		private IEnumerator UseScript()
		{
			if (Get<BatteryCompartment>().GetBatteriesCount() != 2)
			{
				Tree.GUI.Interaction.Scene.Reset();
				yield return Game.Ego.Say(Items_Res.No_power);
				yield break;
			}

			if (!Drone.Enabled || !Drone.Visible)
			{
				Tree.GUI.Interaction.Scene.Reset();
				yield return Game.Ego.Say(Items_Res.I_need_to_put_the_drone_somewhere_first);
				yield break;
			}

			using (Game.CutsceneBlock())
			{
				Game.Ego.Inventory.Hide();

				Tree.GUI.Interaction.Scene.Interactive = false;
				Tree.GUI.Interaction.Scene.Visible = false;

				Game.Ego.Turn(Directions4.Up);

				yield return Game.Ego.StartUse();
				yield return Delay.Updates(10);

				Game.Ego.Get<CameraLocked>().SetEnabled(false);
				Drone.Get<CameraLocked>().SetEnabled(true);

				var menu = Tree.GUI.Dialog.Menu;
				var selection = BaseOption.None;

				while (selection.ID != (int)Basement.DroneCommand.Off && !Drone.Crashed)
				{
					var optionsDialog = GetOptions();

					menu.Open(optionsDialog);

					yield return menu.StartSelectionScript(Game.Ego.Get<Scripts>());
					selection = menu.LastSelectedOption;

					if (null != selection)
					{
						yield return Game.Ego.StartScript(Drone.ExecuteCommand((Basement.DroneCommand)selection.ID));
					}
				}

				while (Drone.Get<Scripts>().ScriptCollection.Count != 0)
				{
					yield return 0;
				}

				Game.Ego.Get<CameraLocked>().SetEnabled(true);
				Drone.Get<CameraLocked>().SetEnabled(false);

				yield return Game.Ego.StopUse();

				if (Drone.Crashed)
				{
					if (Tree.Basement.RFIDAntennaCabinet.FellDown)
					{
						Game.PlaySoundEffect(content.audio.puzzle);

						yield return Game.Ego.Say(Items_Res.Got_it);
					}
					else
					{
						yield return Game.Ego.Say(Items_Res.It_crashed);
					}
				}

				yield return Game.Ego.GoTo(Drone.Get<Transform>().Position);
				yield return Game.Ego.StartScript(Drone.PickScript());

				Game.Ego.Inventory.Show();

				Tree.GUI.Interaction.Scene.Interactive = true;
				Tree.GUI.Interaction.Scene.Visible = true;
			}
		}

		private Options GetOptions()
		{
			return Options.Create()
				.Add((int)Basement.DroneCommand.Forward, Items_Res.Forward)
				.Add((int)Basement.DroneCommand.Left, Items_Res.Turn_Left)
				.Add((int)Basement.DroneCommand.Right, Items_Res.Turn_Right)
				.Add((int)Basement.DroneCommand.Off, Items_Res.Switch_off, () => Drone.Flying)
				.Add((int)Basement.DroneCommand.On, Items_Res.Start_engine_and_ascend, () => !Drone.Flying);
		}

		private Basement.Drone Drone => Tree.Basement.Drone;
	}
}
