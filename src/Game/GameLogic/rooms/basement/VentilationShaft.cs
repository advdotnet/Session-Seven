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
	public class VentilationShaft : Entity
	{
		public VentilationShaft()
		{
			Interaction
				.Create(this)
				.SetPosition(449, 294)
				.SetDirection(Directions8.Right)
				.SetGetInteractionsFn(GetInteractions);

			HotspotMesh
				.Create(this)
				.SetCaption(Basement_Res.ventilation_shaft)
				.SetMesh(CreateMesh());

			Enabled = false;
		}

		private Mesh<TriangleVertexData> CreateMesh()
		{
			var points = new PathVertex[11]
			{
				new PathVertex(539, 0),
				new PathVertex(528, 34),
				new PathVertex(529, 61),
				new PathVertex(572, 60),
				new PathVertex(576, 50),
				new PathVertex(648, 56),
				new PathVertex(650, 41),
				new PathVertex(580, 38),
				new PathVertex(588, 15),
				new PathVertex(601, 15),
				new PathVertex(607, 0)
			};

			var indices = new int[27]
			{
				0, 8, 10, 8, 9, 10,
				0, 1, 8, 1, 7, 8,
				1, 2, 7, 2, 4, 7,
				2, 3, 4, 5, 6, 7,
				4, 5, 7
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
				yield return Game.Ego.Say(Basement_Res.A_ventilation_shaft_At_least_I_have_AC_down_here);
			}
		}
	}
}
