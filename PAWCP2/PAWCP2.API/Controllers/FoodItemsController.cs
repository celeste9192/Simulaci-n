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
    }
}

