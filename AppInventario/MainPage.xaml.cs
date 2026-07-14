using AppInventario.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AppInventario
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

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
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
