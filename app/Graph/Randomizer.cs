using System.Collections.Concurrent;

namespace App.Graph;

/**
 * This is the primary entry point for randomization. A new object is created
 * with a config array dictating how the worlds should be created and prepping
 * all graph infomation for those worlds.
 *
 * Walk thru walls: 7E037F01
 */
sealed class Randomizer
{
    private static readonly string[] ITEM_LOCATIONS = {
        "bigchest",
        "bonk",
        "chest",
        "drop",
        "dig",
        "event",
        "npc",
        //"mob", // enabling this will allow enemies for major items
        "pot",
        "shopitem",
        "standing",
        "prize",
        "pedestal",
        "refill",
        // FIXME: this is likely wrong, PHP doesn't have it. but without this, we don't get medallions in set_locations
        "medallion",
    };

    public Graph graph;
    private Graph starting_graph;
    private readonly Dictionary<string, Graph> graphs;
    private readonly ConcurrentDictionary<string, HashSet<Vertex>> key_doors = new();
    /**
     * Key edges for just this door.
     */
    private readonly Dictionary<Vertex, Graph> key_door_edges = new();
    /** @var Collection<Vertex> */
    private readonly Dictionary<string, Vertex> vertices;
    private Inventory assumed_items;
    private Inventory collected_items;
    public HashSet<Vertex> found_locations;
    public Vertex start;
    /** @var array<array<Vertex>> */
    private readonly Dictionary<string, List<Vertex>> set_locations = new() { { "*", new() } };
    private readonly World[] worlds;
    private readonly Dictionary<string, List<Vertex>> door_chains = new();
    //static array door_cache = [];

    /**
     * Set up the Randomizer. This involves:
     * 1. Creating a new Graph
     * 2. Creating a starting vertex
     * 3. Creating a mapping of placement groups and their vertices
     * 4. using an array of array type configs for all the worlds we would like
     *    to randomize and building them out
     * 5. and keeping track of all the vertices in the graph
     *
     * @param array configs options for the worlds
     *
     * @return void
     */
    public Randomizer(Dictionary<string, object>[] configs)
    {
        this.graph = new Graph();
        this.start = this.graph.newVertex(new()
        {
            { "name", "start" },
            { "type", "meta" },
        });

        this.vertices = new Dictionary<string, Vertex>
        {
            { this.start.name, this.start },
        };
        this.collected_items = new Inventory();

        this.worlds = new World[configs.Length];
        foreach (var (i, config) in configs.Select((c, i) => (i, c)))
        {
            this.worlds[i] = new World(i, config);
            this.collected_items = this.collected_items.merge(this.worlds[i].collected_items);

            var shop_filler = new ShopFiller(this.worlds[i]);
            shop_filler.adjustEdges();

#if enabled
            var entrance_shuffler = new EntranceShuffler(this.worlds[i]);
            entrance_shuffler.adjustEdges();

            // boss shuffler must be called before enemy shuffler as enemy
            // shuffler will update sprite GFX sheets.
            var boss_shuffler = new BossShuffler(this.worlds[i]);
            boss_shuffler.adjustEdges();

            // This will handle challenge rooms
            var enemy_shuffler = new EnemyShuffler(this.worlds[i]);
            enemy_shuffler.adjustEdges();
#endif
            var bunnifier = new BunnyGraphifier(this.worlds[i]);
            bunnifier.adjustEdges();

            var prizepack_shuffler = new PrizePackShuffler(this.worlds[i]);
            prizepack_shuffler.adjustEdges();

            this.graph = this.graph.merge(this.worlds[i].graph);
            this.graph.addDirected(this.start, this.graph.getVertex($"start:{i}"), "fixed");
        }

        foreach (var location in this.graph.getVertices())
        {
            this.vertices[location.name] = location;
            if (!ITEM_LOCATIONS.Contains(location.type))
            {
                continue;
            }
            this.set_locations["*"].Add(location);
            foreach (var set in location.itemset)
            {
                this.set_locations.TryAdd(set, new());
                this.set_locations[set].Add(location);
            }
        }

        // @todo this needs simplified!
        var graphs = new Dictionary<string, Graph>();
        var edge_groups = this.graph.getEdges().Select(edge => edge.Group);
        foreach (var group in edge_groups)
        {
            if (graphs.ContainsKey(group))
            {
                continue;
            }

            if (group.StartsWith("Key") && !group.StartsWith("KeyForKey"))
            {
                var key_edges = this.graph.getEdges().Where(edge => edge.Group == group);

                var keyDoorsForGroup = this.key_doors.GetOrAdd(group, _ => new());
                foreach (var edge in key_edges)
                {
                    keyDoorsForGroup.Add(edge.From);
                }

                foreach (var key_door in keyDoorsForGroup)
                {
                    var graphForKeyDoor = new Graph();
                    // TODO: should this realy overwrite the graph?
                    //this.key_door_edges.Add(key_door, graphForKeyDoor);
                    this.key_door_edges[key_door] = graphForKeyDoor;
                    foreach (var edge in key_edges)
                    {
                        if (edge.From == key_door)
                        {
                            graphForKeyDoor.addEdge(edge);
                        }
                    }
                }
            }
            else
            {
                graphs.Add(group, this.graph.getSubgraph(group));
            }
        }

        this.graphs = graphs;

        this.starting_graph = this.graphs["fixed"];
        this.starting_graph = this.searchGraph(this.collected_items);
    }

