using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STACK;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;
using Window = TomShane.Neoforce.Controls.Window;

namespace SessionSeven
{
	public partial class Menu
	{
		private Dictionary<string, SaveGame> _saveGames;
		private Window _saveGameWindow;
		private ListBox _loadGameListbox, _saveGameListbox;
		private Window _loadGameWindow;

		private void AddLoadGameWindow(Manager gui)
		{
			var loadButton = new MenuButton(gui, _clickSound, _focusSound, _gameSettings);

			_loadGameWindow = new Window(gui)
			{
				Width = 300,
				Height = 200,
				CloseButtonVisible = false,
				Text = GlblRes.Load_Savegame,
				Parent = _mainMenuBackground
			};
			_loadGameWindow.Init();
			_loadGameWindow.Center(new Point(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT));
			_loadGameWindow.Visible = false;
			_loadGameWindow.Resizable = false;
			_loadGameWindow.IconVisible = false;
			_loadGameWindow.DragAlpha = 255;
			_loadGameWindow.Movable = false;

			var saveGameScreenshot = new ImageBox(gui)
			{
				Parent = _mainMenuBackground,
				Width = _mainMenuBackground.Width,
				Height = _mainMenuBackground.Height,
				Visible = false,
				SizeMode = SizeMode.Stretched
			};

			_loadGameWindow.Closed += (s, e) => _loadGameListbox.ItemIndex = -1;

			_loadGameListbox = new ListBox(gui)
			{
				Width = _loadGameWindow.ClientWidth,
				Parent = _loadGameWindow,
				Height = _loadGameWindow.ClientHeight - 35
			};
			_loadGameListbox.ItemIndexChanged += (s, e) =>
			{
				_clickSound.Play(_gameSettings.SoundEffectVolume, 0f, 0f);
				_mainMenuLabel.Text = string.Empty;
				if (_loadGameListbox.ItemIndex > -1)
				{
					saveGameScreenshot.Image?.Dispose();

					var screenshot = _saveGames[_saveGames.Keys.ElementAt(_loadGameListbox.ItemIndex)].Screenshot;
					if (screenshot != null)
					{
						using (Stream screenshotStream = new MemoryStream(screenshot))
						{
							saveGameScreenshot.Image = Texture2D.FromStream(_engine.Renderer.GraphicsDevice, screenshotStream);
						}
					}

					saveGameScreenshot.Show();
					loadButton.Enabled = true;
				}
				else
				{
					saveGameScreenshot.Hide();
					loadButton.Enabled = false;
				}
			};

			var bevel = new Bevel(gui)
			{
				Parent = _loadGameWindow,
				Anchor = Anchors.Bottom | Anchors.Left | Anchors.Right,
				Height = 35,
				Style = BevelStyle.Raised
			};
			bevel.Top = _loadGameWindow.ClientHeight - bevel.Height;
			bevel.Width = _loadGameWindow.ClientWidth;

			loadButton.Init();
			loadButton.Parent = bevel;
			loadButton.Enabled = false;
			loadButton.Text = GlblRes.Load_Savegame;
			loadButton.Click += (s, e) =>
			{
				_mainMenuLabel.Text = string.Empty;
				try
				{
					_engine.LoadState(_saveGames.Keys.ElementAt(_loadGameListbox.ItemIndex));
				}
				catch
				{
					_mainMenuLabel.Text = GlblRes.Could_not_load_save_game_Maybe_it_was_created_in_an_earlier_game_version;
					return;
				}

				_loadGameWindow.Close();
				_mainMenuBackground.Hide();
				_engine.Resume();
				_engine.Renderer.GUIManager.ShowSoftwareCursor = false;
			};
			loadButton.Width = 130;
			loadButton.Left = 5;
			loadButton.Top = 5;

			var cancelButton = new MenuButton(gui, _clickSound, _focusSound, _gameSettings);
			cancelButton.Init();
			cancelButton.Parent = bevel;
			cancelButton.Text = GlblRes.Cancel;
			cancelButton.Click += (s, e) =>
			{
				_loadGameWindow.Close();
				ShowLogo(true);
			};
			cancelButton.Width = 130;
			cancelButton.Left = 150;
			cancelButton.Top = 5;

			gui.Add(_loadGameWindow);
		}

