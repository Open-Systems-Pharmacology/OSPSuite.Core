using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ModuleConfiguration : ContextSpecification<ModuleConfiguration>
   {
      protected Module _module;
      protected MoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock1;
      protected MoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock2;
      protected ParameterStartValuesBuildingBlock _parameterStartValuesBuildingBlock;

      protected override void Context()
      {
         _module = new Module()
         {
            new PassiveTransportBuildingBlock(),
            new SpatialStructure(),
            new ObserverBuildingBlock(),
            new ReactionBuildingBlock(),
            new MoleculeBuildingBlock()
         };

         _moleculeStartValuesBuildingBlock1 = new MoleculeStartValuesBuildingBlock();
         _moleculeStartValuesBuildingBlock2 = new MoleculeStartValuesBuildingBlock();
         _module.Add(_moleculeStartValuesBuildingBlock1);
         _module.Add(_moleculeStartValuesBuildingBlock2);
         _parameterStartValuesBuildingBlock = new ParameterStartValuesBuildingBlock();
         _module.Add(_parameterStartValuesBuildingBlock);

         sut = new ModuleConfiguration(_module);
      }
   }

   public class When_creating_new_module_configuration_from_a_module : concern_for_ModuleConfiguration
   {
      [Observation]
      public void should_return_the_fist_molecule_start_value_and_parameter_start_value_if_one_is_defined()
      {
         sut.SelectedMoleculeStartValues.ShouldBeEqualTo(_moleculeStartValuesBuildingBlock1);
         sut.SelectedParameterStartValues.ShouldBeEqualTo(_parameterStartValuesBuildingBlock);
      }
   }

   public class When_creating_new_module_configuration_from_a_module_with_predefined_start_values : concern_for_ModuleConfiguration
   {
      protected override void Context()
      {
         base.Context();
         sut = new ModuleConfiguration(_module, _moleculeStartValuesBuildingBlock2, _parameterStartValuesBuildingBlock);
      }

      [Observation]
      public void should_return_the_expected_molecule_and_parameter_start_values()
      {
         sut.SelectedMoleculeStartValues.ShouldBeEqualTo(_moleculeStartValuesBuildingBlock2);
         sut.SelectedParameterStartValues.ShouldBeEqualTo(_parameterStartValuesBuildingBlock);
      }
   }

   public class When_returning_the_building_block_by_type_for_a_module_configuration : concern_for_ModuleConfiguration
   {
      protected override void Context()
      {
         base.Context();
         sut = new ModuleConfiguration(_module, _moleculeStartValuesBuildingBlock2, _parameterStartValuesBuildingBlock);
      }

      [Observation]
      public void should_return_the_expected_objects()
      {
         sut.BuildingBlock<PassiveTransportBuildingBlock>().ShouldBeEqualTo(_module.PassiveTransports);
         sut.BuildingBlock<SpatialStructure>().ShouldBeEqualTo(_module.SpatialStructure);
         sut.BuildingBlock<ObserverBuildingBlock>().ShouldBeEqualTo(_module.Observers);
         sut.BuildingBlock<EventGroupBuildingBlock>().ShouldBeNull();
         sut.BuildingBlock<ReactionBuildingBlock>().ShouldBeEqualTo(_module.Reactions);
         sut.BuildingBlock<MoleculeBuildingBlock>().ShouldBeEqualTo(_module.Molecules);
      }
   }
}