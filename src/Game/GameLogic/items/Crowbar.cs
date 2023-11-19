using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class Crowbar : ItemBase
	{
		public Crowbar() : base(content.inventory.crowbar, Items_Res.Crowbar_Crowbar_Crowbar)
		{
		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Any.Object)
					.Add(Verbs.Use, UseCrowbarWith)
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript());
		}

		private Script UseCrowbarWith(InteractionContext context)
		{
			return Game.Ego.StartScript(UseCrowbarWithScript(context));
		}

		private IEnumerator UseCrowbarWithScript(InteractionContext context)
		{
			using (Game.CutsceneBlock())
			{
				if (context.Secondary is ItemBase)
				{
					yield return Game.Ego.Say(Items_Res.Holding_that_in_my_hands_I_cant_apply_force_on_that);
				}
				else
				{
					yield return Game.Ego.GoTo(context.Secondary);
					yield return Game.Ego.StartUse();
					Game.PlaySoundEffect(content.audio.crowbar_break);
					Game.Ego.Inventory.RemoveItem(this);
					Game.Ego.Inventory.AddItem<CrowbarBroken>();
					yield return Game.Ego.StopUse();
					yield return Delay.Seconds(2f);
					yield return Game.Ego.Say(Items_Res.It_broke_in_two_parts);
					yield return Game.Ego.Say(Items_Res.I_cant_believe_what_just_happened);
				}
			}

		}

		private IEnumerator LookScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.Reliable_and_robust);
			}
		}
	}
}
