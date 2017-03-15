using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationRunFactory
   {
      IReadOnlyList<IParameterIdentificationRun> CreateFor(ParameterIdentification parameterIdentification, CancellationToken cancellationToken);
   }

   public interface IParameterIdentificationRunSpecificationFactory : IParameterIdentificationRunFactory, ISpecification<ParameterIdentificationRunMode>
   {
   }

   public class ParameterIdentificationRunFactory : IParameterIdentificationRunFactory
   {
      private readonly List<IParameterIdentificationRunSpecificationFactory> _allCreators;

      public ParameterIdentificationRunFactory(IRepository<IParameterIdentificationRunSpecificationFactory> creatorRepository)
      {
         _allCreators = creatorRepository.All().ToList();
      }

      public IReadOnlyList<IParameterIdentificationRun> CreateFor(ParameterIdentification parameterIdentification, CancellationToken cancellationToken)
      {
         foreach (var creator in _allCreators)
         {
            if (creator.IsSatisfiedBy(parameterIdentification.Configuration.RunMode))
               return creator.CreateFor(parameterIdentification, cancellationToken);
         }

         throw new ArgumentOutOfRangeException(parameterIdentification.Configuration.RunMode.GetType().Name);
      }
   }
}