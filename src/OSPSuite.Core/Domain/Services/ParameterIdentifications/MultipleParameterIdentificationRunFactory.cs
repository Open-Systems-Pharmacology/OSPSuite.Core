using System;
using System.Collections.Generic;
using System.Threading;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Maths.Random;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class MultipleParameterIdentificationRunFactory : IParameterIdentificationRunSpecificationFactory
   {
      private readonly IParameterIdentificationRunInitializerFactory _runInitializerFactory;
      private readonly RandomGenerator _randomGenerator;

      public MultipleParameterIdentificationRunFactory(IParameterIdentificationRunInitializerFactory runInitializerFactory)
      {
         _runInitializerFactory = runInitializerFactory;
         _randomGenerator = new RandomGenerator(Environment.TickCount);
      }

      public IReadOnlyList<IParameterIdentificationRun> CreateFor(ParameterIdentification parameterIdentification, CancellationToken cancellationToken)
      {
         var multipleOptimizationOption = parameterIdentification.Configuration.RunMode.DowncastTo<MultipleParameterIdentificationRunMode>();
         var allParameterIdentificationRuns = new List<IParameterIdentificationRun>();

         for (int i = 0; i < multipleOptimizationOption.NumberOfRuns; i++)
         {
            cancellationToken.ThrowIfCancellationRequested();
            var runInitializer = _runInitializerFactory.Create<MultipleParameterIdentificationRunInitializer>();
            runInitializer.Initialize(parameterIdentification, i, _randomGenerator);
            allParameterIdentificationRuns.Add(runInitializer.Run);
         }

         return allParameterIdentificationRuns;
      }

      public bool IsSatisfiedBy(ParameterIdentificationRunMode parameterIdentificationRunMode)
      {
         return parameterIdentificationRunMode.IsRandomizedStartValue();
      }
   }
}