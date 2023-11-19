using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using SessionSeven.GUI.PositionSelection;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Basement
{
	[Serializable]
	public class NutsOnFloor : Entity
	{
		[Serializable]
		public class Nut : Entity, IPositionable
		{
			public readonly Vector2 INTERACTIONOFFSET = new Vector2(5, 1);

			[NonSerialized]
			private Path _restrictionPathFloor;

			public Nut(Vector2 position)
			{
				Sprite
					.Create(this)
					.SetImage(content.rooms.basement.nut);

				HotspotSprite
					.Create(this)
					.SetCaption(Basement_Res.nut)
					.SetPixelPerfect(true);

				Interaction
					.Create(this)
					.SetDirection(Directions8.Down)
					.SetPosition(position)
					.SetGetInteractionsFn(GetInteractions);

				Transform
					.Create(this)
					.SetPosition(position)
					.SetZ(position.Y);

				SpriteData
					.Create(this)
					.SetOffset(-3, -2);

				CameraLocked
					.Create(this)
					.SetEnabled(false);
			}

			public bool Placed = false;

			private Interactions GetInteractions()
			{
				return Interactions
					.Create()
					.For(Game.Ego)
						.Add(Verbs.Pick, PickScript())
						.Add(Verbs.Look, LookScript());
			}

			private IEnumerator LookScript()
			{
				using (Game.CutsceneBlock())
				{
					Game.Ego.Turn(this);
					yield return Game.Ego.Say(Basement_Res.A_nut_is_lying_on_the_floor);
				}
			}

			private IEnumerator PickScript()
			{
				using (Game.CutsceneBlock())
				{
					Game.Ego.Turn(this);
					yield return Game.Ego.Say(Basement_Res.I_have_plenty_more_of_them);
				}
			}

			public void BeginPosition(int mode)
			{
				Visible = true;
				Enabled = true;
				Get<SpriteData>().Color = new Color(0, 0, 1f, 0.3f);
				Get<CameraLocked>().Enabled = true;
			}

			public void EndPosition(int mode)
			{
				Get<SpriteData>().Color = Color.White;
				Get<CameraLocked>().Enabled = false;
				Visible = false;
				Enabled = false;
			}

			public void SetPosition(Vector2 position, int mode)
			{
				var transformedPosition = DrawScene.Get<Camera>().TransformInverse(position);
				if (null == _restrictionPathFloor)
				{
					_restrictionPathFloor = CreateRestrictionPathFloor();
				}

				var newPosition = _restrictionPathFloor.GetClosestPoint(transformedPosition);
				var transform = Get<Transform>();
				transform.Position = newPosition;
				Get<Interaction>().Position = newPosition;
				transform.Z = newPosition.Y;
			}

			private Path CreateRestrictionPathFloor()
			{
				var points = new PathVertex[12];

				points[0] = new PathVertex(181, 248);
				points[1] = new PathVertex(109, 348);
				points[2] = new PathVertex(282, 396);
				points[3] = new PathVertex(872, 392);
				points[4] = new PathVertex(1042, 347);
				points[5] = new PathVertex(983, 278);
				points[6] = new PathVertex(814, 276);
				points[7] = new PathVertex(791, 256);
				points[8] = new PathVertex(660, 252);
				points[9] = new PathVertex(650, 274);
				points[10] = new PathVertex(387, 278);
				points[11] = new PathVertex(358, 248);

				var indices = new int[30];
				indices[0] = 0; indices[1] = 1; indices[2] = 2;
				indices[3] = 2; indices[4] = 3; indices[5] = 4;
				indices[6] = 2; indices[7] = 4; indices[8] = 5;
				indices[9] = 2; indices[10] = 5; indices[11] = 6;
				indices[12] = 2; indices[13] = 6; indices[14] = 9;
				indices[15] = 2; indices[16] = 10; indices[17] = 11;
				indices[18] = 0; indices[19] = 2; indices[20] = 11;
				indices[21] = 2; indices[22] = 9; indices[23] = 10;
				indices[24] = 7; indices[25] = 8; indices[26] = 9;
				indices[27] = 6; indices[28] = 7; indices[29] = 9;

				return new Path(points, indices, 1.0f, 0.6f);
			}

			public void Place()
			{
				Visible = true;
				Enabled = true;
				Placed = true;
			}
		}

		public List<Nut> Nuts = new List<Nut>();

		public NutsOnFloor()
		{

		}

		public Nut AddNut(int x, int y)
		{
			var nut = new Nut(new Vector2(x, y));
			DrawScene.Push(nut);
			Nuts.Add(nut);
			return nut;
		}

		/// <summary>
		/// Returns the first nut in the watch area
		/// </summary>
		/// <returns>null if there is none</returns>
		public Nut GetNutInWatchArea()
		{
			foreach (var nut in Nuts)
			{
				if (nut.Placed && Tree.Basement.MouseHole.WatchArea.Contains(nut.Get<Transform>().Position))
				{
					return nut;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the closest nut to the given position.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="withinDistance">if != -1 only the given distance is considered</param>
		/// <returns></returns>
		public Nut GetClosestNut(Vector2 position, float withinDistance = -1)
		{
			Nut result = null;
			var closestDistance = float.MaxValue;

			foreach (var nut in Nuts)
			{
				if (nut.Placed)
				{
					var distance = (nut.Get<Transform>().Position - position).Length();

					if (distance < closestDistance && (withinDistance == -1 || distance <= withinDistance))
					{
						result = nut;
						closestDistance = distance;
					}
				}
			}

			return result;
		}

		public void RemoveNut(Nut nut)
		{
			DrawScene.Pop(nut);
			Nuts.Remove(nut);
		}

		public void RemoveAllNuts()
		{
			for (var i = Nuts.Count - 1; i >= 0; i--)
			{
				DrawScene.Pop(Nuts[i]);
				Nuts.RemoveAt(i);
			}
		}
	}
}
