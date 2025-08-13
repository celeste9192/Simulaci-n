using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Data;
using PAWCP2.Models.Models;
using PAWCP2.Models.ViewModels;

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
            var users = await _ctx.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .ToListAsync();

            var roles = await _ctx.Roles.AsNoTracking().ToListAsync();

            // Mapear usuarios a DTO
            var usersDto = users.Select(u => new UserDto
            {
                Id = u.UserId,
                UserName = u.Username,
                Email = u.Email
            }).ToList();

            // Mapear roles a DTO
            var rolesDto = roles.Select(r => new RoleDto
            {
                Id = r.RoleId,
                Name = r.RoleName
            }).ToList();

            // Mapear UserRoles a DTO
            var mapDto = users
                .SelectMany(u => u.UserRoles.Select(ur => new UserRoleMapDto
                {
                    UserId = ur.UserId,
                    RoleId = ur.RoleId
                }))
                .ToList();

            var vm = new UserRolesViewModel
            {
                Users = usersDto,
                Roles = rolesDto,
                Map = mapDto
            };

            return Ok(vm);
        }

        [HttpPost("set")]
        public async Task<IActionResult> Set([FromQuery] int userId, [FromQuery] int roleId, [FromQuery] bool assigned)
        {
            var ur = await _ctx.UserRoles.FindAsync(userId, roleId);
            if (assigned && ur == null)
                _ctx.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            if (!assigned && ur != null)
                _ctx.UserRoles.Remove(ur);

            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}

