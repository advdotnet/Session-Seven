using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Actors
{

	[Serializable]
	public class BloodDropEmitter : Component, IContent, IUpdate
	{
		private readonly List<RyanBlooddrop> _drops = new List<RyanBlooddrop>();
		private bool _spawnDrops = true;
		private int _totalBloodDrops = 0;
		private int _bloodDropCounter = 0;
		private int _commentCounter = -1;
		private int _comments = 0;
		[NonSerialized]
		private Texture2D _texture;
		public Texture2D Texture => _texture;

		public BloodDropEmitter()
		{
			_drops = new List<RyanBlooddrop>();
			Enabled = true;
		}

		public void Start()
		{
			_spawnDrops = true;
			Enabled = true;
		}

		public void Stop()
		{
			_spawnDrops = false;
		}

		private Randomizer Randomizer => Entity.World.Get<Randomizer>();

		public bool Enabled { get; set; }
		public float UpdateOrder { get; set; }

		public void LoadContent(ContentLoader content)
		{
			_texture = content.Load<Texture2D>(SessionSeven.content.rooms.basement.blooddrops);
			foreach (var drop in _drops)
			{
				drop.Get<Sprite>().SetTexture(_texture, 16, 1);
			}
		}

		public void UnloadContent()
		{

		}

		public void Update()
		{
			OnUpdateDrops();
			OnUpdateComments();
		}

		public void ResetCommentCounter()
		{
			_commentCounter = Randomizer.CreateInt(1000, 1500);
		}

		private void OnUpdateComments()
		{
			const string commentBloodScript = "commentblood";

			if (-1 == _commentCounter && !Game.Ego.Get<Scripts>().HasScript(commentBloodScript))
			{
				ResetCommentCounter();
			}

			if (0 == _commentCounter)
			{
				if (Entity.World.Interactive &&
					Verbs.Walk.Equals(Tree.GUI.Interaction.Scene.SelectedVerb) &&
					null == Tree.GUI.Interaction.Scene.SelectedPrimary &&
					!Game.Ego.Get<Scripts>().HasScript(commentBloodScript))
				{
					_commentCounter = -1;
					Game.Ego.Stop();
					Game.Ego.StartScript(CommentScript(), commentBloodScript);
				}
			}
			else if (_commentCounter > 0 && Entity.World.Interactive)
			{
				_commentCounter--;
			}
		}

		private IEnumerator CommentScript()
		{
			Game.Ego.Turn(Directions4.Down);
			Entity.World.Interactive = false;
			string comment;
			switch (_comments)
			{
				case 0:
					comment = Basement_Res.My_hand_is_wounded;
					break;
				case 1:
					comment = Basement_Res.My_hand_is_losing_blood;
					break;
				case 2:
					comment = Basement_Res.My_hand_is_still_losing_blood;
					break;
				case 3:
					comment = Basement_Res.I_need_to_dress_my_hand;
					break;
				default:
					comment = Basement_Res.I_should_have_a_look_at_the_medical_cabinet_over_there_to_dress_my_wound;
					break;
			}

			if (Game.Ego.Inventory.HasItem<InventoryItems.Bandages>() || Game.Ego.Inventory.HasItem<InventoryItems.BandagesCut>())
			{
				comment = Basement_Res.I_should_use_those_bandages_to_dress_my_wound;
			}

			yield return Game.Ego.Say(comment);
			_comments++;
			Entity.World.Interactive = true;
			Tree.GUI.Interaction.Scene.Reset();
		}

		private void OnUpdateDrops()
		{
			if (0 == _bloodDropCounter && _spawnDrops)
			{
				_bloodDropCounter = Randomizer.CreateInt(80, 160);
				SpawnDrop();
			}

			UpdateDrops();
			_bloodDropCounter--;
			if (_drops.Count == 0 && !_spawnDrops)
			{
				Enabled = false;
			}
		}

		private void SpawnDrop()
		{
			// spawn new  blooddrop
			var frame = (byte)Randomizer.CreateInt(15);
			var ttl = 500 + Randomizer.CreateInt(50, 150);
			var transform = Get<Transform>();
			var directionDisplacment = Vector2.Zero;

			switch (transform.Direction4)
			{
				case Directions4.Left: directionDisplacment = new Vector2(-10, -25 + 9); break;
				case Directions4.Up: directionDisplacment = new Vector2(10, -7); break;
				case Directions4.Down: directionDisplacment = new Vector2(-23, -7); break;
				case Directions4.Right: directionDisplacment = new Vector2(-11, -11 + 9); break;
			}

			// some random displacement
			var randomDisplacement = new Vector2(Randomizer.CreateInt(-3, 3), Randomizer.CreateInt(-2, 2));
			var position = transform.Position + directionDisplacment + randomDisplacement;

			var id = typeof(RyanBlooddrop).FullName + "_" + _totalBloodDrops;
			var z = .0f;

			switch (transform.Direction4)
			{
				case Directions4.Up:
				case Directions4.Down: z = transform.Z + 1; break;
				case Directions4.Left: z = transform.Z - 5; break;
				case Directions4.Right: z = transform.Z + 5; break;
			}

			var bloodDrop = new RyanBlooddrop(position, frame, ttl, id, z);
			_drops.Add(bloodDrop);

			Entity.DrawScene.Push(bloodDrop);
			_totalBloodDrops++;
		}

		private void UpdateDrops()
		{
			for (var i = _drops.Count - 1; i >= 0; i--)
			{
				_drops[i].TimeToLive--;
				if (_drops[i].TimeToLive <= 0)
				{
					_drops[i].UpdateScene.Pop(_drops[i]);
					_drops.RemoveAt(i);
				}
			}
		}

		public static BloodDropEmitter Create(Entity entity)
		{
			return entity.Add<BloodDropEmitter>();
		}
	}
}
