using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class DiagramModel : IDiagramModel
   {
      private readonly ICache<string, IBaseNode> _nodes = new Cache<string, IBaseNode>(node => node.Id, x => null);
      private readonly Stack<Dictionary<IBaseNode, LayoutInfo>> _undoStack = new Stack<Dictionary<IBaseNode, LayoutInfo>>();
      private int _updateDepth;
      private bool _changedDuringUpdate;

      internal NodeCollection Children { get; } = new NodeCollection();

      public event Action Changed = delegate { };

      public IDiagramOptions DiagramOptions { get; set; }
      public bool IsLayouted { get; set; }
      public bool InUpdate => _updateDepth > 0;

      public IEnumerable<IBaseLink> AllLinks => GetAllChildren<DiagramNode>().SelectMany(node => node.Links).Distinct().ToList();

      internal void NotifyChanged()
      {
         if (InUpdate)
         {
            _changedDuringUpdate = true;
            return;
         }

         Changed();
      }

      public void BeginUpdate() => _updateDepth++;

      public void EndUpdate()
      {
         if (_updateDepth == 0)
            return;
         _updateDepth--;
         if (_updateDepth > 0 || !_changedDuringUpdate)
            return;
         _changedDuringUpdate = false;
         Changed();
      }

      public PointF Location
      {
         get => Bounds.Location;
         set
         {
            var delta = value.Minus(Location);
            if (delta.IsEmpty)
               return;
            Children.Each(node => node.Location = node.Location.Plus(delta));
         }
      }

      public PointF Center
      {
         get
         {
            var bounds = Bounds;
            return new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
         }
         set
         {
            var bounds = Bounds;
            Location = new PointF(value.X - bounds.Width / 2, value.Y - bounds.Height / 2);
         }
      }

      public SizeF Size
      {
         get => Bounds.Size;
         set { }
      }

      public RectangleF Bounds
      {
         get => CalculateBounds();
         set => Location = value.Location;
      }

      public RectangleF CalculateBounds()
      {
         var nodes = Children.ToList();
         if (!nodes.Any())
            return RectangleF.Empty;

         return nodes.Select(node => node.Bounds).Aggregate(RectangleF.Union);
      }

      public IEnumerable<T> GetDirectChildren<T>() where T : class
      {
         return Children.OfType<T>().Concat(linksOf(Children).OfType<T>()).Distinct().ToList();
      }

      public IEnumerable<T> GetAllChildren<T>() where T : class
      {
         var children = GetDirectChildren<T>().ToList();
         Children.OfType<IContainerNode>().Each(containerNode => children.AddRange(containerNode.GetAllChildren<T>()));

         return children.Distinct().ToList();
      }

      private static IEnumerable<IBaseLink> linksOf(IEnumerable<IBaseNode> nodes)
      {
         return nodes.OfType<DiagramNode>().SelectMany(node => node.Links).Distinct();
      }

      public void AddChildNode(IBaseNode node)
      {
         if (!(node is DiagramNode diagramNode))
            throw new InvalidTypeException(node, typeof(DiagramNode));
         Children.Add(node);
         Attach(diagramNode, this);
         NotifyChanged();
      }

      public void RemoveChildNode(IBaseNode node)
      {
         if (!Children.Remove(node))
            return;
         if (node is DiagramNode diagramNode)
         {
            Detach(diagramNode);
            diagramNode.Parent = null;
         }

         NotifyChanged();
      }

      internal void Attach(DiagramNode node, IContainerBase parent)
      {
         node.Parent = parent;
         node.Model = this;
         AddNodeId(node);
         if (!(node is ContainerNode containerNode))
            return;
         containerNode.Children.OfType<DiagramNode>().Each(child => Attach(child, containerNode));
      }

      internal void Detach(DiagramNode node)
      {
         node.Model = null;
         RemoveNodeId(node);
         if (!(node is ContainerNode containerNode))
            return;
         containerNode.Children.OfType<DiagramNode>().Each(Detach);
      }

      public bool ContainsChildNode(IBaseNode node, bool recursive)
      {
         if (recursive)
            return _nodes.Contains(node.Id);

         return node.GetParent() == this;
      }

      public void SetHiddenRecursive(bool hidden)
      {
         Children.Each(node =>
         {
            if (node is IContainerBase topContainer)
               topContainer.SetHiddenRecursive(hidden);
            else
               node.Hidden = hidden;
         });
      }

      public void PostLayoutStep()
      {
         GetDirectChildren<INeighborhoodNode>().Each(node => node.AdjustPosition());
      }

      public void Collapse(int level)
      {
         GetDirectChildren<IContainerBase>().Each(topContainer => topContainer.Collapse(level - 1));
      }

      public void Expand(int level)
      {
         GetDirectChildren<IContainerBase>().Each(topContainer => topContainer.Expand(level - 1));
      }

      public IBaseNode GetNode(string id) => id == null ? null : _nodes[id];

      public T GetNode<T>(string id) where T : class, IBaseNode => GetNode(id) as T;

      public T CreateNode<T>(string id, PointF location, IContainerBase parentContainerBase) where T : class, IBaseNode, new()
      {
         var node = new T {Id = id, Location = location};

         if (node is IElementBaseNode elementBaseNode && DiagramOptions != null)
         {
            if (node is MoleculeNode) elementBaseNode.NodeSize = DiagramOptions.DefaultNodeSizeMolecule;
            else if (node is ReactionNode) elementBaseNode.NodeSize = DiagramOptions.DefaultNodeSizeReaction;
         }

         _nodes.Add(node);
         parentContainerBase.AddChildNode(node);
         return node;
      }

      public void RemoveNode(string id)
      {
         var node = GetNode(id);
         if (node == null)
            return;

         if (node is DiagramNode diagramNode)
         {
            diagramNode.Links.ToList().Each(link => link.Unlink());
         }

         node.GetParent()?.RemoveChildNode(node);
         RemoveNodeId(node);
      }

      public void RenameNode(string id, string name)
      {
         var node = GetNode(id);
         if (node == null)
            return;
         node.Name = name;
      }

      public void AddNodeId(IBaseNode baseNode)
      {
         if (baseNode != null && !_nodes.Contains(baseNode.Id)) _nodes.Add(baseNode);
      }

      internal void RemoveNodeId(IBaseNode baseNode)
      {
         if (baseNode != null && _nodes.Contains(baseNode.Id)) _nodes.Remove(baseNode.Id);
      }

      public IBaseNode FindByName(string name)
      {
         return GetAllChildren<IBaseNode>().FirstOrDefault(node => string.Equals(node.Name, name));
      }

      public void ReplaceNodeIds(IDictionary<string, string> changedIds)
      {
         foreach (var oldId in changedIds.Keys)
         {
            if (!_nodes.Contains(oldId)) continue;
            var node = _nodes[oldId];
            _nodes.Remove(oldId);
            node.Id = changedIds[oldId];
            _nodes.Add(node);
         }
      }

      public bool IsEmpty() => Children.Count == 0;

      public void Clear()
      {
         Children.OfType<DiagramNode>().Each(node =>
         {
            Detach(node);
            node.Parent = null;
         });

         Children.Clear();
         _nodes.Clear();
         ClearUndoStack();
         NotifyChanged();
      }

      public void SetDefaultExpansion()
      {
         GetAllChildren<IContainerNode>().Each(container => container.IsExpandedByDefault = container.IsExpanded);
      }

      public void ShowDefaultExpansion()
      {
         GetAllChildren<IContainerNode>().Each(container => container.IsExpanded = container.IsExpandedByDefault);
      }

      public void RefreshSize()
      {
      }

      public IDiagramModel CreateCopy(string containerId = null)
      {
         var copy = new DiagramModel {DiagramOptions = DiagramOptions, IsLayouted = IsLayouted};
         var copiedNodes = new Dictionary<IBaseNode, IBaseNode>();

         if (string.IsNullOrEmpty(containerId))
         {
            copyChildren(this, copy, copiedNodes);
            copyLinks(this, copiedNodes);
            return copy;
         }

         var containerNode = GetNode<IContainerNode>(containerId);
         if (containerNode == null)
            return null;

         copyNode(containerNode, copy, copiedNodes);
         copyLinks(containerNode, copiedNodes);
         return copy;
      }

      private static void copyChildren(IContainerBase source, IContainerBase target, IDictionary<IBaseNode, IBaseNode> copiedNodes)
      {
         source.GetDirectChildren<IBaseNode>().Each(node => copyNode(node, target, copiedNodes));
      }

      private static void copyNode(IBaseNode node, IContainerBase target, IDictionary<IBaseNode, IBaseNode> copiedNodes)
      {
         var copy = node.Copy();
         copiedNodes[node] = copy;
         target.AddChildNode(copy);
         if (node is IContainerBase sourceContainer && copy is IContainerBase targetContainer)
            copyChildren(sourceContainer, targetContainer, copiedNodes);
      }

      private static void copyLinks(IContainerBase source, IDictionary<IBaseNode, IBaseNode> copiedNodes)
      {
         foreach (var link in source.GetAllChildren<ReactionLink>())
         {
            if (!copiedNodes.TryGetValue(link.ReactionNode, out var reactionNode) || !copiedNodes.TryGetValue(link.MoleculeNode, out var moleculeNode))
               continue;

            var copy = new ReactionLink();
            copy.Initialize(link.Type, (ReactionNode) reactionNode, (MoleculeNode) moleculeNode);
         }

         foreach (var neighborhoodNode in source.GetAllChildren<NeighborhoodNode>())
         {
            if (neighborhoodNode.FirstNeighbor == null || neighborhoodNode.SecondNeighbor == null)
               continue;

            if (!copiedNodes.TryGetValue(neighborhoodNode, out var copy) || !copiedNodes.TryGetValue(neighborhoodNode.FirstNeighbor, out var first) || !copiedNodes.TryGetValue(neighborhoodNode.SecondNeighbor, out var second))
               continue;

            ((NeighborhoodNode) copy).Initialize((IContainerNode) first, (IContainerNode) second);
         }
      }

      public IDiagramModel Create() => new DiagramModel();

      public bool StartTransaction()
      {
         _undoStack.Push(snapshot());
         return true;
      }

      public bool FinishTransaction(string layoutrecursivedone)
      {
         if (_undoStack.Count == 0)
            return false;
         if (snapshotEquals(_undoStack.Peek(), snapshot()))
            _undoStack.Pop();

         return true;
      }

      public void Undo()
      {
         if (_undoStack.Count == 0)
            return;
         var layoutInfos = _undoStack.Pop();
         try
         {
            BeginUpdate();
            layoutInfos.Where(x => _nodes.Contains(x.Key.Id)).Each(layoutInfo => layoutInfo.Value.ApplyTo(layoutInfo.Key));
         }
         finally
         {
            EndUpdate();
         }
      }

      public void ClearUndoStack() => _undoStack.Clear();

      private Dictionary<IBaseNode, LayoutInfo> snapshot()
      {
         return GetAllChildren<IBaseNode>().ToDictionary(node => node, LayoutInfo.From);
      }

      private static bool snapshotEquals(Dictionary<IBaseNode, LayoutInfo> first, Dictionary<IBaseNode, LayoutInfo> second)
      {
         return first.Count == second.Count && first.All(x => second.TryGetValue(x.Key, out var other) && x.Value.Equals(other));
      }

      private class LayoutInfo : IEquatable<LayoutInfo>
      {
         private PointF _location;
         private SizeF _size;
         private bool _hidden;
         private bool _isVisible;
         private bool _locationFixed;
         private NodeSize? _nodeSize;
         private bool? _isExpanded;
         private bool? _displayEductsRight;

         public static LayoutInfo From(IBaseNode node)
         {
            return new LayoutInfo
            {
               _location = node.Location,
               _size = node.Size,
               _hidden = node.Hidden,
               _isVisible = node.IsVisible,
               _locationFixed = node.LocationFixed,
               _nodeSize = (node as IElementBaseNode)?.NodeSize,
               _isExpanded = (node as IContainerNode)?.IsExpanded,
               _displayEductsRight = (node as ReactionNode)?.DisplayEductsRight
            };
         }

         public void ApplyTo(IBaseNode node)
         {
            node.Location = _location;
            node.Hidden = _hidden;
            node.IsVisible = _isVisible;
            node.LocationFixed = _locationFixed;
            if (node is IElementBaseNode elementBaseNode && _nodeSize.HasValue) elementBaseNode.NodeSize = _nodeSize.Value;
            if (node is IContainerNode containerNode)
            {
               containerNode.Size = _size;
               if (_isExpanded.HasValue) containerNode.IsExpanded = _isExpanded.Value;
            }

            if (node is ReactionNode reactionNode && _displayEductsRight.HasValue) reactionNode.DisplayEductsRight = _displayEductsRight.Value;
         }

         public bool Equals(LayoutInfo other)
         {
            if (other == null)
               return false;
            return _location == other._location && _size == other._size && _hidden == other._hidden && _isVisible == other._isVisible &&
                   _locationFixed == other._locationFixed && _nodeSize == other._nodeSize && _isExpanded == other._isExpanded && _displayEductsRight == other._displayEductsRight;
         }

         public override bool Equals(object obj) => Equals(obj as LayoutInfo);

         public override int GetHashCode() => _location.GetHashCode();
      }
   }
}
