using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class NanException : AbstractImporterExceptions
   {
      public NanException() : base(Error.NaNOnData)
      {
      }
   }
}
