using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class CategorialParameterIdentificationRunFactory : IParameterIdentificationRunSpecificationFactory
   {
      private readonly IParameterIdentificationRunInitializerFactory _runInitializerFactory;
      private readonly ICategorialParameterIdentificationToCalculationMethodPermutationMapper _categorialToCalculationMethodPermutationMapper;

      public CategorialParameterIdentificationRunFactory(IParameterIdentificationRunInitializerFactory runInitializerFactory, ICategorialParameterIdentificationToCalculationMethodPermutationMapper categorialToCalculationMethodPermutationMapper)
      {
         _runInitializerFactory = runInitializerFactory;
         _categorialToCalculationMethodPermutationMapper = categorialToCalculationMethodPermutationMapper;
      }

      public IReadOnlyList<IParameterIdentificationRun> CreateFor(ParameterIdentification parameterIdentification, CancellationToken cancellationToken)
      {
         var allParameterIdentificationRuns = new List<IParameterIdentificationRun>();

         var uniqueCombinations = _categorialToCalculationMethodPermutationMapper.MapFrom(parameterIdentification).ToList();
         var isSingleCategory = hasSingleCategory(uniqueCombinations);
         uniqueCombinations.Each((combination, index) =>
         {
            cancellationToken.ThrowIfCancellationRequested();
            var runInitializer = _runInitializerFactory.Create<CategorialParameterIdentificationRunInitializer>();
            runInitializer.Initialize(parameterIdentification, index, combination, isSingleCategory);
            allParameterIdentificationRuns.Add(runInitializer.Run);
         });

         return allParameterIdentificationRuns;
      }

      private bool hasSingleCategory(IEnumerable<CalculationMethodCombination> allCombinations)
      {
         return allCombinations.All(combination => combination.HasSingleCategory);
      }

      public bool IsSatisfiedBy(ParameterIdentificationRunMode parameterIdentificationRunMode)
      {
         return parameterIdentificationRunMode.IsCategorial();
      }
   }
}