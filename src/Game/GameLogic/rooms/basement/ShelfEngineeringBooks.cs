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
	public class ShelfEngineeringBooks : Entity
	{
		public ShelfEngineeringBooks()
		{
			Interaction
				.Create(this)
				.SetPosition(1020, 295)
				.SetDirection(Directions8.Right)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.books)
				.AddRectangle(1043, 178, 57, 49)
				.AddRectangle(1039, 153, 28, 12);

			Transform
				.Create(this)
				.SetZ(Shelf.Z + 1);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript())
					.Add(Verbs.Use, UseScript())
					.Add(Verbs.Pick, PickScript());
		}

		private IEnumerator UseScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_have_more_important_things_to_tend_to);
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.My_old_engineering_textbooks);
				yield return Game.Ego.Say(Basement_Res.I_keep_them_for_sentimental_reasons);
				Get<HotspotRectangle>().SetCaption(Basement_Res.engineering_books);
				Tree.Basement.ShelfSingleBook.Enabled = true;
			}
		}

		private IEnumerator PickScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.I_dont_need_them_any_more);
			}
		}
	}


}
