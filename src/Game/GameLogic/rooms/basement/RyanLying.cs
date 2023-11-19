using Microsoft.Xna.Framework.Audio;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{

	[Serializable]
	public enum RyanLyingState
	{
		EyesClosed,
		EyesOpen,
		SittingEyesOpen,
		SittingEyesClosed
	}

	[Serializable]
	public class RyanLying : Entity
	{
		public RyanLying()
		{
			Sprite
				.Create(this)
				.SetImage(content.rooms.basement.ryanlying, 8);

			Transform
				.Create(this)
				.SetPosition(259, 178)
				.SetScale(1.25f)
				.SetZ(263);

			Scripts
				.Create(this);

			SpriteTransformAnimation
				.Create(this)
				.SetSetFrameFn(SetFrame);

			State = RyanLyingState.EyesClosed;
		}

		public RyanLyingState State { get; set; }

		public SoundEffectInstance PlayWhiningSound()
		{
			return Game.PlaySoundEffect(content.audio.whining);
		}

		private readonly Frames _framesEyesClosed = Frames.Create(1, 2);
		private readonly Frames _framesEyesOpen = Frames.Create(3, 4);
		private readonly Frames _framesSittingEyesOpen = Frames.Create(5);
		private readonly Frames _framesTalking = Frames.Create(5, 6, 7);
		private readonly Frames _sittingEyesClosed = Frames.Create(8);

		private Frames GetAnimationFrames(RyanLyingState lyingState, State state)
		{
			if (state.Has(STACK.Components.State.Talking))
			{
				return _framesTalking;
			}

			switch (lyingState)
			{
				case RyanLyingState.EyesClosed:
					return _framesEyesClosed;
				case RyanLyingState.EyesOpen:
					return _framesEyesOpen;
				case RyanLyingState.SittingEyesOpen:
					return _framesSittingEyesOpen;
				case RyanLyingState.SittingEyesClosed:
					return _sittingEyesClosed;
			}

			return _framesEyesClosed;
		}

		private int GetAnimationDelay(State state)
		{
			if (state.Has(STACK.Components.State.Talking))
			{
				return 8;
			}

			return 37;
		}

		private int SetFrame(Transform transform, int step, int lastFrame)
		{
			var delay = GetAnimationDelay(transform.State);
			var scaledStep = step / delay;
			var frames = GetAnimationFrames(State, transform.State);

			return frames[scaledStep % frames.Count];
		}
	}
}
