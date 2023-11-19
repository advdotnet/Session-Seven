using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class Guitar : Entity
	{
		public Guitar()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.guitar)
				.SetVisible(false);

			Transform
				.Create(this)
				.SetPosition(128, 129)
				.SetZ(GuitarCase.Z + 1);

			Interaction
				.Create(this)
				.SetPosition(163, 243)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			HotspotSprite
				.Create(this)
				.SetPixelPerfect(true)
				.SetCaption(Basement_Res.guitar);

			Visible = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Tree.InventoryItems.GuitarStrings)
					.Add(Verbs.Use, UseStringsScript(), Game.Ego)
				.For(Game.Ego)
					.Add(Verbs.Use, UseScript())
					.Add(Verbs.Pick, PickScript())
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator UseStringsScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.The_strings_on_the_guitar_are_still_in_pristine_condition);
			}
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_dont_want_to_carry_that_around_now);
			}
		}

		private int _trackNumber = 1;

		private IEnumerator UseScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.StartUse();
				World.Get<AudioManager>().MusicVolume = 0.05f;
				yield return Game.WaitForSoundEffect(content.audio._path_ + "guitar_play" + _trackNumber);
				World.Get<AudioManager>().MusicVolume = 1f;
				yield return Game.Ego.StopUse();

				_trackNumber++;
				if (_trackNumber > 3)
				{
					_trackNumber = 1;
				}
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Its_my_old_guitar_I_used_to_play_every_night_as_Landon_was_going_to_bed);
				yield return Game.Ego.Say(Basement_Res.I_didnt_know_it_was_down_here_now);
			}
		}
	}


}
