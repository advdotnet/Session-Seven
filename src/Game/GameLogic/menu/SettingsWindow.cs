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
        private GameSettings GameSettings;
        private Window SettingsWindow;

        void AddSettingsWindow(Manager gui)
        {
            SettingsWindow = new Window(gui);
            SettingsWindow.Width = 300;
            SettingsWindow.Height = 200;
            SettingsWindow.CloseButtonVisible = false;
            SettingsWindow.Text = GlblRes.Settings;
            SettingsWindow.Parent = MainMenuBackground;
            SettingsWindow.Init();
            SettingsWindow.Center(new Point(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT));
            SettingsWindow.Visible = false;
            SettingsWindow.Resizable = false;
            SettingsWindow.IconVisible = false;
            SettingsWindow.DragAlpha = 255;
            SettingsWindow.Movable = false;

            var MusicVolumeLabel = new Label(gui);
            MusicVolumeLabel.Init();
            MusicVolumeLabel.Width = 120;
            MusicVolumeLabel.Parent = SettingsWindow;
            MusicVolumeLabel.Text = GlblRes.Music_Volume;
            MusicVolumeLabel.Top = 5;
            MusicVolumeLabel.Left = 5;

            var MusicVolumeBar = new TrackBar(gui);
            MusicVolumeBar.Init();
            MusicVolumeBar.Width = 160;
            MusicVolumeBar.Value = (int)(GameSettings.MusicVolume * 100);
            MusicVolumeBar.ValueChanged += (o, e) =>
            {
                GameSettings.MusicVolume = (float)MusicVolumeBar.Value / 100f;
                Engine.ApplyGameSettingsVolume();
                MediaPlayer.Volume = MathHelper.Clamp(GameSettings.MusicVolume, 0.0f, 1.0f);
                MainMenuLabel.Text = string.Empty;
            };
            MusicVolumeBar.Top = 5;
            MusicVolumeBar.Left = 120;
            MusicVolumeBar.Parent = SettingsWindow;

            var SoundVolumeLabel = new Label(gui);
            SoundVolumeLabel.Init();
            SoundVolumeLabel.Width = 120;
            SoundVolumeLabel.Parent = SettingsWindow;
            SoundVolumeLabel.Text = GlblRes.Sound_Volume;
            SoundVolumeLabel.Top = 30;
            SoundVolumeLabel.Left = 5;

            var SoundVolumeBar = new TrackBar(gui);
            SoundVolumeBar.Init();
            SoundVolumeBar.Width = 160;
            SoundVolumeBar.Value = (int)(GameSettings.SoundEffectVolume * 100);
            SoundVolumeBar.ValueChanged += (o, e) =>
            {
                GameSettings.SoundEffectVolume = (float)SoundVolumeBar.Value / 100f;
                Engine.ApplyGameSettingsVolume();
                MainMenuLabel.Text = string.Empty;
            };
            SoundVolumeBar.Top = 30;
            SoundVolumeBar.Left = 120;
            SoundVolumeBar.Parent = SettingsWindow;

            var BloomLabel = new Label(gui);
            BloomLabel.Init();
            BloomLabel.Width = 120;
            BloomLabel.Parent = SettingsWindow;
            BloomLabel.Text = GlblRes.Bloom;
            BloomLabel.Top = 55;
            BloomLabel.Left = 5;

            var BloomCheckBox = new CheckBox(gui);
            BloomCheckBox.Init();
            BloomCheckBox.Parent = SettingsWindow;
            BloomCheckBox.Text = string.Empty;
            BloomCheckBox.Checked = GameSettings.Bloom;
            BloomCheckBox.CheckedChanged += (o, e) =>
            {
                GameSettings.Bloom = BloomCheckBox.Checked;
                MainMenuLabel.Text = GlblRes.Restart_the_game_for_this_setting_to_take_effect;
            };
            BloomCheckBox.Top = 55;
            BloomCheckBox.Left = 120;

            var VSyncLabel = new Label(gui);
            VSyncLabel.Init();
            VSyncLabel.Width = 120;
            VSyncLabel.Parent = SettingsWindow;
            VSyncLabel.Text = GlblRes.VSync;
            VSyncLabel.Top = 80;
            VSyncLabel.Left = 5;

            var VSyncCheckBox = new CheckBox(gui);
            VSyncCheckBox.Init();
            VSyncCheckBox.Parent = SettingsWindow;
            VSyncCheckBox.Text = string.Empty;
            VSyncCheckBox.Checked = GameSettings.VSync;
            VSyncCheckBox.CheckedChanged += (o, e) =>
            {
                GameSettings.VSync = VSyncCheckBox.Checked;
                MainMenuLabel.Text = GlblRes.Restart_the_game_for_this_setting_to_take_effect;
            };
            VSyncCheckBox.Top = 80;
            VSyncCheckBox.Left = 120;

            var DisplayModeLabel = new Label(gui);
            DisplayModeLabel.Init();
            DisplayModeLabel.Width = 120;
            DisplayModeLabel.Parent = SettingsWindow;
            DisplayModeLabel.Text = GlblRes.Display_Mode;
            DisplayModeLabel.Top = 105;
            DisplayModeLabel.Left = 5;

            var DisplayModeCombo = new ComboBox(gui);
            DisplayModeCombo.Init();

            DisplayModeCombo.Items = GetDisplayModes();
            DisplayModeCombo.Width = 120;
            DisplayModeCombo.Parent = SettingsWindow;
            DisplayModeCombo.Width = 160;
            DisplayModeCombo.Text = DisplayModeToString(GameSettings.DisplayMode);
            DisplayModeCombo.ItemIndexChanged += (o, e) =>
            {
                GameSettings.DisplayMode = StringToDisplayMode(DisplayModeCombo.Text);
                MainMenuLabel.Text = GlblRes.Restart_the_game_for_this_setting_to_take_effect;
            };
            DisplayModeCombo.Top = 105;
            DisplayModeCombo.Left = 120;

            var Bevel = new Bevel(gui);
            Bevel.Parent = SettingsWindow;
            Bevel.Anchor = Anchors.Bottom | Anchors.Left | Anchors.Right;
            Bevel.Height = 35;
            Bevel.Style = BevelStyle.Raised;
            Bevel.Top = SettingsWindow.ClientHeight - Bevel.Height;
            Bevel.Width = SettingsWindow.ClientWidth;

            var OKButton = new MenuButton(gui, ClickSound, FocusSound, GameSettings);
            OKButton.Init();
            OKButton.Parent = Bevel;
            OKButton.Text = GlblRes.OK;
            OKButton.Click += (s, e) =>
            {
                SettingsWindow.Close();
                ShowLogo(true);
                GameSettings.Save(Engine.Game.SaveGameFolder);
            };
            OKButton.Width = 130;
            OKButton.Left = 150;
            OKButton.Top = 5;

            gui.Add(SettingsWindow);
        }

        private List<object> GetDisplayModes()
        {
            var Result = new List<object>();

            foreach (STACK.DisplayMode displayMode in Enum.GetValues(typeof(STACK.DisplayMode)))
            {
                Result.Add(DisplayModeToString(displayMode));
            }

            return Result;
        }

        private string DisplayModeToString(STACK.DisplayMode displayMode)
        {
            switch (displayMode)
            {
                case STACK.DisplayMode.Borderless:
                    return GlblRes.Borderless;
                case STACK.DisplayMode.BorderlessMaxInteger:
                    return GlblRes.Borderless_Fit;
                case STACK.DisplayMode.BorderlessScale:
                    return GlblRes.Borderless_Scale;
                case STACK.DisplayMode.Fullscreen:
                    return GlblRes.Fullscreen;
                case STACK.DisplayMode.Window:
                    return GlblRes.Window;
                case STACK.DisplayMode.WindowMaxInteger:
                    return GlblRes.Window_Fit;
            }

            return null;
        }

        private STACK.DisplayMode StringToDisplayMode(string val)
        {
            if (val == GlblRes.Borderless) return STACK.DisplayMode.Borderless;
            if (val == GlblRes.Borderless_Fit) return STACK.DisplayMode.BorderlessMaxInteger;
            if (val == GlblRes.Borderless_Scale) return STACK.DisplayMode.BorderlessScale;
            if (val == GlblRes.Fullscreen) return STACK.DisplayMode.Fullscreen;
            if (val == GlblRes.Window) return STACK.DisplayMode.Window;
            if (val == GlblRes.Window_Fit) return STACK.DisplayMode.WindowMaxInteger;

            return STACK.DisplayMode.WindowMaxInteger;
        }
    }
}
