using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   internal class concern_for_SimulationBuilder : ContextSpecification<SimulationBuilder>
   {
      protected ModuleConfiguration CreateModuleConfiguration()
      {
         var module = new Module();
         var moduleConfiguration = new ModuleConfiguration(module);
         var observerBuildingBlock = new ObserverBuildingBlock();
         module.Add(observerBuildingBlock);
         observerBuildingBlock.Add(new AmountObserverBuilder().WithName("toto").WithDimension(A.Fake<IDimension>()));
         return moduleConfiguration;
      }
   }

   internal class When_mapping_an_observer_from_an_observer_builder_and_there_are_multiple_observer_builders_for_the_name : concern_for_SimulationBuilder
   {
      private SimulationConfiguration _simulationConfiguration;
      private ModuleConfiguration _moduleConfiguration1;
      private ModuleConfiguration _moduleConfiguration2;
      private ObserverBuilder _observerBuilder;
      private ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;
      private InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      private ICloneManagerForModel _cloneManagerForModel;
      private IContainerMergeTask _containerMergeTask;

      protected override void Context()
      {
         base.Context();
         _moduleConfiguration1 = CreateModuleConfiguration();
         _moduleConfiguration2 = CreateModuleConfiguration();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration1);
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration2);

         _moduleConfiguration1.Module.Observers.AmountObserverBuilders.Each(x => x.MoleculeList.AddMoleculeName("molecule1"));
         _moduleConfiguration2.Module.Observers.AmountObserverBuilders.Each(x => x.MoleculeList.AddMoleculeName("molecule2"));


         _expressionProfileBuildingBlock = new ExpressionProfileBuildingBlock
         {
            new InitialCondition {Value = 1.0}.WithName("name")
         };

         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock
         {
            new InitialCondition {Value = 2.0}.WithName("name")
         };
         _simulationConfiguration.AddExpressionProfile(_expressionProfileBuildingBlock);
         _moduleConfiguration2.Module.Add(_initialConditionsBuildingBlock);
         _moduleConfiguration2.SelectedInitialConditions = _initialConditionsBuildingBlock;

         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _containerMergeTask = A.Fake<IContainerMergeTask>();

         sut = new SimulationBuilder(_cloneManagerForModel, _containerMergeTask);
         _observerBuilder = _moduleConfiguration2.Module.Observers.AmountObserverBuilders.First();
      }

      protected override void Because()
      {
         sut.PerformMerge(_simulationConfiguration);
      }

      [Observation]
      public void the_initial_condition_should_take_priority_over_the_expression()
      {
         sut.InitialConditions.Single().Value.ShouldBeEqualTo(2.0);
      }

      [Observation]
      public void the_observer_should_be_created_for_both()
      {
         sut.MoleculeListFor(_observerBuilder).MoleculeNames.ShouldContain("molecule1");
         sut.MoleculeListFor(_observerBuilder).MoleculeNames.ShouldContain("molecule2");
      }
   }
}