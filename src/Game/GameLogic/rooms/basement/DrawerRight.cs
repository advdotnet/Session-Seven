using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using SessionSeven.InventoryItems;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class DrawerRight : Entity
	{
		public bool Unlocked { get; private set; }

		public DrawerRight()
		{
			Interaction
				.Create(this)
				.SetPosition(160, 281)
				.SetDirection(Directions8.Left)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.drawer)
				.AddRectangle(105, 196, 19, 23);

			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.drawerright, 4, 2)
				.SetFrame(1);

			Transform
				.Create(this)
				.SetPosition(98, 194)
				.SetZ(263);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Open, OpenScript())
				.For(Tree.InventoryItems.DrawerKey)
					.Add(Verbs.Use, UseDrawerKeyScript(), Game.Ego)
				.For(Tree.InventoryItems.Paperclip)
					.Add(Verbs.Use, UsePaperclipScript(), Game.Ego)
				.For(Tree.InventoryItems.Paperclips)
					.Add(Verbs.Use, UsePaperclipScript(), Game.Ego)
				.For(Tree.InventoryItems.Hammer)
					.Add(Verbs.Use, UseHammerScript(), Game.Ego);
		}

		private IEnumerator UseHammerScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (!Unlocked)
				{
					yield return Game.Ego.StartUse();
					yield return Game.WaitForSoundEffect(content.audio.hammer_wood);
					yield return Game.Ego.StopUse();
					yield return Game.Ego.Say(Basement_Res.That_didnt_do_anything);
				}
				else
				{
					yield return Game.Ego.Say(Basement_Res.The_drawer_is_unlocked_already);
				}
			}
		}

		private IEnumerator UsePaperclipScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (Unlocked)
				{
					yield return Game.Ego.Say(Basement_Res.The_drawer_is_unlocked_already);
				}
				else
				{
					yield return Game.Ego.Say(Basement_Res.Cant_exert_enough_force_on_this_type_of_lock);
				}
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.The_drawer_belongs_to_my_wifes_old_desk);
			}
		}

		private const float _animationDelay = 0.15f;

		private void SetFrame(byte state)
		{
			var envelopeTaken = Game.Ego.Inventory.HasItem<Envelope>();

			if (envelopeTaken)
			{
				state += 4;
			}

			Get<Sprite>().CurrentFrame = state;
		}

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);

			var tookEnvelope = false;
			var reset = Game.Ego.Inventory.HasItem<Envelope>() || !Unlocked;

			using (Game.CutsceneBlock(reset, reset))
			{
				if (!Unlocked)
				{
					yield return Game.Ego.StartUse();
					yield return Game.WaitForSoundEffect(content.audio.drawer_locked);
					yield return Game.Ego.StopUse();
					yield return Game.Ego.Say(Basement_Res.Locked);
				}
				else
				{
					yield return Game.Ego.StartUse();
					Game.PlaySoundEffect(content.audio.drawer_open);
					yield return Delay.Seconds(_animationDelay * 2);
					SetFrame(2);
					yield return Delay.Seconds(_animationDelay);
					SetFrame(3);
					yield return Delay.Seconds(_animationDelay);
					SetFrame(4);
					yield return Game.Ego.StopUse();

					if (Game.Ego.Inventory.HasItem<Envelope>())
					{
						yield return Game.Ego.Say(Basement_Res.Its_empty);
					}
					else
					{
						yield return Game.Ego.Say(Basement_Res.There_is_an_envelope_and_a_battery_in_the_drawer);
						yield return Game.Ego.StartUse();
						Game.Ego.Inventory.AddItem<Envelope>();
						Game.Ego.Inventory.AddItem<BatteryA>();
						SetFrame(4);
						yield return Game.Ego.StopUse();
						tookEnvelope = true;
					}

					yield return Game.Ego.StartUse();
					Game.PlaySoundEffect(content.audio.drawer_close);
					yield return Delay.Seconds(_animationDelay * 2);
					SetFrame(3);
					yield return Delay.Seconds(_animationDelay * 2);
					SetFrame(2);
					yield return Delay.Seconds(_animationDelay * 2);
					SetFrame(1);
					yield return Game.Ego.StopUse();
				}
			}

			if (tookEnvelope)
			{
				yield return Tree.InventoryItems.Envelope.Open();
			}
		}

		private IEnumerator UseDrawerKeyScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (Unlocked)
				{
					yield return Game.Ego.Say(Basement_Res.The_drawer_is_unlocked_already);
				}
				else
				{
					yield return Game.Ego.StartUse();
					yield return Game.WaitForSoundEffect(content.audio.drawer_unlock);

					yield return Game.Ego.StopUse();

					yield return Delay.Seconds(0.25f);

					yield return Game.Ego.Say(Basement_Res.The_key_works_The_drawer_is_unlocked_now);
					Unlocked = true;
				}
			}
		}
	}


}
