using Microsoft.EntityFrameworkCore;
using PAWCP2.Models.Models;
using PAWCP2.Core.Data;

namespace PAWCP2.Core.Repositories
{
    public class FoodItemRepository : RepositoryBase<FoodItem>, IFoodItemRepository
    {
        public FoodItemRepository(FoodbankContext context) : base(context) { }

        public async Task<IEnumerable<FoodItem>> GetByRoleIdAsync(int roleId)
        {
            return await _dbSet
                .Where(fi => fi.RoleId == roleId && fi.IsActive == true)
                .ToListAsync();
        }
    }
}
