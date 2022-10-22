using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_OutputMappingMatchingService : ContextSpecification<OutputMappingMatchingService>
   {
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      protected IEntityPathResolver _entityPathResolver;
      protected PathCache<IQuantity> _pathCache;
      protected IQuantity _quantity1;
      protected IQuantity _quantity2;
      protected DataRepository _observedData;

      protected override void Context()
      {
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         
         _observedData = DomainHelperForSpecs.ObservedData();
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.ObservedData.ORGAN, Value = "Brain" });
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.ObservedData.COMPARTMENT, Value = "TestCompartment" });
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.ObservedData.MOLECULE, Value = "TestMolecule" });

         _quantity1 = A.Fake<IQuantity>();
         _quantity2 = A.Fake<IQuantity>();
         _pathCache = new PathCache<IQuantity>(_entityPathResolver);
         _pathCache.Add("Brain", _quantity1);
         _pathCache.Add("Test", _quantity2);
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         A.CallTo(() => _entitiesInSimulationRetriever.OutputsFrom(A<ISimulation>._)).Returns(_pathCache);

         sut = new OutputMappingMatchingService(_entitiesInSimulationRetriever);
      }
   }

   public class When_mapping_data_that_has_no_matching_output : concern_for_OutputMappingMatchingService
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
}