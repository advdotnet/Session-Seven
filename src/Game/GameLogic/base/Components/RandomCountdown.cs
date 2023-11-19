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

		private const int _uninitialized = -1;
		private int _counter = _uninitialized;

		public RandomCountdown()
		{
			Action = false;
			Enabled = true;
		}

		public void Reset()
		{
			_counter = World.Get<Randomizer>().CreateInt(MinUpdates, MaxUpdates) + Duration;
		}

		public void Update()
		{
			if (_uninitialized == _counter)
			{
				Reset();
			}

			Action = _counter < Duration;

			_counter = Math.Max(_counter - 1, _uninitialized);
		}

		[NonSerialized]
		private World _world = null;

		private new World World => _world ?? (_world = (null == Entity) ? Scene.World : Entity.World);

		public static RandomCountdown Create(Entity entity)
		{
			return entity.Add<RandomCountdown>();
		}

		public RandomCountdown SetDuration(int value) { Duration = value; return this; }
		public RandomCountdown SetMinUpdates(int value) { MinUpdates = value; return this; }
		public RandomCountdown SetMaxUpdates(int value) { MaxUpdates = value; return this; }
	}
}
