using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class Hazelnuts : ItemBase
	{
		public Hazelnuts() : base(content.inventory.hazelnuts, Items_Res.Hazelnuts_Hazelnuts_Hazelnuts, true, true)
		{
		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Use, UseScript());
		}

		private IEnumerator UseScript()
		{
			var sawFirstTime = false;

			using (Game.CutsceneBlock())
			{
				if (!Tree.Basement.MouseHole.FedMouse || Game.Ego.Inventory.HasItem<Paperclips>())
				{
					yield return Game.Ego.Say(Items_Res.Im_not_hungry);
					yield return Game.Ego.Say(Items_Res.Yet);
					yield break;
				}
				else
				{
					yield return Game.Ego.StartScript(PlaceNutScript());
					if (!SawMouseCollectNuts)
					{
						Game.Ego.Inventory.Hide();
						if (null != Tree.Basement.NutsOnFloor.GetNutInWatchArea())
						{
							yield return Game.Ego.GoTo(660, 280);
						}

						var checkScript = Game.Mouse.CheckForNuts();

						while (!checkScript.Done)
						{
							// make the player watch the mouse the first time it is collecting nuts
							if (Game.Mouse.Visible)
							{
								Game.Ego.Turn(Game.Mouse);
							}

							yield return 0;
						}

						if (Game.Mouse.CollectedNuts)
						{
							yield return Game.Ego.Say(Items_Res.Look_at_that);
							yield return Game.Ego.Say(Items_Res.What_a_beast);
							SawMouseCollectNuts = true;
							sawFirstTime = true;
						}

						Tree.Basement.MouseHole.SawMouse = true;

						Game.Ego.Inventory.Show();
					}
				}
			}

			if (SawMouseCollectNuts && !sawFirstTime)
			{
				Game.Mouse.CheckForNuts();
			}
		}

		public bool SawMouseCollectNuts { get; private set; }

		private IEnumerator PlaceNutScript()
		{
			var selection = Tree.GUI.PositionSelection.Scene;

			Tree.GUI.Interaction.Scene.Interactive = false;
			Tree.GUI.Interaction.Scene.Visible = false;
			Game.Ego.Inventory.Hide();
			Game.Ego.Get<CameraLocked>().Enabled = false;

			var nut = Tree.Basement.NutsOnFloor.AddNut(0, 0);

			yield return selection.StartSelectionScript(Game.Ego.Get<Scripts>(), nut);

			Game.Ego.Get<CameraLocked>().Enabled = true;

			if (selection.Aborted)
			{
				Tree.Basement.NutsOnFloor.RemoveNut(nut);
			}
			else
			{
				yield return Game.Ego.GoTo(nut);
				yield return Game.Ego.Use();
				if (Tree.Basement.WoodenBox.Get<HotspotRectangle>().IsHit(nut.Get<Transform>().Position))
				{
					Tree.Basement.NutsOnFloor.RemoveNut(nut);
					yield return Game.Ego.Say(Items_Res.It_fell_down_the_hole);
				}
				else
				{
					nut.Place();
				}
			}

			Tree.GUI.Interaction.Scene.Interactive = true;
			Tree.GUI.Interaction.Scene.Visible = true;

			Game.Ego.Inventory.Show();
		}

		public static IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.A_bag_of_hazelnuts);
				yield return Game.Ego.Say(Items_Res.Cynthia_is_always_eating_these_when_shes_up_late_crafting_or_working_on_the_house);
			}
		}
	}
}
