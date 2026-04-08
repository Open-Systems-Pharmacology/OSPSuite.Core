using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Elements;

namespace OSPSuite.UI
{
   public abstract class concern_for_ForceLayoutConfiguration : ContextSpecification<ForceLayoutConfiguration>
   {
      protected override void Context()
      {
         sut = new ForceLayoutConfiguration();
      }
   }

   public class When_cloning_force_layout_configuration : concern_for_ForceLayoutConfiguration
   {
      private IForceLayoutConfiguration _clone;

      protected override void Context()
      {
         base.Context();
         sut.BaseGravitationalMass = 5F;
         sut.BaseElectricalCharge = 100F;
         sut.BaseSpringLength = 50F;
         sut.BaseSpringStiffness = 0.5F;
         sut.MaxIterations = 500;
         sut.Epsilon = 0.05F;
         sut.InfinityDistance = 1000F;
         sut.ArrangementSpacingWidth = 200F;
         sut.ArrangementSpacingHeight = 150F;
         sut.LogPositions = true;
         sut.RelativeElectricalChargeMolecule = 3F;
         sut.RelativeGravitationalMassMolecule = 1.5F;
         sut.RelativeSpringLengthContainerContainer = 4F;
         sut.RelativeSpringStiffnessContainerContainer = 2F;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_create_a_new_instance()
      {
         ReferenceEquals(_clone, sut).ShouldBeFalse();
      }

      [Observation]
      public void should_copy_base_properties()
      {
         _clone.BaseGravitationalMass.ShouldBeEqualTo(5F);
         _clone.BaseElectricalCharge.ShouldBeEqualTo(100F);
         _clone.BaseSpringLength.ShouldBeEqualTo(50F);
         _clone.BaseSpringStiffness.ShouldBeEqualTo(0.5F);
      }

      [Observation]
      public void should_copy_iteration_properties()
      {
         _clone.MaxIterations.ShouldBeEqualTo(500);
         _clone.Epsilon.ShouldBeEqualTo(0.05F);
         _clone.InfinityDistance.ShouldBeEqualTo(1000F);
         _clone.ArrangementSpacingWidth.ShouldBeEqualTo(200F);
         _clone.ArrangementSpacingHeight.ShouldBeEqualTo(150F);
         _clone.LogPositions.ShouldBeTrue();
      }

      [Observation]
      public void should_copy_relative_charge_and_mass_arrays()
      {
         _clone.RelativeElectricalChargeMolecule.ShouldBeEqualTo(3F);
         _clone.RelativeGravitationalMassMolecule.ShouldBeEqualTo(1.5F);
      }

      [Observation]
      public void should_copy_relative_spring_matrices()
      {
         _clone.RelativeSpringLengthContainerContainer.ShouldBeEqualTo(4F);
         _clone.RelativeSpringStiffnessContainerContainer.ShouldBeEqualTo(2F);
      }

      [Observation]
      public void should_not_share_array_references_with_original()
      {
         ReferenceEquals(_clone.RelativeGravitationalMassOf, sut.RelativeGravitationalMassOf).ShouldBeFalse();
         ReferenceEquals(_clone.RelativeElectricalChargeOf, sut.RelativeElectricalChargeOf).ShouldBeFalse();
         ReferenceEquals(_clone.RelativeSpringLengthOf, sut.RelativeSpringLengthOf).ShouldBeFalse();
         ReferenceEquals(_clone.RelativeSpringStiffnessOf, sut.RelativeSpringStiffnessOf).ShouldBeFalse();
      }
   }

   public class When_updating_force_layout_configuration_from_another_instance : concern_for_ForceLayoutConfiguration
   {
      private ForceLayoutConfiguration _source;

      protected override void Context()
      {
         base.Context();
         _source = new ForceLayoutConfiguration
         {
            BaseGravitationalMass = 10F,
            BaseElectricalCharge = 200F,
            MaxIterations = 2000,
            LogPositions = true,
            RelativeElectricalChargeReaction = 5F,
            RelativeSpringLengthReactionMolecule = 3F
         };
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_source);
      }

      [Observation]
      public void should_update_base_properties()
      {
         sut.BaseGravitationalMass.ShouldBeEqualTo(10F);
         sut.BaseElectricalCharge.ShouldBeEqualTo(200F);
         sut.MaxIterations.ShouldBeEqualTo(2000);
         sut.LogPositions.ShouldBeTrue();
      }

      [Observation]
      public void should_update_relative_properties()
      {
         sut.RelativeElectricalChargeReaction.ShouldBeEqualTo(5F);
         sut.RelativeSpringLengthReactionMolecule.ShouldBeEqualTo(3F);
      }
   }
}
