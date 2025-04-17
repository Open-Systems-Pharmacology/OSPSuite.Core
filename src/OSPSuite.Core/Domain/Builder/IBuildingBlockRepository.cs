using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IBuildingBlockRepository
   {
      IReadOnlyList<IBuildingBlock> All();
      IReadOnlyList<T> All<T>() where T : IBuildingBlock;
   }
}