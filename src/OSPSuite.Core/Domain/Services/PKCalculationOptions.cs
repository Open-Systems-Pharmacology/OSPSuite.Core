using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.Core.Domain.Services
{
   public class PKCalculationOptions
   {
      /// <summary>
      ///    Total dose (in µmol/kg BW)
      /// </summary>
      public double? Dose { get; set; }

      /// <summary>
      ///    Infusion time (in time unit)
      /// </summary>
      public double? InfusionTime { get; set; }

      /// <summary>
      ///    Time value representing the beginning of first dosing interval in time array
      /// </summary>
      public float? FirstDosingStartValue { get; set; }

      /// <summary>
      ///    Time value representing the end of first dosing interval in time array
      /// </summary>
      public float? FirstDosingEndValue { get; set; }

      /// <summary>
      /// Dose (in µmol/kg BW) of the first dosing interval
      /// </summary>
      public double? FirstDose { get; set; }
     
      /// <summary>
      ///    Time value representing the beginning of the one before last dosing interval in time array
      /// </summary>
      public float? LastMinusOneDosingStartValue { get; set; }

      /// <summary>
      /// Dose (in µmol/kg BW) of the one before last dosing interval
      /// </summary>
      public double? LastMinusOneDose { get; set; }

      /// <summary>
      ///    Time value representing the beginning of last dosing interval in time array
      /// </summary>
      public float? LastDosingStartValue { get; set; }

      /// <summary>
      ///    Time value representing the end of last dosing interval in time array
      /// </summary>
      public float? LastDosingEndValue { get; set; }

      /// <summary>
      /// Dose (in µmol/kg BW) of the last dosing interval
      /// </summary>
      public double? LastDose{ get; set; }

      public bool SingleDosing
      {
         get
         {
            if (!dosingIntervalsWellDefined)
               return true;

            if (FirstDosingStartValue > FirstDosingEndValue)
               return true;

            if (FirstDosingEndValue > LastDosingStartValue)
               return true;

            if (LastMinusOneDosingStartValue > LastDosingStartValue)
               return true;

            if (LastDosingStartValue > LastDosingEndValue)
               return true;

            return false;
         }
      }

      /// <summary>
      /// Returns <c>PKParameterMode.Single</c> if the options represents a single dosing application and <c>PKParameterMode.Multi</c> 
      /// otherwise
      /// </summary>
      public PKParameterMode PKParameterMode => SingleDosing ? PKParameterMode.Single : PKParameterMode.Multi;

      private bool dosingIntervalsWellDefined => FirstDosingStartValue != null && FirstDosingEndValue != null &&
                                                 LastDosingStartValue != null && LastDosingEndValue != null && LastMinusOneDosingStartValue != null;
   }
}