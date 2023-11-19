using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class WoodenBox : ItemBase
	{
		public WoodenBox()
			: base(content.inventory.woodenbox, Items_Res.WoodenBox_WoodenBox_WoodenBox)
		{

		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Open, OpenScript())
					.Add(Verbs.Look, LookScript());
		}

		public bool Opened { get; private set; }

		private IEnumerator OpenScript()
		{
			using (Game.CutsceneBlock())
			{
				Game.Ego.Turn(STACK.Components.Directions4.Up);

				if (Opened)
				{
					yield return Game.Ego.StartUse();
					yield return Game.WaitForSoundEffect(content.audio.box_open);
					yield return Game.Ego.Say(Items_Res.Nothing_more_in_there);
					yield return Game.Ego.StopUse();
					yield break;
				}

				Game.PlayBasementEndSong();

				yield return Game.Ego.Say(Items_Res.Lets_see_whats_in_there);

				yield return Game.Ego.StartUse();
				yield return Game.WaitForSoundEffect(content.audio.box_open);

				Game.Ego.Inventory.AddItem<AnimalSkeleton>();
				yield return Game.Ego.StopUse();

				yield return Game.Ego.Say(Items_Res.Eww);
				yield return Game.Ego.Say(Items_Res.A_rotten_animal_skeleton);

				yield return Game.Ego.StartUse();
				Game.Ego.Inventory.AddItem<Pills>();
				yield return Game.Ego.StopUse();

				yield return Game.Ego.Say(Items_Res.Those_are_Landons_antidepressants_They_have_not_yet_been_opened);

				yield return Game.Ego.StartUse();
				Game.Ego.Inventory.AddItem<Baton>();
				yield return Game.Ego.StopUse();

				yield return Game.Ego.Say(Items_Res.And_an_expendable_baton);
				yield return Game.Ego.Say(Items_Res.What_the_hell_would_he_do_with_a_baton);

				Opened = true;

				yield return Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.Five);
			}
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.This_small_wooden_box_was_hidden_underneath_a_plank);
				yield return Game.Ego.Say(Items_Res.A_RFID_tag_is_glued_to_the_bottom);
			}
		}
	}
}
