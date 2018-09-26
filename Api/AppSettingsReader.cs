using System;
using System.Configuration;

namespace DavidLievrouw.Api
{
    public class AppSettingsReader
    {
        private readonly Configuration _configuration;

        public AppSettingsReader(Configuration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string ReadAppSetting(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var setting = _configuration.AppSettings.Settings[key];
            return setting?.Value;
        }


        public ConnectionStringSettings ReadConnectionString(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var setting = _configuration.ConnectionStrings.ConnectionStrings[name];
            return setting;
        }
    }
}