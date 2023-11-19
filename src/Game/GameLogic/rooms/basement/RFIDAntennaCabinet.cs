using Microsoft.Xna.Framework;
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
	public class RFIDAntennaCabinet : Entity
	{
		public readonly Rectangle Collider = new Rectangle(684, 37, 27, 30);

		public RFIDAntennaCabinet()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.rfidantennacabinet, 5);

			HotspotSprite
				.Create(this)
				.SetCaption(Basement_Res.cardboard)
				.SetPixelPerfect(true);

			Transform
				.Create(this)
				.SetPosition(688, 49)
				.SetZ(1);

			Interaction
				.Create(this)
				.SetPosition(696, 248)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			Scripts
				.Create(this);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Tree.InventoryItems.Drone)
					.Add(Verbs.Use, UseDroneScript(), Game.Ego)
				.For(Game.Ego)
					.Add(Verbs.Open, OpenScript())
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Pick, PickScript());
		}

		private bool _usedDrone = false;

		public Script StartFallDownScript()
		{
			return Get<Scripts>().Start(FallDownScript());
		}

		public bool FellDown => Get<Sprite>().CurrentFrame > 1;

		private IEnumerator FallDownScript()
		{
			yield return Delay.Seconds(0.1f);
			Get<Sprite>().CurrentFrame = 2;
			yield return Delay.Seconds(0.1f);
			Get<Sprite>().CurrentFrame = 3;
			yield return Delay.Seconds(0.1f);
			Get<Sprite>().CurrentFrame = 4;
			yield return Delay.Seconds(0.1f);
			Get<Sprite>().CurrentFrame = 5;
		}

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_should_take_it_first);
			}
		}

		private IEnumerator UseDroneScript()
		{
			Game.Ego.Turn(this);
			using (Game.CutsceneBlock())
			{
				if (FellDown)
				{
					yield return Game.Ego.Say(Basement_Res.I_should_be_able_to_reach_it_now);
					yield return Game.Ego.Say(Basement_Res.No_need_to_fiddle_around_with_the_drone);
					yield break;

				}
				if (!_usedDrone)
				{
					yield return Game.Ego.Say(Basement_Res.Maybe_I_can_make_the_drone_bump_into_the_cardboard);
					yield return Delay.Seconds(0.5f);
					_usedDrone = true;
				}
				yield return Game.Ego.GoTo(802, 262);
				Game.Ego.Turn(Directions4.Down);
				yield return Game.Ego.StartUse();
				Game.Ego.Inventory.RemoveItem<InventoryItems.Drone>();
				Tree.Basement.Drone.PlaceOnFloor();
				yield return Game.Ego.StopUse();
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.A_cardboard_without_any_inscription_is_sitting_on_a_box_on_the_top_of_the_cabinet);

				if (Game.Ego.Inventory.HasItem<InventoryItems.RFIDAntennaBoxShelf>() ||
					Game.Ego.Inventory.HasItem<InventoryItems.RFIDAntennaShelf>())
				{
					yield return Game.Ego.Say(Basement_Res.Looks_exactly_like_the_one_I_got_from_the_shelf);
				}
			}
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);

			using (Game.CutsceneBlock())
			{
				if (!FellDown)
				{
					yield return Game.Ego.Say(Basement_Res.Cant_reach_it_with_bare_hands);
				}
				else
				{
					yield return Game.Ego.Say(Basement_Res.Now_I_can_only_just_reach_it);
					yield return Game.Ego.StartUse();
					Enabled = false;
					Game.Ego.Inventory.AddItem<InventoryItems.RFIDAntennaBoxCabinet>();
					Visible = false;
					yield return Game.Ego.StopUse();
				}
			}
		}
	}
}
