using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.Functional.Test;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (s, e)
                    => FatalExceptionObject(e.ExceptionObject);

                STACK.Logging.Log.AddLogger(new STACK.Logging.SystemConsoleLogHandler());

                if (null != args && "click" == args[0])
                {
                    ExecuteClicks();
                }
                else if (null != args && "interact" == args[0])
                {
                    ExecuteInteractions();
                }
                else
                {
                    var PlaythroughLogic = new Functional.Test.Playthrough();

                    PlaythroughLogic.SolveGameWithSaveGames(GetScoreType());
                }

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

        private static void ExecuteClicks()
        {
            SessionSevenTestEngine.Execute((runner) =>
            {
                Console.WriteLine("Press ESC to quit.");
                var PlayerScripts = Game.Ego.Get<Scripts>();

                while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
                {
                    while (PlayerScripts.ScriptCollection.Count > 0 || !runner.Game.World.Interactive)
                    {
                        runner.Tick();
                        runner.MouseClick(runner.Game.World.Get<Randomizer>().CreateInt(1110), runner.Game.World.Get<Randomizer>().CreateInt(400));
                    }

                    runner.MouseClick(runner.Game.World.Get<Randomizer>().CreateInt(1110), runner.Game.World.Get<Randomizer>().CreateInt(400));
                }
                runner.SaveState("click state");
            });
        }

        private static void ExecuteInteractions()
        {
            SessionSevenTestEngine.Execute((runner) =>
            {
                Console.WriteLine("Press ESC to quit.");
                var PlayerScripts = Game.Ego.Get<Scripts>();
                var Randomizer = runner.Game.World.Get<Randomizer>();

                while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
                {
                    while (PlayerScripts.ScriptCollection.Count > 0 || !runner.Game.World.Interactive)
                    {
                        runner.Tick();
                        if (runner.Game.World.Interactive)
                        {
                            runner.MouseClick(Randomizer.CreateInt(1110), Randomizer.CreateInt(400));
                        }
                    }

                    try
                    {
                        if (Randomizer.CreateInt(2) == 1)
                        {
                            var RandomEntity = ChooseRandomEntity(Randomizer);
                            var RandomVerb = ChooseRandomVerb(Randomizer);

                            runner.Interact(RandomEntity, RandomVerb, false);
                        }
                        else
                        {
                            runner.Interact(ChooseRandomEntity(Randomizer), ChooseRandomEntity(Randomizer), ChooseRandomDitransitiveVerb(Randomizer), false);
                        }
                    }
                    catch (KeyNotFoundException)
                    {

                    }
                }
                runner.SaveState("interaction state");
            });
        }

        private static Entity ChooseRandomEntity(Randomizer randomizer)
        {
            IEnumerable<Entity> ObjectsToChooseFrom;

            if (randomizer.CreateInt(2) == 1)
            {
                ObjectsToChooseFrom = Tree.Basement.Scene.GameObjectCache.Entities.Where(e => e.Enabled && e.Visible && null != e.Get<Interaction>());
            }
            else
            {
                ObjectsToChooseFrom = Game.Ego.Inventory.Scene.GameObjectCache.Entities.Where(e => e.Enabled && e.Visible && null != e.Get<Interaction>());
            }

            var count = ObjectsToChooseFrom.Count();
            if (0 == count)
            {
                return ChooseRandomEntity(randomizer);
            }
            var index = randomizer.CreateInt(0, count);

            return ObjectsToChooseFrom.ElementAt(index);
        }

        private static LockedVerb ChooseRandomVerb(Randomizer randomizer)
        {
            var AllVerbs = Verbs.All;
            var index = randomizer.CreateInt(0, AllVerbs.Count());

            return AllVerbs.ElementAt(index);
        }

        private static LockedVerb ChooseRandomDitransitiveVerb(Randomizer randomizer)
        {
            if (randomizer.CreateInt(2) == 1)
            {
                return Verbs.Use;
            }

            return Verbs.Give;
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
            Console.WriteLine(e.ToString());
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
