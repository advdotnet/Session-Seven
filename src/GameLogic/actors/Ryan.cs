using Microsoft.Xna.Framework;
using SessionSeven.Components;
using STACK;
using STACK.Components;
using STACK.Logging;
using System;
using System.Collections;

namespace SessionSeven.Actors
{

    [Serializable]
    public class Ryan : Entity
    {
        const string ANIMATION_NAME_STARTUSE = "startuse";
        const string ANIMATION_NAME_ENDUSE = "enduse";

        public Inventory Inventory;

        public Ryan(Inventory inventory)
        {
            Inventory = inventory;

            AudioListener
                .Create(this);

            CameraLocked
                .Create(this);

            Transform
                .Create(this)
                .SetPosition(350, 250)
                .SetSpeed(120)
                .SetOrientation(-Vector2.UnitY)
                .SetUpdateZWithPosition(true)
                .SetScale(1.0f);

            Sprite
                .Create(this)
                .SetEnableNormalMap(false)
                .SetImage(content.characters.ryan.sprite_blood, 13, 8, 0);

            SpriteTransformAnimation
                .Create(this)
                .SetSetFrameFn(SetFrame);

            SpriteCustomAnimation
                .Create(this)
                .SetGetFramesAction(GetCustomAnimationFrames);

            SpriteData
                .Create(this)
                .SetOrientationFlip(false)
                .SetOffset(-76, -143);

            Text
                .Create(this)
                .SetFont(content.fonts.pixeloperator_outline_BMF)
                .SetWidth(300)
                .SetConstrain(true)
                .SetConstrainingRectangle(new Rectangle(0, 0, Game.VIRTUAL_WIDTH, Game.VIRTUAL_HEIGHT))
                .SetAlign(Alignment.Bottom);

            Navigation
                .Create(this)
                .SetApplyScale(true)
                .SetApplyColoring(true)
                .SetRestrictPosition(true)
                .SetScale(1.75f);

            Scripts
                .Create(this);

            Lightning
                .Create(this)
                .SetLightPosition(new Vector3(new Vector2(51, 61), 0))
                .SetLightColor(new Vector3(1.25f, 0.6f, 0.6f));

            BloodDropEmitter
                .Create(this);

            Score
                .Create(this);

            RandomCountdown
                .Create(this)
                .SetDuration(7)
                .SetMinUpdates(300)
                .SetMaxUpdates(500);
        }

        void GetCustomAnimationFrames(Transform transform, string animation, Frames frames)
        {
            const byte COLUMNS = 13;
            const byte OFFSET = COLUMNS * 4 + 1;
            const byte TOTALFRAMES = 7;
            var ShiftValue = 0;
            frames.AddRange(OFFSET, TOTALFRAMES);

            switch (transform.Direction4)
            {
                case Directions4.Down: ShiftValue = COLUMNS * 1; break;
                case Directions4.Right: ShiftValue = COLUMNS * 0; break;
                case Directions4.Up: ShiftValue = COLUMNS * 2; break;
                case Directions4.Left: ShiftValue = COLUMNS * 3; frames.Reverse(); break;
            }

            frames.Shift(ShiftValue).AddDelay(3);

            switch (animation)
            {
                case ANIMATION_NAME_STARTUSE:
                    break;
                case ANIMATION_NAME_ENDUSE:
                    frames.Reverse();
                    break;
                default:
                    frames.Clear();
                    break;
            }
        }

        public override void OnNotify<T>(string message, T data)
        {
            if (message == Messages.SceneEnter)
            {
                var Scene = (Location)(object)data;
                if (null != Scene)
                {
                    var Sprite = Scene.Background.Get<Sprite>();
                    if (null != Sprite)
                    {
                        var newWidth = Math.Max((int)Sprite.GetWidth(), Game.VIRTUAL_WIDTH);

                        Get<Text>().SetConstrainingRectangle(new Rectangle(0, 0, newWidth, Game.VIRTUAL_HEIGHT));
                    }
                }
            }

            base.OnNotify(message, data);
        }

        int LastScaledStep = -1;

        Frames FramesTalkingLeft = Frames.CreateRange(0, 4).Shift(7 * 13 + 8);
        Frames FramesTalkingRight = Frames.CreateRange(0, 4).Shift(4 * 13 + 8);
        Frames FramesTalkingUp = Frames.CreateRange(0, 4).Shift(6 * 13 + 8);
        Frames FramesTalkingDown = Frames.CreateRange(0, 4).Shift(5 * 13 + 8);

        private Frames GetTalkingFrames(Directions4 direction)
        {
            switch (direction)
            {
                case Directions4.Down: return FramesTalkingDown;
                case Directions4.Right: return FramesTalkingRight;
                case Directions4.Up: return FramesTalkingUp;
                default: return FramesTalkingLeft;
            }
        }

        private void PlayStepSound()
        {
            //  
            Game.PlaySoundEffect(content.audio._path_ + "step_" + World.Get<Randomizer>().CreateInt(1, 17));
        }

        public void SetWalkingPace(int scale)
        {
            WalkingPace = scale;
        }

        int WalkingPace = 3;

