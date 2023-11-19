using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class SceneTransparentFloor : Entity
	{
		public SceneTransparentFloor()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.scene_transparent_floor);

			Transform
				.Create(this)
				.SetZ(0.5f);
		}
	}
}
