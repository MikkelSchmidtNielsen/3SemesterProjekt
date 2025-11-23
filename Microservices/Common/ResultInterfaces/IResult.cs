using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ResultInterfaces
{
    public interface IResult<T>
    {
        bool IsSuccess();
        IResultSuccess<T> GetSuccess();
        bool IsError();
        IResultError<T> GetError();
        bool IsConflict();
        IResultConflict<T> GetConflict();
    }
}
