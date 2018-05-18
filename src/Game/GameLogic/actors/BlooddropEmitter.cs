using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using Basement_Res = global::SessionSeven.Properties.Basement_Resources;

namespace SessionSeven.Actors
{

    [Serializable]
    public class BloodDropEmitter : Component, IContent, IUpdate
    {
        List<RyanBlooddrop> Drops = new List<RyanBlooddrop>();
        bool SpawnDrops = true;
        int TotalBloodDrops = 0;
        int BloodDropCounter = 0;
        int CommentCounter = -1;
        int Comments = 0;
        [NonSerialized]
        private Texture2D _Texture;
        public Texture2D Texture { get { return _Texture; } }

        public BloodDropEmitter()
        {
            Drops = new List<RyanBlooddrop>();
            Enabled = true;
        }

        public void Start()
        {
            SpawnDrops = true;
            Enabled = true;
        }

        public void Stop()
        {
            SpawnDrops = false;
        }

        Randomizer Randomizer
        {
            get
            {
                return Entity.World.Get<Randomizer>();
            }
        }

        public bool Enabled { get; set; }
        public float UpdateOrder { get; set; }

        public void LoadContent(ContentLoader content)
        {
            _Texture = content.Load<Texture2D>(SessionSeven.content.rooms.basement.blooddrops);
            foreach (var Drop in Drops)
            {
                Drop.Get<Sprite>().SetTexture(_Texture, 16, 1);
            }
        }

        public void UnloadContent()
        {

        }

        public void Update()
        {
            OnUpdateDrops();
            OnUpdateComments();
        }

        public void ResetCommentCounter()
        {
            CommentCounter = Randomizer.CreateInt(1000, 1500);
        }

        void OnUpdateComments()
        {
            const string COMMENT_BLOOD_SCRIPT = "commentblood";

            if (-1 == CommentCounter && !Game.Ego.Get<Scripts>().HasScript(COMMENT_BLOOD_SCRIPT))
            {
                ResetCommentCounter();
            }

            if (0 == CommentCounter)
            {
                if (Entity.World.Interactive &&
                    Verbs.Walk.Equals(Tree.GUI.Interaction.Scene.SelectedVerb) &&
                    null == Tree.GUI.Interaction.Scene.SelectedPrimary &&
                    !Game.Ego.Get<Scripts>().HasScript(COMMENT_BLOOD_SCRIPT))
                {
                    CommentCounter = -1;
                    Game.Ego.Stop();
                    Game.Ego.StartScript(CommentScript(), COMMENT_BLOOD_SCRIPT);
                }
            }
            else if (CommentCounter > 0 && Entity.World.Interactive)
            {
                CommentCounter--;
            }
        }

        IEnumerator CommentScript()
        {
            Game.Ego.Turn(Directions4.Down);
            Entity.World.Interactive = false;
            var Comment = string.Empty;

            switch (Comments)
            {
                case 0:
                    Comment = Basement_Res.My_hand_is_wounded;
                    break;
                case 1:
                    Comment = Basement_Res.My_hand_is_losing_blood;
                    break;
                case 2:
                    Comment = Basement_Res.My_hand_is_still_losing_blood;
                    break;
                case 3:
                    Comment = Basement_Res.I_need_to_dress_my_hand;
                    break;
                default:
                    Comment = Basement_Res.I_should_have_a_look_at_the_medical_cabinet_over_there_to_dress_my_wound;
                    break;
            }

            if (Game.Ego.Inventory.HasItem<InventoryItems.Bandages>() || Game.Ego.Inventory.HasItem<InventoryItems.BandagesCut>())
            {
                Comment = Basement_Res.I_should_use_those_bandages_to_dress_my_wound;
            }

            yield return Game.Ego.Say(Comment);
            Comments++;
            Entity.World.Interactive = true;
            Tree.GUI.Interaction.Scene.Reset();
        }

        void OnUpdateDrops()
        {
            if (0 == BloodDropCounter && SpawnDrops)
            {
                BloodDropCounter = Randomizer.CreateInt(80, 160);
                SpawnDrop();
            }

            UpdateDrops();
            BloodDropCounter--;
            if (Drops.Count == 0 && !SpawnDrops)
            {
                Enabled = false;
            }
        }

        void SpawnDrop()
        {
            // spawn new  blooddrop
            var frame = (byte)Randomizer.CreateInt(15);
            var ttl = 500 + Randomizer.CreateInt(50, 150);
            var Transform = Get<Transform>();
            var DirectionDisplacment = Vector2.Zero;

            switch (Transform.Direction4)
            {
                case Directions4.Left: DirectionDisplacment = new Vector2(-10, -25 + 9); break;
                case Directions4.Up: DirectionDisplacment = new Vector2(10, -7); break;
                case Directions4.Down: DirectionDisplacment = new Vector2(-23, -7); break;
                case Directions4.Right: DirectionDisplacment = new Vector2(-11, -11 + 9); break;
            }

            // some random displacement
            var RandomDisplacement = new Vector2(Randomizer.CreateInt(-3, 3), Randomizer.CreateInt(-2, 2));
            var position = Transform.Position + DirectionDisplacment + RandomDisplacement;

            var id = typeof(RyanBlooddrop).FullName + "_" + TotalBloodDrops;
            var z = .0f;

            switch (Transform.Direction4)
            {
                case Directions4.Up:
                case Directions4.Down: z = Transform.Z + 1; break;
                case Directions4.Left: z = Transform.Z - 5; break;
                case Directions4.Right: z = Transform.Z + 5; break;
            }

            var bloodDrop = new RyanBlooddrop(position, frame, ttl, id, z);
            Drops.Add(bloodDrop);

            Entity.DrawScene.Push(bloodDrop);
            TotalBloodDrops++;
        }

        void UpdateDrops()
        {
            for (int i = Drops.Count - 1; i >= 0; i--)
            {
                Drops[i].TimeToLive--;
                if (Drops[i].TimeToLive <= 0)
                {
                    Drops[i].UpdateScene.Pop(Drops[i]);
                    Drops.RemoveAt(i);
                }
            }
        }

        public static BloodDropEmitter Create(Entity entity)
        {
            return entity.Add<BloodDropEmitter>();
        }
    }


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
