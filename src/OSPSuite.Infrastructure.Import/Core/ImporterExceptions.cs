using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class EmptyNamingConventionsException : OSPSuiteException
   {
      public EmptyNamingConventionsException() : base(Error.NamingConventionEmpty)
      {
      }
   }

   public class NullNamingConventionsException : OSPSuiteException
   {
      public NullNamingConventionsException() : base(Error.NamingConventionNull)
      {
      }
   }

   public class InconsistentMoleculeAndMoleWeightException : OSPSuiteException
   {
      public InconsistentMoleculeAndMoleWeightException() : base(Error.InconsistentMoleculeAndMoleWeightException)
      {
      }
   }
}