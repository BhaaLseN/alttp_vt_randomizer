using System.Collections.Concurrent;

namespace App.Graph;

/**
 * Matrix backed graph. Instead of using a full graph library, this is an
 * attempt at one that is more tuned to our needs. We only allow additions to
 * the graph, so there is no need to consider removal of edges/vertices. And all
 * edges must be directed!
 */
public sealed class Graph
{
    private readonly List<Vertex> vertices = new();
    private readonly Dictionary<string, Vertex> vertices_name = new();
    private readonly ConcurrentDictionary<Vertex, HashSet<Vertex>> visited = new();
    private readonly Dictionary<Vertex, HashSet<Vertex>> adjency_matrix = new();
    private readonly HashSet<Edge> edges = new();
    private readonly ConcurrentDictionary<Vertex, HashSet<Vertex>> marked = new();
    private readonly ConcurrentDictionary<Vertex, HashSet<Vertex>> peg_marked = new();
    private readonly List<Vertex> recheck_nodes = new();

    /**
     * Get the Vertices in the Graph.
     */
    public IEnumerable<Vertex> getVertices()
    {
        return this.vertices;
    }

    /**
     * Get a Vertex by name in the Graph.
     * 
     * @param string name name of vertex to search for
     */
    public Vertex? getVertex(string name)
    {
        return this.vertices_name.GetValueOrDefault(name);
    }

    /**
     * Get the Edges in the Graph.
     */
    public IEnumerable<Edge> getEdges()
    {
        return this.edges;
    }

    /**
     * Get the Items found is last search of the Graph.
     * 
     * @param Vertex start where the search was started from
     * @param callable filter if we should filter the items
     */
    public IEnumerable<Item> getItems(Vertex start, Predicate<Item>? filter = null)
    {
        filter ??= item => true;
        foreach (var vertex in this.visited[start])
        {
            if (vertex.item is not null && filter(vertex.item))
            {
                yield return vertex.item;
            }
            if (vertex.trophy is not null && filter(vertex.trophy))
            {
                yield return vertex.trophy;
            }
        }
    }

    /**
     * Create a new Vertex in this Graph.
     *
     * @param mixed[] attributes attributes for the vertex
     */
    public Vertex newVertex(Dictionary<string, object>? attributes = null)
    {
        var vertex = new Vertex(attributes);

        this.addVertex(vertex);

        return vertex;
    }

    /**
     * Add vertex to graph.
     * 
     * @param Vertex vertex Vertex to add to graph
     */
    public void addVertex(Vertex vertex)
    {
        this.vertices.Add(vertex);
        this.vertices_name[vertex.name] = vertex;
        this.adjency_matrix.Add(vertex, new());
    }

    public Vertex? getTargetVertex(Vertex vertex)
    {
        return this.adjency_matrix.GetValueOrDefault(vertex)?.FirstOrDefault();
    }

    public IEnumerable<Vertex> getTargets(Vertex vertex)
    {
        return this.adjency_matrix.GetValueOrDefault(vertex) ?? Enumerable.Empty<Vertex>();
    }

    /**
     * Create a new Directed Edge in the graph.
     * 
     * @param Vertex from from vertex
     * @param Vertex to to vertex
     * @param string group edge grouping
     */
    public Edge addDirected(Vertex from, Vertex to, string group)
    {
        var edge = new Edge(from, to, group);

        this.addEdge(edge);

        return edge;
    }

    /**
     * Add already constructed Edge to graph, this will also add the related
     * Vertices to the graph if they aren"t there already.
     * 
     * @param Edge edge Edge to add
     */
    public void addEdge(Edge edge)
    {
        if (!this.vertices.Contains(edge.From))
        {
            this.addVertex(edge.From);
        }

        if (!this.vertices.Contains(edge.To))
        {
            this.addVertex(edge.To);
        }

        this.adjency_matrix[edge.From].Add(edge.To);
        this.edges.Add(edge);
    }

    /**
     * Get a subgraph of the graph filtering on given edge group. Used to split
     * the graph up by item requirements.
     * 
     * @param string group name of group
     */
    public Graph getSubgraph(string group)
    {
        var newGraph = new Graph();

        foreach (var edge in this.edges)
        {
            if (edge.Group != group)
            {
                continue;
            }

            newGraph.addEdge(edge);
        }

        return newGraph;
    }

