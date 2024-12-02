using SfirleaAndreiBogdanLab7.Models;

namespace SfirleaAndreiBogdanLab7;

public partial class ProductPage : ContentPage
{
    ShopList sl;
    public ProductPage(ShopList slist)
	{
		InitializeComponent();
        sl = slist;
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var product = (Products)BindingContext;
        await App.Database.SaveProductAsync(product);
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var product = listView.SelectedItem as Products;
        await App.Database.DeleteProductAsync(product);
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }

    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        Products p;
        if (listView.SelectedItem != null)
        {
            p = listView.SelectedItem as Products;
            var lp = new ListProduct()
            {
                ShopListID = sl.ID,
                ProductID = p.ID
            };
            await App.Database.SaveListProductAsync(lp);
            p.ListProducts = new List<ListProduct> { lp };
            await Navigation.PopAsync();
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }
}