using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
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

namespace Application.Services.Command
{
    public class GuestCreateUserService : IGuestCreateUserService
    {
        private readonly IGuestFactory _guestFactory;
        private readonly IGuestRepository guestRepository;



        public async Task<IResult<Guest>> GuestCreateUserAsync(GuestCreateUserDto guestCreateUserDto)
        {
            var domainDto = Mapper.Map<GuestCreateUserFactoryDto>(guestCreateUserDto);

            IResult<Guest> guestRequest = _guestFactory.Create(domainDto);

            if (guestRequest.IsSucces() == false)
            {
                return guestRequest;
            }


 


            return Result<Guest>.Error(guestRequest.GetError().OriginalType, new Exception());
        }
    }
}
