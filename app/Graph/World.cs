using System.Collections.Concurrent;

namespace App.Graph;

/**
 * Model of a world in which a player would be playing.
 */
sealed class World
{
    public readonly int id;
    public readonly Graph graph;
    private List<Vertex> vertices;
    public readonly Inventory collected_items;
    private readonly ConcurrentDictionary<string, object> _config;
    public Dictionary<string, List<string>> sprite_sheets = new()
    {
        { "underworld", new() },
        { "overworld", new() },
        { "sets", new() },
    };

    /**
     * Add all the vertices to the graph for this region.
     *
     * @param int id id of this world
     * @param array config options for this world
     *
     * @return void
     */
    public World(int id = 0, Dictionary<string, object>? config = null)
    {
        this.id = id;
        config ??= new();
        config.TryAdd("difficulty", "normal");
        config.TryAdd("logic", "NoGlitches");
        config.TryAdd("goal", "ganon");
        config.TryAdd("enemizer.enemyShuffle", "none");
        this._config = new(config);

        this.graph = new Graph();
        var start = this.graph.newVertex(new()
        {
            { "name", "start:" + this.id },
            { "type", "meta" },
        });

        var meta = this.graph.newVertex(new()
        {
            { "name", "Meta:" + this.id },
            { "type", "meta" },
        });
        this.graph.addDirected(start, meta, $"fixed:{this.id}");

        var items = new List<object>
        {
            Item.get("MagicBar", this.id),
            Item.get("LiftBush", this.id),
            Item.get("LiftPot", this.id),
            Item.get("UseBomb", this.id),
            Item.get("OpenChest", this.id),
            Item.get("BombUpgrade10", this.id),
            Item.get("ArrowUpgrade10", this.id),
            Item.get("ArrowUpgrade10", this.id),
            Item.get("ArrowUpgrade10", this.id),
            $"fixed:{this.id}",
            $"hop:{this.id}",
        };
        items.AddRange((Item[]?)config.GetValueOrDefault("equipment") ?? new[] {
            Item.get("BossHeartContainer", this.id),
            Item.get("BossHeartContainer", this.id),
            Item.get("BossHeartContainer", this.id),
        });
        if (this.config<string>("mode.state") == "standard")
        {
            items.Add($"EscapeLamp:{this.id}");
        }
        if (this.config<string>("accessibility") != "locations")
        {
            items.Add($"KeyForKey:{this.id}");
        }
        this.collected_items = new Inventory(items);

        var vertices = new VertexCollector().loadYmlData(this);
        vertices.ForEach(data =>
        {
            if (data.TryGetValue("item", out var item) && item is string itemKey)
            {
                data["item"] = Item.get(itemKey, this.id);
            }
            if (data.TryGetValue("trophy", out var trophy) && trophy is string trophyKey)
            {
                data["trophy"] = Item.get(trophyKey, this.id);
            }
            this.graph.newVertex(data);
        });

        var edges = new EdgeCollector().getForWorld(this);
        foreach (var (group, data) in edges)
        {
            foreach (var edge_data in data["directed"])
            {
                var from = this.graph.getVertex(edge_data[0]);
                var to = this.graph.getVertex(edge_data[1]);
                if (from is null || to is null)
                {
                    throw new Exception(
                        "Name Connection Mismatch: " +
                        $"({edge_data[0]}, {edge_data[1]}) => " +
                        $"({from}, {to})");
                }
                this.graph.addDirected(from, to, group);
            }
            foreach (var edge_data in data["undirected"])
            {
                var from = this.graph.getVertex(edge_data[0]);
                var to = this.graph.getVertex(edge_data[1]);
                if (from is null || to is null)
                {
                    throw new Exception(
                        "Undirected Name Connection Mismatch: " +
                        $"({edge_data[0]}, {edge_data[1]}) => " +
                        $"({from}, {to})");
                }
                this.graph.addDirected(from, to, group);
                this.graph.addDirected(to, from, group);
            }
        }
        // set special edges
        if (this.graph.getVertex($"TowerEntry:{this.id}") is Vertex towerEntry)
        {
            string entry = this.config("crystals.tower", 7) == 1
                ? "Crystal:" + this.id
                : "Crystal:" + this.id + "|" + this.config("crystals.tower", 7);

            this.graph.addDirected(this.graph.getVertex($"Meta:{this.id}"), towerEntry, entry);
        }
        if (this.graph.getVertex($"GanonVulnerable:{this.id}") is Vertex ganonVulnerable)
        {
            switch (this.config<string>("goal"))
            {
                case "dungeons":
                    // this has no effect; likely a relic of GraphViz to show us it was AD.
                    //this.graph.newVertex(["AllDungeons"]);
                    break;
                case "ganon":
                case "fastganon":
                default:
                    string vulnerable = this.config("crystals.ganon", 7) == 1
                        ? "Crystal:" + this.id
                        : "Crystal:" + this.id + "|" + this.config("crystals.ganon", 7);

                    this.graph.addDirected(this.graph.getVertex($"Meta:{this.id}"), ganonVulnerable, vulnerable);
                    break;
            }
        }

        this.remapVertices();
    }

    public void remapVertices()
    {
        this.vertices.Clear();
        this.vertices.AddRange(this.graph.getVertices());
    }

    /**
     * Get a vertex by name.
     *
     * @param string location_name name to search for
     */
    public Vertex? getLocation(string location_name)
    {
        if (!location_name.Contains(':'))
        {
            location_name = location_name + ":" + this.id;
        }
        return this.vertices.FirstOrDefault(v => v.name == location_name);
    }

    /**
     * Get a vertices by item contained.
     *
     * @param Item|null item item to search for
     * 
     * @return Collection<Vertex>
     */
    public IEnumerable<Vertex> getLocationsWithItem(Item? item = null)
    {
        return this.vertices.Where((Vertex vertex) => vertex.item == item);
    }

    /**
     * Get a vertices by type.
     *
     * @param string type type to search for
     * 
     * @return Collection<Vertex>
     */
    public IEnumerable<Vertex> getLocationsOfType(string type)
    {
        return this.vertices.Where((Vertex vertex) => vertex.type == type);
    }

    /**
     * Get vertices we can write to rom.
     */
    public IEnumerable<Vertex> getWritableVertices()
    {
        return this.vertices.Where(vertex => vertex.addresses != null);
    }

    /**
     * Get config value based on the currently set rules.
     *
     * @param string key dot notation key of config
     * @param mixed|null default value to return if key is not found
     *
     * @return mixed
     */
    public T config<T>(string key, T @default = default!)
    {
        return (T)this._config.GetOrAdd(key, @default!);
    }
}
