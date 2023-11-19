using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Office
{

	[Serializable]
	public class Scene : Location
	{
		private static readonly string _earlyImage = content.rooms.office.scene;
		private static readonly string _lateImage = content.rooms.office.scenenight;

		public Scene() : base(_earlyImage)
		{
			this.AutoAddEntities();

			DrawOrder = 20;
		}

		public bool IsEarly()
		{
			return _earlyImage == Background.Get<Sprite>().Image;
		}

		public void SetupEarly()
		{
			Background.Get<Sprite>().SetImage(_earlyImage);
			Tree.Office.WaterDay.Visible = true;
			Tree.Office.WaterDay.Enabled = true;
			Tree.Office.WaterNight.Visible = false;
			Tree.Office.WaterNight.Enabled = false;
		}

		public void SetupLate()
		{
			Background.Get<Sprite>().SetImage(_lateImage);
			Tree.Office.WaterDay.Visible = false;
			Tree.Office.WaterDay.Enabled = false;
			Tree.Office.WaterNight.Visible = true;
			Tree.Office.WaterNight.Enabled = true;
		}
	}
}
