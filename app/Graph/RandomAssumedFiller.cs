namespace App.Graph;

// NOTE: same as in ItemPooler, except we cannot reuse aliases this way
using ItemSet = Dictionary<string, /* WeightedSet */ Dictionary<int, List<Item>>>;

/**
 * Meat and potatoes of filling graph based randomizers.
 */
sealed class RandomAssumedFiller
{
    private readonly Randomizer randomizer;
    private readonly Dictionary<string, Dictionary<int, object>> config = new();
    /**
     * Create graph filler.
     * 
     * @return void
     */
    public RandomAssumedFiller(Randomizer randomizer, Dictionary<string, Dictionary<int, object>> config)
    {
        this.randomizer = randomizer;
        this.config = config;
    }

    /**
     * This fill places items in the first available location that it can
     * possibly be in, assuming that unplaced items will be reachable. Those
     * items will then have a smaller set of places that they can be placed.
     *
     * @param array items items to be placed from ItemPooler
     */
    public void fillGraph(ItemSet items)
    {
        var set_counts = items.ToDictionary(k => k.Key, set => set.Value.SelectMany(x => x.Value).Count());

        //var flat_items = items
        //    .SelectMany(set => set.Value.SelectMany(weight => weight.Value
        //        .Select(item => (Set: set.Key, Weight: weight.Key, Item: item))))
        //    .Shuffle()
        //    // fix placement groups
        //    .OrderBy(i => i.Weight)
        //    .ToList();
        var flat_items_a = items
            .SelectMany(set => set.Value.SelectMany(weight => weight.Value
                .Select(item => (Set: set.Key, Weight: weight.Key, Item: item))))
            .ToArray();
        flat_items_a = PHP.fy_shuffle(flat_items_a).OrderBy(i => i.Weight).ToArray();
        // fix placement groups
        //Array.Sort(flat_items_a, (a, b) => a.Weight - b.Weight);
        var flat_items = flat_items_a.ToList();

        foreach (var item_key in flat_items_a)
        //foreach (var item_key in flat_items.ToArray())
        {
            var (item_set, item_weight, item) = item_key;
            if (item_weight > 9000)
            {
                break;
            }

            flat_items.Remove(item_key);
            var item_world_id = item.world_id;
            this.randomizer.assumeItems(flat_items.Where(i => i.Weight <= 9000).Select(i => i.Item).ToList());
            bool required = (string)this.config["accessibility"][item_world_id] != "none"
                || !this.randomizer.collectItems().has($"Triforce:{item_world_id}");
            var locations = this.randomizer.getEmptyLocationsInSet(item_set, set_counts, required);

            if (!locations.Any())
            {
                throw new Exception($"No locations for: {item}");
            }

            var location = PHP.get_random_element(locations);
            System.Console.WriteLine("[{0}] [{1}] Placing `{2}` in `{3}` ({4}:{5})",
                item_weight,
                required ? "R" : " ",
                item,
                location.name,
                item_set,
                locations.Count()
            );

            location.item = item;
            set_counts[item_set]--;
        }

        // assume items after last placement to sort the graph out.
        this.randomizer.assumeItems(flat_items.Select(i => i.Item).ToList());

        this.fastFillItemsInLocations(flat_items);
    }

    /**
     * Quickly place items in locations respecting placemenmt groups.
     *
     * @param array fill_items keys list of items to place
     */
    private void fastFillItemsInLocations(List<(string Set, int Weight, Item Item)> fill_items)
    {
        System.Console.WriteLine("Fast Filling {0} items", fill_items.Count);
        // assure smaller location groups are filled first
        fill_items.Sort((a, b) =>
        {
            int aweight = a.Weight + (a.Set == "*" ? 9999 : 0);
            int bweight = b.Weight + (b.Set == "*" ? 9999 : 0);
            return aweight - bweight;
        });

        string current_key = "";
        var locations = new List<Vertex>();
        foreach (var (item_set, _, item) in fill_items)
        {
            if (current_key != item_set)
            {
                locations = PHP.fy_shuffle(this.randomizer.getEmptyLocationsInSet(item_set, null, false).ToArray()).ToList();
                current_key = item_set;
            }

            var location = locations.LastOrDefault();
            if (location is null)
            {
                System.Console.WriteLine("No Location: `{0}` `{1}`", item, item_set);
                continue;
            }
            location.item = item;
            locations.Remove(location);
            System.Console.WriteLine("[FF] Placing: `{0}` in `{1}` ({2}:{3})",
                item,
                location.name,
                item_set,
                locations.Count + 1
            );
        }
    }
}
