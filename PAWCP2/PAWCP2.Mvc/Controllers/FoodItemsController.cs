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

        public async Task<IActionResult> Index()
        {
            var client = _http.CreateClient("api");

            // Traer todos los items desde el API
            var items = await client.GetFromJsonAsync<List<FoodItem>>("api/fooditems") ?? new();

            // Determinar rol del usuario (1=Admin, 2=Manager, 3=Viewer)
            int roleId = GetRoleIdFromTokenCookie(Request.Cookies["fb_access_token"]) ?? 3;

            // Filtrar según rol
            var filtered = FilterByRole(items, roleId);

            // Mapear a ViewModel (ExpirationDate ya es DateOnly? en tu modelo => asignación directa)
            var vm = filtered.Select(x => new FoodItemViewModel
            {
                FoodItemId = x.FoodItemId,
                Name = x.Name,
                Category = x.Category,
                Brand = x.Brand,
                Description = x.Description,
                Price = x.Price,
                Unit = x.Unit,
                QuantityInStock = x.QuantityInStock,
                ExpirationDate = x.ExpirationDate, // <-- SIN conversión
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

            ViewBag.RoleId = roleId;
            return View(vm);
        }

        private static IEnumerable<FoodItem> FilterByRole(IEnumerable<FoodItem> items, int roleId)
        {
            return roleId switch
            {
                1 => items, // Admin: todo
                2 => items.Where(i => i.RoleId == 2 || i.RoleId == 3), // Manager: manager + viewer
                _ => items.Where(i => i.RoleId == 3) // Viewer: solo viewer
            };
        }

        private static int? GetRoleIdFromTokenCookie(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // Busca posibles nombres de claim
                var roleClaim = jwt.Claims.FirstOrDefault(c =>
                    string.Equals(c.Type, "role_id", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(c.Type, "role", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(c.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase));

                if (roleClaim == null) return null;

                // Convertir a int si es posible, si no mapear por nombre
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
