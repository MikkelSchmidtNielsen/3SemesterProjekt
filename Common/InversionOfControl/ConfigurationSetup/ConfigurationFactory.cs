using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.InversionOfControl.ConfigurationSetup
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        private readonly Dictionary<string, string> _configuration = new Dictionary<string, string>();

        public ConfigurationFactory(IConfiguration configuration) 
        {
            
        }

        public string GetConfiguration(string key)
        {
            if (_configuration.TryGetValue(key, out var value) == false)
            {
                throw new KeyNotFoundException($"Configuration key '{key}' not found");
            }

            return value;
        }
    }
}
