using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class EmptyNamingConventionsException : OSPSuiteException
   {
      public EmptyNamingConventionsException() : base("Column naming conventions cannot be empty.")
      {
      }
   }

   public class NullNamingConventionsException : OSPSuiteException
   {
      public NullNamingConventionsException() : base("Column naming conventions cannot be null.")
      {
      }
   }
}