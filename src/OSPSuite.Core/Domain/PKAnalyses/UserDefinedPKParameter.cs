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
      AucTend,
      AucmTend,
      AucInf,
      AucTendInf,
      Mrt,
      FractionAucEndToInf,
      Thalf,
      Vss,
      Vd,
      Tthreshold,
   }

   public class UserDefinedPKParameter : PKParameter
   {
      public UserDefinedPKParameter()
      {
         // Default dimension for a dynamic PK Parameter for now 
         Dimension = Constants.Dimension.NO_DIMENSION;
      }

      // Parameter such as AUC, Cmax, Tmax that will be used to calculate the parameter
      public StandardPKParameter StandardPKParameter { get; set; } = StandardPKParameter.Unknown;

      public double? StartTime { get; set; }
      public double? EndTime { get; set; }

      /// <summary>
      ///    0-based index of the application that should be used to estimate the start of the interval
      /// </summary>
      public int? StartApplicationIndex { get; set; }

      /// <summary>
      ///    0-based index of the application that should be used to estimate the end of the interval
      /// </summary>
      public int? EndApplicationIndex { get; set; }

      /// <summary>
      ///    Time offset in min to apply to the start time (either from the absolute <see cref="StartTime" /> or the start time
      ///    estimated from the application <see cref="StartApplicationIndex" />
      /// </summary>
      public double? StartTimeOffset { get; set; }

      /// <summary>
      ///    Time offset in min to apply to the start time (either from the absolute <see cref="EndTime" /> or the start time
      ///    estimated from the application <see cref="EndApplicationIndex" />
      /// </summary>
      public double? EndTimeOffset { get; set; }
      
      /// <summary>
      /// Normalization Factory parameter that will be used to calculate a normalized parameter. Not that if specified, only the normalized parameter will be added
      /// </summary>
      public double? NormalizationFactor { get; set; }

      /// <summary>
      /// If defined, the time at which this concentration was reached will be calculated
      /// </summary>
      public double? ConcentrationThreshold { get; set; }

      public float? EstimateStartTimeFrom(PKCalculationOptions pkCalculationOptions)
      {
         double? estimatedStartTime()
         {
            if (StartTime != null)
               return StartTime + StartTimeOffset.GetValueOrDefault(0);

            var applicationIndex = StartApplicationIndex.GetValueOrDefault(-1);
            if (applicationIndex < 0)
               return null;

            var dosingInterval = pkCalculationOptions.DosingIntervalAt(applicationIndex);
            if (dosingInterval?.StartValue == null)
               return null;

            return dosingInterval.StartValue + StartTimeOffset.GetValueOrDefault(0);
         }

         return toNullableFloat(estimatedStartTime());
      }

      public float? EstimateEndTimeFrom(PKCalculationOptions pkCalculationOptions)
      {
         double? estimatedEndTime()
         {
            if (EndTime != null)
               return EndTime + EndTimeOffset.GetValueOrDefault(0);

            var applicationIndex = EndApplicationIndex.GetValueOrDefault(-1);
            if (applicationIndex < 0)
               return null;

            var dosingInterval = pkCalculationOptions.DosingIntervalAt(applicationIndex);
            if (dosingInterval?.StartValue == null)
               return null;

            //We use the start value here as we are not interested in the time after the application
            return dosingInterval.StartValue + EndTimeOffset.GetValueOrDefault(0);
         }

         return toNullableFloat(estimatedEndTime());
      }

      private float? toNullableFloat(double? estimatedEndTime) => (float?) estimatedEndTime;
   }
}