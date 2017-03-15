using OSPSuite.Assets;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class RunResultDTO
   {
      public virtual RunStatus Status { get; set; }
      public ApplicationIcon StatusIcon => Status.Icon;
   }
}