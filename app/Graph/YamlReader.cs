using System.Xml.Linq;
using YamlDotNet.Serialization;

namespace App.Graph;

public class YamlReader
{
    public static string DataRoot { get; set; } = "";

    public static string ItemsPath = "items.yml";
    public static string VerticesPath = "Vertices";

    class YamlItem
    {
        public byte[] bytes { get; set; }
    }

    public static Dictionary<string, byte[]> LoadItems()
    {
        string items_yml = Path.Join(DataRoot, ItemsPath);

        Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();

        var deserializer = new DeserializerBuilder().Build();
        using (TextReader reader = File.OpenText(items_yml))
        {
            var res = deserializer.Deserialize<Dictionary<string, YamlItem>>(reader);

            foreach (var item in res)
            {
                result.Add(item.Key, item.Value.bytes);
            }
        }

        return result;
    }

    public static Dictionary<string, DirectedUndirectedPair> LoadEdgesFromFile(string path)
    {
        string edges_yml = Path.IsPathFullyQualified(path) ? path : Path.Join(DataRoot, path);

        var deserializer = new DeserializerBuilder().Build();
        using (TextReader reader = File.OpenText(edges_yml))
        {
            return deserializer.Deserialize<Dictionary<string, DirectedUndirectedPair>>(reader);
        }
    }

    public static Dictionary<string, DirectedUndirectedPair> LoadEdgesFromDirectory(string path)
    {
        Dictionary<string, DirectedUndirectedPair> result = new Dictionary<string, DirectedUndirectedPair>();

        foreach (string file in Directory.GetFiles(Path.Join(DataRoot, path), "*.yml", SearchOption.AllDirectories))
        {
            var current_file_edges = LoadEdgesFromFile(file);
            MergeEdges(result, current_file_edges);
        }

        return result;
    }

    public static void MergeEdges(Dictionary<string, DirectedUndirectedPair> dest, Dictionary<string, DirectedUndirectedPair> source) {

        foreach (var entry in source)
        {
            DirectedUndirectedPair result_pair;
            if (dest.TryGetValue(entry.Key, out result_pair))
            {
                result_pair.directed.AddRange(entry.Value.directed);
                result_pair.undirected.AddRange(entry.Value.undirected);
            }
            else
            {
                dest.Add(entry.Key, entry.Value);
            }
        }
    }

    public static Entrances LoadEntrances(string name)
    {
        string entrances_yml = Path.Join(DataRoot, "Edges/entrances", name + ".yml");
        var deserializer = new DeserializerBuilder().Build();
        using (TextReader reader = File.OpenText(entrances_yml))
        {
            return deserializer.Deserialize<Entrances>(reader);
        }
    }

    public static Vertices LoadVerticesFromFile(string path)
    {
        string vertices_yml = Path.IsPathFullyQualified(path) ? path : Path.Join(DataRoot, path);
        using (TextReader reader = File.OpenText(vertices_yml))
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<Vertices>(reader);
        }
    }

    public static Vertices LoadVerticesFromDirectory()
    {
        Vertices result = new();

        foreach (string file in Directory.GetFiles(Path.Join(DataRoot, VerticesPath), "*.yml", SearchOption.AllDirectories))
        {
            var current_file_edges = LoadVerticesFromFile(file);
            MergeVertices(result, current_file_edges);
        }

        return result;
    }

    public static void MergeVertices(Vertices dest, Vertices source)
    {
        dest.Maps.AddRange(source.Maps);
        dest.Rooms.AddRange(source.Rooms);
    }
}


public class DirectedUndirectedPair
{
    public List<List<string>> undirected { get; set; }
    public List<List<string>> directed { get; set; }
}

public class ConnectionGroup
{
    [YamlMember(Alias = "in")]
    public List<string> In { get; set; } = new();
    [YamlMember(Alias = "out")]
    public List<string> Out { get; set; } = new();
}

public class Entrances
{
    [YamlMember(Alias = "fixed")]
    public List<List<string>> Fixed { get; set; } = new();
    [YamlMember(Alias = "connections")]
    public List<ConnectionGroup> Connections { get; set; } = new();
}

public partial class Map
{
    [YamlMember(Alias = "map")]
    public long MapMap { get; set; }

    [YamlMember(Alias = "moonpearl")]
    public bool Moonpearl { get; set; }

    [YamlMember(Alias = "nodes")]
    public MapNodes Nodes { get; set; }
}

public partial class MapNodes
{
    [YamlMember(Alias = "regions")]
    public List<Region> Regions { get; set; } = new();

    [YamlMember(Alias = "entrances")]
    public List<Entrance> Entrances { get; set; } = new();

    [YamlMember(Alias = "holes")]
    public List<Hole> Holes { get; set; } = new();

    [YamlMember(Alias = "items")]
    public List<ItemEntry> Items { get; set; } = new();

    [YamlMember(Alias = "warps")]
    public List<Warp> Warps { get; set; } = new();