    /**
     * Randomize the worlds. This handles creating a filler and placing those
     * items into the worlds.
     * 
     * @return World[]
     */
    public World[] randomize()
    {
        var filler = new RandomAssumedFiller(this, new()
        {
            { "accessibility", this.worlds.Select((world, idx) => (Key: idx, Value: world.config<string>("accessibility"))).ToDictionary(k => k.Key, v => (object)v.Value) },
        });

        var sets = new ItemPooler(this.worlds).getPool();

        filler.fillGraph(sets);

        return this.worlds;
    }

    /**
     * Get a complete connected graph that has been searched based on a supplied
     * inventory. One may supply a starting graph to continue searching from
     * along with item locations within the graph that have already been
     * accounted for.
     *
     * @todo Speed this up!!! Every 1ms in here ~= 100ms of total time
     *
     * @param Inventory collected Items assumed to be collected before the search has started
     * @param Graph? starting_graph A graph that is already partially searched
     * @param array collected_item_map a listing of locations that are already accounted for in collected
     */
    private Graph searchGraph(Inventory collected, Graph? starting_graph = null)
    {
        var search_graph = starting_graph ?? this.starting_graph;
        var graphs = new Dictionary<string, Graph>(this.graphs);
        bool newItemsFound;
        do
        {
            var sub_graphs = new List<Graph>();
            foreach (var (item, sub_graph) in graphs)
            {
                if (collected.has(item))
                {
                    sub_graphs.Add(sub_graph);
                    graphs.Remove(item);
                }
            }

            search_graph = search_graph.merge(sub_graphs.ToArray());
            search_graph.search(this.start);

            newItemsFound = false;
            foreach (var item in search_graph.getItems(this.start))
            {
                if (!collected.has(item.name))
                {
                    if (item.name.StartsWith("BigRedBomb") && this.dropOffSearch(item, search_graph))
                    {
                        collected = collected.addItem(Item.get("BigRedBombActive", item.world_id));
                    }
                    newItemsFound = true;
                    collected = collected.addItem(item);
                }
            }
        } while (newItemsFound);

        return search_graph;
    }

    private bool dropOffSearch(Item item, Graph search_graph)
    {
        var start = this.vertices["Bomb Shoppe Lobby:" + item.world_id];
        var end = this.vertices["Pyramid:" + item.world_id];
        // @todo tidy up this exclude by only being hops/entrances.
        string[] exclude = {
            $"hop:{item.world_id}",
            $"Flippers:{item.world_id}",
            $"DarkFlippers:{item.world_id}",
        };
        var bomb_search_graph = search_graph.exclude(exclude);
        bomb_search_graph.search(start);

        return bomb_search_graph.getVisited(start).Contains(end);
    }

    /**
     * Search the graph starting with a set number of keys, and determine the
     * shared outcome if one were to use their keys in every possible way.
     *
     * @param Graph search_graph starting search
     * @param array found previously found locations
     * @param Inventory collected already collected items
     * @param array locked_doors currently locked doors
     * @param string key which key we are unlocking for
     * @param int recursion_level how deep in this %#*& we are
     * @param array chain listing of doors already opened
     */
    private IEnumerable<Vertex> recursiveDoorSearch(
        Graph search_graph,
        IEnumerable<Vertex> found,
        Inventory collected,
        IEnumerable<Vertex> locked_doors,
        string key,
        int recursion_level,
        IEnumerable<Vertex>? chain = null
    )
    {
        if (collected.getCount(key) >= this.key_doors[key].Count)
        {
            var door_graphs = this.key_door_edges.Where(door_id => this.key_doors[key].Contains(door_id.Key)).Select(k => k.Value).ToArray();
            var graph = this.searchGraph(collected, search_graph.merge(door_graphs));
            return graph.getVisited(this.start);
        }

        var in_graph_locations = search_graph.getVisited(this.start);
        var sub_found_locations = new Dictionary<Vertex, List<Vertex>>();
        chain ??= Enumerable.Empty<Vertex>();
        found = found.ToArray();
        foreach (var door in locked_doors.Where(location => in_graph_locations.Contains(location)))
        {
            var current_chain = chain.Concat(new[] { door }).ToArray();
            string chain_id = string.Join('-', current_chain.Select(v => v.GetHashCode()).OrderBy(h => h));
            if (this.door_chains.TryGetValue(chain_id, out var door_chain))
            {
                sub_found_locations.Add(door, door_chain);
                continue;
            }
            var graph = this.searchGraph(collected, search_graph.merge(this.key_door_edges[door]));
            var found_locations = graph.getVisited(this.start).ToList();
            var new_collected = collected.merge(this.collectItems(found_locations.Except(found)));
            sub_found_locations.Add(door, found_locations);
            int new_found_keys = new_collected.getCount(key);
            if (locked_doors.Count() > 1 && new_found_keys > recursion_level)
            {
                var all_found = found_locations.Concat(found).ToArray();
                // TODO: should this be overwriting the door search result?
                //sub_found_locations.Add(door, this.recursiveDoorSearch(
                sub_found_locations[door] = this.recursiveDoorSearch(
                    graph,
                    all_found,
                    new_collected,
                    locked_doors.Except(new[] { door }).ToArray(),
                    key,
                    recursion_level + 1,
                    current_chain
                ).ToList();
            }
            this.door_chains.Add(chain_id, sub_found_locations[door]);
        }

        if (sub_found_locations.Any())
        {
            IEnumerable<Vertex> result = sub_found_locations.Values.First();
            foreach (var other in sub_found_locations.Values.Skip(1))
                result = result.Intersect(other);
            return result;
        }

        return found;
    }

