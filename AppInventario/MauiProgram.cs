using Microsoft.Extensions.Logging;
using AppInventario.ViewsModels;
using AppInventario.Models;
using CommunityToolkit.Maui;

namespace AppInventario
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder

                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
               
                .ConfigureMauiHandlers(handlers =>
                {
                    
                });
            
            // Registrar servicios de datos y autenticación
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "inventario.db3");
            builder.Services.AddSingleton<IDatabaseService>(new DatabaseService(dbPath));
            builder.Services.AddSingleton<IAuthService>(new AuthService(dbPath));

            // Registrar ViewModels
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<ProductPageViewModel>();
            builder.Services.AddTransient<ProductFormPageViewModel>();
            builder.Services.AddTransient<UsersPageViewModel>();

            // Registrar servicios y vistas
            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<Views.RegisterPage>();
            builder.Services.AddTransient<Views.MainPage>();
            builder.Services.AddTransient<Views.ProductPage>();
            builder.Services.AddTransient<Views.ProductFormPage>();
            builder.Services.AddTransient<Views.UsersPage>();

            builder.Services.AddSingleton<AppShell>();



#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
