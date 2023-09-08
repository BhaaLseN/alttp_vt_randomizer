using App.Graph;

namespace RandomizerTests.Logic.Inverted.NoGlitches;

[TestClass]
public class HyruleCastleTowerTest
{
    public static IEnumerable<object[]> TestData => new[]
    {
        new object[] { "Agahnims Tower - First Chest", false, new string[] {  } },
        new object[] { "Agahnims Tower - First Chest", true, new string[] { "Lamp", "ProgressiveGlove" } },
        new object[] { "Agahnims Tower - First Chest", true, new string[] { "OcarinaInactive", "TitansMitt", "MoonPearl" } },

        new object[] { "Agahnims Tower - Second Chest", false, new string[] {  } },
        new object[] { "Agahnims Tower - Second Chest", false, new string[] { "KeyA1", "OcarinaInactive", "TitansMitt", "MoonPearl" } },
        new object[] { "Agahnims Tower - Second Chest", true, new string[] { "KeyA1", "Lamp", "ProgressiveGlove" } },
        new object[] { "Agahnims Tower - Second Chest", true, new string[] { "KeyA1", "Lamp", "OcarinaInactive", "TitansMitt", "MoonPearl" } },

        new object[] { "Agahnims Tower - Boss", false, new string[] {  } },
        new object[] { "Agahnims Tower - Boss", false, new string[] { "KeyA1", "KeyA1", "ProgressiveGlove", "Lamp" } },
        new object[] { "Agahnims Tower - Boss", true, new string[] { "KeyA1", "KeyA1", "L1Sword", "ProgressiveGlove", "Lamp" } },
    };
    [TestMethod]
    [DynamicData(nameof(TestData))]
    public void TestLogic(string location, bool expected, string[] inventory)
    {
        var randomizer = new Randomizer(new[]
        {
            new Dictionary<string, object>()
            {
                { "mode.state", "inverted" },
                { "logic", "NoGlitches" },
            }
        });
        randomizer.assumeItems(inventory.Select(i => Item.get(i, 0)));
        Assert.AreEqual(expected, randomizer.canReachLocation($"{location}:0"));
    }
}
