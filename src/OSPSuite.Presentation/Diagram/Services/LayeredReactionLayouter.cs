using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Services
{
   public class LayeredReactionLayouter
   {
      public const float LAYER_SPACING = 60F;
      public const float ROW_SPACING = 30F;
      public const float DIAGRAM_MARGIN = 40F;
      private const float LABEL_OFFSET = 2F;
      private const float PORT_OFFSET = 0.25F;
      private const int ITERATIONS = 4;
      private const int UNVISITED = 0;
      private const int VISITING = 1;
      private const int VISITED = 2;

      private readonly Func<ElementBaseNode, SizeF> _labelSizeFor;

      public LayeredReactionLayouter() : this(measureLabel)
      {
      }

      public LayeredReactionLayouter(Func<ElementBaseNode, SizeF> labelSizeFor)
      {
         _labelSizeFor = labelSizeFor;
      }

      public void Layout(IContainerBase containerBase)
      {
         var nodes = containerBase.GetDirectChildren<ElementBaseNode>().Where(node => !node.LocationFixed).ToList();
         if (!nodes.Any())
            return;

         var vertices = nodes
            .OrderBy(node => node.Location.Y).ThenBy(node => node.Location.X).ThenBy(node => node.Name)
            .Select(node => new Vertex(node, visualSizeFor(node)))
            .ToList();

         var verticesByNode = vertices.ToDictionary(vertex => vertex.Node);
         containerBase.GetDirectChildren<ReactionLink>().Each(link =>
         {
            if (verticesByNode.TryGetValue(link.FromNode as ElementBaseNode ?? new ElementBaseNode(), out var from) && verticesByNode.TryGetValue(link.ToNode as ElementBaseNode ?? new ElementBaseNode(), out var to))
               Edge.Connect(from, to, link.Type);
         });

         removeCycles(vertices);
         var layers = assignLayers(vertices);
         orderLayers(layers);
         assignRows(layers);
         place(layers, new PointF(DIAGRAM_MARGIN, DIAGRAM_MARGIN));
      }

      private static void removeCycles(IReadOnlyList<Vertex> vertices)
      {
         var state = vertices.ToDictionary(vertex => vertex, vertex => UNVISITED);

         void visit(Vertex vertex)
         {
            state[vertex] = VISITING;
            vertex.Edges.Where(e => e.From == vertex).Each(edge =>
            {
               if (state[edge.To] == VISITING)
                  edge.Reversed = true;
               else if (state[edge.To] == UNVISITED)
                  visit(edge.To);
            });

            state[vertex] = VISITED;
         }

         vertices.Where(vertex => state[vertex] == UNVISITED).Each(visit);
      }

      private static List<List<Vertex>> assignLayers(IReadOnlyList<Vertex> vertices)
      {
         var layerOf = new Dictionary<Vertex, int>();

         int layerFor(Vertex vertex)
         {
            if (layerOf.TryGetValue(vertex, out var layer))
               return layer;
            layerOf[vertex] = 0;
            var predecessors = vertex.Edges.Where(edge => edge.Target == vertex).Select(edge => edge.Source).ToList();
            layer = predecessors.Any() ? predecessors.Max(layerFor) + 1 : 0;
            layerOf[vertex] = layer;
            return layer;
         }

         vertices.Each(vertex => vertex.Layer = layerFor(vertex));
         pullSourcesTowardsSuccessors(vertices);

         var layers = Enumerable.Range(0, vertices.Max(vertex => vertex.Layer) + 1).Select(i => new List<Vertex>()).ToList();
         vertices.Each(vertex =>
         {
            vertex.Order = layers[vertex.Layer].Count;
            layers[vertex.Layer].Add(vertex);
         });

         return layers;
      }

      private static void pullSourcesTowardsSuccessors(IReadOnlyList<Vertex> vertices)
      {
         vertices.Where(vertex => vertex.Edges.Any() && vertex.Edges.All(edge => edge.Source == vertex))
            .Each(vertex => vertex.Layer = vertex.Edges.Min(edge => edge.Target.Layer) - 1);
      }

      private static void orderLayers(List<List<Vertex>> layers)
      {
         for (var iteration = 0; iteration < ITERATIONS; iteration++)
         {
            for (var layer = 1; layer < layers.Count; layer++)
            {
               reorder(layers[layer], layer - 1);
            }

            for (var layer = layers.Count - 2; layer >= 0; layer--)
            {
               reorder(layers[layer], layer + 1);
            }
         }
      }

      private static void reorder(List<Vertex> layer, int adjacentLayer)
      {
         var barycenters = layer.ToDictionary(vertex => vertex, vertex =>
         {
            var neighbors = vertex.Edges.Where(edge => edge.Other(vertex).Layer == adjacentLayer).ToList();
            return neighbors.Any() ? neighbors.Average(edge => edge.Other(vertex).Order + portOffset(vertex, edge)) : vertex.Order;
         });

         var ordered = layer.OrderBy(vertex => barycenters[vertex]).ToList();
         layer.Clear();
         layer.AddRange(ordered);
         for (var i = 0; i < layer.Count; i++)
         {
            layer[i].Order = i;
         }
      }

      private static void assignRows(List<List<Vertex>> layers)
      {
         layers.SelectMany(layer => layer).Each(vertex => vertex.Row = vertex.Order);

         for (var iteration = 0; iteration < ITERATIONS; iteration++)
         {
            layers.Each(relaxRows);
            layers.AsEnumerable().Reverse().Each(relaxRows);
         }
      }

      private static void relaxRows(List<Vertex> layer)
      {
         var desired = layer.ToDictionary(vertex => vertex, vertex => vertex.Edges.Any() ? vertex.Edges.Average(edge => edge.Other(vertex).Row + portOffset(vertex, edge)) : vertex.Row);

         if (layer.Any(vertex => !vertex.IsReaction))
            placeOnConsecutiveRows(layer, desired);
         else
            placeWithMinimumSeparation(layer, desired);
      }

      private static void placeOnConsecutiveRows(List<Vertex> layer, IReadOnlyDictionary<Vertex, float> desired)
      {
         var start = (float) Math.Round(layer.Select((vertex, i) => desired[vertex] - i).Average(), MidpointRounding.AwayFromZero);
         for (var i = 0; i < layer.Count; i++)
         {
            layer[i].Row = start + i;
         }
      }

      private static void placeWithMinimumSeparation(List<Vertex> layer, IReadOnlyDictionary<Vertex, float> desired)
      {
         var previous = float.NegativeInfinity;
         layer.Each(vertex =>
         {
            vertex.Row = Math.Max(desired[vertex], previous + 1);
            previous = vertex.Row;
         });

         var shift = layer.Average(vertex => vertex.Row - desired[vertex]);
         layer.Each(vertex => vertex.Row -= shift);
      }

      private static float portOffset(Vertex vertex, Edge edge)
      {
         var towardsModifierPort = edge.Type == ReactionLinkType.Modifier ? -PORT_OFFSET : PORT_OFFSET;
         return vertex.IsReaction ? -towardsModifierPort : towardsModifierPort;
      }

      private static void place(List<List<Vertex>> layers, PointF origin)
      {
         var vertices = layers.SelectMany(layer => layer).ToList();
         var pitch = vertices.Max(vertex => vertex.VisualSize.Height) + ROW_SPACING;
         var x = 0F;

         layers.Each(layer =>
         {
            var layerWidth = layer.Max(vertex => vertex.VisualSize.Width);
            layer.Each(vertex =>
            {
               var centerX = x + (vertex.IsReaction ? layerWidth : vertex.Node.Size.Width) / 2;
               vertex.Node.Location = new PointF(centerX, vertex.Row * pitch);
            });

            x += layerWidth + LAYER_SPACING;
         });

         var offset = new PointF(origin.X - vertices.Min(vertex => vertex.Node.Bounds.Left), origin.Y - vertices.Min(vertex => vertex.Node.Bounds.Top));
         vertices.Each(vertex => vertex.Node.Location = new PointF(vertex.Node.Location.X + offset.X, vertex.Node.Location.Y + offset.Y));
      }

      private SizeF visualSizeFor(ElementBaseNode node)
      {
         var size = node.Size;
         var label = node.LabelVisible && !string.IsNullOrEmpty(node.Name) ? _labelSizeFor(node) : SizeF.Empty;

         if (node is ReactionNode)
            return new SizeF(Math.Max(size.Width, label.Width), size.Height + LABEL_OFFSET + label.Height);

         return new SizeF(size.Width + LABEL_OFFSET + label.Width, Math.Max(size.Height, label.Height));
      }

      private static SizeF measureLabel(ElementBaseNode node)
      {
         using (var bitmap = new Bitmap(1, 1))
         using (var graphics = Graphics.FromImage(bitmap))
         using (var font = new Font(SystemFonts.DefaultFont.FontFamily, node.LabelFontSize))
         {
            return graphics.MeasureString(node.Name, font);
         }
      }

      private class Vertex
      {
         public ElementBaseNode Node { get; }
         public SizeF VisualSize { get; }
         public bool IsReaction => Node is ReactionNode;
         public List<Edge> Edges { get; } = new List<Edge>();
         public int Layer { get; set; }
         public int Order { get; set; }
         public float Row { get; set; }

         public Vertex(ElementBaseNode node, SizeF visualSize)
         {
            Node = node;
            VisualSize = visualSize;
         }
      }

      private class Edge
      {
         public Vertex From { get; }
         public Vertex To { get; }
         public ReactionLinkType Type { get; }
         public bool Reversed { get; set; }
         public Vertex Source => Reversed ? To : From;
         public Vertex Target => Reversed ? From : To;

         private Edge(Vertex from, Vertex to, ReactionLinkType type)
         {
            From = from;
            To = to;
            Type = type;
         }

         public static void Connect(Vertex from, Vertex to, ReactionLinkType type)
         {
            var edge = new Edge(from, to, type);
            from.Edges.Add(edge);
            to.Edges.Add(edge);
         }

         public Vertex Other(Vertex vertex) => vertex == From ? To : From;
      }
   }
}
