using Microsoft.Extensions.DependencyInjection;

namespace AppInventario
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var shell = Application.Current?.Handler?.MauiContext?.Services.GetService<AppShell>();
            return new Window(shell ?? new AppShell());
        }
    }
}