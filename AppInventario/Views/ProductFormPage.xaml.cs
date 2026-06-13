namespace AppInventario.Views;

public partial class ProductFormPage : ContentPage
{
	public ProductFormPage()
	{
		InitializeComponent();
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("products");
	}

	private async void OnCancelClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("products");
	}
}
