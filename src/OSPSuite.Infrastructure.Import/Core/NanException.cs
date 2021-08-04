using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class NanException : AbstractImporterException
   {
      public NanException() : base(Error.NaNOnData)
      {
      }
   }
}
