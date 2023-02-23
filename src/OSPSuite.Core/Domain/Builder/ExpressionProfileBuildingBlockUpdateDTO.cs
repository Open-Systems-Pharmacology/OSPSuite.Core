using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionProfileBuildingBlockUpdateDTO
   {
      public ExpressionProfileBuildingBlockUpdateDTO(ExpressionProfileBuildingBlock buildingBlock)
      {
         ExpressionParameters = buildingBlock.Select(x => new ExpressionParameterUpdateDTO(x)).ToList();
         Species = buildingBlock.Species;
         MoleculeName = buildingBlock.MoleculeName;
      }

      public IReadOnlyList<ExpressionParameterUpdateDTO> ExpressionParameters { get; }
      public string Species { get; }
      public string MoleculeName { get; }
   }
}