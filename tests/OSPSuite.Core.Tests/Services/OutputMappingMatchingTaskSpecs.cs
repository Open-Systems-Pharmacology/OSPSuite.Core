using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_OutputMappingMatchingTask : ContextSpecification<OutputMappingMatchingTask>
   {
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      protected IEntityPathResolver _entityPathResolver;
      protected PathCache<IQuantity> _pathCache;
      protected IQuantity _quantity1;
      protected IQuantity _quantity2;
      protected IQuantity _compartment;
      protected DataRepository _observedData;
      protected ISimulation _simulation;
      protected IContainer _container;
      protected IContainer _organContainer;
      private IEventPublisher _eventPublisher;

      protected override void Context()
      {
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _observedData = DomainHelperForSpecs.ObservedData();
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.ObservedData.ORGAN, Value = "Brain" });
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.ObservedData.COMPARTMENT, Value = "TestCompartment" });
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.ObservedData.MOLECULE, Value = "TestMolecule" });
         _simulation = A.Fake<ISimulation>();

         _organContainer = new Container().WithName("Brain");
         _organContainer.Add(new MoleculeAmount() { Name = "TestMolecule" });

         _container = new Container().WithName("TestCompartment");
         _container.Add(_organContainer);

         _quantity1 = new MoleculeAmount();
         _quantity1.QuantityType = QuantityType.Drug;

         _quantity2 = new MoleculeAmount();
         _quantity2.QuantityType = QuantityType.Drug;
         
         _simulation.Model.Root = _container;

         _pathCache = new PathCache<IQuantity>(_entityPathResolver)
         {
            { "TestCompartment|Brain|TestMolecule", _quantity1 },
            { "test1|Brain|TestMolecule", _quantity2 }
         };
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         A.CallTo(() => _entitiesInSimulationRetriever.OutputsFrom(A<ISimulation>._)).Returns(_pathCache);

         sut = new OutputMappingMatchingTask(_entitiesInSimulationRetriever, _eventPublisher);
      }
   }

   public class When_mapping_data_that_has_no_matching_output : concern_for_OutputMappingMatchingTask
   {
      [Observation]
      [TestCase("test1|test2|test3", false)]
      [TestCase("TestCompartment|Brain|TestMolecule", true)]
      [TestCase("test1|Brain|TestMolecule", false)]
      public void should_correctly_match(string value, bool isMatch)
      {
         sut.ObservedDataMatchesOutput(_observedData, value).ShouldBeEqualTo(isMatch);
      }
   }

   
   public class When_adding_matching_observed_data_to_a_simulation : concern_for_OutputMappingMatchingTask
   {
      protected override void Context()
      {
         base.Context();
         sut.AddMatchingOutputMapping(_observedData, _simulation);
      }

      [Observation]
      public void matching_simulation_output_mapping_should_have_been_added()
      {
         _simulation.OutputMappings.All.Count.ShouldBeEqualTo(1);
         _simulation.OutputMappings.All.First().Output.Name.ShouldBeEqualTo("TestMolecule");
         _simulation.OutputMappings.All.First().WeightedObservedData.Name.ShouldBeEqualTo("TestData");
      }
   }

   public class When_adding_non_matching_observed_data_to_a_simulation : concern_for_OutputMappingMatchingTask
   {
      protected override void Context()
      {
         base.Context();
         _observedData.ExtendedProperties.Remove(Constants.ObservedData.MOLECULE);
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.ObservedData.MOLECULE, Value = "TestMolecule_2" });
         sut.AddMatchingOutputMapping(_observedData, _simulation);
      }

      [Observation]
      public void no_simulation_output_mapping_should_have_been_added()
      {
         _simulation.OutputMappings.All.ShouldBeEmpty();
      }
   }

}