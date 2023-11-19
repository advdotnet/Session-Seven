using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class BandagesCut : ItemBase
	{
		public BandagesCut() : base(content.inventory.bandages_cut, Items_Res.Bandages_Bandages_ClothBandageRoll, true, false)
		{

		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Use, UseScript())
					.Add(Verbs.Look, LookScript())
				.For(Tree.InventoryItems.Scissors)
					.Add(Verbs.Use, UseScissorsScript(), Game.Ego);
		}

		public Script Use()
		{
			return Game.Ego.StartScript(UseScript());
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.A_cloth_strip_bandage);
			}
		}

		private IEnumerator UseScript()
		{
			using (Game.CutsceneBlock())
			{
				if (Game.Ego.Inventory.HasItem<BandageStrip>())
				{
					yield return Game.Ego.Say(Items_Res.I_should_better_use_the_cut_off_part);
				}
				else
				{
					yield return Game.Ego.Say(Items_Res.I_dressed_my_wound_already);
				}
			}
		}

		private IEnumerator UseScissorsScript()
		{
			using (Game.CutsceneBlock())
			{
				if (Game.Ego.Inventory.HasItem<BandageStrip>())
				{
					yield return Game.Ego.Say(Items_Res.I_cut_off_a_part_of_it_already);
				}
				else
				{
					yield return Game.Ego.Say(Items_Res.I_dont_need_to_cut_any_more_off_of_that);
				}
			}
		}
	}

}
