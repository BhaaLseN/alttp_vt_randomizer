using System.CommandLine;
using System.CommandLine.Invocation;
using App.Graph;

namespace App.Console.Commands;

/**
 * Run randomizer as command.
 */
sealed class Randomize : Command
{
    private readonly Argument<FileInfo> _input_file = new("input_file", "base ROM to randomize");
    private readonly Argument<DirectoryInfo> _output_directory = new("output_directory", "where to place randomized ROM");
    private readonly Option<bool> _unrandomized = new("unrandomized", "do not apply randomization to the ROM");
    private readonly Option<bool> _spoiler = new("spoiler", "generate a spoiler file");
    private readonly Option<HeartBeepSpeed> _heartbeep = new("heartbeep", "set heart beep speed");
    private readonly Option<HeartColor> _heartcolor = new("heartcolor", "set heart color");
    private readonly Option<MenuSpeed> _menuspeed = new("menu-speed", "menu speed");
    private readonly Option<GameGoal> _goal = new("goal", () => GameGoal.Ganon, "set game goal");
    private readonly Option<GameState> _state = new("state", () => GameState.Standard, "set game state");
    private readonly Option<WeaponMode> _weapons = new("weapons", "set weapons mode");
    private readonly Option<GlitchedMode> _glitches = new("glitches", "set glitches");
    private readonly Option<ItemPlacement> _item_placement = new("item_placement", "set item placement rules");
    private readonly Option<DungeonItems> _dungeon_items = new("dungeon_items", "set dungeon item placement");
    private readonly Option<AccessbilityMode> _accessibility = new("accessibility", "set item/location accessibility");
    private readonly Option<ItemPool> _item_pool = new("item_pool", "set item pool");
    private readonly Option<ItemFunctionality> _item_functionality = new("item_functionality", "set item functionality");
    private readonly Option<OnOff> _hints = new("hints", "set hints on or off");
    private readonly Option<bool> _skipmd5 = new("skip-md5", "do not validate md5 of base ROM");
    private readonly Option<bool> _tournament = new("tournament", "enable tournament mode");
    private readonly Option<bool> _nomusic = new("no-music", "mute all music");
    private readonly Option<bool> _norom = new("no-rom", "do not generate output ROM");
    private readonly Option<int> _bulk = new("bulk", "generate multiple ROMs");
    private static readonly string[] _crystalAmount = { "random", "0", "1", "2", "3", "4", "5", "6", "7" };
    private readonly Option<string> _crystals_ganon = new Option<string>("crystals_ganon", () => "7", "set ganon crystal requirement").FromAmong(_crystalAmount);
    private readonly Option<string> _crystals_tower = new Option<string>("crystals_tower", () => "7", "set ganon tower crystal requirement").FromAmong(_crystalAmount);
    private readonly Option<FileInfo> _sprite = new("sprite", "sprite file to change links graphics [zspr format]");
    private readonly Option<bool> _quickswap = new("quickswap", "set quickswap");


    public Randomize()
        : base("randomize", "Generate a randomized ROM.")
    {
        Add(_input_file);
        Add(_output_directory);
        Add(_unrandomized);
        Add(_spoiler);
        Add(_heartbeep);
        Add(_heartcolor);
        Add(_menuspeed);
        Add(_goal);
        Add(_state);
        Add(_weapons);
        Add(_glitches);
        Add(_item_placement);
        Add(_dungeon_items);
        Add(_accessibility);
        Add(_item_pool);
        Add(_item_functionality);
        Add(_hints);
        Add(_skipmd5);
        Add(_tournament);
        Add(_nomusic);
        Add(_norom);
        Add(_bulk);
        Add(_crystals_ganon);
        Add(_crystals_tower);
        Add(_sprite);
        Add(_quickswap);
        this.SetHandler(context => context.ExitCode = handle(context));
    }

#if do_the_rom_stuff
    private byte[]? reset_patch;
#endif

