using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public abstract class DiagramNode : IBaseNode
   {
      private readonly List<IBaseLink> _links = new List<IBaseLink>();
      private string _name;
      private string _description;
      private bool _hidden;
      private bool _isVisible = true;
      private bool _locationFixed;
      private PointF _location;

      public string Id { get; set; }
      public int UserFlags { get; set; }
      public IContainerBase Parent { get; internal set; }
      internal DiagramModel Model { get; set; }

      public virtual string Name
      {
         get => _name;
         set => SetField(ref _name, value);
      }

      public string Description
      {
         get => _description;
         set => SetField(ref _description, value);
      }

      public virtual bool Hidden
      {
         get => _hidden;
         set => SetField(ref _hidden, value);
      }

      public bool IsVisible
      {
         get => _isVisible;
         set => SetField(ref _isVisible, value);
      }

      public bool LocationFixed
      {
         get => _locationFixed;
         set => SetField(ref _locationFixed, value);
      }

      public virtual bool Visible
      {
         get => IsVisible && !Hidden && parentVisible();
         set => IsVisible = value;
      }

      private bool parentVisible()
      {
         if (Parent is ContainerNode container)
            return container.Visible && container.IsExpanded;

         return true;
      }

      public virtual PointF Location
      {
         get => _location;
         set => SetField(ref _location, value);
      }

      public abstract PointF Center { get; set; }
      public abstract SizeF Size { get; set; }
      public abstract RectangleF Bounds { get; set; }

      public IReadOnlyList<IBaseLink> Links => _links;

      public IContainerBase GetParent() => Parent;

      public virtual void AddLinkFrom(IBaseLink link) => addLink(link);

      public virtual void AddLinkTo(IBaseLink link) => addLink(link);

      private void addLink(IBaseLink link)
      {
         if (_links.Contains(link))
            return;

         _links.Add(link);
         NotifyChanged();
      }

      internal void RemoveLink(IBaseLink link)
      {
         if (_links.Remove(link))
            NotifyChanged();
      }

      public IEnumerable<T> GetLinkedNodes<T>() where T : class, IHasLayoutInfo
      {
         return _links.Select(link => link.GetOtherNode(this)).OfType<T>().Distinct().ToList();
      }

      public IEnumerable<IContainerNode> ParentNodes
      {
         get
         {
            var parent = Parent as IContainerNode;
            while (parent != null)
            {
               yield return parent;
               parent = parent.GetParent() as IContainerNode;
            }
         }
      }

      public virtual void ShowParents()
      {
         ParentNodes.Each(parent => parent.Hidden = false);
      }

      public void ToFront()
      {
         var children = parentChildren();
         if (children == null)
            return;

         children.MoveToFront(this);
         NotifyChanged();
      }

      public void ToBack()
      {
         var children = parentChildren();
         if (children == null)
            return;

         children.MoveToBack(this);
         NotifyChanged();
      }

      private NodeCollection parentChildren()
      {
         switch (Parent)
         {
            case DiagramModel model:
               return model.Children;
            case ContainerNode container:
               return container.Children;
            default:
               return null;
         }
      }

      public abstract void SetColorFrom(IDiagramColors diagramColors);

      public virtual void CopyLayoutInfoFrom(IBaseNode node, PointF parentLocation)
      {
         if (node == null)
            return;

         var location = node.Location;
         if (Parent != null && node.GetParent() != null)
            location = location.Plus(parentLocation.Minus(node.GetParent().Location));

         Bounds = node.Bounds;
         Location = location;
         LocationFixed = node.LocationFixed;
         Hidden = node.Hidden;
         IsVisible = node.IsVisible;
      }

      public abstract IBaseNode Copy();

      protected void CopyBasePropertiesTo(DiagramNode target)
      {
         target.Id = Id;
         target.Name = Name;
         target.Description = Description;
         target.UserFlags = UserFlags;
         target.Hidden = Hidden;
         target.IsVisible = IsVisible;
         target.LocationFixed = LocationFixed;
         target.Location = Location;
      }

      protected void SetField<T>(ref T field, T value)
      {
         if (EqualityComparer<T>.Default.Equals(field, value))
            return;

         field = value;
         NotifyChanged();
      }

      internal void NotifyChanged() => Model?.NotifyChanged();
   }
}
