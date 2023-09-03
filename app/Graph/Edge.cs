namespace App.Graph;

/**
 * Edge in Graph.
 */
sealed record struct Edge(Vertex From, Vertex To, string Group);
