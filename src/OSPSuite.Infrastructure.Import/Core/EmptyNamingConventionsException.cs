using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class EmptyNamingConventionsException : AbstractImporterException
   {
      public EmptyNamingConventionsException() : base(Error.NamingConventionEmpty)
      {
      }
   }
}
