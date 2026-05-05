using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
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
         _moduleConfiguration2.Module.MergeBehavior = MergeBehavior.Extend;
         _moduleConfiguration2.SelectedInitialConditions = _initialConditionsBuildingBlock;

         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _containerMergeTask = A.Fake<IContainerMergeTask>();

         var obs1 = _moduleConfiguration1.Module.Observers.AmountObserverBuilders.ElementAt(0);
         var obs2 = _moduleConfiguration2.Module.Observers.AmountObserverBuilders.ElementAt(0);

         //only testing the molecule extend behavior. No need to really clone
         A.CallTo(() => _cloneManagerForModel.CloneAndKeepId<ObserverBuilder>(obs1)).Returns(obs1);
         A.CallTo(() => _cloneManagerForModel.CloneAndKeepId<ObserverBuilder>(obs2)).Returns(obs2);
     
         sut = new SimulationBuilder(_cloneManagerForModel, _containerMergeTask);
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
         var observedBuilder = sut.Observers.ElementAt(0);
         sut.MoleculeListFor(observedBuilder).MoleculeNames.ShouldContain("molecule1");
         sut.MoleculeListFor(observedBuilder).MoleculeNames.ShouldContain("molecule2");
      }
   }

   internal class When_extending_an_observer_builder_with_a_different_formula : concern_for_SimulationBuilder
   {
      private SimulationConfiguration _simulationConfiguration;
      private ModuleConfiguration _moduleConfiguration1;
      private ModuleConfiguration _moduleConfiguration2;
      private ICloneManagerForModel _cloneManagerForModel;
      private IContainerMergeTask _containerMergeTask;
      private IFormula _formulaFromModule1;
      private IFormula _formulaFromModule2;

      protected override void Context()
      {
         base.Context();
         _moduleConfiguration1 = CreateModuleConfiguration();
         _moduleConfiguration2 = CreateModuleConfiguration();

         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration1);
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration2);

         _moduleConfiguration2.Module.MergeBehavior = MergeBehavior.Extend;

         var observer1 = _moduleConfiguration1.Module.Observers.AmountObserverBuilders.ElementAt(0);
         var observer2 = _moduleConfiguration2.Module.Observers.AmountObserverBuilders.ElementAt(0);

         _formulaFromModule1 = A.Fake<IFormula>();
         _formulaFromModule2 = A.Fake<IFormula>();
         observer1.Formula = _formulaFromModule1;
         observer2.Formula = _formulaFromModule2;

         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _containerMergeTask = A.Fake<IContainerMergeTask>();

         //only testing the formula extend behavior. No need to really clone
         A.CallTo(() => _cloneManagerForModel.CloneAndKeepId<ObserverBuilder>(observer1)).Returns(observer1);
         A.CallTo(() => _cloneManagerForModel.CloneAndKeepId<ObserverBuilder>(observer2)).Returns(observer2);

         sut = new SimulationBuilder(_cloneManagerForModel, _containerMergeTask);
      }

      protected override void Because()
      {
         sut.PerformMerge(_simulationConfiguration);
      }

      [Observation]
      public void the_formula_should_be_taken_from_the_extending_module()
      {
         var observerBuilder = sut.Observers.ElementAt(0);
         observerBuilder.Formula.ShouldBeEqualTo(_formulaFromModule2);
      }
   }

   internal class When_extending_an_observer_builder_across_three_modules_with_different_formulas : concern_for_SimulationBuilder
   {
      private SimulationConfiguration _simulationConfiguration;
      private ModuleConfiguration _moduleConfiguration1;
      private ModuleConfiguration _moduleConfiguration2;
      private ModuleConfiguration _moduleConfiguration3;
      private ICloneManagerForModel _cloneManagerForModel;
      private IContainerMergeTask _containerMergeTask;
      private IFormula _formulaFromModule3;

      protected override void Context()
      {
         base.Context();
         _moduleConfiguration1 = CreateModuleConfiguration();
         _moduleConfiguration2 = CreateModuleConfiguration();
         _moduleConfiguration3 = CreateModuleConfiguration();

         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration1);
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration2);
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration3);

         _moduleConfiguration2.Module.MergeBehavior = MergeBehavior.Extend;
         _moduleConfiguration3.Module.MergeBehavior = MergeBehavior.Extend;

         var observer1 = _moduleConfiguration1.Module.Observers.AmountObserverBuilders.ElementAt(0);
         var observer2 = _moduleConfiguration2.Module.Observers.AmountObserverBuilders.ElementAt(0);
         var observer3 = _moduleConfiguration3.Module.Observers.AmountObserverBuilders.ElementAt(0);

         observer1.Formula = A.Fake<IFormula>();
         observer2.Formula = A.Fake<IFormula>();
         _formulaFromModule3 = A.Fake<IFormula>();
         observer3.Formula = _formulaFromModule3;

         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _containerMergeTask = A.Fake<IContainerMergeTask>();

         A.CallTo(() => _cloneManagerForModel.CloneAndKeepId<ObserverBuilder>(observer1)).Returns(observer1);
         A.CallTo(() => _cloneManagerForModel.CloneAndKeepId<ObserverBuilder>(observer2)).Returns(observer2);
         A.CallTo(() => _cloneManagerForModel.CloneAndKeepId<ObserverBuilder>(observer3)).Returns(observer3);

         sut = new SimulationBuilder(_cloneManagerForModel, _containerMergeTask);
      }

      protected override void Because()
      {
         sut.PerformMerge(_simulationConfiguration);
      }

      [Observation]
      public void the_formula_should_be_taken_from_the_last_extending_module()
      {
         var observerBuilder = sut.Observers.ElementAt(0);
         observerBuilder.Formula.ShouldBeEqualTo(_formulaFromModule3);
      }
   }
}