using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.InversionOfControl.ConfigurationSetup
{
    public interface IConfigurationFactory
    {
        string GetConfiguration(string key);
    }
}
