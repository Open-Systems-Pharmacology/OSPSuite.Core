using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public class BinInterval
   {
      public double Min { get; }
      public double Max { get; }

      private readonly bool _maxAllowed;
      private readonly bool _minAllowed;

      public BinInterval(double min, double max, bool minAllowed =true, bool maxAllowed = false)
      {
         Min = min;
         Max = max;
         _maxAllowed = maxAllowed;
         _minAllowed = minAllowed;
      }

      public bool Contains(double value)
      {
         if (value < Min && _minAllowed)
            return false;

         if (value <= Min && !_minAllowed)
            return false;

         //constant can be equal to max
         if (IsConstant || _maxAllowed)
            return value <= Max;

         return value < Max;
      }

      public double MeanValue
      {
         get
         {
            if (IsConstant)
               return Min;

            return (Min + Max) / 2;
         }
      }

      public bool IsConstant => Min.EqualsByTolerance(Max, Min * 0.01);
   }
}