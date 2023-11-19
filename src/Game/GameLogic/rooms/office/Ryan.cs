using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using System.Linq;

namespace SessionSeven.Office
{
	[Serializable]
	public class Ryan : Entity
	{
		private readonly Frames _framesTransitionArmsCrossed = Frames.Create(1, 2, 3);
		private readonly Frames _framesTransitionArmsRaised = Frames.Create(11, 12, 13, 14);
		private readonly Frames _framesTransitionHandsIntertwined = Frames.Create(6, 7);
		private readonly Frames _framesTransitionNeutral = Frames.Create(_frameNeutral);
		private readonly Frames _framesTalkingArmsCrossed = Frames.Create(3, 4, 5);
		private readonly Frames _framesTalkingArmsRaised = Frames.Create(14, 15, 16);
		private readonly Frames _framesTalkingHandsIntertwined = Frames.Create(7, 8, 9, 10);
		private readonly Frames _framesTalkingNeutral = Frames.Create(17, 18, 19);
		private const int _animationDelay = 7;
		private const int _frameNeutral = 17;
		private bool _wasTalking = false;
		private int _lastScaledStep = -1;

		public const int PRIORITY = 5;
		public RyanState State { get; private set; }

		public Ryan()
		{
			Transform
				.Create(this)
				.SetPosition(290, 170)
				.SetAbsolute(true);

			Text
				.Create(this)
				.SetColor(Color.White)
				.SetFont(content.fonts.pixeloperator_outline_BMF)
				.SetWidth(300);

			Sprite
				.Create(this)
				.SetImage(content.rooms.office.ryan, 20, 1, 19)
				.SetFrame(_frameNeutral);

			SpriteTransformAnimation
				.Create(this)
				.SetSetFrameFn(SetFrame);

			SpriteData
				.Create(this)
				.SetOffset(-29, 50);

			Scripts
				.Create(this);

			State = RyanState.Neutral;
			DrawOrder = PRIORITY;
		}

		public Script Say(string text, float duration = 0)
		{
			return Get<Scripts>().Say(text, duration);
		}

		public Script TransitionTo(RyanState state, int delay = _animationDelay)
		{
			return Get<Scripts>().Start(TransitionScript(state, delay));
		}

		private Frames GetTransitionFrames(RyanState state)
		{
			switch (state)
			{
				case RyanState.ArmsCrossed:
					return _framesTransitionArmsCrossed;
				case RyanState.ArmsRaised:
					return _framesTransitionArmsRaised;
				case RyanState.HandsIntertwined:
					return _framesTransitionHandsIntertwined;
			}

			return _framesTransitionNeutral;
		}

		private Frames GetTalkingFrames()
		{
			switch (State)
			{
				case RyanState.ArmsCrossed:
					return _framesTalkingArmsCrossed;
				case RyanState.ArmsRaised:
					return _framesTalkingArmsRaised;
				case RyanState.HandsIntertwined:
					return _framesTalkingHandsIntertwined;
			}

			return _framesTalkingNeutral;
		}

		private IEnumerator TransitionScript(RyanState newState, int delay)
		{
			if (State == newState)
			{
				yield break;
			}

			var sprite = Get<Sprite>();
			var transitionFrames = GetTransitionFrames(State).Copy();

			transitionFrames.Reverse();
			transitionFrames.Add(_frameNeutral);
			transitionFrames.AddRange(GetTransitionFrames(newState));

			foreach (var frame in transitionFrames)
			{
				if (delay > 0)
				{
					yield return Delay.Updates(delay);
				}
				sprite.CurrentFrame = frame;
			}

			State = newState;
			if (delay > 0)
			{
				yield return Delay.Updates(delay);
			}
		}

		private int SetFrame(Transform transform, int step, int lastFrame)
		{
			if (!transform.State.Has(STACK.Components.State.Talking))
			{
				if (_wasTalking)
				{
					_wasTalking = false;
					return GetTransitionFrames(State).Last();
				}
				else
				{
					return lastFrame;
				}
			}

			_wasTalking = true;
			var scaledStep = step / 10;

			if (scaledStep != _lastScaledStep)
			{
				_lastScaledStep = scaledStep;
				var talkingFrames = GetTalkingFrames();

				return talkingFrames.GetRandomExcluding(World.Get<Randomizer>(), lastFrame);
			}

			return lastFrame;
		}
	}
}
