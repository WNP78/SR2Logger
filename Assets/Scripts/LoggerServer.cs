using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Logger
{
    /// <summary>
    /// A server for logging values to UDP.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class LoggerServer : IDisposable
    {
        /// <summary>
        /// The UDP client.
        /// </summary>
        private UdpClient client;

        /// <summary>
        /// The memory stream for the packet being crafted.
        /// </summary>
        private MemoryStream _packetStream;

        /// <summary>
        /// The binary writer for the packet being crafted.
        /// </summary>
        private BinaryWriter _packetWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerServer"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        public LoggerServer(string host, int port)
        {
            Hostname = host;
            Port = port;
            client = new UdpClient();
            _packetStream = new MemoryStream();
            _packetWriter = new BinaryWriter(_packetStream);
        }

        /// <summary>
        /// Gets or sets the hostname to send packets to.
        /// </summary>
        /// <value>
        /// The hostname.
        /// </value>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        /// Gets the binary writer.
        /// </summary>
        /// <value>
        /// The writer.
        /// </value>
        public BinaryWriter Writer => _packetWriter;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// Also stops server.
        /// </summary>
        public void Dispose()
        {
            client.Close();
            client.Dispose();
        }

        /// <summary>
        /// Sends the raw data as a UDP packet.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="length">The length of bytes to send.</param>
        private void SendData(byte[] bytes, int length)
        {
            client.Send(bytes, length, Hostname, Port);
        }

        /// <summary>
        /// Resets the state.
        /// </summary>
        public void ResetState()
        {
            Array.Clear(_packetStream.GetBuffer(), 0, (int)_packetStream.Length);
            _packetStream.Position = 0;
            _packetStream.SetLength(0);
        }

        /// <summary>
        /// Sends the packet that was crafted.
        /// </summary>
        public void FinishSend()
        {
            SendData(_packetStream.GetBuffer(), (int)_packetStream.Length);
            ResetState();
        }

        /// <summary>
        /// Sends the variable name to the packet buffer.
        /// </summary>
        /// <param name="name">The name.</param>
        public void SendName(string name)
        {
            var bytes = Encoding.ASCII.GetBytes(name);
            _packetWriter.Write((ushort)bytes.Length);
            _packetWriter.Write(bytes);
        }
    }

    enum TypeCodes
    {
        Null,
        Float64,
        Boolean,
        Vector3d
    }
}
