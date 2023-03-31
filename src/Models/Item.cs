using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace reFresh.Models;

internal class Item
{
	[JsonProperty("id")]
	public string ItemID { get; set; }
	[JsonProperty("name")]
    public string ItemName { get; set; }
	[JsonProperty("quantity")]
	public string Quantity { get; set; } = string.Empty;
	[JsonProperty("expirationDate")]
    public DateTime? ExpirationDate { get; set; }
	[JsonProperty("purchaseDate")]
	public DateTime? PurchaseDate { get; set; }
	[JsonProperty("store")]
	public string Store { get; set; } = "Any";
	[JsonProperty("notificationDisabled")]
	public bool NotificationDisabled { get; set; } = true;
	[JsonProperty("hasNotification")]
	public bool HasNotification { get; set; } = false;
	public void SaveItem(string FileName)
    {
		var items = new List<Item>();

		if (File.Exists(FileName))
		{
			string content = File.ReadAllText(FileName);
			items = JsonConvert.DeserializeObject<List<Item>>(content);

			Item item = items.Find(x => x.ItemID == ItemID);
			if (item != null)
			{
				items.Remove(item);
			}
		}

		items.Add(this);

		File.WriteAllText(FileName, JsonConvert.SerializeObject(items));
    }

	public void DeleteItem(string FileName)
	{
		var items = new List<Item>();

		if (File.Exists(FileName))
		{
			string content = File.ReadAllText(FileName);
			items = JsonConvert.DeserializeObject<List<Item>>(content);
			Item toRemove = items.Find(x => x.ItemID == this.ItemID);
			items.Remove(toRemove);
			File.WriteAllText(FileName, JsonConvert.SerializeObject(items));
		}
	}

	public void MoveItem(string SourceFileName, string  DestinationFileName)
	{
		this.DeleteItem(SourceFileName);
		this.SaveItem(DestinationFileName);
	}
}