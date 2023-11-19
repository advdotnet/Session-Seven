using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Office
{
	[Serializable]
	public class WaterDay : Water
	{
		public override Vector2 GetPosition() { return new Vector2(0, 247); }
		public override string GetImagePath() { return content.rooms.office.anim.day._path_; }
	}

	[Serializable]
	public class WaterNight : Water
	{
		public override Vector2 GetPosition() { return new Vector2(0, 261); }
		public override string GetImagePath() { return content.rooms.office.anim.night._path_; }
	}

	[Serializable]
	public abstract class Water : Entity
	{
		public abstract Vector2 GetPosition();
		public abstract string GetImagePath();

		[NonSerialized]
		private Texture2D[] _textures;

		public Water()
		{
			Sprite
				.Create(this);

			Transform
				.Create(this)
				.SetPosition(GetPosition())
				.SetZ(1);

		}

		public override void OnLoadContent(ContentLoader content)
		{
			_textures = new Texture2D[20];

			for (var i = 0; i < _textures.Length; i++)
			{
				_textures[i] = content.Load<Texture2D>(GetImagePath() + i);
			}

			base.OnLoadContent(content);
		}

		private int _current = 1;
		private int _oldFrame = -1;

		public override void OnUpdate()
		{
			base.OnUpdate();

			_current++;

			var frame = _current / 7 % 20;

			// Optimization for skipping cutscenes
			if (null != World.Get<SkipContent>().SkipCutscene && !World.Get<SkipContent>().SkipCutscene.Enabled)
			{
				if (_oldFrame != frame)
				{
					_oldFrame = frame;
					Get<Sprite>().SetTexture(_textures[frame]);
				}
			}
		}
	}
}
