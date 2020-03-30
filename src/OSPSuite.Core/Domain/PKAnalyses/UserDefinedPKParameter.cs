using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.PKAnalyses
{
   public enum StandardPKParameter
   {
      Unknown,
      C_max,
      C_max_norm,
      C_min,
      C_min_norm,
      t_max,
      t_min,
      C_trough,
      C_trough_norm,
      AUC_tEnd,
      AUC_tEnd_norm,
      AUCM_tEnd,
      AUC_inf,
      AUC_inf_norm,
      AUC_tEnd_inf,
      AUC_tEnd_inf_norm,
      MRT,
      FractionAucEndToInf,
      Thalf,
      Vss,
      Vd,
      Tthreshold,
   }

   public class UserDefinedPKParameter : PKParameter
   {
      //User defined PK Parameters should be created via the DimensionTask method to ensure that dimensions are set as expected.
      internal UserDefinedPKParameter()
      {
         // Default dimension for a dynamic PK Parameter for now 
         Dimension = Constants.Dimension.NO_DIMENSION;

         // Set the mode to always by default for now. User will be responsible to define the pk parameters in a meaningful way
         Mode = PKParameterMode.Always;
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

      public double? EstimateDrugMassPerBodyWeight(PKCalculationOptions pkCalculationOptions)
      {
         // It can be quite complicated to estimate the total drug mass between two intervals
         // If a start time and end time is provided or any time offset, we cannot calculate for sure and we'll return NULL
         // if a start application and an end infusion index is provided, we'll sum up the interval between those two application
         // if a start application is provided, we'll sum up the drug mass starting at this application until the end
         // if an end application is provided, we'll sum up the drug mass from the start until the application

         if (StartTime.HasValue || EndTime.HasValue || EndTimeOffset.HasValue || StartTimeOffset.HasValue)
            return null;

         //no intervals defined
         if (!StartApplicationIndex.HasValue && !EndApplicationIndex.HasValue)
            return pkCalculationOptions.TotalDrugMassPerBodyWeight;

         var startApplicationIndex = StartApplicationIndex.GetValueOrDefault(0);
         var endApplicationIndex = EndApplicationIndex.GetValueOrDefault(pkCalculationOptions.DosingIntervals.Count);


         // invalid intervals
         if (startApplicationIndex >= endApplicationIndex)
            return null;

         double drugMassPerBodyWeight = 0;
         for (int applicationIndex = startApplicationIndex; applicationIndex < endApplicationIndex; applicationIndex++)
         {
            //wrong indexes
            var interval = pkCalculationOptions.DosingIntervalAt(applicationIndex);
            if (interval == null)
               return null;

            drugMassPerBodyWeight += interval.DrugMassPerBodyWeight.GetValueOrDefault(0);
         }


         return drugMassPerBodyWeight;
      }

      private float? toNullableFloat(double? doubleValue) => (float?) doubleValue;
   }
}