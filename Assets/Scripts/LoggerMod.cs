namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using ModApi;
    using ModApi.Common;
    using ModApi.Mods;
    using UnityEngine;

    /// <summary>
    /// A singleton object representing this mod that is instantiated and initialize when the mod is loaded.
    /// </summary>
    public class LoggerMod : GameMod
    {
        int _port = -1;

        UdpClient _client;

        LoggerScript _script;

        /// <summary>
        /// Prevents a default instance of the <see cref="LoggerMod"/> class from being created.
        /// </summary>
        private LoggerMod() : base()
        {
            var obj = new GameObject("Logger");
            UnityEngine.Object.DontDestroyOnLoad(obj);
            _script = obj.AddComponent<LoggerScript>();

            DevConsole.DevConsoleService.Instance.RegisterCommand<float>("SetSampleRate", (v) => _script.SetFrequency(v));
        }

        /// <summary>
        /// Called when the port changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The object containing the event data.</param>
        public void OnPortChanged(string val)
        {
            if (int.TryParse(val, out int newPort) && newPort >= 0 && newPort <= 65535)
            {
                if (newPort != _port)
                {
                    _port = newPort;
                    this.StartServer();
                }
            }
            else
            {
                Debug.LogWarning($"Invalid Port: {val}");
                _client?.Close();
                _client = null;
            }
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        private void StartServer()
        {
            _client?.Close();

            _client = new UdpClient(_port);
        }

        internal void SendPacket(byte[] bytes)
        {
            _client.Send(bytes, bytes.Length, "localhost", _port);
        }

        /// <summary>
        /// Sends the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="data">The data.</param>
        public void Send(FieldNames field, object data)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((int)field);
            if (data == null)
            {
                writer.Write((byte)TypeCodes.NullType);
            }
            else if (data is float f)
            {
                writer.Write((byte)TypeCodes.Float32);
                writer.Write(f);
            }
            else if (data is double d)
            {
                writer.Write((byte)TypeCodes.Float64);
                writer.Write(d);
            }
            else if (data is Vector3 v)
            {
                writer.Write((byte)TypeCodes.Vector3);
                writer.Write(v.x);
                writer.Write(v.y);
                writer.Write(v.z);
            }
            else if (data is Vector3d vd)
            {
                writer.Write((byte)TypeCodes.Vector3d);
                writer.Write(vd.x);
                writer.Write(vd.y);
                writer.Write(vd.z);
            }
            else if (data is Vector3i vi)
            {
                writer.Write((byte)TypeCodes.Vector3i);
                writer.Write(vi.x);
                writer.Write(vi.y);
                writer.Write(vi.z);
            }
            else if (data is bool b)
            {
                writer.Write((byte)TypeCodes.Boolean);
                writer.Write(b);
            }
            else if (data is int i)
            {
                writer.Write((byte)TypeCodes.Int32);
                writer.Write(i);
            }
            else if (data is Quaternion q)
            {
                writer.Write((byte)TypeCodes.Quaternion);
                writer.Write(q.x);
                writer.Write(q.y);
                writer.Write(q.z);
                writer.Write(q.w);
            }
            else if (data is Quaterniond qd)
            {
                writer.Write((byte)TypeCodes.Quaterniond);
                writer.Write(qd.x);
                writer.Write(qd.y);
                writer.Write(qd.z);
                writer.Write(qd.w);
            }
            else
            {
                throw new ArgumentException($"Type {data.GetType().Name} not supported.");
            }

            SendPacket(stream.ToArray());
        }

        /// <summary>
        /// Gets the singleton instance of the mod object.
        /// </summary>
        /// <value>The singleton instance of the mod object.</value>
        public static LoggerMod Instance { get; } = GetModInstance<LoggerMod>();
    }

    enum TypeCodes
    {
        Float32,
        Float64,
        Vector3,
        Vector3d,
        Vector3i,
        Boolean,
        Int32,
        Quaternion,
        Quaterniond,
        NullType
    }
}