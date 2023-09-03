namespace App.Graph;

public record class Position(short X, short Y, short Z);
// FIXME: we need a sprite class that does something.
public record class Sprite(string Name)
{
    public byte?[] sheets = new byte?[4] { null, null, null, null };
    public static Sprite get(string name) => new (name);
    public static IEnumerable<Sprite> all() => Enumerable.Empty<Sprite>();
}

/**
 * Vertex in Graph.
 */
sealed class Vertex
{
    public readonly string type;
    public readonly string name;
    public readonly bool @switch;
    public int? cost = null;
    public Item? item = null;
    public readonly Item? trophy;
    public string? peg = null;
    public Sprite? sprite = null;
    public string? enemizerBoss = null;
    public readonly int? roomid;
    public readonly int? map;
    public readonly bool? moonpearl;
    public readonly string[]? itemset;
    public readonly long[]? addresses;
    public readonly int? offset;
    public readonly Position? position;
    public readonly int[]? state;
    public readonly int? entranceid;
    public readonly int? outletid;
    public readonly int? inletid;
    public readonly int[]? entranceids;
    public readonly int? shopstyle;
    public readonly int? shopkeeper;
    public readonly int? group;

    public Vertex(Dictionary<string, object>? attributes = null)
    {
        attributes ??= new();
        this.@switch = (bool)(attributes.GetValueOrDefault("switch") ?? false);
        this.peg = (string?)attributes.GetValueOrDefault("peg");
        this.cost = (int?)attributes.GetValueOrDefault("cost");
        this.type = (string?)attributes.GetValueOrDefault("type") ?? "unknown";
        this.name = (string?)attributes.GetValueOrDefault("name") ?? $"vertex{GetHashCode()}";
        this.sprite = (Sprite?)attributes.GetValueOrDefault("sprite");
        this.item = (Item?)attributes.GetValueOrDefault("item");
        this.trophy = (Item?)attributes.GetValueOrDefault("trophy");
        this.roomid = (int?)attributes.GetValueOrDefault("roomid");
        this.map = (int?)attributes.GetValueOrDefault("map");
        this.moonpearl = (bool?)attributes.GetValueOrDefault("moonpearl");
        this.itemset = (string[]?)attributes.GetValueOrDefault("itemset");
        this.addresses = (long[]?)attributes.GetValueOrDefault("addresses");
        this.offset = (int?)attributes.GetValueOrDefault("offset");
        this.position = (Position?)attributes.GetValueOrDefault("position");
        this.state = (int[]?)attributes.GetValueOrDefault("state");
        this.entranceid = (int?)attributes.GetValueOrDefault("entranceid");
        this.outletid = (int?)attributes.GetValueOrDefault("outletid");
        this.inletid = (int?)attributes.GetValueOrDefault("inletid");
        this.entranceids = (int[]?)attributes.GetValueOrDefault("entranceids");
        this.shopstyle = (int?)attributes.GetValueOrDefault("shopstyle");
        this.shopkeeper = (int?)attributes.GetValueOrDefault("shopkeeper");
        this.group = (int?)attributes.GetValueOrDefault("group");
    }
}
