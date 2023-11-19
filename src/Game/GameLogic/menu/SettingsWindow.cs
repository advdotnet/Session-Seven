using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using STACK;
using System;
using System.Collections.Generic;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;
using Window = TomShane.Neoforce.Controls.Window;

namespace SessionSeven
{
	public partial class Menu
	{
		private readonly GameSettings _gameSettings;
		private Window _settingsWindow;

		private void AddSettingsWindow(Manager gui)
		{
			_settingsWindow = new Window(gui)
			{
				Width = 300,
				Height = 200,
				CloseButtonVisible = false,
				Text = GlblRes.Settings,
				Parent = _mainMenuBackground
			};
			_settingsWindow.Init();
			_settingsWindow.Center(new Point(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT));
			_settingsWindow.Visible = false;
			_settingsWindow.Resizable = false;
			_settingsWindow.IconVisible = false;
			_settingsWindow.DragAlpha = 255;
			_settingsWindow.Movable = false;

			var musicVolumeLabel = new Label(gui);
			musicVolumeLabel.Init();
			musicVolumeLabel.Width = 120;
			musicVolumeLabel.Parent = _settingsWindow;
			musicVolumeLabel.Text = GlblRes.Music_Volume;
			musicVolumeLabel.Top = 5;
			musicVolumeLabel.Left = 5;

			var musicVolumeBar = new TrackBar(gui);
			musicVolumeBar.Init();
			musicVolumeBar.Width = 160;
			musicVolumeBar.Value = (int)(_gameSettings.MusicVolume * 100);
			musicVolumeBar.ValueChanged += (o, e) =>
			{
				_gameSettings.MusicVolume = musicVolumeBar.Value / 100f;
				_engine.ApplyGameSettingsVolume();
				MediaPlayer.Volume = MathHelper.Clamp(_gameSettings.MusicVolume, 0.0f, 1.0f);
				_mainMenuLabel.Text = string.Empty;
			};
			musicVolumeBar.Top = 5;
			musicVolumeBar.Left = 120;
			musicVolumeBar.Parent = _settingsWindow;

			var soundVolumeLabel = new Label(gui);
			soundVolumeLabel.Init();
			soundVolumeLabel.Width = 120;
			soundVolumeLabel.Parent = _settingsWindow;
			soundVolumeLabel.Text = GlblRes.Sound_Volume;
			soundVolumeLabel.Top = 30;
			soundVolumeLabel.Left = 5;

			var soundVolumeBar = new TrackBar(gui);
			soundVolumeBar.Init();
			soundVolumeBar.Width = 160;
			soundVolumeBar.Value = (int)(_gameSettings.SoundEffectVolume * 100);
			soundVolumeBar.ValueChanged += (o, e) =>
			{
				_gameSettings.SoundEffectVolume = soundVolumeBar.Value / 100f;
				_engine.ApplyGameSettingsVolume();
				_mainMenuLabel.Text = string.Empty;
			};
			soundVolumeBar.Top = 30;
			soundVolumeBar.Left = 120;
			soundVolumeBar.Parent = _settingsWindow;

			var bloomLabel = new Label(gui);
			bloomLabel.Init();
			bloomLabel.Width = 120;
			bloomLabel.Parent = _settingsWindow;
			bloomLabel.Text = GlblRes.Bloom;
			bloomLabel.Top = 55;
			bloomLabel.Left = 5;

			var bloomCheckBox = new CheckBox(gui);
			bloomCheckBox.Init();
			bloomCheckBox.Parent = _settingsWindow;
			bloomCheckBox.Text = string.Empty;
			bloomCheckBox.Checked = _gameSettings.Bloom;
			bloomCheckBox.CheckedChanged += (o, e) =>
			{
				_gameSettings.Bloom = bloomCheckBox.Checked;
				_mainMenuLabel.Text = GlblRes.Restart_the_game_for_this_setting_to_take_effect;
			};
			bloomCheckBox.Top = 55;
			bloomCheckBox.Left = 120;

			var vSyncLabel = new Label(gui);
			vSyncLabel.Init();
			vSyncLabel.Width = 120;
			vSyncLabel.Parent = _settingsWindow;
			vSyncLabel.Text = GlblRes.VSync;
			vSyncLabel.Top = 80;
			vSyncLabel.Left = 5;

			var vSyncCheckBox = new CheckBox(gui);
			vSyncCheckBox.Init();
			vSyncCheckBox.Parent = _settingsWindow;
			vSyncCheckBox.Text = string.Empty;
			vSyncCheckBox.Checked = _gameSettings.VSync;
			vSyncCheckBox.CheckedChanged += (o, e) =>
			{
				_gameSettings.VSync = vSyncCheckBox.Checked;
				_mainMenuLabel.Text = GlblRes.Restart_the_game_for_this_setting_to_take_effect;
			};
			vSyncCheckBox.Top = 80;
			vSyncCheckBox.Left = 120;

			var displayModeLabel = new Label(gui);
			displayModeLabel.Init();
			displayModeLabel.Width = 120;
			displayModeLabel.Parent = _settingsWindow;
			displayModeLabel.Text = GlblRes.Display_Mode;
			displayModeLabel.Top = 105;
			displayModeLabel.Left = 5;

			var displayModeCombo = new ComboBox(gui);
			displayModeCombo.Init();

			displayModeCombo.Items = GetDisplayModes();
			displayModeCombo.Width = 120;
			displayModeCombo.Parent = _settingsWindow;
			displayModeCombo.Width = 160;
			displayModeCombo.Text = DisplayModeToString(_gameSettings.DisplayMode);
			displayModeCombo.ItemIndexChanged += (o, e) =>
			{
				_gameSettings.DisplayMode = StringToDisplayMode(displayModeCombo.Text);
				_mainMenuLabel.Text = GlblRes.Restart_the_game_for_this_setting_to_take_effect;
			};
			displayModeCombo.Top = 105;
			displayModeCombo.Left = 120;

			var bevel = new Bevel(gui)
			{
				Parent = _settingsWindow,
				Anchor = Anchors.Bottom | Anchors.Left | Anchors.Right,
				Height = 35,
				Style = BevelStyle.Raised
			};
			bevel.Top = _settingsWindow.ClientHeight - bevel.Height;
			bevel.Width = _settingsWindow.ClientWidth;

			var okButton = new MenuButton(gui, _clickSound, _focusSound, _gameSettings);
			okButton.Init();
			okButton.Parent = bevel;
			okButton.Text = GlblRes.OK;
			okButton.Click += (s, e) =>
			{
				_settingsWindow.Close();
				ShowLogo(true);
				_gameSettings.Save(_engine.Game.SaveGameFolder);
			};
			okButton.Width = 130;
			okButton.Left = 150;
			okButton.Top = 5;

			gui.Add(_settingsWindow);
		}

