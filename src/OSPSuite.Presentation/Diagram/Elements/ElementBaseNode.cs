using System;
using System.Drawing;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class ElementBaseNode : DiagramNode, IElementBaseNode
   {
      private const float UNFIXED_BORDER_WIDTH = 1F;
      private const float FIXED_BORDER_WIDTH = 2F;
      private const float NODE_SIZE_PERCENT_BASE = 100F;
      private const float LARGE_LABEL_FONT_SIZE = 10F;
      private const float DEFAULT_LABEL_FONT_SIZE = 8F;
      private const int MAX_ALPHA = 255;
      private const int FALLBACK_ALPHA = 128;

      private NodeSize _nodeSize = NodeSize.Middle;
      private SizeF _nodeBaseSize = new SizeF(20, 20);
      private bool _canLink = true;

      public Color FillColor { get; protected set; }
      public Color PortColor { get; protected set; }
      public Color BorderColor { get; protected set; }
      public float BorderWidth { get; protected set; } = UNFIXED_BORDER_WIDTH;

      public SizeF NodeBaseSize
      {
         get => _nodeBaseSize;
         set => SetField(ref _nodeBaseSize, value);
      }

      public virtual NodeSize NodeSize
      {
         get => _nodeSize;
         set => SetField(ref _nodeSize, value);
      }

      public virtual bool CanLink
      {
         get => _canLink;
         set => SetField(ref _canLink, value);
      }

      public T FindChild<T>(string childName) => default;

      public override PointF Center
      {
         get => Location;
         set => Location = value;
      }

      private float scale => (int) NodeSize / NODE_SIZE_PERCENT_BASE;

      public override SizeF Size
      {
         get => new SizeF(scale * NodeBaseSize.Width, scale * NodeBaseSize.Height);
         set { }
      }

      public override RectangleF Bounds
      {
         get
         {
            var size = Size;
            return new RectangleF(Location.X - size.Width / 2, Location.Y - size.Height / 2, size.Width, size.Height);
         }
         set => Location = new PointF(value.X + value.Width / 2, value.Y + value.Height / 2);
      }

      public virtual bool LabelVisible => NodeSize != NodeSize.Small;

      public virtual float LabelFontSize => NodeSize == NodeSize.Large ? LARGE_LABEL_FONT_SIZE : DEFAULT_LABEL_FONT_SIZE;

      public virtual Color LabelColor => NodeSize == NodeSize.Middle ? SuiteColors.Gray : Color.Black;

      public int Alpha(float nodeSizeOpacity)
      {
         switch (NodeSize)
         {
            case NodeSize.Small:
               return Convert.ToInt16(nodeSizeOpacity * nodeSizeOpacity * MAX_ALPHA);
            case NodeSize.Middle:
               return Convert.ToInt16(nodeSizeOpacity * MAX_ALPHA);
            case NodeSize.Large:
               return MAX_ALPHA;
            default:
               return FALLBACK_ALPHA;
         }
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         BorderWidth = LocationFixed ? FIXED_BORDER_WIDTH : UNFIXED_BORDER_WIDTH;
         BorderColor = LocationFixed ? diagramColors.BorderFixed : diagramColors.BorderUnfixed;
         NotifyChanged();
      }

      protected void SetFillColorFrom(IDiagramColors diagramColors, Color color)
      {
         var nodeAlpha = Alpha(diagramColors.NodeSizeOpacity);
         FillColor = Color.FromArgb(Convert.ToInt16(diagramColors.PortOpacity * nodeAlpha), color);
         PortColor = Color.FromArgb(nodeAlpha, color);
      }

      public override void CopyLayoutInfoFrom(IBaseNode node, PointF parentLocation)
      {
         if (!(node is IElementBaseNode elementNode))
            return;
         base.CopyLayoutInfoFrom(node, parentLocation);
         NodeSize = elementNode.NodeSize;
      }

      public override IBaseNode Copy()
      {
         var copy = CreateInstance();
         CopyPropertiesTo(copy);
         return copy;
      }

      protected virtual ElementBaseNode CreateInstance() => new ElementBaseNode();

      protected virtual void CopyPropertiesTo(ElementBaseNode target)
      {
         CopyBasePropertiesTo(target);
         target.NodeBaseSize = NodeBaseSize;
         target.NodeSize = NodeSize;
         target.CanLink = CanLink;
         target.FillColor = FillColor;
         target.PortColor = PortColor;
         target.BorderColor = BorderColor;
         target.BorderWidth = BorderWidth;
      }
   }
}
