using Microsoft.EntityFrameworkCore;
using PAWCP2.Models.Models;
using PAWCP2.Core.Data;

namespace PAWCP2.Core.Repositories
{
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(FoodbankContext context) : base(context) { }

        public async Task<IEnumerable<UserRole>> GetAllWithUsersAndRolesAsync()
        {
            return await _dbSet
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .ToListAsync();
        }
    }
}
