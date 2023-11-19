using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class AnimalSkeleton : ItemBase
	{
		public AnimalSkeleton()
			: base(content.inventory.animalskeleton, Items_Res.AnimalSkeleton_AnimalSkeleton_AnimalSkeleton)
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
				yield return Game.Ego.Say(Items_Res.Eww);
			}
		}
	}

}
