using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ResultInterfaces
{
    public interface IResultError<T> : IResultSuccess<T>
    {
        Exception? Exception { get; }
    }
}
