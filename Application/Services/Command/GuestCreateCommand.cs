using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;

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
            CreatedGuestDto dto = Mapper.Map<CreatedGuestDto>(guestCreateDto);

            IResult<Guest> result = _guestFactory.Create(dto);

            if (result.IsSucces())
            {
                Guest guest = result.GetSuccess().OriginalType;

                result = await _repo.CreateGuestAsync(guest);
            }

            return result;
        }
    }
}
