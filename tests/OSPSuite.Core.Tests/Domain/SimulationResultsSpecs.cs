using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SimulationResults : ContextSpecification<SimulationResults>
   {
      protected override void Context()
      {
         sut = new SimulationResults();
      }
   }

   public class When_querying_values_defined_for_a_simulation_results : concern_for_SimulationResults
   {
      private QuantityValues _quantityValue1;
      private QuantityValues _quantityValue2;
      private QuantityValues _anotherQuantityValue;
      private QuantityValues _timeValues;

      protected override void Context()
      {
         base.Context();
         _quantityValue1 = new QuantityValues {QuantityPath = "Path1",Values = new []{1f,2f,3f}};
         _quantityValue2 = new QuantityValues {QuantityPath = "Path1", Values = new[] { 4f, 5f, 6f } };
         _anotherQuantityValue = new QuantityValues {QuantityPath = "Path2", Values = new[] { 7f, 8f, 9f } };
         _timeValues = new QuantityValues {QuantityPath = "Time", Values = new[] { 10f, 20f, 30f } };

         var individualResults1 = new IndividualResults {IndividualId = 3};
         individualResults1.Add(_quantityValue1);
         individualResults1.Add(_anotherQuantityValue);

         var individualResults2 = new IndividualResults {IndividualId = 2};
         individualResults2.Add(_quantityValue2);

         sut.Add(individualResults1);
         sut.Add(individualResults2);
         sut.Time = _timeValues;

         //reorder results before accessing them
         sut.ReorderByIndividualId();
      }

      [Observation]
      public void should_return_all_quantity_values_for_the_path_defined_in_all_individuals_ordered_by_individual_id()
      {
         sut.AllQuantityValuesFor("Path1").ShouldOnlyContainInOrder(_quantityValue2, _quantityValue1);
      }

      [Observation]
      public void should_return_all_values_for_the_path_defined_if_not_id_is_specified()
      {
         sut.AllValuesFor("Path1", null).ShouldOnlyContainInOrder(new []{ 4f, 5f, 6f, 1f, 2f, 3f, });
         sut.AllValuesFor("Path1").ShouldOnlyContainInOrder(new []{ 4f, 5f, 6f, 1f, 2f, 3f, });
      }

      [Observation]
      public void should_return_all_values_for_the_specified_id_and_path()
      {
         sut.AllValuesFor("Path1", 2).ShouldOnlyContainInOrder(new[] { 4f, 5f, 6f});
         sut.AllValuesFor("Path1", 3).ShouldOnlyContainInOrder(new[] { 1f, 2f, 3f});
         sut.AllValuesFor("Path1", 3, 4).ShouldOnlyContainInOrder(new[] { 1f, 2f, 3f, float.NaN, float.NaN, float.NaN});
         sut.AllValuesFor("unknown", 3, 4).ShouldOnlyContainInOrder(new[] { float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN});
      }
   }

   public class When_return_the_list_of_all_quantity_path_defined_in_the_results : concern_for_SimulationResults
   {
      private QuantityValues _quantityValue1;
      private QuantityValues _quantityValue2;

      protected override void Context()
      {
         base.Context();
         _quantityValue1 = new QuantityValues {QuantityPath = "Path1"};
         _quantityValue2 = new QuantityValues {QuantityPath = "Path2"};

         var individualResults1 = new IndividualResults {IndividualId = 1};
         individualResults1.Add(_quantityValue1);
         individualResults1.Add(_quantityValue2);

         sut.Add(individualResults1);
      }

      [Observation]
      public void should_return_the_expected_paths()
      {
         sut.AllQuantityPaths().ShouldOnlyContain("Path1", "Path2");
      }

      [Observation]
      public void should_return_an_empty_list_if_the_results_are_empty()
      {
         new SimulationResults().AllQuantityPaths().ShouldBeEmpty();
      }
   }

   public class When_return_the_list_of_all_individual_ids_defined_in_the_results : concern_for_SimulationResults
   {
      protected override void Context()
      {
         base.Context();

         sut.Add(new IndividualResults { IndividualId = 1 });
         sut.Add(new IndividualResults { IndividualId = 3 });
      }

      [Observation]
      public void should_return_the_expected_paths()
      {
         sut.AllIndividualIds().ShouldOnlyContain(1, 3);
      }

      [Observation]
      public void should_return_an_empty_list_if_the_results_are_empty()
      {
         new SimulationResults().AllIndividualIds().ShouldBeEmpty();
      }
   }

   public class When_return_the_max_individual_id_defined_in_the_results : concern_for_SimulationResults
   {
      protected override void Context()
      {
         base.Context();

         sut.Add(new IndividualResults { IndividualId = 1 });
         sut.Add(new IndividualResults { IndividualId = 3 });
      }

      [Observation]
      public void should_return_the_expected_value_if_the_number_of_individual_plus_one_is_less_than_the_max_individual_id()
      {
         sut.NumberOfIndividuals.ShouldBeEqualTo(4);
      }
      
      [Observation]
      public void should_return_the_expected_value()
      {
         sut.Add(new IndividualResults { IndividualId = 0 });
         sut.Add(new IndividualResults { IndividualId = 2 });
         sut.Add(new IndividualResults { IndividualId = 4 });
         sut.NumberOfIndividuals.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_return_an_empty_list_if_the_results_are_empty()
      {
         new SimulationResults().NumberOfIndividuals.ShouldBeEqualTo(0);
      }
   }

   public class When_adding_a_range_of_results : concern_for_SimulationResults
   {
      protected override void Context()
      {
         base.Context();
         var quantityValue1 = new QuantityValues {QuantityPath = "Path1"};
         var quantityValue2 = new QuantityValues {QuantityPath = "Path1"};

         var individualResults1 = new IndividualResults {IndividualId = 1};
         individualResults1.Add(quantityValue1);

         var individualResults2 = new IndividualResults {IndividualId = 2};
         individualResults2.Add(quantityValue2);

         sut.Time = new QuantityValues {QuantityPath = "Time"};
         individualResults1.Time = sut.Time;
         individualResults2.Time = sut.Time;
         sut.AddRange(new[] {individualResults1, individualResults2});
      }

      [Observation]
      public void should_be_able_to_retrieve_the_results()
      {
         sut.Count.ShouldBeEqualTo(2);
      }
   }
}