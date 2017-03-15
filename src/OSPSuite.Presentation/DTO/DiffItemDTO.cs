using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class DiffItemDTO : PathRepresentableDTO
   {
      public string ObjectName { get; set; }
      public string Property { get; set; }
      public string Value1 { get; set; }
      public string Value2 { get; set; }
      public string Description { get; set; }
      public bool ItemIsMissing { get; set; }

      public string PathForExport
      {
         get
         {
            var path = PathAsString(ObjectPath.PATH_DELIMITER);
            return string.IsNullOrEmpty(path) ? ObjectName : path;
         }
      }
   }
}