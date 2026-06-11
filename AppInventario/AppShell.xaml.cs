using AppInventario.ViewsModels;

namespace AppInventario
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                nameof(ProductFormPage),
                typeof(ProductFormPage));

            Routing.RegisterRoute(
                nameof(LoginPage),
                typeof(LoginPage));
        }

    }
}
