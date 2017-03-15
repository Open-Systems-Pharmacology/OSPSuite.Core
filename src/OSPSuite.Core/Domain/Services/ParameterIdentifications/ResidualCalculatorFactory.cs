using System;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IResidualCalculatorFactory
   {
      IResidualCalculator CreateFor(ParameterIdentificationConfiguration parameterIdentificationConfiguration);
   }

   public class ResidualCalculatorFactory : IResidualCalculatorFactory
   {
      private readonly OSPSuite.Utility.Container.IContainer _container;

      public ResidualCalculatorFactory(OSPSuite.Utility.Container.IContainer container)
      {
         _container = container;
      }

      public IResidualCalculator CreateFor(ParameterIdentificationConfiguration parameterIdentificationConfiguration)
      {
         var residualCalculator = createCalculator(parameterIdentificationConfiguration.LLOQMode);
         residualCalculator.Initialize(parameterIdentificationConfiguration.RemoveLLOQMode);
         return residualCalculator;
      }

      private IResidualCalculator createCalculator(LLOQMode lloqMode)
      {
         // in case of log scale can arise problems, therefore different LLOQMode methods are available
         if (lloqMode == LLOQModes.OnlyObservedData)
            return _container.Resolve<ResidualCalculatorForOnlyObservedData>();

         if (lloqMode == LLOQModes.SimulationOutputAsObservedDataLLOQ)
            return _container.Resolve<ResidualCalculatorForSimulationOutputAsObservedDataLLOQ>();

         throw new ArgumentOutOfRangeException(nameof(lloqMode));
      }
   }
}