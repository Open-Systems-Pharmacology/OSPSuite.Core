using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class InconsistentMoleculeAndMolWeightException : AbstractImporterException
   {
      public InconsistentMoleculeAndMolWeightException() : base(Error.InconsistentMoleculeAndMolWeightException)
      {
      }
   }
}