using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class SawKit : ItemBase
	{
		public SawKit()
			: base(content.inventory.sawkit, Items_Res.SawKit_SawKit_BiMetalHoleSawKit)
		{

		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.In_there_are_different_attachments_for_drilling_machines);
				yield return Game.Ego.Say(Items_Res.They_can_even_cut_through_metal);
			}
		}
	}

}
