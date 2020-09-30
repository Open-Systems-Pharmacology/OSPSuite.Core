using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Core
{
   public class InvalidFileException : Exception
   {
      private const string MESSAGE = "An error occurred while reading the file. Please check the content";

      public InvalidFileException() : base(MESSAGE)
      {
      }
   }
}
