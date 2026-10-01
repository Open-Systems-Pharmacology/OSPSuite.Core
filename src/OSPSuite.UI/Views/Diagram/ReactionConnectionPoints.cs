using System;
using DevExpress.Utils;
using DevExpress.XtraDiagram;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Views.Diagram
{
   public static class ReactionConnectionPoints
   {
      public const int EDUCT_INDEX = 0;
      public const int PRODUCT_INDEX = 1;
      public const int MODIFIER_INDEX = 2;

      private static readonly PointFloat _bottomLeft = new PointFloat(0F, 1F);
      private static readonly PointFloat _bottomRight = new PointFloat(1F, 1F);
      private static readonly PointFloat _apex = new PointFloat(0.5F, 0F);

      public static PointCollection For(bool displayEductsRight)
      {
         return new PointCollection(new[]
         {
            displayEductsRight ? _bottomRight : _bottomLeft,
            displayEductsRight ? _bottomLeft : _bottomRight,
            _apex
         });
      }

      public static int IndexFor(ReactionLinkType linkType)
      {
         switch (linkType)
         {
            case ReactionLinkType.Educt:
               return EDUCT_INDEX;
            case ReactionLinkType.Product:
               return PRODUCT_INDEX;
            case ReactionLinkType.Modifier:
               return MODIFIER_INDEX;
            default:
               throw new ArgumentOutOfRangeException(nameof(linkType), linkType, null);
         }
      }

      public static ReactionLinkType? LinkTypeFor(int pointIndex)
      {
         switch (pointIndex)
         {
            case EDUCT_INDEX:
               return ReactionLinkType.Educt;
            case PRODUCT_INDEX:
               return ReactionLinkType.Product;
            case MODIFIER_INDEX:
               return ReactionLinkType.Modifier;
            default:
               return null;
         }
      }
   }
}
