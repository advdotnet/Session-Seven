using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Office
{
	/// <summary>
	/// Image of the table to obstruct Ryan's pixelated feet.
	/// </summary>
	[Serializable]
	public class Table : Entity
	{
		public Table()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.office.table);

			Transform
				.Create(this)
				.SetZ(400);
		}
	}
}
