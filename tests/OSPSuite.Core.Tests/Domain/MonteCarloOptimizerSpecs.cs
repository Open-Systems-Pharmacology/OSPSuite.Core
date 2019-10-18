using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_MonteCarloOptimizerSpecs : ContextSpecification<MonteCarloOptimizer>
   {
      protected override void Context()
      {
         sut = new MonteCarloOptimizer();
      }
   }

   class When_trnsforming_values_to_the_distribution : concern_for_MonteCarloOptimizerSpecs
   {
      private IList<double> _values;
      private IList<double> _results;
      private double _a = 0.8;
      private double _alpha=30;

      protected override void Context()
      {
         base.Context();
         _values = new List<double>();
         for (int i = 0; i < 11; i++)
         {
            _values.Add(i/10.0);
         }
         
      }

      protected override void Because()
      {
         _results = _values.Select(v => sut.TransformNumberIntoAlphaDistribution(v, _a, _alpha)).ToList();
      }

      [Observation]
      public void should_compute_the_correct_values()
      {
         _results[0].ShouldBeEqualTo(0,1e-8);
         _results[1].ShouldBeEqualTo(0.748097239052678, 1e-8);
         _results[2].ShouldBeEqualTo(0.765012791597004, 1e-8);
         _results[3].ShouldBeEqualTo(0.775084514476363, 1e-8);
         _results[4].ShouldBeEqualTo(0.782310829068346, 1e-8);
         _results[5].ShouldBeEqualTo(0.787962358466107, 1e-8);
         _results[6].ShouldBeEqualTo(0.792610288060986, 1e-8);
         _results[7].ShouldBeEqualTo(0.796561439632065, 1e-8);
         _results[8].ShouldBeEqualTo(0.800000000000000, 1e-8);
         _results[9].ShouldBeEqualTo(0.804422292732913, 1e-8);
         _results[10].ShouldBeEqualTo(1, 1e-8);
      }
   }
}	