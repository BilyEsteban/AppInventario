using AppInventario.ViewsModels;
using AppInventario.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AppInventario
{
    public partial class AppShell : Shell
    {
        private readonly IAuthService _authService;

        public AppShell()
        {
            InitializeComponent();

            _authService = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<IAuthService>()
                ?? throw new InvalidOperationException("No auth service available.");

            // Registrar rutas de navegación
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("products", typeof(ProductPage));
            Routing.RegisterRoute("users", typeof(UsersPage));
            Routing.RegisterRoute("productform", typeof(ProductFormPage));
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("register", typeof(RegisterPage));

            Navigating += OnShellNavigating;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                await Shell.Current.GoToAsync("//login");
            }
        }

        private async void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
        {
            var target = e.Target?.Location?.OriginalString?.TrimStart('/') ?? string.Empty;
            if (string.IsNullOrWhiteSpace(target))
            {
                return;
            }

            var route = target.Split('/')[0];
            if (route == "login" || route == "register")
            {
                return;
            }

            var protectedRoutes = new[] { "main", "products", "users", "productform" };
            if (protectedRoutes.Contains(route))
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    e.Cancel();
                    await Shell.Current.GoToAsync("//login");
                }
            }
        }

    }
}
