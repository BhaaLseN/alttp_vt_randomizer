using System.Xml.Linq;
using YamlDotNet.Serialization;

namespace App.Graph;

public class YamlReader
{
    public static string DataRoot { get; set; } = "";

    public static string ItemsPath = "items.yml";

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

    public static Vertices LoadVerticesFromDirectory(string path)
    {
        Vertices result = new();

        foreach (string file in Directory.GetFiles(Path.Join(DataRoot, path), "*.yml", SearchOption.AllDirectories))
        {
            // TODO: Load those XX files with prizepacks and meta nodes
            if (file.Contains("XX-")) continue;

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
    public List<string> In { get; set; }
    [YamlMember(Alias = "out")]
    public List<string> Out { get; set; }
}

public class Entrances
{
    [YamlMember(Alias = "fixed")]
    public List<List<string>> Fixed { get; set; }
    [YamlMember(Alias = "connections")]
    public List<ConnectionGroup> Connections { get; set; }
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
    public Region[] Regions { get; set; }

    [YamlMember(Alias = "entrances")]
    public Entrance[] Entrances { get; set; }

    [YamlMember(Alias = "holes")]
    public Hole[] Holes { get; set; }

    [YamlMember(Alias = "items")]
    public ItemEntry[] Items { get; set; }

    [YamlMember(Alias = "warps")]
    public Warp[] Warps { get; set; }

    [YamlMember(Alias = "mobs")]
    public Entity[] Mobs { get; set; }
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
    public long[] Entranceids { get; set; }
}

public partial class ItemEntry
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "addresses")]
    public long[] Addresses { get; set; }

    [YamlMember(Alias = "type")]
    public string Type { get; set; }

    [YamlMember(Alias = "item")]
    public string? Item { get; set; }

    [YamlMember(Alias = "itemset")]
    public string[] Itemset { get; set; }

    [YamlMember(Alias = "address")]
    public long[] Address { get; set; }
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
    public Region[] Regions { get; set; }

    [YamlMember(Alias = "keydoors")]
    public Keydoor[] Keydoors { get; set; }

    [YamlMember(Alias = "bigkeydoors")]
    public Keydoor[] BigKeydoors { get; set; }

    [YamlMember(Alias = "shutters")]
    public Shutter[] Shutters { get; set; }

    [YamlMember(Alias = "items")]
    public ItemEntry[] Items { get; set; }

    [YamlMember(Alias = "inventory")]
    public InventoryEntry[] Inventory { get; set; }

    [YamlMember(Alias = "pots")]
    public Entity[] Pots { get; set; }

    [YamlMember(Alias = "mobs")]
    public Entity[] Mobs { get; set; }

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
    public string[] Itemset { get; set; }
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
    public long[] State { get; set; }

    [YamlMember(Alias = "type")]
    public string? Type { get; set; }

    [YamlMember(Alias = "item")]
    public string? Item { get; set; }

    [YamlMember(Alias = "itemset")]
    public string[] ItemSet { get; set; }

    [YamlMember(Alias = "deny")]
    public string[] Deny { get; set; }

    [YamlMember(Alias = "allow")]
    public string[] Allow { get; set; }

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