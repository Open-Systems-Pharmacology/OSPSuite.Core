using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;

namespace OSPSuite.Starter.Services
{
   public class CompoundCalculationMethodRepository : ICompoundCalculationMethodRepository
   {
      public IEnumerable<CalculationMethod> All()
      {
         return Enumerable.Empty<CalculationMethod>();
      }
   }
}