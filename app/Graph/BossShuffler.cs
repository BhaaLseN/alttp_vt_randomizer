namespace App.Graph;

internal static class PHP
{
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> array, params IDictionary<TKey, TValue>[] arrays)
        where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(array);
        foreach (var (key, value) in arrays.SelectMany(d => d))
            result.TryAdd(key, value);

        return result;
    }
    public static Dictionary<TKey, TValue> Diff<TKey, TValue>(this IDictionary<TKey, TValue> array, params TValue[] arrays)
        where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(array);
        var comparer = EqualityComparer<TValue>.Default;
        foreach (var value in arrays.Select(d => d))
            result.Remove(result.FirstOrDefault(e => comparer.Equals(e.Value, value)).Key);

        return result;
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.OrderBy(_ => Random.Shared.Next());
}

public sealed class YamlSprite
{
    public Dictionary<string, object?> ToDictionary() => new(); // FIXME: read this from YAML.
}

/**
 * Modify the edges of the graph to place bosses.
 */
sealed class BossShuffler
{
    private static readonly Dictionary<string, string> BOSS_ITEMS = new()
    {
        { "Armos", "DefeatArmos" },
        { "Lanmola", "DefeatLanmolas" },
        { "Moldorm", "DefeatMoldorm" },
        { "Agahnim", "DefeatAgahnim" },
        { "Helmasaur", "DefeatHelmasaur" },
        { "Arrghus", "DefeatArrghus" },
        { "Mothula", "DefeatMothula" },
        { "Blind", "DefeatBlind" },
        { "Kholdstare", "DefeatKholdstare" },
        { "Vitreous", "DefeatVitreous" },
        { "Trinexx", "DefeatTrinexx" },
        { "Agahnim2", "DefeatAgahnim2" },
        { "Ganon", "DefeatGanon" },
    };
    private static readonly string[] NEVER_PLACE = new[]
    {
        "DefeatAgahnim",
        "DefeatAgahnim2",
        "DefeatGanon",
    };
    private static readonly Dictionary<string, string[]> NO_PLACE = new()
    {
        {  "Ganon's Tower - Moldorm", new[] {
            "DefeatArmos",
            "DefeatArrghus",
            "DefeatBlind",
            "DefeatLanmolas",
            "DefeatTrinexx",
        } },
        { "Ganon's Tower - Lanmolas", new[] {
            "DefeatBlind",
        } },
        { "Ganon's Tower - Ice Armos", new[] {
        "DefeatTrinexx",
        } },
        { "Skull Woods - Boss", new[] {
        "DefeatTrinexx",
        } },
        { "Tower Of Hera - Boss", new[] {
            "DefeatArmos",
            "DefeatArrghus",
            "DefeatBlind",
            "DefeatLanmolas",
            "DefeatTrinexx",
        } },
    };
    private static readonly Dictionary<string, string> BOSS_FROM_LOCATION = new()
    {
        { "Ganon's Tower - Moldorm", "Ganon's Tower - Moldorm - Kill Zone" },
        { "Ganon's Tower - Lanmolas", "Ganon's Tower - Gauntlet Refill" },
        { "Tower Of Hera - Boss", "Tower Of Hera - Boss Room" },
        { "Skull Woods - Boss", "Skull Woods - Boss Room" },
        { "Eastern Palace - Boss", "Eastern Palace - Boss Room" },
        { "Desert Palace - Boss", "Desert Palace - Boss Room" },
        { "Palace of Darkness - Boss", "Palace of Darkness - Boss Room" },
        { "Swamp Palace - Boss", "Swamp Palace - Boss Room" },
        { "Thieves' Town - Boss", "Thieves' Town - Boss Room" },
        { "Ice Palace - Boss", "Ice Palace - Boss Room" },
        { "Misery Mire - Boss", "Misery Mire - Boss Room" },
        { "Turtle Rock - Boss", "Turtle Rock - Boss Room" },
        { "Ganon's Tower - Ice Armos", "Ganon's Tower - Ice Room" },
    };

    private Dictionary<string, ILookup<string, YamlSprite>> boss_location_map;
    private readonly World world;

    /**
     * Add all the vertices to the graph for this region.
     *
     * @param World world world to reduce graph for
     *
     * @return void
     */
    public BossShuffler(World world)
    {
        this.world = world;
        this.boss_location_map = Yaml.parse(
            file_get_contents(app_path("Graph/data/Bosses/SpriteLocations.yml"))
         ?? []).keyBy((item, key) => $"{key}:{world.id}");
    }

