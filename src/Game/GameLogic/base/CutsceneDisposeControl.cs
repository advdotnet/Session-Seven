using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven
{
    /// <summary>
    /// The liftetime of this class represents an ingame cutscene: the world's interactive 
    /// state is disabled when constructing this class and enabled when disposing.
    /// </summary>
    [Serializable]
    public class CutsceneDisposeControl : IDisposable
    {
        [NonSerialized]
        World World;
        Action ResetGUIFn;
        bool ResetInteractive, ResetGUI, WasSkippingEnabled;

        public CutsceneDisposeControl(World world, Action resetGUIFn, bool resetInteractive = true, bool resetGUI = true, bool updateLabel = true)
        {
            World = world;
            ResetGUI = resetGUI;
            ResetGUIFn = resetGUIFn;
            ResetInteractive = resetInteractive;
            WasSkippingEnabled = IsSkippingEnabled();
            if (updateLabel)
            {
                ((GUI.Interaction.Scene)World.GetScene(Tree.GUI.Interaction.SceneID))?.UpdateLabel();
            }
            World.Interactive = false;

            if (!WasSkippingEnabled)
            {
                Game.EnableSkipping();
            }
        }

        public void Dispose()
        {
            if (null == World)
            {
                World = Tree.World;
            }

            if (ResetInteractive)
            {
                World.Interactive = true;
            }

            if (ResetGUI && null != ResetGUIFn)
            {
                ResetGUIFn();
            }

            if (!WasSkippingEnabled)
            {
                Game.StopSkipping();
            }
        }

        private bool IsSkippingEnabled()
        {
            var SkipContent = World.Get<SkipContent>();

            if (null != SkipContent && null != SkipContent.SkipCutscene)
            {
                return SkipContent.SkipCutscene.Possible;
            }

            return false;
        }
    }
}
