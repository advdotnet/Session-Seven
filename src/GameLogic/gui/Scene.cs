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
                if (!Game.Engine.Paused)
                {
                    Game.Engine.Pause();
                    Game.MainMenu.Show();
                    Game.Engine.Renderer.GUIManager.ShowSoftwareCursor = true;
                }
            }

            if (Keys.F5 == key)
            {
                Game.Engine.SaveState("Quicksave");
            }

            if (Keys.F9 == key)
            {
                var FileName = Game.Engine.ExistsStateByName("Quicksave");
                if (null != FileName)
                {
                    Game.Engine.LoadState(FileName);
                }
            }
        }
    }
}
