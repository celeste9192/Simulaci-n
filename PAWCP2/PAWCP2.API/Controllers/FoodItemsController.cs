using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using PAWCP2.Core.Manager.Interfaces;

namespace PAWCP2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class FoodItemsController : ControllerBase
    {
        private readonly IFoodItemManager _foodItemManager;

       
        public FoodItemsController(IFoodItemManager foodItemManager)
        {
            _foodItemManager = foodItemManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Obtenemos el UserId desde el token
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Usamos el Manager para aplicar el filtro de rol
            var items = await _foodItemManager.GetByUserIdWithRoleFilterAsync(userId);

            return Ok(items);
        }
    

    [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var items = await _foodItemManager.GetByUserIdWithRoleFilterAsync(userId);

            var filters = new
            {
                Categories = items
                    .Where(x => !string.IsNullOrEmpty(x.Category))
                    .Select(x => x.Category!)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),

                Brands = items
                    .Where(x => !string.IsNullOrEmpty(x.Brand))
                    .Select(x => x.Brand!)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),

                Suppliers = items
                    .Where(x => !string.IsNullOrEmpty(x.Supplier))
                    .Select(x => x.Supplier!)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList()
            };

            return Ok(filters);
        }

    }

}