using App.Graph;

namespace RandomizerTests.Logic.Standard.NoGlitches;

[TestClass]
public class EastTest
{
    public static IEnumerable<object[]> TestData => new[]
    {
        new object[] { "Sanctuary Chest", true, new string[] { "UncleSword", "KeyH2" } },

        new object[] { "Sewers - Secret Room - Left", true, new string[] { "UncleSword", "KeyH2" } },

        new object[] { "Sewers - Secret Room - Middle", true, new string[] { "UncleSword", "KeyH2" } },

        new object[] { "Sewers - Secret Room - Right", true, new string[] { "UncleSword", "KeyH2" } },

        new object[] { "Sewers - Dark Cross Chest", true, new string[] { "UncleSword" } },

        new object[] { "Hyrule Castle - Boomerang Chest", true, new string[] { "UncleSword" } },

        new object[] { "Hyrule Castle - Map Chest", true, new string[] { "UncleSword" } },

        new object[] { "Hyrule Castle - Zelda's Cell", true, new string[] { "UncleSword" } },
    };
    [TestMethod]
    [DynamicData(nameof(TestData))]
    public void TestLogic(string location, bool expected, string[] inventory)
    {
        var randomizer = new Randomizer(new[]
        {
            new Dictionary<string, object>()
            {
                { "mode.state", "standard" },
                { "logic", "NoGlitches" },
            }
        });
        randomizer.assumeItems(inventory.Select(i => Item.get(i, 0)));
        Assert.AreEqual(expected, randomizer.canReachLocation($"{location}:0"));
    }
}
