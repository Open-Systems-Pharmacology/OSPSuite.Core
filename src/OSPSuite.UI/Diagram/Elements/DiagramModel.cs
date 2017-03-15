using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using Northwoods.Go;
using OSPSuite.Assets;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public class DiagramModel : GoDocument, IDiagramModel
   {
      public IBaseNode FindByName(string name)
      {
         return _nodes.FindByName(name);
      }

      private readonly ICache<string, IBaseNode> _nodes;
      public string Id { get; set; } 
      public IDiagramOptions DiagramOptions { get; set; }

      public PointF Location
      {
         get { return TopLeft + Size.Times(0.1F); }
         set { TopLeft = value - Size.Times(0.1F); }
      }

      public PointF Center
      {
         get { return TopLeft + Size.Times(0.5F); }
         set { TopLeft = value - Size.Times(0.5F); }
      }

      public DiagramModel()
      {
         Id = DateTime.Now.Millisecond.ToString();
         UndoManager = new GoUndoManager();
         _nodes = new Cache<string, IBaseNode>(item => item.Id, x => null);
      }

      public override void Clear()
      {
         _nodes.Clear();
         base.Clear();
      }

      public void RefreshSize()
      {
         Bounds = ComputeBounds();
      }

      public void ClearUndoStack()
      {
         UndoManager.Clear();
      }

      private bool _inUpdate = false;

      public bool InUpdate
      {
         get { return _inUpdate; }
      }

      public void BeginUpdate()
      {
         SuspendsUpdates = true;
         SuspendsRouting = true;
         _inUpdate = true;
      }

      public void EndUpdate()
      {
         _inUpdate = false;

         SuspendsRouting = false;
         DoDelayedRouting(null);

         SuspendsUpdates = false;

         RectangleF b = ComputeBounds();
         Size = b.Size;
         TopLeft = b.Location;

         EnsureUniquePartID();
         InvalidateViews();
      }

      public IDiagramModel CreateCopy(string containerId = null)
      {
         IContainerBase source = null;
         if (String.IsNullOrEmpty(containerId))
            source = this;
         else if (_nodes.Contains(containerId))
            source = GetNode<IContainerNode>(containerId);

         // cannot create copy for other
         if (source == null)
            return null;


         var diagramModel = new DiagramModel { DiagramOptions = DiagramOptions };
         var goCollection = new GoCollection { source as GoNode };

         var copy = diagramModel.CopyFromCollection(goCollection);
         // _nodes is empty - overridden Add method seems not to be used in CopyFromCollection
         foreach (var node in diagramModel.GetAllChildren<IBaseNode>())
            diagramModel.AddNodeId(node);

         return diagramModel;
      }

      public void ReplaceNodeIds(IDictionary<string, string> changedIds)
      {
         foreach (string oldId in changedIds.Keys)
            if (_nodes.Contains(oldId))
            {
               var node = _nodes[oldId];
               node.Id = changedIds[oldId];
               _nodes.Remove(oldId);
               _nodes.Add(node);
               Console.WriteLine(oldId + "->" + node.Id + " " + node.Name);
            }
      }

      public new bool IsEmpty()
      {
         return (Count == 0);
      }

      public bool IsLayouted { get; set; }

      public RectangleF CalculateBounds()
      {
         return ComputeBounds();
      }

      public void SetHiddenRecursive(bool hidden)
      {
         foreach (var node in GetDirectChildren<IBaseNode>())
         {
            var topContainer = node as IContainerBase;
            if (topContainer != null) topContainer.SetHiddenRecursive(hidden);
            else node.Hidden = hidden;
         }
      }

      public void Collapse(int level)
      {
         foreach (var topContainer in GetDirectChildren<IContainerBase>()) topContainer.Collapse(level - 1);
      }

      public void Expand(int level)
      {
         foreach (var topContainer in GetDirectChildren<IContainerBase>()) topContainer.Expand(level - 1);
      }

      public void SetDefaultExpansion()
      {
         foreach (var container in GetAllChildren<IContainerNode>()) container.IsExpandedByDefault = container.IsExpanded;
      }

      public void ShowDefaultExpansion()
      {
         foreach (var container in GetAllChildren<IContainerNode>()) container.IsExpanded = container.IsExpandedByDefault;
      }

      public virtual void PostLayoutStep()
      {
         foreach (INeighborhoodNode node in GetDirectChildren<INeighborhoodNode>())
            node.AdjustPosition();
      }

      public IBaseNode GetNode(string id)
      {
         return _nodes[id];
      }

      public T GetNode<T>(string id) where T : class, IBaseNode
      {
         return GetNode(id) as T;
      }

      public T CreateNode<T>(string id, PointF location, IContainerBase parentContainerBase) where T : class, IBaseNode, new()
      {
         // create node and add it to _nodes collection
         T node = new T { Id = id, Location = location };

         if (node.IsAnImplementationOf<IElementBaseNode>() && DiagramOptions != null)
         {
            var elementBaseNode = (IElementBaseNode)node;
            if (elementBaseNode.IsAnImplementationOf<MoleculeNode>()) elementBaseNode.NodeSize = DiagramOptions.DefaultNodeSizeMolecule;
            else if (elementBaseNode.IsAnImplementationOf<ReactionNode>()) elementBaseNode.NodeSize = DiagramOptions.DefaultNodeSizeReaction;
            else if (elementBaseNode.IsAnImplementationOf<ObserverNode>()) elementBaseNode.NodeSize = DiagramOptions.DefaultNodeSizeObserver;
         }

         _nodes.Add(node);

         parentContainerBase.AddChildNode(node);

         return node;
      }

      public void RemoveNode(string id)
      {
         IBaseNode node = GetNode(id);
         if (node == null) return;

         IContainerBase parent = node.GetParent();
         parent.RemoveChildNode(node);

         if (_nodes.Contains(id)) _nodes.Remove(id);
      }

      public void RenameNode(string id, string name)
      {
         IBaseNode node = GetNode(id);
         if (node == null) return;
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

      public override void Add(GoObject goObject)
      {
         base.Add(goObject);
         AddNodeId(goObject as IBaseNode);
      }

      public override bool Remove(GoObject goObject)
      {
         RemoveNodeId(goObject as IBaseNode);
         return base.Remove(goObject);
      }

      public IEnumerable<T> GetDirectChildren<T>() where T : class
      {
         IList<T> children = new List<T>();
         foreach (GoObject child in this)
         {
            T childAsT = child as T;
            if (childAsT != null) children.Add(childAsT);
         }
         return children;
      }

      public IEnumerable<T> GetAllChildren<T>() where T : class
      {
         // returns all children, e.g. links, too
         IList<T> children = (IList<T>)GetDirectChildren<T>();
         foreach (var topNode in GetDirectChildren<IContainerNode>())
            foreach (var child in topNode.GetAllChildren<T>())
               children.Add(child);

         return children;
      }

      public void AddChildNode(IBaseNode node)
      {
         GoObject nodeAsGoObject = node as GoObject;
         if (nodeAsGoObject != null) Add(nodeAsGoObject);
         if (nodeAsGoObject == null) throw new InvalidTypeException(node, typeof(GoObject));
      }

      public void RemoveChildNode(IBaseNode node)
      {
         Remove(node as GoObject);
      }

      public bool ContainsChildNode(IBaseNode node, bool recursive)
      {
         if (recursive)
            return _nodes.Contains(node.Id);
         return (node.GetParent() == this);
      }

      public override string ToString()
      {
         return ObjectTypes.DiagramModel;
      }

      public virtual IDiagramModel Create()
      {
         return new DiagramModel();
      }
   }
}