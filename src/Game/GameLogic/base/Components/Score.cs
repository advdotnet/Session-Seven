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
		private Dictionary<ScoreType, int> _results;

		public Score()
		{
			Reset();
		}

		public void Reset()
		{
			_results = new Dictionary<ScoreType, int>(Enum.GetNames(typeof(ScoreType)).Length);
			var enumValues = Enum.GetValues(typeof(ScoreType)).Cast<ScoreType>();
			foreach (var enumValue in enumValues)
			{
				_results.Add(enumValue, 0);
			}
		}

		public int GetScore(ScoreType type) => _results[type];

		public bool HasScore() => _results.Any(r => r.Value > 0);

		public void Add(ScoreType type, int points)
		{
			_results[type] = _results[type] + points;
		}

		public ScoreType GetScoreTypeResult()
		{
			var maxScoreKVP = _results.Aggregate((agg, next) => next.Value > agg.Value ? next : agg);

			return maxScoreKVP.Key;
		}

		public static Score Create(Ryan entity)
		{
			return entity.Add<Score>();
		}
	}
}
