using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using STACK;
using STACK.Components;
using System;
using TomShane.Neoforce.Controls;

namespace SessionSeven
{
	public partial class Menu : IDisposable
	{
		private readonly StackEngine _engine;
		private readonly ImageBox _mainMenuBackground;
		private readonly Label _mainMenuLabel;
		private readonly SoundEffect _focusSound, _clickSound;

		public Song MenuSong { get; private set; }

		public void Show()
		{
			RefreshSaveGames();
			InitMenuButtons(_engine.Renderer.GUIManager);
			_mainMenuBackground.Show();
			ShowLogo(true);
		}

		private Cursor CreateCursor()
		{
			var texture = _engine.EngineContent.Load<Texture2D>(content.ui.cursor);
			var hotSpot = new Vector2(20, 20);

			return new Cursor(texture, hotSpot, texture.Width, texture.Height);
		}

		private void ShowLogo(bool show)
		{
			if (_menuButtons != null && _menuButtons.Count > 0)
			{
				foreach (var button in _menuButtons)
				{
					if (show)
					{
						button.Show();
					}
					else
					{
						button.Hide();
					}
				}
			}

			if (show)
			{
				_mainMenuBackground.Image = _engine.EngineContent.Load<Texture2D>(content.rooms.title.scene);
				_mainMenuLabel.Text = "www.sessionseven.com";
			}
			else
			{
				_mainMenuBackground.Image = _engine.Renderer.WhitePixelTexture;
				_mainMenuLabel.Text = string.Empty;
			}
		}

		public void Dispose()
		{
			_focusSound?.Dispose();
			_clickSound?.Dispose();

			if (null != MenuSong)
			{
				//MenuSong.Dispose();
			}
		}

		public Menu(StackEngine engine)
		{
			_focusSound = engine.EngineContent.Load<SoundEffect>(content.audio.menu_click);
			_clickSound = engine.EngineContent.Load<SoundEffect>(content.audio.menu_focus);
			MenuSong = engine.EngineContent.Load<Song>(content.audio.menu);

			_engine = engine;
			_gameSettings = engine.GameSettings;

			MediaPlayer.Volume = MathHelper.Clamp(_gameSettings.MusicVolume, 0.0f, 1.0f);
			if (!AudioManager.SoundDisabled)
			{
				MediaPlayer.Play(MenuSong);
			}
			MediaPlayer.IsRepeating = true;

			var gui = engine.Renderer.GUIManager;
			var cursor = CreateCursor();

			gui.ShowSoftwareCursor = true;

			gui.Skin.Cursors["Default"].Resource = cursor;
			gui.SetCursor(cursor);

			_mainMenuBackground = new ImageBox(gui); // Imagebox showing the image                        
			_mainMenuBackground.Init();
			_mainMenuBackground.SizeMode = SizeMode.Stretched;

			_mainMenuBackground.Width = engine.Renderer.DisplaySettings.VirtualResolution.X;
			_mainMenuBackground.Height = engine.Renderer.DisplaySettings.VirtualResolution.Y;

			_mainMenuLabel = new Label(gui);
			_mainMenuLabel.Init();
			_mainMenuLabel.Text = string.Empty;
			_mainMenuLabel.TextColor = Color.Black;
			_mainMenuLabel.Parent = _mainMenuBackground;
			_mainMenuLabel.Width = engine.Renderer.DisplaySettings.VirtualResolution.X - 12;
			_mainMenuLabel.Height = 24;
			_mainMenuLabel.Left = 6;
			_mainMenuLabel.Top = engine.Renderer.DisplaySettings.VirtualResolution.Y - _mainMenuLabel.Height;

			AddExitConfirmationWindow(gui);
			AddLoadGameWindow(gui);
			AddSaveGameWindow(gui);
			AddSettingsWindow(gui);
			AddCreditsWindow(gui);
			RefreshSaveGames();
			InitMenuButtons(gui);

			gui.Add(_mainMenuBackground);

			ShowLogo(true);
		}
	}
}
