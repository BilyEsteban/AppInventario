using AppInventario.ViewsModels;

namespace AppInventario.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

	private async void OnLoginClicked(object sender, EventArgs e)
	{		
        if (BindingContext is not LoginPageViewModel viewModel)
        {
            return;
        }

        var response = await viewModel.LoginAsync();

        if (response.Success && response.User != null)
        {
            await DisplayAlertAsync("Bienvenido", $"Hola {response.User.DisplayName}", "Continuar");
            await Shell.Current.GoToAsync("//main");
            return;
        }

        await DisplayAlertAsync("Error", response.Message, "Aceptar");
    }

    private async void OnRegisterTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//register");
	}
}
