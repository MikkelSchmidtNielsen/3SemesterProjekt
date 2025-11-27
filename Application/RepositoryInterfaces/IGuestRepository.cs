using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryInterfaces
{
    public interface IGuestRepository
    {
        Task<IResult<Guest>> GetGuestByIdAsync(int id);
        Task<IResult<Guest>> CreateGuestAsync(Guest guest);
        Task<IResult<Guest>> ReadGuestByEmailAsync(string email);
    }
}
