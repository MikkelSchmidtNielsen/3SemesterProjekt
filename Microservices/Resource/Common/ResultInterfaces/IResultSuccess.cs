using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.ResultInterfaces
{
    public interface IResultSuccess<T>
    {
        T OriginalType { get; }
    }
}
