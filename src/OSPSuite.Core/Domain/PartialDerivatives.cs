using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public class PartialDerivatives
   {
      public string FullOutputPath { get; }
      public List<string> ParameterNames { get; }
      private readonly List<double[]> _partialDerivatives = new List<double[]>();

      [Obsolete("For serialization")]
      public PartialDerivatives()
      {
      }

      public PartialDerivatives(string fullOutputPath, IEnumerable<string> parameterNames)
      {
         FullOutputPath = fullOutputPath;
         ParameterNames = parameterNames.ToList();
      }

      public void AddPartialDerivative(double[] derivatives)
      {
         if (derivatives.Length != ParameterNames.Count)
            throw new InvalidArgumentException($"No the expected number of values ({derivatives.Length} vs {ParameterNames.Count})");

         _partialDerivatives.Add(derivatives);
      }

      public double[] PartialDerivativeAt(int index0)
      {
         if (_partialDerivatives.Count <= index0)
            return new double[ParameterNames.Count].InitializeWith(0);

         return _partialDerivatives[index0];
      }

      public IReadOnlyList<double[]> AllPartialDerivatives => _partialDerivatives;
   }
}