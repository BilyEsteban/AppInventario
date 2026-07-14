using AppInventario.ViewsModels;

namespace AppInventario.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginPageViewModel _viewModel;

    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(_viewModel.EmailOrUsername))
            {
                await DisplayAlert("Validación", "Por favor, ingresa tu usuario o email", "Aceptar");
                return;
            }

            if (string.IsNullOrWhiteSpace(_viewModel.Password))
            {
                await DisplayAlert("Validación", "Por favor, ingresa tu contraseña", "Aceptar");
                return;
            }

            var response = await _viewModel.LoginAsync();

            if (response.Success && response.User != null)
            {
                // Guardar datos de sesión
                Preferences.Set("auth_token", response.Token);
                Preferences.Set("auth_user_id", response.User.Id.ToString());
                Preferences.Set("auth_user_name", response.User.DisplayName);
                Preferences.Set("auth_user_role", response.User.Role.ToString());

                await DisplayAlert("Bienvenido", $"Hola {response.User.DisplayName}", "Continuar");
                await Shell.Current.GoToAsync("//main");
                return;
            }

            await DisplayAlert("Error", response.Message, "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "Aceptar");
        }
    }

    // Método corregido para navegar a registro
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//register");
    }
}
