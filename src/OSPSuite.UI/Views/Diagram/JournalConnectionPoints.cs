using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraDiagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.UI.Views.Diagram
{
   public enum JournalPort
   {
      Parent,
      Child,
      RelatedItem
   }

   public static class JournalConnectionPoints
   {
      public const int PARENT_INDEX = 0;
      public const int CHILD_INDEX = 1;
      public const int RELATED_ITEM_INDEX = 2;
      public const float RELATED_ITEM_RELATIVE_X = JournalPageNode.RELATED_ITEM_RELATIVE_X;

      public static readonly PointCollection PagePoints = new PointCollection(new[]
      {
         new PointFloat(0F, 0.5F),
         new PointFloat(1F, 0.5F),
         new PointFloat(RELATED_ITEM_RELATIVE_X, 1F)
      });

      public static readonly PointCollection RelatedItemPoints = new PointCollection(new[] {new PointFloat(0.5F, 0.5F)});

      public static JournalPort? PortFor(int pointIndex)
      {
         switch (pointIndex)
         {
            case PARENT_INDEX:
               return JournalPort.Parent;
            case CHILD_INDEX:
               return JournalPort.Child;
            case RELATED_ITEM_INDEX:
               return JournalPort.RelatedItem;
            default:
               return null;
         }
      }
   }
}
