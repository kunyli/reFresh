using System.Collections;
using System.Collections.ObjectModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace reFresh.Models;

internal class ShoppingList
{
    public List<ItemGroupByStore> AllItems { get; private set; } = new List<ItemGroupByStore>();	

    public ShoppingList() =>
        LoadItems();

    public void LoadItems()
    {
        AllItems.Clear();

		string appDataPath = FileSystem.AppDataDirectory;
		string dir = Path.Combine(appDataPath, "ShoppingList.json");

		if (File.Exists(dir))
		{
			string content = File.ReadAllText(dir);
			var items = JsonConvert.DeserializeObject<List<Item>>(content);

			Dictionary<string, List<Item>> itemDict = new Dictionary<string, List<Item>>();
			foreach( var item in items )
			{
				if (!itemDict.ContainsKey(item.Store))
				{
					itemDict.Add(item.Store, new List<Item>());
				}
				itemDict[item.Store].Add(item);
			}

			foreach(var store in itemDict.Keys)
			{
				var itemList = itemDict[store];
				ItemGroupByStore itemGroup = new ItemGroupByStore(store, itemList);
				AllItems.Add(itemGroup);
			}

			itemDict.Clear();

		}
	}
}