using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using STACK;
using System;
using TomShane.Neoforce.Controls;

namespace SessionSeven
{
    public partial class Menu : IDisposable
    {
        private StackEngine Engine;
        private ImageBox MainMenuBackground;
        private Label MainMenuLabel;
        private SoundEffect FocusSound, ClickSound;

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

        public void Dispose()
        {
            if (null != FocusSound)
            {
                FocusSound.Dispose();
            }
            if (null != ClickSound)
            {
                FocusSound.Dispose();
            }
        }

        public Menu(StackEngine engine)
        {
            FocusSound = engine.EngineContent.Load<SoundEffect>(content.audio.menu_click);
            ClickSound = engine.EngineContent.Load<SoundEffect>(content.audio.menu_focus);

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
            AddCreditsWindow(GUI);
            RefreshSaveGames();
            InitMenuButtons(GUI);

            GUI.Add(MainMenuBackground);

            ShowLogo(true);
        }
    }
}
