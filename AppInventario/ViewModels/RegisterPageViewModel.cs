using AppInventario.Models;

namespace AppInventario.ViewsModels
{
    public class RegisterPageViewModel
    {
        private readonly IAuthService _authService;
        private readonly RegisterRequest _registerRequest = new RegisterRequest();

        public RegisterPageViewModel(IAuthService authService, RegisterRequest registerRequest)
        {
            _authService = authService;
            _registerRequest = registerRequest;
        }

        public async Task<AuthResponse> RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(_registerRequest.Email) || string.IsNullOrWhiteSpace(_registerRequest.Username) || string.IsNullOrWhiteSpace(_registerRequest.Password))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Completa correo, usuario y contraseña."
                };
            }

            return await _authService.RegisterAsync(new RegisterRequest
            {
                Email = _registerRequest.Email.Trim(),
                Username = _registerRequest.Username.Trim(),
                Password = _registerRequest.Password,
                FullName = _registerRequest.FullName.Trim(),
                Phone = _registerRequest.Phone.Trim()
            });
        }
    }
}
