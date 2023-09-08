using App.Graph;

namespace RandomizerTests.Logic.Open.NoGlitches.DarkWorld;

[TestClass]
public class SouthTest
{
    public static IEnumerable<object[]> TestData => new[]
    {
        new object[] { "Hype Cave - Top", false, new string[] {  } },
        new object[] { "Hype Cave - Top", false, new string[] { "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - Top", true, new string[] { "MoonPearl", "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - Top", true, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Hype Cave - Top", true, new string[] { "MoonPearl", "TitansMitt" } },
        new object[] { "Hype Cave - Top", true, new string[] { "MoonPearl", "ProgressiveGlove", "Hammer" } },
        new object[] { "Hype Cave - Top", true, new string[] { "MoonPearl", "PowerGlove", "Hammer" } },
        new object[] { "Hype Cave - Top", true, new string[] { "MoonPearl", "AgahnimDefeated", "ProgressiveGlove", "Hookshot" } },
        new object[] { "Hype Cave - Top", true, new string[] { "MoonPearl", "AgahnimDefeated", "PowerGlove", "Hookshot" } },
        new object[] { "Hype Cave - Top", true, new string[] { "MoonPearl", "AgahnimDefeated", "Flippers", "Hookshot" } },

        new object[] { "Hype Cave - Middle Right", false, new string[] {  } },
        new object[] { "Hype Cave - Middle Right", false, new string[] { "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - Middle Right", true, new string[] { "MoonPearl", "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - Middle Right", true, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Hype Cave - Middle Right", true, new string[] { "MoonPearl", "TitansMitt" } },
        new object[] { "Hype Cave - Middle Right", true, new string[] { "MoonPearl", "ProgressiveGlove", "Hammer" } },
        new object[] { "Hype Cave - Middle Right", true, new string[] { "MoonPearl", "PowerGlove", "Hammer" } },
        new object[] { "Hype Cave - Middle Right", true, new string[] { "MoonPearl", "AgahnimDefeated", "ProgressiveGlove", "Hookshot" } },
        new object[] { "Hype Cave - Middle Right", true, new string[] { "MoonPearl", "AgahnimDefeated", "PowerGlove", "Hookshot" } },
        new object[] { "Hype Cave - Middle Right", true, new string[] { "MoonPearl", "AgahnimDefeated", "Flippers", "Hookshot" } },

        new object[] { "Hype Cave - Middle Left", false, new string[] {  } },
        new object[] { "Hype Cave - Middle Left", false, new string[] { "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - Middle Left", true, new string[] { "MoonPearl", "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - Middle Left", true, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Hype Cave - Middle Left", true, new string[] { "MoonPearl", "TitansMitt" } },
        new object[] { "Hype Cave - Middle Left", true, new string[] { "MoonPearl", "ProgressiveGlove", "Hammer" } },
        new object[] { "Hype Cave - Middle Left", true, new string[] { "MoonPearl", "PowerGlove", "Hammer" } },
        new object[] { "Hype Cave - Middle Left", true, new string[] { "MoonPearl", "AgahnimDefeated", "ProgressiveGlove", "Hookshot" } },
        new object[] { "Hype Cave - Middle Left", true, new string[] { "MoonPearl", "AgahnimDefeated", "PowerGlove", "Hookshot" } },
        new object[] { "Hype Cave - Middle Left", true, new string[] { "MoonPearl", "AgahnimDefeated", "Flippers", "Hookshot" } },

        new object[] { "Hype Cave - Bottom", false, new string[] {  } },
        new object[] { "Hype Cave - Bottom", false, new string[] { "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - Bottom", true, new string[] { "MoonPearl", "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - Bottom", true, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Hype Cave - Bottom", true, new string[] { "MoonPearl", "TitansMitt" } },
        new object[] { "Hype Cave - Bottom", true, new string[] { "MoonPearl", "ProgressiveGlove", "Hammer" } },
        new object[] { "Hype Cave - Bottom", true, new string[] { "MoonPearl", "PowerGlove", "Hammer" } },
        new object[] { "Hype Cave - Bottom", true, new string[] { "MoonPearl", "AgahnimDefeated", "ProgressiveGlove", "Hookshot" } },
        new object[] { "Hype Cave - Bottom", true, new string[] { "MoonPearl", "AgahnimDefeated", "PowerGlove", "Hookshot" } },
        new object[] { "Hype Cave - Bottom", true, new string[] { "MoonPearl", "AgahnimDefeated", "Flippers", "Hookshot" } },

        new object[] { "Hype Cave - NPC", false, new string[] {  } },
        new object[] { "Hype Cave - NPC", false, new string[] { "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - NPC", true, new string[] { "MoonPearl", "AgahnimDefeated", "Hammer" } },
        new object[] { "Hype Cave - NPC", true, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Hype Cave - NPC", true, new string[] { "MoonPearl", "TitansMitt" } },
        new object[] { "Hype Cave - NPC", true, new string[] { "MoonPearl", "ProgressiveGlove", "Hammer" } },
        new object[] { "Hype Cave - NPC", true, new string[] { "MoonPearl", "PowerGlove", "Hammer" } },
        new object[] { "Hype Cave - NPC", true, new string[] { "MoonPearl", "AgahnimDefeated", "ProgressiveGlove", "Hookshot" } },
        new object[] { "Hype Cave - NPC", true, new string[] { "MoonPearl", "AgahnimDefeated", "PowerGlove", "Hookshot" } },
        new object[] { "Hype Cave - NPC", true, new string[] { "MoonPearl", "AgahnimDefeated", "Flippers", "Hookshot" } },

        new object[] { "Stumpy", false, new string[] {  } },
        new object[] { "Stumpy", false, new string[] { "AgahnimDefeated", "Hammer" } },
        new object[] { "Stumpy", true, new string[] { "MoonPearl", "AgahnimDefeated", "Hammer" } },
        new object[] { "Stumpy", true, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Stumpy", true, new string[] { "MoonPearl", "TitansMitt" } },
        new object[] { "Stumpy", true, new string[] { "MoonPearl", "ProgressiveGlove", "Hammer" } },
        new object[] { "Stumpy", true, new string[] { "MoonPearl", "PowerGlove", "Hammer" } },
        new object[] { "Stumpy", true, new string[] { "MoonPearl", "AgahnimDefeated", "ProgressiveGlove", "Hookshot" } },
        new object[] { "Stumpy", true, new string[] { "MoonPearl", "AgahnimDefeated", "PowerGlove", "Hookshot" } },
        new object[] { "Stumpy", true, new string[] { "MoonPearl", "AgahnimDefeated", "Flippers", "Hookshot" } },

        new object[] { "Digging Game - Item", false, new string[] {  } },
        new object[] { "Digging Game - Item", false, new string[] { "AgahnimDefeated", "Hammer" } },
        new object[] { "Digging Game - Item", true, new string[] { "MoonPearl", "AgahnimDefeated", "Hammer" } },
        new object[] { "Digging Game - Item", true, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Digging Game - Item", true, new string[] { "MoonPearl", "TitansMitt" } },
        new object[] { "Digging Game - Item", true, new string[] { "MoonPearl", "ProgressiveGlove", "Hammer" } },
        new object[] { "Digging Game - Item", true, new string[] { "MoonPearl", "PowerGlove", "Hammer" } },
        new object[] { "Digging Game - Item", true, new string[] { "MoonPearl", "AgahnimDefeated", "ProgressiveGlove", "Hookshot" } },
        new object[] { "Digging Game - Item", true, new string[] { "MoonPearl", "AgahnimDefeated", "PowerGlove", "Hookshot" } },
        new object[] { "Digging Game - Item", true, new string[] { "MoonPearl", "AgahnimDefeated", "Flippers", "Hookshot" } },
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
