using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain;
using Domain.DomainInterfaces;
using Domain.ModelsDto;

namespace Application.Services.Command
{
    public class CreateUserCommandHandler : ICreateUserCommandHandler
    {
        private readonly IUserRepository _repository;
        private readonly IUserFactory _factory;
        private readonly ICreateTokenCommandHandler _tokenHandler;

        public CreateUserCommandHandler(IUserRepository repository, IUserFactory factory, ICreateTokenCommandHandler tokenHandler)
        {
            _repository = repository;
            _factory = factory;
            _tokenHandler = tokenHandler;
        }

        public async Task<IResult<CreateUserResponseDto>> Handle(string input)
        {
            // Creates dto to handle different returns
            CreateUserResponseDto dto = new CreateUserResponseDto();
            dto.Email = input;

            // Find existing user
            // NOT IMPLEMENTET YET

            // Create user by factory
            IResult<User> userResponse = await _factory.Create(dto);

            if (userResponse.IsSuccess() == false)
            {
                // Get exception
                Exception exception = userResponse.GetError().Exception!;

                return Result<CreateUserResponseDto>.Error(dto, exception);
            }

            // Get factory success
            User user = userResponse.GetSuccess().OriginalType;

            // Create user in DB
            IResult<User> repoResponse = await _repository.CreateUserAsync(user);

            if (repoResponse.IsSuccess() == false)
            {
                // Get exception
                Exception exception = repoResponse.GetError().Exception!;

                return Result<CreateUserResponseDto>.Error(dto, exception);
            }

            // Get repo success
            user = repoResponse.GetSuccess().OriginalType;

            // Create token
            IResult<string> token = _tokenHandler.Handle(user);

            // Return end result
            return Result<CreateUserResponseDto>.Success(dto);
        }
    }
}
