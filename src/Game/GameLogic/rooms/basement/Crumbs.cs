using Microsoft.Xna.Framework;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class CrumbsLeft : Crumbs
	{
		protected override Rectangle GetHotspotRectangle() { return new Rectangle(185, 233, 32, 10); }
		protected override Vector2 GetInteractionPosition() { return new Vector2(198, 258); }
	}

	[Serializable]
	public class CrumbsRight : Crumbs
	{
		protected override Rectangle GetHotspotRectangle() { return new Rectangle(774, 225, 20, 11); }
		protected override Vector2 GetInteractionPosition() { return new Vector2(783, 251); }
	}

	[Serializable]
	public abstract class Crumbs : Entity
	{
		public Crumbs()
		{
			Interaction
				.Create(this)
				.SetPosition(GetInteractionPosition())
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.crumbs)
				.AddRectangle(GetHotspotRectangle());

			Enabled = false;
		}

		protected abstract Rectangle GetHotspotRectangle();
		protected abstract Vector2 GetInteractionPosition();

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript());
		}

		private bool LookedAt { get; set; }

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (!LookedAt)
				{
					yield return Game.Ego.Say(Basement_Res.Since_when_has_Cynthia_allowed_crumbs_into_her_house);
					LookedAt = true;
				}

				yield return Game.Ego.Say(Basement_Res.Looks_like_plastic_and_paper);
			}
		}
	}
}
