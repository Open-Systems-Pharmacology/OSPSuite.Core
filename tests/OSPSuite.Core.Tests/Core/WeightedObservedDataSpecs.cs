using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;

namespace OSPSuite.Core
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
}	