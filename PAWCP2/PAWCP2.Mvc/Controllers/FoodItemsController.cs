using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.Models;
using PAWCP2.Models.ViewModels;

namespace PAWCP2.Mvc.Controllers
{
    public class FoodItemsController : Controller
    {
        private readonly IHttpClientFactory _http;
        public FoodItemsController(IHttpClientFactory http) => _http = http;

        public class FoodItemFiltersDto
        {
            public List<string> Categories { get; set; } = new();
            public List<string> Brands { get; set; } = new();
            public List<string> Suppliers { get; set; } = new();
        }

        public async Task<IActionResult> Index(
            string? Category,
            string? Brand,
            string? Supplier,
            decimal? PriceMin,
            decimal? PriceMax,
            int? CaloriesMax,
            DateTime? ExpirationDate,
            bool? IsPerishable,
            bool? IsActive)
        {
            var client = _http.CreateClient("api");

            // Traer productos
            var items = await client.GetFromJsonAsync<List<FoodItem>>("api/fooditems") ?? new();

            // Filtrar según parámetros
            if (!string.IsNullOrWhiteSpace(Category)) items = items.Where(x => x.Category == Category).ToList();
            if (!string.IsNullOrWhiteSpace(Brand)) items = items.Where(x => x.Brand == Brand).ToList();
            if (!string.IsNullOrWhiteSpace(Supplier)) items = items.Where(x => x.Supplier == Supplier).ToList();
            if (PriceMin.HasValue) items = items.Where(x => x.Price >= PriceMin.Value).ToList();
            if (PriceMax.HasValue) items = items.Where(x => x.Price <= PriceMax.Value).ToList();
            if (CaloriesMax.HasValue) items = items.Where(x => x.CaloriesPerServing <= CaloriesMax.Value).ToList();
            if (ExpirationDate.HasValue)
                items = items.Where(x => x.ExpirationDate.HasValue && x.ExpirationDate.Value.ToDateTime(TimeOnly.MinValue) >= ExpirationDate.Value).ToList();

            if (IsPerishable.HasValue) items = items.Where(x => x.IsPerishable == IsPerishable.Value).ToList();
            if (IsActive.HasValue) items = items.Where(x => x.IsActive == IsActive.Value).ToList();

            int roleId = GetRoleIdFromTokenCookie(Request.Cookies["fb_access_token"]) ?? 3;

            var vm = items.Select(x => new FoodItemViewModel
            {
                FoodItemId = x.FoodItemId,
                Name = x.Name,
                Category = x.Category,
                Brand = x.Brand,
                Description = x.Description,
                Price = x.Price,
                Unit = x.Unit,
                QuantityInStock = x.QuantityInStock,
                ExpirationDate = x.ExpirationDate,
                IsPerishable = x.IsPerishable,
                CaloriesPerServing = x.CaloriesPerServing,
                Ingredients = x.Ingredients,
                Barcode = x.Barcode,
                Supplier = x.Supplier,
                DateAdded = x.DateAdded,
                IsActive = x.IsActive,
                RoleId = x.RoleId,
                Role = x.Role
            }).ToList();

            // Traer listas de filtros
            var filters = await client.GetFromJsonAsync<FoodItemFiltersDto>("api/fooditems/filters") ?? new FoodItemFiltersDto();

            ViewBag.RoleId = roleId;
            ViewBag.Categories = filters.Categories;
            ViewBag.Brands = filters.Brands;
            ViewBag.Suppliers = filters.Suppliers;

            return View(vm);
        }

        private static int? GetRoleIdFromTokenCookie(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var roleClaim = jwt.Claims.FirstOrDefault(c =>
                    string.Equals(c.Type, "role_id", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(c.Type, "role", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(c.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase));

                if (roleClaim == null) return null;

                if (!int.TryParse(roleClaim.Value, out var roleId))
                {
                    var val = roleClaim.Value.Trim().ToLowerInvariant();
                    return val switch
                    {
                        "admin" => 1,
                        "manager" => 2,
                        "viewer" => 3,
                        _ => (int?)null
                    };
                }

                return roleId;
            }
            catch
            {
                return null;
            }
        }
    }
}

