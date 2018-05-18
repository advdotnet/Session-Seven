using System;

namespace SessionSeven.GUI.Dialog
{
    [Serializable]
    public class BaseOption
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public static BaseOption None
        {
            get
            {
                return new BaseOption();
            }
        }
    }
}
