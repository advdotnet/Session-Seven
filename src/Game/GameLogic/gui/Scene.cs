using Microsoft.Xna.Framework.Input;
using STACK.Components;
using System;

namespace SessionSeven.GUI
{
	[Serializable]
	public class Scene : STACK.Scene
	{
		public Scene()
		{
			this.AutoAddEntities();

			InputDispatcher
				.Create(this)
				.SetOnKeyDownFn(OnKeyDown);

			Enabled = true;
			Visible = true;
			DrawOrder = 1019;
		}

		public void OnKeyDown(Keys key)
		{
			if (Keys.Escape == key)
			{
				if (!STACK.StackGame.Engine.Paused)
				{
					STACK.StackGame.Engine.Pause();
					Game.MainMenu.Show();
					STACK.StackGame.Engine.Renderer.GUIManager.ShowSoftwareCursor = true;
				}
			}

			if (Keys.F5 == key)
			{
				STACK.StackGame.Engine.SaveState("Quicksave");
			}

			if (Keys.F9 == key)
			{
				var fileName = STACK.StackGame.Engine.ExistsStateByName("Quicksave");
				if (null != fileName)
				{
					STACK.StackGame.Engine.LoadState(fileName);
				}
			}
		}
	}
}
