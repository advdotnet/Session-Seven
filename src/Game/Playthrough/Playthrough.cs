using SessionSeven.Components;
using STACK;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SessionSeven
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Playthrough
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (s, e)
                    => FatalExceptionObject(e.ExceptionObject);

                STACK.Logging.Log.AddLogger(new STACK.Logging.SystemConsoleLogHandler());

                var PlaythroughLogic = new SessionSeven.Functional.Test.Playthrough();

                PlaythroughLogic.SolveGameWithSaveGames(GetScoreType());

                Console.WriteLine("Done.");
            }
            catch (Exception e)
            {
                HandleException(e);

                if (Debugger.IsAttached)
                {
                    throw;
                }
            }
        }

        static ScoreType GetScoreType()
        {
            var ValidChars = new char[] { '1', '2', '3' };
            ConsoleKeyInfo Key;

            do
            {
                Console.Clear();
                Console.WriteLine("Choose a target ending ");
                Console.WriteLine(" (1) Freedom");
                Console.WriteLine(" (2) Insanity");
                Console.WriteLine(" (3) Jail");
                Console.Write(" > ");
                Key = Console.ReadKey();
            } while (!ValidChars.Contains(Key.KeyChar));

            Console.Clear();

            switch (Key.KeyChar)
            {
                case '1': return ScoreType.Freedom;
                case '2': return ScoreType.Insanity;
                default: return ScoreType.Jail;
            }
        }

        static void FatalExceptionObject(object exceptionObject)
        {
            var Exception = exceptionObject as Exception;
            if (Exception == null)
            {
                Exception = new NotSupportedException(
                  "Unhandled exception doesn't derive from System.Exception: "
                   + exceptionObject.ToString()
                );
            }
            HandleException(Exception);
        }

        static void HandleException(Exception e)
        {
            AppendToFile("error.log", e.ToString());
        }

        static void AppendToFile(string filename, string text)
        {
            SaveGame.EnsureStorageFolderExists(Game.SAVEGAMEFOLDER);
            var Directory = SaveGame.UserStorageFolder(Game.SAVEGAMEFOLDER);
            var Path = System.IO.Path.Combine(Directory, filename);
            using (var w = File.AppendText(Path))
            {
                w.WriteLine(text);
            }
        }
    }
}
