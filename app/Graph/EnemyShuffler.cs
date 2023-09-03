namespace App.Graph;

/**
 * Modify the edges of the graph to shuffle entrances.
 */
sealed class EnemyShuffler
{
    private static readonly Dictionary<string, string[]> CHALLENGE_ROOMS = new Dictionary<string, string[]> {
        { "Mini Moldorm Cave Entrance", new[] {
            "Mini Moldorm Cave - Mini Moldorm 1",
            "Mini Moldorm Cave - Mini Moldorm 2",
            "Mini Moldorm Cave - Mini Moldorm 3",
            "Mini Moldorm Cave - Mini Moldorm 4",
        }},
        { "Turtle Rock - Bomb Ambush", new[] {
            "Turtle Rock - Bomb Ambush - Zol 1",
            "Turtle Rock - Bomb Ambush - Zol 2",
            "Turtle Rock - Bomb Ambush - Zol 3",
        }},
        { "Palace of Darkness - Turtle Party", new[] {
            "Palace of Darkness - Turtle Party - Terrorpin TR",
            "Palace of Darkness - Turtle Party - Terrorpin TL",
            "Palace of Darkness - Turtle Party - Terrorpin R",
            "Palace of Darkness - Turtle Party - Terrorpin L",
            "Palace of Darkness - Turtle Party - Terrorpin BR",
            "Palace of Darkness - Turtle Party - Terrorpin BL",
        }},
        { "Ice Palace - Entrance", new[] {
            "Ice Palace - Entrance - Freezor",
        }},
        { "Ice Palace - Bari Key", new[] {
            "Ice Palace - Bari Key - Top Bari",
            "Ice Palace - Bari Key - Middle Bari",
            "Ice Palace - Bari Key - Bottom Bari",
        }},
        { "Palace of Darkness - Mimics 2", new[] {
            "Palace of Darkness - Mimics 2 - Red Eyegore",
            "Palace of Darkness - Mimics 2 - Green Eyegore L",
            "Palace of Darkness - Mimics 2 - Green Eyegore R",
        }},
        // "Ganon's Tower - Ice Armos" // covered by boss shuffle code for now.
        { "Turtle Rock - Pokeys 2", new[] {
            "Turtle Rock - Pokeys 2 - Pokey 1",
            "Turtle Rock - Pokeys 2 - Pokey 2",
            "Turtle Rock - Pokeys 2 - Medusa 3",
        }},
        { "Tower Of Hera - Beetle Gate", new[] {
            "Tower Of Hera - Beetle Gate - Hardhat Beetle R",
            "Tower Of Hera - Beetle Gate - Hardhat Beetle L",
            "Tower Of Hera - Beetle Gate - Hardhat Beetle B",
        }},
        { "Ice Palace - Stalfos Ambush", new[] {
            "Ice Palace - Stalfos Ambush - Stalfos Knight T",
            "Ice Palace - Stalfos Ambush - Stalfos Knight B",
        }},
        { "Thieves' Town - Basement Block Totems", new[] {
            "Thieves' Town - Basement Block Totems - Red Zazak",
            "Thieves' Town - Basement Block Totems - Blue Zazak",
            "Thieves' Town - Basement Block Totems - Stalfos",
        }},
        { "Palace of Darkness - Mimics 1", new[] {
            "Palace of Darkness - Mimics 1 - Red Eyegore",
            "Palace of Darkness - Mimics 1 - Green Eyegore L",
            "Palace of Darkness - Mimics 1 - Green Eyegore R",
        }},
        { "Desert Palace - Popo Genocide", new[] {
            "Desert Palace - Popo Genocide - Beamos",
            "Desert Palace - Popo Genocide - Popo TR",
            "Desert Palace - Popo Genocide - Popo TL",
            "Desert Palace - Popo Genocide - Popo BR",
            "Desert Palace - Popo Genocide - Popo BL",
        }},
        { "Ganon's Tower - Mimics 1", new[] {
            "Ganon's Tower - Mimics 1 - Statue",
            "Ganon's Tower - Mimics 1 - Red Eyegore 2",
            "Ganon's Tower - Mimics 1 - Spike Trap 1",
            "Ganon's Tower - Mimics 1 - Spike Trap 2",
            "Ganon's Tower - Mimics 1 - Red Eyegore 3",
        }},
        { "Ganon's Tower - Mimics 2", new[] {
            "Ganon's Tower - Mimics 2 - Red Eyegore T",
            "Ganon's Tower - Mimics 2 - Beamos TR",
            "Ganon's Tower - Mimics 2 - Beamos BL",
            "Ganon's Tower - Mimics 2 - Red Eyegore B",
        }},
        { "Ganon's Tower - Gauntlet 1", new[] {
            "Ganon's Tower - Gauntlet 1 - Red Zazak",
            "Ganon's Tower - Gauntlet 1 - Stalfos L",
            "Ganon's Tower - Gauntlet 1 - Blue Zazak",
            "Ganon's Tower - Gauntlet 1 - Stalfos R",
        }},
        { "Ganon's Tower - Gauntlet 2", new[] {
            "Ganon's Tower - Gauntlet 2 - Beamos",
            "Ganon's Tower - Gauntlet 2 - Stalfos T",
            "Ganon's Tower - Gauntlet 2 - Stalfos L",
            "Ganon's Tower - Gauntlet 2 - Stalfos B",
        }},
        { "Ganon's Tower - Gauntlet 3", new[] {
            "Ganon's Tower - Gauntlet 3 - Beamos TL",
            "Ganon's Tower - Gauntlet 3 - Blue Zazak TR",
            "Ganon's Tower - Gauntlet 3 - Blue Zazak BL",
            "Ganon's Tower - Gauntlet 3 - Blue Zazak B",
            "Ganon's Tower - Gauntlet 3 - Beamos BR",
        }},
        { "Ganon's Tower - Gauntlet 4", new[] {
            "Ganon's Tower - Gauntlet 4 - Red Zazak TL",
            "Ganon's Tower - Gauntlet 4 - Beamos TR",
            "Ganon's Tower - Gauntlet 4 - Beamos BL",
            "Ganon's Tower - Gauntlet 4 - Red Zazak BR",
        }},
        { "Ganon's Tower - Gauntlet 5", new[] {
            "Ganon's Tower - Gauntlet 5 - Medusa",
            "Ganon's Tower - Gauntlet 5 - Beamos",
            "Ganon's Tower - Gauntlet 5 - Stalfos",
            "Ganon's Tower - Gauntlet 5 - Red Zazak",
            "Ganon's Tower - Gauntlet 5 - Spark",
        }},
        // "Ganon's Tower - Lanmolas" // covered by boss shuffle code for now.
        { "Ice Palace - Penguin Line Up", new[] {
            "Ice Palace - Penguin Line Up - Pengator 1",
            "Ice Palace - Penguin Line Up - Pengator 2",
            "Ice Palace - Penguin Line Up - Pengator 3",
            "Ice Palace - Penguin Line Up - Pengator 4",
            "Ice Palace - Penguin Line Up - Pengator 5",
        }},
        { "Hyrule Castle - Basement Trap", new[] {
            "Hyrule Castle - Basement Trap - Green Guard"
        }},
        { "Hyrule Castle - Basement Statue Trap", new[] {
            "Hyrule Castle - Basement Statue Trap - Guard Key",
        }},
        // skipping room 0x75 (someone else can impl)
        // skipping room 0x7b (someone else can impl)
        // skipping room 0x7d (someone else can impl)
        // skipping room 0x7e (freezor room chest)
        { "Desert Palace - Compass Room", new[] {
            "Desert Palace - Compass Room - Popo TL",
            "Desert Palace - Compass Room - Popo TR",
            "Desert Palace - Compass Room - Popo BL",
            "Desert Palace - Compass Room - Beamos",
        }},
        { "Ganon's Tower - Tile Room", new[] { // chest
            "Ganon's Tower - Tile Room - Tiles",
            "Ganon's Tower - Tile Room - Yomo Medusa T",
            "Ganon's Tower - Tile Room - Antifairy",
            "Ganon's Tower - Tile Room - Bunny Beam",
            "Ganon's Tower - Tile Room - Yomo Medusa B",
            "Ganon's Tower - Tile Room - Wallmaster",
        }},
        { "Ganon's Tower - Wizzrobes 1", new[] {
            "Ganon's Tower - Wizzrobes 1 - Wizzrobe TL",
            "Ganon's Tower - Wizzrobes 1 - Wizzrobe TR",
            "Ganon's Tower - Wizzrobes 1 - Wizzrobe B",
        }},
        { "Ganon's Tower - Wizzrobes 2", new[] {
            "Ganon's Tower - Wizzrobes 2 - Wizzrobe TL",
            "Ganon's Tower - Wizzrobes 2 - Wizzrobe TR",
            "Ganon's Tower - Wizzrobes 2 - Spike Trap",
            "Ganon's Tower - Wizzrobes 2 - Wizzrobe BL",
            "Ganon's Tower - Wizzrobes 2 - Wizzrobe BR",
        }},
        { "Eastern Palace - West Wing - Stalfos Ambush", new[] {
            "Eastern Palace - West Wing - Stalfos Ambush - Stalfos Party",
        }},
        { "Agahnims Tower - Circle of Pots", new[] {
            "Agahnims Tower - Circle of Pots - Keese TL",
            "Agahnims Tower - Circle of Pots - Keese TR",
            "Agahnims Tower - Circle of Pots - Red Spear Guard C",
            "Agahnims Tower - Circle of Pots - Red Spear Guard TR",
        }},
        { "Agahnims Tower - Guards", new[] {
            "Agahnims Tower - Guards - Red Spear Guard B",
            "Agahnims Tower - Guards - Red Spear Guard C",
        }},
        { "Agahnims Tower - Throwing Guards", new[] {
            "Agahnims Tower - Throwing Guards - Keese L",
            "Agahnims Tower - Throwing Guards - Keese R",
            "Agahnims Tower - Throwing Guards - Red Javelin Guard L",
            "Agahnims Tower - Throwing Guards - Red Javelin Guard R",
        }},
        { "Misery Mire - Sluggula Cross", new[] {
            "Misery Mire - Sluggula Cross - Sluggula TL",
            "Misery Mire - Sluggula Cross - Sluggula TR",
            "Misery Mire - Sluggula Cross - Antifairy",
            "Misery Mire - Sluggula Cross - Sluggula BL",
            "Misery Mire - Sluggula Cross - Sluggula BR",
        }},
        // skipping room 0xb6 (someone else can impl tile room)
        // skipping 0xb8 (antifairy circle on pot, need to consider further)
        { "Agahnims Tower - Dark Key Guards", new[] {
            "Agahnims Tower - Dark Key Guards - Blue Guard",
            "Agahnims Tower - Dark Key Guards - Blue Archer R",
            "Agahnims Tower - Dark Key Guards - Blue Archer L",
        }},
        { "Misery Mire - Mire 2", new[] {
            "Misery Mire - Mire 2 - Wizzrobe T",
            "Misery Mire - Mire 2 - Popo TR",
            "Misery Mire - Mire 2 - Wizzrobe TL",
            "Misery Mire - Mire 2 - Wizzrobe TR",
            "Misery Mire - Mire 2 - Beamos",
            "Misery Mire - Mire 2 - Popo C",
            "Misery Mire - Mire 2 - Popo L",
            "Misery Mire - Mire 2 - Wizzrobe B",
            "Misery Mire - Mire 2 - Popo BL",
            "Misery Mire - Mire 2 - Popo BR",
        }},
        { "Eastern Palace - Kill Room 1", new[] {
            "Eastern Palace - Kill Room 1 - Red Eyegore",
            "Eastern Palace - Kill Room 1 - Stalfos T",
            "Eastern Palace - Kill Room 1 - Stalfos B",
        }},
        { "Eastern Palace - Kill Room 2", new[] {
            "Eastern Palace - Kill Room 2 - Red Eyegore L",
            "Eastern Palace - Kill Room 2 - Red Eyegore R",
            "Eastern Palace - Kill Room 2 - Popo B TL",
            "Eastern Palace - Kill Room 2 - Popo B TR",
            "Eastern Palace - Kill Room 2 - Popo B LT",
            "Eastern Palace - Kill Room 2 - Popo B RT",
            "Eastern Palace - Kill Room 2 - Popo LB",
            "Eastern Palace - Kill Room 2 - Popo RB",
        }},
        { "Agahnims Tower - Chain Guards", new[] {
            "Agahnims Tower - Chain Guards - Ball 'n' Chain Guard L",
            "Agahnims Tower - Chain Guards - Ball 'n' Chain Guard R",
        }},
        { "Agahnims Tower - Knife Guards", new[] {
            "Agahnims Tower - Knife Guards - Charging Blue Guard T",
            "Agahnims Tower - Knife Guards - Charging Blue Guard B",
        }},
        { "Swamp Palace - Entrance Ledge", new[] {
            "Swamp Palace - Entrance Ledge - Kyameron",
            "Swamp Palace - Entrance Ledge - Hover 1",
            "Swamp Palace - Entrance Ledge - Hover 2",
            "Swamp Palace - Entrance Ledge - Hover 3",
            "Swamp Palace - Entrance Ledge - Spike Trap",
        }},
        { "Mimic Cave Entrance", new[] {
            "Mimic Cave Entrance - Green Eyegore TL",
            "Mimic Cave Entrance - Green Eyegore TR",
            "Mimic Cave Entrance - Green Eyegore CR",
            "Mimic Cave Entrance - Green Eyegore BR",
        }},
    };
    private static readonly byte[] PIT_ROOMS = {
        0x01,
    };
    private static readonly short[] TONGUE_ROOMS = {
        0x0004,
        0x00CE,
        0x003F,
    };