		private void AddSaveGameWindow(Manager gui)
		{
			var saveButton = new MenuButton(gui, _clickSound, _focusSound, _gameSettings);
			var nameTextBox = new TextBox(gui);

			_saveGameWindow = new Window(gui)
			{
				Width = 300,
				Height = 240,
				CloseButtonVisible = false,
				Text = GlblRes.Save,
				IconVisible = false,
				Parent = _mainMenuBackground
			};
			_saveGameWindow.Init();
			_saveGameWindow.Visible = false;
			_saveGameWindow.Resizable = false;
			_saveGameWindow.Center(new Point(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT));
			_saveGameWindow.Movable = false;

			_saveGameWindow.Closed += (s, e) => _saveGameListbox.ItemIndex = -1;

			_saveGameListbox = new ListBox(gui)
			{
				Width = _saveGameWindow.ClientWidth,
				Parent = _saveGameWindow,
				Height = _saveGameWindow.ClientHeight - 35 - 30
			};
			_saveGameListbox.ItemIndexChanged += (s, e) =>
			{
				_clickSound.Play(_gameSettings.SoundEffectVolume, 0f, 0f);
				if (_saveGameListbox.ItemIndex > -1)
				{
					nameTextBox.Text = _saveGames[_saveGames.Keys.ElementAt(_saveGameListbox.ItemIndex)].Name;
				}
			};

			var bevel = new Bevel(gui)
			{
				Parent = _saveGameWindow,
				Anchor = Anchors.Bottom | Anchors.Left | Anchors.Right,
				Height = 35,
				Style = BevelStyle.Raised
			};
			bevel.Top = _saveGameWindow.ClientHeight - bevel.Height;
			bevel.Width = _saveGameWindow.ClientWidth;

			saveButton.Init();
			saveButton.Parent = bevel;
			saveButton.Enabled = false;
			saveButton.Text = GlblRes.Save;
			saveButton.Click += (s, e) =>
			{
				_engine.SaveState(nameTextBox.Text);
				ShowLogo(true);
				_saveGameWindow.Close();
				RefreshSaveGames();
			};
			saveButton.Width = 130;
			saveButton.Left = 5;
			saveButton.Top = 5;

			var cancelButton = new MenuButton(gui, _clickSound, _focusSound, _gameSettings);
			cancelButton.Init();
			cancelButton.Parent = bevel;
			cancelButton.Text = GlblRes.Cancel;
			cancelButton.Click += (s, e) =>
			{
				_saveGameWindow.Close();
				ShowLogo(true);
			};
			cancelButton.Width = 130;
			cancelButton.Left = bevel.ClientWidth - 130 - 5;
			cancelButton.Top = 5;

			gui.Add(_saveGameWindow);

			var saveGameNameLabel = new Label(gui)
			{
				Parent = _saveGameWindow,
				Top = _saveGameListbox.Height + 5,
				Left = 5,
				Width = 95,
				Text = GlblRes.Name
			};

			nameTextBox.Parent = _saveGameWindow;
			nameTextBox.Init();
			nameTextBox.Left = 60;
			nameTextBox.Top = _saveGameListbox.Height + 5;
			nameTextBox.Width = 220;
			nameTextBox.Height = 20;
			nameTextBox.TextChanged += (s, e) => saveButton.Enabled = !string.IsNullOrWhiteSpace(nameTextBox.Text);
			nameTextBox.KeyPress += (s, e) =>
			{
				if (e.Key == Microsoft.Xna.Framework.Input.Keys.Enter)
				{
					_engine.SaveState(nameTextBox.Text);
					_saveGameWindow.Close();
					ShowLogo(true);
					RefreshSaveGames();
				}
			};

			gui.Add(_saveGameWindow);
		}

		private void RefreshSaveGames()
		{
			_saveGames = new Dictionary<string, SaveGame>();
			var listBoxes = new[] { _loadGameListbox, _saveGameListbox };

			foreach (var listBox in listBoxes)
			{
				listBox.Items.Clear();
			}

			var allSaveGames = _engine.GetSaveGames();
			var currentCulture = GameSettings.GetCurrentCultureName();
			var currentLocaleSavegames = allSaveGames.Where(s => s.Value.Culture == currentCulture);

			foreach (var game in currentLocaleSavegames)
			{
				_saveGames.Add(game.Key, game.Value);

				foreach (var listBox in listBoxes)
				{
					listBox.Items.Add($"{game.Value.Name} ({game.Value.Date.ToShortDateString()} {game.Value.Date.ToLongTimeString()})");
				}
			}
		}
	}
}
