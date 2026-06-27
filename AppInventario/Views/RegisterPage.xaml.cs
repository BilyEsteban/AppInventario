using AppInventario.ViewsModels;

namespace AppInventario.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (BindingContext is not RegisterPageViewModel viewModel)
        {
            return;
        }

        var response = await viewModel.RegisterAsync();
        if (response.Success)
        {
            await DisplayAlertAsync("Registro correcto", response.Message, "Aceptar");
            await Shell.Current.GoToAsync("//main");
            return;
        }

        await DisplayAlertAsync("Error", response.Message, "Aceptar");
    }

    private async void OnLoginTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }
}
