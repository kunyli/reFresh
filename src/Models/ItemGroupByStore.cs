using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reFresh.Models
{
	internal class ItemGroupByStore : List<Item>
	{
		public string StoreName { get; private set; }
		public ItemGroupByStore(string storeName, List<Item> items) : base(items) 
		{
			StoreName = storeName;
		}
		public override string ToString()
		{
			return StoreName;
		}
	}
}
