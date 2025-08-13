using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.DTOs;
using PAWCP2.Models.Models;
using PAWCP2.Models.ViewModels;
using LoginRequest = PAWCP2.Models.DTOs.LoginRequest;
using RegisterRequest = PAWCP2.Models.DTOs.RegisterRequest;

namespace PAWCP2.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _http;
        public AccountController(IHttpClientFactory http) => _http = http;
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var client = _http.CreateClient("api");
            var res = await client.PostAsJsonAsync("api/auth/login", req);
            if (!res.IsSuccessStatusCode) return Unauthorized();

            var loginData = await res.Content.ReadFromJsonAsync<LoginResponseWithRole>();

            // Guardar token como cookie
            Response.Cookies.Append("fb_access_token", loginData!.access_token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = loginData.expires_at
            });

            // Guardar rol en sesión
            HttpContext.Session.SetInt32("RoleId", loginData.role_id);

            return Ok();
        }


        public class LoginResponseWithRole
        {
            public string access_token { get; set; }
            public DateTime expires_at { get; set; }
            public int role_id { get; set; }
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserViewModel req)
        {
            // Completar datos por defecto
            req.IsActive = true;
            req.CreatedAt = DateTime.UtcNow;
            req.LastLogin = null;

            // NO asignar UserRoles aquí, solo los datos del usuario

            var client = _http.CreateClient("api");
            var res = await client.PostAsJsonAsync("api/auth/register", req);

            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("fb_access_token");
            return RedirectToAction("Index", "Home");
        }


    }
}
