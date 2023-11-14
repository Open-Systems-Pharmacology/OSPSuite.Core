using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   internal class concern_for_SimulationBuilder : ContextSpecification<SimulationBuilder>
   {
   }

   internal class When_mapping_an_observer_from_an_observer_builder_and_there_are_multiple_observer_builders_for_the_name : concern_for_SimulationBuilder
   {
      private SimulationConfiguration _simulationConfiguration;
      private ModuleConfiguration _moduleConfiguration1;
      private ModuleConfiguration _moduleConfiguration2;
      private ObserverBuilder _observerBuilder;

      protected override void Context()
      {
         base.Context();
         _moduleConfiguration1 = createModuleConfiguration();
         _moduleConfiguration2 = createModuleConfiguration();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration1);
         _simulationConfiguration.AddModuleConfiguration(_moduleConfiguration2);

         _moduleConfiguration1.Module.Observers.AmountObserverBuilders.Each(x => x.MoleculeList.AddMoleculeName("molecule1"));
         _moduleConfiguration2.Module.Observers.AmountObserverBuilders.Each(x => x.MoleculeList.AddMoleculeName("molecule2"));

         sut = new SimulationBuilder(_simulationConfiguration);
         _observerBuilder = _moduleConfiguration2.Module.Observers.AmountObserverBuilders.First();
      }

      private static ModuleConfiguration createModuleConfiguration()
      {
         var module = new Module();
         var moduleConfiguration = new ModuleConfiguration(module);
         var observerBuildingBlock = new ObserverBuildingBlock();
         module.Add(observerBuildingBlock);
         observerBuildingBlock.Add(new AmountObserverBuilder().WithName("toto").WithDimension(A.Fake<IDimension>()));
         return moduleConfiguration;
      }

      [Observation]
      public void the_observer_should_be_created_for_both_()
      {
         sut.MoleculeListFor(_observerBuilder).MoleculeNames.ShouldContain("molecule1");
         sut.MoleculeListFor(_observerBuilder).MoleculeNames.ShouldContain("molecule2");
      }
   }
}