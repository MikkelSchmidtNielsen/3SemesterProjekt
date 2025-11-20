using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExternalConfig
{
    public class ConfigurationDictionary
    {
        private static Dictionary<string, string> _configurations = new Dictionary<string, string>();

        public static string GetValue(string key)
        {
            if (!_configurations.TryGetValue(key, out var value))
                throw new KeyNotFoundException($"Configuration key '{key}' not found.");

            return value;
        }

        public static void CreateDictionary(IConfiguration configuration)
        {
            _configurations.Add("connectionString", configuration["ConnectionStrings:Default"]!);
        }
    }
}
