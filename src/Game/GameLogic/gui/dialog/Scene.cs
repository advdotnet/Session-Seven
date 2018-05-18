using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;


namespace SessionSeven.GUI.Dialog
{
    [Serializable]
    public class Scene : STACK.Scene
    {
        public Scene()
        {
            InputDispatcher
                .Create(this)
                .SetOnMouseUpFn(OnMouseUp);

            Enabled = true;
            Visible = true;
            Push(new Menu());
            DrawOrder = 124;
        }

        public void OnMouseUp(Vector2 position, MouseButton button)
        {
            var OUM = World.Get<STACK.Components.Mouse>().ObjectUnderMouse;

            if (OUM == Tree.GUI.Dialog.Menu)
            {
                Tree.GUI.Dialog.Menu.Click();
            }
        }
    }
}
