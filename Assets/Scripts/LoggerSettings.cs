namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ModApi.Common;
    using ModApi.Settings.Core;

    /// <summary>
    /// The settings for the mod.
    /// </summary>
    /// <seealso cref="SettingsCategory{LoggerSettings}" />
    public class LoggerSettings : SettingsCategory<LoggerSettings>
    {
        /// <summary>
        /// The mod settings instance.
        /// </summary>
        private static LoggerSettings _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerSettings"/> class.
        /// </summary>
        public LoggerSettings() : base("SR2Logger")
        {
        }

        /// <summary>
        /// Gets the mod settings instance.
        /// </summary>
        /// <value>
        /// The mod settings instance.
        /// </value>
        public static LoggerSettings Instance => _instance ?? (_instance = Game.Instance.Settings.ModSettings.GetCategory<LoggerSettings>());

        ///// <summary>
        ///// Gets the TestSetting1 value
        ///// </summary>
        ///// <value>
        ///// The TestSetting1 value.
        ///// </value>
        //public NumericSetting<float> TestSetting1 { get; private set; }

        /// <summary>
        /// The port number.
        /// </summary>
        public StringSetting Port;

        /// <summary>
        /// Initializes the settings in the category.
        /// </summary>
        protected override void InitializeSettings()
        {
            Port = CreateString("Port", "port")
                .SetDescription("The network port to communicate on.")
                .SetDefault("2873")
                .SetApplyType(SettingApplyType.Immediate);
            LoggerMod.Instance.OnPortChanged(Port.Value);
            Port.Changed += (sender, e) => LoggerMod.Instance.OnPortChanged(Port.Value);
            //this.TestSetting1 = this.CreateNumeric<float>("Test Setting 1", 1f, 10f, 1f)
            //    .SetDescription("A test setting that does nothing.")
            //    .SetDisplayFormatter(x => x.ToString("F1"))
            //    .SetDefault(2f);
        }
    }
}