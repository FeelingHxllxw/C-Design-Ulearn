using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FluentApi.Graph
{
    public enum NodeShape
    {
        Box,
        Ellipse
    }

    public class DotGraphBuilder
    {
        protected Graph dotGraph;
        private bool isDirectGraph = false;
        public DotGraphBuilder(Graph graph)
        {
            dotGraph = graph;
        }

        public DotGraphBuilder(string graphName, bool isDirectGraph)
        {
            dotGraph = new Graph(graphName, isDirectGraph, false);
        }

        public string Build()
        {
            string builded= dotGraph.ToDotFormat();
            return builded;
        }

         public DotNode AddNode(string node)
         {
            dotGraph.AddNode(node);
            return new DotNode(graph: dotGraph, node);
         }

        public DotEdge AddEdge(string from, string to)
        {
            dotGraph.AddEdge(from, to);
            return new DotEdge(graph: dotGraph, from, to);
        }

        public static DotGraphBuilder DirectedGraph(string graphName)
        {
            DotGraphBuilder newGraph= new DotGraphBuilder(graphName, true);
            return newGraph;
        }

        public static DotGraphBuilder UndirectedGraph(string graphName)
        {
            DotGraphBuilder newGraph = new DotGraphBuilder(graphName, false);
            return newGraph;
        }
    }


    public class DotNode : DotGraphBuilder
    {
        private readonly string node;
        public DotNode(Graph graph, string node) : base(graph) => this.node = node;
        public DotGraphBuilder With(Action<NodeBuilder> action)
        {
            foreach (var element in dotGraph.Nodes)
                if (element.Name == node)
                {
                    action(new NodeBuilder(element));
                    break;
                }
            return this;
        }
    }

    public class DotEdge : DotGraphBuilder
    {
        private readonly (string from, string to) edge;
        public DotEdge(Graph graph, string from, string to) : base(graph) => edge = (from, to);
        public DotGraphBuilder With(Action<EdgeBuilder> action)
        {
            foreach (var element in dotGraph.Edges)
                if (element.SourceNode == edge.from && element.DestinationNode == edge.to)
                {
                    action(new EdgeBuilder(element));
                    break;
                }
            return this;
        }
    }

    public class NodeBuilder : Attributess<NodeBuilder>
    {
        public NodeBuilder(GraphNode node) => Attributes = node.Attributes;
        public NodeBuilder Shape(NodeShape shape)
        {
            Attributes["shape"] = shape.ToString().ToLower();
            return this;
        }
    }

    public class EdgeBuilder : Attributess<EdgeBuilder>
    {
        public EdgeBuilder(GraphEdge edge) => Attributes = edge.Attributes;
        public EdgeBuilder Weight(double weight)
        {
            Attributes["weight"] = weight.ToString();
            return this;
        }
    }

    public class Attributess<T> where T : class
    {
        private Dictionary<string, string> attributes = new Dictionary<string, string>();
        protected Dictionary<string, string> Attributes { get => attributes; set => attributes = value; }

        public T Color(string color)
        {
            Attributes["color"] = color;
            return this as T;
        }

        public T Label(string label)
        {
            Attributes["label"] = label;
            return this as T;
        }

        public T FontSize(int fontsize)
        {
            string font= fontsize.ToString();
            Attributes["fontsize"] = font;
            return this as T;
        }
    }
}