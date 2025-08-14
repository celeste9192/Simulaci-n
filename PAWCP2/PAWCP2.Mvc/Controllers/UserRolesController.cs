using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.ViewModels;
using PAWCP2.Models.Models;

namespace PAWCP2.Mvc.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly IHttpClientFactory _http;
        public UserRolesController(IHttpClientFactory http) => _http = http;

        public async Task<IActionResult> Index()
        {
            var client = _http.CreateClient("api");
            var model = await client.GetFromJsonAsync<UserRolesViewModel>("api/userroles");

            if (model == null) model = new UserRolesViewModel();

            // Asignar el usuario logeado (de sesión o token)
            model.LoggedUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Set(int userId, int roleId, bool assigned)
        {
            var client = _http.CreateClient("api");
            var res = await client.PostAsync(
                $"api/userroles/set?userId={userId}&roleId={roleId}&assigned={assigned}",
                null
            );

            return res.IsSuccessStatusCode ? Ok() : BadRequest();
        }
    }
}

