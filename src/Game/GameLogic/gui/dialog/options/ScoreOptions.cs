using SessionSeven.Components;
using System;
using System.Collections.Generic;

namespace SessionSeven.GUI.Dialog
{
    /// <summary>
    /// Dialog options that are associated with one or more <see cref="ScoreType"/> points
    /// </summary>
    [Serializable]
    public class ScoreOptions : BaseOptions<ScoreOption>
    {
        private ScoreOptions() { }

        public static ScoreOptions Create()
        {
            return new ScoreOptions();
        }

        public ScoreOptions Add(int id, string text, ScoreType scoreType, int scorePoints)
        {
            Add(id, text, new Dictionary<ScoreType, int>()
                {
                    { scoreType, scorePoints }
                });

            return this;
        }

        public ScoreOptions Add(int id, string text, Dictionary<ScoreType, int> scores)
        {
            if (id == -1)
            {
                throw new ArgumentException("Option.ID must not equal -1.");
            }

            OptionSet.Add(new ScoreOption(id, text, scores));

            return this;
        }

        public ScoreOptions Add(int id, string text, ScoreType scoreType, int scorePoints, Func<bool> predicate)
        {
            if (predicate())
            {
                Add(id, text, scoreType, scorePoints);
            }

            return this;
        }

        public ScoreOptions RemoveByID(int id)
        {
            for (int i = 0; i < OptionSet.Count; i++)
            {
                if (id == OptionSet[i].ID)
                {
                    OptionSet.RemoveAt(i);
                    return this;
                }
            }

            return this;
        }
    }
}
