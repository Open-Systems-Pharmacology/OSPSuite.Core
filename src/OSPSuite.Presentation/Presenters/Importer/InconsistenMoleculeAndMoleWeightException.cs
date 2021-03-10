using OSPSuite.Assets;
using System;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class InconsistenMoleculeAndMoleWeightException : Exception
   {
      public InconsistenMoleculeAndMoleWeightException() : base(Error.InconsistenMoleculeAndMoleWeightException) { }
   }
}
