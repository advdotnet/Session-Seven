using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STACK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;
using Window = TomShane.Neoforce.Controls.Window;

namespace SessionSeven
{
    public class Menu
    {
        private GameSettings GameSettings;
        private StackEngine Engine;
        private Dictionary<string, SaveGame> SaveGames;
        private List<Button> MenuButtons;
        private ImageBox MainMenuBackground;
        private Window ExitConfirmationWindow;
        private Window SaveGameWindow;
        private ListBox LoadGameListbox, SaveGameListbox;
        private Window LoadGameWindow;
        private Window SettingsWindow;
        private Label MainMenuLabel;

        class Item
        {
            public string Text;
            public Func<bool> IsVisible = () => { return true; };
            public Action OnClick;

            public Item(string text)
            {
                Text = text;
            }
        }

        IEnumerable<Item> GetMenuItems()
        {
            Item Item = new Item(GlblRes.Quit);
            Item.OnClick = Engine.Exit;

            yield return Item;

            //Item = new Item("Credits");

            //yield return Item;

            Item = new Item(GlblRes.Settings);
            Item.OnClick = () =>
            {
                SettingsWindow.ShowModal();
                ShowLogo(false);
                SettingsWindow.Focused = true;
            };

            yield return Item;

            Item = new Item(GlblRes.Load);
            Item.IsVisible = () => { return SaveGames.Count > 0; };
            Item.OnClick = () =>
            {
                LoadGameWindow.ShowModal();
                ShowLogo(false);
                LoadGameListbox.Focused = true;
            };
            yield return Item;

            Item = new Item(GlblRes.Save);
            Item.IsVisible = () => { return Engine.Game.World != null; };
            Item.OnClick = () =>
            {
                ShowLogo(false);
                SaveGameWindow.ShowModal();
            };

            yield return Item;

            Item = new Item(GlblRes.Continue);
            Item.OnClick = () =>
            {
                MainMenuBackground.Hide();
                Engine.Resume();
                Engine.Renderer.GUIManager.ShowSoftwareCursor = false;
            };
            Item.IsVisible = () => { return Engine.Game.World != null; };
            yield return Item;

            Item = new Item(GlblRes.New_Game);
            Item.OnClick = () =>
            {
                MainMenuBackground.Hide();
                Engine.Renderer.GUIManager.ShowSoftwareCursor = false;
                Engine.StartGame();
            };
            yield return Item;
        }

        void InitMenuButtons(Manager gui)
        {
            int i = 0;

            if (MenuButtons != null && MenuButtons.Count > 0)
            {
                foreach (var Button in MenuButtons)
                {
                    MainMenuBackground.Remove(Button);
                    gui.Remove(Button);
                }

                MenuButtons.Clear();
            }

            MenuButtons = new List<Button>();

            foreach (var Item in GetMenuItems().Where(it => it.IsVisible()))
            {
                var MenuButton = new Button(gui);

                MenuButton.Init();
                MenuButton.Text = Item.Text;
                if (Item.OnClick != null)
                {
                    MenuButton.Click += (s, e) => Item.OnClick();
                }
                MenuButton.Width = 120;
                MenuButton.Height = 24;
                MenuButton.Left = (MainMenuBackground.ClientWidth) - (MenuButton.Width) - 6;
                MenuButton.Top = MainMenuBackground.ClientHeight - MenuButton.Height - 6 - (30 * i);
                MenuButton.Anchor = Anchors.Top;

                MainMenuBackground.Add(MenuButton);
                MenuButtons.Add(MenuButton);

                i++;
            }
        }

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

            var OKButton = new Button(gui);
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

        public void Show()
        {
            RefreshSaveGames();
            InitMenuButtons(Engine.Renderer.GUIManager);
            MainMenuBackground.Show();
            ShowLogo(true);
        }

        private Cursor CreateCursor()
        {
            var Texture = Engine.EngineContent.Load<Texture2D>(content.ui.cursor);
            var HotSpot = new Vector2(20, 20);

            return new Cursor(Texture, HotSpot, Texture.Width, Texture.Height);
        }

        private void ShowLogo(bool show)
        {
            if (MenuButtons != null && MenuButtons.Count > 0)
            {
                foreach (var Button in MenuButtons)
                {
                    if (show)
                    {
                        Button.Show();
                    }
                    else
                    {
                        Button.Hide();
                    }
                }
            }

            if (show)
            {
                MainMenuBackground.Image = Engine.EngineContent.Load<Texture2D>(content.rooms.title.scene);
                MainMenuLabel.Text = "www.sessionseven.com";
            }
            else
            {
                MainMenuBackground.Image = Engine.Renderer.WhitePixelTexture;
                MainMenuLabel.Text = string.Empty;
            }
        }

        public Menu(StackEngine engine)
        {
            Engine = engine;
            GameSettings = engine.GameSettings;

            var GUI = engine.Renderer.GUIManager;
            var Cursor = CreateCursor();

            GUI.ShowSoftwareCursor = true;

            GUI.Skin.Cursors["Default"].Resource = Cursor;
            GUI.SetCursor(Cursor);

            MainMenuBackground = new ImageBox(GUI); // Imagebox showing the image                        
            MainMenuBackground.Init();
            MainMenuBackground.SizeMode = SizeMode.Stretched;

            MainMenuBackground.Width = engine.Renderer.DisplaySettings.VirtualResolution.X;
            MainMenuBackground.Height = engine.Renderer.DisplaySettings.VirtualResolution.Y;

            MainMenuLabel = new Label(GUI);
            MainMenuLabel.Init();
            MainMenuLabel.Text = string.Empty;
            MainMenuLabel.TextColor = Color.Black;
            MainMenuLabel.Parent = MainMenuBackground;
            MainMenuLabel.Width = engine.Renderer.DisplaySettings.VirtualResolution.X - 12;
            MainMenuLabel.Height = 24;
            MainMenuLabel.Left = 6;
            MainMenuLabel.Top = engine.Renderer.DisplaySettings.VirtualResolution.Y - MainMenuLabel.Height;

            AddExitConfirmationWindow(GUI);
            AddLoadGameWindow(GUI);
            AddSaveGameWindow(GUI);
            AddSettingsWindow(GUI);
            RefreshSaveGames();
            InitMenuButtons(GUI);

            GUI.Add(MainMenuBackground);

            ShowLogo(true);
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
