using System;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Helpers.ConstantsForSpecs;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimulationEntitySourceReferenceFactory : ContextForIntegration<ISimulationEntitySourceReferenceFactory>
   {
      private IBuildingBlockRepository _buildingBlockRepository;
      protected IModel _model;
      protected SimulationBuilder _simulationBuilder;

      protected virtual Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehavior();

      protected virtual SimulationConfiguration CreateConfiguration()
      {
         var moduleHelper = IoC.Resolve<ModuleHelperForSpecs>();
         return SimulationConfigurationBuilder()(moduleHelper);
      }

      public override void GlobalContext()
      {
         base.GlobalContext();
         var modelConstructor = IoC.Resolve<IModelConstructor>();
         var simulationConfiguration = CreateConfiguration();
         var result = modelConstructor.CreateModelFrom(simulationConfiguration, "model");
         _model = result.Model;
         _simulationBuilder = result.SimulationBuilder;


         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         A.CallTo(() => _buildingBlockRepository.All()).Returns(simulationConfiguration.All<IBuildingBlock>());
         A.CallTo(() => _buildingBlockRepository.All<SpatialStructure>()).Returns(simulationConfiguration.All<SpatialStructure>());

         sut = new SimulationEntitySourceReferenceFactory(_buildingBlockRepository, IoC.Resolve<IContainerTask>());
      }

      protected SimulationEntitySourceReference SourceReferenceFor(SimulationEntitySource source) =>
         sut.CreateFor(new[] {source}).FirstOrDefault();

      protected SimulationEntitySourceReference SourceReferenceFor(IEntity entity)
      {
         var entitySource = _simulationBuilder.SimulationEntitySourceFor(entity);
         return SourceReferenceFor(entitySource);
      }

      protected SimulationEntitySourceReference ValidateSourceReferenceFor(IEntity entity, bool noModule = false)
      {
         var sourceReference = SourceReferenceFor(entity);
         ReferenceShouldBeDefined(sourceReference, noModule);
         return sourceReference;
      }

      protected void ReferenceShouldBeDefined(SimulationEntitySourceReference reference, bool noModule = false)
      {
         reference.ShouldNotBeNull();
         reference.Source.ShouldNotBeNull();
         reference.BuildingBlock.ShouldNotBeNull();
         if (noModule) return;
         reference.Module.ShouldNotBeNull();
      }
   }

   public class When_creating_a_simulation_source_reference_from_a_simulation_source_in_overwrite_mode : concern_for_SimulationEntitySourceReferenceFactory
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfiguration();

      [Observation]
      public void should_be_able_to_find_the_source_for_a_parameter_in_global_molecule_container()
      {
         var moleculeAGlobalContainer = _model.Root.Container("A");
         var parameter1 = moleculeAGlobalContainer.Parameter("P1");
         var parameter2 = moleculeAGlobalContainer.Parameter("P2");

         var parameter1SourceReference = ValidateSourceReferenceFor(parameter1);
         parameter1SourceReference.Module.Name.ShouldBeEqualTo("Module2");

         var parameter2SourceReference = ValidateSourceReferenceFor(parameter2);
         parameter2SourceReference.Module.Name.ShouldBeEqualTo("Module1");
      }

      [Observation]
      public void should_be_able_to_find_the_source_for_a_container_that_was_replaced()
      {
         var lungContainer = _model.Root.EntityAt<IContainer>(ORGANISM, Lung);
         var lungReference = ValidateSourceReferenceFor(lungContainer);
         lungReference.Module.Name.ShouldBeEqualTo("Module2");
      }
   }

   public class When_creating_a_simulation_source_reference_from_a_simulation_source_in_extend_mode : concern_for_SimulationEntitySourceReferenceFactory
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehavior();

      [Observation]
      public void should_be_able_to_find_the_source_for_a_parameter_in_global_molecule_container()
      {
         var moleculeAGlobalContainer = _model.Root.Container("A");
         var parameter1 = moleculeAGlobalContainer.Parameter("P1");
         var parameter2 = moleculeAGlobalContainer.Parameter("P2");

         var parameter1SourceReference = ValidateSourceReferenceFor(parameter1);
         parameter1SourceReference.Module.Name.ShouldBeEqualTo("Module2");

         var parameter2SourceReference = ValidateSourceReferenceFor(parameter2);
         parameter2SourceReference.Module.Name.ShouldBeEqualTo("Module1");
      }

      [Observation]
      public void should_be_able_to_find_the_source_for_a_container_that_was_replaced()
      {
         var lungContainer = _model.Root.EntityAt<IContainer>(ORGANISM, Lung);
         var lungReference = ValidateSourceReferenceFor(lungContainer);
         lungReference.Module.Name.ShouldBeEqualTo("Module1");

         var lungParameterP = lungContainer.Parameter(P);
         var lungParameterPReference = ValidateSourceReferenceFor(lungParameterP);
         lungParameterPReference.Module.Name.ShouldBeEqualTo("Module1");

         var lungParameterQ = lungContainer.Parameter(Q);
         var lungParameterQReference = ValidateSourceReferenceFor(lungParameterQ);
         lungParameterQReference.Module.Name.ShouldBeEqualTo("Module2");
      }
   }

   public class When_creating_a_simulation_source_reference_from_a_simulation_source_with_parameter_overwritten_from_parameter_values : concern_for_SimulationEntitySourceReferenceFactory
   {
      protected override SimulationConfiguration CreateConfiguration()
      {
         return IoC.Resolve<ModelHelperForSpecs>().CreateSimulationConfiguration();
      }

      [Observation]
      public void should_be_able_to_retrieve_source_coming_from_parameter_value()
      {
         var bone_cell = _model.ModelOrganCompartment(Bone, Cell);
         var parameter = bone_cell.Parameter("FormulaParameterOverwritten");

         var parameter1SourceReference = ValidateSourceReferenceFor(parameter);
         parameter1SourceReference.Module.Name.ShouldBeEqualTo("Module");
      }
   }
}