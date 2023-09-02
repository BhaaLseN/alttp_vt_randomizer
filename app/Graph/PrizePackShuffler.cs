namespace App.Graph;

/**
 * Modify Prizepacks based on configuration.
 */
sealed class PrizePackShuffler
{
    private readonly World world;
    /**
     * @param World world 
     *
     * @return void
     */
    public PrizePackShuffler(World world)
    {
        this.world = world;
    }

    /**
     * Pick items for each prize pack.
     */
    public void adjustEdges()
    {
        var prizepacks = this.world.getLocationsOfType("prizepack");

        if (!this.world.config("customPrizePacks", false))
        {
            var random_vanilla_packs = new Stack<string>(new[]
            {
                new[] { "Heart", "Heart", "Heart", "Heart", "RupeeGreen", "Heart", "Heart", "RupeeGreen" },
                new[] { "RupeeBlue", "RupeeGreen", "RupeeBlue", "RupeeRed", "RupeeBlue", "RupeeGreen", "RupeeBlue", "RupeeBlue" },
                new[] { "MagicRefillFull", "MagicRefillSmall", "MagicRefillSmall", "RupeeBlue", "MagicRefillFull", "MagicRefillSmall", "Heart", "MagicRefillSmall" },
                new[] { "BombRefill1", "BombRefill1", "BombRefill1", "BombRefill4", "BombRefill1", "BombRefill1", "BombRefill8", "BombRefill1" },
                new[] { "ArrowRefill5", "Heart", "ArrowRefill5", "ArrowRefill10", "ArrowRefill5", "Heart", "ArrowRefill5", "ArrowRefill10" },
                new[] { "MagicRefillSmall", "RupeeGreen", "Heart", "ArrowRefill5", "MagicRefillSmall", "BombRefill1", "RupeeGreen", "Heart" },
                new[] { "Heart", "Fairy", "MagicRefillFull", "RupeeRed", "BombRefill8", "Heart", "RupeeRed", "ArrowRefill10" },
            }.Shuffle().SelectMany(s => s));

            var prizepacksOrdered = prizepacks.OrderBy(v => v.offset);
            foreach (var pack in prizepacksOrdered)
            {
                if (!random_vanilla_packs.TryPop(out var spriteName))
                {
                    pack.sprite = null;
                }
                else
                {
                    pack.sprite = Sprite.get(spriteName);
                }
            }
        }

        var emptypacks = prizepacks.Where((pack) => pack.sprite is null);

        if (emptypacks.Any())
        {
            // TODO refactor this
            var drops = new List<Sprite>();
            // TODO: what is the type of "item.drop"?
            foreach (var (sprite_name, count) in this.world.config("item.drop", Enumerable.Empty<(string SpriteName, int Count)>()))
            {
                drops.AddRange(Enumerable.Repeat(Sprite.get(sprite_name), Math.Min(this.world.config("drop.count." + sprite_name, count), 63)));
            }
            var drop_pool = new Stack<Sprite>(drops.Shuffle());

            foreach (var pack in emptypacks)
            {
                if (drop_pool.TryPop(out var sprite))
                    pack.sprite = sprite;
            }
        }

        // hard+ does not allow fairies/full magics
        if (this.world.config("rom.HardMode", 0) >= 2)
        {
            var fairy = Sprite.get("Fairy");
            var heart = Sprite.get("Heart");
            var magic = Sprite.get("MagicRefillFull");
            var small_magic = Sprite.get("MagicRefillSmall");
            foreach (var prizepack in prizepacks)
            {
                if (prizepack.sprite == fairy)
                {
                    prizepack.sprite = heart;
                }
                if (prizepack.sprite == magic)
                {
                    prizepack.sprite = small_magic;
                }
            }
        }

        if (this.world.config("rom.rupeeBow", false))
        {
            var arrows5 = Sprite.get("ArrowRefill5");
            var arrows10 = Sprite.get("ArrowRefill10");
            var rupeeBlue = Sprite.get("RupeeBlue");
            var rupeeRed = Sprite.get("RupeeRed");
            foreach (var prizepack in prizepacks)
            {
                if (prizepack.sprite == arrows5)
                {
                    prizepack.sprite = rupeeBlue;
                }
                if (prizepack.sprite == arrows10)
                {
                    prizepack.sprite = rupeeRed;
                }
            }
        }
    }
}
