using DevExpress.Utils;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.UI.Views.Diagram;

namespace OSPSuite.UI.Diagram
{
   public class concern_for_ReactionConnectionPoints : StaticContextSpecification
   {
      [Observation]
      public void should_place_the_educt_bottom_left_the_product_bottom_right_and_the_modifier_at_the_apex_by_default()
      {
         var points = ReactionConnectionPoints.For(displayEductsRight: false);
         points.Count.ShouldBeEqualTo(3);
         points[ReactionConnectionPoints.EDUCT_INDEX].ShouldBeEqualTo(new PointFloat(0F, 1F));
         points[ReactionConnectionPoints.PRODUCT_INDEX].ShouldBeEqualTo(new PointFloat(1F, 1F));
         points[ReactionConnectionPoints.MODIFIER_INDEX].ShouldBeEqualTo(new PointFloat(0.5F, 0F));
      }

      [Observation]
      public void should_swap_educt_and_product_when_educts_are_displayed_right()
      {
         var points = ReactionConnectionPoints.For(displayEductsRight: true);
         points.Count.ShouldBeEqualTo(3);
         points[ReactionConnectionPoints.EDUCT_INDEX].ShouldBeEqualTo(new PointFloat(1F, 1F));
         points[ReactionConnectionPoints.PRODUCT_INDEX].ShouldBeEqualTo(new PointFloat(0F, 1F));
         points[ReactionConnectionPoints.MODIFIER_INDEX].ShouldBeEqualTo(new PointFloat(0.5F, 0F));
      }

      [Observation]
      public void should_map_link_types_to_fixed_point_indices()
      {
         ReactionConnectionPoints.IndexFor(ReactionLinkType.Educt).ShouldBeEqualTo(0);
         ReactionConnectionPoints.IndexFor(ReactionLinkType.Product).ShouldBeEqualTo(1);
         ReactionConnectionPoints.IndexFor(ReactionLinkType.Modifier).ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_map_point_indices_back_to_link_types()
      {
         ReactionConnectionPoints.LinkTypeFor(0).ShouldBeEqualTo(ReactionLinkType.Educt);
         ReactionConnectionPoints.LinkTypeFor(1).ShouldBeEqualTo(ReactionLinkType.Product);
         ReactionConnectionPoints.LinkTypeFor(2).ShouldBeEqualTo(ReactionLinkType.Modifier);
      }

      [Observation]
      public void should_return_no_link_type_for_a_border_glue_or_unknown_index()
      {
         ReactionConnectionPoints.LinkTypeFor(-1).ShouldBeNull();
         ReactionConnectionPoints.LinkTypeFor(3).ShouldBeNull();
      }
   }
}
