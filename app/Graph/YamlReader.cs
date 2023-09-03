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
