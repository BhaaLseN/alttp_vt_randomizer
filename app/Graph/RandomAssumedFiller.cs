namespace App.Graph;

// NOTE: same as in ItemPooler, except we cannot reuse aliases this way
using ItemSet = Dictionary<string, /* WeightedSet */ Dictionary<int, List<Item>>>;

/**
 * Meat and potatoes of filling graph based randomizers.
 */
sealed class RandomAssumedFiller
{
    private readonly Randomizer randomizer;
    private readonly Dictionary<string, Dictionary<int, object>> config = [];
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

        var flat_items = items
            .SelectMany(set => set.Value.SelectMany(weight => weight.Value
                .Select(item => (Set: set.Key, Weight: weight.Key, Item: item))))
            .Shuffle()
            // fix placement groups
            .OrderBy(i => i.Weight)
            .ToList();

        foreach (var item_key in flat_items)
        {
            var (item_set, item_weight, item) = item_key;
            if (item_weight > 9000)
            {
                break;
            }

            flat_items.Remove(item_key);
            var item_world_id = item.world_id;
            this.randomizer.assumeItems(flat_items.Where(i => i.Weight <= 9000).Select(i => i.Item).ToList());
            bool required = false;
            IEnumerable<Vertex> locations;
            if (
                (string)this.config["accessibility"][item_world_id] == "none"
                && this.randomizer.collectItems().has($"Triforce:{item_world_id}")
            )
            {
                locations = this.randomizer.getEmptyLocationsInSet(item_set, set_counts, false);
            }
            else
            {
                required = true;
                locations = this.randomizer.getEmptyLocationsInSet(item_set, set_counts);
            }

            if (!locations.Any())
            {
                throw new Exception($"No locations for: {item}");
            }

            var location = locations.Shuffle().First();
            Console.WriteLine("[%s] [%s] Placing `%s` in `%s` (%s:%d)", 
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
        //Log.debug(sprintf("Fast Filling %s items", count(fill_items)));
        // assure smaller location groups are filled first
        fill_items.Sort((a, b) =>
        {
            int aweight = a.Weight + (a.Set == "*" ? 9999 : 0);
            int bweight = b.Weight + (b.Set == "*" ? 9999 : 0);
            return aweight - bweight;
        });

        string current_key = "";
        var locations = new List<Vertex>();
        foreach (var (item_set, _, item) in fill_items) {
            if (current_key != item_set)
            {
                locations = this.randomizer.getEmptyLocationsInSet(item_set, [], false).Shuffle().ToList();
                current_key = item_set;
            }

            var location = locations.LastOrDefault();
            if (location is null)
            {
                //Log.debug(sprintf("No Location: `%s` `%s`", item, item_set));
                continue;
            }
            location.item = item;
            locations.Remove(location);
            //Log.debug(vsprintf("[FF] Placing: `%s` in `%s` (%s:%d)", [
            //    item,
            //    location.name,
            //    item_set,
            //    count(locations) + 1,
            //]));
        }
    }
}
