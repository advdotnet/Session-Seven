using SessionSeven.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SessionSeven.GUI.Dialog
{
    public interface IDialogOptions
    {
        BaseOption this[int index]
        {
            get;
        }

        int Count { get; }

        IEnumerable<string> SelectTexts();
    }

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

    [Serializable]
    public abstract class BaseOptions<T> : IDialogOptions where T : BaseOption
    {
        protected List<T> OptionSet = new List<T>(3);

        public int Count
        {
            get
            {
                return OptionSet.Count;
            }
        }

        public BaseOption this[int index]
        {
            get
            {
                return OptionSet[index];
            }
        }

        public IEnumerable<string> SelectTexts()
        {
            return OptionSet.Select(o => o.Text);
        }
    }

    [Serializable]
    public class Options : BaseOptions<BaseOption>
    {
        private Options() { }

        public static Options Create()
        {
            return new Options();
        }

        public Options Add(int id, string text)
        {
            if (id == -1)
            {
                throw new ArgumentException("Option.ID must not equal -1.");
            }

            OptionSet.Add(new BaseOption()
            {
                ID = id,
                Text = text
            });

            return this;
        }

        public Options Add(int id, string text, Func<bool> predicate)
        {
            if (predicate())
            {
                Add(id, text);
            }

            return this;
        }

        public Options RemoveByID(int id)
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

            OptionSet.Add(new ScoreOption()
            {
                ID = id,
                Text = text,
                ScoreSet = scores
            });

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
