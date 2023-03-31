using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace reFresh.Views;

public partial class InventoryPage : ContentPage
{
	public InventoryPage()
	{
		InitializeComponent();
		BindingContext = new Models.Inventory();
	}

	protected override void OnAppearing()
	{
		((Models.Inventory)BindingContext).LoadItems();
	}

	private async void Add_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(InventoryItem));
	}

	private async void itemsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.Count != 0)
		{
			// Get the item model
			var item = (Models.Item)e.CurrentSelection[0];

			// Should navigate to "InventoryItem?ItemId=item.ItemID"
			await Shell.Current.GoToAsync($"{nameof(InventoryItem)}?{nameof(InventoryItem.ItemId)}={item.ItemID}");

			// Unselect the UI
			itemsCollection.SelectedItem = null;
		}
	}
}