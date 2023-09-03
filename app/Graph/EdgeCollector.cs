namespace App.Graph;

/**
 * Pull data files to create all edges for a given world configuration.
 */
class EdgeCollector
{
    /**
     * Given a particular world (configuration), read all the edge data files
     * and create edges based on the world to connect the vertices.
     *
     * @param World world world to attach preset items to
     */
    public Dictionary<string, DirectedUndirectedPair> getForWorld(World world)
    {
        var edges_data = YamlReader.LoadEdgesFromDirectory("Edges/base");

        switch (world.config<string>("mode.state"))
        {
            case "standard":
                YamlReader.MergeEdges(edges_data, YamlReader.LoadEdgesFromDirectory("Edges/normal"));
                YamlReader.MergeEdges(edges_data, YamlReader.LoadEdgesFromDirectory("Edges/standard"));
                edges_data["fixed"].directed.Add(new() { "start", "Rain - Link's House" });
                edges_data["RescueZelda"].directed.Add(new() { "start", "Sanctuary Hall" });
                // edges_data["OldManFound"]["directed"][] = ["start", "Old Man Cave"];

                break;
            case "inverted":
                YamlReader.MergeEdges(edges_data, YamlReader.LoadEdgesFromDirectory("Edges/inverted"));
                // @todo move these once we have the nodes made
                edges_data["fixed"].directed.Add(new() { "start", "Link's House - Bedroom" });
                edges_data["fixed"].directed.Add(new() { "start", "Dark Sanctuary" });
                // edges_data["OldManFound"]["directed"][] = ["start", "Old Man Cave"];

                break;
            case "open":
            default:
                YamlReader.MergeEdges(edges_data, YamlReader.LoadEdgesFromDirectory("Edges/normal"));
                YamlReader.MergeEdges(edges_data, YamlReader.LoadEdgesFromDirectory("Edges/open"));
                edges_data["fixed"].directed.Add(new() { "start", "Link's House - Bedroom" });
                edges_data["fixed"].directed.Add(new() { "start", "Sanctuary Hall" });
                // edges_data["OldManFound"]["directed"][] = ["start", "Old Man Cave"];
                break;
        }

        foreach (var tech in world.config("tech", Enumerable.Empty<string>()))
        {
            YamlReader.MergeEdges(edges_data, YamlReader.LoadEdgesFromFile("Edges/tech/" + tech + ".yml"));
        }

        var return_data = new Dictionary<string, DirectedUndirectedPair>();
        var world_id = world.id;
        foreach (var (group, edges) in edges_data)
        {
            var name = group + ":" + world_id;

            if (group.Contains('|'))
            {
                name = group.Replace("|", ":" + world_id + "|");
            }

            return_data[name] = new DirectedUndirectedPair
            {
                directed = edges.directed.Select((es) => es.Select((v) => $"{v}:{world_id}").ToList()).ToList(),
                undirected = edges.undirected.Select((es) => es.Select((v) => $"{v}:{world_id}").ToList()).ToList(),
            };
        }

        return return_data;
    }
}
