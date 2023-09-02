namespace App.Graph;

using WeightedSet = Dictionary<int, List<Item>>;
using ItemSet = Dictionary<string, /* WeightedSet */ Dictionary<int, List<Item>>>;

/**
 * Get the sets of items to place.
 */
sealed class ItemPooler
{
    private readonly World[] worlds;
    /**
     * Create new Item Pooler.
     * 
     * @param World[] worlds worlds to get Item pools for
     */
    public ItemPooler(World[] worlds)
    {
        this.worlds = worlds;
    }

    /**
     * Get list of all items in their weighted sets.
     */
    public ItemSet getPool()
    {
        var sets = new ItemSet();

        foreach (var world in this.worlds)
        {
            var world_set = array_merge_recursive(
                this.getMedallions(world),
                this.getPrizes(world),
                this.getSmallKeys(world),
                this.getBigKeys(world),
                this.getMaps(world),
                this.getCompasses(world),
                this.getBottles(world),
                this.getShopItems(world),
                new ItemSet
                {
                    { "*", new WeightedSet
                        {
                            // placing behind keys for now.
                            { 3, new List<Item>
                                {
                                    Item.get("Hammer", world.id),
                                    Item.get("Hookshot", world.id),
                                    Item.get("Flippers", world.id),
                                    Item.get("FireRod", world.id),
                                    Item.get("IceRod", world.id),
                                    Item.get("ProgressiveBow", world.id),
                                    Item.get("ProgressiveBow", world.id),
                                    Item.get("ProgressiveSword", world.id),
                                    Item.get("ProgressiveSword", world.id),
                                    Item.get("ProgressiveShield", world.id),
                                    Item.get("ProgressiveShield", world.id),
                                    Item.get("ProgressiveShield", world.id),
                                    Item.get("PegasusBoots", world.id),
                                    Item.get("BookOfMudora", world.id),
                                    Item.get("ProgressiveGlove", world.id),
                                    Item.get("ProgressiveGlove", world.id),
                                    Item.get("CaneOfSomaria", world.id),
                                    Item.get("CaneOfByrna", world.id),
                                    Item.get("Cape", world.id),
                                    Item.get("Lamp", world.id),
                                    Item.get("Bombos", world.id),
                                    Item.get("Ether", world.id),
                                    Item.get("Quake", world.id),
                                    Item.get("Mushroom", world.id),
                                    Item.get("MoonPearl", world.id),
                                    Item.get("MagicMirror", world.id),
                                    Item.get("OcarinaInactive", world.id),
                                    Item.get("Shovel", world.id),
                                    Item.get("BugCatchingNet", world.id),
                                    Item.get("Powder", world.id),
                                    Item.get("HalfMagic", world.id),
                                }
                            },
                            { 9001, new[]
                                {
                                    Item.get("Boomerang", world.id),
                                    Item.get("RedBoomerang", world.id),
                                    Item.get("HeartContainer", world.id),
                                }
                                .Concat(Enumerable.Repeat(Item.get("ProgressiveSword", world.id), 2))
                                .Concat(Enumerable.Repeat(Item.get("ProgressiveArmor", world.id), 2))
                                .Concat(Enumerable.Repeat(Item.get("BossHeartContainer", world.id), 10))
                                .Concat(Enumerable.Repeat(Item.get("PieceOfHeart", world.id), 24))
                                .ToList()
                            },
                            // order here matters, items at end may get lopped off
                            // if too many items to place
                            { 9999, new[]
                                {
                                    Item.get("Arrow", world.id),
                                    Item.get("OneHundredRupees", world.id),
                                }
                                .Concat(Enumerable.Repeat(Item.get("TenArrows", world.id), 12))
                                .Concat(Enumerable.Repeat(Item.get("ThreeBombs", world.id), 17))
                                .Concat(Enumerable.Repeat(Item.get("OneRupee", world.id), 2))
                                .Concat(Enumerable.Repeat(Item.get("FiveRupees", world.id), 4))
                                .Concat(Enumerable.Repeat(Item.get("TwentyRupees", world.id), 28))
                                .Concat(Enumerable.Repeat(Item.get("FiftyRupees", world.id), 7))
                                .Concat(Enumerable.Repeat(Item.get("ThreeHundredRupees", world.id), 5))
                                .ToList()
                            },
                        }
                    },
                }
            );

            if (
                world.config<string>("logic") != "None"
                && (world.config<string>("mode.state") == "inverted"
                    || !(world.config<string>("logic") is "OverworldGlitches" or "MajorGlitches"))
            )
            {
                var crystal_ratio = world.config("crystals.tower", 7) / 7f;
                int fill_count;
                if (world.config<string>("goal") is "triforce-hunt" or "pedestal")
                {
                    fill_count = Random.Shared.Next((int)(15 * crystal_ratio), (int)(25 * crystal_ratio));
                }
                else
                {
                    fill_count = Random.Shared.Next((int)(15 * crystal_ratio));
                }
                if (fill_count > 0)
                {
                    var junkFill = world_set["*"][9999].Shuffle().Take(fill_count).ToArray();
                    foreach (var key in junkFill)
                    {
                        world_set["gt:" + world.id][2].Add(key);
                        world_set["*"][9999].Remove(key);
                    }
                }
            }

            sets = array_merge_recursive(
                sets,
                world_set
            );
        }

        return sets;
    }

