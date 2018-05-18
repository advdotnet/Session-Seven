using SessionSeven.Components;
using System;
using System.Collections.Generic;

namespace SessionSeven.GUI.Dialog
{
    [Serializable]
    public class ScoreOption : BaseOption
    {
        public Dictionary<ScoreType, int> ScoreSet;

        public static new ScoreOption None
        {
            get
            {
                return new ScoreOption();
            }
        }
    }
}
