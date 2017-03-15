using System.Data;

namespace OSPSuite.Presentation.Mappers
{
   public interface IDataTableToStringMapper
   {
      string MapFrom(DataTable dataTable, bool includeHeaders);
   }
}