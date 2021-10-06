using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class NanException : AbstractImporterException
   {
      public NanException() : base(Error.NaNOnData)
      {
      }
   }
}