		private List<object> GetDisplayModes()
		{
			var result = new List<object>();

			foreach (STACK.DisplayMode displayMode in Enum.GetValues(typeof(STACK.DisplayMode)))
			{
				result.Add(DisplayModeToString(displayMode));
			}

			return result;
		}

		private string DisplayModeToString(STACK.DisplayMode displayMode)
		{
			switch (displayMode)
			{
				case DisplayMode.Borderless:
					return GlblRes.Borderless;
				case DisplayMode.BorderlessMaxInteger:
					return GlblRes.Borderless_Fit;
				case DisplayMode.BorderlessScale:
					return GlblRes.Borderless_Scale;
				case DisplayMode.Fullscreen:
					return GlblRes.Fullscreen;
				case DisplayMode.Window:
					return GlblRes.Window;
				case DisplayMode.WindowMaxInteger:
					return GlblRes.Window_Fit;
			}

			return null;
		}

		private STACK.DisplayMode StringToDisplayMode(string val)
		{
			if (val == GlblRes.Borderless)
			{
				return DisplayMode.Borderless;
			}

			if (val == GlblRes.Borderless_Fit)
			{
				return DisplayMode.BorderlessMaxInteger;
			}

			if (val == GlblRes.Borderless_Scale)
			{
				return DisplayMode.BorderlessScale;
			}

			if (val == GlblRes.Fullscreen)
			{
				return DisplayMode.Fullscreen;
			}

			if (val == GlblRes.Window)
			{
				return DisplayMode.Window;
			}

			if (val == GlblRes.Window_Fit)
			{
				return DisplayMode.WindowMaxInteger;
			}

			return DisplayMode.WindowMaxInteger;
		}
	}
}
