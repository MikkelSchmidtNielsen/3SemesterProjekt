using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.ResultInterfaces
{
    public interface IResult<T>
    {
        bool IsSucces();
        IResultSuccess<T> GetSuccess();
        bool IsError();
        IResultError<T> GetError();
        bool IsConflict();
        IResultConflict<T> GetConflict();
    }
}
