using Microsoft.Xna.Framework;
using System;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;

namespace SessionSeven
{
	public partial class Menu
	{
		private Bevel _creditsWindow;

		private void AddCreditsWindow(Manager gui)
		{
			_creditsWindow = new Bevel(gui)
			{
				Parent = _mainMenuBackground,
				Height = _mainMenuBackground.ClientHeight,
				Color = Color.Black,
				Style = BevelStyle.None,
				Visible = false,
				Top = 0,
				StayOnTop = true,
				Width = _mainMenuBackground.ClientWidth
			};

			var i = 0;
			var newLineNewLine = new string[] { Environment.NewLine + Environment.NewLine };

			foreach (var creditsText in Cutscenes.Director.GetCreditTexts(false))
			{
				var top = 110 + (i * 23) + (i > 4 ? 23 : 0);
				var text = creditsText.Split(newLineNewLine, StringSplitOptions.RemoveEmptyEntries);
				if (text.Length == 2)
				{
					var label = new Label(gui);
					label.Init();
					label.Width = 300;
					label.Height = 100;
					label.Alignment = Alignment.TopLeft;
					label.TextColor = Color.Black;
					label.Parent = _creditsWindow;
					label.Text = text[0];
					label.Top = top;
					label.Left = (_mainMenuBackground.ClientWidth / 2) - (label.Width / 2);

					label = new Label(gui);
					label.Init();
					label.Width = 300;
					label.Height = 100;
					label.Alignment = Alignment.TopRight;
					label.TextColor = new Color(188, 22, 0, 255);
					label.Parent = _creditsWindow;
					label.Text = text[1];
					label.Top = top;
					label.Left = (_mainMenuBackground.ClientWidth / 2) - (label.Width / 2);
				}
				else
				{
					var label = new Label(gui);
					label.Init();
					label.Width = 300;
					label.Height = 100;
					label.Alignment = Alignment.TopCenter;
					label.TextColor = Color.Black;
					label.Parent = _creditsWindow;
					label.Text = text[0];
					label.Top = top;
					label.Left = (_mainMenuBackground.ClientWidth / 2) - (label.Width / 2);
				}
				i++;
			}

			var okButton = new MenuButton(gui, _clickSound, _focusSound, _gameSettings);
			okButton.Init();
			okButton.Parent = _creditsWindow;
			okButton.Text = GlblRes.OK;
			okButton.Click += (s, e) =>
			{
				_creditsWindow.Hide();
				ShowLogo(true);
			};

			okButton.Width = 120;
			okButton.Height = 24;
			okButton.Left = _mainMenuBackground.ClientWidth - okButton.Width - 6;
			okButton.Top = _mainMenuBackground.ClientHeight - okButton.Height - 6 - (30 * 0);
			okButton.Anchor = Anchors.Top;

			gui.Add(_creditsWindow);
		}
	}
}
