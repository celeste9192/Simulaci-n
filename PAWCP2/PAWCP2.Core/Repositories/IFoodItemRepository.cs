using PAWCP2.Models.Models;

namespace PAWCP2.Core.Repositories
{
    public interface IFoodItemRepository : IRepositoryBase<FoodItem>
    {
        // Método adicional específico
        Task<IEnumerable<FoodItem>> GetByRoleIdAsync(int roleId);
    }
}
