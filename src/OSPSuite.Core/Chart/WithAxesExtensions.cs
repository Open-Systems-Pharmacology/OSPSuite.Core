using System.Linq;
using OSPSuite.Utility;

namespace OSPSuite.Core.Chart
{
   public static class WithAxesExtensions
   {
      public static Axis AddNewAxis(this IWithAxes withAxes)
      {
         return (from axisType in EnumHelper.AllValuesFor<AxisTypes>()
            where !withAxes.HasAxis(axisType)
            select withAxes.AddNewAxisFor(axisType)).FirstOrDefault();
      }
   }
}