    /**
     * Execute the console command.
     *
     * @return mixed
     */
    public int handle(InvocationContext context)
    {
#if do_the_rom_stuff
        var hasher = new Hashids("local", 15);

        var output_directory = context.ParseResult.GetValueForArgument(_output_directory);
        string filename = Path.Combine(output_directory.FullName, string.Format("alttpr_{0}_{1}_{2}_{{0}}.{{1}}",
            context.ParseResult.GetValueForOption(_glitches),
            context.ParseResult.GetValueForOption(_state),
            context.ParseResult.GetValueForOption(_goal)
        ));

        var input_file = context.ParseResult.GetValueForArgument(_input_file);
        if (!input_file.Exists)
        {
            this.error("Source File not readable");
            return 1;
        }

        if (output_directory.Exists)
        {
            this.error("Target Directory not writable");
            return 2;
        }
#endif

        int bulk = Math.Max(context.ParseResult.GetValueForOption(_bulk), 1);

        for (int i = 0; i < bulk; i++)
        {
#if do_the_rom_stuff
            var rom = new Rom(input_file);
            var hash = hasher.encode((int)(microtime(true) * 1000));

            if (!context.ParseResult.GetValueForOption(_skipmd5) && !rom.checkMD5())
            {
                rom.resize();

                rom.applyPatch(this.resetPatch());
            }

            if (!context.ParseResult.GetValueForOption(_skipmd5) && !rom.checkMD5())
            {
                this.error("MD5 check failed :(");
                return 3;
            }

            var heartColorToUse = context.ParseResult.GetValueForOption(_heartcolor);
            if (heartColorToUse == HeartColor.Random)
            {
                var colorOptions = new[] { HeartColor.Blue, HeartColor.Green, HeartColor.Yellow, HeartColor.Red };
                heartColorToUse = colorOptions.Shuffle().First();
            }
            rom.setHeartColors(heartColorToUse.ToString().ToLowerInvariant());
            rom.setHeartBeepSpeed(context.ParseResult.GetValueForOption(_heartbeep));
            rom.setQuickSwap(context.ParseResult.GetValueForOption(_quickswap));

            // break out for unrandomized base game
            if (context.ParseResult.GetValueForOption(_unrandomized))
            {
                string output_fileV = Path.Combine(output_directory, string.Format("alttp-{0}.sfc", Rom.BUILD));
                rom.save(output_fileV);
                this.info("ROM Saved: {0}", output_fileV);

                return 0;
            }
#endif

            string? crystals_ganonS = context.ParseResult.GetValueForOption(_crystals_ganon);
            if (crystals_ganonS == "random" || !int.TryParse(crystals_ganonS, out int crystals_ganon))
                crystals_ganon = PHP.get_random_int(0, 7);
            string? crystals_towerS = context.ParseResult.GetValueForOption(_crystals_tower);
            if (crystals_towerS == "random" || !int.TryParse(crystals_towerS, out int crystals_tower))
                crystals_tower = PHP.get_random_int(0, 7);
            var logic = context.ParseResult.GetValueForOption(_glitches);

            var randomizer = new Randomizer(new[] { new Dictionary<string, object>()
            {
                { "mode.state", context.ParseResult.GetValueForOption(_state).ToString().ToLowerInvariant() },
                { "itemPlacement", context.ParseResult.GetValueForOption(_item_placement).ToString().ToLowerInvariant() },
                { "dungeonItems", context.ParseResult.GetValueForOption(_dungeon_items).ToString().ToLowerInvariant() },
                { "accessibility", context.ParseResult.GetValueForOption(_accessibility).ToString().ToLowerInvariant() },
                { "goal", context.ParseResult.GetValueForOption(_goal).ToString().ToLowerInvariant() },
                { "crystals.ganon", crystals_ganon },
                { "crystals.tower", crystals_tower },
                { "entrances", "none" },
                { "mode.weapons", context.ParseResult.GetValueForOption(_weapons).ToString().ToLowerInvariant() },
                { "tournament", context.ParseResult.GetValueForOption(_tournament) },
                { "spoil.Hints", context.ParseResult.GetValueForOption(_hints).ToString().ToLowerInvariant() },
                { "logic", logic.ToString() },
                { "item.pool", context.ParseResult.GetValueForOption(_item_pool).ToString().ToLowerInvariant() },
                { "item.functionality", context.ParseResult.GetValueForOption(_item_functionality).ToString().ToLowerInvariant() },
                { "enemizer.bossShuffle", "random" },
                { "enemizer.enemyShuffle", "none" },
                { "enemizer.enemyDamage", "default" },
                { "enemizer.enemyHealth", "default" },
                { "enemizer.potShuffle", "off" },
                //"region.shopSupply" => "shuffled",
                //"equipment" => [
                //    // for testing
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "BossHeartContainer:0",
                //    "ThreeHundredRupees:0",
                //    "ThreeHundredRupees:0",
                //    "PegasusBoots:0",
                //    "BowAndArrows:0",
                //    "ProgressiveSword:0",
                //    "ProgressiveSword:0",
                //    "ProgressiveSword:0",
                //    "ProgressiveGlove:0",
                //    "ProgressiveGlove:0",
                //    "Lamp:0",
                //    "OcarinaActive:0",
                //    "CaneOfSomaria:0",
                //    "FireRod:0",
                //    "MagicMirror:0",
                //    "Flippers:0",
                //    "MoonPearl:0",
                //    "Hookshot:0",
                //    "Hammer:0",
                //],
            } });
            var worlds = randomizer.randomize();
            if (!randomizer.collectItems().has("Triforce:0"))
            {
                throw new Exception("Game Unwinnable");
            }

            // anything beyond here is just writing a rom or whatever...
#if do_the_rom_stuff
            var writer = new RomWriterService();
            writer.writeWorldToRom(worlds[0], rom);

            rom.muteMusic(context.ParseResult.GetValueForOption(_nomusic));
            rom.setMenuSpeed(context.ParseResult.GetValueForOption(_menuspeed).ToString().ToLowerInvariant());

            string output_file = string.Format(filename, hash, "sfc");

            if (!context.ParseResult.GetValueForOption(_norom))
            {
                if (context.ParseResult.GetValueForOption(_sprite) is { Exists: true } spriteFile)
                {
                    this.info("sprite");
                    try
                    {
                        var zspr = new Zspr(spriteFile.FullName);

                        rom.write(0x80000, zspr.getPixelData(), false);
                        rom.write(0xDD308, zspr.getPaletteData()[0..120], false);
                        rom.write(0xDEDF5, zspr.getPaletteData()[120..124], false);
                    }
                    catch (Exception e)
                    {
                        this.error("Sprite not in ZSPR format");

                        return 4;
                    }
                }

                if (context.ParseResult.GetValueForOption(_tournament))
                {
                    rom.setTournamentType("standard");
                    rom.rummageTable();
                }

                rom.updateChecksum();
                rom.save(output_file);

                this.info("ROM Saved: {0}", output_file);
            }

            if (context.ParseResult.GetValueForOption(_spoiler))
            {
                var spoilerService = new SpoilerService();
                var spoiler = spoilerService.getSpoiler(worlds[0]);

                var spoiler_file = string.Format(filename, hash, "json");

                File.WriteAllText(spoiler_file, json_encode(spoiler, JSON_PRETTY_PRINT));
                this.info("Spoiler Saved: {0}", spoiler_file);
            }
#endif
        }

        //this.info(vsprintf("Peak Memory: %d", [
        //    memory_get_peak_usage(true),
        //]));
        return 0;
    }

