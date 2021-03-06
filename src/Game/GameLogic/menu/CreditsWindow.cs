﻿using Microsoft.Xna.Framework;
using System;
using TomShane.Neoforce.Controls;
using GlblRes = global::SessionSeven.Properties.Resources;

namespace SessionSeven
{
    public partial class Menu
    {
        private Bevel CreditsWindow;

        void AddCreditsWindow(Manager gui)
        {
            CreditsWindow = new Bevel(gui);
            CreditsWindow.Parent = MainMenuBackground;
            CreditsWindow.Height = MainMenuBackground.ClientHeight;
            CreditsWindow.Color = Color.Black;
            CreditsWindow.Style = BevelStyle.None;
            CreditsWindow.Visible = false;
            CreditsWindow.Top = 0;
            CreditsWindow.StayOnTop = true;
            CreditsWindow.Width = MainMenuBackground.ClientWidth;

            var i = 0;
            var NLNL = new string[] { Environment.NewLine + Environment.NewLine };

            foreach (var CreditsText in Cutscenes.Director.GetCreditTexts(false))
            {
                var Top = 110 + i * 23 + (i > 4 ? 23 : 0);
                var Text = CreditsText.Split(NLNL, StringSplitOptions.RemoveEmptyEntries);
                if (Text.Length == 2)
                {
                    var Label = new Label(gui);
                    Label.Init();
                    Label.Width = 300;
                    Label.Height = 100;
                    Label.Alignment = Alignment.TopLeft;
                    Label.TextColor = Color.Black;
                    Label.Parent = CreditsWindow;
                    Label.Text = Text[0];
                    Label.Top = Top;                    
                    Label.Left = MainMenuBackground.ClientWidth / 2 - Label.Width / 2;

                    Label = new Label(gui);
                    Label.Init();
                    Label.Width = 300;
                    Label.Height = 100;
                    Label.Alignment = Alignment.TopRight;
                    Label.TextColor = new Color(188, 22, 0, 255);
                    Label.Parent = CreditsWindow;
                    Label.Text = Text[1];
                    Label.Top = Top;                                       
                    Label.Left = MainMenuBackground.ClientWidth / 2 - Label.Width / 2;
                }
                else
                {
                    var Label = new Label(gui);
                    Label.Init();
                    Label.Width = 300;
                    Label.Height = 100;
                    Label.Alignment = Alignment.TopCenter;
                    Label.TextColor = Color.Black;
                    Label.Parent = CreditsWindow;
                    Label.Text = Text[0];
                    Label.Top = Top;
                    Label.Left = MainMenuBackground.ClientWidth / 2 - Label.Width / 2;
                }
                i++;
            }

            var OKButton = new MenuButton(gui, ClickSound, FocusSound, GameSettings);
            OKButton.Init();
            OKButton.Parent = CreditsWindow;
            OKButton.Text = GlblRes.OK;
            OKButton.Click += (s, e) =>
            {
                CreditsWindow.Hide();
                ShowLogo(true);
            };

            OKButton.Width = 120;
            OKButton.Height = 24;
            OKButton.Left = (MainMenuBackground.ClientWidth) - (OKButton.Width) - 6;
            OKButton.Top = MainMenuBackground.ClientHeight - OKButton.Height - 6 - (30 * 0);
            OKButton.Anchor = Anchors.Top;

            gui.Add(CreditsWindow);
        }
    }
}
