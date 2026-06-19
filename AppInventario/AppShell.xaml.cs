using AppInventario.Views;
using AppInventario.ViewsModels;

namespace AppInventario
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar rutas de navegación
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("products", typeof(ProductPage));
            Routing.RegisterRoute("productform", typeof(ProductFormPage));
            Routing.RegisterRoute("login", typeof(LoginPage));
        }
    }
}
