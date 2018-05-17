using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;
using System.Collections;

namespace SessionSeven.GUI.PositionSelection
{
    [Serializable]
    public class Scene : STACK.Scene
    {
        public bool Aborted { get; private set; }
        public Vector2 Result { get; private set; }
        private bool SelectionMade = false;

        public Scene()
        {
            this.AutoAddEntities();

            InputDispatcher
                .Create(this)
                .SetOnMouseUpFn(OnMouseUp);

            ScenePath
                .Create(this);

            Enabled = false;

            DrawOrder = 125;
        }

        void OnMouseUp(Vector2 position, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                Result = position;
                SelectionMade = true;
            }
            else
            {
                Aborted = true;
            }
        }

        public const string SCRIPTID = "WAITFORDIALOGSELECTIONSCRIPTID";

        public Script StartSelectionScript(Scripts scripts, IPositionable entityToPosition, bool setInteractive = true, bool abortable = true, int mode = 0)
        {
            return scripts.Start(WaitForSelection(entityToPosition, setInteractive, abortable, mode), SCRIPTID);
        }

        /// <summary>
        /// IEnumerator that yields until a selection was made.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="entityToPosition"></param>
        /// <param name="setInteractive"></param>
        /// <param name="abortable"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private IEnumerator WaitForSelection(IPositionable entityToPosition, bool setInteractive = true, bool abortable = true, int mode = 0)
        {
            Aborted = false;
            Enabled = true;
            Visible = true;
            SelectionMade = false;

            bool previousWorldInteractive = World.Interactive;

            if (setInteractive)
            {
                World.Interactive = true;
            }

            entityToPosition.BeginPosition(mode);

            while (!SelectionMade && !Aborted)
            {
                SetPosition(entityToPosition, mode);
                yield return 0;
            }

            SetPosition(entityToPosition, mode);
            entityToPosition.EndPosition(mode);

            if (setInteractive)
            {
                World.Interactive = previousWorldInteractive;
            }

            Enabled = false;
            Visible = false;
        }

        void SetPosition(IPositionable entityToPosition, int mode)
        {
            var MousePosition = World.Get<STACK.Components.Mouse>().Position;
            entityToPosition.SetPosition(MousePosition, mode);
        }
    }
}
