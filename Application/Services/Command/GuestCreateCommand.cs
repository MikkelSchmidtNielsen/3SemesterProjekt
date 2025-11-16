using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;

namespace Application.Services.Command
{
    public class GuestCreateCommand : IGuestCreateCommand
    {
        IGuestFactory _guestFactory;
        IGuestRepository _repo;

        public GuestCreateCommand(IGuestFactory guestFactory, IGuestRepository repo)
        {
            _guestFactory = guestFactory;
            _repo = repo;
        }

        public async Task<IResult<Guest>> CreateGuestAsync(GuestCreateDto guestCreateDto)
        {
            IResult<Guest> result = _guestFactory.Create(
                guestCreateDto.FirstName, 
                guestCreateDto.LastName, 
                guestCreateDto.PhoneNumber, 
                guestCreateDto.Email, 
                guestCreateDto.Country,
                guestCreateDto.Language,
                guestCreateDto.Address
            );

            if (result.IsSucces())
            {
                Guest guest = result.GetSuccess().OriginalType;

                result = await _repo.CreateGuestAsync(guest);
            }

            return result;
        }
    }
}
