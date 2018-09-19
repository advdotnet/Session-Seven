using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using STACK.Graphics;
using System;
using System.Collections;
using System.Linq;

namespace SessionSeven.GUI.Dialog
{
    [Serializable]
    public class Menu : Entity
    {
        public const string SCRIPTID = "WAITFORDIALOGSELECTIONSCRIPTID";
        public DialogMenuState State { get; private set; }
        public BaseOption LastSelectedOption { get; private set; }

        int Height, SelectedOptionIndex;
        int Width = Game.VIRTUAL_WIDTH;
        float currentY = 0;
        public IDialogOptions Options { get; private set; }

        public Menu()
        {
            Enabled = false;
            Visible = false;

            State = DialogMenuState.Closed;

            Text
                .Create(this)
                .SetFont(content.fonts.pixeloperator_BMF)
                .SetWidth(Game.VIRTUAL_WIDTH - 2 * Game.SCREEN_PADDING)
                .SetWordWrap(true)
                .SetAlign(Alignment.Left | Alignment.Bottom)
                .SetColor(new Color(150, 150, 150, 255))
                .SetVisible(false);

            HotspotPersistent
                .Create(this);

            Sprite
                .Create(this)
                .SetRenderStage(RenderStage.PostBloom)
                .SetImage(Sprite.WHITEPIXELIMAGE);

            SpriteData
                .Create(this)
                .SetColor(Color.Black);
        }

        Text Lines
        {
            get
            {
                return Get<Text>();
            }
        }

        public override void OnDraw(Renderer renderer)
        {
            base.OnDraw(renderer);

            if (renderer.Stage == RenderStage.PostBloom)
            {
                if (State != DialogMenuState.Closed)
                {
                    var DrawX = 0;
                    var DrawY = Game.VIRTUAL_HEIGHT - (int)currentY - 5;
                    var DrawHeight = Height + 5;
                    var BackgroundColor = new Color(19, 19, 19, (byte)(200 * 1));

                    // border                     
                    var Rectangle = new Rectangle(DrawX, DrawY, Width, DrawHeight);
                    renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, Rectangle, BackgroundColor);

                    Lines.Offset.Y = -(Height - currentY);
                    Lines.Draw(renderer);
                }
            }
        }

        public Script StartSelectionScript(Scripts scripts)
        {
            return scripts.Start(WaitForSelection(), SCRIPTID);
        }

        /// <summary>
        /// Opens the dialog menu and yields as long as no selection was made.
        /// </summary>
        private IEnumerator WaitForSelection(bool setInteractive = true)
        {
            Game.StopSkipping();

            while (State != DialogMenuState.Open && State != DialogMenuState.Closed)
            {
                yield return 0;
            }

            if (setInteractive)
            {
                yield return 0;
                World.Interactive = true;
            }

            while (State != DialogMenuState.Closing && State != DialogMenuState.Closed)
            {
                yield return 0;
            }

            if (setInteractive)
            {
                World.Interactive = false;
            }

            while (State != DialogMenuState.Closed)
            {
                yield return 0;
            }
        }

        /// <summary>
        /// Shows the dialog menu for the given dialog options.
        /// </summary>        
        public void Open(IDialogOptions options)
        {
            if (options == null || options.Count == 0)
            {
                Close();
                return;
            }

            Options = options;

            SelectedOptionIndex = -1;
            LastSelectedOption = BaseOption.None;
            State = DialogMenuState.Opening;

            currentY = -200;

            Visible = true;
            Enabled = true;

            Lines.Set(string.Join("\n", options.SelectTexts()), TextDuration.Persistent, new Vector2(Game.VIRTUAL_WIDTH / 2, 0));

            var FirstOptionLine = Lines.Lines[0];
            var LastOptionLine = Lines.Lines[Lines.Lines.Count - 1];

            Height = (int)(LastOptionLine.Position.Y - FirstOptionLine.Position.Y + LastOptionLine.Hitbox.Height + Game.SCREEN_PADDING);

            // shift the options eventually
            Lines.SetPosition(new Vector2(Game.VIRTUAL_WIDTH / 2, Game.VIRTUAL_HEIGHT - Height + FirstOptionLine.Origin.Y));

            for (int i = 0; i < Lines.Lines.Count; i++)
            {
                var CurrentHitbox = Lines.Lines[i].Hitbox;
                Lines.Lines[i] = Lines.Lines[i].ChangeHitbox(new Rectangle(0, CurrentHitbox.Y, Width, CurrentHitbox.Height));
            }
        }

        public void Close()
        {
            State = DialogMenuState.Closing;
        }

        public void Click()
        {
            if (State == DialogMenuState.Open)
            {
                if (SelectedOptionIndex > -1)
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// Selects an option programatically.
        /// </summary>
        /// <param name="optionId"></param>
        public void SelectOption(int optionId)
        {
            if (State != DialogMenuState.Open)
            {
                throw new InvalidOperationException("Options not open.");
            }

            for (int i = 0; i < Options.Count; i++)
            {
                if (optionId == Options[i].ID)
                {
                    SelectedOptionIndex = i;
                    LastSelectedOption = Options[i];
                    Close();

                    return;
                }
            }

            throw new InvalidOperationException("Option with given ID is not avaiable.");
        }

        public override void OnUpdate()
        {
            if (State == DialogMenuState.Opening)
            {
                currentY += ((Height - currentY) / 10f) + 2;

                if (Height - currentY < 1f)
                {
                    currentY = Height;
                }

                if (currentY >= Height)
                {
                    State = DialogMenuState.Open;
                }
            }

            if (State == DialogMenuState.Closing)
            {
                currentY -= (Height - currentY) + 2;

                if (currentY <= -200)
                {
                    State = DialogMenuState.Closed;
                    Visible = false;
                    Enabled = false;
                }
            }

            if (State == DialogMenuState.Open)
            {
                SelectedOptionIndex = -1;

                var MousePosition = World.Get<STACK.Components.Mouse>().Position;

                for (int i = 0; i < Lines.Lines.Count; i++)
                {
                    if (Lines.Lines[i].Hitbox.Contains(MousePosition))
                    {
                        SelectedOptionIndex = i;
                        Lines.Lines[i] = Lines.Lines[i].ChangeColor(Color.White);
                    }
                    else
                    {
                        Lines.Lines[i] = Lines.Lines[i].ChangeColor(Lines.Color);
                    }
                }

                if (SelectedOptionIndex == -1)
                {
                    LastSelectedOption = BaseOption.None;
                }
                else
                {
                    LastSelectedOption = Options[SelectedOptionIndex];
                }
            }

            base.OnUpdate();
        }
    }
}
