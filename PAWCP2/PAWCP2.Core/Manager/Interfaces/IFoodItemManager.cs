using PAWCP2.Models.Models;

namespace PAWCP2.Core.Manager.Interfaces
{
    public interface IFoodItemManager
    {
        Task<IEnumerable<FoodItem>> GetAllAsync();
        Task<FoodItem?> GetByIdAsync(int id);
        Task<IEnumerable<FoodItem>> GetByRoleIdAsync(int roleId);
        Task AddAsync(FoodItem foodItem);
        Task UpdateAsync(FoodItem foodItem);
        Task DeleteAsync(int id);
    }
}

