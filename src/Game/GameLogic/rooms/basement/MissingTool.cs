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
	public class MissingTool : Entity
	{
		public MissingTool()
		{
			Interaction
				.Create(this)
				.SetPosition(517, 264)
				.SetDirection(Directions8.Up)
				.SetGetInteractionsFn(GetInteractions);

			HotspotRectangle
				.Create(this)
				.SetCaption(Basement_Res.missing_tool)
				.AddRectangle(479, 95, 12, 45);

			Transform
				.Create(this)
				.SetZ(ToolBar.Z + 1);

			Enabled = false;
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Game.Ego)
					.Add(Verbs.Push, MissingCommentScript())
					.Add(Verbs.Pull, MissingCommentScript())
					.Add(Verbs.Pick, MissingCommentScript())
					.Add(Verbs.Talk, TalkScript())
					.Add(Verbs.Look, LookScript());

		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				if (Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Six))
				{
					yield return Game.Ego.Say(Basement_Res.A_wrench_is_missing);
				}
				else
				{
					yield return Game.Ego.Say(Basement_Res.A_tool_in_my_collection_is_missing_I_wonder_which_it_might_be);
				}
			}
		}

		private IEnumerator MissingCommentScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.The_tool_is_missing);
			}
		}

		private IEnumerator TalkScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Here_I_find_myself_in_the_basement_trying_to_talk_to_nonexistent_objects);
				yield return Game.Ego.Say(Basement_Res.How_could_this_happen);
			}
		}
	}


}
