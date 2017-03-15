using System.Collections.Generic;
using System.Threading;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class StandardParameterIdentificationRunFactory : IParameterIdentificationRunSpecificationFactory
   {
      private readonly IParameterIdentificationRunInitializerFactory _runInitializerFactory;

      public StandardParameterIdentificationRunFactory(IParameterIdentificationRunInitializerFactory runInitializerFactory)
      {
         _runInitializerFactory = runInitializerFactory;
      }

      public IReadOnlyList<IParameterIdentificationRun> CreateFor(ParameterIdentification parameterIdentification, CancellationToken cancellationToken)
      {
         var runInitializer = _runInitializerFactory.Create<StandardParameterIdentificationRunInitializer>();
         runInitializer.Initialize(parameterIdentification, runIndex: 0);
         return new[] { runInitializer.Run};
      }

      public bool IsSatisfiedBy(ParameterIdentificationRunMode parameterIdentificationRunMode)
      {
         return parameterIdentificationRunMode.IsAnImplementationOf<StandardParameterIdentificationRunMode>();
      }
   }
}