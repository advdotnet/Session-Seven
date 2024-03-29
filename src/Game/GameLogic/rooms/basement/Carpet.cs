﻿using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using StarFinder;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class Carpet : Entity
	{
		public const int Z = 2;
		public Carpet()
		{
			Interaction
				.Create(this)
				.SetPosition(397, 275)
				.SetWalkToClickPosition(true)
				.SetDirection(Directions8.Left)
				.SetGetInteractionsFn(GetInteractions);

			HotspotMesh
				.Create(this)
				.SetCaption(Basement_Res.carpet)
				.SetMesh(CreateCarpetMesh());

			Transform
				.Create(this)
				.SetZ(Z);

			Enabled = false;
		}

		private Mesh<TriangleVertexData> CreateCarpetMesh()
		{
			var points = new PathVertex[4]
			{
				new PathVertex(155, 341),
				new PathVertex(373, 342),
				new PathVertex(382, 251),
				new PathVertex(216, 250)
			};

			var indices = new int[6] { 0, 1, 3, 1, 2, 3 };

			return new Mesh<TriangleVertexData>(points, indices);
		}

		public Interactions GetInteractions()
		{
			return Interactions
				.Create()
				.For(Tree.InventoryItems.Scissors)
					.Add(Verbs.Use, UseScissorsScript(), Game.Ego)
				.For(Tree.InventoryItems.Drone)
					.Add(Verbs.Use, UseDroneScript(), Game.Ego)
				.For(Game.Ego)
					.Add(Verbs.Look, LookScript());
		}

		private IEnumerator UseScissorsScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Cynthia_would_kill_me);
			}
		}

		private IEnumerator UseDroneScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.No_time_to_play);
			}
		}

		private IEnumerator LookScript()
		{
			yield return Game.Ego.GoTo(this);
			using (Game.CutsceneBlock())
			{
				yield return Game.Ego.Say(Basement_Res.Cynthia_would_kill_me_if_she_could_see_the_mess_Ive_made_on_her_carpet);

				var currentScoreIsFreedom = ScoreType.Freedom == Game.Ego.Get<Score>().GetScoreTypeResult();
				var forgaveCynthia = Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Three) &&
					Tree.Cutscenes.Director.ForgaveCynthia;

				if (currentScoreIsFreedom || forgaveCynthia)
				{
					yield return Game.Ego.Say(Basement_Res.I_hope_shes_okay);
				}
			}
		}
	}
}
