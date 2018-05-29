using System.Collections.Generic;

namespace OSPSuite.Core.Maths.Interpolations
{
    public interface IInterpolation
    {
        T Interpolate<T>(IEnumerable<Sample<T>> knownSamples, double valueToInterpolate);
        double Interpolate(IEnumerable<Sample<double>> knownSamples, double valueToInterpolate);
    }
}