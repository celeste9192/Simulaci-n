using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PAWCP2.Core.Data;
using PAWCP2.Api.Jwt;
using PAWCP2.Models.DTOs;
using PAWCP2.Models.ViewModels;

namespace PAWCP2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly FoodbankContext _ctx;
        private readonly IJwtTokenService _jwt;

        public AuthController(FoodbankContext ctx, IJwtTokenService jwt)
        {
            _ctx = ctx; _jwt = jwt;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email))
                return BadRequest("Email requerido.");

            var user = await _ctx.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == req.Email && u.IsActive);

            if (user == null)
                return Unauthorized();

            var roles = await _ctx.UserRoles
                .Where(ur => ur.UserId == user.UserId)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();

            var token = _jwt.Create(user, roles, out var expires);
            // (Opcional) Actualizar LastLogin
            var tracked = await _ctx.Users.FindAsync(user.UserId);
            if (tracked != null) { tracked.LastLogin = DateTime.UtcNow; await _ctx.SaveChangesAsync(); }

            return new TokenResponse(token, expires);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
                return BadRequest("Email requerido.");

            var existe = await _ctx.Users.AnyAsync(u => u.Email == model.Email && u.IsActive);
            if (existe)
                return BadRequest("El correo ya está registrado.");

            try
            {
                var nuevoUsuario = new Models.Models.User
                {
                    Username = model.Username ?? "",
                    Email = model.Email,
                    FullName = model.FullName,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastLogin = null
                };

                _ctx.Users.Add(nuevoUsuario);
                await _ctx.SaveChangesAsync();

                var userRole = new Models.Models.UserRole
                {
                    UserId = nuevoUsuario.UserId,
                    RoleId = 3
                    // NO asignar Role ni User aquí
                };

                _ctx.UserRoles.Add(userRole);
                await _ctx.SaveChangesAsync();

                return Ok("Usuario registrado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar: {ex.Message}");
            }
        }
    }

    }