    private static ItemSet array_merge_recursive(params ItemSet[] itemSets)
    {
        var result = new ItemSet();

        foreach (var itemSet in itemSets)
        {
            foreach (var (location, prioritySets) in itemSet)
            {
                if (!result.TryGetValue(location, out var locationSet))
                    result[location] = locationSet = new WeightedSet();
                foreach (var (priority, items) in prioritySets)
                {
                    if (!locationSet.TryGetValue(priority, out var prioritySet))
                        locationSet[priority] = prioritySet = new List<Item>();

                    prioritySet.AddRange(items);
                }
            }
        }

        return result;
    }

    /**
     * Get Medallions meta locations for what ends up being required for TR/MM
     * entry.
     *
     * @param World world world to get items for
     */
    private ItemSet getMedallions(World world)
    {
        return new ItemSet
        {
            { "mm-medallion:" + world.id, new WeightedSet
                {
                    { 0, new List<Item>
                        {
                            Item.get(new [] { "MireEntryBombos", "MireEntryEther", "MireEntryQuake" }[Random.Shared.Next(2)], world.id),
                        }
                    },
                }
            },
            { "tr-medallion:" + world.id, new WeightedSet
                {
                    { 0, new List<Item>
                        {
                            Item.get(new [] { "TurtleRockEntryBombos", "TurtleRockEntryEther", "TurtleRockEntryQuake" }[Random.Shared.Next(2)], world.id),
                        }
                    },
                }
            },
        };
    }

    /**
     * Get Prizes for a world.
     *
     * @param World world world to get items for
     */
    private ItemSet getPrizes(World world)
    {
        return new ItemSet
        {
            { "prize:" + world.id, new WeightedSet
                {
                    { 0, new List<Item>
                         {
                             Item.get("PendantOfCourage", world.id),
                             Item.get("PendantOfWisdom", world.id),
                             Item.get("PendantOfPower", world.id),
                             Item.get("Crystal1", world.id),
                             Item.get("Crystal2", world.id),
                             Item.get("Crystal3", world.id),
                             Item.get("Crystal4", world.id),
                             Item.get("Crystal5", world.id),
                             Item.get("Crystal6", world.id),
                             Item.get("Crystal7", world.id),
                         }
                    },
                }
            },
        };
    }

