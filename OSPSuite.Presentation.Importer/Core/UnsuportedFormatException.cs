using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Core
{
   public class UnsuportedFormatException : Exception
   {
      private static string _message = "The file format is not supported";

      public UnsuportedFormatException() : base(_message)
      {
      }
   }
}
