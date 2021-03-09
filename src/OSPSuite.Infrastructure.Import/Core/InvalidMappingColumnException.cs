using OSPSuite.Assets;
using System;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InvalidMappingColumnException : Exception
   {
      InvalidMappingColumnException() : base(Error.InvalidMappingColumn)
      {

      }
   }
}
