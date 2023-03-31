using System.Collections.ObjectModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace reFresh.Models;

internal class Inventory
{
    public ObservableCollection<Item> AllItems { get; set; } = new ObservableCollection<Item>();

    public Inventory() =>
        LoadItems();

    public void LoadItems()
    {
        AllItems.Clear();

		string appDataPath = FileSystem.AppDataDirectory;
		string dir = Path.Combine(appDataPath, "Inventory.json");

		if (File.Exists(dir))
		{
			string content = File.ReadAllText(dir);
			var items = JsonConvert.DeserializeObject<List<Item>>(content);
			items.OrderBy(item => item.ExpirationDate);
			items.Reverse();
			foreach ( var item in items )
			{
				AllItems.Add(item);
			}
		}

	}
}