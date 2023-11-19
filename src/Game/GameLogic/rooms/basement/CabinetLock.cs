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
	public class CabinetLock : Entity
	{
		public CabinetLock()
		{
			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res._lock)
				.SetRectangle(694, 165, 10, 14);

			Interaction
				.Create(this)
				.SetPosition(698, 250)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			Transform
				.Create(this)
				.SetPosition(695, 165)
				.SetZ(3);

			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.cabinetlock);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Tree.InventoryItems.Hammer)
					.Add(Verbs.Use, UseHammerScript(), Game.Ego)
				.For(Tree.Basement.DrillingMachine)
					.Add(Verbs.Use, UseDrillingMachineScript(), Game.Ego)
				.For(Tree.InventoryItems.Paperclips)
					.Add(Verbs.Use, UsePaperclipsScript(), Game.Ego)
				.For(Tree.InventoryItems.Paperclip)
					.Add(Verbs.Use, UsePaperclipScript(), Game.Ego)
				.For(Tree.InventoryItems.DrawerKey)
					.Add(Verbs.Use, UseDrawerKeyScript(), Game.Ego)
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Open, OpenScript())
					.Add(Verbs.Push, MoveScript())
					.Add(Verbs.Pull, MoveScript())
					.Add(Verbs.Pick, MoveScript())
					.Add(Verbs.Use, UseScript());
		}

		private IEnumerator UseHammerScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.StartUse();
				yield return Game.WaitForSoundEffect(content.audio.hammer_once);
				yield return Game.Ego.StopUse();
				yield return Game.Ego.Say(Basement_Res.That_didnt_do_anything);
			}
		}

		private IEnumerator UseDrillingMachineScript()
		{
			Game.Ego.Turn(this);
			using (Game.CutsceneBlock())
			{
				if (Tree.Basement.DrillingMachineCable.PluggedIn)
				{
					yield return Game.Ego.Say(Basement_Res.The_cable_is_not_long_enough);
				}
				else
				{
					yield return Game.Ego.Say(Basement_Res.No_power);
				}
			}
		}

		public bool Opened => !Visible;

		private IEnumerator UsePaperclipsScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (!_pickLockComment)
				{
					yield return Game.Ego.Say(Basement_Res.This_could_work_actually);
					yield return Game.Ego.Say(Basement_Res.We_learned_how_these_locks_function_and_how_to_pick_them_in_university);
					_pickLockComment = true;
				}

				yield return Game.Ego.StartUse();
				Visible = false;
				yield return Game.WaitForSoundEffect(content.audio.lock_pick);
				yield return Game.Ego.StopUse();
				yield return Game.Ego.Say(Basement_Res.Got_it);
			}
		}

		private IEnumerator UsePaperclipScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (!_pickLockComment)
				{
					yield return Game.Ego.Say(Basement_Res.This_could_work_actually);
					yield return Game.Ego.Say(Basement_Res.We_learned_how_these_locks_function_and_how_to_pick_them_in_university);
					yield return Game.Ego.Say(Basement_Res.I_need_a_second_paper_clip_however);
					_pickLockComment = true;
				}
				else
				{
					yield return Game.Ego.Say(Basement_Res.I_need_a_second_paper_clip);
				}
			}
		}

		private bool _pickLockComment = false;

		private IEnumerator UseDrawerKeyScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Use();
				yield return Game.Ego.Say(Basement_Res.This_key_does_not_fit);
			}
		}

		private IEnumerator UseScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Its_locked);
			}
		}

		private IEnumerator MoveScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Use();
				yield return Game.Ego.Say(Basement_Res.Cant_move_it_as_long_as_it_is_locked);
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.This_cabinet_door_has_a_lock_on_it);
			}
		}

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_need_a_key_or_appropriate_tooling);
			}
		}
	}
}
