using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Components
{
    /// <summary>
    /// Component that sequentially counts down a random amount of updates.
    /// </summary>
    [Serializable]
    public class RandomCountdown : Component, IUpdate
    {
        /// <summary>
        /// True if the count down finished for the amount of frames given by Duration
        /// </summary>
        public bool Action { get; private set; }
        public int Duration { get; set; }
        public int MinUpdates { get; set; }
        public int MaxUpdates { get; set; }
        public bool Enabled { get; set; }
        public float UpdateOrder { get; set; }
        const int UNINITIALIZED = -1;
        int Counter = UNINITIALIZED;

        public RandomCountdown()
        {
            Action = false;
            Enabled = true;
        }

        public void Reset()
        {
            Counter = World.Get<Randomizer>().CreateInt(MinUpdates, MaxUpdates) + Duration;
        }

        public void Update()
        {
            if (UNINITIALIZED == Counter)
            {
                Reset();
            }

            Action = Counter < Duration;

            Counter = Math.Max(Counter - 1, UNINITIALIZED);
        }

        [NonSerialized]
        World _World = null;
        new World World
        {
            get
            {
                return _World ?? (_World = (null == Entity) ? Scene.World : Entity.World);
            }
        }

        public static RandomCountdown Create(Entity entity)
        {
            return entity.Add<RandomCountdown>();
        }

        public RandomCountdown SetDuration(int value) { Duration = value; return this; }
        public RandomCountdown SetMinUpdates(int value) { MinUpdates = value; return this; }
        public RandomCountdown SetMaxUpdates(int value) { MaxUpdates = value; return this; }
    }
}
