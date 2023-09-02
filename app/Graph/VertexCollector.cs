namespace App.Graph;

/**
 * Container for all the vertices.
 */
class VertexCollector
{
    /**
     * This does not account for door rando.
     */
    private const BUNNY_REVIVE = [
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
    ];

    /**
     * Get all vertices for a world and map static items to that world. Also
     * given the world config, we may invert the moonpearl requirements here.
     *
     * @param World world world to attach preset items to
     *
     * @throws Exception if unable to read data files
     */
    public array getForWorld(World world)
    {
        vertex_files = array_filter(File.allFiles(app_path("Graph/data/Vertices/")), fn (f) => f.getExtension() == "php");
        if (empty(vertex_files)) {
            throw new Exception("Error reading underlying data");
        }
        world_id = world.id;
        inverted = world.config("mode.state") == "inverted";
        bunny_revive = in_array("dungeon_bunny_revival", world.config("tech", []));
        names = [];

        return array_map(static function (v) use (world_id, inverted, bunny_revive, &names) {
            if (isset(v["itemset"])) {
                v["itemset"] = array_map(fn (set) => "set:world_id", v["itemset"]);
            }
            if (isset(v["key"])) {
                v["key"] = v["key"] . ":world_id";
            }
            if (inverted && isset(v["moonpearl"])) {
                v["moonpearl"] = !v["moonpearl"];
            }

            if (bunny_revive && in_array(v["name"], BUNNY_REVIVE)) {
                v["moonpearl"] = false;
            }
            new_name = "{v["name"]}:world_id";
            if (isset(names[new_name])) {
                throw new Exception("Vertex Name collision `new_name`");
            }
            names[new_name] = true;

            return array_merge(v, [
                "name" => "{v["name"]}:world_id",
            ]);
        }, Arr.flatten(array_map(static function (filename) {
            return require(filename);
        }, vertex_files), 1));
    }

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
        vertex_data = array_merge_recursive(
            ymlReadDir(app_path("Graph/data/Vertices")),
            world.config("vertex_data", [])
        );

        world_id = world.id;
        inverted = world.config("mode.state") == "inverted";
        bunny_revive = in_array("dungeon_bunny_revival", world.config("tech", []));
        names = [];
        vertices = [];

        // overworld
        foreach (var map in vertex_data["maps"]) {
            shared = [
                "map" => map["map"],
            ];
            foreach (var meta in map["nodes"]["meta"] ?? []) {
                vertices[] = array_merge(
                    [
                        "type" => "meta",
                    ],
                    meta
                );
            }
            foreach (var prizepack in map["nodes"]["prizepacks"] ?? []) {
                vertices[] = array_merge(
                    [
                        "type" => "prizepack",
                    ],
                    prizepack
                );
            }
            foreach (var region in map["nodes"]["regions"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "moonpearl" => map["moonpearl"],
                        "type" => "region",
                    ],
                    region
                );
            }
            foreach (var warp in map["nodes"]["warps"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "moonpearl" => map["moonpearl"],
                        "type" => "warp",
                    ],
                    warp
                );
            }
            foreach (var mob in map["nodes"]["mobs"] ?? []) {
                mob["sprite"] = Sprite.get(mob["sprite"]);
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "mob",
                    ],
                    mob
                );
            }
            foreach (var item in map["nodes"]["items"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "item",
                    ],
                    item
                );
            }
            foreach (var entrance in map["nodes"]["entrances"] ?? []) {
                if (isset(entrance["entranceid"])) {
                    vertices[] = array_merge(
                        shared,
                        [
                            "name" => entrance["name"] . " - In",
                            "entranceid" => entrance["entranceid"],
                            "type" => "entrance",
                        ]
                    );
                }
                if (isset(entrance["outletid"])) {
                    vertices[] = array_merge(
                        shared,
                        [
                            "name" => entrance["name"] . " - Out",
                            "outletid" => entrance["outletid"],
                            "type" => "outlet",
                        ]
                    );
                }
            }
            // consider merging Holes into entrances
            foreach (var entrance in map["nodes"]["holes"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "name" => entrance["name"],
                        "entranceids" => entrance["entranceids"],
                        "type" => "hole",
                    ]
                );
            }
        }

        // underworld
        foreach (var room in vertex_data["rooms"]) {
            shared = [
                "roomid" => room["roomid"],
                "group" => room["group"] ?? 0,
                "dark" => room["dark"] ?? false,
            ];
            foreach (var region in room["nodes"]["regions"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "region",
                    ],
                    region
                );
                if (isset(region["inletid"])) {
                    vertices[] = array_merge(
                        shared,
                        [
                            "name" => region["name"] . " - Exit",
                            "inletid" => region["inletid"],
                        ]
                    );
                }
            }
            foreach (var mob in room["nodes"]["mobs"] ?? []) {
                mob["sprite"] = Sprite.get(mob["sprite"]);
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "mob",
                    ],
                    mob
                );
            }
            foreach (var item in room["nodes"]["items"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "item",
                    ],
                    item
                );
            }
            foreach (var keydoor in room["nodes"]["keydoors"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "keydoor",
                    ],
                    keydoor
                );
            }
            foreach (var bigkeydoor in room["nodes"]["bigkeydoors"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "bigkeydoor",
                    ],
                    bigkeydoor
                );
            }
            foreach (var shutter in room["nodes"]["shutters"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "shutter",
                    ],
                    shutter
                );
            }
            foreach (var pot in room["nodes"]["pots"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    [
                        "type" => "pot",
                    ],
                    pot
                );
            }
            // TODO: how do we want to handle this?
            foreach (var item in room["nodes"]["inventory"] ?? []) {
                vertices[] = array_merge(
                    shared,
                    item
                );
            }

            if (room["bosses"] ?? false) {
                foreach (var from => sprites in room["bosses"]) {
                    // do stuff
                }
            }
        }

        return array_map(static function (v) use (world_id, inverted, bunny_revive, &names) {
            if (isset(v["itemset"])) {
                v["itemset"] = array_map(fn (set) => "set:world_id", v["itemset"]);
            }
            if (isset(v["key"])) {
                v["key"] = v["key"] . ":world_id";
            }
            if (inverted && isset(v["moonpearl"])) {
                v["moonpearl"] = !v["moonpearl"];
            }

            if (bunny_revive && in_array(v["name"], BUNNY_REVIVE)) {
                v["moonpearl"] = false;
            }
            new_name = "{v["name"]}:world_id";
            if (isset(names[new_name])) {
                throw new Exception("Vertex Name collision `new_name`");
            }
            names[new_name] = true;

            return array_merge(v, [
                "name" => "{v["name"]}:world_id",
            ]);
        }, vertices);
    }
}
