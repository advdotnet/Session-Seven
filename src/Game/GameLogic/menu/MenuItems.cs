using System;
using System.Collections.Generic;
using System.Linq;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;

namespace SessionSeven
{
	public partial class Menu
	{
		private List<Button> _menuButtons;

		private class Item
		{
			public string Text;
			public Func<bool> IsVisible = () => true;
			public Action OnClick;

			public Item(string text)
			{
				Text = text;
			}
		}

		private bool GameRunning => null != _engine.Game.World;

		private void ContinueGame()
		{
			_mainMenuBackground.Hide();
			_engine.Resume();
			_engine.Renderer.GUIManager.ShowSoftwareCursor = false;
		}

		private IEnumerable<Item> GetMenuItems()
		{
			// Quit
			var item = new Item(GlblRes.Quit)
			{
				OnClick = () =>
				{
					if (GameRunning)
					{
						_exitConfirmationWindow.ShowModal();
						ShowLogo(false);
						_exitConfirmationWindow.Focused = true;
					}
					else
					{
						_engine.Exit();
					}
				}
			};

			yield return item;

			// Credits
			item = new Item(GlblRes.Credits)
			{
				OnClick = () =>
				{
					_creditsWindow.Show();
					ShowLogo(false);
					_creditsWindow.Focused = true;
				}
			};

			yield return item;

			// Settings
			item = new Item(GlblRes.Settings)
			{
				OnClick = () =>
				{
					_settingsWindow.ShowModal();
					ShowLogo(false);
					_settingsWindow.Focused = true;
				}
			};

			yield return item;

			// Load
			item = new Item(GlblRes.Load)
			{
				IsVisible = () => _saveGames.Count > 0,
				OnClick = () =>
				{
					_loadGameWindow.ShowModal();
					ShowLogo(false);
					_loadGameListbox.Focused = true;
				}
			};
			yield return item;

			// Save
			item = new Item(GlblRes.Save)
			{
				IsVisible = () => GameRunning,
				OnClick = () =>
				{
					ShowLogo(false);
					_saveGameWindow.ShowModal();
				}
			};

			yield return item;

			// Continue
			item = new Item(GlblRes.Continue)
			{
				OnClick = () => ContinueGame(),
				IsVisible = () => GameRunning
			};
			yield return item;

			// New Game
			item = new Item(GlblRes.New_Game)
			{
				OnClick = () =>
				{
					_mainMenuBackground.Hide();
					_engine.Renderer.GUIManager.ShowSoftwareCursor = false;
					_engine.StartGame();
				}
			};
			yield return item;
		}

		private void InitMenuButtons(Manager gui)
		{
			var i = 0;

			if (_menuButtons != null && _menuButtons.Count > 0)
			{
				foreach (var button in _menuButtons)
				{
					_mainMenuBackground.Remove(button);
					gui.Remove(button);
				}

				_menuButtons.Clear();
			}

			_menuButtons = new List<Button>();

			foreach (var item in GetMenuItems().Where(it => it.IsVisible()))
			{
				var menuButton = new MenuButton(gui, _clickSound, _focusSound, _gameSettings);

				menuButton.Init();
				menuButton.Text = item.Text;
				if (item.OnClick != null)
				{
					menuButton.Click += (s, e) => item.OnClick();
				}
				menuButton.Width = 120;
				menuButton.Height = 24;
				menuButton.Left = _mainMenuBackground.ClientWidth - menuButton.Width - 6;
				menuButton.Top = _mainMenuBackground.ClientHeight - menuButton.Height - 6 - (30 * i);
				menuButton.Anchor = Anchors.Top;

				_mainMenuBackground.Add(menuButton);
				_menuButtons.Add(menuButton);

				i++;
			}
		}

	}
}
