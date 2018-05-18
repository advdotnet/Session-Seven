using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Office
{

    [Serializable]
    public class Scene : Location
    {
        static string EARLY_IMAGE = content.rooms.office.scene;
        static string LATE_IMAGE = content.rooms.office.scenenight;

        public Scene() : base(EARLY_IMAGE)
        {
            this.AutoAddEntities();

            DrawOrder = 20;
        }

        public bool IsEarly()
        {
            return (EARLY_IMAGE == Background.Get<Sprite>().Image);
        }

        public void SetupEarly()
        {
            Background.Get<Sprite>().SetImage(EARLY_IMAGE);
            Tree.Office.WaterDay.Visible = true;
            Tree.Office.WaterDay.Enabled = true;
            Tree.Office.WaterNight.Visible = false;
            Tree.Office.WaterNight.Enabled = false;
        }

        public void SetupLate()
        {
            Background.Get<Sprite>().SetImage(LATE_IMAGE);
            Tree.Office.WaterDay.Visible = false;
            Tree.Office.WaterDay.Enabled = false;
            Tree.Office.WaterNight.Visible = true;
            Tree.Office.WaterNight.Enabled = true;
        }
    }
}
