namespace AppInventario.Views;

public partial class ProductPage : ContentPage
{
	public ProductPage()
	{
		InitializeComponent();
	}

	private async void OnNewProductClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("productform");
	}

	private async void OnEditProductClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("productform");
	}

	private async void OnDeleteProductClicked(object sender, EventArgs e)
	{
		bool result = await DisplayAlertAsync("Confirmar", "¿Deseas eliminar este producto?", "Sí", "No");
		if (result)
		{
			await DisplayAlertAsync("Éxito", "Producto eliminado", "OK");
		}
	}
}
