using App.Graph;

namespace RandomizerTests.Logic.Open.NoGlitches;

[TestClass]
public class TurtleRockTest
{
    public static IEnumerable<object[]> TestData => new[]
    {
        new object[] { "Turtle Rock - Chain Chomps", false, new string[] {  } },
        new object[] { "Turtle Rock - Chain Chomps", false, new string[] { "OcarinaActive", "MagicMirror", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Quake", "UncleSword", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7" } },
        new object[] { "Turtle Rock - Chain Chomps", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7" } },

        new object[] { "Turtle Rock - Compass Chest", false, new string[] {  } },
        new object[] { "Turtle Rock - Compass Chest", false, new string[] { "OcarinaActive", "MagicMirror", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "Hammer", "Quake", "UncleSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Quake", "UncleSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword" } },
        new object[] { "Turtle Rock - Compass Chest", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Compass Chest", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria" } },

        new object[] { "Turtle Rock - Roller Room - Left", false, new string[] {  } },
        new object[] { "Turtle Rock - Roller Room - Left", false, new string[] { "OcarinaActive", "MagicMirror", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", false, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria" } },
        new object[] { "Turtle Rock - Roller Room - Left", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Left", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "FireRod" } },

        new object[] { "Turtle Rock - Roller Room - Right", false, new string[] {  } },
        new object[] { "Turtle Rock - Roller Room - Right", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Right", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Right", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Right", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Right", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Right", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Right", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "FireRod" } },
        new object[] { "Turtle Rock - Roller Room - Right", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "FireRod" } },

        new object[] { "Turtle Rock - Big Chest", false, new string[] {  } },
        new object[] { "Turtle Rock - Big Chest", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Big Chest", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Big Chest", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Big Chest", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Big Chest", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Big Chest", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Big Chest", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Big Chest", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },

        new object[] { "Turtle Rock - Big Key Chest", false, new string[] {  } },
        new object[] { "Turtle Rock - Big Key Chest", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7" } },
        new object[] { "Turtle Rock - Big Key Chest", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7" } },
        new object[] { "Turtle Rock - Big Key Chest", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7" } },
        new object[] { "Turtle Rock - Big Key Chest", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7" } },
        new object[] { "Turtle Rock - Big Key Chest", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7" } },
        new object[] { "Turtle Rock - Big Key Chest", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7" } },
        new object[] { "Turtle Rock - Big Key Chest", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7" } },
        new object[] { "Turtle Rock - Big Key Chest", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7" } },

        new object[] { "Turtle Rock - Crystaroller Room Chest", false, new string[] {  } },
        new object[] { "Turtle Rock - Crystaroller Room Chest", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Crystaroller Room Chest", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Crystaroller Room Chest", true, new string[] { "OcarinaActive", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Crystaroller Room Chest", true, new string[] { "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Crystaroller Room Chest", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Crystaroller Room Chest", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Crystaroller Room Chest", true, new string[] { "OcarinaActive", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Crystaroller Room Chest", true, new string[] { "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "BigKeyD7" } },

        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", false, new string[] {  } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "Cape", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "Cape", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "CaneOfByrna", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "CaneOfByrna", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "CaneOfByrna", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "CaneOfByrna", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "MirrorShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "MirrorShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "MirrorShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "MirrorShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Left", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },

        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", false, new string[] {  } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", false, new string[] { "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "Cape", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "Cape", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "CaneOfByrna", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "CaneOfByrna", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "CaneOfByrna", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "CaneOfByrna", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "MirrorShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "MirrorShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "MirrorShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "MirrorShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Bottom Right", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },

        new object[] { "Turtle Rock - Eye Bridge - Top Left", false, new string[] {  } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "Cape", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "Cape", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "CaneOfByrna", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "CaneOfByrna", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "CaneOfByrna", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "CaneOfByrna", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "MirrorShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "MirrorShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "MirrorShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "MirrorShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Left", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },

        new object[] { "Turtle Rock - Eye Bridge - Top Right", false, new string[] {  } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "Cape", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "Cape", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "Cape", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "CaneOfByrna", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "CaneOfByrna", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "CaneOfByrna", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "CaneOfByrna", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "MirrorShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "MirrorShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "MirrorShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "MirrorShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Eye Bridge - Top Right", true, new string[] { "Lamp", "ProgressiveShield", "ProgressiveShield", "ProgressiveShield", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },

        new object[] { "Turtle Rock - Boss", false, new string[] {  } },
        new object[] { "Turtle Rock - Boss", false, new string[] { "FireRod", "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Boss", false, new string[] { "IceRod", "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Boss", true, new string[] { "HalfMagic", "Bottle", "IceRod", "FireRod", "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Boss", true, new string[] { "HalfMagic", "Bottle", "IceRod", "FireRod", "Lamp", "MagicMirror", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Boss", true, new string[] { "HalfMagic", "Bottle", "IceRod", "FireRod", "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "UncleSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
        new object[] { "Turtle Rock - Boss", true, new string[] { "HalfMagic", "Bottle", "IceRod", "FireRod", "Lamp", "Hookshot", "MoonPearl", "TitansMitt", "Hammer", "Quake", "ProgressiveSword", "CaneOfSomaria", "KeyD7", "KeyD7", "KeyD7", "KeyD7", "BigKeyD7" } },
    };
    [TestMethod]
    [DynamicData(nameof(TestData))]
    public void TestLogic(string location, bool expected, string[] inventory)
    {
        var randomizer = new Randomizer(new[]
        {
            new Dictionary<string, object>()
            {
                { "mode.state", "open" },
                { "logic", "NoGlitches" },
            }
        });
        randomizer.assumeItems(inventory.Select(i => Item.get(i, 0)));
        Assert.AreEqual(expected, randomizer.canReachLocation($"{location}:0"));
    }
}
