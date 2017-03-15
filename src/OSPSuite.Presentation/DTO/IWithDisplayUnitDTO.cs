using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.DTO
{
   public interface IWithDisplayUnitDTO : IWithDisplayUnit
   {
      IEnumerable<Unit> AllUnits { get; set; }
   }
}