using System.Linq;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.PKAnalyses
{
   public enum StandardPKParameter
   {
      Unknown,
      Cmax,
      Cmin,
      Tmax,
      Tmin,
      CTrough,
      Auc,
      Aucm,
      AucInf,
      AucTendInf,
      Mrt,
      FractionAucEndToInf,
      Thalf,
      Vss,
      Vd,
      Tthreshold
   }

   public class DynamicPKParameter : PKParameter
   {
      // Parameter such as AUC, Cmax, Tmax that will be used to calculate the parameter
      public StandardPKParameter StandardPKParameter { get; set; } = StandardPKParameter.Unknown;

      public float? StartTime { get; set; }
      public float? EndTime { get; set; }

      /// <summary>
      ///    1-based index of the application that should be used to estimate the start of the interval
      /// </summary>
      public int? StartApplicationIndex { get; set; }

      /// <summary>
      ///    1-based index of the application that should be used to estimate the end of the interval
      /// </summary>
      public int? EndApplicationIndex { get; set; }

      /// <summary>
      ///    Time offset in min to apply to the start time (either from the absolute <see cref="StartTime" /> or the start time
      ///    estimated from the application <see cref="StartApplicationIndex" />
      /// </summary>
      public float? StartTimeOffset { get; set; }

      /// <summary>
      ///    Time offset in min to apply to the start time (either from the absolute <see cref="EndTime" /> or the start time
      ///    estimated from the application <see cref="EndApplicationIndex" />
      /// </summary>
      public float? EndTimeOffset { get; set; }
      
      /// <summary>
      /// Normalization Factory parameter that will be used to calculate a normalized parameter. Not that if specified, only the normalized parameter will be added
      /// </summary>
      public double? NormalizationFactor { get; set; }

      /// <summary>
      /// If defined, the time at which this concentration was reached will be calculated
      /// </summary>
      public float? ConcentrationThreshold { get; set; }

      public float? EstimateStartTimeFrom(PKCalculationOptions pkCalculationOptions)
      {
         if (StartTime != null)
            return StartTime + StartTimeOffset.GetValueOrDefault(0);

         var oneBaseApplicationIndex = StartApplicationIndex.GetValueOrDefault(0);
         if (oneBaseApplicationIndex <= 0)
            return null;

         var dosingInterval = pkCalculationOptions.DosingIntervals.ElementAt(oneBaseApplicationIndex - 1);
         if (dosingInterval?.StartValue == null)
            return null;

         return dosingInterval.StartValue + StartTimeOffset.GetValueOrDefault(0);
      }

      public float? EstimateEndTimeFrom(PKCalculationOptions pkCalculationOptions)
      {
         if (EndTime != null)
            return EndTime + EndTimeOffset.GetValueOrDefault(0);

         var oneBaseApplicationIndex = EndApplicationIndex.GetValueOrDefault(0);
         if (oneBaseApplicationIndex <= 0)
            return null;

         var dosingInterval = pkCalculationOptions.DosingIntervals.ElementAt(oneBaseApplicationIndex - 1);
         if (dosingInterval?.StartValue == null)
            return null;

         //We use the start value here as we are not interested in the time after the application
         return dosingInterval.StartValue + EndTimeOffset.GetValueOrDefault(0);
      }
   }
}