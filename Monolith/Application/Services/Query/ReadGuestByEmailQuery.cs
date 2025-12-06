using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.Services.Query
{
    public class ReadGuestByEmailQuery : IReadGuestByEmailQuery
    {
        private readonly IGuestRepository _guestRepository;

        public ReadGuestByEmailQuery(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public async Task<IResult<Guest>> ReadGuestByEmailAsync(string email)
        {
            IResult<Guest> response = await _guestRepository.ReadGuestByEmailAsync(email);

            if (response.IsSucces())
            {
                // Get success
                Guest guest = response.GetSuccess().OriginalType;

                return Result<Guest>.Success(guest);
            }
            else
            {
                // Get exception
                Exception ex = response.GetError().Exception!;

                return Result<Guest>.Error(null!, ex);
            }
        }
    }
}
