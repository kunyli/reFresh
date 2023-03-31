using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Maui;
using Newtonsoft.Json;

namespace reFresh.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class ShoppingListItem : ContentPage
{
	public string ItemId
	{
		set { LoadItem(value); }
	}

	public ShoppingListItem()
	{
		InitializeComponent();

		Guid guid = Guid.NewGuid();
		ItemId = guid.ToString();
		LoadItem(guid.ToString());
	}

	private async void SaveButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Models.Item item)
		{
			string appDataPath = FileSystem.AppDataDirectory;
			var shoppingListFile = "ShoppingList.json";
			var dir = Path.Combine(appDataPath, shoppingListFile);

			item.SaveItem(dir);
		}

		await Shell.Current.GoToAsync("//ShoppingListTab");
	}

	private async void DeleteButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Models.Item item)
		{
			string appDataPath = FileSystem.AppDataDirectory;
			var shoppingListFile = "ShoppingList.json";
			var dir = Path.Combine(appDataPath, shoppingListFile);

			item.DeleteItem(dir);
		}

		await Shell.Current.GoToAsync("..");
	}

	private async void MoveButton_Clicked(object obj, EventArgs e)
	{
		if (BindingContext is Models.Item item)
		{
			item.PurchaseDate = DateTime.Today;

			string appDataPath = FileSystem.AppDataDirectory;

			var inventoryFile = Path.Combine(appDataPath, "Inventory.json");
			var shoppingListFile = Path.Combine(appDataPath, "ShoppingList.json");

			item.MoveItem(shoppingListFile, inventoryFile);

			await Shell.Current.GoToAsync($"{nameof(InventoryItem)}?{nameof(InventoryItem.ItemId)}={item.ItemID}");
		}
	}

	private void LoadItem(string ItemID)
	{
		Models.Item itemModel = new Models.Item();
		itemModel.ItemID = ItemID;

		var dir = Path.Combine(FileSystem.AppDataDirectory, "ShoppingList.json");

		if (File.Exists(dir))
		{
			string content = File.ReadAllText(dir);
			var items = JsonConvert.DeserializeObject<List<Models.Item>>(content);
			var item = items.Find(x => x.ItemID == ItemID);
			if (item != null)
			{
				itemModel.ItemName = item.ItemName;
				itemModel.Quantity = item.Quantity;
				itemModel.Store = item.Store;
			}

		}
		BindingContext = itemModel;
	}
}