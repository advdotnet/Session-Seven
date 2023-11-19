using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class DrawerKey : ItemBase
	{
		public DrawerKey() : base(content.inventory.drawerkey, Items_Res.DrawerKey_DrawerKey_SmallKey)
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
				yield return Game.Ego.Say(Items_Res.Cant_use_the_key_with_that);
			}
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.This_small_key_was_glued_to_the_back_of_Landons_portrait);
			}
		}
	}
}
