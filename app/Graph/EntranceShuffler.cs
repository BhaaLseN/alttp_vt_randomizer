namespace App.Graph;

/**
 * Modify the edges of the graph to shuffle entrances.
 */
sealed class EntranceShuffler
{
    private readonly array definition;

    /**
     * Add all the vertices to the graph for this region.
     *
     * @param World world world to reduce graph for
     *
     * @return void
     */
    public function __construct(private World world)
    {
        definition = match (world.config("entrances")) {
            "simple" => "simple.yml",
            "restricted" => "vanilla.yml",
            "full" => "vanilla.yml",
            "crossed" => "vanilla.yml",
            "insanity" => "vanilla.yml",
            "none" => "vanilla.yml",
            default => world.config("entrances") ?? "vanilla.yml",
        };

        if (is_string(definition) && is_readable(app_path("Graph/data/Edges/entrances/definition"))) {
            definition = Yaml.parse(file_get_contents(app_path("Graph/data/Edges/entrances/definition")));
        }

        if (!is_array(definition)) {
            throw new Exception("No valid entrance definition found");
        }

        this.definition = definition;
    }

    /**
     * Connect Entrances, Exits, Outlets, and rooms based on World settings.
     */
    public void adjustEdges()
    {
        world_id = this.world.id;
        foreach (var connection in this.definition["fixed"]) {
            from = this.world.graph.getVertex(connection[0] . ":world_id");
            to = this.world.graph.getVertex(connection[1] . ":world_id");
            if (
                !from instanceof Vertex
                || !to instanceof Vertex
            ) {
                dd([
                    "ER",
                    connection,
                    from,
                    to,
                ]);
            }
            this.world.graph.addDirected(from, to, "fixed:world_id");
        }

        foreach (var group in this.definition["connections"]) {
            ins = fy_shuffle(group["in"]);
            outs = fy_shuffle(group["out"]);
            if (count(ins) != count(outs)) {
                throw new Exception("Entrance count mismatch");
            }

            while (count(ins)) {
                in_items = Arr.wrap(array_pop(ins));
                out_items = Arr.wrap(array_pop(outs));
                if (count(in_items) != count(out_items)) {
                    throw new Exception("Entrance sub-count mismatch");
                }
                foreach (var offset => in in in_items) {
                    out = out_items[offset];
                    from = this.world.graph.getVertex(in . ":world_id");
                    to = this.world.graph.getVertex(out . ":world_id");
                    this.world.graph.addDirected(from, to, "fixed:world_id");
                }
            }
        }
    }
}
