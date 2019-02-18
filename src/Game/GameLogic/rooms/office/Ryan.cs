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
        readonly Frames FramesTransitionArmsCrossed = Frames.Create(1, 2, 3);
        readonly Frames FramesTransitionArmsRaised = Frames.Create(11, 12, 13, 14);
        readonly Frames FramesTransitionHandsIntertwined = Frames.Create(6, 7);
        readonly Frames FramesTransitionNeutral = Frames.Create(FRAMENEUTRAL);
        readonly Frames FramesTalkingArmsCrossed = Frames.Create(3, 4, 5);
        readonly Frames FramesTalkingArmsRaised = Frames.Create(14, 15, 16);
        readonly Frames FramesTalkingHandsIntertwined = Frames.Create(7, 8, 9, 10);
        readonly Frames FramesTalkingNeutral = Frames.Create(17, 18, 19);
        const int ANIMATIONDELAY = 7;
        const int FRAMENEUTRAL = 17;
        bool WasTalking = false;
        int LastScaledStep = -1;

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
                .SetFrame(FRAMENEUTRAL);

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

        public Script TransitionTo(RyanState state, int delay = ANIMATIONDELAY)
        {
            return Get<Scripts>().Start(TransitionScript(state, delay));
        }

        private Frames GetTransitionFrames(RyanState state)
        {
            switch (state)
            {
                case RyanState.ArmsCrossed:
                    return FramesTransitionArmsCrossed;
                case RyanState.ArmsRaised:
                    return FramesTransitionArmsRaised;
                case RyanState.HandsIntertwined:
                    return FramesTransitionHandsIntertwined;
            }

            return FramesTransitionNeutral;
        }

        private Frames GetTalkingFrames(RyanState state)
        {
            switch (State)
            {
                case RyanState.ArmsCrossed:
                    return FramesTalkingArmsCrossed;
                case RyanState.ArmsRaised:
                    return FramesTalkingArmsRaised;
                case RyanState.HandsIntertwined:
                    return FramesTalkingHandsIntertwined;
            }

            return FramesTalkingNeutral;
        }

        private IEnumerator TransitionScript(RyanState newState, int delay)
        {
            if (State == newState)
            {
                yield break;
            }

            var Sprite = Get<Sprite>();
            var TransitionFrames = GetTransitionFrames(State).Copy();

            TransitionFrames.Reverse();
            TransitionFrames.Add(FRAMENEUTRAL);
            TransitionFrames.AddRange(GetTransitionFrames(newState));

            foreach (var Frame in TransitionFrames)
            {
                if (delay > 0)
                {
                    yield return Delay.Updates(delay);
                }
                Sprite.CurrentFrame = Frame;
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
                if (WasTalking)
                {
                    WasTalking = false;
                    return GetTransitionFrames(State).Last();
                }
                else
                {
                    return lastFrame;
                }
            }

            WasTalking = true;
            var scaledStep = step / 10;

            if (scaledStep != LastScaledStep)
            {
                LastScaledStep = scaledStep;
                var TalkingFrames = GetTalkingFrames(State);

                return TalkingFrames.GetRandomExcluding(World.Get<Randomizer>(), lastFrame);
            }

            return lastFrame;
        }
    }
}
