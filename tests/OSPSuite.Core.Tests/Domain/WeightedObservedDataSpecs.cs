using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_WeightedObservedData : ContextSpecification<WeightedObservedData>
   {
      protected DataRepository _observedData;

      protected override void Context()
      {
         _observedData = DomainHelperForSpecs.ObservedData();
         sut = new WeightedObservedData(_observedData);
      }
   }

   public class When_creating_a_new_weighted_observed_data_based_on_some_observed_data : concern_for_WeightedObservedData
   {
      [Observation]
      public void should_initialize_the_weight_array_to_the_default_weight_value()
      {
         sut.Weights.Length.ShouldBeEqualTo(_observedData.BaseGrid.Count);
         sut.Weights.Distinct().ShouldOnlyContain(Constants.DEFAULT_WEIGHT);
      }
   }

   public class When_returning_the_display_name_of_a_weighted_observed_data : concern_for_WeightedObservedData
   {
      [Observation]
      public void should_return_the_name_if_the_observed_data_if_the_id_is_not_set()
      {
         sut.DisplayName.ShouldBeEqualTo(_observedData.Name);
      }

      [Observation]
      public void should_return_the_name_with_id_if_the_id_is_set()
      {
         sut.Id = 5;
         sut.DisplayName.ShouldBeEqualTo($"{_observedData.Name} - {sut.Id}");
      }
   }
}