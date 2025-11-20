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
        public IResult<Guest> Create(GuestCreateUserDomainDto guestCreateUserDomainDto)
        {
            try
            {
                Guest guest = new Guest(
                    guestCreateUserDomainDto.FirstName,
                    guestCreateUserDomainDto.LastName,
                    guestCreateUserDomainDto.PhoneNumber,
                    guestCreateUserDomainDto.Email,
                    guestCreateUserDomainDto.Country,
                    guestCreateUserDomainDto.Language,
                    guestCreateUserDomainDto.Address);

                return Result<Guest>.Success(guest);
            }
            catch (Exception ex)
            {
                return Result<Guest>.Error(null, ex!);
            }
        }
    }
}
