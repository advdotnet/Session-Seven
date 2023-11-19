using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class Desk : Entity
	{
		public Desk()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.desk);

			HotspotSprite
				.Create(this)
				.SetCaption(Basement_Res.desk)
				.SetPixelPerfect(true);

			Transform
				.Create(this)
				.SetZ(262);

			Interaction
				.Create(this)
				.SetPosition(127, 276)
				.SetDirection(Directions8.Left)
				.SetGetInteractionsFn(GetInteractions);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Its_Cynthias_work_desk);
			}
		}
	}
}
