using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.ViewModels;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PAWCP2.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly IHttpClientFactory _http;
        public UserRolesController(IHttpClientFactory http) => _http = http;

        public async Task<IActionResult> Index()
        {
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
        }

        [HttpPost]
        public async Task<IActionResult> Set(int userId, int roleId, bool assigned)
        {
            var client = _http.CreateClient("api");
            var res = await client.PostAsync($"api/userroles/set?userId={userId}&roleId={roleId}&assigned={assigned}", null);
            return res.IsSuccessStatusCode ? Ok() : BadRequest();
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
