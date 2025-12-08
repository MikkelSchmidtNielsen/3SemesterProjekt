using Common;
using Common.ResultInterfaces;
using Application.RepositoryInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MySqlServerDbContext _db;

        public UserRepository(MySqlServerDbContext db)
        {
            _db = db;
        }

        // CREATE
        public async Task<IResult<User>> CreateUserAsync(User user)
        {
            try
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Error(user, ex);
            }
        }

        public async Task<IResult<User>> ReadUserByEmailAsync(string email)
        {

            User? user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

            return Result<User>.Success(user);
        }

        public async Task<IResult<User>> UpdateOtpAsync(User user)
        {

        }
    }
}
