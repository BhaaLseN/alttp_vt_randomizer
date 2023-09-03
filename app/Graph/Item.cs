namespace App.Graph;

/**
 * An Item is any collectable thing in game.
 */
sealed class Item
{
    public readonly byte[] bytes;
    public readonly string raw_name;
    public readonly string name;
    public readonly string nice_name;
    public readonly string i18n_name;
    public readonly int world_id;
    public bool meta = false;
    private static readonly Dictionary<int, Dictionary<string, Item>> items = new();
    private static Dictionary<string, byte[]> raw_items = null!;

    /**
     * Get the Item by name
     *
     * @param string name Name of Item
     * @param int world_id World item belongs to
     */
    public static Item get(string name, int world_id)
    {
        var itemsForWorld = all(world_id);
        string world_name = name + ":" + world_id;

        if (itemsForWorld.TryGetValue(world_name, out var matchingItem))
        {
            return matchingItem;
        }

        // allow made up items
        var item = new Item(name, new byte[0], world_id);
        item.meta = true;
        itemsForWorld.Add(item.name, item);

        return item;
    }

    /**
     * Get the all known Items.
     *
     * @param int world_id World item belongs to
     */
    public static Dictionary<string, Item> all(int world_id)
    {
        if (items.TryGetValue(world_id, out var itemsForWorld))
        {
            return itemsForWorld;
        }

        items.Add(world_id, itemsForWorld = new());

        raw_items ??= YamlReader.LoadItems();

        foreach (var(item_name, item_data) in raw_items) {
            itemsForWorld.Add($"{item_name}:{world_id}", new Item(item_name, item_data/*["bytes"]*/, world_id));
        }


        return all(world_id);
    }

    /**
     * Create a new Item.
     *
     * @param string name Unique name of item
     * @param int[]|null[] bytes data to write to Location addresses
     * @param int world_id world for which the item belongs
     *
     * @return void
     */
    public Item(string name, byte[] bytes, int world_id)
    {
        this.raw_name = name;
        this.name = name + ":" + world_id; ;
        this.i18n_name = "item." + name;
        string? formatted = __(this.i18n_name);
        this.nice_name = formatted ?? "";
        this.bytes = bytes;
        this.world_id = world_id;
    }
    private static string? __(string name) => null; // TODO: this should call localization and return the translated name

    /**
     * serialized version of Item.
     *
     * @return string
     */
    public override string ToString()
    {
        return this.name;
    }
}
