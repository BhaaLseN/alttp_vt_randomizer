using System.Text.RegularExpressions;

namespace App.Graph;

/**
 * Modify the edges of the graph to deal with MoonPearl/Bunny state.
 */
sealed class BunnyGraphifier
{
    private static readonly Dictionary<string, string> ITEM_MAP = new Dictionary<string, string> {
        { "L1Sword", "DarkL1Sword" },
        { "FireRod", "DarkFireRod" },
        { "IceRod", "DarkIceRod" },
        { "Hammer", "DarkHammer" },
        { "Hookshot", "DarkHookshot" },
        { "BowAndArrows", "DarkBowAndArrows" },
        { "Boomerang", "DarkBoomerang" },
        { "PegasusBoots", "DarkPegasusBoots" },
        { "Powder", "DarkPowder" },
        { "ActiveBombos", "DarkActiveBombos" },
        { "ActiveEther", "DarkActiveEther" },
        { "ActiveQuake", "DarkActiveQuake" },
        { "Lamp", "DarkLamp" }, // Lamp for seeing, Lamp for lighting, ??
        { "Shovel", "DarkShovel" },
        { "OcarinaInactive", "DarkOcarinaInactive" },
        { "CaneOfSomaria", "DarkCaneOfSomaria" },
        { "CaneOfByrna", "DarkCaneOfByrna" },
        { "MirrorShield", "DarkMirrorShield" },
        { "Cape", "DarkCape" },
        { "PowerGlove", "DarkPowerGlove" },
        { "TitansMitt", "DarkTitansMitt" },
        { "Flippers", "DarkFlippers" },
        { "BugCatchingNet", "DarkBugCatchingNet" },
        { "RedBoomerang", "DarkRedBoomerang" },
        { "LiftBush", "DarkLiftBush" },
        { "LiftPot", "DarkLiftPot" },
        { "UseBomb", "DarkUseBomb" },
        { "OpenChest", "DarkOpenChest" },
    };

    private World world;

    /**
     * Add all the vertices to the graph for Bunny Dark world.
     *
     * @param World world world to reduce graph for
     * 
     * @return void
     */
    public BunnyGraphifier(World world)
    {
        this.world = world;
        var graph = this.world.graph;

        var world_id = world.id;
        var moonpearl = graph.newVertex(new() {
            { "name", "MoonPearl:" + world_id },
            { "type", "meta" },
        });
        var meta = graph.getVertex("Meta:" + world_id);
        graph.addDirected(meta!, moonpearl, "MoonPearl:" + world_id);

        foreach (var (light_item, dark_item) in ITEM_MAP)
        {
            var dark_vertex = graph.newVertex(new() {
                { "name", $"{dark_item}:{world_id}" },
                { "type", "meta" },
                { "item", Item.get(dark_item, world_id) },
            });

            this.world.graph.addDirected(moonpearl, dark_vertex, $"{light_item}:{world_id}");
        }
    }

    /**
     * Add edges for new dark items required based on dark world and moon pearl.
     */
    public void adjustEdges()
    {
        var dark_nodes =
            from vertex in this.world.graph.getVertices()
            where vertex.moonpearl == true
            select vertex;

        var edge_map = this.world.graph.getEdges()
            .GroupBy(o => o.From)
            .ToDictionary(g => g.Key, g => g.ToList());
        //.ToLookup(o => o.From);

        var work_queue = new Queue<Vertex>(dark_nodes);
        var marked = new HashSet<Vertex>();

        while (work_queue.TryDequeue(out var node))
        {
            if (marked.Contains(node))
            {
                continue;
            }
            marked.Add(node);

            if (!edge_map.ContainsKey(node))
            {
                continue;
            }

            for (int i = 0; i < edge_map[node].Count; ++i)
            {
                var edge = edge_map[node][i];
                var alt_node = edge.To;
                if (alt_node.moonpearl != false)
                {
                    work_queue.Enqueue(alt_node);
                    var group = Regex.Replace(edge.Group, ":\\d+", "");
                    if (!ITEM_MAP.ContainsKey(group))
                    {
                        continue;
                    }

                    edge.Group = ITEM_MAP[group] + ":" + this.world.id;
                }
            }
        }
    }
}
