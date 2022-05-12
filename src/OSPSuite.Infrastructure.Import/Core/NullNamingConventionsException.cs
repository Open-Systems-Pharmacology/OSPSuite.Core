using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class NullNamingConventionsException : AbstractImporterException
   {
      public NullNamingConventionsException() : base(Error.NamingConventionNull)
      {
      }
   }
}
