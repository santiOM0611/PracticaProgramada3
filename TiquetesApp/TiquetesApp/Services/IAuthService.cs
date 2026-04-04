using TiquetesApp.Models.ViewModels;

namespace TiquetesApp.Services
{
    public interface IAuthService
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAsync(RegisterViewModel model);
        Task<(bool Succeeded, string? Token, string? ErrorMessage)> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}