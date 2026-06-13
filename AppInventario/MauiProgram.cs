using Microsoft.Extensions.Logging;
using AppInventario.Views;
using AppInventario.ViewsModels;

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
                })
               
                .ConfigureMauiHandlers(handlers =>
                {
                    
                });

            // Registrar servicios y vistas
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ProductPage>();
            builder.Services.AddSingleton<ProductFormPage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<AppShell>();

            // Registrar ViewModels
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<ProductPageViewModel>();
            builder.Services.AddSingleton<ProductFormPageViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
