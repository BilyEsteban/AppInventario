using System.Threading.Tasks;

namespace AppInventario.Models
{
    /// <summary>
    /// Interfaz para el servicio de autenticación
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(int userId);
        Task<User?> GetCurrentUserAsync();
        Task<List<User>> GetAllUsersAsync();
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ValidateTokenAsync(string token);
        JwtClaims? GetClaimsFromToken(string token);
        string GenerateToken(User user);
        string GenerateRefreshToken();
    }
}