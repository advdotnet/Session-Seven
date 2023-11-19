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
		private static void Main(string[] args)
		{
			try
			{
				AppDomain.CurrentDomain.UnhandledException += (s, e)
					=> FatalExceptionObject(e.ExceptionObject);

				STACK.Logging.Log.AddLogger(new STACK.Logging.SystemConsoleLogHandler());

				if (null != args && 0 < args.Count() && "click" == args[0])
				{
					ExecuteClicks();
				}
				else if (null != args && 0 < args.Count() && "interact" == args[0])
				{
					ExecuteInteractions();
				}
				else
				{
					var playthroughLogic = new Functional.Test.Playthrough();

					playthroughLogic.SolveGameWithSaveGames(GetScoreType());
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
				var playerScripts = Game.Ego.Get<Scripts>();

				while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
				{
					while (playerScripts.ScriptCollection.Count > 0 || !runner.Game.World.Interactive)
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
				var playerScripts = Game.Ego.Get<Scripts>();
				var randomizer = runner.Game.World.Get<Randomizer>();

				while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
				{
					while (playerScripts.ScriptCollection.Count > 0 || !runner.Game.World.Interactive)
					{
						runner.Tick();
						if (runner.Game.World.Interactive)
						{
							runner.MouseClick(randomizer.CreateInt(1110), randomizer.CreateInt(400));
						}
					}

					try
					{
						if (randomizer.CreateInt(2) == 1)
						{
							var randomEntity = ChooseRandomEntity(randomizer);
							var randomVerb = ChooseRandomVerb(randomizer);

							runner.Interact(randomEntity, randomVerb, false);
						}
						else
						{
							runner.Interact(ChooseRandomEntity(randomizer), ChooseRandomEntity(randomizer), ChooseRandomDitransitiveVerb(randomizer), false);
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
			IEnumerable<Entity> objectsToChooseFrom;

			if (randomizer.CreateInt(2) == 1)
			{
				objectsToChooseFrom = Tree.Basement.Scene.GameObjectCache.Entities.Where(e => e.Enabled && e.Visible && null != e.Get<Interaction>());
			}
			else
			{
				objectsToChooseFrom = Game.Ego.Inventory.Scene.GameObjectCache.Entities.Where(e => e.Enabled && e.Visible && null != e.Get<Interaction>());
			}

			var count = objectsToChooseFrom.Count();
			if (0 == count)
			{
				return ChooseRandomEntity(randomizer);
			}
			var index = randomizer.CreateInt(0, count);

			return objectsToChooseFrom.ElementAt(index);
		}

		private static LockedVerb ChooseRandomVerb(Randomizer randomizer)
		{
			var allVerbs = Verbs.All;
			var index = randomizer.CreateInt(0, allVerbs.Count());

			return allVerbs.ElementAt(index);
		}

		private static LockedVerb ChooseRandomDitransitiveVerb(Randomizer randomizer)
		{
			if (randomizer.CreateInt(2) == 1)
			{
				return Verbs.Use;
			}

			return Verbs.Give;
		}

		private static ScoreType GetScoreType()
		{
			var validChars = new char[] { '1', '2', '3' };
			ConsoleKeyInfo key;

			do
			{
				Console.Clear();
				Console.WriteLine("Choose a target ending ");
				Console.WriteLine(" (1) Freedom");
				Console.WriteLine(" (2) Insanity");
				Console.WriteLine(" (3) Jail");
				Console.Write(" > ");
				key = Console.ReadKey();
			} while (!validChars.Contains(key.KeyChar));

			Console.Clear();

			switch (key.KeyChar)
			{
				case '1': return ScoreType.Freedom;
				case '2': return ScoreType.Insanity;
				default: return ScoreType.Jail;
			}
		}

		private static void FatalExceptionObject(object exceptionObject)
		{
			var exception = exceptionObject as Exception ?? new NotSupportedException(
				  $"Unhandled exception doesn't derive from System.Exception: {exceptionObject}");
			HandleException(exception);
		}

		private static void HandleException(Exception e)
		{
			Console.WriteLine(e.ToString());
			AppendToFile("error.log", e.ToString());
		}

		private static void AppendToFile(string filename, string text)
		{
			SaveGame.EnsureStorageFolderExists(Game.SAVEGAMEFOLDER);
			var directory = SaveGame.UserStorageFolder(Game.SAVEGAMEFOLDER);
			var path = System.IO.Path.Combine(directory, filename);
			using (var w = File.AppendText(path))
			{
				w.WriteLine(text);
			}
		}
	}
}
