using AppInventario.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace AppInventario.ViewsModels
{
    public partial class RegisterPageViewModel : ObservableValidator
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [MinLength(3, ErrorMessage = "Debe tener al menos 3 caracteres.")]
        private string _username = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "Debe tener al menos 6 caracteres.")]
        private string _password = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Debes confirmar la contraseña.")]
        private string _confirmPassword = string.Empty;


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression(@"^\([0-9]{3}\) [0-9]{3}-[0-9]{4}$",
           ErrorMessage = "Formato: (XXX) XXX-XXXX.")]
        private string _phone = string.Empty;

        [ObservableProperty]
        private string _fullName = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasGeneralError))]
        private string? _generalError;

        public bool HasGeneralError => !string.IsNullOrWhiteSpace(GeneralError);

        // Propiedades de error para la UI
        public string? EmailError => GetErrors(nameof(Email)).FirstOrDefault()?.ErrorMessage;
        public bool HasEmailError => GetErrors(nameof(Email)).Any();

        public string? UsernameError => GetErrors(nameof(Username)).FirstOrDefault()?.ErrorMessage;
        public bool HasUsernameError => GetErrors(nameof(Username)).Any();

        public string? PasswordError => GetErrors(nameof(Password)).FirstOrDefault()?.ErrorMessage;
        public bool HasPasswordError => GetErrors(nameof(Password)).Any();

        public string? ConfirmPasswordError => GetErrors(nameof(ConfirmPassword)).FirstOrDefault()?.ErrorMessage;
        public bool HasConfirmPasswordError => GetErrors(nameof(ConfirmPassword)).Any();

        public string? PhoneError => GetErrors(nameof(Phone)).FirstOrDefault()?.ErrorMessage;
        public bool HasPhoneError => GetErrors(nameof(Phone)).Any();

        public RegisterPageViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public async Task RegisterAsync()
        {
            ValidateAllProperties();

            // Notificar cambios para actualizar etiquetas de error
            OnPropertyChanged(nameof(EmailError));
            OnPropertyChanged(nameof(HasEmailError));
            OnPropertyChanged(nameof(UsernameError));
            OnPropertyChanged(nameof(HasUsernameError));
            OnPropertyChanged(nameof(PasswordError));
            OnPropertyChanged(nameof(HasPasswordError));
            OnPropertyChanged(nameof(ConfirmPasswordError));
            OnPropertyChanged(nameof(HasConfirmPasswordError));
            OnPropertyChanged(nameof(PhoneError));
            OnPropertyChanged(nameof(HasPhoneError));

            if (HasErrors) return;

            try
            {
                IsBusy = true;
                GeneralError = null;

                var response = await _authService.RegisterAsync(new RegisterRequest
                {
                    Email = Email.Trim(),
                    Username = Username.Trim(),
                    Password = Password,
                    FullName = FullName.Trim(),
                    Phone = Phone.Trim()
                });

                if (response.Success)
                {
                    Preferences.Set("auth_token", response.Token);
                    Preferences.Set("auth_user_id", response.User?.Id.ToString() ?? string.Empty);

                    // Limpiar formulario tras éxito
                    Email = Username = Password = ConfirmPassword = FullName = Phone = string.Empty;

                    await Shell.Current.GoToAsync("//main");
                }
                else
                {
                    GeneralError = response.Message;
                }
            }
            catch (Exception)
            {
                GeneralError = "Ocurrió un error inesperado al conectar con el servidor.";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
