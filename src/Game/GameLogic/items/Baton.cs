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
	public class Baton : ItemBase
	{
		private const string _imageUnexpended = content.inventory.baton;
		private const string _imageExpended = content.inventory.baton_expanded;

		public Baton()
			: base(_imageUnexpended, Items_Res.Baton_Baton_ExpandableBaton)
		{

		}

		public bool Expanded
		{
			get => _imageUnexpended != Get<Sprite>().Image;

			set
			{
				var image = value ? _imageExpended : _imageUnexpended;
				if (image != Get<Sprite>().Image)
				{
					Get<Sprite>().SetImage(image);
				}
			}
		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Tree.InventoryItems.GuitarStrings)
					.Add(Verbs.Use, UseStringsScript(), Game.Ego)
				.For(Game.Ego)
					.Add(Verbs.Close, PushScript())
					.Add(Verbs.Push, PushScript())
					.Add(Verbs.Open, PullScript())
					.Add(Verbs.Pull, PullScript())
					.Add(Verbs.Look, LookScript());
		}

		public IEnumerator UseStringsScript()
		{
			using (Game.CutsceneBlock())
			{
				if (Expanded)
				{
					yield return Game.Ego.Say(Items_Res.Im_knotting_a_string_on_the_ring_at_the_very_top);
					Game.Ego.Turn(Directions4.Up);
					yield return Game.Ego.StartUse();
					Game.Ego.Inventory.RemoveItem(this);
					Game.Ego.Inventory.RemoveItem<GuitarStrings>();
					Game.Ego.Inventory.AddItem<BatonWithString>();
					yield return Game.Ego.StopUse();
				}
				else
				{
					yield return Game.Ego.Say(Items_Res.I_dontt_have_anything_to_knot_the_string_to);
				}
			}
		}

		private IEnumerator PullScript()
		{
			using (Game.CutsceneBlock())
			{
				if (Expanded)
				{
					yield return Game.Ego.Say(Items_Res.Cant_expand_it_any_further);
				}
				else
				{
					Game.Ego.Turn(Directions4.Up);
					yield return Game.Ego.StartUse();
					Expanded = true;
					yield return Game.Ego.StopUse();
				}
			}
		}

		private IEnumerator PushScript()
		{
			using (Game.CutsceneBlock())
			{
				if (!Expanded)
				{
					yield return Game.Ego.Say(Items_Res.Cant_reduce_it_any_further);
				}
				else
				{
					Game.Ego.Turn(Directions4.Up);
					yield return Game.Ego.StartUse();
					Expanded = false;
					yield return Game.Ego.StopUse();
				}
			}
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				if (Expanded)
				{
					yield return Game.Ego.Say(Items_Res.An_expanded_baton_There_is_a_small_ring_on_the_top);
				}
				else
				{
					yield return Game.Ego.Say(Items_Res.An_expandable_baton);
				}
			}
		}
	}

}
