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
        Dictionary<Sessions, bool> FinishedSessions = new Dictionary<Sessions, bool>();

        public Director()
        {
            Scripts
                .Create(this);
        }

        Office.Therapist Psychiatrist { get { return Tree.Office.Therapist; } }
        Office.Ryan RyanVoice { get { return Tree.Office.Ryan; } }
        GUI.Fader Fader { get { return Tree.GUI.Fader; } }
        Office.RyanEyesClosed RyanEyesClosed { get { return Tree.Office.RyanEyesClosed; } }

        IEnumerator FadeInScript(bool hideFader = false)
        {
            yield return FadeScript(true, hideFader);
        }

        IEnumerator FadeOutScript(bool hideFader = false)
        {
            yield return FadeScript(false, hideFader);
        }

        IEnumerator FadeScript(bool fadein, bool hideFader = false, int loops = 255)
        {
            Fader.Visible = true;

            for (int j = 0; j < loops; j++)
            {
                Fader.Color = new Color(Color.Black, fadein ? 255 - j : j);

                yield return 1;
            }

            if (hideFader)
            {
                Fader.Visible = false;
            }
        }

        void ProcessScore(BaseOption option)
        {
            if (!(option is ScoreOption))
            {
                throw new ArgumentException("No ScoreOption");
            }

            foreach (var Score in ((ScoreOption)option).ScoreSet)
            {
                Game.Ego.Get<Score>().Add(Score.Key, Score.Value);

                var Identifier = Score.Key.ToString();
                var NewValue = Game.Ego.Get<Score>().GetScore(Score.Key);

                Log.WriteLine(Identifier + "Score: " + NewValue, LogLevel.Notice);
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
            IEnumerator SessionScript = null;

            switch (session)
            {
                case Sessions.One: SessionScript = SessionOneScript(); break;
                case Sessions.Two: SessionScript = SessionTwoScript(); break;
                case Sessions.Three: SessionScript = SessionThreeScript(); break;
                case Sessions.Four: SessionScript = SessionFourScript(); break;
                case Sessions.Five: SessionScript = SessionFiveScript(); break;
                case Sessions.Six: SessionScript = SessionSixScript(); break;
                default: SessionScript = SessionSevenScript(); break;
            }

            yield return Get<Scripts>().Start(SessionScript);

            FinishSession(session);

            if (session == Sessions.Seven)
            {
                yield break; ;
            }

            var AlreadyFinishedSessions = FinishedSessionsCount;

            if (AlreadyFinishedSessions % 2 == 1)
            {
                if (session == Sessions.One)
                {
                    Game.PlaySong(content.audio.basement);
                }
                else
                {
                    World.Get<AudioManager>().RepeatSong = false;
                    Game.EnqueueSong(content.audio.basement);
                }
            }
        }

        public bool FinishedSession(Sessions session)
        {
            bool Result = false;

            if (FinishedSessions.TryGetValue(session, out Result))
            {
                return Result;
            }

            return false;
        }

        private int FinishedSessionsCount
        {
            get
            {
                return FinishedSessions.Where(fs => fs.Value).Count();
            }
        }

        private void FinishSession(Sessions session)
        {
            FinishedSessions[session] = true;
        }
    }
}
