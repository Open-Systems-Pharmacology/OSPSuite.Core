using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Maths.Interpolations
{
    public class LinearInterpolation : IInterpolation
    {
        public double Interpolate(IEnumerable<Sample> knownSamples, double valueToInterpolate)
        {
            //at least 2 elements
            var orderedSamples = knownSamples.OrderBy(sample => sample.X).ToList();
            
            //trivial cases
            if (orderedSamples.Count == 0) return 0;
            if (orderedSamples.Count == 1) return orderedSamples[0].Y;
          
            //find greatest element smaller than the value to interpolate
            int iMin  = orderedSamples.FindLastIndex(sample => sample.X <= valueToInterpolate);

            //value smaller than any item in the sample list. return min
            if (iMin == -1) return orderedSamples[0].Y;
            if (iMin == orderedSamples.Count - 1) return orderedSamples[orderedSamples.Count - 1].Y;

            double m = (orderedSamples[iMin + 1].Y - orderedSamples[iMin].Y) / (orderedSamples[iMin + 1].X - orderedSamples[iMin].X);
            double p = orderedSamples[iMin].Y - m * orderedSamples[iMin].X;

            return m*valueToInterpolate + p;
        }
    }
}