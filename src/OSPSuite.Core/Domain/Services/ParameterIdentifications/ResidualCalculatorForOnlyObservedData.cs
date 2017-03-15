using System;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class ResidualCalculatorForOnlyObservedData : ResidualCalculator
   {
      public ResidualCalculatorForOnlyObservedData(ITimeGridRestrictor timeGridRestrictor, IDimensionFactory dimensionFactory) : base(timeGridRestrictor, dimensionFactory)
      {
      }
         
      protected override double LogResidual(float simulatedValue, float observedValue, float lloq)
      {
         //simple, but 
         // a) different LLOQ-Handling for measured and simulated values, 
         // b) discontinuity in LLOQ for observed values, 
         // c) simulation values are forced to LLOQ/2 for Observed data LLOQ values
         // d) for simulation values near 0 the corresponding log values can become very big and dominate other error information
         return LogSafe(simulatedValue) - LogSafe(observedValue);
      }

      protected override double LinResidual(float simulatedValue, float observedValue, float lloq)
      {
         return Convert.ToDouble(simulatedValue) - Convert.ToDouble(observedValue);
      }
   }
}