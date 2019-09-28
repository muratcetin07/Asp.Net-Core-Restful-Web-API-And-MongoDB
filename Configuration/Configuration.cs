using Microsoft.Extensions.Configuration;

namespace Configuration
{
    public class Configuration
    {
        public static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConfiguration(string key) => _configuration.GetValue<string>(key);

        public static string GetConfiguration(IConfigurationSection configSection, string key) => configSection.GetValue<string>(key);

        public static IConfigurationSection GetSection(string key) => _configuration.GetSection(key);

        public static string MongoDbConnectionString
        {
            get
            {
                var result = GetConfiguration("MongoDbConnectionString");
                return result;
            }
        }

        public static string MongoDbName
        {
            get
            {
                var result = GetConfiguration("MongoDbName");
                return result;
            }
        }
    }
}
