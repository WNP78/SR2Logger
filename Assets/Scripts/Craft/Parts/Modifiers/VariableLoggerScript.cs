namespace Assets.Scripts.Craft.Parts.Modifiers
{
    using Assets.Scripts.Logger;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using ModApi.Craft.Parts;
    using ModApi.Craft.Program;
    using ModApi.GameLoop.Interfaces;
    using UnityEngine;
    using ModApi.GameLoop;

    public class VariableLoggerScript : PartModifierScript<VariableLoggerData>, IFlightFixedUpdate
    {
        #region Static Members (reflection)
        static FieldInfo _processField;

        static VariableLoggerScript()
        {
            _processField = typeof(FlightProgramScript).GetField("_process", BindingFlags.NonPublic | BindingFlags.Instance);
        }
        #endregion Static Members (reflection)



        bool _inited = false;

        FlightProgramScript flightProgramScript;

        LoggerServer server;

        float sampleRate = 0;

        float samplePeriod = 0;

        float timeSinceLast = 0;

        public override void OnActivated()
        {
            if (!_inited)
            {
                // get flight program
                flightProgramScript = PartScript.GetModifier<FlightProgramScript>();
                if (flightProgramScript == null)
                {
                    Debug.LogError("Logger script has no flight program: deactivating");
                    enabled = false;
                    return;
                }

                server = new LoggerServer(Data.Hostname, Data.Port);

                _inited = true;
            }
        }

        private ExpressionResult GetVariable(string name)
        {
            if (!_inited)
            {
                throw new Exception("Not initialised");
            }

            return flightProgramScript.GetGlobalVariable(name);
        }

        public void FlightFixedUpdate(in FlightFrameData frame)
        {
            if (frame.IsPaused || frame.IsWarping || !PartScript.Data.Activated || !_inited) { return; }

            var rate = (float?)GetVariable("LogFrequency")?.NumberValue ?? 0f;
            if (!Mathf.Approximately(rate, sampleRate))
            {
                sampleRate = rate;
                samplePeriod = 1 / rate;
                timeSinceLast = 0f;
            }

            timeSinceLast += (float)frame.DeltaTimeWorld;
            while (timeSinceLast > samplePeriod)
            {
                timeSinceLast -= samplePeriod;
                SendUpdate();
            }
        }

        private void SendUpdate()
        {
            server.ResetState();
            var w = server.Writer;
            Process process = (Process)_processField.GetValue(flightProgramScript);
            foreach (var v in process.GlobalVariables.Variables)
            {
                if (v.Name.StartsWith("log_") && v.Name.Length > 4)
                {
                    server.SendName(v.Name.Substring(4, v.Name.Length - 4));
                    var val = v.Value;
                    switch (val.ExpressionType)
                    {
                        case ExpressionType.Boolean:
                            w.Write((byte)TypeCodes.Boolean);
                            w.Write(val.BoolValue);
                            break;
                        case ExpressionType.Number:
                            w.Write((byte)TypeCodes.Float64);
                            w.Write(val.NumberValue);
                            break;
                        case ExpressionType.Vector:
                            w.Write((byte)TypeCodes.Vector3d);
                            w.Write(val.VectorValue.x);
                            w.Write(val.VectorValue.y);
                            w.Write(val.VectorValue.z);
                            break;
                        default:
                            Debug.LogWarning("Cannot log type: " + val.ExpressionType);
                            w.Write((byte)TypeCodes.Null);
                            break;
                    }
                }
            }
            server.FinishSend();
        }

        public override void FlightEnd()
        {
            server.Dispose();
        }
    }
}