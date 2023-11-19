using Microsoft.Xna.Framework;
using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Dialog;
using STACK;
using STACK.Components;
using STACK.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SessionSeven.Cutscenes
{
	[Serializable]
	public class Scene : STACK.Scene
	{
		public Scene()
		{
			Enabled = true;
			Visible = true;
			DrawOrder = 10;

			Push(new Director());
		}
	}

	[Serializable]
	public enum Sessions
	{
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven
	}

	[Serializable]
	public partial class Director : Entity
	{
		private readonly Dictionary<Sessions, bool> _finishedSessions = new Dictionary<Sessions, bool>();

		public Director()
		{
			Scripts
				.Create(this);
		}

		private Office.Therapist Psychiatrist => Tree.Office.Therapist;

		private Office.Ryan RyanVoice => Tree.Office.Ryan;

		private GUI.Fader Fader => Tree.GUI.Fader;

		private Office.RyanEyesClosed RyanEyesClosed => Tree.Office.RyanEyesClosed;

		private IEnumerator FadeInScript(bool hideFader = false, bool fadeMusic = false)
		{
			yield return FadeScript(true, hideFader, fadeMusic);
		}

		private IEnumerator FadeOutScript(bool hideFader = false, bool fadeMusic = false)
		{
			yield return FadeScript(false, hideFader, fadeMusic);
		}

		private IEnumerator FadeScript(bool fadein, bool hideFader = false, bool fadeMusic = false, int loops = 255)
		{
			Fader.Visible = true;

			for (var j = 0; j < loops; j++)
			{
				Fader.Color = new Color(Color.Black, fadein ? 255 - j : j);
				if (fadeMusic)
				{
					World.Get<AudioManager>().MusicVolume = fadein ? (float)j / loops : 1 - ((float)j / loops);
				}

				yield return 1;
			}

			if (hideFader)
			{
				Fader.Visible = false;
			}

			if (fadeMusic)
			{
				World.Get<AudioManager>().MusicVolume = fadein ? 1 : 0;
			}
		}

		private void ProcessScore(BaseOption option)
		{
			if (!(option is ScoreOption))
			{
				throw new ArgumentException("No ScoreOption");
			}

			foreach (var score in ((ScoreOption)option).ScoreSet)
			{
				Game.Ego.Get<Score>().Add(score.Key, score.Value);

				var identifier = score.Key.ToString();
				var newValue = Game.Ego.Get<Score>().GetScore(score.Key);

				Log.WriteLine(identifier + "Score: " + newValue, LogLevel.Notice);
			}
		}

		public Script StartSession(Sessions session)
		{
			if (FinishedSession(session))
			{
				return Script.None;
			}

			Tree.World.Interactive = false;
			Tree.Cutscenes.Scene.Visible = true;
			Tree.Cutscenes.Scene.Enabled = true;

			Log.WriteLine("Starting Session " + session.ToString());

			return Get<Scripts>().Start(GetSessionScript(session));
		}

		private IEnumerator GetSessionScript(Sessions session)
		{
			IEnumerator sessionScript;
			switch (session)
			{
				case Sessions.One: sessionScript = SessionOneScript(); break;
				case Sessions.Two: sessionScript = SessionTwoScript(); break;
				case Sessions.Three: sessionScript = SessionThreeScript(); break;
				case Sessions.Four: sessionScript = SessionFourScript(); break;
				case Sessions.Five: sessionScript = SessionFiveScript(); break;
				case Sessions.Six: sessionScript = SessionSixScript(); break;
				default: sessionScript = SessionSevenScript(); break;
			}

			yield return Get<Scripts>().Start(sessionScript);

			FinishSession(session);

			if (session == Sessions.Seven)
			{
				yield break;
			}

			var alreadyFinishedSessions = FinishedSessionsCount;

			if (alreadyFinishedSessions % 2 == 1)
			{
				Game.EnqueueSong(content.audio.basement);
			}

			if (session != Sessions.One)
			{
				World.Get<AudioManager>().RepeatSong = false;
			}
		}

		public bool FinishedSession(Sessions session)
		{
			if (_finishedSessions.TryGetValue(session, out var result))
			{
				return result;
			}

			return false;
		}

		private int FinishedSessionsCount => _finishedSessions.Where(fs => fs.Value).Count();

		private void FinishSession(Sessions session)
		{
			_finishedSessions[session] = true;
		}
	}
}
