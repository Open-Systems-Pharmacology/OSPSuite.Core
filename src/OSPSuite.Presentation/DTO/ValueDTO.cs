using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.DTO
{
   public class ValueDTO : IWithDisplayUnit
   {
      public IDimension Dimension { get; set; }

      /// <summary>
      ///    Value in <see cref="DisplayUnit" />
      /// </summary>
      public double DisplayValue => Dimension.BaseUnitValueToUnitValue(DisplayUnit, Value);

      public Unit DisplayUnit { get; set; }

      /// <summary>
      /// Value in base unit
      /// </summary>
      public double Value { get; set; }
   }
}