    /**
     * Get Small keys for world in proper placement groups.
     *
     * @param World world world to get items for
     */
    private ItemSet getSmallKeys(World world)
    {
        var keys = new ItemSet
        {
            { "escape:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("KeyH2", world.id) } },
                }
            },
            { "desert:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("KeyP2", world.id) } },
                }
            },
            { "hera:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("KeyP3", world.id) } },
                }
            },
            { "agahnim:" + world.id, new WeightedSet
                {
                    { 1, Enumerable.Repeat(Item.get("KeyA1", world.id), 2).ToList() },
                }
            },
            { "pod:" + world.id, new WeightedSet
                {
                    { 1, Enumerable.Repeat(Item.get("KeyD1", world.id), 6).ToList() },
                }
            },
            { "swamp:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("KeyD2", world.id) } },
                }
            },
            { "skull:" + world.id, new WeightedSet
                {
                    { 1, Enumerable.Repeat(Item.get("KeyD3", world.id), 3).ToList() },
                }
            },
            { "thieves:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("KeyD4", world.id) } },
                }
            },
            { "ice:" + world.id, new WeightedSet
                {
                    { 1, Enumerable.Repeat(Item.get("KeyD5", world.id), 2).ToList() },
                }
            },
            { "mire:" + world.id, new WeightedSet
                {
                    { 1, Enumerable.Repeat(Item.get("KeyD6", world.id), 3).ToList() },
                }
            },
            { "turtlerock:" + world.id, new WeightedSet
                {
                    { 1, Enumerable.Repeat(Item.get("KeyD7", world.id), 4).ToList() },
                }
            },
            { "gt:" + world.id, new WeightedSet
                {
                    { 1, Enumerable.Repeat(Item.get("KeyA2", world.id), 4).ToList() },
                }
            },
        };

        if (world.config("region.wildKeys", false))
        {
            return new ItemSet
            {
                { "*",
                    new WeightedSet
                    {
                        { 3, keys.SelectMany(dungeon => dungeon.Value.SelectMany(priority => priority.Value)).ToList() }
                    }
                }
            };
        }

        return keys;
    }

    /**
     * Get Big keys for world in proper placement groups.
     *
     * @param World world world to get items for
     */
    private ItemSet getBigKeys(World world)
    {
        var big_keys = new ItemSet
        {
            { "eastern:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("BigKeyP1", world.id) } },
                }
            },
            { "desert:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("BigKeyP2", world.id) } },
                }
            },
            { "hera:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("BigKeyP3", world.id) } },
                }
            },
            { "pod:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("BigKeyD1", world.id) } },
                }
            },
            { "swamp:" + world.id, new WeightedSet
                {
                    { 2, new List<Item> { Item.get("BigKeyD2", world.id) } },
                }
            },
            { "skull:" + world.id, new WeightedSet
                {
                    { 2, new List<Item> { Item.get("BigKeyD3", world.id) } },
                }
            },
            { "thieves:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("BigKeyD4", world.id) } },
                }
            },
            { "ice:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("BigKeyD5", world.id) } },
                }
            },
            { "mire:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("BigKeyD6", world.id) } },
                }
            },
            { "turtlerock:" + world.id, new WeightedSet
                {
                    { 1, new List<Item> { Item.get("BigKeyD7", world.id) } },
                }
            },
            { "gt:" + world.id, new WeightedSet
                {
                    { 0, new List<Item> { Item.get("BigKeyA2", world.id) } },
                }
            },
        };

        if (world.config("region.wildBigKeys", false))
        {
            return new ItemSet
            {
                { "*",
                    new WeightedSet
                    {
                        { 3, big_keys.SelectMany(dungeon => dungeon.Value.SelectMany(priority => priority.Value)).ToList() }
                    }
                }
            };
        }

        return big_keys;
    }

    /**
     * Get Maps for world in proper placement groups.
     *
     * @param World world world to get items for
     */
    private ItemSet getMaps(World world)
    {
        var maps = new ItemSet
        {
            { "escape:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapH2", world.id) } },
                }
            },
            { "eastern:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapP1", world.id) } },
                }
            },
            { "desert:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapP2", world.id) } },
                }
            },
            { "hera:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapP3", world.id) } },
                }
            },
            { "pod:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapD1", world.id) } },
                }
            },
            { "swamp:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapD2", world.id) } },
                }
            },
            { "skull:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapD3", world.id) } },
                }
            },
            { "thieves:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapD4", world.id) } },
                }
            },
            { "ice:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapD5", world.id) } },
                }
            },
            { "mire:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapD6", world.id) } },
                }
            },
            { "turtlerock:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapD7", world.id) } },
                }
            },
            { "gt:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("MapA2", world.id) } },
                }
            },
        };

        if (world.config("region.wildMaps", false))
        {
            return new ItemSet
            {
                { "*",
                    new WeightedSet
                    {
                        { 3, maps.SelectMany(dungeon => dungeon.Value.SelectMany(priority => priority.Value)).ToList() }
                    }
                }
            };
        }

        if (world.config<string>("accessibility") == "items")
        {
            foreach (var (_, parts) in maps)
            {
                if (parts.Remove(9010, out var items))
                    parts.Add(9999, items);
            }
        }

        return maps;
    }

    /**
     * Get Compasses for world in proper placement groups.
     *
     * @param World world world to get items for
     */
    private ItemSet getCompasses(World world)
    {
        var compasses = new ItemSet
        {
            { "eastern:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassP1", world.id) } }
                }
            },
            { "desert:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassP2", world.id) } }
                }
            },
            { "hera:"+world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassP3", world.id) } }
                }
            },
            { "pod:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassD1", world.id) } }
                }
            },
            { "swamp:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassD2", world.id) } }
                }
            },
            { "skull:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassD3", world.id) } }
                }
            },
            { "thieves:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassD4", world.id) } }
                }
            },
            { "ice:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassD5", world.id) } }
                }
            },
            { "mire:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassD6", world.id) } }
                }
            },
            { "turtlerock:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassD7", world.id) } }
                }
            },
            { "gt:" + world.id, new WeightedSet
                {
                    { 9010, new List<Item> { Item.get("CompassA2", world.id) } }
                }
            },
        };

        if (world.config("region.wildCompasses", false))
        {
            return new ItemSet
            {
                { "*",
                    new WeightedSet
                    {
                        { 3, compasses.SelectMany(dungeon => dungeon.Value.SelectMany(priority => priority.Value)).ToList() }
                    }
                }
            };
        }

        if (world.config<string>("accessibility") == "items")
        {
            foreach (var (_, parts) in compasses)
            {
                if (parts.Remove(9010, out var items))
                    parts.Add(9999, items);
            }
        }

        return compasses;
    }

    /**
     * Get Bottles for world in proper placement groups.
     *
     * @param World world world to get items for
     */
    private ItemSet getBottles(World world)
    {
        string[] bottles = {
            "Bottle",
            "BottleWithRedPotion",
            "BottleWithGreenPotion",
            "BottleWithBluePotion",
            "BottleWithBee",
            "BottleWithGoldBee",
            "BottleWithFairy",
        };

        return new ItemSet()
        {
            { "bottle:" + world.id,
                new WeightedSet()
                {
                    { 0, new List<Item>
                         {
                             Item.get("Fairy" + bottles[Random.Shared.Next(bottles.Length)], world.id),
                             Item.get("Fairy" + bottles[Random.Shared.Next(bottles.Length)], world.id),
                         }
                    },
                }
            },
            { "*",
                new WeightedSet()
                {
                    { 3, new List<Item> { Item.get(bottles[Random.Shared.Next(bottles.Length)], world.id) } },
                    { 9001, new List<Item>
                            {
                                Item.get(bottles[Random.Shared.Next(bottles.Length)], world.id),
                                Item.get(bottles[Random.Shared.Next(bottles.Length)], world.id),
                                Item.get(bottles[Random.Shared.Next(bottles.Length)], world.id),
                            }
                    },
                }
            },
        };
    }

    /**
     * Get Shop Items for world in proper placement groups.
     * 
     * @todo verify these counts, they are definitely wrong
     *
     * @param World world world to get items for
     */
    private ItemSet getShopItems(World world)
    {
        if (world.config<string>("region.shopSupply") != "shuffled")
        {
            return new();
        }

        return new ItemSet()
        {
            { "*", new WeightedSet()
                {
                    { 9999, Enumerable.Repeat(Item.get("RedPotion", world.id), 6)
                        .Concat(Enumerable.Repeat(Item.get("GreenPotion", world.id), 1))
                        .Concat(Enumerable.Repeat(Item.get("BluePotion", world.id), 6))
                        .Concat(Enumerable.Repeat(Item.get("Heart", world.id), 10))
                        .Concat(Enumerable.Repeat(Item.get("TenBombs", world.id), 10))
                        .Concat(Enumerable.Repeat(Item.get("BlueShield", world.id), 2))
                        .Concat(Enumerable.Repeat(Item.get("RedShield", world.id), 1))
                        .ToList()
                    },
                }
            },
        };
    }
}
