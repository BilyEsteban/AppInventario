using Microsoft.Extensions.Logging;
using System.IO;
using AppInventario.Views;
using AppInventario.ViewsModels;
using AppInventario.Models;

namespace AppInventario
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registrar servicios y vistas
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ProductPage>();
            builder.Services.AddSingleton<ProductFormPage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<UsersPage>();
            builder.Services.AddSingleton<AppShell>();

            // Registrar ViewModels
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<ProductPageViewModel>();
            builder.Services.AddSingleton<ProductFormPageViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<RegisterPageViewModel>();
            builder.Services.AddSingleton<UsersPageViewModel>();

            // Registrar servicios de datos y autenticación
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "inventario.db3");
            builder.Services.AddSingleton<IDatabaseService>(new DatabaseService(dbPath));
            builder.Services.AddSingleton<IAuthService>(new AuthService(dbPath));

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
