using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionProfileBuildingBlockUpdate
   {
      public ExpressionProfileBuildingBlockUpdate(ExpressionProfileBuildingBlock buildingBlock)
      {
         ExpressionParameters = buildingBlock.Select(x => new ExpressionParameterUpdate(x)).ToList();
         Species = buildingBlock.Species;
         MoleculeName = buildingBlock.MoleculeName;
      }

      public IReadOnlyList<ExpressionParameterUpdate> ExpressionParameters { get; }
      public string Species { get; }
      public string MoleculeName { get; }
   }
}