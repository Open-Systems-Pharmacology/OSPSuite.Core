using System.Drawing;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Services
{
   public interface IOptimizedParameterRangeImageCreator
   {
      Image CreateFor(OptimizedParameterDTO optimizedParameterDTO);
      Image CreateLegendFor(OptimizedParameterDTO optimizedParameterDTO);
   }
}