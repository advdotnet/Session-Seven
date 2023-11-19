using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class Paperclips : ItemBase
	{
		public Paperclips() : base(content.inventory.paperclips, Items_Res.Paperclips_Paperclips_PaperClips)
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
				yield return Game.Ego.Say(Items_Res.A_small_box_with_paper_clips);
				yield return Game.Ego.Say(Items_Res.The_plastic_wrapping_is_missing_on_some_of_them);
			}
		}
	}
}
