namespace App.Graph;

/**
 * Fill initial shop state.
 */
sealed class ShopFiller
{
    /**
     * @param World world world to reduce graph for
     * 
     * @return void
     */
    public ShopFiller(World world)
    {
        var shops = world.getLocationsOfType("shop");
        var graph = world.graph;

        if (world.config<string>("region.shopSupply") == "shuffled")
        {
            foreach (var shop in shops)
            {
                // Potion shop canot be modified at this time.
                if (shop.name == $"Potion Shop:{world.id}")
                {
                    continue;
                }
                var inventory = graph.getTargets(shop).Where(target => target.type == "shopitem");
                foreach (var shop_item in inventory)
                {
                    shop_item.item = null;
                    shop_item.cost = null;
                }
            }
        }

        if (world.config<string>("mode.state") == "inverted")
        {
            // put blue potion in DW shop.
        }
    }

    /**
     * No edge adjustment is necessary with shop inventories.
     */
    public void adjustEdges()
    {
    }
}
