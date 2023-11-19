using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using STACK.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SessionSeven.GUI.Dialog
{
	[Serializable]
	public class Menu : Entity
	{
		public const string SCRIPTID = "WAITFORDIALOGSELECTIONSCRIPTID";
		public DialogMenuState State { get; private set; }
		public BaseOption LastSelectedOption { get; private set; }

		private int _height, _selectedOptionIndex;
		private readonly int _width = Game.VIRTUAL_WIDTH;
		private float _currentY = 0;
		public IDialogOptions Options { get; private set; }

		public Menu()
		{
			Enabled = false;
			Visible = false;

			State = DialogMenuState.Closed;

			Text
				.Create(this)
				.SetFont(content.fonts.pixeloperator_BMF)
				.SetWidth(Game.VIRTUAL_WIDTH - (2 * Game.SCREEN_PADDING))
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

		private Text Lines => Get<Text>();

		public override void OnDraw(Renderer renderer)
		{
			base.OnDraw(renderer);

			if (renderer.Stage == RenderStage.PostBloom)
			{
				if (State != DialogMenuState.Closed)
				{
					var drawX = 0;
					var drawY = Game.VIRTUAL_HEIGHT - (int)_currentY - 5;
					var drawHeight = _height + 5;
					var backgroundColor = new Color(19, 19, 19, 200 * 1);

					// border                     
					var rectangle = new Rectangle(drawX, drawY, _width, drawHeight);
					renderer.SpriteBatch.Draw(renderer.WhitePixelTexture, rectangle, backgroundColor);

					Lines.Offset.Y = -(_height - _currentY);
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

		private List<TextInfo> CreateTextInfos(IDialogOptions options)
		{
			var result = new List<TextInfo>();

			for (var i = 0; i < options.Count; i++)
			{
				var option = options[i];
				result.Add(new TextInfo(option.Text, option.ID.ToString()));
			}

			return result;
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

			_selectedOptionIndex = -1;
			LastSelectedOption = BaseOption.None;
			State = DialogMenuState.Opening;

			_currentY = -200;

			Visible = true;
			Enabled = true;

			var textInfos = CreateTextInfos(options);

			Lines.Set(textInfos, TextDuration.Persistent, new Vector2(Game.VIRTUAL_WIDTH / 2, 0));

			var firstOptionLine = Lines.Lines[0];
			var lastOptionLine = Lines.Lines[Lines.Lines.Count - 1];

			_height = (int)(lastOptionLine.Position.Y - firstOptionLine.Position.Y + lastOptionLine.Hitbox.Height + Game.SCREEN_PADDING);

			// shift the options eventually
			Lines.SetPosition(new Vector2(Game.VIRTUAL_WIDTH / 2, Game.VIRTUAL_HEIGHT - _height + firstOptionLine.Origin.Y));

			for (var i = 0; i < Lines.Lines.Count; i++)
			{
				var currentHitbox = Lines.Lines[i].Hitbox;
				Lines.Lines[i] = Lines.Lines[i].ChangeHitbox(new Rectangle(0, currentHitbox.Y, _width, currentHitbox.Height));
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
				if (_selectedOptionIndex > -1)
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

			for (var i = 0; i < Options.Count; i++)
			{
				if (optionId == Options[i].ID)
				{
					_selectedOptionIndex = i;
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
				_currentY += ((_height - _currentY) / 10f) + 2;

				if (_height - _currentY < 1f)
				{
					_currentY = _height;
				}

				if (_currentY >= _height)
				{
					State = DialogMenuState.Open;
				}
			}

			if (State == DialogMenuState.Closing)
			{
				_currentY -= _height - _currentY + 2;

				if (_currentY <= -200)
				{
					State = DialogMenuState.Closed;
					Visible = false;
					Enabled = false;
				}
			}

			if (State == DialogMenuState.Open)
			{
				var selectedOptionId = -1;
				var mousePosition = World.Get<STACK.Components.Mouse>().Position;

				for (var i = 0; i < Lines.Lines.Count; i++)
				{
					if (Lines.Lines[i].Hitbox.Contains(mousePosition))
					{
						selectedOptionId = Convert.ToInt32(Lines.Lines[i].Tag);
					}
				}

				if (selectedOptionId != -1)
				{
					for (var i = 0; i < Lines.Lines.Count; i++)
					{
						var optionIsSelected = Lines.Lines[i].Tag == selectedOptionId.ToString();
						if (optionIsSelected && Lines.Lines[i].Color != Color.White)
						{
							Lines.Lines[i] = Lines.Lines[i].ChangeColor(Color.White);
						}
						else if (!optionIsSelected && Lines.Lines[i].Color != Lines.Color)
						{
							Lines.Lines[i] = Lines.Lines[i].ChangeColor(Lines.Color);
						}
					}
				}

				LastSelectedOption = BaseOption.None;
				_selectedOptionIndex = -1;

				for (var i = 0; i < Options.Count; i++)
				{
					if (selectedOptionId == Options[i].ID)
					{
						LastSelectedOption = Options[i];
						_selectedOptionIndex = i;

						break;
					}
				}
			}

			base.OnUpdate();
		}
	}
}
