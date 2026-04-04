using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TiquetesApp.Constants;
using TiquetesApp.Models;
using TiquetesApp.Models.ViewModels;

namespace TiquetesApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAsync(RegisterViewModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return (false, ["Este correo ya está registrado."]);

            var usuario = new Usuario
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(usuario, model.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(usuario, Roles.Usuario);

            return (true, []);
        }

        public async Task<(bool Succeeded, string? Token, string? ErrorMessage)> LoginAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
                return (false, null, "Correo o contraseña incorrectos.");

            // Generar JWT
            var usuario = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(usuario!);
            var token = GenerarJwt(usuario!, roles);

            return (true, token, null);
        }

        public async Task LogoutAsync()
            => await _signInManager.SignOutAsync();

        private string GenerarJwt(Usuario usuario, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Email, usuario.Email!),
                new(ClaimTypes.Name, usuario.NombreCompleto),
            };

            // Agregar roles como claims
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}