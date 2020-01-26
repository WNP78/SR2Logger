namespace Assets.Scripts.Craft.Parts.Modifiers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using ModApi.Craft.Parts;
    using ModApi.Craft.Parts.Attributes;
    using UnityEngine;

    [Serializable]
    [DesignerPartModifier("Logger")]
    [PartModifierTypeId("SR2Logger.VariableLogger")]
    public class VariableLoggerData : PartModifierData<VariableLoggerScript>
    {
        public const string DefaultHost = "localhost";
        public const int DefaultPort = 2873;

        [SerializeField]
        [DesignerPropertyTextInput(Label = "Hostname", Order = 99, Tooltip = "Host to send UDP packets to.")]
        private string _host = DefaultHost;

        [SerializeField]
        [DesignerPropertyTextInput(Label = "Port", Tooltip = "The UDP port to log on")]
        private string _port = DefaultPort.ToString();

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port
        {
            get
            {
                if (int.TryParse(_port, out int res))
                {
                    return res;
                }

                Debug.LogError($"Invalid port number: \"{_port}\". Using default port {DefaultPort} instead.");
                return DefaultPort;
            }
            set
            {
                _port = value.ToString();
            }
        }

        /// <summary>
        /// Gets the hostname.
        /// </summary>
        /// <value>
        /// The hostname.
        /// </value>
        public string Hostname => _host;
    }
}