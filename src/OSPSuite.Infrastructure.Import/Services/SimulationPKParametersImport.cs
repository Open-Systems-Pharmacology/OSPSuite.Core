using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Importer.Services
{
   public class SimulationPKParametersImport : ImportLogger
   {
      public virtual IEnumerable<QuantityPKParameter> PKParameters { get; set; }
   }
}