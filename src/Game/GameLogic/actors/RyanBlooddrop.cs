using Microsoft.Xna.Framework;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Actors
{
    [Serializable]
    public class RyanBlooddrop : Entity
    {
        public int TimeToLive { get; set; }
        private Vector2 TargetPosition;
        private byte UpdateCount = 0;
        private byte TargetFrame = 1;

        public RyanBlooddrop(Vector2 position, byte frame, int ttl, string id, float z) : base(id)
        {
            Sprite
                .Create(this)
                .SetTexture(Game.Ego.Get<BloodDropEmitter>().Texture, 16, 1)
                .SetFrame(16);

            SpriteData
                .Create(this)
                .SetColor(Color.White);

            HotspotSprite
                .Create(this)
                .SetPixelPerfect(true)
                .SetCaption(Basement_Res.drop_of_blood);

            Transform
                .Create(this)
                .SetZ(z)
                .SetPosition(position - new Vector2(0, 55));

            Interaction
                .Create(this)
                .SetPosition(position - new Vector2(0, 55))
                .SetGetInteractionsFn(GetInteractions);

            Scripts
                .Create(this);

            TimeToLive = Math.Min(500, ttl);
            UpdateCount = 0;
            TargetFrame = frame;
            TargetPosition = position;

            Visible = true;
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                if (0 == World.Get<Randomizer>().CreateInt(2))
                {
                    yield return Game.Ego.Say(Basement_Res.Im_losing_blood);
                }
                else
                {
                    yield return Game.Ego.Say(Basement_Res.My_hand_is_wounded);
                }

                Game.Ego.Get<BloodDropEmitter>().ResetCommentCounter();
            }
        }

        public override void OnUpdate()
        {
            if (TimeToLive <= 500)
            {
                Get<SpriteData>().Color = new Color(255, 255, 255, TimeToLive / 2);
            }

            if (UpdateCount < 11)
            {
                UpdateCount++;
                var Displacement = GetDisplacement(UpdateCount);

                Get<Transform>().Position = TargetPosition + Displacement;
                Get<Interaction>().Position = Get<Transform>().Position;
            }
            else if (UpdateCount == 11)
            {
                UpdateCount++;
                Get<Sprite>().CurrentFrame = TargetFrame;
            }

            base.OnUpdate();
        }

        private Vector2 GetDisplacement(int updates)
        {
            switch (updates)
            {
                case 0: return new Vector2(0, -50);
                case 1: return new Vector2(0, -45);
                case 2: return new Vector2(0, -40);
                case 3: return new Vector2(0, -35);
                case 4: return new Vector2(0, -30);
                case 5: return new Vector2(0, -25);
                case 6: return new Vector2(0, -20);
                case 7: return new Vector2(0, -15);
                case 8: return new Vector2(0, -10);
                case 9: return new Vector2(0, -5);
                default: return Vector2.Zero;
            }
        }
    }
}