    /**
     * Merge this graph with a variable amount of other graphs and return new
     * Graph.
     *
     * @param Graph ...graphs graphs to merge
     */
    public Graph merge(params Graph[] graphs)
    {
        var newGraph = new Graph(this);

        foreach (var graph in graphs)
        {
            foreach (var edge in graph.edges)
            {
                if (newGraph.edges.Contains(edge))
                {
                    continue;
                }

                newGraph.addEdge(edge);
                newGraph.recheck_nodes.Add(edge.From);
            }
        }

        return newGraph;
    }

    /**
     * Get new graph with certain edge groups excluded.
     * 
     * @param string ...groups edge groups to exclude
     */
    public Graph exclude(params string[] groups)
    {
        var newGraph = new Graph();

        foreach (var edge in this.edges)
        {
            if (groups.Contains(edge.Group))
            {
                continue;
            }

            newGraph.addEdge(edge);
        }

        return newGraph;
    }

    /**
     * Get all vertices that were visited in a given search (which has been
     * called first) from a set starting point.
     *
     * @param Vertex start vertex where search was started from
     */
    public IEnumerable<Vertex> getVisited(Vertex start)
    {
        return this.visited.GetValueOrDefault(start) ?? Enumerable.Empty<Vertex>();
    }

    /**
     * Perform a search of reachable Vertices from a given start. This is really
     * meat an potatoes of the whole class... I"m sure you were expecting good
     * documentation. Eventually my friend, eventually.
     *
     * @param Vertex start vertex to start search from
     */
    public IEnumerable<Vertex> search(Vertex start)
    {
        if (!this.vertices.Contains(start))
        {
            return Enumerable.Empty<Vertex>();
        }

        if (!this.visited.ContainsKey(start))
        {
            this.visited.TryAdd(start, new() { start });
            this.peg_marked.TryAdd(start, new());
            this.marked.TryAdd(start, new() { start });
        }
        var queue = new Queue<Vertex>();
        var peg_queue = new Queue<Vertex>();
        queue.Enqueue(start);
        foreach (var vertex in this.marked[start])
        {
            if (this.recheck_nodes.Contains(vertex))
                queue.Enqueue(vertex);
        }
        foreach (var vertex in this.peg_marked[start])
        {
            if (this.recheck_nodes.Contains(vertex))
                queue.Enqueue(vertex);
        }
        this.recheck_nodes.Clear();

        do
        {
            while (peg_queue.TryDequeue(out var vertex))
            {
                if (vertex.@switch)
                {
                    if (!this.marked.TryGetValue(start, out var markedFromStart) || !markedFromStart.Contains(vertex))
                    {
                        queue.Enqueue(vertex);
                    }
                }
                if (vertex.peg == "orange")
                {
                    continue;
                }
                foreach (var next_vertex in this.adjency_matrix.GetValueOrDefault(vertex, new()))
                {
                    if (!this.peg_marked.TryGetValue(start, out var pegMarkedFromStart) || !pegMarkedFromStart.Contains(next_vertex))
                    {
                        peg_queue.Enqueue(next_vertex);
                    }
                }
                this.visited.GetOrAdd(start, _ => new()).Add(vertex);
                this.peg_marked.GetOrAdd(start, _ => new()).Add(vertex);
            }

            while (queue.TryDequeue(out var vertex))
            {
                if (vertex.@switch)
                {
                    if (!this.peg_marked.TryGetValue(start, out var pegMarkedFromStart) || !pegMarkedFromStart.Contains(vertex))
                    {
                        peg_queue.Enqueue(vertex);
                    }
                }
                if (vertex.peg == "blue")
                {
                    continue;
                }
                foreach (var next_vertex in this.adjency_matrix.GetValueOrDefault(vertex, new()))
                {
                    if (!this.marked.TryGetValue(start, out var markedFromStart) || !markedFromStart.Contains(next_vertex))
                    {
                        queue.Enqueue(next_vertex);
                    }
                }
                this.visited.GetOrAdd(start, _ => new()).Add(vertex);
                this.marked.GetOrAdd(start, _ => new()).Add(vertex);
            }
        } while (queue.Any() || peg_queue.Any());

        return this.visited.GetValueOrDefault(start) ?? Enumerable.Empty<Vertex>();
    }

    public Graph()
    {
    }
    private Graph(Graph other)
    {
        adjency_matrix = new(other.adjency_matrix);
        edges = new(other.edges);
        marked = new(other.marked);
        peg_marked = new(other.peg_marked);
        recheck_nodes = new(other.recheck_nodes);
        vertices = new(other.vertices);
        vertices_name = new(other.vertices_name);
        visited = new(other.visited);
    }
}