    /**
     * Swap Entrances based on world settings.
     */
    public void adjustEdges()
    {
        // most restrictive first
        var boss_locations = new List<string>()
        {
            "Ganon's Tower - Moldorm",
            "Ganon's Tower - Lanmolas",
            "Tower Of Hera - Boss",
            "Skull Woods - Boss",
            "Eastern Palace - Boss",
            "Desert Palace - Boss",
            "Palace of Darkness - Boss",
            "Swamp Palace - Boss",
            "Thieves' Town - Boss",
            "Ice Palace - Boss",
            "Misery Mire - Boss",
            "Turtle Rock - Boss",
            "Ganon's Tower - Ice Armos",
        };

        // force Kholdstare for swordless to be in Ice Palace
        if (this.world.config("mode.weapons") == "swordless")
        {
            // remove Ice Palace
            boss_locations.RemoveAt(9);
            this.placeBossItemInLocation("DefeatKholdstare", "Ice Palace - Boss");
        }

        List<string> place_bosses;
        switch (this.world.config("enemizer.bossShuffle"))
        {
            case "random":
                foreach (var location in boss_locations)
                {
                    var bosses = NO_PLACE.TryGetValue(location, out var noPlaceLocations)
                        ? BOSS_ITEMS.Values.Except(noPlaceLocations).Except(NEVER_PLACE)
                        : BOSS_ITEMS.Values.Except(NEVER_PLACE);
                    var boss = bosses.Shuffle().First();
                    this.placeBossItemInLocation(boss, location);
                }
                break;
            case "full": // 1 copy of each, +3 other copies
                place_bosses = new()
                {
                    "DefeatArmos",
                    "DefeatLanmolas",
                    "DefeatMoldorm",
                    "DefeatHelmasaur",
                    "DefeatArrghus",
                    "DefeatMothula",
                    "DefeatBlind",
                    "DefeatKholdstare",
                    "DefeatVitreous",
                    "DefeatTrinexx",
                };
                place_bosses.AddRange(place_bosses.Shuffle().Take(3));

                foreach (var location in boss_locations)
                {
                    var bosses = NO_PLACE.TryGetValue(location, out var noPlaceLocations)
                        ? place_bosses.Except(noPlaceLocations).Except(NEVER_PLACE)
                        : place_bosses.Except(NEVER_PLACE);
                    var boss = bosses.Shuffle().First();
                    place_bosses.Remove(boss);
                    this.placeBossItemInLocation(boss, location);
                }
                break;
            case "simple": // 1:1
                place_bosses = new()
                {
                    "DefeatArmos",
                    "DefeatLanmolas",
                    "DefeatMoldorm",
                    "DefeatHelmasaur",
                    "DefeatArrghus",
                    "DefeatMothula",
                    "DefeatBlind",
                    "DefeatKholdstare",
                    "DefeatVitreous",
                    "DefeatTrinexx",
                    "DefeatArmos",
                    "DefeatLanmolas",
                    "DefeatMoldorm",
                };

                foreach (var location in boss_locations)
                {
                    var bosses = NO_PLACE.TryGetValue(location, out var noPlaceLocations)
                        ? place_bosses.Except(noPlaceLocations).Except(NEVER_PLACE)
                        : place_bosses.Except(NEVER_PLACE);
                    var boss = bosses.Shuffle().First();
                    place_bosses.Remove(boss);
                    this.placeBossItemInLocation(boss, location);
                }
                break;
            case "none":
            default:
                this.placeBossItemInLocation("DefeatArmos", "Eastern Palace - Boss");
                this.placeBossItemInLocation("DefeatLanmolas", "Desert Palace - Boss");
                this.placeBossItemInLocation("DefeatMoldorm", "Tower Of Hera - Boss");
                this.placeBossItemInLocation("DefeatHelmasaur", "Palace of Darkness - Boss");
                this.placeBossItemInLocation("DefeatArrghus", "Swamp Palace - Boss");
                this.placeBossItemInLocation("DefeatMothula", "Skull Woods - Boss");
                this.placeBossItemInLocation("DefeatBlind", "Thieves' Town - Boss");
                this.placeBossItemInLocation("DefeatKholdstare", "Ice Palace - Boss");
                this.placeBossItemInLocation("DefeatVitreous", "Misery Mire - Boss");
                this.placeBossItemInLocation("DefeatTrinexx", "Turtle Rock - Boss");
                this.placeBossItemInLocation("DefeatArmos", "Ganon's Tower - Ice Armos");
                this.placeBossItemInLocation("DefeatLanmolas", "Ganon's Tower - Lanmolas");
                this.placeBossItemInLocation("DefeatMoldorm", "Ganon's Tower - Moldorm");
                break;
        }

        // since we added the bosses at this step we need to recache the
        // vertices.
        this.world.remapVertices();
    }

    /**
     * Place Boss item in location.
     * 
     * @throws Exception If Can't place boss in location
     * 
     * @param string boss_item Boss item name
     * @param string location Location name
     */
    private void placeBossItemInLocation(string boss_item, string location)
    {
        string world_boss_item = boss_item + ":" + this.world.id;
        string from_location = BOSS_FROM_LOCATION[location] + ":" + this.world.id;
        var from = this.world.graph.getVertex(from_location);
        location = location + ":" + this.world.id;
        var to_boss = this.world.graph.getVertex(location);

        if (from is null || to_boss is null)
        {
            //Log.error("Can't place boss.", [from_location, from, location, to_boss]);

            throw new Exception("Can't place boss.");
        }

        to_boss.enemizerBoss = BOSS_ITEMS.FirstOrDefault(kvp => kvp.Value == boss_item).Key;
        this.updateSprites(from, boss_item);
        this.world.graph.addDirected(from, to_boss, world_boss_item);
    }

    private void updateSprites(Vertex bossRoom, string boss)
    {
        foreach (var sprite_definition in this.boss_location_map[bossRoom.name][boss])
        {
            this.world.graph.newVertex(
                sprite_definition.ToDictionary().Merge(new Dictionary<string, object?>()
                {
                    { "type", "mob" },
                    //{ "sprite", Sprite.get(sprite_definition["sprite"]) },
                }));
        }
    }
}
