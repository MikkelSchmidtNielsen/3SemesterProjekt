using Domain;
using Common;
using Common.ResultInterfaces;
using Application.RepositoryInterfaces;

namespace Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlServerDbContext _db;

        public UserRepository(SqlServerDbContext db)
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
    }
}
