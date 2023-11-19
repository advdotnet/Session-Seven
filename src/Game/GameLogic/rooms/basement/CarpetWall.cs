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
	public class CarpetWall : Entity
	{
		public CarpetWall()
		{
			Interaction
				.Create(this)
				.SetPosition(198, 246)
				.SetDirection(Directions8.Left)
				.SetGetInteractionsFn(GetInteractions);

			HotspotMesh
				.Create(this)
				.SetCaption(Basement_Res.rugs)
				.SetMesh(CreateMesh());

			Enabled = false;
		}

		private Mesh<TriangleVertexData> CreateMesh()
		{
			var points = new PathVertex[9]
			{
				new PathVertex(84, 84),
				new PathVertex(107, 83),
				new PathVertex(128, 169),
				new PathVertex(161, 222),
				new PathVertex(152, 236),
				new PathVertex(125, 237),
				new PathVertex(94, 183),
				new PathVertex(95, 126),
				new PathVertex(138, 208)
			};

			var indices = new int[21]
			{
				0, 1, 2, 2, 6, 7,
				0, 2, 7, 4, 6, 8,
				4, 5, 6, 2, 6, 8,
				3, 4, 8
			};

			return new Mesh<TriangleVertexData>(points, indices);
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
				yield return Game.Ego.Say(Basement_Res.Some_of_our_failed_statement_rugs);
				yield return Game.Ego.Say(Basement_Res.It_took_Cynthia_almost_a_year_to_finally_be_happy_with_the_design_scheme_of_the_house);
			}
		}
	}
}
