using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;
using Window = TomShane.Neoforce.Controls.Window;

namespace SessionSeven
{
	public partial class Menu
	{
		private Window _exitConfirmationWindow;

		private void AddExitConfirmationWindow(Manager gui)
		{
			_exitConfirmationWindow = new Window(gui);
			_exitConfirmationWindow.Init();
			_exitConfirmationWindow.Text = GlblRes.Really_quit;
			_exitConfirmationWindow.Width = 290;
			_exitConfirmationWindow.Height = 130;
			_exitConfirmationWindow.Resizable = false;
			_exitConfirmationWindow.IconVisible = false;
			_exitConfirmationWindow.Movable = false;
			_exitConfirmationWindow.Center(new Point(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT));
			_exitConfirmationWindow.Visible = false;
			_exitConfirmationWindow.CloseButtonVisible = false;

			var bevel = new Bevel(gui)
			{
				Parent = _exitConfirmationWindow,
				Anchor = Anchors.Bottom | Anchors.Left | Anchors.Right,
				Height = 35,
				Style = BevelStyle.Raised
			};
			bevel.Top = _exitConfirmationWindow.ClientHeight - bevel.Height;
			bevel.Width = _exitConfirmationWindow.ClientWidth;


			var text = new Label(gui)
			{
				Text = GlblRes.Do_you_really_want_to_quit_the_game,
				Parent = _exitConfirmationWindow,
				Top = 10,
				Left = 10,
				Width = _exitConfirmationWindow.ClientWidth - 20,
				Height = 20
			};

			var yes = new MenuButton(gui, _clickSound, _focusSound, _gameSettings)
			{
				Parent = bevel,
				Width = 100,
				Text = GlblRes.Yes,
				Left = 10,
				Top = 5
			};
			yes.Click += (se, ve) => _engine.Exit();


			var no = new MenuButton(gui, _clickSound, _focusSound, _gameSettings)
			{
				Parent = bevel,
				Text = GlblRes.Menu_AddExitConfirmationWindow_No,
				Width = 100,
				Left = 165,
				Top = 5
			};
			no.Click += (se, ev) =>
			{
				_exitConfirmationWindow.Close();
				ShowLogo(true);
			};

			gui.Add(_exitConfirmationWindow);
		}
	}
}
