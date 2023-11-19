using Microsoft.Xna.Framework;
using STACK;
using System;
using System.Diagnostics;
using System.IO;

namespace SessionSeven
{
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		[DebuggerStepThrough]
		private static void Main()
		{
			try
			{
				AppDomain.CurrentDomain.UnhandledException += (s, e)
					=> FatalExceptionObject(e.ExceptionObject);

				using (var game = new Window(new Game()))
				{
					game.IsMouseVisible = false;
					game.Run();
				}
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

		private static void FatalExceptionObject(object exceptionObject)
		{
			var exception = exceptionObject as Exception ?? new NotSupportedException(
				$"Unhandled exception doesn't derive from System.Exception: {exceptionObject}");
			HandleException(exception);
		}

		private static void HandleException(Exception e)
		{
			FNALoggerEXT.LogInfo("Exception: " + e.ToString());
			AppendToFile("error.log", e.ToString());

			if (null != e.InnerException)
			{
				HandleException(e.InnerException);
			}
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
