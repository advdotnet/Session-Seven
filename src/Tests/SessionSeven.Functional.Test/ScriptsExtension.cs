﻿using STACK;

namespace SessionSeven.Functional.Test
{
	public static class ScriptsExtension
	{
		/// <summary>
		/// Returns whether this script waits for a dialog / position selection.
		/// </summary>
		/// <param name="script"></param>
		/// <returns></returns>
		public static bool WaitsforSelection(this Script script)
		{
			return InnerMostScriptHasID(script, GUI.Dialog.Menu.SCRIPTID) ||
				InnerMostScriptHasID(script, GUI.PositionSelection.Scene.SCRIPTID);
		}

		private static bool InnerMostScriptHasID(Script script, string id)
		{
			var innerScript = script.GetNestedScript();

			while (innerScript != null && innerScript.GetNestedScript() != null)
			{
				innerScript = innerScript.GetNestedScript();
			}

			return (null != innerScript) && (id == innerScript.ID);
		}
	}
}
