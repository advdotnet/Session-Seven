using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;
using Window = TomShane.Neoforce.Controls.Window;

namespace SessionSeven
{
    public partial class Menu
    {
        private Window ExitConfirmationWindow;

        void AddExitConfirmationWindow(Manager gui)
        {
            ExitConfirmationWindow = new Window(gui);
            ExitConfirmationWindow.Init();
            ExitConfirmationWindow.Text = GlblRes.Really_quit;
            ExitConfirmationWindow.Width = 290;
            ExitConfirmationWindow.Height = 130;
            ExitConfirmationWindow.Resizable = false;
            ExitConfirmationWindow.IconVisible = false;
            ExitConfirmationWindow.Movable = false;
            ExitConfirmationWindow.Center(new Point(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT));
            ExitConfirmationWindow.Visible = false;
            ExitConfirmationWindow.CloseButtonVisible = false;

            Bevel Bevel = new Bevel(gui);
            Bevel.Parent = ExitConfirmationWindow;
            Bevel.Anchor = Anchors.Bottom | Anchors.Left | Anchors.Right;
            Bevel.Height = 35;
            Bevel.Style = BevelStyle.Raised;
            Bevel.Top = ExitConfirmationWindow.ClientHeight - Bevel.Height;
            Bevel.Width = ExitConfirmationWindow.ClientWidth;


            Label Text = new Label(gui);
            Text.Text = GlblRes.Do_you_really_want_to_quit_the_game;
            Text.Parent = ExitConfirmationWindow;
            Text.Top = 10;
            Text.Left = 10;
            Text.Width = ExitConfirmationWindow.ClientWidth - 20;
            Text.Height = 20;

            Button Yes = new Button(gui);
            Yes.Parent = Bevel;
            Yes.Width = 100;
            Yes.Text = GlblRes.Yes;
            Yes.Left = 10;
            Yes.Top = 5;
            Yes.Click += (se, ve) =>
            {
                Engine.Exit();
            };

            Button No = new Button(gui);
            No.Parent = Bevel;
            No.Text = GlblRes.No;
            No.Width = 100;
            No.Left = 165;
            No.Top = 5;
            No.Click += (se, ev) =>
            {
                ExitConfirmationWindow.Close();
            };

            gui.Add(ExitConfirmationWindow);
        }
    }
}
