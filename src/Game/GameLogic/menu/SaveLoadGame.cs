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
        private Dictionary<string, SaveGame> SaveGames;
        private Window SaveGameWindow;
        private ListBox LoadGameListbox, SaveGameListbox;
        private Window LoadGameWindow;

        void AddLoadGameWindow(Manager gui)
        {
            var LoadButton = new Button(gui);

            LoadGameWindow = new Window(gui);
            LoadGameWindow.Width = 300;
            LoadGameWindow.Height = 200;
            LoadGameWindow.CloseButtonVisible = false;
            LoadGameWindow.Text = GlblRes.Load_Savegame;
            LoadGameWindow.Parent = MainMenuBackground;
            LoadGameWindow.Init();
            LoadGameWindow.Center(new Point(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT));
            LoadGameWindow.Visible = false;
            LoadGameWindow.Resizable = false;
            LoadGameWindow.IconVisible = false;
            LoadGameWindow.DragAlpha = 255;
            LoadGameWindow.Movable = false;

            var SaveGameScreenshot = new ImageBox(gui);
            SaveGameScreenshot.Parent = MainMenuBackground;
            SaveGameScreenshot.Width = MainMenuBackground.Width;
            SaveGameScreenshot.Height = MainMenuBackground.Height;
            SaveGameScreenshot.Visible = false;
            SaveGameScreenshot.SizeMode = SizeMode.Stretched;

            LoadGameWindow.Closed += (s, e) => { LoadGameListbox.ItemIndex = -1; };

            LoadGameListbox = new ListBox(gui);
            LoadGameListbox.Width = LoadGameWindow.ClientWidth;
            LoadGameListbox.Parent = LoadGameWindow;
            LoadGameListbox.Height = LoadGameWindow.ClientHeight - 35;
            LoadGameListbox.ItemIndexChanged += (s, e) =>
            {
                MainMenuLabel.Text = string.Empty;
                if (LoadGameListbox.ItemIndex > -1)
                {
                    if (SaveGameScreenshot.Image != null)
                    {
                        SaveGameScreenshot.Image.Dispose();
                    }

                    var Screenshot = SaveGames[SaveGames.Keys.ElementAt(LoadGameListbox.ItemIndex)].Screenshot;
                    if (Screenshot != null)
                    {
                        using (Stream ScreenshotStream = new MemoryStream(Screenshot))
                        {
                            SaveGameScreenshot.Image = Texture2D.FromStream(Engine.Renderer.GraphicsDevice, ScreenshotStream);
                        }
                    }

                    SaveGameScreenshot.Show();
                    LoadButton.Enabled = true;
                }
                else
                {
                    SaveGameScreenshot.Hide();
                    LoadButton.Enabled = false;
                }
            };

            var Bevel = new Bevel(gui);
            Bevel.Parent = LoadGameWindow;
            Bevel.Anchor = Anchors.Bottom | Anchors.Left | Anchors.Right;
            Bevel.Height = 35;
            Bevel.Style = BevelStyle.Raised;
            Bevel.Top = LoadGameWindow.ClientHeight - Bevel.Height;
            Bevel.Width = LoadGameWindow.ClientWidth;

            LoadButton.Init();
            LoadButton.Parent = Bevel;
            LoadButton.Enabled = false;
            LoadButton.Text = GlblRes.Load_Savegame;
            LoadButton.Click += (s, e) =>
            {
                MainMenuLabel.Text = string.Empty;
                try
                {
                    Engine.LoadState(SaveGames.Keys.ElementAt(LoadGameListbox.ItemIndex));
                }
                catch
                {
                    MainMenuLabel.Text = GlblRes.Could_not_load_save_game_Maybe_it_was_created_in_an_earlier_game_version;
                    return;
                }

                LoadGameWindow.Close();
                MainMenuBackground.Hide();
                Engine.Resume();
                Engine.Renderer.GUIManager.ShowSoftwareCursor = false;
            };
            LoadButton.Width = 130;
            LoadButton.Left = 5;
            LoadButton.Top = 5;

            var CancelButton = new Button(gui);
            CancelButton.Init();
            CancelButton.Parent = Bevel;
            CancelButton.Text = GlblRes.Cancel;
            CancelButton.Click += (s, e) =>
            {
                LoadGameWindow.Close();
                ShowLogo(true);
            };
            CancelButton.Width = 130;
            CancelButton.Left = 150;
            CancelButton.Top = 5;

            gui.Add(LoadGameWindow);
        }

        void AddSaveGameWindow(Manager gui)
        {
            var SaveButton = new Button(gui);
            var NameTextBox = new TextBox(gui);

            SaveGameWindow = new Window(gui);
            SaveGameWindow.Width = 300;
            SaveGameWindow.Height = 240;
            SaveGameWindow.CloseButtonVisible = false;
            SaveGameWindow.Text = GlblRes.Save;
            SaveGameWindow.IconVisible = false;
            SaveGameWindow.Parent = MainMenuBackground;
            SaveGameWindow.Init();
            SaveGameWindow.Visible = false;
            SaveGameWindow.Resizable = false;
            SaveGameWindow.Center(new Point(Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT));
            SaveGameWindow.Movable = false;

            SaveGameWindow.Closed += (s, e) => { SaveGameListbox.ItemIndex = -1; };

            SaveGameListbox = new ListBox(gui);
            SaveGameListbox.Width = SaveGameWindow.ClientWidth;
            SaveGameListbox.Parent = SaveGameWindow;
            SaveGameListbox.Height = SaveGameWindow.ClientHeight - 35 - 30;
            SaveGameListbox.ItemIndexChanged += (s, e) =>
            {
                if (SaveGameListbox.ItemIndex > -1)
                {
                    NameTextBox.Text = SaveGames[SaveGames.Keys.ElementAt(SaveGameListbox.ItemIndex)].Name;
                }
            };

            var Bevel = new Bevel(gui);
            Bevel.Parent = SaveGameWindow;
            Bevel.Anchor = Anchors.Bottom | Anchors.Left | Anchors.Right;
            Bevel.Height = 35;
            Bevel.Style = BevelStyle.Raised;
            Bevel.Top = SaveGameWindow.ClientHeight - Bevel.Height;
            Bevel.Width = SaveGameWindow.ClientWidth;


            SaveButton.Init();
            SaveButton.Parent = Bevel;
            SaveButton.Enabled = false;
            SaveButton.Text = GlblRes.Save;
            SaveButton.Click += (s, e) =>
            {
                Engine.SaveState(NameTextBox.Text);
                ShowLogo(true);
                SaveGameWindow.Close();
                RefreshSaveGames();
            };
            SaveButton.Width = 130;
            SaveButton.Left = 5;
            SaveButton.Top = 5;

            var CancelButton = new Button(gui);
            CancelButton.Init();
            CancelButton.Parent = Bevel;
            CancelButton.Text = GlblRes.Cancel;
            CancelButton.Click += (s, e) =>
            {
                SaveGameWindow.Close();
                ShowLogo(true);
            };
            CancelButton.Width = 130;
            CancelButton.Left = Bevel.ClientWidth - 130 - 5;
            CancelButton.Top = 5;

            gui.Add(SaveGameWindow);

            var SaveGameNameLabel = new Label(gui);
            SaveGameNameLabel.Parent = SaveGameWindow;
            SaveGameNameLabel.Top = SaveGameListbox.Height + 5;
            SaveGameNameLabel.Left = 5;
            SaveGameNameLabel.Width = 95;
            SaveGameNameLabel.Text = GlblRes.Name;

            NameTextBox.Parent = SaveGameWindow;
            NameTextBox.Init();
            NameTextBox.Left = 60;
            NameTextBox.Top = SaveGameListbox.Height + 5;
            NameTextBox.Width = 220;
            NameTextBox.Height = 20;
            NameTextBox.TextChanged += (s, e) =>
            {
                SaveButton.Enabled = !string.IsNullOrWhiteSpace(NameTextBox.Text);
            };
            NameTextBox.KeyPress += (s, e) =>
            {
                if (e.Key == Microsoft.Xna.Framework.Input.Keys.Enter)
                {
                    Engine.SaveState(NameTextBox.Text);
                    SaveGameWindow.Close();
                    ShowLogo(true);
                    RefreshSaveGames();
                }
            };

            gui.Add(SaveGameWindow);
        }

        void RefreshSaveGames()
        {
            SaveGames = new Dictionary<string, SaveGame>();
            var ListBoxes = new[] { LoadGameListbox, SaveGameListbox };

            foreach (var ListBox in ListBoxes)
            {
                ListBox.Items.Clear();
            }

            foreach (var Game in Engine.GetSaveGames())
            {
                SaveGames.Add(Game.Key, Game.Value);

                foreach (var ListBox in ListBoxes)
                {
                    ListBox.Items.Add(Game.Value.Name + " (" + Game.Value.Date.ToShortDateString() + " " + Game.Value.Date.ToLongTimeString() + ")");
                }
            }
        }
    }
}
