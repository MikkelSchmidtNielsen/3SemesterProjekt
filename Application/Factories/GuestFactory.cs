using Application.ApplicationDto.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
    public class GuestFactory : IGuestFactory
    {
        public IResult<Guest> Create(GuestCreateUserFactoryDto guestCreateUserFactoryDto)
        {
            Guest guest = new Guest
                (guestCreateUserFactoryDto.FirstName,
                guestCreateUserFactoryDto.LastName,
                guestCreateUserFactoryDto.PhoneNumber,
                guestCreateUserFactoryDto.Email,
                guestCreateUserFactoryDto.Country,
                guestCreateUserFactoryDto.Language,
                guestCreateUserFactoryDto.Address);

            return Result<Guest>.Success(guest);
        }
    }
}
