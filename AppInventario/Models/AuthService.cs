using Microsoft.Maui.Storage;
using System.Security.Cryptography;
using System.Text;

namespace AppInventario.Models
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _context;

        public AuthService(string dbPath)
        {
            _context = new DatabaseContext(dbPath);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrUsername) || string.IsNullOrWhiteSpace(request.Password))
            {
                return CreateResponse(false, "Ingresa tu usuario y contraseña.");
            }

            await EnsureInitializedAsync();

            var user = await _context.AuthenticateAsync(request.EmailOrUsername, request.Password);
            if (user == null || !user.IsActive)
            {
                return CreateResponse(false, "Credenciales inválidas. Usa el usuario admin por defecto o crea una cuenta nueva.");
            }

            user.LastLoginAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveUserAsync(user);

            var token = GenerateToken(user);
            Preferences.Set("auth_token", token);
            Preferences.Set("auth_user_id", user.Id.ToString());

            return new AuthResponse
            {
                Success = true,
                Message = "Inicio de sesión correcto.",
                Token = token,
                User = user,
                ExpiresAt = DateTime.Now.AddDays(7)
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return CreateResponse(false, "Completa al menos correo, usuario y contraseña.");
            }

            await EnsureInitializedAsync();

            var exists = await _context.UserExistsAsync(request.Email, request.Username);
            if (exists)
            {
                return CreateResponse(false, "El correo o nombre de usuario ya existe.");
            }

            var user = new User
            {
                Email = request.Email.Trim(),
                Username = request.Username.Trim(),
                FullName = request.FullName.Trim(),
                Phone = request.Phone.Trim(),
                PasswordHash = DatabaseContext.HashPassword(request.Password),
                Role = UserRole.User,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.SaveUserAsync(user);

            var token = GenerateToken(user);
            Preferences.Set("auth_token", token);
            Preferences.Set("auth_user_id", user.Id.ToString());

            return new AuthResponse
            {
                Success = true,
                Message = "Usuario registrado correctamente.",
                Token = token,
                User = user,
                ExpiresAt = DateTime.Now.AddDays(7)
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            await EnsureInitializedAsync();
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return CreateResponse(false, "Token inválido.");
            }

            var userId = Preferences.Get("auth_user_id", string.Empty);
            if (!int.TryParse(userId, out var id))
            {
                return CreateResponse(false, "Sesión no encontrada.");
            }

            var user = await _context.GetUserByIdAsync(id);
            if (user == null)
            {
                return CreateResponse(false, "Usuario no encontrado.");
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Sesión renovada.",
                Token = GenerateToken(user),
                User = user,
                ExpiresAt = DateTime.Now.AddDays(7)
            };
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            await EnsureInitializedAsync();
            Preferences.Remove("auth_token");
            Preferences.Remove("auth_user_id");
            return true;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            await EnsureInitializedAsync();
            var userId = Preferences.Get("auth_user_id", string.Empty);
            if (!int.TryParse(userId, out var id))
            {
                return null;
            }

            return await _context.GetUserByIdAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            await EnsureInitializedAsync();
            return await _context.GetAllUsersAsync();
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                return false;
            }

            await EnsureInitializedAsync();
            var user = await _context.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            if (!user.PasswordHash.Equals(DatabaseContext.HashPassword(currentPassword), StringComparison.Ordinal))
            {
                return false;
            }

            user.PasswordHash = DatabaseContext.HashPassword(newPassword);
            user.UpdatedAt = DateTime.Now;
            await _context.SaveUserAsync(user);
            return true;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            await EnsureInitializedAsync();
            return !string.IsNullOrWhiteSpace(token);
        }

        public JwtClaims? GetClaimsFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var parts = token.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[1], out var userId))
            {
                return null;
            }

            return new JwtClaims
            {
                UserId = userId,
                IssuedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(7)
            };
        }

        public string GenerateToken(User user)
        {
            return $"token:{user.Id}:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString("N");
        }

        private static AuthResponse CreateResponse(bool success, string message) => new()
        {
            Success = success,
            Message = message
        };

        private async Task EnsureInitializedAsync()
        {
            await _context.InitializeAsync();
        }
    }
}
