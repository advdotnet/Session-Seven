﻿using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
	[Serializable]
	public class Blanket : ItemBase
	{
		public Blanket() : base(content.inventory.blanket, Items_Res.Blanket_Blanket_PicnicBlanket, true, false)
		{

		}

		protected override Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Use, UseScript())
					.Add(Verbs.Look, LookScript(true))
				.For(Tree.InventoryItems.Scissors)
					.Add(Verbs.Use, UseScissorsScript(), Game.Ego);
		}

		public bool LookedAt { get; private set; }

		public Script LookAt()
		{
			return Game.Ego.StartScript(LookScript(false));
		}

		private IEnumerator LookScript(bool updateLabel)
		{
			using (Game.CutsceneBlock(true, true, updateLabel))
			{
				var startSession = Game.Ego.Inventory.HasItem<Envelope>() &&
					Tree.InventoryItems.Envelope.ReadLetter &&
					!Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Three);

				if (startSession)
				{
					Game.PlayBasementEndSong();
				}
				else if (!LookedAt)
				{
					Game.PlaySoundEffect(content.audio.puzzle);
				}

				yield return Game.Ego.Say(Items_Res.Our_old_picnic_blanket_wow_I_didnt_know_we_kept_it);
				yield return Game.Ego.Say(Items_Res.Cynthia_and_me_spent_much_time_on_that_together_especially_before_Landon_was_born);

				LookedAt = true;

				if (startSession)
				{
					yield return Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.Three);
				}
			}
		}

		private IEnumerator UseScript()
		{
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Items_Res.No_picnic_for_now);
			}
		}

		private IEnumerator UseScissorsScript()
		{
			using (Game.CutsceneBlock())
			{
				switch (Game.Ego.Get<Score>().GetScoreTypeResult())
				{
					case ScoreType.Freedom:
						yield return Game.Ego.Say(Items_Res.No_There_are_many_good_memories_attached_to_it);
						break;
					case ScoreType.Jail:
					case ScoreType.Insanity:
						yield return Game.Ego.Say(Items_Res.Id_love_to_but_I_have_more_important_things_to_tend_to);
						break;
				}

			}
		}
	}
}
