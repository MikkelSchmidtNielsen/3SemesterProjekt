using Common;
using Common.ResultInterfaces;
using Domain;
using Domain.DomainInterfaces;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
    public class UserFactory : IUserFactory
    {
        public async Task<IResult<User>> Create(CreateUserResponseDto dto)
        {
            User user = new User(dto.Email);

            return Result<User>.Success(user);
        }
    }
}
