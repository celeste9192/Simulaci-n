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

        public async Task<IEnumerable<FoodItem>> GetByUserIdWithRoleFilterAsync(int userId)
        {
            // Obtenemos el RoleId principal del usuario
            var roleId = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .FirstOrDefaultAsync();

            IQueryable<FoodItem> query = _dbSet.AsNoTracking()
                .Where(f => (f.IsActive ?? false));

            // Filtros según rol
            if (roleId == 1)
            {
                // Admin: todos los FoodItems
            }
            else if (roleId == 2)
            {
                // Manager: ve rol 2 y 3
                query = query.Where(f => f.RoleId == 2 || f.RoleId == 3);
            }
            else if (roleId == 3)
            {
                // Viewer: solo rol 3
                query = query.Where(f => f.RoleId == 3);
            }
            else
            {
                
                return new List<FoodItem>();
            }

            return await query.OrderBy(f => f.Name).ToListAsync();
        }

    }
}
