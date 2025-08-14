using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAWCP2.Models.ViewModels;  // importa el namespace de tus VM

namespace PAWCP2.Mvc.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly IHttpClientFactory _http;
        public UserRolesController(IHttpClientFactory http) => _http = http;

        public async Task<IActionResult> Index()
        {
            var client = _http.CreateClient("api");
            // Aquí debes mapear el resultado a tu ViewModel fuerte (no solo dynamic)
            var data = await client.GetFromJsonAsync<UserRolesCompositeViewModel>("api/userroles");

            // Serializa aquí los UserRoles para JS
            ViewData["UserRolesJson"] = JsonConvert.SerializeObject(data.UserRoles);

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Set(int userId, int roleId, bool assigned)
        {
            var client = _http.CreateClient("api");
            var res = await client.PostAsync($"api/userroles/set?userId={userId}&roleId={roleId}&assigned={assigned}", null);
            return res.IsSuccessStatusCode ? Ok() : BadRequest();
        }
    }
}
