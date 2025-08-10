using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Data;

namespace PAWCP2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class UserRolesController : ControllerBase
    {
        private readonly FoodbankContext _ctx;
        public UserRolesController(FoodbankContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _ctx.Users.AsNoTracking().ToListAsync();
            var roles = await _ctx.Roles.AsNoTracking().ToListAsync();
            var map = await _ctx.UserRoles.AsNoTracking().ToListAsync();
            return Ok(new { users, roles, map });
        }

        [HttpPost("set")]
        public async Task<IActionResult> Set([FromQuery] int userId, [FromQuery] int roleId, [FromQuery] bool assigned)
        {
            var ur = await _ctx.UserRoles.FindAsync(userId, roleId);
            if (assigned && ur == null)
                _ctx.UserRoles.Add(new Models.Models.UserRole { UserId = userId, RoleId = roleId });
            if (!assigned && ur != null)
                _ctx.UserRoles.Remove(ur);

            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
