using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class ShelfComicBox : Entity
	{
		public ShelfComicBox()
		{
			Interaction
				.Create(this)
				.SetPosition(1020, 295)
				.SetDirection(Directions8.Right)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.box)
				.AddRectangle(1047, 220, 29, 39);

			Transform
				.Create(this)
				.SetZ(Shelf.Z + 1);

			Enabled = false;
		}

		private Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Open, OpenScript())
					.Add(Verbs.Pick, PickScript());
		}

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (!LookedAt)
				{
					yield return Game.Ego.StartScript(LookScript(false));
				}
				yield return Game.Ego.StartUse();

				yield return Game.WaitForSoundEffect(content.audio.rustling1);

				yield return Game.Ego.StopUse();

				yield return Game.Ego.Say(Basement_Res.Nothing_in_there_but_comic_books);
				yield return Game.Ego.Say(Basement_Res.Theyre_looking_pretty_torn_up);
			}
		}

		public bool LookedAt { get; private set; }

		private IEnumerator LookScript(bool reset = true)
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock(reset))
			{
				yield return Game.Ego.Say(Basement_Res.In_this_box_we_store_some_of_Landons_old_comic_books);
				LookedAt = true;
				Get<HotspotRectangle>().SetCaption(Basement_Res.box_with_comic_books);
			}
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (!LookedAt)
				{
					yield return Game.Ego.StartScript(LookScript(false));
				}
				yield return Game.Ego.Say(Basement_Res.I_have_more_important_things_to_tend_to);
			}
		}
	}
}
