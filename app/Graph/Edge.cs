namespace App.Graph;

/**
 * Edge in Graph.
 */
public record struct Edge(Vertex From, Vertex To, string Group);
