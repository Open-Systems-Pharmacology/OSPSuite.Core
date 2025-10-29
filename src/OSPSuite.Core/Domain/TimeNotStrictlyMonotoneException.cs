using OSPSuite.Assets;

namespace OSPSuite.Core.Domain
{
   public class TimeNotStrictlyMonotoneException : InvalidArgumentException
   {
      private readonly double _beforeValue;
      private readonly double _afterValue;
      private readonly string _displayUnit;

      public TimeNotStrictlyMonotoneException(double beforeValue, double afterValue, string displayUnit) : base(Error.TimeNotStrictlyMonotone(beforeValue, afterValue, displayUnit))
      {
         _beforeValue = beforeValue;
         _afterValue = afterValue;
         _displayUnit = displayUnit;
      }

      public TimeNotStrictlyMonotoneException(TimeNotStrictlyMonotoneException ex, string repositoryName) : base(Error.TimeNotStrictlyMonotone(ex._beforeValue, ex._afterValue, ex._displayUnit, repositoryName))
      {
         _beforeValue = ex._beforeValue;
         _afterValue = ex._afterValue;
         _displayUnit = ex._displayUnit;
      }
   }
}