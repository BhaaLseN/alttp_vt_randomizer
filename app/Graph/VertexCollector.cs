namespace App.Graph;

/**
 * Container for all the vertices.
 */
class VertexCollector
{
    /**
     * This does not account for door rando.
     */
    private static readonly string[] BUNNY_REVIVE =
    {
        "Eastern Palace - Entrance",
        "Desert Palace - Main Room - Center",
        "Desert Palace - Right Entrance",
        "Desert Palace - Beemos Torches",
        "Desert Palace - Beemos 2",
        "Agahnims Tower - Entrance",
        "Palace of Darkness - Lobby",
        "Skull Woods - Main Entrance",
        "Skull Woods - Pinball Room",
        "Skull Woods - Firebar Pits",
        "Skull Woods - Statue Puzzle",
        "Skull Woods - Bumper Buddy",
        "Skull Woods - Bridge Room",
        "Thieves' Town - Grand Room SW",
        "Ice Palace - Entrance",
        "Misery Mire - Entrance",
        "Turtle Rock - Big Chest Entrance",
        "Turtle Rock - Laser Entrance",
        "Turtle Rock - Eye Bridge",
        "Ganon's Tower - Lobby",
    };

    /**
     * Get all vertices for a world and map static items to that world. Also
     * given the world config, we may invert the moonpearl requirements here.
     *
     * @param World world world to attach preset items to
     *
     * @throws Exception if unable to read data files
     */
    public List<Dictionary<string, object>> loadYmlData(World world)
    {
        var vertex_data = array_merge_recursive(
            ymlReadDir(app_path("Graph/data/Vertices")),
            world.config("vertex_data", new List<Vertex>())
        );

        int world_id = world.id;
        bool inverted = world.config<string>("mode.state") == "inverted";
        var bunny_revive = world.config("tech", new string[0]).Contains("dungeon_bunny_revival");
        var names = new HashSet<string>();
        var vertices = new List<Dictionary<string, object>>();

        // overworld
        foreach (var map in vertex_data["maps"] ?? new List<Dictionary<string, object>>())
        {
            var shared = new Dictionary<string, object>()
            {
                { "map", map["map"] },
            };
            foreach (var meta in map["nodes"]["meta"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("type", "meta")
                    .Merge(meta));
            }
            foreach (var prizepack in map["nodes"]["prizepacks"] ?? new Dictionary<string, object>())
            {
                vertices.Add(prizepack
                    .MergeOne("type", "prizepack"));
            }
            foreach (var region in map["nodes"]["regions"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("moonpearl", map["moonpearl"])
                    .MergeOne("type", "region")
                    .Merge(region));
            }
            foreach (var warp in map["nodes"]["warps"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("moonpearl", map["moonpearl"])
                    .MergeOne("type", "warp")
                    .Merge(warp));
            }
            foreach (var mob in map["nodes"]["mobs"] ?? new Dictionary<string, object>())
            {
                mob["sprite"] = Sprite.get(mob["sprite"]);
                vertices.Add(shared
                    .MergeOne("type", "mob")
                    .Merge(mob));
            }
            foreach (var item in map["nodes"]["items"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("type", "item")
                    .Merge(item));
            }
            foreach (var entrance in map["nodes"]["entrances"] ?? new Dictionary<string, object>())
            {
                if (entrance.TryGetValue("entranceid", out var entranceid))
                {
                    vertices.Add(shared
                        .MergeOne("name", entrance["name"] + " - In")
                        .MergeOne("entranceid", entranceid)
                        .MergeOne("type", "entrance"));
                }
                if (entrance.TryGetValue("outletid", out var outletid))
                {
                    vertices.Add(shared
                        .MergeOne("name", entrance["name"] + " - Out")
                        .MergeOne("outletid", outletid)
                        .MergeOne("type", "outlet"));
                }
            }
            // consider merging Holes into entrances
            foreach (var entrance in map["nodes"]["holes"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("name", entrance["name"])
                    .MergeOne("entranceids", entrance["entranceids"])
                    .MergeOne("type", "hole"));
            }
        }

        // underworld
        foreach (var room in vertex_data["rooms"] ?? new List<Dictionary<string, object>>())
        {
            var shared = new Dictionary<string, object>()
            {
                { "roomid", room["roomid"] },
                { "group", room.GetValueOrDefault("group") ?? 0 },
                { "dark", room.GetValueOrDefault("dark") ?? false },
            };
            foreach (var region in room["nodes"]["regions"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("type", "region")
                    .Merge(region));
                if (region.TryGetValue("inletid", out var inletid))
                {
                    vertices.Add(shared
                    .MergeOne("name", region["name"] + " - Exit")
                    .MergeOne("inletid", inletid));
                }
            }
            foreach (var mob in room["nodes"]["mobs"] ?? new Dictionary<string, object>())
            {
                mob["sprite"] = Sprite.get(mob["sprite"]);
                vertices.Add(shared
                    .MergeOne("type", "mob")
                    .Merge(mob));
            }
            foreach (var item in room["nodes"]["items"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("type", "item")
                    .Merge(item));
            }
            foreach (var keydoor in room["nodes"]["keydoors"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("type", "keydoor")
                    .Merge(keydoor));
            }
            foreach (var bigkeydoor in room["nodes"]["bigkeydoors"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("type", "bigkeydoor")
                    .Merge(bigkeydoor));
            }
            foreach (var shutter in room["nodes"]["shutters"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("type", "shutter")
                    .Merge(shutter));
            }
            foreach (var pot in room["nodes"]["pots"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .MergeOne("type", "pot")
                    .Merge(pot));
            }
            // TODO: how do we want to handle this?
            foreach (var item in room["nodes"]["inventory"] ?? new Dictionary<string, object>())
            {
                vertices.Add(shared
                    .Merge(item));
            }

            //if (room["bosses"] ?? false)
            //{
            //    foreach (var (from, sprites) in room["bosses"]) {
            //        // do stuff
            //    }
            //}
        }

        return vertices.Select((v) =>
        {
            if (v.TryGetValue("itemset", out var itemSetO) && itemSetO is string[] itemSet)
            {
                v["itemset"] = itemSet.Select((set) => $"{set}:{world_id}").ToArray();
            }
            if (v.TryGetValue("key", out var keyO) && keyO is string key)
            {
                v["key"] = $"{key}:{world_id}";
            }
            if (inverted && v.TryGetValue("moonpearl", out var moonpearlO) && moonpearlO is bool moonpearl)
            {
                v["moonpearl"] = !moonpearl;
            }

            if (bunny_revive && BUNNY_REVIVE.Contains(v["name"]))
            {
                v["moonpearl"] = false;
            }
            string new_name = $"{v["name"]}:{world_id}";
            if (!names.Add(new_name))
            {
                throw new Exception($"Vertex Name collision `{new_name}`");
            }

            v["name"] = $"{v["name"]}:{world_id}";
            return v;
        }).ToList();
    }
}
