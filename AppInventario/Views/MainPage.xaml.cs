using AppInventario.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AppInventario.Views;

public partial class MainPage : ContentPage
{
    private readonly IAuthService _authService;

    public MainPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Verificar que el usuario esté autenticado
        var token = Preferences.Get("auth_token", string.Empty);
        var isValid = await _authService.ValidateTokenAsync(token);
        
        if (!isValid)
        {
            await Shell.Current.GoToAsync("//login");
            return;
        }

        // Cargar datos del usuario actual
        var currentUser = await _authService.GetCurrentUserAsync();
        // Aquí puedes actualizar la UI con la información del usuario
    }

    private async void OnProductsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("products");
    }

    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("productform");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await _authService.LogoutAsync(0);
        await Shell.Current.GoToAsync("//login");
    }

    private async void OnReportsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Reportes", "Sección de reportes en desarrollo", "OK");
    }

    private async void OnUsersClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("users");
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Configuración", "Sección de configuración en desarrollo", "OK");
    }
}