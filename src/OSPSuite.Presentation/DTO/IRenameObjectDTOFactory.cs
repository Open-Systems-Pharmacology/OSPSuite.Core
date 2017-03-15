using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public interface IRenameObjectDTOFactory
   {
      RenameObjectDTO CreateFor(IWithName objectBase);
   }
}