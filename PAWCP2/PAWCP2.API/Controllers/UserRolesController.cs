using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using PAWCP2.Models.ViewModels;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
=======
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Data;
using PAWCP2.Models.Models;
using PAWCP2.Models.ViewModels;
>>>>>>> c7c388460afefd5cba3188925770c5d6862b2085

namespace PAWCP2.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly IHttpClientFactory _http;
        public UserRolesController(IHttpClientFactory http) => _http = http;

        public async Task<IActionResult> Index()
        {
<<<<<<< HEAD
            var client = _http.CreateClient("api");
            var data = await client.GetFromJsonAsync<UserRolesData>("api/userroles");

            // Convertir datos recibidos a tus ViewModels
            var users = new List<UserViewModel>();
            var roles = new List<RoleViewModel>();
            var map = new List<UserRoleViewModel>();

            if (data != null)
            {
                // Mapea usuarios
                foreach (var u in data.users)
                {
                    users.Add(new UserViewModel
                    {
                        UserId = u.UserId,
                        Username = u.Username,
                        Email = u.Email,
                        FullName = u.FullName
                    });
                }

                // Mapea roles
                foreach (var r in data.roles)
                {
                    roles.Add(new RoleViewModel
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName
                    });
                }

                // Mapea relaciones
                foreach (var ur in data.map)
                {
                    map.Add(new UserRoleViewModel
                    {
                        UserId = ur.UserId,
                        RoleId = ur.RoleId
                    });
                }
            }

            var vm = new UserRolesCompositeViewModel
            {
                Users = users,
                Roles = roles,
                UserRoles = map
            };

            return View(vm);
=======
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
>>>>>>> c7c388460afefd5cba3188925770c5d6862b2085
        }

        [HttpPost]
        public async Task<IActionResult> Set(int userId, int roleId, bool assigned)
        {
<<<<<<< HEAD
            var client = _http.CreateClient("api");
            var res = await client.PostAsync($"api/userroles/set?userId={userId}&roleId={roleId}&assigned={assigned}", null);
            return res.IsSuccessStatusCode ? Ok() : BadRequest();
=======
            var ur = await _ctx.UserRoles.FindAsync(userId, roleId);
            if (assigned && ur == null)
                _ctx.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            if (!assigned && ur != null)
                _ctx.UserRoles.Remove(ur);

            await _ctx.SaveChangesAsync();
            return NoContent();
>>>>>>> c7c388460afefd5cba3188925770c5d6862b2085
        }
    }

    // Clase auxiliar para pasar a la vista
    public class UserRolesData
    {
        public List<UserDto> users { get; set; } = new();
        public List<RoleDto> roles { get; set; } = new();
        public List<UserRoleDto> map { get; set; } = new();
    }

    public class UserDto { public int UserId; public string Username; public string Email; public string? FullName; }
    public class RoleDto { public int RoleId; public string RoleName; }
    public class UserRoleDto { public int UserId; public int RoleId; }

    public class UserRolesCompositeViewModel
    {
        public List<UserViewModel> Users { get; set; } = new();
        public List<RoleViewModel> Roles { get; set; } = new();
        public List<UserRoleViewModel> UserRoles { get; set; } = new();
    }
}

