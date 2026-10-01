using System.Drawing;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public abstract class BaseLink : IBaseLink
   {
      public IBaseNode FromNode { get; private set; }
      public IBaseNode ToNode { get; private set; }
      public bool IsVisible { get; set; } = true;
      public Color Color { get; protected set; }
      public bool IsDashed { get; protected set; }

      public virtual void Initialize(IBaseNode fromNode, IBaseNode toNode)
      {
         FromNode = fromNode;
         ToNode = toNode;
         fromNode.AddLinkFrom(this);
         toNode.AddLinkTo(this);
      }

      public IContainerBase GetParent() => FromNode?.GetParent();

      public bool Visible
      {
         get => IsVisible && (FromNode?.Visible ?? true) && (ToNode?.Visible ?? true);
         set => IsVisible = value;
      }

      public abstract void SetColorFrom(IDiagramColors diagramColors);

      public IBaseNode GetOtherNode(IBaseNode node)
      {
         if (node == FromNode)
            return ToNode;
         if (node == ToNode)
            return FromNode;
         return null;
      }

      public IBaseNode GetFromNode() => FromNode;

      public IBaseNode GetToNode() => ToNode;

      public void Unlink()
      {
         (FromNode as DiagramNode)?.RemoveLink(this);
         (ToNode as DiagramNode)?.RemoveLink(this);
      }

      protected void NotifyChanged()
      {
         (FromNode as DiagramNode)?.NotifyChanged();
      }
   }
}
