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
        float _Amplitude = 10;
        public float Amplitude
        {
            get
            {
                return _Amplitude;
            }
            set
            {
                _Amplitude = Math.Max(value, 0);
            }
        }

        public float Speed { get; set; }
        public bool Enabled { get; set; }
        public float UpdateOrder { get; set; }

        int AccelerationPhase = 0;
        int PositionPhase = 0;
        int AmplitudePhase = 0;

        public DroneDisplacement()
        {
            Speed = 150f;
            Enabled = true;
        }

        public void Reset()
        {
            PositionPhase = Entity.World.Get<Randomizer>().CreateInt(71);
            AmplitudePhase = 0;
            AccelerationPhase = Entity.World.Get<Randomizer>().CreateInt(71);
        }

        public Vector2 GetDisplacement()
        {
            float scale = 150f;
            var acceleratedPositionPhase = Math.Sin(AccelerationPhase / scale) * 2;

            var x = Math.Cos(PositionPhase / scale + acceleratedPositionPhase);
            var y = Math.Sin(PositionPhase / scale + acceleratedPositionPhase);

            var ampl = Math.Sin(AmplitudePhase / scale) * Amplitude;

            var Result = new Vector2((float)x, (float)y);
            Result.Normalize();

            return Result * (float)ampl;
        }

        public void Update()
        {
            AccelerationPhase++;
            PositionPhase++;
            AmplitudePhase++;
        }

        public static DroneDisplacement Create(Entity entity)
        {
            return entity.Add<DroneDisplacement>();
        }

        public DroneDisplacement SetAmplitude(float val) { Amplitude = val; return this; }
    }
}
