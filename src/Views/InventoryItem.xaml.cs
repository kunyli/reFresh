using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Maui;
using Newtonsoft.Json;
using Plugin.LocalNotification;

namespace reFresh.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class InventoryItem : ContentPage
{
	public string ItemId
	{
		set { LoadItem(value); }
	}

	public bool AddNotificationButtonEnabled { get; set; }

	public InventoryItem()
	{
		InitializeComponent();

		Guid guid = Guid.NewGuid();
		ItemId = guid.ToString();
		LoadItem(guid.ToString());
	}

	private void AddNotificationButton_Clicked(object sender, EventArgs e) 
	{
		if (BindingContext is Models.Item item)
		{
			item.NotificationDisabled = false;

			DateTime reminderDateTime = item.ExpirationDate.GetValueOrDefault().AddDays(-1);
			reminderDateTime.AddHours(12);

			var request = new NotificationRequest
			{
				NotificationId = 627,
				Title = "Expiration alert",
				Subtitle = "The item " + item.ItemName + " will expire tomorrow!",
				Description = "Expiration alert",
				CategoryType = NotificationCategoryType.Reminder,
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = reminderDateTime,
					RepeatType = NotificationRepeat.No
				}

			};

			item.HasNotification = true;

			string appDataPath = FileSystem.AppDataDirectory;
			var inventoryFile = "Inventory.json";
			var dir = Path.Combine(appDataPath, inventoryFile);

			item.SaveItem(dir);
		}
	}

	private async void SaveButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Models.Item item)
		{
			string appDataPath = FileSystem.AppDataDirectory;
			var inventoryFile = "Inventory.json";
			var dir = Path.Combine(appDataPath, inventoryFile);

			item.SaveItem(dir);
		}
		await Shell.Current.Navigation.PopToRootAsync();
	}

	private async void DeleteButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Models.Item item)
		{
			string appDataPath = FileSystem.AppDataDirectory;
			var inventoryFile = "Inventory.json";
			var dir = Path.Combine(appDataPath, inventoryFile);

			item.DeleteItem(dir);
		}

		await Shell.Current.GoToAsync("..");
	}

	private async void MoveButton_Clicked(object obj, EventArgs e)
	{
		if (BindingContext is Models.Item item)
		{
			item.PurchaseDate = null;
			item.ExpirationDate = null;
			item.Store = "Any";

			string appDataPath = FileSystem.AppDataDirectory;
			var inventoryFile = Path.Combine(appDataPath, "Inventory.json");
			var shoppingListFile = Path.Combine(appDataPath, "ShoppingList.json");

			item.MoveItem(inventoryFile, shoppingListFile);

			await Shell.Current.GoToAsync($"{nameof(ShoppingListItem)}?{nameof(ShoppingListItem.ItemId)}={item.ItemID}");
		}
	}

	private void LoadItem(string ItemID)
	{
		Models.Item itemModel = new Models.Item();
		itemModel.ItemID = ItemID;
		itemModel.ExpirationDate = DateTime.Today;
		itemModel.PurchaseDate = DateTime.Today;


		var dir = Path.Combine(FileSystem.AppDataDirectory, "Inventory.json");

		if (File.Exists(dir))
		{
			string content = File.ReadAllText(dir);
			var items = JsonConvert.DeserializeObject<List<Models.Item>>(content);
			var item = items.Find(x => x.ItemID == ItemID);
			if (item != null)
			{
				itemModel.ItemName = item.ItemName;
				itemModel.Quantity = item.Quantity;
				itemModel.ExpirationDate = item.ExpirationDate;
				itemModel.PurchaseDate = item.PurchaseDate;
				itemModel.NotificationDisabled = item.NotificationDisabled;
				itemModel.HasNotification = item.HasNotification;
			}

		}
		BindingContext = itemModel;
	}
}