using System.Collections.Concurrent;

namespace App.Graph;

/**
 * Representation of Players inventory for graph based traversal.
 *
 * @immutable
 */
sealed class Inventory
{
    private readonly ConcurrentDictionary<string, int> item_count = new();
    /** @var array<float> */
    private readonly ConcurrentDictionary<int, float> health = new();

    /**
     * Create new Inventory instance.
     *
     * @param iterable<Item|string> items items to add
     *
     * @return void
     */
    public Inventory(params object[] items)
    {
        foreach (var item in items)
        {
            this.addItemByName(item is Item i ? i.name : (string)item);
        }
    }
    private Inventory(Inventory other)
    {
        item_count = new(other.item_count);
        health = new(other.health);
    }

    /**
     * Add item to inventory.
     *
     * @param Item|string item item to add
     */
    public Inventory addItem(object item)
    {
        var newInventory = new Inventory(this);
        newInventory.addItemByName(item is Item i ? i.name : (string)item);

        return newInventory;
    }

    /**
     * Add an item to this by name.
     *
     * @param string item_name name of item
     */
    private void addItemByName(string item_name)
    {
        int newCount = this.item_count.AddOrUpdate(item_name, 1, (_, v) => v + 1);

        if (item_name.StartsWith("HeartContainer"))
        {
            string[] parts = item_name.Split(':');
            if (!int.TryParse(parts.Skip(1).FirstOrDefault(), out int world_id))
                world_id = 0;
            this.health.AddOrUpdate(world_id, 1, (_, v) => v + 1);
        }
        else if (item_name.StartsWith("PieceOfHeart"))
        {
            string[] parts = item_name.Split(':');
            if (!int.TryParse(parts.Skip(1).FirstOrDefault(), out int world_id))
                world_id = 0;
            this.health.AddOrUpdate(world_id, 0.25f, (_, v) => v + 0.25f);
        }
        else if (item_name.StartsWith("Bottle"))
        {
            string[] parts = item_name.Split(':');
            if (!int.TryParse(parts.Skip(1).FirstOrDefault(), out int world_id))
                world_id = 0;
            this.addItemByName($"LogicalBottle:{world_id}");
        }

        if (newCount > 1)
        {
            this.item_count[item_name + "|" + newCount] = 1;
        }
    }

    /**
     * Get listing of all items contained in Inventory.
     */
    public string[] all()
    {
        return this.item_count.Keys.ToArray();
    }

    /**
     * Get all items in inventory as Items.
     */
    public Item[] toArray()
    {
        var result = new List<Item>();
        foreach (var (key, count) in this.item_count)
        {
            if (key.Contains('|'))
            {
                continue;
            }
            string[] itemParts = key.Split(':');
            string name = itemParts[0];
            if (!int.TryParse(itemParts.Skip(1).FirstOrDefault(), out int world_id))
                world_id = 0;
            for (int i = 0; i < count; ++i)
            {
                result.Add(Item.get(name, world_id));
            }
        }
        return result.ToArray();
    }

    /**
     * Determine how many of a particular item are in inventory.
     * 
     * @param string key item name to search for
     */
    public int getCount(string key)
    {
        if (key.StartsWith("Bottle"))
        {
            string[] itemParts = key.Split(':');
            if (!int.TryParse(itemParts.Skip(1).FirstOrDefault(), out int world_id))
                world_id = 0;
            key = $"LogicalBottle:{world_id}";
        }

        return this.item_count.GetValueOrDefault(key, 0);
    }

    /**
     * Verify if item is in inventory.
     *
     * @param string item_name
     */
    public bool has(string item_name)
    {
        return this.item_count.GetValueOrDefault(item_name, 0) > 0;
    }

    /**
     * Get new Inventory with merge from another Inventory.
     *
     * @param self inventory Inventory to merge
     */
    public Inventory merge(Inventory inventory)
    {
        var newInventory = new Inventory(this);

        foreach (var (key, count) in inventory.item_count)
        {
            if (key.Contains('|'))
            {
                continue;
            }

            int newCount = newInventory.item_count.AddOrUpdate(key, count, (_, v) => v + count);

            for (int i = 2; i <= newCount; ++i)
            {
                newInventory.item_count[key + "|" + i] = 1;
            }
        }

        return newInventory;
    }

    /**
     * Get the health value available based on items in this world.
     * 
     * @param int world_id world id for which we care about count
     */
    public float heartCount(int world_id)
    {
        return this.health.GetValueOrDefault(world_id, 0);
    }
}
