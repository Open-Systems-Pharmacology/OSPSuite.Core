namespace OSPSuite.Core.Domain.Services
{
   public class DosingInterval
   {
      /// <summary>
      ///    Time value representing the beginning of the interval in [min]
      /// </summary>
      public float? StartValue { get; set; }

      /// <summary>
      ///    Time value representing the end of the dosing interval in [min]
      /// </summary>
      public float? EndValue { get; set; }

      /// <summary>
      ///    Drug mass per body weight (in µmol/kg BW) of the interval
      /// </summary>
      public double? DrugMassPerBodyWeight { get; set; }

      public bool IsValid => StartValue != null && EndValue != null && StartValue < EndValue;
   }
}