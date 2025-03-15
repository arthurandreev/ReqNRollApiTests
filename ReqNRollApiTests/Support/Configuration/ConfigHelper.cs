using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace SpecflowApiTests.Support.Configuration
{
    public static class ConfigHelper
    {
        private static IConfigurationRoot _config;

        static ConfigHelper()
        {
            var builder = new ConfigurationBuilder();
            builder.Sources.Add(new JsonConfigurationSource { Path = "reqnroll.json" });

            _config = builder.Build();
        }

        public static string GetBaseUrl() => _config["ApiSettings:BaseUrl"] ?? string.Empty;
        public static int GetTimeout() => int.TryParse(_config["ApiSettings:Timeout"], out int timeout) ? timeout : 30;
        public static string GetAuthToken() => _config["ApiSettings:AuthToken"] ?? string.Empty;
    }
}
