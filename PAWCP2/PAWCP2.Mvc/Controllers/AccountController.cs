using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.DTOs;

namespace PAWCP2.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _http;
        public AccountController(IHttpClientFactory http) => _http = http;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var client = _http.CreateClient("api");
            var res = await client.PostAsJsonAsync("api/auth/login", req);
            if (!res.IsSuccessStatusCode) return Unauthorized();

            var token = await res.Content.ReadFromJsonAsync<TokenResponse>();
            Response.Cookies.Append("fb_access_token", token!.access_token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = token.expires_at
            });
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
