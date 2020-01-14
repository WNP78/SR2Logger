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
            logger.Send(FieldNames.AltitudeAgl, craft.AltitudeAgl);
            logger.Send(FieldNames.CanWarp, craft.CanWarp);
            logger.Send(FieldNames.Brake, craft.Controls.Brake);
            logger.Send(FieldNames.Pitch, craft.Controls.Pitch);
            logger.Send(FieldNames.Roll, craft.Controls.Roll);
            logger.Send(FieldNames.Slider1, craft.Controls.Slider1);
            logger.Send(FieldNames.Slider2, craft.Controls.Slider2);
            logger.Send(FieldNames.TargetDirection, craft.Controls.TargetDirection);
            logger.Send(FieldNames.TargetHeading, craft.Controls.TargetHeading);
            logger.Send(FieldNames.Throttle, craft.Controls.Throttle);
            logger.Send(FieldNames.TranslateForward, craft.Controls.TranslateForward);
            logger.Send(FieldNames.TranslateRight, craft.Controls.TranslateRight);
            logger.Send(FieldNames.TranslateUp, craft.Controls.TranslateUp);
            logger.Send(FieldNames.TranslationModeEnabled, craft.Controls.TranslationModeEnabled);
            logger.Send(FieldNames.Yaw, craft.Controls.Yaw);
            logger.Send(FieldNames.CraftMass, craft.CraftMass);
            logger.Send(FieldNames.CraftPartCount, craft.CraftPartCount);
            logger.Send(FieldNames.CurrentStage, craft.CraftScript.ActiveCommandPod.CurrentStage);
            logger.Send(FieldNames.NumStages, craft.CraftScript.ActiveCommandPod.NumStages);
            logger.Send(FieldNames.AirDensity, craft.CraftScript.AtmosphereSample.AirDensity);
            logger.Send(FieldNames.AirPressure, craft.CraftScript.AtmosphereSample.AirPressure);
            logger.Send(FieldNames.AtmosphereHeight, craft.CraftScript.AtmosphereSample.AtmosphereHeight);
            logger.Send(FieldNames.SampleAltitude, craft.CraftScript.AtmosphereSample.SampleAltitude);
            logger.Send(FieldNames.SpeedOfSound, craft.CraftScript.AtmosphereSample.SpeedOfSound);
            logger.Send(FieldNames.Temperature, craft.CraftScript.AtmosphereSample.Temperature);
            logger.Send(FieldNames.eulerAngles, craft.CraftScript.CenterOfMass.eulerAngles);
            logger.Send(FieldNames.forward, craft.CraftScript.CenterOfMass.forward);
            logger.Send(FieldNames.localEulerAngles, craft.CraftScript.CenterOfMass.localEulerAngles);
            logger.Send(FieldNames.localPosition, craft.CraftScript.CenterOfMass.localPosition);
            logger.Send(FieldNames.localRotation, craft.CraftScript.CenterOfMass.localRotation);
            logger.Send(FieldNames.localScale, craft.CraftScript.CenterOfMass.localScale);
            logger.Send(FieldNames.position, craft.CraftScript.CenterOfMass.position);
            logger.Send(FieldNames.right, craft.CraftScript.CenterOfMass.right);
            logger.Send(FieldNames.rotation, craft.CraftScript.CenterOfMass.rotation);
            logger.Send(FieldNames.up, craft.CraftScript.CenterOfMass.up);
            logger.Send(FieldNames.CurrentEngineThrust, craft.CraftScript.FlightData.CurrentEngineThrust);
            logger.Send(FieldNames.GravityMagnitude, craft.CraftScript.FlightData.GravityMagnitude);
            logger.Send(FieldNames.MachNumber, craft.CraftScript.FlightData.MachNumber);
            logger.Send(FieldNames.MaxActiveEngineThrust, craft.CraftScript.FlightData.MaxActiveEngineThrust);
            logger.Send(FieldNames.RemainingBattery, craft.CraftScript.FlightData.RemainingBattery);
            logger.Send(FieldNames.RemainingFuelInStage, craft.CraftScript.FlightData.RemainingFuelInStage);
            logger.Send(FieldNames.RemainingMonopropellant, craft.CraftScript.FlightData.RemainingMonopropellant);
            logger.Send(FieldNames.SolarRadiationDirection, craft.CraftScript.FlightData.SolarRadiationDirection);
            logger.Send(FieldNames.SolarRadiationFrameDirection, craft.CraftScript.FlightData.SolarRadiationFrameDirection);
            logger.Send(FieldNames.SolarRadiationIntensity, craft.CraftScript.FlightData.SolarRadiationIntensity);
            logger.Send(FieldNames.SurfaceVelocity, craft.CraftScript.FlightData.SurfaceVelocity);
            logger.Send(FieldNames.SurfaceVelocityMagnitude, craft.CraftScript.FlightData.SurfaceVelocityMagnitude);
            logger.Send(FieldNames.VelocityMagnitude, craft.CraftScript.FlightData.VelocityMagnitude);
            logger.Send(FieldNames.WeightedThrottleResponse, craft.CraftScript.FlightData.WeightedThrottleResponse);
            logger.Send(FieldNames.WeightedThrottleResponseTime, craft.CraftScript.FlightData.WeightedThrottleResponseTime);
            logger.Send(FieldNames.FramePosition, craft.CraftScript.FramePosition);
            logger.Send(FieldNames.GravityForce, craft.CraftScript.GravityForce);
            logger.Send(FieldNames.GravityNormal, craft.CraftScript.GravityNormal);
            logger.Send(FieldNames.AirEfficiency, craft.CraftScript.InletAir.AirEfficiency);
            logger.Send(FieldNames.AvailableAir, craft.CraftScript.InletAir.AvailableAir);
            logger.Send(FieldNames.ReEntryIntensity, craft.CraftScript.ReEntryIntensity);
            logger.Send(FieldNames.Heading, craft.Heading);
            logger.Send(FieldNames.InContactWithPlanet, craft.InContactWithPlanet);
            logger.Send(FieldNames.IsDestroyed, craft.IsDestroyed);
            logger.Send(FieldNames.Position, craft.Position);
            logger.Send(FieldNames.SolarPosition, craft.SolarPosition);
            logger.Send(FieldNames.SolarVelocity, craft.SolarVelocity);
            logger.Send(FieldNames.SphereOfInfluence, craft.SphereOfInfluence);
            logger.Send(FieldNames.SurfacePosition, craft.SurfacePosition);
            logger.Send(FieldNames.SurfaceRotation, craft.SurfaceRotation);

            logger.FinishSend();
        }
    }

    public enum FieldNames
    {
        Velocity,
        Altitude,
        AltitudeAgl,
        CanWarp,
        Brake,
        Pitch,
        Roll,
        Slider1,
        Slider2,
        TargetDirection,
        TargetHeading,
        Throttle,
        TranslateForward,
        TranslateRight,
        TranslateUp,
        TranslationModeEnabled,
        Yaw,
        CraftMass,
        CraftPartCount,
        CurrentStage,
        NumStages,
        AirDensity,
        AirPressure,
        AtmosphereHeight,
        SampleAltitude,
        SpeedOfSound,
        Temperature,
        eulerAngles,
        forward,
        localEulerAngles,
        localPosition,
        localRotation,
        localScale,
        position,
        right,
        rotation,
        up,
        CurrentEngineThrust,
        GravityMagnitude,
        MachNumber,
        MaxActiveEngineThrust,
        RemainingBattery,
        RemainingFuelInStage,
        RemainingMonopropellant,
        SolarRadiationDirection,
        SolarRadiationFrameDirection,
        SolarRadiationIntensity,
        SurfaceVelocity,
        SurfaceVelocityMagnitude,
        VelocityMagnitude,
        WeightedThrottleResponse,
        WeightedThrottleResponseTime,
        FramePosition,
        GravityForce,
        GravityNormal,
        AirEfficiency,
        AvailableAir,
        ReEntryIntensity,
        Heading,
        InContactWithPlanet,
        IsDestroyed,
        Position,
        SolarPosition,
        SolarVelocity,
        SphereOfInfluence,
        SurfacePosition,
        SurfaceRotation
    }
}
