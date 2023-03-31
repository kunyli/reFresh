namespace reFresh;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(Views.InventoryItem), typeof(Views.InventoryItem));
		Routing.RegisterRoute(nameof(Views.ShoppingListItem), typeof(Views.ShoppingListItem));
		Routing.RegisterRoute(nameof(Views.ShoppingListPage), typeof(Views.ShoppingListPage));
		Routing.RegisterRoute(nameof(Views.InventoryPage), typeof(Views.InventoryPage));
	}
}
