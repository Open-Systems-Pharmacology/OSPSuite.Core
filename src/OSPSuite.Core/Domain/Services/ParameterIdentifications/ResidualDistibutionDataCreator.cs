using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IResidualDistibutionDataCreator
   {
      ContinuousDistributionData CreateFor(OptimizationRunResult optimizationRunResult);
   }

   public class ResidualDistibutionDataCreator : IResidualDistibutionDataCreator
   {
      private readonly IBinIntervalsCreator _binIntervalsCreator;

      public ResidualDistibutionDataCreator(IBinIntervalsCreator binIntervalsCreator)
      {
         _binIntervalsCreator = binIntervalsCreator;
      }

      public ContinuousDistributionData CreateFor(OptimizationRunResult optimizationRunResult)
      {
         var allResidualValues = optimizationRunResult.AllResidualValues;

         var allValidValues = allResidualValues.Where(x => !double.IsNaN(x)).ToList();
         var allIntervals = _binIntervalsCreator.CreateUniformIntervalsFor(allValidValues);

         var data = new ContinuousDistributionData(AxisCountMode.Count, allIntervals.Count);

         if (!allValidValues.Any())
            return data;


         foreach (var interval in allIntervals)
         {
            var currentInterval = interval;
            double count = allValidValues.Count(item => currentInterval.Contains(item));
            data.AddData(currentInterval.MeanValue, count, "Residuals");
         }

         data.XMinData = allValidValues.Min();
         data.XMaxData = allValidValues.Max();

         return data;
      }
   }
}