using SessionSeven.Actors;
using STACK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SessionSeven.Components
{
    [Serializable]
    public enum ScoreType
    {
        Jail,
        Freedom,
        Insanity
    }

    /// <summary>
    /// Component that keeps track of the jail / freedom / mental hospital score.
    /// </summary>
    [Serializable]
    public class Score : Component
    {
        Dictionary<ScoreType, int> Results;

        public Score()
        {
            Reset();
        }

        public void Reset()
        {
            Results = new Dictionary<ScoreType, int>(Enum.GetNames(typeof(ScoreType)).Length);
            var EnumValues = Enum.GetValues(typeof(ScoreType)).Cast<ScoreType>();
            foreach (var EnumValue in EnumValues)
            {
                Results.Add(EnumValue, 0);
            }
        }

        public int GetScore(ScoreType type)
        {
            return Results[type];
        }

        public bool HasScore()
        {
            return Results.Any(r => r.Value > 0);
        }

        public void Add(ScoreType type, int points)
        {
            Results[type] = Results[type] + points;
        }

        public ScoreType GetScoreTypeResult()
        {
            var MaxScoreKVP = Results.Aggregate((agg, next) => next.Value > agg.Value ? next : agg);

            return MaxScoreKVP.Key;
        }

        public static Score Create(Ryan entity)
        {
            return entity.Add<Score>();
        }
    }
}
