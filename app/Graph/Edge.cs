namespace App.Graph;

/**
 * Edge in Graph.
 */
public sealed class Edge
{
    public Vertex From { get; }
    public Vertex To { get; }
    public string Group { get; set; }
    public Edge(Vertex from, Vertex to, string group) => (From, To, Group) = (from, to, group);
}
