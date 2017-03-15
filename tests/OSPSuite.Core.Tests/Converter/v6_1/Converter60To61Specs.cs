using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Converter.v6_1;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Converter.v6_1
{
   public abstract class concern_for_Converter60To61 : ContextSpecification<Converter60To61>
   {
      protected override void Context()
      {
         base.Context();
         sut = new Converter60To61(new IdGenerator());
      }
   }

   public class When_converting_a_building_block_to_version_6_1 : concern_for_Converter60To61
   {
      private IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _buildingBlock.Icon = "Spatial Structure";
      }

      protected override void Because()
      {
         sut.Convert(_buildingBlock);
      }

      [Observation]
      public void should_remove_all_empty_space_in_the_icon_name()
      {
         _buildingBlock.Icon.ShouldBeEqualTo("SpatialStructure");
      }
   }

   public class When_converting_an_application_molecule_builder : concern_for_Converter60To61
   {
      private IEventGroupBuildingBlock _eventBuildingBlock;
      private IApplicationMoleculeBuilder _moleculeBuilder;

      protected override void Context()
      {
         base.Context();
         _eventBuildingBlock = new EventGroupBuildingBlock();
         var applicationBuilder = new ApplicationBuilder().WithName("App1");
         _moleculeBuilder = new ApplicationMoleculeBuilder();
         applicationBuilder.AddMolecule(_moleculeBuilder);
         _eventBuildingBlock.Add(applicationBuilder);
      }

      protected override void Because()
      {
         sut.Convert(_eventBuildingBlock);
      }

      [Observation]
      public void should_set_a_dummy_name_if_not_set_already()
      {
         string.IsNullOrEmpty(_moleculeBuilder.Name).ShouldBeFalse();
      }
   }

   public class When_converting_a_molecule_start_value_building_block : concern_for_Converter60To61
   {
      private IMoleculeStartValuesBuildingBlock _moleculeStartValueBuildingBlock;
      private IMoleculeStartValue _msv;

      protected override void Context()
      {
         base.Context();
         _msv = new MoleculeStartValue {NegativeValuesAllowed = false};
         _moleculeStartValueBuildingBlock = new MoleculeStartValuesBuildingBlock {_msv};
      }

      protected override void Because()
      {
         sut.Convert(_moleculeStartValueBuildingBlock);
      }

      [Observation]
      public void should_set_the_allow_negative_values_to_true()
      {
         _msv.NegativeValuesAllowed.ShouldBeTrue();
      }
   }

   public class When_converting_a_simulation : concern_for_Converter60To61
   {
      private IMoleculeAmount _moleculeAmount;
      private ModelCoreSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _moleculeAmount = new MoleculeAmount {NegativeValuesAllowed = false};
         var rootContainer = new Container {_moleculeAmount};
         _simulation = new ModelCoreSimulation {Model = new Model {Root = rootContainer}, BuildConfiguration = new BuildConfiguration()};
      }

      protected override void Because()
      {
         sut.Convert(_simulation);
      }

      [Observation]
      public void should_set_the_allow_negative_values_to_true_for_all_molecule_amounts_defined_in_the_simulation()
      {
         _moleculeAmount.NegativeValuesAllowed.ShouldBeTrue();
      }
   }
}