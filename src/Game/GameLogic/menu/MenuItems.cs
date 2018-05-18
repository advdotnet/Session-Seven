﻿using System;
using System.Collections.Generic;
using System.Linq;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;

namespace SessionSeven
{
    public partial class Menu
    {
        private List<Button> MenuButtons;

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
            Item.OnClick = () =>
            {
                ExitConfirmationWindow.ShowModal();
                ShowLogo(false);
                ExitConfirmationWindow.Focused = true;
            };

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
    }
}
