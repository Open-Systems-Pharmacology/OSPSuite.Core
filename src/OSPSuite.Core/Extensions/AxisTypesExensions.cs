using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Extensions
{
   public static class AxisTypesExensions
   {
      public static bool IsXAxis(this AxisTypes axisType)
      {
         return axisType == AxisTypes.X;
      }

      public static bool IsYAxis(this AxisTypes axisType)
      {
         return !axisType.IsXAxis();
      }
   }
}