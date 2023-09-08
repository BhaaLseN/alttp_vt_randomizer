using App.Graph;

namespace RandomizerTests.Logic.Open.NoGlitches.DarkWorld;

[TestClass]
public class MireTest
{
    public static IEnumerable<object[]> TestData => new[]
    {
        new object[] { "Mire Shed - Left", false, new string[] {  } },
        new object[] { "Mire Shed - Left", false, new string[] { "OcarinaActive", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Mire Shed - Left", false, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Mire Shed - Left", false, new string[] { "MoonPearl", "OcarinaActive", "ProgressiveGlove" } },
        new object[] { "Mire Shed - Left", true, new string[] { "MoonPearl", "OcarinaActive", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Mire Shed - Left", true, new string[] { "MoonPearl", "OcarinaActive", "TitansMitt" } },

        new object[] { "Mire Shed - Right", false, new string[] {  } },
        new object[] { "Mire Shed - Right", false, new string[] { "OcarinaActive", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Mire Shed - Right", false, new string[] { "MoonPearl", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Mire Shed - Right", false, new string[] { "MoonPearl", "OcarinaActive", "ProgressiveGlove" } },
        new object[] { "Mire Shed - Right", true, new string[] { "MoonPearl", "OcarinaActive", "ProgressiveGlove", "ProgressiveGlove" } },
        new object[] { "Mire Shed - Right", true, new string[] { "MoonPearl", "OcarinaActive", "TitansMitt" } },
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