    /**
     * Get locations that are accessible no matter how one uses the keys
     * available.
     *
     * @param Inventory collected Assumed collected items
     */
    public IEnumerable<Vertex> getStrongLocations(Inventory collected)
    {
        var search_graph = this.searchGraph(collected);
        var found_locations = search_graph.getVisited(this.start).ToHashSet();
        var new_found_locations = new HashSet<Vertex>();
        do
        {
            this.door_chains.Clear();
            new_found_locations.Clear();
            var new_items = this.collectItems(found_locations);
            var new_collected = collected.merge(new_items);
            search_graph = this.searchGraph(new_collected, null);
            foreach (var (key, doors) in this.key_doors)
            {
                if (new_collected.has(key))
                {
                    var strong_locations = this.recursiveDoorSearch(
                        search_graph,
                        found_locations,
                        new_collected,
                        doors,
                        key,
                        1
                    ).ToHashSet();
                    strong_locations.ExceptWith(found_locations);
                    found_locations.UnionWith(strong_locations);
                    new_found_locations.UnionWith(strong_locations);
                }
            }
        } while (new_found_locations.Any());

        return found_locations;
    }

    /**
     * Assume a set of items and search for locations that would be accessable.
     *
     * @param array<Item> items items to assume
     */
    public void assumeItems(IEnumerable<Item> items)
    {
        this.assumed_items = new Inventory(items.ToArray());
        this.found_locations = this.getStrongLocations(this.collected_items.merge(this.assumed_items)).ToHashSet();
    }

    /**
     * Get locations in an item set, one may toggle reachability of these
     * locations.
     *
     * @param string item_set contrain results to item set
     * @param array item_sets counts of items required in each set
     * @param bool reachable only return reachable locations
     *
     * @throws Exception if there are no available locations in a set
     *
     * @return array<Vertex>
     */
    public IEnumerable<Vertex> getEmptyLocationsInSet(string item_set = "*", Dictionary<string, int>? item_sets = null, bool reachable = true)
    {
        var empty_locations = this.set_locations[item_set].Where((vertex) =>
        {
            return (!reachable || this.found_locations.Contains(vertex)) && vertex.item == null;
        }).ToList();

        item_sets ??= new();
        foreach (var (set_name, set_count) in item_sets)
        {
            if (set_name == "*")
            {
                continue;
            }
            var set_locations = this.set_locations[set_name].Where(static (location) =>
            {
                return location.item == null;
            });
            if (set_locations.Count() < set_count)
            {
                throw new Exception($"Not enough set locations available: {set_name}");
            }
            // if a set has the same number of items to place as set locations
            // left, remove it from this return.
            if (item_set != set_name && set_locations.Count() == set_count)
            {
                empty_locations.RemoveAll(set_locations.Contains);
            }
        }

        return empty_locations.ToArray();
    }

    /**
     * Determine if a location is considered reachable after last assumeItems
     * call.
     *
     * @param string location_name location to check reachability of
     */
    public bool canReachLocation(string location_name)
    {
        return this.found_locations.Contains(this.vertices[location_name]);
    }

    /**
     * Get array of locations that currently have placed items.
     *
     * @param array? locations filtered locations to check, otherwise all locations
     */
    public IEnumerable<Vertex> locationsWithItems(IEnumerable<Vertex>? locations = null)
    {
        return (locations ?? this.found_locations).Where((location) => location.item is not null || location.trophy is not null);
    }

    // return all items for locations that have items
    private List<Item> getItems(IEnumerable<Vertex>? locations = null)
    {
        var items = new List<Item>();
        var locationsWithItems = this.locationsWithItems(locations);
        foreach (var location in locationsWithItems)
        {
            if (location.item is not null)
            {
                items.Add(location.item);
            }
            if (location.trophy is not null)
            {
                items.Add(location.trophy);
            }
        }
        return items;
    }
    /**
     * Search world and get all items that are found.
     *
     * @param array? locations filtered locations to check, otherwise all locations
     */
    public Inventory collectItems(IEnumerable<Vertex>? locations = null)
    {
        var items = getItems(locations);
        return new Inventory(items.ToArray());
    }

    /**
     * Get a vertex by name.
     *
     * @param string location_name name to search for
     */
    public Vertex? getLocation(string location_name)
    {
        return this.vertices.GetValueOrDefault(location_name);
    }
}
