using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_SimulationResultsCreator : ContextSpecification<ISimulationResultsCreator>
   {
      protected DataRepository _dataRepository;
      protected SimulationResults _simulationResults;

      protected override void Context()
      {
         sut = new SimulationResultsCreator();
      }
   }

   public class When_creating_a_simulation_results_based_on_a_given_data_repository : concern_for_SimulationResultsCreator
   {
      private IndividualResults _individualResults;

      protected override void Context()
      {
         base.Context();
         _dataRepository = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("sim");
      }

      protected override void Because()
      {
         _simulationResults = sut.CreateResultsFrom(_dataRepository);
         _individualResults = _simulationResults.First();
      }

      [Observation]
      public void should_return_a_simulation_results_containing_only_one_individual_result()
      {
         _simulationResults.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_created_individual_results_should_contain_one_entry_for_each_available_column()
      {
         _individualResults.AllValues.Count.ShouldBeEqualTo(1);
         firstValue.Time.ShouldBeEqualTo(_individualResults.Time);
      }

      private QuantityValues firstValue => _individualResults.AllValues.First();

      [Observation]
      public void should_have_removed_the_first_entry_in_the_path_corresponding_to_the_simulation_name_of_the_returned_columns()
      {
         firstValue.QuantityPath.Contains("sim").ShouldBeFalse();
      }

      [Observation]
      public void should_use_the_expected_values()
      {
         firstValue.Values.ShouldOnlyContain(10f, 20f, 30f);
         firstValue.Time.Values.ShouldOnlyContain(1f, 2f, 3f);
      }
   }

   public class When_creating_a_simulation_results_based_on_an_empty_data_repository : concern_for_SimulationResultsCreator
   {
      protected override void Context()
      {
         base.Context();
         _dataRepository = new DataRepository();
      }

      protected override void Because()
      {
         _simulationResults = sut.CreateResultsFrom(_dataRepository);
      }

      [Observation]
      public void should_return_an_empty_simulation_results()
      {
         _simulationResults.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_creating_a_simulation_results_with_null_quantity_info : concern_for_SimulationResultsCreator
   {
      protected override void Context()
      {
         base.Context();
         _dataRepository = new DataRepository("Results");
         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs())
         {
            Values = new[] { 1.0f, 2.0f, 3.0f }
         };
         _dataRepository.Add(baseGrid);

         // Create a column with null QuantityInfo to simulate corrupted data
         var dataColumn = new DataColumn("Col", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid)
         {
            Values = new[] { 10f, 20f, 30f },
            DataInfo = { Origin = ColumnOrigins.Calculation },
            QuantityInfo = null
         };

         _dataRepository.Add(dataColumn);
      }

      protected override void Because()
      {
         _simulationResults = sut.CreateResultsFrom(_dataRepository);
      }

      [Observation]
      public void should_not_crash()
      {
         _simulationResults.ShouldNotBeNull();
      }

      [Observation]
      public void should_create_results_with_default_individual()
      {
         _simulationResults.Count.ShouldBeEqualTo(1);
         _simulationResults.First().IndividualId.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_create_quantity_values_with_empty_path()
      {
         var firstValue = _simulationResults.First().AllValues.First();
         firstValue.QuantityPath.ShouldBeEqualTo(string.Empty);
      }

      [Observation]
      public void should_preserve_values()
      {
         var firstValue = _simulationResults.First().AllValues.First();
         firstValue.Values.ShouldOnlyContain(10f, 20f, 30f);
      }
   }
}