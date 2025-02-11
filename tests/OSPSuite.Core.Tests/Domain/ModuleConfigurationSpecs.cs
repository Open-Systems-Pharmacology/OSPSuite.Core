using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ModuleConfiguration : ContextSpecification<ModuleConfiguration>
   {
      protected Module _module;
      protected InitialConditionsBuildingBlock _initialConditionsBuildingBlock1;
      protected InitialConditionsBuildingBlock _initialConditionsBuildingBlock2;
      protected ParameterValuesBuildingBlock _parameterValuesBuildingBlock;

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

         _initialConditionsBuildingBlock1 = new InitialConditionsBuildingBlock();
         _initialConditionsBuildingBlock2 = new InitialConditionsBuildingBlock();
         _module.Add(_initialConditionsBuildingBlock1);
         _module.Add(_initialConditionsBuildingBlock2);
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock();
         _module.Add(_parameterValuesBuildingBlock);

         _module.MergeBehavior = MergeBehavior.Overwrite;

         sut = new ModuleConfiguration(_module);
      }
   }

   public class When_creating_new_module_configuration_from_a_module : concern_for_ModuleConfiguration
   {
      [Observation]
      public void should_have_set_the_merge_behavior_based_on_the_module()
      {
         sut.MergeBehavior.ShouldBeEqualTo(MergeBehavior.Overwrite);
      }

      [Observation]
      public void should_return_the_fist_initial_condition_and_parameter_value_if_one_is_defined()
      {
         sut.SelectedInitialConditions.ShouldBeEqualTo(_initialConditionsBuildingBlock1);
         sut.SelectedParameterValues.ShouldBeEqualTo(_parameterValuesBuildingBlock);
      }
   }

   public class When_creating_new_module_configuration_from_a_module_with_predefined_start_values : concern_for_ModuleConfiguration
   {
      protected override void Context()
      {
         base.Context();
         sut = new ModuleConfiguration(_module, _initialConditionsBuildingBlock2, _parameterValuesBuildingBlock);
      }

      [Observation]
      public void should_return_the_expected_molecule_and_parameter_values()
      {
         sut.SelectedInitialConditions.ShouldBeEqualTo(_initialConditionsBuildingBlock2);
         sut.SelectedParameterValues.ShouldBeEqualTo(_parameterValuesBuildingBlock);
      }
   }

   public class When_cloning_a_module_configuration : concern_for_ModuleConfiguration
   {
      private ICloneManager _cloneManager;
      private IDataRepositoryTask _dataRepositoryTask;
      private ModuleConfiguration _result;

      protected override void Context()
      {
         base.Context();
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _cloneManager = new CloneManagerForBuildingBlock(new ObjectBaseFactoryForSpecs(new DimensionFactoryForIntegrationTests()), _dataRepositoryTask);

         sut.SelectedInitialConditions = _initialConditionsBuildingBlock2;
         sut.SelectedParameterValues = null;
         sut.Module.MergeBehavior = MergeBehavior.Extend;
         sut.Module.ModuleImportVersion = "1.2.3";
      }

      protected override void Because()
      {
         _result = _cloneManager.Clone(sut);
      }

      [Observation]
      public void the_cloned_building_block_should_have_the_properly_selected_start_values()
      {
         _result.SelectedParameterValues.ShouldBeNull();
         _result.SelectedInitialConditions.ShouldNotBeEqualTo(sut.SelectedInitialConditions);
         _result.SelectedInitialConditions.Name.ShouldBeEqualTo(sut.SelectedInitialConditions.Name);
      }

      [Observation]
      public void the_module_should_be_a_clone_of_the_original()
      {
         _result.Module.ShouldNotBeEqualTo(sut.Module);
         _result.Module.Name.ShouldBeEqualTo(sut.Module.Name);
         _result.Module.MergeBehavior.ShouldBeEqualTo(sut.Module.MergeBehavior);
         _result.Module.ModuleImportVersion.ShouldBeEqualTo(sut.Module.ModuleImportVersion);
         _result.MergeBehavior.ShouldBeEqualTo(sut.MergeBehavior);
      }
   }

   public class When_returning_the_building_block_by_type_for_a_module_configuration : concern_for_ModuleConfiguration
   {
      protected override void Context()
      {
         base.Context();
         sut = new ModuleConfiguration(_module, _initialConditionsBuildingBlock2, _parameterValuesBuildingBlock);
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