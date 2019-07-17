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

   public class When_returning_all_the_values_defined_for_a_given_path : concern_for_SimulationResults
   {
      private QuantityValues _quantityValue1;
      private QuantityValues _quantityValue2;
      private QuantityValues _anotherQuantityValue;

      protected override void Context()
      {
         base.Context();
         _quantityValue1 = new QuantityValues {QuantityPath = "Path1"};
         _quantityValue2 = new QuantityValues {QuantityPath = "Path1"};
         _anotherQuantityValue = new QuantityValues {QuantityPath = "Path2"};

         var individualResults1 = new IndividualResults {IndividualId = 3};
         individualResults1.Add(_quantityValue1);
         individualResults1.Add(_anotherQuantityValue);

         var individualResults2 = new IndividualResults {IndividualId = 2};
         individualResults2.Add(_quantityValue2);

         sut.Add(individualResults1);
         sut.Add(individualResults2);

         //reorder results before accessing them
         sut.ReorderByIndividualId();
      }

      [Observation]
      public void should_return_all_quantity_values_for_the_path_defined_in_all_individuals_ordered_by_individual_id()
      {
         sut.AllValuesFor("Path1").ShouldOnlyContainInOrder(_quantityValue2, _quantityValue1);
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
      public void should_reutnr_an_empty_list_if_the_results_are_empty()
      {
         new SimulationResults().AllQuantityPaths().ShouldBeEmpty();
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