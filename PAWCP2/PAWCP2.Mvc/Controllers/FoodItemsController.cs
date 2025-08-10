using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.Models;

namespace PAWCP2.Mvc.Controllers
{
    public class FoodItemsController : Controller
    {
        private readonly IHttpClientFactory _http;
        public FoodItemsController(IHttpClientFactory http) => _http = http;

        public async Task<IActionResult> Index()
        {
            var client = _http.CreateClient("api");
            var items = await client.GetFromJsonAsync<List<FoodItem>>("api/fooditems");
            return View(items ?? new());
        }
    }
}
