using AppInventario.Views;
using AppInventario.ViewsModels;
using AppInventario.Models;
using AppInventario.Views;
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
            Routing.RegisterRoute("main", typeof(Views.MainPage));
            Routing.RegisterRoute("products", typeof(Views.ProductPage));
            Routing.RegisterRoute("users", typeof(Views.UsersPage));
            Routing.RegisterRoute("productform", typeof(Views.ProductFormPage));
            Routing.RegisterRoute("login", typeof(Views.LoginPage));
            Routing.RegisterRoute("register", typeof(Views.RegisterPage));

            Navigating += OnShellNavigating;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var token = Preferences.Get("auth_token", string.Empty);
            var isvalid = await _authService.ValidateTokenAsync(token);

            if(!isvalid){

                await Current.GoToAsync("//login");
            }
            else{

                var currentPage = Current.CurrentPage;
                if (currentPage is Views.LoginPage || currentPage is Views.RegisterPage)
                {
                    await Current.GoToAsync("//main");
                }
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
                var token = Preferences.Get("auth_token", string.Empty);
                var isValid = await _authService.ValidateTokenAsync(token);
                
                if (!isValid)
                {
                    e.Cancel();
                    await Current.GoToAsync("//login");
                    return;
                }
            }
        }
    }
}
