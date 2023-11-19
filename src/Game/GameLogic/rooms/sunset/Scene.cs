using STACK;
using STACK.Components;
using System;

namespace SessionSeven.SunSet
{

	[Serializable]
	public class Scene : Location
	{
		public Scene() : base(content.rooms.sunset.scene)
		{
			this.AutoAddEntities();

			ScenePath
				.Create(this)
				.SetPathFile(content.rooms.sunset.path);

			DrawOrder = 22;
		}
	}
}
