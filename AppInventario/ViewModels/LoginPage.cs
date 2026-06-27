using AppInventario.Models;

namespace AppInventario.ViewsModels
{
    public class LoginPageViewModel
    {
        private readonly IAuthService _authService;

        public LoginPageViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public async Task<AuthResponse> LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Ingresa usuario y contraseña."
                };
            }

            return await _authService.LoginAsync(new LoginRequest
            {
                EmailOrUsername = Email.Trim(),
                Password = Password
            });
        }

        public async Task<AuthResponse> RegisterAsync(string email, string username, string password, string fullName)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Completa correo, usuario y contraseña."
                };
            }

            return await _authService.RegisterAsync(new RegisterRequest
            {
                Email = email.Trim(),
                Username = username.Trim(),
                Password = password,
                FullName = fullName.Trim(),
                Phone = string.Empty
            });
        }
    }
}