        private int SetFrame(Transform transform, int step, int lastFrame)
        {
            if (transform.State.Has(State.Custom))
            {
                return lastFrame;
            }

            var scaledStep = step / WalkingPace;
            var Row = 0;
            int columns = 13;
            int result;
            bool left = false;
            bool walking = transform.State.Has(State.Walking);
            bool talking = transform.State.Has(State.Talking);

            if (talking && !walking)
            {
                scaledStep = step / 13;

                if (LastScaledStep != scaledStep)
                {
                    LastScaledStep = scaledStep;

                    var TalkingFrames = GetTalkingFrames(transform.Orientation.ToDirection4());

                    return TalkingFrames.GetRandomExcluding(World.Get<Randomizer>(), lastFrame);
                }

                return lastFrame;
            }

            switch (transform.Orientation.ToDirection4())
            {
                case Directions4.Down: Row = 1; break;
                case Directions4.Right: Row = 3; break;
                case Directions4.Up: Row = 2; break;
                case Directions4.Left: Row = 4; left = true; break;
            }

            if (!walking)
            {
                if (!left)
                {
                    result = 1 + (Row - 1) * columns;
                }
                else
                {
                    result = (Row) * columns;
                }

                if (Get<RandomCountdown>().Action)
                {
                    switch (transform.Orientation.ToDirection4())
                    {
                        case Directions4.Down: result = 77; break;
                        case Directions4.Right: result = 64; break;
                        case Directions4.Up: result = 90; break;
                        case Directions4.Left: result = 103; break;
                    }
                }
            }
            else
            {
                result = (scaledStep % (columns - 1)) + ((Row - 1) * (columns - 1)) + 2 + (Row - 1);
            }

            if (left && walking)
            {
                result = columns * 3 + (columns * 4 - result) + 1;
            }

            if (walking && result != lastFrame)
            {
                /*
                 *01..12
                 *13..24
                 *25..36
                 *37..48
                 * */
                if (result == 6 || result == 12 ||
                    result == 17 || result == 21 ||
                    result == 30 || result == 36 ||
                    result == 42 || result == 48)
                {
                    PlayStepSound();
                }

            }

            return result;
        }

        public void Turn(Directions4 direction)
        {
            Get<Transform>().Turn(direction);
        }

        /// <summary>
        /// Makes the player face the given entity based on its Transform's position.
        /// </summary>
        /// <param name="entity"></param>
        public void Turn(Entity entity)
        {
            var Transform = entity.Get<Transform>();
            if (null == Transform)
            {
                Log.WriteLine("Turn to entity <" + entity.ID + ">: no Transform component avaiable.", LogLevel.Warning);
                return;
            }

            var DiffVector = Transform.Position - Get<Transform>().Position;

            Turn(DiffVector.ToDirection4());
        }

        public Script StartScript(IEnumerator script, string name = "")
        {
            return Get<Scripts>().Start(script, name);
        }

        public Script Say(string text, float duration = 0)
        {
            return Get<Scripts>().Say(text, duration);
        }

        public Script StartUse()
        {
            return StartScript(StartUseScript());
        }

        private IEnumerator StartUseScript()
        {
            yield return PlayAnimation(ANIMATION_NAME_STARTUSE, false);
            Get<Transform>().State = State.Custom;
        }

        private IEnumerator UseScript(int delayUpdates = 5)
        {
            yield return StartScript(StartUseScript());
            yield return Delay.Updates(delayUpdates);
            yield return StartScript(StopUseScript());
        }

        private IEnumerator StopUseScript()
        {
            yield return PlayAnimation(ANIMATION_NAME_ENDUSE, false);
        }

        public Script Use(int delayUpdates = 5)
        {
            return StartScript(UseScript(delayUpdates));
        }

        public Script StopUse()
        {
            return StartScript(StopUseScript());
        }

        private Script PlayAnimation(string animation, bool looped = false)
        {
            return Get<Scripts>().PlayAnimation(animation, looped);
        }

        public Script GoTo(int x, int y, Directions8 direction = Directions8.None, Action cb = null)
        {
            return GoTo(new Vector2(x, y), direction, cb);
        }

        public Script GoTo(Vector2 position, Directions8 direction = Directions8.None, Action cb = null)
        {
            return Get<Scripts>().GoTo(position, direction, cb);
        }

        public Script GoTo(Interaction interaction, Action cb = null)
        {
            return Get<Scripts>().GoTo(interaction.Position, interaction.Direction, cb);
        }

        public Script GoTo(Entity entity, Directions8 direction = Directions8.None, Action cb = null)
        {
            var Interaction = entity.Get<Interaction>();
            if (null != Interaction)
            {
                return GoTo(Interaction, cb);
            }

            var Transform = entity.Get<Transform>();
            if (null != Transform)
            {
                return GoTo(Transform.Position, direction, cb);
            }

            Log.WriteLine("GoTo entity <" + entity.ID + ">: no Interaction and no Transform component avaiable.", LogLevel.Warning);

            return Script.None;
        }

        public void Stop()
        {
            Get<Scripts>().Clear();
            Get<Text>().Clear();
            Get<Transform>().State = State.Idle;
            Get<SpriteCustomAnimation>().StopAnimation();
        }

        public override void OnUpdate()
        {
            var NewPosition = Get<Transform>().Position;
            var NewOffsetY = Get<Sprite>().GetHeight() + 50;
            var Text = Get<Text>();

            Text.SetPosition(NewPosition);
            Text.Offset.Y = NewOffsetY;

            var Direction = Get<Transform>().Orientation.ToDirection4();

            if (Directions4.Left == Direction ||
                Directions4.Right == Direction)
            {
                Get<Navigation>().Scale = 1.84f;
            }
            else
            {
                Get<Navigation>().Scale = 1.75f;
            }

            base.OnUpdate();
        }
    }
}
