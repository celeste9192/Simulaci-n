using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Data;
using System.Security.Claims;

namespace PAWCP2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // cualquier usuario autenticado
    public class FoodItemsController : ControllerBase
    {
        private readonly FoodbankContext _ctx;
        public FoodItemsController(FoodbankContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var roleIds = await _ctx.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            // Items activos, visibles si RoleId es null o está dentro de los roles del usuario
            var items = await _ctx.FoodItems.AsNoTracking()
    .Where(f => (f.IsActive ?? false) && (f.RoleId == null || roleIds.Contains(f.RoleId.Value)))
    .OrderBy(f => f.Name)
    .ToListAsync();

            return Ok(items);
        }
    }
}
