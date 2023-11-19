using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Components
{
	/// <summary>
	/// Component that calculates a displacement for the drone's position with respect to time.
	/// </summary>
	[Serializable]
	public class DroneDisplacement : Component, IUpdate
	{
		private float _amplitude = 10;
		public float Amplitude
		{
			get => _amplitude;
			set => _amplitude = Math.Max(value, 0);
		}

		public float Speed { get; set; }
		public bool Enabled { get; set; }
		public float UpdateOrder { get; set; }

		private int _accelerationPhase = 0;
		private int _positionPhase = 0;
		private int _amplitudePhase = 0;

		public DroneDisplacement()
		{
			Speed = 150f;
			Enabled = true;
		}

		public void Reset()
		{
			_positionPhase = Entity.World.Get<Randomizer>().CreateInt(71);
			_amplitudePhase = 0;
			_accelerationPhase = Entity.World.Get<Randomizer>().CreateInt(71);
		}

		public Vector2 GetDisplacement()
		{
			var scale = 150f;
			var acceleratedPositionPhase = Math.Sin(_accelerationPhase / scale) * 2;

			var x = Math.Cos((_positionPhase / scale) + acceleratedPositionPhase);
			var y = Math.Sin((_positionPhase / scale) + acceleratedPositionPhase);

			var ampl = Math.Sin(_amplitudePhase / scale) * Amplitude;

			var result = new Vector2((float)x, (float)y);
			result.Normalize();

			return result * (float)ampl;
		}

		public void Update()
		{
			_accelerationPhase++;
			_positionPhase++;
			_amplitudePhase++;
		}

		public static DroneDisplacement Create(Entity entity)
		{
			return entity.Add<DroneDisplacement>();
		}

		public DroneDisplacement SetAmplitude(float val) { Amplitude = val; return this; }
	}
}
