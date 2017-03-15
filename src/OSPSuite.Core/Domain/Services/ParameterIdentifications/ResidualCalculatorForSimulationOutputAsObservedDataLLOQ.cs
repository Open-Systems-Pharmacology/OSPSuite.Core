using System;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class ResidualCalculatorForSimulationOutputAsObservedDataLLOQ : ResidualCalculator
   {
      public ResidualCalculatorForSimulationOutputAsObservedDataLLOQ(ITimeGridRestrictor timeGridRestrictor, IDimensionFactory dimensionFactory) : base(timeGridRestrictor,dimensionFactory)
      {
      }

      protected override double LogResidual(float simulatedValue, float observedValue, float lloq)
      {
         // same LLOQ-Handling for measured and simulated values, 
         // e) lack for sensitivity for simulation values beloq LLOQ, meets reality, can perhaps cause algorithmic problems

         if (simulatedValue < lloq)
            simulatedValue = lloq;

         if (observedValue < lloq)
            observedValue = lloq;

         return LogSafe(simulatedValue) - LogSafe(observedValue);
      }

      protected override double LinResidual(float simulatedValue, float observedValue, float lloq)
      {
         if (simulatedValue < lloq)
            simulatedValue = lloq;

         if (observedValue < lloq)
            observedValue = lloq;

         return Convert.ToDouble(simulatedValue) - Convert.ToDouble(observedValue);
      }
   }
}