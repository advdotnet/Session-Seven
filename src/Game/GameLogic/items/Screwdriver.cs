using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class Screwdriver : ItemBase
	{
		public Screwdriver()
			: base(content.inventory.screwdriver, Items_Res.Screwdriver_Screwdriver_Screwdriver)
		{

		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Any.Object)
					.Add(Verbs.Use, UseAnyScript(), Game.Ego)
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator UseAnyScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.This_does_not_need_to_be_screwed);
			}
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.Its_a_screwdriver);
			}
		}
	}

}
