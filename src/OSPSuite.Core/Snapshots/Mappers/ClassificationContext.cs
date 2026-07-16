using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ClassificationContext
   {
      public IReadOnlyCollection<Domain.Classification> Classifications { get; set; }
      public IReadOnlyCollection<IClassifiableWrapper> Classifiables { get; set; }
   }
}