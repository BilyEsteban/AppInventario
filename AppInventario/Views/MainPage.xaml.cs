namespace AppInventario.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnProductsClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("products");
	}

	private async void OnAddProductClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("productform");
	}

	private async void OnReportsClicked(object sender, EventArgs e)
	{
		await DisplayAlertAsync("Reportes", "Sección de reportes en desarrollo", "OK");
	}

	private async void OnSettingsClicked(object sender, EventArgs e)
	{
		await DisplayAlertAsync("Configuración", "Sección de configuración en desarrollo", "OK");
	}
}
