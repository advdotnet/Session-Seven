using STACK;
using STACK.Components;
using System;

namespace SessionSeven.PaddedCell
{

	[Serializable]
	public class Scene : Location
	{
		public Scene() : base(content.rooms.paddedcell.scene)
		{
			this.AutoAddEntities();

			ScenePath
				.Create(this)
				.SetPathFile(content.rooms.paddedcell.path);

			DrawOrder = 22;
		}
	}
}
