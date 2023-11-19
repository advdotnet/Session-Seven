using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{

	[Serializable]
	public class Scene : Location
	{
		public Scene() : base(content.rooms.basement.scene)
		{
			this.AutoAddEntities();

			ScenePath
				.Create(this)
				.SetPathFile(content.rooms.basement.path);

			DrawOrder = 20;
		}
	}
}
