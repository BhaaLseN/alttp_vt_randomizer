namespace App.Graph;

/**
 * Edge in Graph.
 */
sealed record class Edge(Vertex From, Vertex To, string Group);
