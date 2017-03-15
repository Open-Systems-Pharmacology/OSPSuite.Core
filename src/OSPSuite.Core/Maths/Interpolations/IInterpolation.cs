using System.Collections.Generic;

namespace OSPSuite.Core.Maths.Interpolations
{
    public interface IInterpolation
    {
        double Interpolate(IEnumerable<Sample> knownSamples, double valueToInterpolate);
    }
}