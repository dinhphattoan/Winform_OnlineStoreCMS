using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess.Repositories
{
    public class UsersRepository : GenericRepository<User>
    {
        public UsersRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
