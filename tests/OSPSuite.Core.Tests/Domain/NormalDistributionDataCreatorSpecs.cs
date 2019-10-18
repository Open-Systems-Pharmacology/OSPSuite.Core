using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Maths.Statistics;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_NormalDistributionDataCreator : ContextSpecification<INormalDistributionDataCreator>
   {
      protected override void Context()
      {
         sut = new NormalDistributionDataCreator();
      }
   }

   public class When_creating_normal_distributed_data_between_a_given_min_and_max : concern_for_NormalDistributionDataCreator
   {
      private DataTable _data;

      protected override void Because()
      {
         _data = sut.CreateNormalData(0,1, -1d, 1d, 10);
      }

      [Observation]
      public void should_return_the_expected_count_of_normal_distributed_data()
      {
         _data.Rows.Count.ShouldBeEqualTo(10);
      }


      [Observation]
      public void the_return_values_should_be_the_value_of_the_probability_density_function()
      {
         var normalDistribution = new NormalDistribution(0,1);
         foreach (DataRow row in _data.Rows)
         {
            var xValue = row.ValueAt<double>(Constants.X);
            xValue.ShouldBeGreaterThanOrEqualTo(-1);
            xValue.ShouldBeSmallerThanOrEqualTo(1);
            var yValue = row.ValueAt<double>(Constants.Y);
            yValue.ShouldBeEqualTo(normalDistribution.ProbabilityDensityFor(xValue));
         }
      }
   }
}	