    [YamlMember(Alias = "mobs")]
    public List<Entity> Mobs { get; set; } = new();

    [YamlMember(Alias = "meta")]
    public List<MetaEntry> Meta { get; set; } = new();

    [YamlMember(Alias = "prizepacks")]
    public List<Prizepack> Prizepacks { get; set; } = new();
}

public class MetaEntry
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "item")]
    public string? Item { get; set; }
}

public class Prizepack
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "offset")]
    public byte Offset { get; set; }

    [YamlMember(Alias = "sprite")]
    public string Sprite { get; set; }
}

public partial class Entrance
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "entranceid")]
    public long Entranceid { get; set; }

    [YamlMember(Alias = "outletid")]
    public long Outletid { get; set; }
}

public partial class Hole
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "entranceids")]
    public List<long> Entranceids { get; set; } = new();
}

public partial class ItemEntry
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "addresses")]
    public List<long> Addresses { get; set; } = new();

    [YamlMember(Alias = "type")]
    public string Type { get; set; }

    [YamlMember(Alias = "item")]
    public string? Item { get; set; }

    [YamlMember(Alias = "itemset")]
    public List<string> Itemset { get; set; } = new();
}

public partial class Warp
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "position")]
    public Position Position { get; set; }
}

public partial class Room
{
    [YamlMember(Alias = "roomid")]
    public long Roomid { get; set; }

    [YamlMember(Alias = "nodes")]
    public RoomNodes Nodes { get; set; }

    [YamlMember(Alias = "group")]
    public int? Group { get; set; }

    [YamlMember(Alias = "dark")]
    public bool Dark { get; set; } = false;

    [YamlMember(Alias = "bosses")]
    public object Bosses { get; set; }
}

public partial class RoomNodes
{
    [YamlMember(Alias = "regions")]
    public List<Region> Regions { get; set; } = new();

    [YamlMember(Alias = "keydoors")]
    public List<Keydoor> Keydoors { get; set; } = new();

    [YamlMember(Alias = "bigkeydoors")]
    public List<Keydoor> BigKeydoors { get; set; } = new();

    [YamlMember(Alias = "shutters")]
    public List<Shutter> Shutters { get; set; } = new();

    [YamlMember(Alias = "items")]
    public List<ItemEntry> Items { get; set; } = new();

    [YamlMember(Alias = "inventory")]
    public List<InventoryEntry> Inventory { get; set; } = new();

    [YamlMember(Alias = "pots")]
    public List<Entity> Pots { get; set; } = new();

    [YamlMember(Alias = "mobs")]
    public List<Entity> Mobs { get; set; } = new();

}

public partial class Keydoor
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "key")]
    public string Key { get; set; }
}

public partial class Shutter
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }
}

public partial class InventoryEntry
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "type")]
    public string Type { get; set; }

    [YamlMember(Alias = "item")]
    public string Item { get; set; }

    [YamlMember(Alias = "cost")]
    public long Cost { get; set; }

    [YamlMember(Alias = "itemset")]
    public List<string> Itemset { get; set; } = new();
}

public partial class Entity
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    // TODO: Some entries (pots) use a short, some have an x,y,z triple. Fix data
    [YamlMember(Alias = "position")]
    //public Position Position { get; set; }
    public object Position { get; set; }

    [YamlMember(Alias = "sprite")]
    public string Sprite { get; set; }

    [YamlMember(Alias = "state")]
    public List<long> State { get; set; } = new();

    [YamlMember(Alias = "type")]
    public string? Type { get; set; }

    [YamlMember(Alias = "item")]
    public string? Item { get; set; }

    [YamlMember(Alias = "itemset")]
    public List<string> ItemSet { get; set; } = new();

    [YamlMember(Alias = "deny")]
    public List<string> Deny { get; set; } = new();

    [YamlMember(Alias = "allow")]
    public List<string> Allow { get; set; } = new();

    [YamlMember(Alias = "trophy")]
    public string? Trophy { get; set; }
}

public partial class Position
{
    [YamlMember(Alias = "x")]
    public long X { get; set; }

    [YamlMember(Alias = "y")]
    public long Y { get; set; }

    [YamlMember(Alias = "z")]
    public long? Z { get; set; }
}

public partial class Region
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "inletid")]
    public long? Inletid { get; set; }

    [YamlMember(Alias = "type")]
    public string? Type { get; set; }

    [YamlMember(Alias = "peg")]
    public string? Peg { get; set; }

    [YamlMember(Alias = "shopkeeper")]
    public long? Shopkeeper { get; set; }

    [YamlMember(Alias = "shopstyle")]
    public long? Shopstyle { get; set; }

    [YamlMember(Alias = "switch")]
    public bool? Switch { get; set; }
}

public class Vertices
{
    [YamlMember(Alias = "maps")]
    public List<Map> Maps { get; set; } = new();
    [YamlMember(Alias = "rooms")]
    public List<Room> Rooms { get; set; } = new();
}