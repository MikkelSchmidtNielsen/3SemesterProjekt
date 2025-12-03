using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.Services.Command
{
    public class CreateUserCommandHandler : ICreateUserCommandHandler
    {
        private readonly IUserRepository _repository;
        private readonly ICreateTokenCommandHandler _tokenHandler;

        public CreateUserCommandHandler(IUserRepository repository, ICreateTokenCommandHandler tokenHandler)
        {
            _repository = repository;
            _tokenHandler = tokenHandler;
        }

        public async Task<IResult<CreateUserResponseDto>> HandleAsync(string command)
        {
            // Creates dto to handle different returns
            CreateUserResponseDto dto = new CreateUserResponseDto();
            dto.Email = command;

            // Find existing user
            // NOT IMPLEMENTET YET

            User user;

            try
            {
                user = new User(dto.Email);
            }
            catch (Exception ex)
            {
                return Result<CreateUserResponseDto>.Error(dto, ex);
            }

            // Create user in DB
            IResult<User> repoResponse = await _repository.CreateUserAsync(user);

            if (repoResponse.IsSuccess() == false)
            {
                // Get exception
                Exception exception = repoResponse.GetError().Exception!;

                CreateUserResponseDto createdUser = Mapper.Map<CreateUserResponseDto>(dto);

                return Result<CreateUserResponseDto>.Error(createdUser, exception);
            }

            // Get repo success
            user = repoResponse.GetSuccess().OriginalType;

            // Create token
            IResult<string> tokenResponse = _tokenHandler.Handle(user);

            if (tokenResponse.IsSuccess() == false)
            {
                // Get exception
                Exception exception = tokenResponse.GetError().Exception!;

                CreateUserResponseDto createdUser = Mapper.Map<CreateUserResponseDto>(dto);

                return Result<CreateUserResponseDto>.Error(createdUser, exception);
            }
            else
            {
                // Get token
                string token = tokenResponse.GetSuccess().OriginalType;

                // Add token to dto
                dto.Token = token;

                CreateUserResponseDto createdUser = Mapper.Map<CreateUserResponseDto>(dto);

                // Return end result
                return Result<CreateUserResponseDto>.Success(createdUser);
            }
        }
    }
}
