using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Query
{
    public interface IGuestIdQuery
    {
        Task<IResult<Guest>> GetGuestByIdAsync(int id);
    }
}
