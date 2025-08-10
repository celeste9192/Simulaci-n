using PAWCP2.Models.Models;
using PAWCP2.Core.Repositories;
using PAWCP2.Core.Manager.Interfaces;

namespace PAWCP2.Core.Manager
{
    public class FoodItemManager : IFoodItemManager
    {
        private readonly IFoodItemRepository _foodItemRepository;

        public FoodItemManager(IFoodItemRepository foodItemRepository)
        {
            _foodItemRepository = foodItemRepository;
        }

        public async Task<IEnumerable<FoodItem>> GetAllAsync()
        {
            return await _foodItemRepository.GetAllAsync();
        }

        public async Task<FoodItem?> GetByIdAsync(int id)
        {
            return await _foodItemRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<FoodItem>> GetByRoleIdAsync(int roleId)
        {
            return await _foodItemRepository.GetByRoleIdAsync(roleId);
        }

        public async Task AddAsync(FoodItem foodItem)
        {
            await _foodItemRepository.AddAsync(foodItem);
            await _foodItemRepository.SaveAsync();
        }

        public async Task UpdateAsync(FoodItem foodItem)
        {
            _foodItemRepository.Update(foodItem);
            await _foodItemRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _foodItemRepository.GetByIdAsync(id);
            if (item != null)
            {
                _foodItemRepository.Delete(item);
                await _foodItemRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<FoodItem>> GetByUserIdWithRoleFilterAsync(int userId)
        {
            return await _foodItemRepository.GetByUserIdWithRoleFilterAsync(userId);
        }

    }
}

