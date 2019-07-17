using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_VariationData : ContextSpecification<VariationData>
   {
      protected override void Context()
      {
         sut = new VariationData();
      }
   }

   public class When_adding_variation_to_a_variation_data : concern_for_VariationData
   {
      private IReadOnlyList<ParameterVariation> _result;

      protected override void Context()
      {
         base.Context();
         sut.AddVariationValues("P0", new List<IReadOnlyList<double>> {new List<double> {10d, 20d}});
         sut.AddVariationValues("P1", new List<IReadOnlyList<double>> {new List<double> {1d, 2d}, new List<double> {3d, 4}});
      }

      protected override void Because()
      {
         _result = sut.VariationsFor("P1");
      }

      [Observation]
      public void should_return_the_expected_variation_for_a_given_parameter()
      {
         _result.Count.ShouldBeEqualTo(2);
         _result[0].ParameterName.ShouldBeEqualTo("P1");
         _result[0].VariationId.ShouldBeEqualTo(1);
         _result[0].Variation.ShouldOnlyContainInOrder(1d, 2d);

         _result[1].ParameterName.ShouldBeEqualTo("P1");
         _result[1].VariationId.ShouldBeEqualTo(2);
         _result[1].Variation.ShouldOnlyContainInOrder(3d, 4d);
      }

      [Observation]
      public void should_return_the_expected_number_of_variations()
      {
         sut.NumberOfVariations.ShouldBeEqualTo(4);
      }

      [Observation]
      public void the_default_variation_id_should_be_set_to_the_number_of_parmaeter_variations()
      {
         sut.DefaultVariationId.ShouldBeEqualTo(3);
      }
   }
}