    // remove this and use the same alg for UW enemies
    private readonly Dictionary<byte, byte?[]> OW_MAP_SHEETS = new() {
        { 0x02, new byte?[] { 0x0F, null, 0x4A, null } },
        { 0x03, new byte?[] { null, null, 0x12, 0x10 } },
        { 0x14, new byte?[] { 0x0E, null, null, null } },
        { 0x18, new byte?[] { 0x4F, 0x49, 0x4A, 0x50 } },
        { 0x1B, new byte?[] { null, null, null, 0x1D } },
        { 0x30, new byte?[] { null, null, 0x12, null } },
        { 0x3A, new byte?[] { null, null, null, 0x11 } }, // this should be handled?
        { 0x4F, new byte?[] { null, null, 0x18, null } },
        { 0x5E, new byte?[] { null, null, null, 0x19 } },
    };

    private readonly Dictionary<string, List<string>> defeats = new();
    private readonly array challenge_enemies;
    private readonly List<string> no_place_sprites;

    private readonly World world;

    /**
     * Add all the vertices to the graph for this region.
     * 
     * 1) Rearrange all the sprite sheet sets
     * 2) find out which sprites can be placed with each set now
     * 3) pick a random sheet for a room
     * 4) pick random sprites for room
     *
     * @param World world world to reduce graph for
     *
     * @return void
     */
    public EnemyShuffler(World world)
    {
        this.world = world;
        this.defeats = Yaml.parse(file_get_contents(app_path("Graph/data/Enemizer/enemies.yml"))) ?? [];
        this.challenge_enemies = Yaml.parse(file_get_contents(app_path("Graph/data/Enemizer/challenge.yml"))) ?? [];
        this.no_place_sprites = Yaml.parse(file_get_contents(app_path("Graph/data/Enemizer/noplace.yml"))) ?? [];

        int world_id = this.world.id;
        foreach (var token in this.defeats.Keys)
        {
            this.world.graph.newVertex(new()
            {
                { "name", $"{token}:{world_id}" },
                { "type", "meta" },
                { "item", Item.get(token, world_id) },
            });
        }

        var enemies = world.getLocationsOfType("mob");

        /** @var array enemy_rooms */
        var enemy_rooms = enemies.ToLookup((enemy) => enemy.roomid);
        /** @var array enemy_ows */
        var enemy_ows = enemies.ToLookup((enemy) => enemy.map);

        // Set up sprite sheets
        var sheetable_sprites = Sprite.all().Where(
            (s) => s.sheets.Count((v) => v != null) != 0
        );
        // sprites that can be moved to any room as they don"t have any sheet
        // requirements
        var nosheet_sprites = Sprite.all().Where(
            (s) => s.sheets.Count((v) => v != null) == 0
                && this.no_place_sprites.Contains(s.Name)
        );

        var sheet_sets = Enumerable.Repeat(new byte?[] { null, null, null, null }, 124).ToArray();
        var sheets_to_sprites = Enumerable.Repeat(nosheet_sprites.ToArray(), 124).ToArray();

        var room_sheets = new int[0x180];
        var ow_sheets = new int[0x80];

        // deal with OW required sheet sets (j carries over to next block, it"s
        // important for filling the array properly)
        int j = 0;
        foreach (var (map, ow_set) in OW_MAP_SHEETS)
        {
            ow_sheets[map] = j;
            sheet_sets[j] = ow_set;
            j++;
        }

        if (world.config<string>("enemizer.enemyShuffle") == "none")
        {
            for (int i = 0; i < 0x80; i++)
            {
                if (enemy_ows[i].Count() == 0)
                {
                    ow_sheets[i] = 0xFF;
                    continue;
                }
                if (ow_sheets[i] != 0x00) // TODO: is zero valid?
                {
                    var fixed_set = [];
                    var enemySprites = enemy_ows[i].Select((e) => e.sprite);
                    foreach (var sprite in enemySprites)
                    {
                        var filtered_sprite = sprite.sheets.Where((v) => v != null);
                        var filtered_set = fixed_set.Where((v) => v != null);
                        fixed_set = array_replace([null, null, null, null], filtered_set, filtered_sprite);
                    }
                    if (!fixed_set.Any((v) => v != null))
                    {
                        continue;
                    }
                    bool foundSheet = false;
                    for (int k = 0; k < j; ++k)
                    {
                        if (
                            (fixed_set[0] == null || sheet_sets[k][0] == fixed_set[0])
                            && (fixed_set[1] == null || sheet_sets[k][1] == fixed_set[1])
                            && (fixed_set[2] == null || sheet_sets[k][2] == fixed_set[2])
                            && (fixed_set[3] == null || sheet_sets[k][3] == fixed_set[3])
                        )
                        {
                            ow_sheets[i] = k;
                            foundSheet = true;
                            break;
                        }
                    }
                    if (foundSheet)
                        continue;
                    sheet_sets[j] = fixed_set;
                    ow_sheets[i] = j;
                    ++j;
                }
            }
        }

        // force fixed room sets! If we have a few "no move" sprites in a room
        // we need to guarantee that a sheet set exists for that room to look
        // correct.
        for (int i = 0; i < 0x180; i++)
        {
            if (!enemy_rooms[i].Any())
            {
                continue;
            }
            var filtered = enemy_rooms[i].Where(
                 (e) => world.config<string>("enemizer.enemyShuffle") == "none"
                    || this.no_place_sprites.Contains(e.sprite.Name)
            );
            if (!filtered.Any())
            {
                continue;
            }
            var fixed_set = [];
            var enemySprites = filtered.Select((e) => e.sprite);
            foreach (var sprite in enemySprites)
            {
                var filtered_sprite = sprite.sheets.Where((v) => v != null);
                var filtered_set = fixed_set.Where((v) => v != null);
                fixed_set = array_replace([null, null, null, null], filtered_set, filtered_sprite);
            }
            if (!fixed_set.Any((v) => v != null))
            {
                continue;
            }
            bool foundSheet = false;
            // potential bug here where fixed set is full, we may end up making 2+ copies in table
            if (world.config<string>("enemizer.enemyShuffle") == "none" || Random.Shared.Next(1) == 1)
            {
                for (int k = 0; k < j; ++k)
                {
                    if (
                        (fixed_set[0] == null || sheet_sets[k][0] == fixed_set[0])
                        && (fixed_set[1] == null || sheet_sets[k][1] == fixed_set[1])
                        && (fixed_set[2] == null || sheet_sets[k][2] == fixed_set[2])
                        && (fixed_set[3] == null || sheet_sets[k][3] == fixed_set[3])
                    )
                    {
                        room_sheets[i] = k;
                        foundSheet = true;
                        break;
                    }
                }
            }
            if (foundSheet)
                continue;

            sheet_sets[j] = fixed_set;
            room_sheets[i] = j;
            ++j;
        }

        // fill in all sheet sets with valid layouts for sprites
        for (int i = 0; i < 124; ++i)
        {
            while (sheet_sets[i].Contains(null))
            {
                var sprite = sheetable_sprites.Shuffle().First();
                if (
                    (sprite.sheets[0] == null || sheet_sets[i][0] == null)
                    && (sprite.sheets[1] == null || sheet_sets[i][1] == null)
                    && (sprite.sheets[2] == null || sheet_sets[i][2] == null)
                    && (sprite.sheets[3] == null || sheet_sets[i][3] == null)
                )
                {
                    var filtered_sprite = sprite.sheets.Where((v) => v != null);
                    var filtered_set = sheet_sets[i].Where((v) => v != null);
                    sheet_sets[i] = array_replace([null, null, null, null], filtered_set, filtered_sprite);
                }
            }
        }

        // find all the sprites that can be placed validly with a particular sheet set.
        foreach (var sprite in sheetable_sprites)
        {
            foreach (var (i, set) in sheet_sets.Select((s, i) => (i, s))) {
                if (
                    world.config<string>("enemizer.enemyShuffle") != "none"
                    && (sprite.sheets[0] == null || set[0] == sprite.sheets[0])
                    && (sprite.sheets[1] == null || set[1] == sprite.sheets[1])
                    && (sprite.sheets[2] == null || set[2] == sprite.sheets[2])
                    && (sprite.sheets[3] == null || set[3] == sprite.sheets[3])
                    && !this.no_place_sprites.Contains(sprite.Name)
                )
                {
                    sheets_to_sprites[i][sprite.Name] = sprite;
                }
            }
        }

        var all_challenge_enemies = CHALLENGE_ROOMS.Values.Select((e) => $"{e}:{this.world.id}");
        for (int i = 0; i < 0x180; i++)
        {
            int sheet;
            if (!enemy_rooms[i].Any())
            {
                room_sheets[i] = 0x00;
                continue;
            }
            if (room_sheets[i] == 0x00)  // TODO: is zero valid?
            {
                do
                {
                    sheet = Random.Shared.Next(sheets_to_sprites.Length);
                } while (!sheets_to_sprites[sheet].Any());
                room_sheets[i] = sheet;
            }
            sheet = room_sheets[i];
            var filtered_placable = enemy_rooms[i].Where(
                (e) => world.config<string>("enemizer.enemyShuffle") != "none"
                    && !this.no_place_sprites.Contains(e.sprite.Name)
            );
            if (!filtered_placable.Any())
            {
                continue;
            }
            foreach (var enemy in filtered_placable)
            {
                Sprite newSprite;
                if (all_challenge_enemies.Contains(enemy.name))
                {
                    newSprite = sheets_to_sprites[sheet].Where(
                        (sprite) => this.challenge_enemies.Contains(sprite.Name)
                    ).Shuffle().FirstOrDefault();
                    if (newSprite is null)
                    {
                        throw new Exception("ugh");
                    }
                }
                else
                {
                    newSprite = get_random_element(sheets_to_sprites[sheet]);
                }
                //Log.debug(vsprintf("%s: placing %s", [
                //    enemy.name,
                //    newSprite.getNiceName(),
                //]));
                enemy.sprite = newSprite;
            }
        }

        for (int i = 0; i < 0x80; i++)
        {
            if (!enemy_ows[i].Any())
            {
                ow_sheets[i] = 0xFF;
                continue;
            }
            if (ow_sheets[i] == 0x00) // TODO: is zero valid?
            {
                do
                {
                    sheet = get_random_key(sheets_to_sprites);
                } while (!sheets_to_sprites[sheet].Any());
                ow_sheets[i] = sheet;
            }
            sheet = ow_sheets[i];
            var filtered_placable = enemy_ows[i].Where(
                (e) => world.config<string>("enemizer.enemyShuffle") != "none"
                    && !this.no_place_sprites.Contains(e.sprite.Name)
            );
            if (count(filtered_placable) == 0)
            {
                continue;
            }
            foreach (var enemy in filtered_placable)
            {
                var newSprite = get_random_element(sheets_to_sprites[sheet]);
                //Log.debug(vsprintf("%s: placing %s", [
                //    enemy.name,
                //    newSprite.getNiceName(),
                //]));
                enemy.sprite = newSprite;
            }
        }
        ksort(ow_sheets);
        ow_sheets = array_merge(
            array_slice(ow_sheets, 0, 0x40),
            array_slice(ow_sheets, 0, 0x40),
            array_slice(ow_sheets, 0, 0x40),
            array_slice(ow_sheets, 0x40, 0x80),
        );

        // pick random sheets where we have options
        sheet_sets = sheet_sets.Select(
            (set) => set.Select(
                (sheet) => is_array(sheet) ? get_random_element(sheet) : sheet
            )
        );

        world.sprite_sheets = new()
        {
            { "underworld", room_sheets.Select((s) => s - 0x40).ToArray() },
            { "overworld", ow_sheets },
            { "sets", sheet_sets },
        };
    }

    /**
     * Swap Edges based on new enemy locations settings.
     * 
     * @todo is this going to have a problem with the BunnyGraphifier??
     */
    public void adjustEdges()
    {
        var from = this.world.getLocation("Meta");
        int world_id = this.world.id;
        foreach (var (token, items) in this.defeats)
        {
            var to = this.world.graph.getVertex($"{token}:{world_id}");
            foreach (var item in items)
            {
                this.world.graph.addDirected(from, to, $"{item}:{world_id}");
            }
        }

        foreach (var (room, enemies) in CHALLENGE_ROOMS)
        {
            from = this.world.getLocation(room);
            foreach (var enemy in enemies)
            {
                var to = this.world.getLocation(enemy);
                if (to is null)
                {
                    throw new Exception($"Cannot find location for {enemy}: {to}");
                }
                string take = $"Defeat{to.sprite.Name}:{world_id}";
                this.world.graph.addDirected(from, to, take);
            }
        }
    }
}
