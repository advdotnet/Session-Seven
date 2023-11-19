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
		private World _world;
		private readonly Action _resetGUIFn;
		private readonly bool _resetInteractive, _resetGUI, _wasSkippingEnabled;

		public CutsceneDisposeControl(World world, Action resetGUIFn, bool resetInteractive = true, bool resetGUI = true, bool updateLabel = true)
		{
			_world = world;
			_resetGUI = resetGUI;
			_resetGUIFn = resetGUIFn;
			_resetInteractive = resetInteractive;
			_wasSkippingEnabled = IsSkippingEnabled();
			if (updateLabel)
			{
				((GUI.Interaction.Scene)_world.GetScene(Tree.GUI.Interaction.SceneID))?.UpdateLabel();
			}
			_world.Interactive = false;

			if (!_wasSkippingEnabled)
			{
				Game.EnableSkipping();
			}
		}

		public void Dispose()
		{
			if (null == _world)
			{
				_world = Tree.World;
			}

			if (_resetInteractive)
			{
				_world.Interactive = true;
			}

			if (_resetGUI && null != _resetGUIFn)
			{
				_resetGUIFn();
			}

			if (!_wasSkippingEnabled)
			{
				Game.StopSkipping();
			}
		}

		private bool IsSkippingEnabled()
		{
			var skipContent = _world.Get<SkipContent>();

			if (null != skipContent && null != skipContent.SkipCutscene)
			{
				return skipContent.SkipCutscene.Possible;
			}

			return false;
		}
	}
}
