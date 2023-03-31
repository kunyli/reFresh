using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;

namespace reFresh.Views
{
	public partial class ShoppingListPage : ContentPage
	{
		public ShoppingListPage()
		{
			InitializeComponent();

			BindingContext = new Models.ShoppingList();	
		}

		protected override void OnAppearing()
		{
			((Models.ShoppingList)BindingContext).LoadItems();
		}

		private async void Add_Clicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync(nameof(ShoppingListItem));
		}

		private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (e.Value == true)
			{
				
			}
		}

		private async void itemsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection.Count != 0)
			{
				// Get the item model
				var item = (Models.Item)e.CurrentSelection[0];

				// Should navigate to "ShoppingListItem?ItemId=item.ItemID"
				await Shell.Current.GoToAsync($"{nameof(ShoppingListItem)}?{nameof(ShoppingListItem.ItemId)}={item.ItemID}");

				// Unselect the UI
				itemsCollection.SelectedItem = null;
			}
		}
	}
}