    private void info(string format, params object[] args)
    {
        System.Console.WriteLine(format, args);
    }
    private void error(string format, params object[] args)
    {
        var previousColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(format, args);
        System.Console.ForegroundColor = previousColor;
    }

#if do_the_rom_stuff
    /**
     * Apply base patch to ROM file.
     *
     * @throws Exception when base patch has no content.
     */
    private byte[] resetPatch()
    {
        if (this.reset_patch is not null)
        {
            return this.reset_patch;
        }

        if (!File.Exists(Rom.getJsonPatchLocation()))
            throw new Exception("base patch not readable");

        var file_contents = File.ReadAllText(Rom.getJsonPatchLocation());
        var patch_left = json_decode(file_contents, true);

        this.reset_patch = patch_merge_minify(patch_left ?? []);

        return this.reset_patch;
    }
#endif
}

public enum HeartBeepSpeed { Off, Half, Quarter, Double, Normal }
public enum HeartColor { Random, Blue, Green, Yellow, Red }
public enum MenuSpeed { }
public enum GameGoal { Ganon, Fast_Ganon, Dungeons, Pedestal }
public enum GameState { Standard, Inverted, Open }
public enum WeaponMode { }
public enum GlitchedMode
{
    NoGlitches,
    None = NoGlitches,
    OverworldGlitches,
    Overworld_Glitches = OverworldGlitches,
    MajorGlitches,
    Major_Glitches = MajorGlitches,
    NoLogic,
    No_Logic = NoLogic,
}
public enum ItemPlacement { }
public enum DungeonItems { }
public enum AccessbilityMode { }
public enum ItemPool { }
public enum ItemFunctionality { }
public enum OnOff { On, Off }