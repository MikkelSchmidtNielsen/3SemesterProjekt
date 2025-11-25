using Application.ApplicationDto.Command;
using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Command
{
    public interface IGuestCreateCommand
    {
        public Task<IResult<Guest>> CreateGuestAsync(GuestCreateRequestDto guestCreateDto);
    }
}
