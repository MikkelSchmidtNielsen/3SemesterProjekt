using Common.ResultInterfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<IResult<User>> CreateUserAsync(User user);
    }
}
