using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class BatonWithString : ItemBase
	{

		public BatonWithString()
			: base(content.inventory.baton_string, Items_Res.BatonWithString_BatonWithString_BatonWithString)
		{

		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Close, PushScript())
					.Add(Verbs.Push, PushScript())
					.Add(Verbs.Open, PullScript())
					.Add(Verbs.Pull, PullScript())
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator PullScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.Cant_expand_it_any_further);
			}
		}

		private IEnumerator PushScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.Cant_reduce_it_with_the_string_tied_to_it);
			}
		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.Almost_like_a_net);
			}
		}
	}
}
