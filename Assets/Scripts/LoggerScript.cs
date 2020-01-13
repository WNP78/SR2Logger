namespace Assets.Scripts
{
    using UnityEngine;
    using ModApi.GameLoop;
    using ModApi.GameLoop.Interfaces;

    class LoggerScript : MonoBehaviourBase, IFlightFixedUpdate
    {
        float _time;

        float _period;

        public void SetFrequency(float freq)
        {
            if (freq <= 0)
            {
                OnDisable();
                return;
            }

            OnEnable();
            _time = 0;
            _period = 1f / freq;
        }

        public void FlightFixedUpdate(in FlightFrameData frame)
        {
            _time += frame.DeltaTime;
            while (_time > _period)
            {
                SendUpdate(frame.FlightScene.CraftNode);
                _time -= _period;
            }
        }

        private void SendUpdate(ModApi.Craft.ICraftNode craft)
        {
            var logger = LoggerMod.Instance;
            logger.Send(FieldNames.Velocity, craft.Velocity);
            logger.Send(FieldNames.Altitude, craft.Altitude);
        }
    }

    public enum FieldNames
    {
        Velocity,
        Altitude
    }
}
