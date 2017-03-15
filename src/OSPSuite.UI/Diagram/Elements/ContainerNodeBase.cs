using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public abstract class ContainerNodeBase : CustomSubGraph, IContainerNode
   {
      private bool _locationFixed;
      public string Id { get; set; }

      protected ContainerPortBase GetPort()
      {
         return Port as ContainerPortBase;
      }

      //constructor not called in deserialization for each object, but only once for prototype, therefore implement CopyObject if reference members exist
      protected ContainerNodeBase()
      {
         IsVisible = true;
         AutoRescales = false;
         ToolTipText = "";
         Text = "";

         IsLogical = false;
         IsExpandedByDefault = false;

         Corner = new SizeF(10, 10);
         TopLeftMargin = new SizeF(15, 3);
         BottomRightMargin = new SizeF(3, 3);
         CollapsedCorner = new SizeF(5, 5);
         CollapsedTopLeftMargin = new SizeF(5, 5);
         CollapsedBottomRightMargin = new SizeF(5, 5);

         Label.Editable = false;
         LocationFixed = false;
         PickableBackground = true;
      }

      protected override GoPort CreatePort()
      {
         return new ContainerPortBase();
      }

      public override void LayoutLabel()
      {
         if (IsExpanded)
         {
            base.LayoutLabel();
            return;
         } 

         base.LayoutLabel();
      }

      protected DiagramModel DiagramModel
      {
         get { return Document as DiagramModel; }
      }

      public string Name
      {
         get { return Text; }
         set { Text = value; }
      }

      public string Description
      {
         get { return ToolTipText; }
         set { ToolTipText = value; }
      }

      public IContainerBase GetParent()
      {
         if (Parent == null) return Document as IContainerBase;
         return Parent as IContainerBase;
      }

      public bool IsLogical { get; set; }

      public virtual void SetColorFrom(IDiagramColors diagramColors)
      {
         Opacity = diagramColors.ContainerOpacity*100F;

         if (_locationFixed)
         {
            BorderPenWidth = 2.0F;
            BorderPenColor = diagramColors.BorderFixed;
         }
         else
         {
            BorderPenWidth = 1.0F;
            BorderPenColor = diagramColors.BorderUnfixed;
         }

         if (IsLogical) BackgroundColor = diagramColors.ContainerLogical;
         else BackgroundColor = diagramColors.ContainerPhysical;

         Handle.BrushColor = diagramColors.ContainerHandle;
      }

      // do not consider inVisible nodes (like MoleculeProperties) in Bounds calculation
      protected override bool ComputeInsideMarginsSkip(GoObject child)
      {
         var containerNode = child as IContainerNode;

         // do not exclude MoleculeProperties Container from Bounds calculation, if it is located near to ParentContainer
         if (containerNode != null && containerNode.Name == Constants.MOLECULE_PROPERTIES && (Bounds.Location.DistanceTo(containerNode.Location)) < 30)
            return false;

         var hasLayoutInfo = child as IHasLayoutInfo;
         if (hasLayoutInfo != null && !hasLayoutInfo.IsVisible) 
         return true;
         return base.ComputeInsideMarginsSkip(child);
      }


      // Visible used by Go to hide nodes in collapsed subgraphs, overridden: setter sets base.Visible, getter evaluates base.Visible, Hidden, IsVisible
      public override bool Visible
      {
         get { return base.Visible && IsVisible && !Hidden; }
         set { base.Visible = value; } // base.Label.Visible = value; }
      }

      public bool IsVisible { get; set; }

      private bool _hidden;

      public virtual bool Hidden
      {
         get { return _hidden; }
         set
         {
            _hidden = value;
            var containerNode = GetParent() as IContainerNode;
            if (value == false && containerNode != null) containerNode.Hidden = false;
         }
      }

      public RectangleF CalculateBounds()
      {
         return ComputeBounds();
      }

      public void SetHiddenRecursive(bool hidden)
      {
         // visible nodes are only displayed, if parent subgraphnodes are also visible
         if (!hidden) ShowParents();

         Hidden = hidden;
         foreach (var child in GetAllChildren<IBaseNode>()) child.Hidden = hidden;
      }

      public void ShowChildrenAndLinkedNodes()
      {
         foreach (var childNode in GetDirectChildren<IBaseNode>()) childNode.Hidden = false;
         foreach (var neighborNode in GetLinkedNodes<IBaseNode>(true)) neighborNode.Hidden = false;
      }

      public virtual void PostLayoutStep()
      {
         var oldLocation = Location;
         foreach (INeighborhoodNode node in GetDirectChildren<INeighborhoodNode>()) node.AdjustPosition();
         foreach (var neighborhoodNode in GetLinkedNodes<INeighborhoodNode>(true)) neighborhoodNode.AdjustPosition();
         Location = oldLocation;
      }

      public virtual void ShowParents()
      {
         foreach (var parent in this.GetParentNodes())
         {
            parent.Hidden = false;
         }
      }

      public void Collapse(int level)
      {
         if (level > 0) foreach (var childContainer in GetDirectChildren<IContainerNode>()) childContainer.Collapse(level - 1);
         if (level >= 0) base.Collapse();
      }

      public void Expand(int level)
      {
         if (level >= 0) base.Expand();
         if (level > 0) foreach (var childContainer in GetDirectChildren<IContainerNode>()) childContainer.Expand(level - 1);
      }

      public bool IsExpandedByDefault { get; set; }

      public override void Add(GoObject obj)
      {
         if (obj == null) return;
         base.Add(obj);

         //DiagramModel is null in deserialization process therefore fill DiagramModel._nodes in DiagramSerializer.XmlDocumentToModel
         if (obj.IsAnImplementationOf<IBaseNode>() && DiagramModel != null) DiagramModel.AddNodeId(obj as IBaseNode);
      }

      public override bool Remove(GoObject obj)
      {
         if (obj.IsAnImplementationOf<IBaseNode>() && DiagramModel != null) DiagramModel.RemoveNodeId(obj as IBaseNode);
         return base.Remove(obj);
      }

      public virtual void AddChildNode(IBaseNode node)
      {
         GoObject nodeAsGoObject = node as GoObject;
         if (nodeAsGoObject != null) Add(nodeAsGoObject);
      }

      public virtual void RemoveChildNode(IBaseNode node)
      {
         Remove(node as GoObject);
      }

      public bool ContainsChildNode(IBaseNode node, bool recursive)
      {
         // returns true, if this is within the Parent-hierarchy of node
         if (node == this) return true;
         return (((GoObject) node).IsChildOf(this));
      }

      public IEnumerable<T> GetDirectChildren<T>() where T : class
      {
         IList<T> children = new List<T>();
         addChildren<T, T>(this, children, false);
         return children;
      }

      public IEnumerable<T> GetAllChildren<T>() where T : class
      {
         IList<T> children = new List<T>();
         addChildren<T, T>(this, children, true);
         return children;
      }

      private void addUnique<T>(IList<T> list, T item)
      {
         if (!list.Contains(item)) list.Add(item);
      }

      private void addChildren<TList, T>(ContainerNodeBase containerBaseNode, IList<TList> children, bool recursive)
         where T : class, TList
      {
         foreach (GoObject child in Enumerable.Where<GoObject>(containerBaseNode, child => ConversionExtensions.IsAnImplementationOf<T>(child)))
            addUnique(children, child as T);

         if (recursive)
            foreach (GoObject child in Enumerable.Where<GoObject>(containerBaseNode, child => ConversionExtensions.IsAnImplementationOf<ContainerNodeBase>(child)))
               addChildren<TList, T>(child as ContainerNodeBase, children, recursive);
      }

      public virtual void AddLinkFrom(IBaseLink link)
      {
         Port.AddDestinationLink((GoLink) link);
      }

      public virtual void AddLinkTo(IBaseLink link)
      {
         Port.AddSourceLink((GoLink) link);
      }

      public virtual IEnumerable<T> GetLinkedNodes<T>() where T : class, IHasLayoutInfo
      {
         IList<T> linkedNodes = new List<T>();
         foreach (IGoLink link in Port.Links)
         {
            T linkedNode = link.GetOtherNode(this) as T;
            if (linkedNode != null) linkedNodes.Add(linkedNode);
         }
         return linkedNodes;
      }

      // returns external linked Nodes
      public IEnumerable<T> GetLinkedNodes<T>(bool recursive) where T : class, IBaseNode
      {
         IEnumerable<T> linkedNodes = GetLinkedNodes<T>();
         if (!recursive) return linkedNodes;

         IList<IBaseNode> children = new List<IBaseNode>();
         addChildren<IBaseNode, IContainerNode>(this, children, true);

         foreach (var containerNode in children)
         {
            linkedNodes = linkedNodes.Union(
               containerNode.GetLinkedNodes<T>()
                  .Where(n => !ContainsChildNode(n, true)));
         }

         return linkedNodes;
      }

      public void ClearLinks()
      {
         Port.ClearLinks();
      }

      public bool LocationFixed
      {
         get { return _locationFixed; }
         set
         {
            _locationFixed = value;
         }
      }

      public void ToFront()
      {
         BaseDiagramExtensions.ToFront(this);
      }

      public void ToBack()
      {
         BaseDiagramExtensions.ToBack(this);
      }

      public virtual void CopyLayoutInfoFrom(IBaseNode baseNode, PointF parentLocation)
      {
         var node = baseNode as IContainerNode;
         if (node == null) return;

         // copy location relative to parent
         PointF location = node.Location;
         if (GetParent() != null && node.GetParent() != null)
            location = location.Plus(parentLocation.Minus(node.GetParent().Location));

         Location = location;
         LocationFixed = node.LocationFixed;
         Hidden = node.Hidden;
         IsVisible = node.IsVisible;
         IsExpanded = node.IsExpanded;
         IsExpandedByDefault = node.IsExpandedByDefault;
         CopySavedBoundsFrom(node);
      }

      public new IBaseNode Copy()
      {
         return base.Copy() as IBaseNode;
      }

      // allows LayoutCopyService to copy node positions inside collapsed subgraphs
      protected void CopySavedBoundsFrom(IContainerNode otherContainer)
      {
         try
         {
            var otherSubGraph = otherContainer as GoSubGraph;
            if (otherSubGraph == null) return;

            // build Cache of GoNode children
            ICache<string, GoNode> childNodes = new Cache<string, GoNode>(x => x.Text); //Text = Name
            
            foreach (GoNode childNode in Enumerable.Where<GoObject>(this, child => ConversionExtensions.IsAnImplementationOf<GoNode>(child)))
            {
               if (!String.IsNullOrEmpty(childNode.Text))
                  addUnique(childNodes, childNode.Text, childNode);
               else if (childNode.IsAnImplementationOf<INeighborhoodNode>()) //childNode.Text is empty and childNode is NeighborhoodNode
                  addUnique(childNodes, ((INeighborhoodNode) childNode).Name, childNode);
            }

            // store SavedBounds for each child GoNode with the same name
            foreach (GoNode otherNode in otherSubGraph.SavedBounds.Keys.Where(child => child.IsAnImplementationOf<GoNode>()))
            {
               string name = otherNode.Text;
               if (String.IsNullOrEmpty(name) && otherNode.IsAnImplementationOf<INeighborhoodNode>())
                  name = ((INeighborhoodNode) otherNode).Name;

               if (!String.IsNullOrEmpty(name) && childNodes.Contains(name))
               {
                  var childNode = childNodes[name];
                  if (SavedBounds.ContainsKey(childNode)) SavedBounds[childNode] = otherSubGraph.SavedBounds[otherNode];
                  else SavedBounds.Add(childNode, otherSubGraph.SavedBounds[otherNode]);
               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
         }
      }

      private void addUnique(ICache<string, GoNode> childNodes, string name, GoNode childNode)
      {
         if (!childNodes.Contains(name)) childNodes.Add(name, childNode);
      }

      private string goInfo(GoObject go)
      {
         if (go == null) return "null";
         string info = " B=" + go.Bounds.ToString();
         var baseNode = go as IBaseNode;
         if (baseNode != null) info = baseNode.GetLongName() + info;
         else info = go.GetType().Name + info;
         return info;
      }

      private static bool debug = false;

      private void print(string line)
      {
         if (debug) Console.WriteLine(line);
      }

      public override SizeF ComputeCollapsedSize(bool visible)
      {
         print("< ComputeCollapsedSize " + visible + " CO=" + goInfo(CollapsedObject));

         var collapsedSize = base.ComputeCollapsedSize(true);
         if (!visible && collapsedSize.IsEmpty) collapsedSize = Size; 

         print("| ComputeCollapsedSize " + collapsedSize);
         foreach (var child in this)
            if (!ComputeCollapsedSizeSkip(child) && child.Width > collapsedSize.Width && child as GoSubGraph == null)
               collapsedSize.Width = child.Width;

         print("> ComputeCollapsedSize " + collapsedSize);
         return collapsedSize;
      }

      protected override GoObject CreateCollapsedObject()
      {
         var co = base.CreateCollapsedObject();
         print("| CreateCollapsedObject " + goInfo(co));
         return co;
      }
   }
}