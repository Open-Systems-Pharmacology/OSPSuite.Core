using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class ContainerNode : DiagramNode, IContainerNode
   {
      private const float FIXED_BORDER_WIDTH = 2F;
      private const float UNFIXED_BORDER_WIDTH = 1F;

      private bool _isExpanded = true;
      private bool _isLogical;
      private SizeF _size = new SizeF(100, 60);

      internal NodeCollection Children { get; } = new NodeCollection();

      public Color BackgroundColor { get; private set; }
      public Color BorderColor { get; private set; }
      public float BorderWidth { get; private set; } = UNFIXED_BORDER_WIDTH;
      public bool IsExpandedByDefault { get; set; }

      public ContainerNode()
      {
         UserFlags = NodeLayoutType.CONTAINER_NODE;
      }

      public bool IsLogical
      {
         get => _isLogical;
         set => SetField(ref _isLogical, value);
      }

      public bool IsExpanded
      {
         get => _isExpanded;
         set => SetField(ref _isExpanded, value);
      }

      public override bool Hidden
      {
         get => base.Hidden;
         set
         {
            base.Hidden = value;
            if (!value && Parent is IContainerNode parentContainer)
               parentContainer.Hidden = false;
         }
      }

      public override PointF Location
      {
         get => base.Location;
         set
         {
            var delta = value.Minus(base.Location);
            base.Location = value;
            if (delta.IsEmpty)
               return;

            Children.Each(child => child.Location = child.Location.Plus(delta));
         }
      }

      public override PointF Center
      {
         get => new PointF(Location.X + Size.Width / 2, Location.Y + Size.Height / 2);
         set => Location = new PointF(value.X - Size.Width / 2, value.Y - Size.Height / 2);
      }

      public override SizeF Size
      {
         get => _size;
         set => SetField(ref _size, value);
      }

      public override RectangleF Bounds
      {
         get => new RectangleF(Location, Size);
         set
         {
            Location = value.Location;
            Size = value.Size;
         }
      }

      public RectangleF CalculateBounds()
      {
         var visibleChildren = Children.Where(node => node.IsVisible).ToList();
         if (!visibleChildren.Any())
            return Bounds;

         return visibleChildren.Select(node => node.Bounds).Aggregate(RectangleF.Union);
      }

      public void SetHiddenRecursive(bool hidden)
      {
         if (!hidden) ShowParents();

         Hidden = hidden;
         GetAllChildren<IBaseNode>().Each(child => child.Hidden = hidden);
      }

      public void ShowChildrenAndLinkedNodes()
      {
         GetDirectChildren<IBaseNode>().Each(childNode => childNode.Hidden = false);

         GetLinkedNodes<IBaseNode>(true).Each(neighborNode => neighborNode.Hidden = false);
      }

      public void PostLayoutStep()
      {
         GetDirectChildren<INeighborhoodNode>().Each(node => node.AdjustPosition());

         GetLinkedNodes<INeighborhoodNode>(true).Each(neighborhoodNode => neighborhoodNode.AdjustPosition());
      }

      public void Collapse(int level)
      {
         if (level > 0)
            GetDirectChildren<IContainerNode>().Each(childContainer => childContainer.Collapse(level - 1));

         if (level >= 0) IsExpanded = false;
      }

      public void Expand(int level)
      {
         if (level >= 0) IsExpanded = true;

         if (level > 0)
            GetDirectChildren<IContainerNode>().Each(childContainer => childContainer.Expand(level - 1));
      }

      public IEnumerable<T> GetLinkedNodes<T>(bool recursive) where T : class, IBaseNode
      {
         var linkedNodes = GetLinkedNodes<T>().ToList();
         if (!recursive)
            return linkedNodes;

         foreach (var child in Children.OfType<DiagramNode>())
         {
            linkedNodes.AddRange(child is ContainerNode childContainer ? childContainer.GetLinkedNodes<T>(true) : child.GetLinkedNodes<T>());
         }

         return linkedNodes.Distinct().ToList();
      }

      public IEnumerable<T> GetDirectChildren<T>() where T : class
      {
         return Children.OfType<T>().Concat(linksOf(Children).OfType<T>()).Distinct().ToList();
      }

      public IEnumerable<T> GetAllChildren<T>() where T : class
      {
         var children = GetDirectChildren<T>().ToList();
         Children.OfType<IContainerNode>().Each(childContainer => children.AddRange(childContainer.GetAllChildren<T>()));

         return children.Distinct().ToList();
      }

      private static IEnumerable<IBaseLink> linksOf(IEnumerable<IBaseNode> nodes)
      {
         return nodes.OfType<DiagramNode>().SelectMany(node => node.Links).Distinct();
      }

      public void AddChildNode(IBaseNode node)
      {
         if (!(node is DiagramNode diagramNode))
            return;
         Children.Add(node);
         diagramNode.Parent = this;
         Model?.Attach(diagramNode, this);
         NotifyChanged();
      }

      public void RemoveChildNode(IBaseNode node)
      {
         if (!Children.Remove(node))
            return;
         if (node is DiagramNode diagramNode)
         {
            Model?.Detach(diagramNode);
            diagramNode.Parent = null;
         }

         NotifyChanged();
      }

      public bool ContainsChildNode(IBaseNode node, bool recursive)
      {
         if (node == this)
            return true;
         return recursive ? GetAllChildren<IBaseNode>().Contains(node) : Children.Contains(node);
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         BackgroundColor = IsLogical ? diagramColors.ContainerLogical : diagramColors.ContainerPhysical;
         BorderWidth = LocationFixed ? FIXED_BORDER_WIDTH : UNFIXED_BORDER_WIDTH;
         BorderColor = LocationFixed ? diagramColors.BorderFixed : diagramColors.BorderUnfixed;
         NotifyChanged();
      }

      public override void CopyLayoutInfoFrom(IBaseNode node, PointF parentLocation)
      {
         if (!(node is IContainerNode containerNode))
            return;
         base.CopyLayoutInfoFrom(node, parentLocation);
         IsExpanded = containerNode.IsExpanded;
         Size = containerNode.Size;
      }

      public override IBaseNode Copy()
      {
         var copy = new ContainerNode();
         CopyBasePropertiesTo(copy);
         copy.IsLogical = IsLogical;
         copy.IsExpanded = IsExpanded;
         copy.IsExpandedByDefault = IsExpandedByDefault;
         copy.Size = Size;
         return copy;
      }
   }
}
