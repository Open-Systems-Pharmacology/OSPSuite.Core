using System.Drawing;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Journal;

namespace OSPSuite.UI.Diagram.Elements
{
   public class RelatedItemNode : ElementBaseNode, IRelatedItemNode
   {
      private const int MAXIMUM_TITLE_LENGTH = 32;
      private const int ELLIPSIS_LENGTH = 3;
      public static readonly SizeF RelatedItemNodeSize = new SizeF(20, 20);

      public RelatedItemNode()
      {
         Shape = new GoEllipse();

         NodeBaseSize = RelatedItemNodeSize;
         NodeSize = NodeSize.Middle;

         Port.SetSpotLocation(Middle, new PointF(Shape.Left, (Shape.Top + Shape.Bottom) / 2));
         configureLabel();
      }

      private void configureLabel()
      {
         LabelSpot = MiddleRight;
         Label.Multiline = true;
         Label.Wrapping = true;
         Label.WrappingWidth = 200;
         Label.TextColor = Color.Black;
      }

      protected override void RefreshSize()
      {
         SetPortSize(1F);
         SetShapeSize();
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         Port.BrushColor = diagramColors.RelatedItemNode;

         foreach (var link in GetLinks<RelatedItemLink>()) link.SetColorFrom(diagramColors);
      }

      public override bool Printable
      {
         get { return base.Printable; }
         set
         {
            base.Printable = value;
            foreach (var destination in Destinations)
            {
               destination.GoObject.Printable = value;
            }
         }
      }

      public override bool Visible
      {
         get { return base.Visible; }
         set
         {
            base.Visible = value;
            foreach (var inlink in SourceLinks)
            {
               inlink.GoObject.Visible = true;
               inlink.GoObject.Printable = true;
            }
            foreach (var destination in Destinations)
            {
               destination.GoObject.Visible = value;
            }
         }
      }

      public void UpdateAttributesFromItem(RelatedItem item)
      {
         var maxNameLength = MAXIMUM_TITLE_LENGTH - item.ItemType.Length;
         configureLabel();
         Label.Text = $"  {(shouldShortenName(item, maxNameLength) ? shortenedName(item, maxNameLength) : item.Name)} ({item.ItemType})";
      }

      private static string shortenedName(RelatedItem item, int maxNameLength)
      {
         return maxNameLength >= ELLIPSIS_LENGTH ? item.Name.Substring(0, maxNameLength - ELLIPSIS_LENGTH).WithEllipsis() : string.Empty;
      }

      private static bool shouldShortenName(RelatedItem item, int maxNameLength)
      {
         return item.Name.Length > maxNameLength;
      }
   }
}