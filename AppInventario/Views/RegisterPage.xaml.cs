using AppInventario.ViewsModels;

namespace AppInventario.Views;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterPageViewModel _viewModel;
    public RegisterPage(RegisterPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            var response = await _viewModel.RegisterAsync();

            if (response.Success)
            {                
                await DisplayAlertAsync("Registro exitoso", response.Message, "Aceptar");
            }

            await DisplayAlertAsync("Error de registro", response.Message, "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Ocurrió un error: {ex.Message}", "Aceptar");
        }
    }


    private async void OnLoginTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }
}
