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

        private Sprite Sprite
        {
            get
            {
                return Get<Sprite>();
            }
        }

        public SoundEffectInstance PlayWhiningSound()
        {
            return Game.PlaySoundEffect(content.audio.whining);
        }

        Frames FramesEyesClosed = Frames.Create(1, 2);
        Frames FramesEyesOpen = Frames.Create(3, 4);
        Frames FramesSittingEyesOpen = Frames.Create(5);
        Frames FramesTalking = Frames.Create(5, 6, 7);
        Frames SittingEyesClosed = Frames.Create(8);

        private Frames GetAnimationFrames(RyanLyingState lyingState, State state)
        {
            if (state.Has(STACK.Components.State.Talking))
            {
                return FramesTalking;
            }

            switch (lyingState)
            {
                case RyanLyingState.EyesClosed:
                    return FramesEyesClosed;
                case RyanLyingState.EyesOpen:
                    return FramesEyesOpen;
                case RyanLyingState.SittingEyesOpen:
                    return FramesSittingEyesOpen;
                case RyanLyingState.SittingEyesClosed:
                    return SittingEyesClosed;
            }

            return FramesEyesClosed;
        }

        private int GetAnimationDelay(RyanLyingState lyingState, State state)
        {
            if (state.Has(STACK.Components.State.Talking))
            {
                return 8;
            }

            return 37;
        }

        private int SetFrame(Transform transform, int step, int lastFrame)
        {
            var Delay = GetAnimationDelay(State, transform.State);
            var ScaledStep = step / Delay;
            var Frames = GetAnimationFrames(State, transform.State);

            return Frames[ScaledStep % Frames.Count];
        }
    }
}
