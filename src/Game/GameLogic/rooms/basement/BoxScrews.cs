using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class BoxScrews : Entity
	{
		public BoxScrews()
		{
			Interaction
				.Create(this)
				.SetPosition(573, 260)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.box)
				.AddRectangle(558, 206, 37, 18);

			Transform
				.Create(this)
				.SetZ(Workbench.Z + 1);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Open, OpenScript())
					.Add(Verbs.Look, LookScript());

		}

		private IEnumerator OpenScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.There_are_just_some_screws_and_nail_in_there);
				yield return Game.Ego.Say(Basement_Res.I_dont_need_them);
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				Get<HotspotRectangle>().SetCaption(Basement_Res.box_with_screws_and_nails);
				yield return Game.Ego.Say(Basement_Res.Im_keeping_some_screws_and_nails_in_this_box);
			}
		}
	}


}
