using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.DTO
{
   public class DefaultUnitMapDTO : DxValidatableDTO<DisplayUnitMap>
   {
      public DisplayUnitMap Subject { get; private set; }

      public DefaultUnitMapDTO(DisplayUnitMap displayUnit)
         : base(displayUnit)
      {
         Subject = displayUnit;
      }

      public IDimension Dimension
      {
         get { return Subject.Dimension; }
         set { Subject.Dimension = value; }
      }

      public Unit DisplayUnit
      {
         get { return Subject.DisplayUnit; }
         set { Subject.DisplayUnit = value; }
      }
   }
}