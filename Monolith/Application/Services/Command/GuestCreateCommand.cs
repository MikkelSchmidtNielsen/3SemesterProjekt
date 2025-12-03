using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace Application.Services.Command
{
    public class GuestCreateCommand : IGuestCreateCommand
    {
		private readonly IGuestFactory _guestFactory;
        private readonly IGuestRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IUserAuthenticationApiService _userAuthenticationApiService;

		public GuestCreateCommand(IGuestFactory guestFactory, IGuestRepository repo, IUnitOfWork uow, IUserAuthenticationApiService userAuthenticationApiService)
		{
			_guestFactory = guestFactory;
			_repo = repo;
			_uow = uow;
			_userAuthenticationApiService = userAuthenticationApiService;
		}

		public async Task<IResult<Guest>> CreateGuestAsync(GuestCreateRequestDto guestCreateDto)
		{
			// Check via Factory
			CreatedGuestDto dto = Mapper.Map<CreatedGuestDto>(guestCreateDto);

			IResult<Guest> result = await _guestFactory.CreateAsync(dto);

			if (result.IsSucces() is false)
			{
				return result;
			}

			// Creates in Database, but starts transaction, so we can rollback if Api call fails
			_uow.BeginTransaction();

			Guest guest = result.GetSuccess().OriginalType;

			result = await _repo.CreateGuestAsync(guest);

			if (result.IsSucces() is false)
			{
				_uow.Rollback();
				return result;
			}

			// Create a commit if email isn't provided
			if (guest.Email is null)
			{
				_uow.Commit();
				return result;
			}

			// Notify our Api with new User
			IResult<CreateUserByApiReponseDto> apiResult = await _userAuthenticationApiService.RegisterUserAsync(guestCreateDto.Email!);

			if (apiResult.IsSucces() is false)
			{
				_uow.Rollback();

				var apiException = apiResult.GetError().Exception;
				return Result<Guest>.Error(guest, apiException!);
			}

			_uow.Commit();
			return result;
		}
	}
}
