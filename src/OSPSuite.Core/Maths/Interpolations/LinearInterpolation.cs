using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Maths.Interpolations
{
   public class LinearInterpolation : IInterpolation
   {
      public T Interpolate<T>(IEnumerable<Sample<T>> knownSamples, double valueToInterpolate)
      {
         var (minRange, maxRange) = interpolationRangeFrom(knownSamples, valueToInterpolate);
         if (minRange == null)
            return default(T);

         if (maxRange == null)
            return minRange.Y;

         var distanceToMin = Math.Abs(minRange.X - valueToInterpolate);
         var distanceToMax = Math.Abs(maxRange.X - valueToInterpolate);

         return distanceToMin > distanceToMax ? maxRange.Y : minRange.Y;
      }

      public double Interpolate(IEnumerable<Sample<double>> knownSamples, double valueToInterpolate)
      {
         var (minRange, maxRange) = interpolationRangeFrom(knownSamples, valueToInterpolate);
         if (minRange == null)
            return 0.0;

         if (maxRange == null)
            return minRange.Y;

         double m = (maxRange.Y - minRange.Y) / (maxRange.X - minRange.X);
         double p = minRange.Y - m * minRange.X;

         return m * valueToInterpolate + p;
      }
      
      private (Sample<T> minSample, Sample<T> maxSample) interpolationRangeFrom<T>(IEnumerable<Sample<T>> samples, double valueToInterpolate)
      {
         var orderedSamples = samples.OrderBy(sample => sample.X).ToList();

         //trivial cases
         if (orderedSamples.Count == 0)
            return (null, null);

         if (orderedSamples.Count == 1)
            return (orderedSamples[0], null);

         // At least 2 elements; find greatest element smaller than the value to interpolate
         int iMin = orderedSamples.FindLastIndex(sample => sample.X <= valueToInterpolate);

         //value smaller than any item in the sample list. return min
         if (iMin == -1)
            return (orderedSamples[0], null);

         //the last element is the greatest element smalller than our value, return this one

         var smallerSample = orderedSamples[iMin];
         if (iMin == orderedSamples.Count - 1)
            return (smallerSample, null);

         return (smallerSample, orderedSamples[iMin + 1]);
      }
   }
}