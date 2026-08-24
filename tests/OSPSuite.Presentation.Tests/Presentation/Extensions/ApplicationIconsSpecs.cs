using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Presentation.Extensions
{
   public abstract class concern_for_ApplicationIcons : StaticContextSpecification
   {
   }

   public class When_retrieving_the_orange_overlay_for_a_simulation_icon : concern_for_ApplicationIcons
   {
      [Observation]
      public void should_return_the_orange_overlay_variant_when_one_is_defined()
      {
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.SimulationGreen).ShouldBeEqualTo(ApplicationIcons.SimulationGreenOrange);
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.SimulationRed).ShouldBeEqualTo(ApplicationIcons.SimulationRedOrange);
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.PopulationSimulationGreen).ShouldBeEqualTo(ApplicationIcons.PopulationSimulationGreenOrange);
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.PopulationSimulationRed).ShouldBeEqualTo(ApplicationIcons.PopulationSimulationRedOrange);
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.AgingSimulationGreen).ShouldBeEqualTo(ApplicationIcons.AgingSimulationGreenOrange);
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.AgingSimulationRed).ShouldBeEqualTo(ApplicationIcons.AgingSimulationRedOrange);
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.AgingPopulationSimulationGreen).ShouldBeEqualTo(ApplicationIcons.AgingPopulationSimulationGreenOrange);
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.AgingPopulationSimulationRed).ShouldBeEqualTo(ApplicationIcons.AgingPopulationSimulationRedOrange);
      }

      [Observation]
      public void should_return_the_original_icon_when_no_orange_overlay_is_defined()
      {
         ApplicationIcons.OrangeOverlayFor(ApplicationIcons.Application).ShouldBeEqualTo(ApplicationIcons.Application);
      }
   }

   public class When_loading_the_orange_overlay_simulation_icons : concern_for_ApplicationIcons
   {
      [Observation]
      public void should_have_embedded_the_svg_resource_for_each_variant()
      {
         ApplicationIcons.SimulationGreenOrange.IconBytes.ShouldNotBeNull();
         ApplicationIcons.SimulationRedOrange.IconBytes.ShouldNotBeNull();
         ApplicationIcons.PopulationSimulationGreenOrange.IconBytes.ShouldNotBeNull();
         ApplicationIcons.PopulationSimulationRedOrange.IconBytes.ShouldNotBeNull();
         ApplicationIcons.AgingSimulationGreenOrange.IconBytes.ShouldNotBeNull();
         ApplicationIcons.AgingSimulationRedOrange.IconBytes.ShouldNotBeNull();
         ApplicationIcons.AgingPopulationSimulationGreenOrange.IconBytes.ShouldNotBeNull();
         ApplicationIcons.AgingPopulationSimulationRedOrange.IconBytes.ShouldNotBeNull();
      }
   }
}
