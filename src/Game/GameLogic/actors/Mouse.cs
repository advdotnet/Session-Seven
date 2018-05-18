using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using STACK;
using STACK.Components;
using STACK.Logging;
using System;
using System.Collections;

namespace SessionSeven.Actors
{

    [Serializable]
    public class Mouse : Entity
    {
        public static Vector2 STARTPOSITION = new Vector2(783, 227);
        const string ANIMATION_NAME_SNACK = "snack";

        public Mouse()
        {
            AudioEmitter
                .Create(this);

            Transform
                .Create(this)
                .SetPosition(STARTPOSITION)
                .SetSpeed(120)
                .SetOrientation(-Vector2.UnitX)
                .SetUpdateZWithPosition(true)
                .SetScale(1.0f);

            Sprite
                .Create(this)
                .SetEnableNormalMap(false)
                .SetImage(content.characters.mouse.sprite, 9, 7);

            SpriteTransformAnimation
                .Create(this)
                .SetSetFrameFn(SetFrame);

            SpriteCustomAnimation
                .Create(this)
                .SetGetFramesAction(GetCustomAnimationFrames);

            SpriteData
                .Create(this)
                .SetOrientationFlip(false)
                .SetOffset(-7, -13);

            Navigation
                .Create(this)
                .SetApplyScale(false)
                .SetApplyColoring(false)
                .SetRestrictPosition(true)
                .SetScale(1.0f);

            Scripts
                .Create(this);

            CameraLocked
                .Create(this)
                .SetEnabled(false);

            Visible = false;
            Enabled = false;
        }

        private int SetFrame(Transform transform, int step, int lastFrame)
        {
            if (transform.State.Has(State.Custom))
            {
                return lastFrame;
            }

            var scaledStep = step / 3;
            int result = lastFrame;
            bool walking = transform.State.Has(State.Walking);

            // idle
            if (!walking)
            {
                result = (scaledStep % 9) + 1;
            }
            else
            {
                switch (transform.Orientation.ToDirection4())
                {
                    case Directions4.Down:
                        result = (scaledStep % 6) + 2 * 9 + 1;
                        break;
                    case Directions4.Right:
                        result = -(scaledStep % 6) + 4 * 9 + 1 + 5;
                        break;
                    case Directions4.Up:
                        result = (scaledStep % 6) + 1 * 9 + 1;
                        break;
                    case Directions4.Left:
                        result = (scaledStep % 6) + 3 * 9 + 1;
                        break;
                }
            }

            return result;
        }

        void GetCustomAnimationFrames(Transform transform, string animation, Frames frames)
        {
            const byte COLUMNS = 9;
            const byte OFFSET = COLUMNS * 5 + 1;
            const byte TOTALFRAMES = COLUMNS * 2;

            frames.AddRange(OFFSET, TOTALFRAMES);
            frames.AddDelay(3);

            if (ANIMATION_NAME_SNACK != animation)
            {
                frames.Clear();
            }
        }

        public bool CollectedNuts { get; private set; }

        const string CHECK_FOR_NUTS_SCRIPT_NAME = "CHECKFORNUTSSCRIPT";

        public Script CheckForNuts()
        {
            Enabled = true;
            if (!Get<Scripts>().HasScript(CHECK_FOR_NUTS_SCRIPT_NAME))
            {
                return StartScript(CheckForNutsScript(), CHECK_FOR_NUTS_SCRIPT_NAME);
            }

            return Script.None;
        }

        IEnumerator CheckForNutsScript()
        {
            Get<Transform>().Position = STARTPOSITION;
            Turn(Directions4.Down);

            if (!Tree.InventoryItems.Hazelnuts.SawMouseCollectNuts)
            {
                Game.Ego.Get<CameraLocked>().Enabled = false;
                Get<CameraLocked>().Enabled = true;
            }

            // check if there is a nut in the area the rat can observe
            var NextTargetNut = Tree.Basement.NutsOnFloor.GetNutInWatchArea();

            if (null != NextTargetNut)
            {
                Visible = true;
            }

            while (null != NextTargetNut)
            {
                yield return Delay.Seconds(0.5f);
                var Position = NextTargetNut.Get<Transform>().Position + NextTargetNut.INTERACTIONOFFSET;
                yield return GoTo(Position);
                yield return Delay.Seconds(0.75f);
                CollectedNuts = true;
                Tree.Basement.NutsOnFloor.RemoveNut(NextTargetNut);
                Game.PlaySoundEffect(content.audio.mouse_eat, false, Get<AudioEmitter>(), Game.Ego.Get<AudioListener>());
                yield return PlayAnimation(ANIMATION_NAME_SNACK, false);

                yield return Delay.Seconds(0.75f);

                // check if there are other nuts
                NextTargetNut = Tree.Basement.NutsOnFloor.GetClosestNut(Get<Transform>().Position);

                if (null == NextTargetNut)
                {
                    var GoToMouseHoleScript = Get<Scripts>().Start(GoToMouseHole(), GOTOMOUSEHOLESCRIPTID);
                    while (!GoToMouseHoleScript.Done)
                    {
                        yield return 1;
                        NextTargetNut = Tree.Basement.NutsOnFloor.GetClosestNut(Get<Transform>().Position);
                        if (null != NextTargetNut)
                        {
                            Get<Scripts>().Remove(GOTOMOUSEHOLESCRIPTID);
                            Get<Navigation>().RestrictPosition = true;
                        }
                    }
                }
            }

            yield return Get<Scripts>().Start(GoToMouseHole(), GOTOMOUSEHOLESCRIPTID);

            if (!Tree.InventoryItems.Hazelnuts.SawMouseCollectNuts)
            {
                Game.Ego.Get<CameraLocked>().Enabled = true;
                Get<CameraLocked>().Enabled = false;
            }

            Visible = false;
        }

        const string GOTOMOUSEHOLESCRIPTID = "gotomouseholescriptid";

        private IEnumerator GoToMouseHole()
        {
            if ((Get<Transform>().Position - STARTPOSITION).Length() > 2)
            {
                yield return GoTo(STARTPOSITION + new Vector2(0, 15));
                Get<Navigation>().RestrictPosition = false;
                yield return GoTo(STARTPOSITION);
                Get<Navigation>().RestrictPosition = true;
            }
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
            Get<Transform>().State = State.Idle;
            Get<SpriteCustomAnimation>().StopAnimation();
        }
    }
}
