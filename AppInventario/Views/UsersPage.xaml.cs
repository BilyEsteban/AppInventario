using AppInventario.ViewsModels;

namespace AppInventario.Views;

public partial class UsersPage : ContentPage
{
    private readonly UsersPageViewModel _viewModel;

    public UsersPage(UsersPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadUsersAsync();
    }

    private async void OnAddUserClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("users");
    }

    private async void OnUsersClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("users");
    }
}
