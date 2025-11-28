using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExternalConfig
{
    public interface IConfigurationDictionary
    {
        string GetValue(string key);
    }
}
