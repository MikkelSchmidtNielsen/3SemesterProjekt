using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ResultInterfaces
{
    public interface IResultConflict<T> : IResultError<T>, IResultSuccess<T>
    {
        T? CurrentType { get; }
    }
}
