<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAWCP2.Models.ViewModels;  // importa el namespace de tus VM
=======
﻿using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.ViewModels;
using PAWCP2.Models.Models;
>>>>>>> c7c388460afefd5cba3188925770c5d6862b2085

namespace PAWCP2.Mvc.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly IHttpClientFactory _http;
        public UserRolesController(IHttpClientFactory http) => _http = http;

        public async Task<IActionResult> Index()
        {
            var client = _http.CreateClient("api");
<<<<<<< HEAD
            // Aquí debes mapear el resultado a tu ViewModel fuerte (no solo dynamic)
            var data = await client.GetFromJsonAsync<UserRolesCompositeViewModel>("api/userroles");

            // Serializa aquí los UserRoles para JS
            ViewData["UserRolesJson"] = JsonConvert.SerializeObject(data.UserRoles);

            return View(data);
=======
            var model = await client.GetFromJsonAsync<UserRolesViewModel>("api/userroles");

            if (model == null) model = new UserRolesViewModel();

            // Asignar el usuario logeado (de sesión o token)
            model.LoggedUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            return View(model);
>>>>>>> c7c388460afefd5cba3188925770c5d6862b2085
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

